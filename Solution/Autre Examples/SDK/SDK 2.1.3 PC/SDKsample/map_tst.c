#include "sdk_tst.h"
#include "profiles_tst.h"
#include "Btsdk_Stru.h"
#include "map_tst.h"
#include "bmsg_parser.h"
#include "hlp_parser.h"
#include "ShlObj.h"

static BTCONNHDL s_currMAPConnHdl = BTSDK_INVALID_HANDLE;
static BTDEVHDL s_currRmtMAPDevHdl = BTSDK_INVALID_HANDLE;
static BTSVCHDL s_currMAPSvcHdl = BTSDK_INVALID_HANDLE;
static BTUINT8 s_currstrPath[BTSDK_PATH_MAXLENGTH] = {0};
static BTUINT8 s_currstrFile[BTSDK_PATH_MAXLENGTH] = {0};
static int s_totalMsg = 0;

#define MAX_LOW_IDX		0x00030000
/* Used for message handle assign */
static BTUINT8 g_root_mse[MAX_PATH] = {0};
static BTUINT32 s_high_idx = 0x10000001;
static BTUINT32 s_low_dix = 0x00000000;
static BTBOOL s_mse_notif_on = BTSDK_TRUE;
static BTCONNHDL s_mns_conn_hdl = BTSDK_INVALID_HANDLE;

const BTUINT8 g_MsgRoot[] = "root\\";
static BTUINT8 s_msg_num = 0;

#define MAX_FOLDER_NUM 20
#define MAX_MSG_NUM	   40
#define SUBJECT_LEN	   8
#define USER_NAME_LEN  12

struct MAP_APP_PushMsgParam
{
	BTUCHAR						*path;
	BtSdkMAPPushMsgParamStru	req_param;
};

/* Remote Message */
typedef struct AppMsgObj {
	BTUINT8 msg_handle[BTSDK_MAP_MSGHDL_LEN];
	BTUINT8 msg_type[BTSDK_MAP_MSGTYPE_LEN];
	BTUINT8 subject[SUBJECT_LEN];
	BTUINT8 sender_name[USER_NAME_LEN];
	BTUINT8 date_time[BTSDK_MAP_TIME_LEN];
	BTUINT8 read_st;						/* 0 - Unread, 1 - Read */
} AppMsgObjStru, *PAppMsgObjStru;

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Convert FILETIME to the string in format "YYYYMMDDTHHMMSS".
------------------------------------------------------------------------------------*/
void MAP_ConvertFileTime(BTUINT8 *time_str, PFILETIME file_time)
{
	SYSTEMTIME sys_time;
	FileTimeToSystemTime(file_time, &sys_time);
	sprintf(time_str, "%04d%02d%02dT%02d%02d%02dZ",sys_time.wYear,sys_time.wMonth,
		sys_time.wDay,sys_time.wHour,sys_time.wMinute,sys_time.wSecond);
}

char* MAP_file_open(const char* filename)
{
	char* buff = NULL;
	int fsize;
	int num;
	FILE* fp;
	fp = fopen(filename, "rb+");
	if(fp == 0)
	{
		printf("couldn't open file %s\n", filename);
		return NULL;
	}
	fseek(fp, 0, SEEK_END);
	fsize = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	
	buff = (char*)malloc(fsize+1);
	memset(buff, 0, sizeof(buff));
	num = fread(buff, 1, fsize, fp);
	if(num < fsize)
	{
		if(0 != ferror(fp))
		{
			printf("there's some mistakes when reading\n");
			return NULL;
		}
		if(0 == feof(fp))
			printf("the file has reached the end\n");
		else
		{
			printf("the file ended adnomrally\n");
			return NULL;
		}
	}
	fclose(fp);
	
	return buff;
}

char* MAP_set_value(char* buffer, char* key, char* value)
{
	char* location;
	char* first = NULL;
	int totalLen;
	int firstLen;
	int secondLen;
	char* second;
	char* retBuf;
	char* interBuf = buffer;
	location = strstr(interBuf, key);
	location += strlen(key);
	
	first = malloc(location - interBuf + 1);
	memset(first, 0, sizeof(first));
	strncpy(first, interBuf, location - interBuf);
	first[location - interBuf] = '\0';

	totalLen = strlen(interBuf);
	firstLen = strlen(first);
	secondLen = totalLen - firstLen - 2;
	second = malloc(secondLen + 1);
	memset(second, 0, sizeof(second));
	strncpy(second, location, secondLen);
	second[secondLen] = '\0'; 

	retBuf = (char*)malloc(sizeof(char)*(totalLen + strlen(value) +1));
	memset(retBuf, 0, sizeof(retBuf));
	sprintf(retBuf, "%s%s%s", first, value, second);
	retBuf[totalLen + strlen(value)] = '\0';

	free(first);
	free(second);
	free(buffer);
	return retBuf;
}

void MAP_noti_func(BTSVCHDL svc_hdl, PBtSdkMAPEvReportObjStru ev_ob)
{
	char evt_type[MAX_PATH] = {0};

	switch (ev_ob->ev_type) {
	case BTSDK_MAP_EVT_NEWMSG:
		strcpy(evt_type, "NewMessage");
		break;
	case BTSDK_MAP_EVT_DELIVERY_OK:
		strcpy(evt_type, "DeliverySuccess");
		break;
	case BTSDK_MAP_EVT_SEND_OK:
		strcpy(evt_type, "SendingSuccess");
		break;
	case BTSDK_MAP_EVT_DELIVERY_FAIL:
		strcpy(evt_type, "DeliveryFailure");
		break;
	case BTSDK_MAP_EVT_SEND_FAIL:
		strcpy(evt_type, "SendingFailure");
		break;
	case BTSDK_MAP_EVT_MEM_FULL:
		strcpy(evt_type, "MemoryFull");
		break;
	case BTSDK_MAP_EVT_MEM_READY:
		strcpy(evt_type, "MemoryAvailable");
		break;
	case BTSDK_MAP_EVT_MSG_DELETED:
		strcpy(evt_type, "MessageDeleted");
		break;
	case BTSDK_MAP_EVT_MSG_SHIFT:
		strcpy(evt_type, "MessageShift");
		break;
	default:
	    break;
	}
	printf("A new message is coming: %s \n", evt_type);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to return the root directory.
---------------------------------------------------------------------------*/
BTUCHAR *MAP_GetRootDir(BTUCHAR *cur_path)
{
	LPITEMIDLIST pIDL = NULL;
	TCHAR szMyDocumentPath[MAX_PATH]={0};
	SHGetSpecialFolderLocation(NULL, CSIDL_PERSONAL, &pIDL);
	if(pIDL != NULL)
	{
		SHGetPathFromIDList(pIDL, szMyDocumentPath);
	}
	strcpy(g_root_mse, szMyDocumentPath);
	strcat(g_root_mse, "\\Bluetooth");
	_mkdir(g_root_mse);
	strcat(g_root_mse, "\\MAP\\");
	_mkdir(g_root_mse);
	strcat(g_root_mse, g_MsgRoot);
	_mkdir(g_root_mse);

	strcpy(cur_path, g_root_mse);
	return cur_path;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to change the working directory.
---------------------------------------------------------------------------*/
BTBOOL MAP_ChangeDir(const BTUCHAR *cur_path)
{
	return (_chdir(cur_path) == 0 ? BTSDK_TRUE : BTSDK_FALSE);
}

BTSDKHANDLE MAP_OpenFile(const BTUINT8* file_name)
{
	FILE *filehdl = fopen(file_name, "rb");
	if (filehdl != NULL)
	{
		return (BTSDKHANDLE)filehdl;
	}
	return (BTSDKHANDLE)NULL;
}

BTSDKHANDLE MAP_CreateFile(const BTUINT8* file_name)
{
	BTSDKHANDLE ret;
	ret = (BTSDKHANDLE)(file_name!=NULL ? fopen(file_name,"wb+") : tmpfile());
	return ret;
}

BTUINT32 MAP_WriteFile(BTSDKHANDLE file_hdl, BTUINT8* buf, BTUINT32 bytes_to_write)
{
	if (file_hdl != BTSDK_INVALID_HANDLE) 
	{
		return (fwrite((const void*)buf, 1, bytes_to_write, (FILE*)file_hdl));
	} 
	else 
	{
		return 0;
	}
}

BTUINT32 MAP_ReadFile(BTSDKHANDLE file_hdl, BTUINT8* buf, BTUINT32 len, BTBOOL *is_end)
{
	if (file_hdl != BTSDK_INVALID_HANDLE) 
	{
		BTUINT32 read_len = fread(buf, sizeof(char), len, (FILE*)file_hdl);
		if (read_len != len)
		{
			*is_end = TRUE;
		}
		else
		{
			*is_end = FALSE;
		}
		return read_len;
	} 
	else 
	{
		return 0;
	}
}

BTUINT32 MAP_GetFilesize(BTSDKHANDLE file_hdl)
{
	if (file_hdl != BTSDK_INVALID_HANDLE) 
	{
		BTUINT32 ori_pos;
		BTUINT32 len;
		ori_pos = ftell((FILE*)file_hdl);
		if (ori_pos != 0xFFFFFFFF)
		{
			fseek((FILE*)file_hdl, 0, SEEK_END);
			len = ftell((FILE*)file_hdl);
			fseek((FILE*)file_hdl, ori_pos, SEEK_SET);
		}
		if (len == 0xFFFFFFFF)
			len = 0;
		return len;
	} 
	else 
	{
		return 0;
	}
}

int MAP_Rewind(BTSDKHANDLE file_hdl, BTUINT32 offset)
{
	if (file_hdl != BTSDK_INVALID_HANDLE)
	{
		return fseek((FILE*)file_hdl, offset, SEEK_SET);
	}
	else
	{
		return -1;
	}
}

void MAP_CloseFile(BTSDKHANDLE file_hdl)
{
	if (file_hdl != BTSDK_INVALID_HANDLE) 
	{
		fclose((FILE*)file_hdl);
	}
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Check whether message fulfills FilterMessageType condition.
------------------------------------------------------------------------------------*/
BOOL MAP_CheckFilterMsgType(char *msg_type, BTUINT8 filter_type)
{
	BOOL ret;

	if ((filter_type & BTSDK_MAP_FILTEROUT_SMSGSM) &&
		(_stricmp(msg_type, "SMS_GSM") == 0))
	{
		ret = FALSE;
	} 
	else if ((filter_type & BTSDK_MAP_FILTEROUT_SMSCDMA) &&
		     (_stricmp(msg_type, "SMS_CDMA") == 0))
	{
		ret = FALSE;
	}
	else if ((filter_type & BTSDK_MAP_FILTEROUT_EMAIL) &&
		     (_stricmp(msg_type, "EMAIL") == 0))
	{
		ret = FALSE;
	}
	else if ((filter_type & BTSDK_MAP_FILTEROUT_MMS) &&
		     (_stricmp(msg_type, "MMS") == 0))
	{
		ret = FALSE;
	}
	else
	{
		ret = TRUE;
	}
	return ret;
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Check whether message fulfills FilterPeriodBegin and FilterPeriodEnd conditions.
------------------------------------------------------------------------------------*/
BOOL MAP_CheckFilterPeriod(char *date_time, PBtSdkMAPMsgFilterStru pfilter)
{
	BOOL ret = FALSE;
	
	do
	{
		if ((pfilter->mask & BTSDK_MAP_GMLP_PERIODBEGIN) &&
			(pfilter->mask & BTSDK_MAP_GMLP_PERIODEND)) {
			/* End is smaller than Begin */
			if (strcmp(pfilter->filter_period_end, pfilter->filter_period_begin) < 0)
			{
				break;
			}
		}
    	if (pfilter->mask & BTSDK_MAP_GMLP_PERIODBEGIN) {
    		/* Delivery time is smaller than Begin */
    		if (strcmp(date_time, pfilter->filter_period_begin) < 0) {
    			break;
    		}
    	}
    	if (pfilter->mask & BTSDK_MAP_GMLP_PERIODEND) {
    		/* Delivery time is larger than end */
    		if (strcmp(pfilter->filter_period_end, date_time) < 0) {
    			break;
    		} 
    	}
		ret = TRUE;
	} while(0);

	return ret;
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Check whether message fulfills FilterReadStatus condition.
------------------------------------------------------------------------------------*/
BOOL MAP_CheckFilterReadStatus(char *read_status, BTUINT8 filter_read_status)
{
	BOOL ret;

	if (filter_read_status == BTSDK_MAP_MSG_FILTER_ST_ALL)
	{
		ret = TRUE;
	}
	else if ((filter_read_status & BTSDK_MAP_MSG_FILTER_ST_UNREAD) &&
			 (_stricmp(read_status, "UNREAD") == 0))
	{
		ret = TRUE;
	}
	else if ((filter_read_status & BTSDK_MAP_MSG_FILTER_ST_READ) &&
			 (_stricmp(read_status, "READ") == 0))
	{
		ret = TRUE;
	}
	else
	{
		ret = FALSE;
	}

	return ret;
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Check whether message fulfills FilterRecipient condition.
------------------------------------------------------------------------------------*/
BOOL MAP_CheckFilterRecipient(char *parse_obj, BTUCHAR *filter_recipient)
{
	char *prop_val;
	char *find_hdl = NULL;
	BOOL is_matched = FALSE;
	BTUINT32 prop_len = 0;

	/* NOTE: May require charset conversion before compare. Below is a demo only, assume UTF-8. */
	prop_val = FindFirstPropertyValue(parse_obj, IDX_RPOP_BMSG_RECIPIENT_N, &find_hdl, &prop_len);
	while (prop_val != NULL)
	{
		if (strstr(prop_val, filter_recipient) != NULL)
		{
			free(prop_val);
			is_matched = TRUE;
			break;
		}
		free(prop_val);
		prop_val = FindNextPropertyValue(find_hdl, &prop_len);
	}
	EndFindPropertyValue(find_hdl);

	if (!is_matched)
	{
		prop_val = FindFirstPropertyValue(parse_obj, IDX_RPOP_BMSG_RECIPIENT_TEL, &find_hdl, &prop_len);
		while (prop_val != NULL)
		{
			if (strstr(prop_val, filter_recipient) != NULL)
			{
				free(prop_val);
				is_matched = TRUE;
				break;
			}
			free(prop_val);
			prop_val = FindNextPropertyValue(find_hdl, &prop_len);
		}
		EndFindPropertyValue(find_hdl);
	}
	if (!is_matched)
	{
		prop_val = FindFirstPropertyValue(parse_obj, IDX_RPOP_BMSG_RECIPIENT_EMAIL, &find_hdl, &prop_len);
		while (prop_val != NULL)
		{
			if (strstr(prop_val, filter_recipient) != NULL)
			{
				free(prop_val);
				is_matched = TRUE;
				break;
			}
			free(prop_val);
			prop_val = FindNextPropertyValue(find_hdl, &prop_len);
		}
		EndFindPropertyValue(find_hdl);
	}
	return is_matched;
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Check whether message fulfills FilterOriginator condition.
------------------------------------------------------------------------------------*/
BOOL MAP_CheckFilterOriginator(char *parse_obj, BTUCHAR *filter_originator)
{
	char *prop_val;
	BOOL is_matched = FALSE;
	BTUINT32 prop_len = 0;

	/* NOTE: May require charset conversion before compare. Below is a demo only, assume UTF-8. */
	prop_val = GetPropertValue(parse_obj, IDX_RPOP_BMSG_SENDER_N, &prop_len);
	if (prop_val != NULL)
	{
		if (strstr(prop_val, filter_originator) != NULL)
		{
			is_matched = TRUE;
		}
		free(prop_val);
	}

	if (!is_matched)
	{
		prop_val = GetPropertValue(parse_obj, IDX_RPOP_BMSG_SENDER_TEL, &prop_len);
		if (prop_val != NULL)
		{
			if (strstr(prop_val, filter_originator) != NULL)
			{
				is_matched = TRUE;
			}
			free(prop_val);
		}
	}
	if (!is_matched)
	{
		prop_val = GetPropertValue(parse_obj, IDX_RPOP_BMSG_SENDER_EMAIL, &prop_len);
		if (prop_val != NULL)
		{
			if (strstr(prop_val, filter_originator) != NULL)
			{
				is_matched = TRUE;
			}
			free(prop_val);
		}
	}
	return is_matched;
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Get sender_addressing value.
------------------------------------------------------------------------------------*/
BOOL MAP_GetAddressing(char *parse_obj, BOOL is_sender, BTUCHAR *addr)
{
	char *prop_val;
	BTUINT32 prop_len;
	BOOL ret = FALSE;
	BTUINT8 id_tel = is_sender ? IDX_RPOP_BMSG_SENDER_TEL : IDX_RPOP_BMSG_RECIPIENT_TEL;
	BTUINT8 id_email = is_sender ? IDX_RPOP_BMSG_SENDER_EMAIL : IDX_RPOP_BMSG_RECIPIENT_EMAIL;

	/*Get Message type Value*/
	prop_val = GetPropertValue(parse_obj, IDX_RPOP_BMSG_TYPE, &prop_len);
	if (prop_val != NULL)
	{
		if (_stricmp(prop_val, "EMAIL") == 0)
		{
			free(prop_val);
			prop_val = GetPropertValue(parse_obj, id_email, &prop_len);
		}
		else if ((_stricmp(prop_val, "SMS_CDMA") == 0) || (_stricmp(prop_val, "SMS_GSM") == 0))
		{
			free(prop_val);
			prop_val = GetPropertValue(parse_obj, id_tel, &prop_len);
		}
		else
		{
			/* MMS or others */
			free(prop_val);
			prop_val = GetPropertValue(parse_obj, id_email, &prop_len);
			if (prop_val == NULL)
			{
				prop_val = GetPropertValue(parse_obj, id_tel, &prop_len);
			} 
		}
		if (prop_val != NULL)
		{
			strncpy(addr, prop_val, (prop_len >= BTSDK_MAP_ADDR_LEN) ? (BTSDK_MAP_ADDR_LEN - 1) : prop_len);
			free(prop_val);
			ret = TRUE;
		}
	}
	return ret;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to parse the message file and check whether the message fulfills
all filter conditions. It returns message property values if the message passes
the condition check.
---------------------------------------------------------------------------*/
BOOL MAP_SetMsgObjValue(PWIN32_FIND_DATA file_info, PBtSdkMAPMsgFilterStru pfilter, PBtSdkMAPMsgObjStru pmsg)
{
	BOOL ret = FALSE;
	BTSDKHANDLE file_hdl;
	char *parse_obj;
	char *prop_val;
	BTUINT32 prop_len;
	BTUINT32 param_mask;

	file_hdl = MAP_OpenFile(file_info->cFileName);
	if (file_hdl != BTSDK_INVALID_HANDLE)
	{
		parse_obj = BMessageParser((BMSG_FILE_HDL)file_hdl);
		if (parse_obj != NULL)
		{
			do
			{
				if (pfilter != NULL)
				{
					param_mask = pfilter->param_mask;
				}
				else
				{
					param_mask = 0xFFFFFFFF;	/* Default to return all possible attributes */
				}
				/* Get message type Value */
				prop_val = GetPropertValue(parse_obj, IDX_RPOP_BMSG_TYPE, &prop_len);
				if ((pfilter != NULL) && (pfilter->mask & BTSDK_MAP_GMLP_MSGTYPE))
				{
					/* Check FilterMessageType */
					if (prop_val == NULL)
					{
						break;
					}
					if (!MAP_CheckFilterMsgType(prop_val, pfilter->filter_msg_type))
					{
						free(prop_val);
						break;
					}
				}
				if (prop_val != NULL)
				{
					if ((param_mask & BTSDK_MAP_MP_TYPE) && (pmsg != NULL))
					{
						strcpy(pmsg->msg_type, prop_val);
						pmsg->mask |= BTSDK_MAP_MP_TYPE;
					}
					free(prop_val);
				}

				/* Get datetime value */
				prop_val = malloc(BTSDK_MAP_TIME_LEN);
				MAP_ConvertFileTime(prop_val, &file_info->ftLastWriteTime);
				if ((pfilter != NULL) && (pfilter->mask & (BTSDK_MAP_GMLP_PERIODBEGIN|BTSDK_MAP_GMLP_PERIODEND)))
				{
					/* Check FilterPeriod */
					if (prop_val == NULL)
					{
						break;
					}
					if (!MAP_CheckFilterPeriod(prop_val, pfilter))
					{
						free(prop_val);
						break;
					}
				}
				if (prop_val != NULL)
				{
					if ((param_mask & BTSDK_MAP_MP_DATATIME) && (pmsg != NULL))
					{
						strcpy(pmsg->date_time, prop_val);
						pmsg->mask |= BTSDK_MAP_MP_DATATIME;
					}
					free(prop_val);
				}

				/* Get read status value */
				prop_val = GetPropertValue(parse_obj, IDX_RPOP_BMSG_READSTATUS, &prop_len);
				if ((pfilter != NULL) && (pfilter->mask & BTSDK_MAP_GMLP_READSTATUS))
				{
					/* Check FilterReadStatus */
					if (prop_val == NULL)
					{
						break;
					}
					if (!MAP_CheckFilterReadStatus(prop_val, pfilter->filter_read_status))
					{
						free(prop_val);
						break;
					}
				}
				if (prop_val != NULL)
				{
					if ((param_mask & BTSDK_MAP_MP_READ) && (pmsg != NULL))
					{
						if (_stricmp(prop_val, "READ") == 0)
						{
							pmsg->read = BTSDK_TRUE;
						}
						pmsg->mask |= BTSDK_MAP_MP_READ;
					}
					free(prop_val);
				}

				if ((pfilter != NULL) && (pfilter->mask & BTSDK_MAP_GMLP_RECIPIENT))
				{
					/* Check FilterRecipient */
					if (!MAP_CheckFilterRecipient(parse_obj, pfilter->filter_recipient))
					{
						break;
					}
				}

				if ((pfilter != NULL) && (pfilter->mask & BTSDK_MAP_GMLP_ORIGINATOR))
				{
					/* Check FilterOriginator */
					if (!MAP_CheckFilterOriginator(parse_obj, pfilter->filter_originator))
					{
						break;
					}
				}

				if ((pfilter != NULL) && (pfilter->mask & BTSDK_MAP_GMLP_PRIORITY))
				{
					/* Check priority */
					/* TO BE IMPLEMENTED */
				}
				/* Get priority value */
				if (param_mask & BTSDK_MAP_MP_PRIORITY)
				{
					/* Demo only - return what is expected simply */
					pmsg->mask |= BTSDK_MAP_MP_PRIORITY;
					if (pfilter->filter_priority & BTSDK_MAP_MSG_FILTER_PRI_HIGH)
					{
						pmsg->priority = BTSDK_TRUE;
					}
				}

				ret = TRUE;
				if (pmsg == NULL)
				{
					break;
				}

				/* Get message handle value */
				prop_val = strstr(file_info->cFileName, ".msg");
				if (prop_val != NULL)
				{
					*prop_val = 0;
					strncpy((char*)pmsg->msg_handle, file_info->cFileName, BTSDK_MAP_MSGHDL_LEN);
				}

				/* Get subject value */
				if (param_mask & BTSDK_MAP_MP_SUBJECT)
				{
					prop_val = GetPropertValue(parse_obj, IDX_PROP_BMSG_BODY_CONTENT, &prop_len);
					if (prop_val != NULL)
					{
						if ((pfilter != NULL) && (pfilter->mask & BTSDK_MAP_GMLP_SUBJECTLENTH))
						{
							prop_len = prop_len > pfilter->subject_length ? pfilter->subject_length : prop_len;
						}
						else
						{
							prop_len = prop_len >= BTSDK_MAP_SUBJECT_LEN ? (BTSDK_MAP_SUBJECT_LEN - 1) : prop_len;
						}
						strncpy(pmsg->subject, prop_val, prop_len);
						pmsg->mask |= BTSDK_MAP_MP_SUBJECT;
						free(prop_val);
					}
				}

				/* Get sender_name value */
				if (param_mask & BTSDK_MAP_MP_SENDERNAME)
				{
					prop_val = GetPropertValue(parse_obj, IDX_RPOP_BMSG_SENDER_N, &prop_len);
					if (prop_val != NULL)
					{
						strncpy(pmsg->sender_name, prop_val, (prop_len >= BTSDK_MAP_USERNAME_LEN) ? (BTSDK_MAP_USERNAME_LEN - 1) : prop_len);
						free(prop_val);
						pmsg->mask |= BTSDK_MAP_MP_SENDERNAME;
					}
				}

				/* Get sender_addressing value */
				if (param_mask & BTSDK_MAP_MP_SENDERADDR)
				{
					if (MAP_GetAddressing(parse_obj, TRUE, pmsg->sender_addr))
					{
						pmsg->mask |= BTSDK_MAP_MP_SENDERADDR;
					}
				}

				/* Get replyto_addressing value */
				if (param_mask & BTSDK_MAP_MP_REPLY2ADDR)
				{
					/* TO BE IMPLEMENTED */
				}

				/* Get recipient_name value */
				if (param_mask & BTSDK_MAP_MP_RECIPIENTNAME)
				{
					prop_val = GetPropertValue(parse_obj, IDX_RPOP_BMSG_RECIPIENT_N, &prop_len);
					if (prop_val != NULL)
					{
						strncpy(pmsg->recipient_name, prop_val, (prop_len >= BTSDK_MAP_USERNAME_LEN) ? (BTSDK_MAP_USERNAME_LEN - 1) : prop_len);
						free(prop_val);
						pmsg->mask |= BTSDK_MAP_MP_RECIPIENTNAME;
					}
				}

				/* Get recipient_addressing value */
				if (param_mask & BTSDK_MAP_MP_RECIPIENTADDR)
				{
					if (MAP_GetAddressing(parse_obj, FALSE, pmsg->recipient_addr))
					{
						pmsg->mask |= BTSDK_MAP_MP_RECIPIENTADDR;
					}
				}

				/* Get reception_status value */
				if (param_mask & BTSDK_MAP_MP_RECPSTATUS)
				{
					/* TO BE IMPLEMENTED */
					/* Assume complete */
					pmsg->reception_status = BTSDK_MAP_MSG_RCVST_COMPLETE;
					pmsg->mask |= BTSDK_MAP_MP_RECPSTATUS;
				}
				
				/* Get message size value */
				if (param_mask & BTSDK_MAP_MP_SIZE)
				{
					prop_val = GetPropertValue(parse_obj, IDX_RPOP_BMSG_BODY_LENGTH, &prop_len);
					if (prop_val != NULL)
					{
						pmsg->msg_size = atol(prop_val);
						free(prop_val);
						pmsg->mask |= BTSDK_MAP_MP_SIZE;
					}
				}

				/* Get attachment size value */
				if (param_mask & BTSDK_MAP_MP_ATTACHSIZE)
				{
					/* TO BE IMPLEMENTED */
					/* Assume 0 */
					pmsg->mask |= BTSDK_MAP_MP_ATTACHSIZE;
				}

				/* Get text value */
				if (param_mask & BTSDK_MAP_MP_TEXT)
				{
					/* TO BE IMPLEMENTED */
					/* Assume text */
					pmsg->text = BTSDK_TRUE;
					pmsg->mask |= BTSDK_MAP_MP_TEXT;
				}
				
				/* Get sent value */
				if (param_mask & BTSDK_MAP_MP_SENT)
				{
					/* TO BE IMPLEMENTED */
					/* Assume sent */
					pmsg->sent = BTSDK_TRUE;
					pmsg->mask |= BTSDK_MAP_MP_SENT;
				}
				
				/* Get protected value */
				if (param_mask & BTSDK_MAP_MP_PROTECTED)
				{
					/* TO BE IMPLEMENTED */
					/* Assume not protected */
					pmsg->mask |= BTSDK_MAP_MP_PROTECTED;
				}
			} while (0);
			BMessageClose(parse_obj);
		}
		MAP_CloseFile(file_hdl);
	}

	return ret;
}


/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Start a folder search.
------------------------------------------------------------------------------------*/
BTSDKHANDLE MAP_FindFirstFolder(const BTUINT8 *path, PBtSdkMAPFolderObjStru pfd)
{	
	BTUINT8 *full_path;
	WIN32_FIND_DATA data_info;	 
	HANDLE hdl;
	
	full_path = malloc(strlen(path) + strlen("*.*") + 1);
	strcpy(full_path, path);
	strcat(full_path, "*.*");
	if ((hdl = FindFirstFile(full_path, &data_info)) != INVALID_HANDLE_VALUE) 
	{
		while (!strcmp(data_info.cFileName, ".") ||
			!strcmp(data_info.cFileName, "..") ||
			(strlen(data_info.cFileName) >= BTSDK_MAP_FOLDER_LEN) ||
			!(data_info.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)) 
		{
			if (!FindNextFile(hdl, &data_info))
			{
				FindClose(hdl);
				hdl = (HANDLE)BTSDK_INVALID_HANDLE;
				break;
			}
		}
		strcpy(pfd->name, data_info.cFileName);
		MAP_ConvertFileTime(pfd->create_time, &data_info.ftCreationTime);
		MAP_ConvertFileTime(pfd->access_time, &data_info.ftLastAccessTime);
		MAP_ConvertFileTime(pfd->modify_time, &data_info.ftLastWriteTime);
	} else {
		hdl = (HANDLE)BTSDK_INVALID_HANDLE;
	}
	free(full_path);
	return (BTSDKHANDLE)hdl;
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Continues a folder search from a previous call to the MAP_APP_FindFirstFolder function.
------------------------------------------------------------------------------------*/
BTBOOL MAP_FindNextFolder(BTSDKHANDLE hdl, PBtSdkMAPFolderObjStru pfd)
{
	WIN32_FIND_DATA data_info;
	BOOL r = FALSE;
	BTBOOL ret = BTSDK_FALSE;
	
	do
	{
		r = FindNextFile((HANDLE)hdl, &data_info);
	} while (r && ((strlen(data_info.cFileName) >= BTSDK_MAP_FOLDER_LEN) || 
		!(data_info.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)));
	if (r)
	{
		strcpy(pfd->name, data_info.cFileName);
		MAP_ConvertFileTime(pfd->create_time, &data_info.ftCreationTime);
		MAP_ConvertFileTime(pfd->access_time, &data_info.ftLastAccessTime);
		MAP_ConvertFileTime(pfd->modify_time, &data_info.ftLastWriteTime);
		ret = BTSDK_TRUE;
	}

	return ret;
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Closes the specified search handle. 
------------------------------------------------------------------------------------*/
BTBOOL MAP_FindFolderClose(BTSDKHANDLE hdl)
{
	FindClose((HANDLE)hdl);
	return BTSDK_TRUE;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to search a directory for a message file.
---------------------------------------------------------------------------*/
BTSDKHANDLE MAP_FindFirstMessage(const BTUINT8 *path, PBtSdkMAPMsgFilterStru pfilter, PBtSdkMAPMsgObjStru pmsg)
{
	WIN32_FIND_DATA data_info = {0};
	HANDLE hdl = (HANDLE)BTSDK_INVALID_HANDLE;
	
	if (MAP_ChangeDir(path))
	{
		hdl = FindFirstFile("*.msg", &data_info);
		if (hdl != INVALID_HANDLE_VALUE)
		{
			while ((data_info.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ||
				!MAP_SetMsgObjValue(&data_info, pfilter, pmsg))
			{
				if (!FindNextFile(hdl, &data_info))
				{
					FindClose(hdl);
					hdl = (HANDLE)BTSDK_INVALID_HANDLE;
					break;
				}
			}
		}
	}
	
	return (BTSDKHANDLE)hdl;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to search a directory for a message file.
---------------------------------------------------------------------------*/
BTBOOL MAP_FindNextMessage(BTSDKHANDLE hdl, PBtSdkMAPMsgFilterStru pfilter, PBtSdkMAPMsgObjStru pmsg)
{
	WIN32_FIND_DATA data_info = {0};
	BOOL r = TRUE;
	
	do
	{
		r = FindNextFile((HANDLE)hdl, &data_info);
	} while (r && ((data_info.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) || 
		!MAP_SetMsgObjValue(&data_info, pfilter, pmsg)));
	
	return (r ? BTSDK_TRUE : BTSDK_FALSE);
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Closes the specified search handle. 
------------------------------------------------------------------------------------*/
BTBOOL MAP_FindMessageClose(BTSDKHANDLE hdl)
{
	FindClose((HANDLE)hdl);
	return BTSDK_TRUE;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to search message in a directory and its sub-folders.
---------------------------------------------------------------------------*/
BTBOOL MAP_FindMessageInDir(const BTUCHAR *cur_path, BTUCHAR *msg_name, BTSDKHANDLE *file_hdl)
{
	HANDLE find_hdl;
	PWIN32_FIND_DATA find_data;
	BOOL find_result = TRUE;
	BTBOOL ret = BTSDK_FALSE;
	BTUCHAR *sub_path = NULL;
	
	if (file_hdl != NULL)
	{
		*file_hdl = BTSDK_INVALID_HANDLE;
	}
	if (MAP_ChangeDir(cur_path))
	{
		find_data = malloc(sizeof(WIN32_FIND_DATA));
		find_hdl = FindFirstFile("*.*", find_data);
		while (find_result && (find_hdl != INVALID_HANDLE_VALUE))
		{
			if (!strcmp((const UCHAR*)find_data->cFileName, "..") ||
				!strcmp((const UCHAR*)find_data->cFileName, "."))
			{
				find_result = FindNextFile(find_hdl, find_data);
			}
			else
			{
				if (find_data->dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
				{
					/* Search sub-folder */
					sub_path = malloc(strlen(cur_path) + strlen(find_data->cFileName) + 2);
					strcpy(sub_path, cur_path);
					strcat(sub_path, find_data->cFileName);
					strcat(sub_path, "\\");
					ret = MAP_FindMessageInDir(sub_path, msg_name, file_hdl);
					free(sub_path);
					if (ret)
					{
						break;
					}
				}
				else if (!_stricmp(find_data->cFileName, msg_name))
				{
					ret = BTSDK_TRUE;
					if (file_hdl != NULL)
					{
						*file_hdl = MAP_OpenFile(find_data->cFileName);
					}
					break;
				}
				find_result = FindNextFile(find_hdl, find_data);
			}
		}
		FindClose(find_hdl);
		free(find_data);
	}
	
	return ret;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to search message and returns its relative path.
---------------------------------------------------------------------------*/
BTBOOL MAP_GetMessagePath(const BTUCHAR *cur_path, BTUCHAR *msg_name, BTUCHAR **sub_path)
{
	HANDLE find_hdl;
	PWIN32_FIND_DATA find_data;
	BOOL find_result = TRUE;
	BTUCHAR *tmp_path = NULL;
	BTBOOL ret = FALSE;
	
	*sub_path = NULL;
	if (MAP_ChangeDir(cur_path))
	{
		find_data = malloc(sizeof(WIN32_FIND_DATA));
		find_hdl = FindFirstFile("*.*", find_data);
		while (find_result && (find_hdl != INVALID_HANDLE_VALUE))
		{
			if (!strcmp((const UCHAR*)find_data->cFileName, "..") ||
				!strcmp((const UCHAR*)find_data->cFileName, "."))
			{
				find_result = FindNextFile(find_hdl, find_data);
			}
			else
			{
				if (find_data->dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
				{
					/* Search sub-folder */
					tmp_path = malloc(strlen(cur_path) + strlen(find_data->cFileName) + 2);
					strcpy(tmp_path, cur_path);
					strcat(tmp_path, find_data->cFileName);
					strcat(tmp_path, "\\");
					ret = MAP_GetMessagePath(tmp_path, msg_name, sub_path);
					free(tmp_path);
					if (ret)
					{
						if (*sub_path != NULL)
						{
							tmp_path = malloc(strlen(find_data->cFileName) + strlen(*sub_path) + 2);
							strcpy(tmp_path, find_data->cFileName);
							strcat(tmp_path, "\\");
							strcat(tmp_path, *sub_path);
						}
						else
						{
							tmp_path = malloc(strlen(find_data->cFileName) + 2);
							strcpy(tmp_path, find_data->cFileName);
							strcat(tmp_path, "\\");
						}
						free(*sub_path);
						*sub_path = tmp_path;
						break;
					}
				}
				else if (!_stricmp(find_data->cFileName, msg_name))
				{
					ret = BTSDK_TRUE;
					break;
				}
				find_result = FindNextFile(find_hdl, find_data);
			}
		}
		FindClose(find_hdl);
		free(find_data);
	}
	
	return ret;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to verify the validity of a message handle.
---------------------------------------------------------------------------*/
BTBOOL MAP_IsValidHdl(BTUINT8 *msg_hdl)
{
	BTUCHAR *path;
	BTUCHAR *msg_name;
	BTBOOL is_valid = TRUE;
	
	path = malloc(strlen(g_root_mse) + sizeof("\\telecom\\msg\\"));
	strcpy(path, g_root_mse);
	strcat(path, "\\telecom\\msg\\");
	msg_name = malloc(BTSDK_MAP_MSGHDL_LEN + sizeof(".msg"));
	strcpy(msg_name, msg_hdl);
	strcat(msg_name, ".msg");
	if (MAP_FindMessageInDir(path, msg_name, NULL))
	{
		is_valid = FALSE;
	}
	free(msg_name);
	free(path);
	
	return is_valid;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to assign a message handle, the handle is represent as 16 
hexs digits.
---------------------------------------------------------------------------*/
BTBOOL MAP_AssigndHdl(BTUINT8 *hdl)
{
	BTBOOL ret;
	do
	{
		sprintf(hdl, "%08x%08x", s_high_idx, s_low_dix);
		s_low_dix++;
		ret = MAP_IsValidHdl(hdl);
	} while (!ret && (s_low_dix < MAX_LOW_IDX));
	return ret;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to simulate pushing message to the network and send event to
the MCE.
---------------------------------------------------------------------------*/
BTUINT32 WINAPI MAP_PushMsgThread(struct MAP_APP_PushMsgParam *thread_param)
{
	BtSdkMAPEvReportObjStru ev_obj;
	BTUCHAR *src_path;
	BTUCHAR *dest_path;
	
	/* TO BE IMPLEMENTED: Send the message to the telecom network if required */
	
	if (strcmp(thread_param->req_param.folder, "outbox") == 0)
	{
		/* Assume the message is sending to the network successfully */
		if (s_mse_notif_on)
		{
			memset(&ev_obj, 0, sizeof(BtSdkMAPEvReportObjStru));
			ev_obj.ev_type = BTSDK_MAP_EVT_SEND_OK;
			strcpy(ev_obj.msg_handle, thread_param->req_param.msg_handle);
			Btsdk_MAPSendEvent(s_mns_conn_hdl, &ev_obj);
		}
		
		if (thread_param->req_param.save_copy)
		{
			/* Move file from \outbox to \sent */
			src_path = malloc(strlen(thread_param->path) + strlen(thread_param->req_param.msg_handle) + sizeof(".msg"));
			dest_path = malloc(strlen(g_root_mse) + sizeof("telecom\\msg\\sent\\.msg") + strlen(thread_param->req_param.msg_handle));
			strcpy(src_path, thread_param->path);
			strcat(src_path, thread_param->req_param.msg_handle);
			strcat(src_path, ".msg");
			strcpy(dest_path, g_root_mse);
			strcat(dest_path, "telecom\\msg\\sent\\");
			strcat(dest_path, thread_param->req_param.msg_handle);
			strcat(dest_path, ".msg");
			MoveFile(src_path, dest_path);
			free(src_path);
			free(dest_path);
		}
	}
	free(thread_param->path);
	free(thread_param);
	return 1;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is create the bMessage file under the current directory. 
the file name is in the format of <msg_handle>.msg.
---------------------------------------------------------------------------*/
BTSDKHANDLE MAP_CreateBMsgFile(const BTUCHAR *cur_path, PBtSdkMAPMsgHandleStru msg_hdl)
{
	BTUCHAR* msg_name;
	BTSDKHANDLE file_hdl = BTSDK_INVALID_HANDLE;
	
	if (MAP_AssigndHdl(msg_hdl->msg_handle))
	{
		if (MAP_ChangeDir(cur_path))
		{
			/* Create file under current directory */
			msg_name = malloc(BTSDK_MAP_MSGHDL_LEN + sizeof(".msg"));
			strcpy(msg_name, msg_hdl->msg_handle);
			strcat(msg_name, ".msg");
			file_hdl = MAP_CreateFile(msg_name);
			free(msg_name);
		}
	}
	return file_hdl;	
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is open a BMessage file under current directory.
---------------------------------------------------------------------------*/
BTSDKHANDLE MAP_OpenBMsgFile(PBtSdkMAPGetMsgParamStru msg_info)
{
	BTUCHAR *msg_name;
	BTSDKHANDLE file_hdl = BTSDK_INVALID_HANDLE;
	
	msg_name = malloc(BTSDK_MAP_MSGHDL_LEN + sizeof(".msg"));
	strcpy(msg_name, msg_info->msg_handle);
	strcat(msg_name, ".msg");
	MAP_FindMessageInDir(g_root_mse, msg_name, &file_hdl);
	if (file_hdl != BTSDK_INVALID_HANDLE)
	{
		/* TO BE IMPLEMENTED:: Trans-coding bMessage content and prepare attachment as specified by msg_info */
		/* Assume this is the last fraction */
		msg_info->fraction_deliver = BTSDK_MAP_FRACT_RSPLAST;
	}
	free(msg_name);
	return file_hdl;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to send the message to the network received from MCE device recently.
---------------------------------------------------------------------------*/
BTBOOL MAP_ModifyMsgStatus(PBtSdkMAPMsgStatusStru msg_info)
{
	/*Open file according to cur_path and msg_info->msg_handle*/
	BTUCHAR *msg_name;
	BTUCHAR *file_path;
	BTUCHAR *src_path;
	BTUCHAR *dest_path;
	char *parse_obj;
	BTSDKHANDLE file_hdl;
	BTBOOL ret = FALSE;
	
	msg_name = malloc(BTSDK_MAP_MSGHDL_LEN + sizeof(".msg"));
	strcpy(msg_name, msg_info->msg_handle);
	strcat(msg_name, ".msg");
	if (msg_info->status_indicator == BTSDK_MAP_MSG_READ_STATUS)
	{
		MAP_FindMessageInDir(g_root_mse, msg_name, &file_hdl);
		if (file_hdl != BTSDK_INVALID_HANDLE)
		{
			parse_obj = BMessageParser((BMSG_FILE_HDL)file_hdl);
			if (parse_obj != NULL)
			{
				if (msg_info->status_value)
				{
					SetPropertyValue(parse_obj, IDX_RPOP_BMSG_READSTATUS, "READ", 4);
				}
				else
				{
					SetPropertyValue(parse_obj, IDX_RPOP_BMSG_READSTATUS, "UNREAD", 6);
				}
				MAP_CloseFile(file_hdl);
				file_hdl = MAP_CreateFile(msg_name);
				WriteVBMsgToFile((BMSG_FILE_HDL)file_hdl, parse_obj);
				BMessageClose(parse_obj);
				ret = BTSDK_TRUE;
			}
		}
	}
	else /* msg_info->status_indicator == BTSDK_MAP_MSG_DELETE_STATUS */
	{
		ret = MAP_GetMessagePath(g_root_mse, msg_name, &file_path);
		if (ret)
		{
			if (file_path != NULL)
			{
				src_path = malloc(strlen(g_root_mse) + strlen(file_path) + strlen(msg_name) + 1);
				strcpy(src_path, g_root_mse);
				strcat(src_path, file_path);
				strcat(src_path, msg_name);
				free(file_path);
			}
			else
			{
				src_path = malloc(strlen(g_root_mse) + strlen(msg_name) + 1);
				strcpy(src_path, g_root_mse);
				strcat(src_path, msg_name);
			}
			if (msg_info->status_value)
			{
				/* deletedStatus == Yes */
				if (strstr(src_path, "\\deleted\\") == NULL)
				{
					/* Move message to deleted folder */
					dest_path = malloc(strlen(g_root_mse) + sizeof("telecom\\msg\\deleted\\") + strlen(msg_name));
					strcpy(dest_path, g_root_mse);
					strcat(dest_path, "telecom\\msg\\deleted\\");
					strcat(dest_path, msg_name);
					MoveFile(src_path, dest_path);
					free(dest_path);
				}
				else
				{
					/* Delete message in deleted folder */
					remove(src_path);
				}
			}
			else
			{
				/* deletedStatus == No */
				if (strstr(src_path, "\\deleted\\") != NULL)
				{
					/* Move message from deleted to inbox folder */
					dest_path = malloc(strlen(g_root_mse) + sizeof("telecom\\msg\\inbox\\") + strlen(msg_name));
					strcpy(dest_path, g_root_mse);
					strcat(dest_path, "telecom\\msg\\inbox\\");
					strcat(dest_path, msg_name);
					MoveFile(src_path, dest_path);
					free(dest_path);
				}
			}
			free(src_path);
		}
	}
	free(msg_name);
	return ret;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to enable/disable notification.
---------------------------------------------------------------------------*/
BTBOOL MAP_RegisterNotification(BTCONNHDL mns_conn_hdl, BTSVCHDL mas_svc_hdl, BTBOOL turn_on)
{
	s_mse_notif_on = turn_on;
	if (turn_on)
	{
		printf("MCE requires notification and the MNS connection is created!\n");
		s_mns_conn_hdl = mns_conn_hdl;
	}
	else
	{
		printf("Notification is not required and the MNS connection is released!\n");
		s_mns_conn_hdl = BTSDK_INVALID_HANDLE;
	}
	return BTSDK_TRUE;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to update inbox.
---------------------------------------------------------------------------*/
BTBOOL MAP_UnpdateInbox(void)
{
	printf("MCE requires MSE to update its inbox folder\n");
	/* TO BE IMPLEMENTED:: Contact the network to retrieve new messages */
	
	return BTSDK_TRUE;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to update inbox.
---------------------------------------------------------------------------*/
BTBOOL MAP_GetMSETime(PBtSdkMAPMSETimeStru mse_time)
{
	SYSTEMTIME sys_time;
	GetSystemTime(&sys_time);
	sprintf(mse_time->mse_time, "%04d%02d%02dT%02d%02d%02dZ", sys_time.wYear, sys_time.wMonth,
		sys_time.wDay, sys_time.wHour, sys_time.wMinute, sys_time.wSecond);
	strcat(mse_time->mse_time, "+0800");
	return BTSDK_TRUE;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
This function is to send the message received from MCE device recently.
---------------------------------------------------------------------------*/
BTBOOL MAP_PushMessage(const BTUINT8 *cur_path, PBtSdkMAPPushMsgParamStru msg_info)
{
	struct MAP_APP_PushMsgParam *thread_param;
	BTUINT32 id;
	HANDLE thread_handle;

	/* NOTE: DONOT block this callback. Finish message sending in another thread */
	thread_param = malloc(sizeof(struct MAP_APP_PushMsgParam));
	thread_param->path = malloc(strlen(cur_path) + 1);
	strcpy(thread_param->path, cur_path);
	memcpy(&thread_param->req_param, msg_info, sizeof(BtSdkMAPPushMsgParamStru));
	thread_handle = CreateThread(NULL, 0, MAP_PushMsgThread, thread_param, 0, &id);
	CloseHandle(thread_handle);

	return BTSDK_TRUE;
}

void RegisterMNSService(void)
{
	BTSVCHDL svc_hdl = BTSDK_INVALID_HANDLE;
	Btsdk_MNS_MessageNotification_Func st_func;
	BtSdkMAPFileIORoutinesStru st_mapsvr_cb = {0};

	st_func = (Btsdk_MNS_MessageNotification_Func)MAP_noti_func;

	st_mapsvr_cb.open_file = (Btsdk_OpenFile_Func)MAP_OpenFile;
	st_mapsvr_cb.create_file = (Btsdk_CreateFile_Func)MAP_CreateFile;
	st_mapsvr_cb.write_file = (Btsdk_WriteFile_Func)MAP_WriteFile;
	st_mapsvr_cb.read_file = (Btsdk_ReadFile_Func)MAP_ReadFile;
	st_mapsvr_cb.get_file_size = (Btsdk_GetFileSize_Func)MAP_GetFilesize;
	st_mapsvr_cb.rewind_file = (Btsdk_RewindFile_Func)MAP_Rewind;
	st_mapsvr_cb.close_file = (Btsdk_CloseFile_Func)MAP_CloseFile;

	svc_hdl =  Btsdk_RegisterMNSService("message notification server", st_func, &st_mapsvr_cb);
	if (svc_hdl != BTSDK_INVALID_HANDLE)
	{
		printf("Register MNS service successfully!\n");
	}
	else
	{
		printf("Register MNS service Failed!\n");
	}
}

void MSERegisterMASService(void)
{
	BTSVCHDL svc_hdl = BTSDK_INVALID_HANDLE;
	BtSdkMASSvrCBStru  cbks;
	BtSdkLocalMASServerAttrStru svr_info;
	
	memset(&svr_info, 0, sizeof(BtSdkLocalMASServerAttrStru));
	svr_info.size = sizeof(BtSdkLocalMASServerAttrStru);
	MAP_GetRootDir(svr_info.root_dir);
	strcpy(svr_info.path_delimiter, "\\");
	svr_info.sup_msg_types = BTSDK_MAP_SUP_MSG_SMSGSM;
	
	memset(&cbks, 0, sizeof(BtSdkMASSvrCBStru));
	cbks.file_io_rtns.create_file = MAP_CreateFile;
	cbks.file_io_rtns.read_file = MAP_ReadFile;
	cbks.file_io_rtns.write_file = MAP_WriteFile;
	cbks.file_io_rtns.rewind_file = MAP_Rewind;
	cbks.file_io_rtns.close_file = MAP_CloseFile;
	cbks.file_io_rtns.open_file = MAP_OpenFile;
	cbks.file_io_rtns.get_file_size = MAP_GetFilesize;
	
	cbks.find_folder_rtns.find_first_folder = MAP_FindFirstFolder;
	cbks.find_folder_rtns.find_next_folder = MAP_FindNextFolder;
	cbks.find_folder_rtns.find_folder_close = MAP_FindFolderClose;
	
	cbks.find_msg_rtns.find_first_msg = MAP_FindFirstMessage;
	cbks.find_msg_rtns.find_next_msg = MAP_FindNextMessage;
	cbks.find_msg_rtns.find_msg_close = MAP_FindMessageClose;
	
	cbks.msg_io_rtns.create_bmsg_file = MAP_CreateBMsgFile;
	cbks.msg_io_rtns.open_bmsg_file = MAP_OpenBMsgFile;
	cbks.msg_io_rtns.modify_msg_status = MAP_ModifyMsgStatus;
	cbks.msg_io_rtns.push_msg = MAP_PushMessage;

	cbks.mse_status_rtns.register_notification = MAP_RegisterNotification;
	cbks.mse_status_rtns.update_inbox = MAP_UnpdateInbox;
	cbks.mse_status_rtns.get_mse_time = MAP_GetMSETime;
	
	svc_hdl = Btsdk_RegisterMASService("Sample MAP SMS Server", &svr_info, &cbks);
	if (svc_hdl != BTSDK_INVALID_HANDLE)
	{
		printf("Register MAS service successfully\n");
	}
	else
	{
		printf("Register MAS service Failed\n");
	}
}

void TestSelectRmtMAPDev()
{
	s_currRmtMAPDevHdl = SelectRemoteDevice(0);
	if (BTSDK_INVALID_HANDLE == s_currRmtMAPDevHdl)
	{
		printf("Please make sure that the expected device is in discoverable state and search again.\n");
	}	
}

void TestSelectMAPSvc()
{
	s_currMAPSvcHdl = SelectRemoteService(s_currRmtMAPDevHdl);
	if (BTSDK_INVALID_HANDLE == s_currMAPSvcHdl)
	{
		printf("Cann't get expected service handle.\n");
	}
}

void TestConnectMAPSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;
	ulRet = Btsdk_Connect(s_currMAPSvcHdl, 0, &s_currMAPConnHdl);
	if (BTSDK_OK != ulRet)
	{
		printf("Please make sure that the expected device is powered on and connectable.\n");
	}
}

void MCETestShowMenu()
{
	printf("\n");
	printf("***********************************\n");
	printf("*          MCE Testing Menu       *\n");
	printf("* <1> get folder listing in MSE   *\n");
	printf("* <2> set folder for MSE          *\n");
	printf("* <3> get message list            *\n");
	printf("* <4> get message                 *\n");
	printf("* <5> set message status          *\n");
	printf("* <6> push message                *\n");
	printf("* <7> update inbox                *\n");
	printf("* <8> Delete a Message            *\n");
	printf("* <9> set notification on         *\n");
	printf("* <a> set notification off        *\n");
	printf("* <b> disconnect                  *\n");
	printf("* <r> Return to upper menu        *\n");
	printf("***********************************\n");
	printf(">");	
}

void MAPAppStatusCB(BTUINT8 first, BTUINT8 last, BTUINT8* filename, BTUINT32 filesize, BTUINT32 cursize)
{
	static BTUINT32 s_cur_size = 0;
	static BTUINT32 s_total_size = 0;
	s_total_size = filesize;
	
	if (!last)
	{
		if (first)
		{
			printf("\nstart transferring %s:\n", filename);
			printf("%10d byte / %d transferred \n", s_cur_size, s_total_size);
		}
		else
		{
			s_cur_size += cursize;
			printf("\r%10d bytes", s_cur_size);
		}
		
	}
	else
	{
		s_cur_size += cursize;
		printf("\r%10d byte / %d transferred\n", s_cur_size, s_total_size);
		s_cur_size = 0;
		s_total_size = 0;
	}
}

void TestGetFolderList()
{
	BTSDKHANDLE hFile;
	BTUINT8 *strPath = NULL;
	BTUINT32 result = BTSDK_OK;
	BtSdkMAPGetFolderListParamStru param = {0};
	BTCONNHDL hconn = s_currMAPConnHdl;
	BtSdkMAPFileIORoutinesStru fileio_rtns = {0};
	if (hconn == BTSDK_INVALID_HANDLE)
	{
		printf("No MAP connection selected!\n");
		return;
	}

	fileio_rtns.open_file = (Btsdk_OpenFile_Func)MAP_OpenFile;
	fileio_rtns.create_file = (Btsdk_CreateFile_Func)MAP_CreateFile;
	fileio_rtns.write_file = (Btsdk_WriteFile_Func)MAP_WriteFile;
	fileio_rtns.read_file = (Btsdk_ReadFile_Func)MAP_ReadFile;
	fileio_rtns.get_file_size = (Btsdk_GetFileSize_Func)MAP_GetFilesize;
	fileio_rtns.rewind_file = (Btsdk_RewindFile_Func)MAP_Rewind;
	fileio_rtns.close_file = (Btsdk_CloseFile_Func)MAP_CloseFile;

	Btsdk_MAPRegisterFileIORoutines(hconn, &fileio_rtns);
	Btsdk_MAPRegisterStatusCallback(hconn, MAPAppStatusCB);

	param.mask = BTSDK_MAP_GFLP_MAXCOUNT|BTSDK_MAP_GFLP_STARTOFF|BTSDK_MAP_GFLP_LISTSIZE;
	param.start_off = 0;
	param.max_count = 0xFFFF;

	Btsdk_MAPSetFolder(hconn, NULL);
	Btsdk_MAPSetFolder(hconn, TEXT("telecom"));
	Btsdk_MAPSetFolder(hconn, TEXT("msg"));	

	printf("Enter the file name (e.g. c:\\gl.vcf) to store the folder Listing Object:\n>>");
	strPath = malloc(BTSDK_PATH_MAXLENGTH);
	scanf("%s", strPath);

	hFile = MAP_CreateFile(strPath);
	result = Btsdk_MAPGetFolderList(hconn, &param, hFile);
	printf("param.list_size is %d\n", param.list_size);
	if (result == BTSDK_OK)
	{
		printf("The folder Listing Object retrieved is saved to %s\n", strPath);
		memset(s_currstrPath, 0, BTSDK_PATH_MAXLENGTH);
		strcpy(s_currstrPath, strPath);		
	}
	else
	{
		printf("0X%04X\n",result);
	}
	free(strPath);
	MAP_CloseFile(hFile);
}

void TestSetFolder()
{
	BTUINT32 result;
	BTCONNHDL hconn = s_currMAPConnHdl;
	BTUINT8 *strFoler = NULL;

	if (hconn == BTSDK_INVALID_HANDLE)
	{
		printf("No MAP connection selected!\n");
		return;
	}

	printf("Enter the folder to change to\n");
	strFoler = malloc(BTSDK_PATH_MAXLENGTH);
	scanf("%s", strFoler);
	result = Btsdk_MAPSetFolder(hconn, strFoler); 

	if (result == BTSDK_OK)
	{
		printf("Set Path succeed.\n");
	}
	else
	{
		printf("%d\n",result);
	}
	free(strFoler);
}

void TestGetMessageList()
{
	BTSDKHANDLE hFile;
	int count = 0;
	int i;
	BTUINT32 result = BTSDK_OK;
	BTSDKHANDLE hdlEnumFolder;
	BTUINT8 *strPath = NULL;
	BtSdkMAPFileIORoutinesStru fileio_rtns = {0};
	BtSdkMAPGetMsgListParamStru param = {0};
	CHAR pszSubFolderName[10][BTSDK_MAP_FOLDER_LEN] = {0};
	BTUINT8 strflist[MAX_PATH] = {0};
	BTCONNHDL hconn = s_currMAPConnHdl;

	fileio_rtns.open_file = (Btsdk_OpenFile_Func)MAP_OpenFile;
	fileio_rtns.create_file = (Btsdk_CreateFile_Func)MAP_CreateFile;
	fileio_rtns.write_file = (Btsdk_WriteFile_Func)MAP_WriteFile;
	fileio_rtns.read_file = (Btsdk_ReadFile_Func)MAP_ReadFile;
	fileio_rtns.get_file_size = (Btsdk_GetFileSize_Func)MAP_GetFilesize;
	fileio_rtns.rewind_file = (Btsdk_RewindFile_Func)MAP_Rewind;
	fileio_rtns.close_file = (Btsdk_CloseFile_Func)MAP_CloseFile;
	Btsdk_MAPRegisterFileIORoutines(hconn, &fileio_rtns);
	Btsdk_MAPRegisterStatusCallback(hconn, MAPAppStatusCB);

	param.mask = BTSDK_MAP_GMLP_MAXCOUNT|BTSDK_MAP_GMLP_STARTOFF|BTSDK_MAP_GMLP_PARAMMASK;
	param.max_count = 0xFFFF;
	param.start_off = 0;
	param.param_mask = 0;

	if (*s_currstrPath == 0)
	{
		printf("No folder list is available, please get folder list from MSE first. \n");
		return;
	}

	hFile = MAP_OpenFile(s_currstrPath);
	if (hFile == BTSDK_INVALID_HANDLE)
	{
		printf("Opening folder list file fail, please check the file. \n");
		return;
	}
	hdlEnumFolder = Btsdk_MAPStartEnumFolderList(fileio_rtns.read_file, fileio_rtns.rewind_file, hFile);
	if (hdlEnumFolder != BTSDK_INVALID_HANDLE)
	{
		BtSdkMAPFolderObjStru struFolder = {0};
		while (Btsdk_MAPEnumFolderList(hdlEnumFolder, &struFolder))
		{
			strcpy(pszSubFolderName[count], (CHAR*)struFolder.name);
			count++;
		}
		Btsdk_MAPEndEnumFolderList(hdlEnumFolder);
	}
	else
	{
		printf("There is no folder in the current folder_list file. \n");
		MAP_CloseFile(hFile);
		hFile = BTSDK_INVALID_HANDLE;
		return;
	}
	MAP_CloseFile(hFile);
	hFile = BTSDK_INVALID_HANDLE;

	for(i=0; i<count; i++)
	{
		printf("%d.	%s\n", i, pszSubFolderName[i]);
	}
	printf("please input the folder name you want to list\n");
	strPath= malloc(BTSDK_PATH_MAXLENGTH);
	scanf("%s", strPath);

	Btsdk_MAPSetFolder(hconn, NULL);
	Btsdk_MAPSetFolder(hconn, TEXT("telecom"));
	Btsdk_MAPSetFolder(hconn, TEXT("msg"));	
	result = Btsdk_MAPSetFolder(hconn, strPath);
	if (result == BTSDK_OK)
	{
		printf("you are in %s folder now\n", strPath);
	}
	else
	{
		printf("wrong input, error code is %d\n",result);
	}

	printf("please input a file name you wantta use to store (e.g. c:\\gl.vcf)\n");
	memset(strPath, 0, BTSDK_PATH_MAXLENGTH);
	scanf("%s", strPath);
	
	hFile = MAP_CreateFile(strPath);
	result = Btsdk_MAPGetMessageList(hconn, &param, hFile);
	if (result == BTSDK_OK)
	{
		printf("The message Listing Object retrieved is saved to %s\n", strPath);
		memset(s_currstrFile, 0, BTSDK_PATH_MAXLENGTH);
		strcpy(s_currstrFile, strPath);
	}
	else
	{
		printf("0X%04X\n",result);
	}
	free(strPath);
	MAP_CloseFile(hFile);
	hFile = BTSDK_INVALID_HANDLE;
}

void TestGetMessage()
{
	BTSDKHANDLE hFile;
	BTSDKHANDLE hdlEnumMsg;
	BTUINT8 *strPath = NULL;
	BTUINT32 result = BTSDK_OK;
	int msgNum = 1;
	BTUINT8 *msgPath = NULL;
	BTCONNHDL hconn = s_currMAPConnHdl;
	BTSDKHANDLE hdlMessage = BTSDK_INVALID_HANDLE;
	BtSdkMAPFileIORoutinesStru fileio_rtns = {0};
	BtSdkMAPGetMsgParamStru msgparam = {0};

	if (*s_currstrFile == 0)
	{
		printf("No message list file available, please get message list first. \n");
		return;
	}

	fileio_rtns.open_file = (Btsdk_OpenFile_Func)MAP_OpenFile;
	fileio_rtns.create_file = (Btsdk_CreateFile_Func)MAP_CreateFile;
	fileio_rtns.write_file = (Btsdk_WriteFile_Func)MAP_WriteFile;
	fileio_rtns.read_file = (Btsdk_ReadFile_Func)MAP_ReadFile;
	fileio_rtns.get_file_size = (Btsdk_GetFileSize_Func)MAP_GetFilesize;
	fileio_rtns.rewind_file = (Btsdk_RewindFile_Func)MAP_Rewind;
	fileio_rtns.close_file = (Btsdk_CloseFile_Func)MAP_CloseFile;
	Btsdk_MAPRegisterFileIORoutines(hconn, &fileio_rtns);
	Btsdk_MAPRegisterStatusCallback(hconn, MAPAppStatusCB); 
	
	hFile = MAP_OpenFile(s_currstrFile);
	if (hFile == BTSDK_INVALID_HANDLE)
	{
		printf("Open message list file fail, please check the file. \n");
		return;
	}
	hdlEnumMsg = Btsdk_MAPStartEnumMessageList(fileio_rtns.read_file, fileio_rtns.rewind_file, hFile);
	if (hdlEnumMsg != BTSDK_INVALID_HANDLE)
	{
		BtSdkMAPMsgObjStru struMsg = {0};

		printf("please input a folder name to store message (e.g. c:\\gl.vcf)\n");
		strPath = (char*)malloc(sizeof(char)*BTSDK_PATH_MAXLENGTH);
		memset(strPath, 0, BTSDK_PATH_MAXLENGTH);
		scanf("%s", strPath);
		CreateDirectory(strPath, NULL);

		msgPath = (char*)malloc(sizeof(char)*BTSDK_PATH_MAXLENGTH);
		memset(msgPath, 0, BTSDK_PATH_MAXLENGTH);
		while (Btsdk_MAPEnumMessageList(hdlEnumMsg, &struMsg))
		{
			msgparam.charset = BTSDK_MAP_CHARSET_UTF8;
			msgparam.attachment = BTSDK_TRUE;
			memcpy(msgparam.msg_handle, struMsg.msg_handle, BTSDK_MAP_MSGHDL_LEN);
	
			sprintf(msgPath, "%s\\%d", strPath, msgNum);
			hdlMessage = MAP_CreateFile(msgPath);
			result = Btsdk_MAPGetMessage(hconn, &msgparam, hdlMessage);
			if(result != BTSDK_OK)
			{
				MAP_CloseFile(hdlMessage);
				break;
			}
			result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle, BTSDK_MAP_MSG_SETST_READ);
			if(result == BTSDK_OK)
			{
				printf("a message has been set to READ\n");
			}
			MAP_CloseFile(hdlMessage);
			msgNum++;
		}
		Btsdk_MAPEndEnumMessageList(hdlEnumMsg);
		s_totalMsg = msgNum;
//		MAP_CloseFile(hdlMessage);
		hdlMessage = BTSDK_INVALID_HANDLE;
	}
	MAP_CloseFile(hFile);
	hFile = BTSDK_INVALID_HANDLE;

	if(result == BTSDK_OK)
	{
		printf("messages have been stored under %s\n", strPath);
	}
	else
	{
		printf("sth wrong happened, error code is %d\n", result);
	}

	free(strPath);
}

void ShowStatusMenu()
{
	printf("\n");
	printf("***********************************\n");
	printf("*         status  Menu            *\n");
	printf("* <1> set status to read          *\n");
	printf("* <2> set status to unread        *\n");
	printf("* <3> set status to deleted       *\n");
	printf("* <4> set status to undeleted     *\n");
	printf("***********************************\n");
	printf(">");
}

void TestSetMessageStatus()
{
	BTUINT8 ch;
	BTSDKHANDLE hFile;
	BTUINT32 result = BTSDK_OK;
	BTSDKHANDLE hdlEnumMsg;
	BTCONNHDL hconn = s_currMAPConnHdl;
	BtSdkMAPFileIORoutinesStru fileio_rtns = {0};
	BtSdkMAPGetMsgParamStru msgparam = {0};

	printf("the function will set messages status\n");
	if (*s_currstrFile == 0)
	{
		printf("No message list file available, please get message list first. \n");
		return;
	}

	fileio_rtns.open_file = (Btsdk_OpenFile_Func)MAP_OpenFile;
	fileio_rtns.create_file = (Btsdk_CreateFile_Func)MAP_CreateFile;
	fileio_rtns.write_file = (Btsdk_WriteFile_Func)MAP_WriteFile;
	fileio_rtns.read_file = (Btsdk_ReadFile_Func)MAP_ReadFile;
	fileio_rtns.get_file_size = (Btsdk_GetFileSize_Func)MAP_GetFilesize;
	fileio_rtns.rewind_file = (Btsdk_RewindFile_Func)MAP_Rewind;
	fileio_rtns.close_file = (Btsdk_CloseFile_Func)MAP_CloseFile;
	Btsdk_MAPRegisterFileIORoutines(hconn, &fileio_rtns);
	Btsdk_MAPRegisterStatusCallback(hconn, MAPAppStatusCB);

	hFile = MAP_OpenFile(s_currstrFile);
	if (hFile == BTSDK_INVALID_HANDLE)
	{
		printf("Open message list file fail, please check the file. \n");
		return;
	}
	hdlEnumMsg = Btsdk_MAPStartEnumMessageList(fileio_rtns.read_file, fileio_rtns.rewind_file, hFile);
	if (hdlEnumMsg != BTSDK_INVALID_HANDLE)
	{
		BtSdkMAPMsgObjStru struMsg = {0};
		ShowStatusMenu();
		printf("please select a status you want to set\n");
		scanf(" %c", &ch);
		getchar();
		while (Btsdk_MAPEnumMessageList(hdlEnumMsg, &struMsg))
		{
			msgparam.charset = BTSDK_MAP_CHARSET_NATIVE;
			msgparam.attachment = BTSDK_TRUE;
			memcpy(msgparam.msg_handle, struMsg.msg_handle, BTSDK_MAP_MSGHDL_LEN);
			
			if(ch == '1')
			{
				result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle, BTSDK_MAP_MSG_SETST_READ);
			}
			else if(ch == '2')
			{
				result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle, BTSDK_MAP_MSG_SETST_UNREAD);
			}
			else if(ch == '3')
			{
				result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle, BTSDK_MAP_MSG_SETST_DELETED);
			}
			else if(ch == '4')
			{
				result = Btsdk_MAPSetMessageStatus(hconn, msgparam.msg_handle, BTSDK_MAP_MSG_SETST_UNDELETED);
			}

			if(result != BTSDK_OK)
			{
				printf("0X%04X\n",result);
				break;
			}
			else if (ch == '1')
			{
				printf("a message has been set to READ status\n");
			}
			else if (ch == '2')
			{
				printf("a message has been set to UNREAD status\n");
			}
			else if (ch == '3')
			{
				printf("a message has been set to DELETED status\n");
			}
			else if (ch == '4')
			{
				printf("a message has been set to UNDELETED status\n");
			}			
		}
	}
	Btsdk_MAPEndEnumMessageList(hdlEnumMsg);
	MAP_CloseFile(hFile);
}

void TestPushMessage()
{
	FILE* fp;
	BTSDKHANDLE hFile;
	BTUINT8 *strPath = NULL;
	BTUINT8 *phoneNum = NULL;
	BTUINT8 *phoneContent = NULL;
	BTUINT32 result = BTSDK_OK;
	char* VObj = NULL;
	CHAR* pszDraft = TEXT("draft");
	CHAR* pszOutbox = TEXT("outbox");
	BTCONNHDL hconn = s_currMAPConnHdl;
	BtSdkMAPPushMsgParamStru param = {0};
	BtSdkMAPFileIORoutinesStru fileio_rtns = {0};

	Btsdk_MAPSetFolder(hconn, NULL);
	Btsdk_MAPSetFolder(hconn, TEXT("telecom"));
	Btsdk_MAPSetFolder(hconn, TEXT("msg"));

	param.retry = BTSDK_TRUE;
	param.save_copy = BTSDK_TRUE;
	memcpy(param.folder, pszOutbox, strlen(pszOutbox));

	printf("please input a message path\n");
	strPath = malloc(BTSDK_PATH_MAXLENGTH);
	scanf("%s", strPath);


	hFile = MAP_OpenFile(strPath);
	if(hFile != BTSDK_INVALID_HANDLE)
	{
		VObj = MAP_file_open(strPath);
		if(VObj != NULL)
		{
			printf("please input a number you want to send\n");
			phoneNum = malloc(BTSDK_PATH_MAXLENGTH);
			scanf("%s", phoneNum);
			VObj = MAP_set_value(VObj, "TEL:", phoneNum);

			printf("please input content you want to send\n");
			phoneContent = malloc(BTSDK_PATH_MAXLENGTH);
			scanf("%s", phoneContent);
			VObj = MAP_set_value(VObj, "BEGIN:MSG\r\n", phoneContent);
			
			fp = fopen("forsend.xml", "wb+");
			fwrite(VObj, 1, strlen(VObj), fp);
			fclose(fp);
		}
	}
	MAP_CloseFile(hFile);
	hFile = BTSDK_INVALID_HANDLE;

	fileio_rtns.open_file = (Btsdk_OpenFile_Func)MAP_OpenFile;
	fileio_rtns.create_file = (Btsdk_CreateFile_Func)MAP_CreateFile;
	fileio_rtns.write_file = (Btsdk_WriteFile_Func)MAP_WriteFile;
	fileio_rtns.read_file = (Btsdk_ReadFile_Func)MAP_ReadFile;
	fileio_rtns.get_file_size = (Btsdk_GetFileSize_Func)MAP_GetFilesize;
	fileio_rtns.rewind_file = (Btsdk_RewindFile_Func)MAP_Rewind;
	fileio_rtns.close_file = (Btsdk_CloseFile_Func)MAP_CloseFile;
	Btsdk_MAPRegisterFileIORoutines(hconn, &fileio_rtns);
	Btsdk_MAPRegisterStatusCallback(hconn, MAPAppStatusCB);

	hFile = MAP_OpenFile("forsend.xml");
	if(hFile != BTSDK_INVALID_HANDLE)
	{
		result = Btsdk_MAPPushMessage(hconn, &param, hFile);
	}
	
	if(result == BTSDK_OK)
	{
		printf("setMessageStatus succeed\n");
	}
	else
	{
		printf("error code is 0X%04X\n",result);
 	}
	MAP_CloseFile(hFile);
	free(strPath);
}

void TestUpdateInbox()
{
	BTUINT32 result = BTSDK_OK;
	BTCONNHDL hconn = s_currMAPConnHdl;	

	result = Btsdk_MAPUpdateInbox(hconn);
	if(result == BTSDK_OK)
	{
		printf("update inbox succeed\n");
	}
	else
	{
		printf("sth wrong happened, error code is %d\n", result);
	}
}

int SelectMessage(void)
{
	int i;
	BTUINT8 s[10];
	printf("Select the message ID:\n");
	printf("(Press key 'q' to return to the function list.)\n");
	do
	{
		printf("Message ID = ");
		scanf("%s", s);
		if ((s[0] == 'q') || (s[0] == 'Q'))
		{
			printf("\nUser abort the operation.\n");
			return 0;
		}
		i = atoi(s);
		if ((i < 1) || (i > s_msg_num))
		{
			printf("%d is not a valid ID, please select again.\n", i);
			continue;
		}
		else
		{
			break;
		}
	} while (1);
	return i;
}

void TestDeleteMessage(void)
{
	int iMsgID, i;
	BTCONNHDL hconn = s_currMAPConnHdl;
	BTSDKHANDLE hFile;
	BTSDKHANDLE hdlEnumMsg;	
	PAppMsgObjStru pobj;
	AppMsgObjStru s_msg_arr[MAX_MSG_NUM];
	BtSdkMAPFileIORoutinesStru fileio_rtns = {0};
	BTUINT8 *read[2] = {"Unread", "Read"};

	/*test begin*/
	if (*s_currstrFile == 0)
	{
		printf("No message list file available, please get message list first. \n");
		return;
	}

	memset(s_msg_arr, 0, MAX_MSG_NUM*sizeof(AppMsgObjStru));
	fileio_rtns.write_file = MAP_WriteFile;
	fileio_rtns.read_file = MAP_ReadFile;
	fileio_rtns.rewind_file = MAP_Rewind;
	fileio_rtns.get_file_size = MAP_GetFilesize;
	Btsdk_MAPRegisterFileIORoutines(hconn, &fileio_rtns);
	Btsdk_MAPRegisterStatusCallback(hconn, MAPAppStatusCB);	

	// get messages frome the message list

	hFile = MAP_OpenFile(s_currstrFile);
	if (hFile == BTSDK_INVALID_HANDLE)
	{
		printf("Open message list file fail, please check the file. \n");
		return;
	}
	hdlEnumMsg = Btsdk_MAPStartEnumMessageList(fileio_rtns.read_file, fileio_rtns.rewind_file, hFile);
	i = 0;
	if (hdlEnumMsg != BTSDK_INVALID_HANDLE)
	{
		BtSdkMAPMsgObjStru obj = {0};

		while ((Btsdk_MAPEnumMessageList(hdlEnumMsg, &obj) != NULL)
				&& (i < MAX_MSG_NUM))
		{
			strcpy(s_msg_arr[i].msg_handle, obj.msg_handle);
			strcpy(s_msg_arr[i].msg_type, obj.msg_type);
			memcpy(s_msg_arr[i].subject, obj.subject, SUBJECT_LEN);
			s_msg_arr[i].subject[SUBJECT_LEN - 1] = 0;
			memcpy(s_msg_arr[i].sender_name, obj.sender_name, USER_NAME_LEN);
			s_msg_arr[i].sender_name[USER_NAME_LEN - 1] = 0;
			strcpy(s_msg_arr[i].date_time, obj.date_time);
			s_msg_arr[i].read_st = (obj.read == BTSDK_TRUE ? 1 : 0);
			i++;
		}
		Btsdk_MAPEndEnumMessageList(hdlEnumMsg);
	}
	s_msg_num = i;
	MAP_CloseFile(hFile);
	hFile = BTSDK_INVALID_HANDLE;
	
	// Show the messages	
	if (s_msg_num != 0)
	{
		printf("--------------------------- Message List -----------------------------\n");
		printf("ID  Handle            Status  Type    Subject   Sender          Time\n");
		printf("----------------------------------------------------------------------\n");
		for (i = 0; i < s_msg_num; i++)
		{
			pobj = &s_msg_arr[i];
			printf("%-2d  %-18hs%-8hs%-8hs%-10hs%-16hs%-18hs\n", i + 1, pobj->msg_handle, read[pobj->read_st], pobj->msg_type, pobj->subject, pobj->sender_name, pobj->date_time);
		}
		printf("----------------------------------------------------------------\n");
	}
	// Select a message to delete
	iMsgID = SelectMessage();
	if (iMsgID != 0)
	{
		iMsgID--;
		s_msg_arr[iMsgID].read_st = BTSDK_FALSE;
		Btsdk_MAPSetMessageStatus(hconn, s_msg_arr[iMsgID].msg_handle, BTSDK_MAP_MSG_SETST_DELETED);
	}
}

void SetNotificationOn()
{
	BTINT32 result = BTSDK_OK;
	result = Btsdk_MAPSetNotificationRegistration(s_currMAPConnHdl, BTSDK_TRUE);
	if (result == BTSDK_OK)
	{
		printf("set notification on succeed\n");
	}
	else
	{
		printf("error code is 0X%04X\n",result);
	}
}

void SetNotificationOff()
{
	BTINT32 result = BTSDK_OK;
	result = Btsdk_MAPSetNotificationRegistration(s_currMAPConnHdl, BTSDK_FALSE);
	if (result == BTSDK_OK)
	{
		printf("set notification off succeed\n");
	}
	else
	{
		printf("error code is 0X%04X\n",result);
	}
}

void MSELaunchServer()
{
	MSERegisterMASService();
}


void MCEExecCmd(BTUINT8 choice)
{
	switch (choice) {
	case '1':
		TestGetFolderList();
		break;
	case '2':
		TestSetFolder();
		break;
	case '3':
		TestGetMessageList();
		break;
	case '4':
		TestGetMessage();
		break;
	case '5':
		TestSetMessageStatus();
		break;
	case '6':
		TestPushMessage();
		break;
	case '7':
		TestUpdateInbox();
		break;
	case '8':
		TestDeleteMessage();
		break;
	case '9':
		SetNotificationOn();
		break;
	case 'a':
		SetNotificationOff();
		break;
	case 'b':
		if (BTSDK_INVALID_HANDLE != s_currMAPConnHdl)
		{
			Btsdk_Disconnect(s_currMAPConnHdl);
			s_currMAPConnHdl = BTSDK_INVALID_HANDLE;
		}
		break;
	case 'r':
		break;
	default:
		printf("Invalid command.\n");
		break;
	}
}

void MAPTestShowMenu()
{
	printf("\n");
	printf("***********************************\n");
	printf("*         MAP  Testing Menu       *\n");
	printf("* <1> Register MNS Service        *\n");
	printf("* <2> MCE Functions               *\n");
	printf("* <3> MSE Functions               *\n");
	printf("* <r> Return to upper menu        *\n");
	printf("***********************************\n");
	printf(">");
}

void MAPExecCmd(BTUINT8 choice)
{
	BTUINT8 ch = 0;	
	
	if (choice == '1')
	{
		RegisterMNSService();
	}
	else if (choice == '2')
	{
		TestSelectRmtMAPDev();
		TestSelectMAPSvc();
		TestConnectMAPSvc();
		if (BTSDK_INVALID_HANDLE == s_currMAPConnHdl)
		{
			printf("Establish MAP connection unsuccessfully.\n");
			printf("Please make sure the expected device's MAP service is connectable.\n");
			return;
		}
		MCETestShowMenu();
		while (ch != 'r')
		{
			scanf(" %c", &ch);
			getchar();		
			if (ch == '\n')
			{
				printf(">>");
			}
			else if('r' == ch)
			{
				break;
			}
			else
			{
				MCEExecCmd(ch);
				printf("\n");
				MCETestShowMenu();
			}
		}
		if (BTSDK_INVALID_HANDLE != s_currMAPConnHdl)
		{
			Btsdk_Disconnect(s_currMAPConnHdl);
			s_currMAPConnHdl = BTSDK_INVALID_HANDLE;
		}
	}
	else if (choice == '3')
	{
		MSELaunchServer();
	}
	else
	{
		printf("Invalid command.\n");
	}
}

void TestMAPFunc(void)
{
	BTUINT8 ch = 0;
	
	MAPTestShowMenu();	
	while (ch != 'r')
	{
		scanf(" %c", &ch);
		getchar();		
		if (ch == '\n')
		{
			printf(">>");
		}
		else if('r' == ch)
		{
			break;
		}
		else
		{
			MAPExecCmd(ch);
			printf("\n");
			MAPTestShowMenu();
		}
	}
}