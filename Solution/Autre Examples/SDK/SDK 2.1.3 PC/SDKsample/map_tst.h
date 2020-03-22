#ifndef _MAP_TST_H
#define _MAP_TST_H

#include "Btsdk_Stru.h"

/*************** Macro Definition ******************/
/* Maximum length */
#define BTSDK_MAP_PATH_LEN				512
#define BTSDK_MAP_FOLDER_LEN			32
#define BTSDK_MAP_TIME_LEN				20
#define BTSDK_MAP_MSE_TIME_LEN			24
#define BTSDK_MAP_MSGHDL_LEN			20
#define BTSDK_MAP_MSGTYPE_LEN           16
#define BTSDK_MAP_SUBJECT_LEN			256
#define BTSDK_MAP_USERNAME_LEN			256
#define BTSDK_MAP_ADDR_LEN				256

/* Bit mask of SupportedMessagetypes - possible values of BtSdkRmtMASSvcAttrStru::sup_msg_type */
#define BTSDK_MAP_SUP_MSG_EMAIL			0x01
#define BTSDK_MAP_SUP_MSG_SMSGSM		0x02
#define BTSDK_MAP_SUP_MSG_SMSCDMA		0x04
#define BTSDK_MAP_SUP_MSG_MMS			0x08

/* Message types */
#define BTSDK_MAP_MSG_TYPE_EMAIL		0x01		
#define BTSDK_MAP_MSG_TYPE_SMSGSM		0x02
#define BTSDK_MAP_MSG_TYPE_SMSCDMA		0x03
#define BTSDK_MAP_MSG_TYPE_MMS			0x04
#define BTSDK_MAP_MSG_TYPE_APPEXT		0xFF 

/* Event report type - possible values of BtSdkEvReportObjStru::ev_type */
#define BTSDK_MAP_EVT_NEWMSG			0x01
#define BTSDK_MAP_EVT_DELIVERY_OK		0x02
#define BTSDK_MAP_EVT_SEND_OK			0x03
#define BTSDK_MAP_EVT_DELIVERY_FAIL		0x04
#define BTSDK_MAP_EVT_SEND_FAIL			0x05
#define BTSDK_MAP_EVT_MEM_FULL			0x06
#define BTSDK_MAP_EVT_MEM_READY			0x07
#define BTSDK_MAP_EVT_MSG_DELETED		0x08
#define BTSDK_MAP_EVT_MSG_SHIFT			0x09
#define BTSDK_MAP_EVT_APPEXT			0xFF

/* Bit mask - possible values of BtSdkMAPGetFolderListParamStru::mask */
#define BTSDK_MAP_GFLP_MAXCOUNT			0x0001
#define BTSDK_MAP_GFLP_STARTOFF			0x0002
#define BTSDK_MAP_GFLP_LISTSIZE			0x0004

/* Bit mask - possible values of BtSdkMAPGetMsgListParamStru::mask */
#define BTSDK_MAP_GMLP_MAXCOUNT			0x00000001
#define BTSDK_MAP_GMLP_STARTOFF			0x00000002
#define BTSDK_MAP_GMLP_MSGTYPE			0x00000004
#define BTSDK_MAP_GMLP_PERIODBEGIN		0x00000008
#define BTSDK_MAP_GMLP_PERIODEND		0x00000010
#define BTSDK_MAP_GMLP_READSTATUS		0x00000020
#define BTSDK_MAP_GMLP_RECIPIENT		0x00000040
#define BTSDK_MAP_GMLP_ORIGINATOR		0x00000080
#define BTSDK_MAP_GMLP_PRIORITY			0x00000100
#define BTSDK_MAP_GMLP_NEWMSG			0x00001000
#define BTSDK_MAP_GMLP_PARAMMASK		0x00008000
#define BTSDK_MAP_GMLP_LISTSIZE			0x00020000
#define BTSDK_MAP_GMLP_SUBJECTLENTH		0x00040000
#define BTSDK_MAP_GMLP_MSETIME			0x01000000

/* Bit mask - possible values of BtSdkMAPGetMsgListParamStru::param_mask, BtSdkMAPMsgObjStru::mask */
#define BTSDK_MAP_MP_SUBJECT			0x0001
#define BTSDK_MAP_MP_DATATIME			0x0002
#define BTSDK_MAP_MP_SENDERNAME			0x0004
#define BTSDK_MAP_MP_SENDERADDR			0x0008
#define BTSDK_MAP_MP_RECIPIENTNAME		0x0010
#define BTSDK_MAP_MP_RECIPIENTADDR		0x0020
#define BTSDK_MAP_MP_TYPE				0x0040
#define BTSDK_MAP_MP_SIZE				0x0080
#define BTSDK_MAP_MP_RECPSTATUS			0x0100
#define BTSDK_MAP_MP_TEXT				0x0200
#define BTSDK_MAP_MP_ATTACHSIZE			0x0400
#define BTSDK_MAP_MP_PRIORITY			0x0800
#define BTSDK_MAP_MP_READ				0x1000
#define BTSDK_MAP_MP_SENT				0x2000
#define BTSDK_MAP_MP_PROTECTED			0x4000
#define BTSDK_MAP_MP_REPLY2ADDR			0x8000

/* Bit mask - possible values of BtSdkMAPGetMsgListParamStru::filter_msg_type */
#define BTSDK_MAP_FILTEROUT_NO			0x00
#define BTSDK_MAP_FILTEROUT_SMSGSM		0x01
#define BTSDK_MAP_FILTEROUT_SMSCDMA		0x02
#define BTSDK_MAP_FILTEROUT_EMAIL		0x03
#define BTSDK_MAP_FILTEROUT_MMS			0x04

/* Read status filter - possible values of BtSdkMAPGetMsgListParamStru::filter_read_status */
#define BTSDK_MAP_MSG_FILTER_ST_ALL		0x00
#define BTSDK_MAP_MSG_FILTER_ST_UNREAD	0x01
#define BTSDK_MAP_MSG_FILTER_ST_READ	0x02

/* Priority filter - possible values of BtSdkMAPGetMsgListParamStru::filter_priority */
#define BTSDK_MAP_MSG_FILTER_PRI_ALL	0x00
#define BTSDK_MAP_MSG_FILTER_PRI_HIGH	0x01
#define BTSDK_MAP_MSG_FILTER_PRI_NOHIGH	0x02

/* Char-set requirement - possible values of BtSdkMAPGetMsgParamStru::charset, BtSdkMAPPushMsgParamStru::charset */
#define BTSDK_MAP_CHARSET_NATIVE		0x00
#define BTSDK_MAP_CHARSET_UTF8			0x01

/* Fraction requirement - possible values of BtSdkMAPGetMsgParamStru::fraction_req */
#define BTSDK_MAP_FRACT_NONE			0x00
#define BTSDK_MAP_FRACT_REQFIRST		0x01
#define BTSDK_MAP_FRACT_REQNEXT			0x02

/* Fraction indication - possible values of BtSdkMAPGetMsgParamStru::fraction_deliver */
#define BTSDK_MAP_FRACT_RSPMORE			0x00
#define BTSDK_MAP_FRACT_RSPLAST			0x01

/* Reception status - possible values of BtSdkMAPMsgObjStru::reception_status */
#define BTSDK_MAP_MSG_RCVST_COMPLETE		0x00
#define BTSDK_MAP_MSG_RCVST_FRACTION		0x01
#define BTSDK_MAP_MSG_RCVST_NOTIFICATION	0x02

/* Message status indicator value - possible values of Btsdk_MAPSetMessageStatus::status */
#define BTSDK_MAP_MSG_SETST_READ            0x02
#define BTSDK_MAP_MSG_SETST_UNREAD          0x00
#define BTSDK_MAP_MSG_SETST_DELETED         0x03
#define BTSDK_MAP_MSG_SETST_UNDELETED       0x01

/* Message status indicator - possible values of BtSdkMAPMsgStatusStru::status_indicator */
#define BTSDK_MAP_MSG_READ_STATUS           0x00
#define BTSDK_MAP_MSG_DELETE_STATUS         0x01

#define BT_FILE_HDL UINT8*

#define IDX_PROP_BMSG_VERSION	0x01
#define IDX_RPOP_BMSG_READSTATUS	0x02
#define IDX_RPOP_BMSG_TYPE		0x03
#define IDX_RPOP_BMSG_FOLDER		0x04
#define IDX_RPOP_BMSG_SENDER_VESION  0x05
#define IDX_RPOP_BMSG_SENDER_N		0x06
#define IDX_RPOP_BMSG_SENDER_FN		0x07
#define IDX_RPOP_BMSG_SENDER_TEL	0x08
#define IDX_RPOP_BMSG_SENDER_EMAIL	0x09
#define IDX_RPOP_BMSG_RECIPIENT_VESION	0x0A
#define IDX_RPOP_BMSG_RECIPIENT_N		0x0B
#define IDX_RPOP_BMSG_RECIPIENT_FN		0x0C
#define IDX_RPOP_BMSG_RECIPIENT_TEL		0x0D
#define IDX_RPOP_BMSG_RECIPIENT_EMAIL	0x0E

#define IDX_RPOP_BMSG_PART_ID		0x0F
#define IDX_RPOP_BMSG_BODY_ENCODING	 0x10
#define IDX_RPOP_BMSG_BODY_CHARSET		0x11
#define IDX_RPOP_BMSG_BODY_LANGUAGE  0x12
#define IDX_RPOP_BMSG_BODY_LENGTH	0x13
#define IDX_PROP_BMSG_BODY_CONTENT		0x14

typedef struct _BtSdkLocalMASServerAttrStru
{
	BTUINT32 size;
	BTUINT16 mask;
	BTUINT8	 root_dir[BTSDK_PATH_MAXLENGTH + 1];
	BTUINT8  path_delimiter[BTSDK_PBAP_MAX_DELIMITER + 1];
	BTUINT8  mas_inst_id;
	BTUINT8  sup_msg_types;
} BtSdkLocalMASServerAttrStru, *PBtSdkLocalMASServerAttrStru;

typedef struct _BtSdkRmtMASSvcAttrStru
{
	BTUINT32 	size;
	BTUINT16 	mask;
	BTUINT8  	mas_inst_id;
	BTUINT8	sup_msg_types;
} BtSdkRmtMASSvcAttrStru, *PBtSdkRmtMASSvcAttrStru;

typedef struct _BtSdkMAPEvReportObjStru
{
	BTUINT8		ev_type;
	BTUINT8		msg_type[BTSDK_MAP_MSGTYPE_LEN];
	BTUINT8		msg_handle[BTSDK_MAP_MSGHDL_LEN];
	BTUINT8		folder[BTSDK_MAP_PATH_LEN];
	BTUINT8		old_folder[BTSDK_MAP_PATH_LEN];
	BTUINT8		mas_inst_id;
} BtSdkMAPEvReportObjStru, *PBtSdkMAPEvReportObjStru;

typedef struct _BtSdkMAPFileIORoutinesStru
{
	Btsdk_OpenFile_Func    		open_file;
	Btsdk_CreateFile_Func  		create_file;
	Btsdk_WriteFile_Func   		write_file;
	Btsdk_ReadFile_Func    		read_file;
	Btsdk_GetFileSize_Func 	get_file_size;
	Btsdk_RewindFile_Func  	rewind_file;
	Btsdk_CloseFile_Func   		close_file;
} BtSdkMAPFileIORoutinesStru, *PBtSdkMAPFileIORoutinesStru;

typedef struct _BtSdkMAPGetFolderListParamStru {
	BTUINT16 	mask;
	BTUINT16 	max_count;
	BTUINT16 	start_off; 
	BTUINT16 	list_size;
} BtSdkMAPGetFolderListParamStru, *PBtSdkMAPGetFolderListParamStru;

typedef struct _BtSdkMAPGetMsgListParamStru {
	BTUINT32 	mask;
	BTUINT8	folder[BTSDK_MAP_FOLDER_LEN];
	BTUINT16 	max_count;
	BTUINT16 	start_off; 
	BTUINT32	param_mask;
	BTUINT8	filter_period_begin[BTSDK_MAP_TIME_LEN];
	BTUINT8	filter_period_end[BTSDK_MAP_TIME_LEN];
	BTUINT8	filter_originator[BTSDK_MAP_USERNAME_LEN];
	BTUINT8	filter_recipient[BTSDK_MAP_USERNAME_LEN];
	BTUINT8  	filter_msg_type;
	BTUINT8  	filter_read_status;
	BTUINT8  	filter_priority;
	BTUINT8  	subject_length;
	BTUINT16 	list_size;
	BTBOOL	new_msg;
	BTUINT8	mse_time[BTSDK_MAP_MSE_TIME_LEN];
} BtSdkMAPGetMsgListParamStru, *PBtSdkMAPGetMsgListParamStru;

typedef struct _BtSdkMAPGetMsgParamStru {
	BTUINT8 	msg_handle[BTSDK_MAP_MSGHDL_LEN];
	BTUINT8 	charset;
	BTBOOL	attachment;
	BTUINT8	fraction_req;
	BTUINT8	fraction_deliver;
} BtSdkMAPGetMsgParamStru, *PBtSdkMAPGetMsgParamStru;	

typedef struct _BtSdkMAPPushMsgParamStru {
	BTUINT8	folder[BTSDK_MAP_FOLDER_LEN];
	BTBOOL 	save_copy;
	BTBOOL	retry;
	BTUINT8	charset;
	BTUINT8 	msg_handle[BTSDK_MAP_MSGHDL_LEN];
} BtSdkMAPPushMsgParamStru, *PBtSdkMAPPushMsgParamStru;

typedef struct _BtSdkMAPFolderObjStru {
	BTUINT32 	size;
	BTUINT8 	name[BTSDK_MAP_FOLDER_LEN];
	BTUINT8 	create_time[BTSDK_MAP_TIME_LEN]; 
	BTUINT8 	access_time[BTSDK_MAP_TIME_LEN];
	BTUINT8 	modify_time[BTSDK_MAP_TIME_LEN];
} BtSdkMAPFolderObjStru, *PBtSdkMAPFolderObjStru;

typedef struct _BtSdkMAPMsgObjStru {
	BTUINT8 	msg_handle[BTSDK_MAP_MSGHDL_LEN];
	BTUINT32	mask;
	BTUINT32 	msg_size;
	BTUINT32 	attachment_size;
	BTUINT8 	subject[BTSDK_MAP_SUBJECT_LEN]; 
	BTUINT8 	sender_name[BTSDK_MAP_USERNAME_LEN];
	BTUINT8 	sender_addr[BTSDK_MAP_ADDR_LEN];
	BTUINT8 	replyto_addr[BTSDK_MAP_ADDR_LEN];
	BTUINT8 	recipient_name[BTSDK_MAP_USERNAME_LEN];
	BTUINT8 	recipient_addr[BTSDK_MAP_ADDR_LEN];
	BTUINT8 	msg_type[BTSDK_MAP_MSGTYPE_LEN];
	BTUINT8 	date_time[BTSDK_MAP_TIME_LEN];
	BTUINT8 	reception_status;
	BTBOOL 	text;
	BTBOOL	read;
	BTBOOL	sent;
	BTBOOL	protect;
	BTBOOL	priority;
} BtSdkMAPMsgObjStru, *PBtSdkMAPMsgObjStru;

typedef struct _BtSdkMAPMsgFilterStru {
	BTUINT32 	mask;
	BTUINT32	param_mask;
	BTUINT8		filter_period_begin[BTSDK_MAP_TIME_LEN];
	BTUINT8		filter_period_end[BTSDK_MAP_TIME_LEN];
	BTUINT8		filter_originator[BTSDK_MAP_USERNAME_LEN];
	BTUINT8		filter_recipient[BTSDK_MAP_USERNAME_LEN];
	BTUINT8  	filter_msg_type;        /* BTSDK_MAP_FILTEROUT_NO, etc.*/
	BTUINT8  	filter_read_status;     /* BTSDK_MAP_MSG_FILTER_ST_ALL, etc.*/
	BTUINT8  	filter_priority;        /* BTSDK_MAP_MSG_FILTER_PRI_ALL, etc.*/
	BTUINT8  	subject_length;
} BtSdkMAPMsgFilterStru, *PBtSdkMAPMsgFilterStru;

typedef struct _BtSdkMAPMsgStatusStru {
	BTUINT8 msg_handle[BTSDK_MAP_MSGHDL_LEN];
	BTUINT8 status_indicator;
	BTUINT8 status_value;
} BtSdkMAPMsgStatusStru, *PBtSdkMAPMsgStatusStru;

typedef struct _BtSdkMAPMSETimeStru {
    BTUINT8 mse_time[BTSDK_MAP_MSE_TIME_LEN];
} BtSdkMAPMSETimeStru, *PBtSdkMAPMSETimeStru;

typedef struct _BtSdkMAPMsgHandleStru {
    BTUINT8 msg_handle[BTSDK_MAP_MSGHDL_LEN];
} BtSdkMAPMsgHandleStru, *PBtSdkMAPMsgHandleStru;

/* Callback functions used to enumrate sub-folders under a directory. */
typedef BTSDKHANDLE (*Btsdk_MAP_FindFirstFolder_Func)(const BTUINT8 *path, PBtSdkMAPFolderObjStru pfd);
typedef BTBOOL (*Btsdk_MAP_FindNextFolder_Func)(BTSDKHANDLE find_hdl, PBtSdkMAPFolderObjStru pfd);
typedef BTBOOL (*Btsdk_MAP_FindFolderClose_Func)(BTSDKHANDLE find_hdl);

typedef struct _BtSdkMAPFindFolderRoutinesStru
{
	Btsdk_MAP_FindFirstFolder_Func find_first_folder;
	Btsdk_MAP_FindNextFolder_Func  find_next_folder;
	Btsdk_MAP_FindFolderClose_Func find_folder_close;
} BtSdkMAPFindFolderRoutinesStru, *PBtSdkMAPFindFolderRoutinesStru;

/* Callback functions used to enumrate messages under a directory. */
typedef BTSDKHANDLE (*Btsdk_MAP_FindFirstMsg_Func)(const BTUINT8 *path, PBtSdkMAPMsgFilterStru pfilter, PBtSdkMAPMsgObjStru pfd);
typedef BTBOOL  	(*Btsdk_MAP_FindNextMsg_Func)(BTSDKHANDLE hdl, PBtSdkMAPMsgObjStru pfd);
typedef BTBOOL	    (*Btsdk_MAP_FindMsgClose_Func)(BTSDKHANDLE hdl);

typedef struct _BtSdkMAPFindMsgRoutinesStru
{
	Btsdk_MAP_FindFirstMsg_Func find_first_msg;
	Btsdk_MAP_FindNextMsg_Func  find_next_msg;
	Btsdk_MAP_FindMsgClose_Func find_msg_close;
} BtSdkMAPFindMsgRoutinesStru, *PBtSdkMAPFindMsgRoutinesStru;

/* Callback functions used to operate message file. */
typedef BTBOOL (*Btsdk_ModifyMsgStatus_Func)(const BTUINT8 *cur_path, PBtSdkMAPMsgStatusStru msg_info);
typedef BTSDKHANDLE (*Btsdk_CreateBMsgFile_Func)(const BTUINT8 *cur_path, /*out*/BTUINT8 *msg_hdl);
typedef BTSDKHANDLE (*Btsdk_OpenBMsgFile_Func)(const BTUINT8 *cur_path, PBtSdkMAPGetMsgParamStru msg_info);
typedef BTBOOL (*Btsdk_PushMsg_Func)(const BTUINT8 *cur_path, PBtSdkMAPPushMsgParamStru msg_info);

typedef struct _BtSdkMAPMsgIORoutinesStru {
    Btsdk_ModifyMsgStatus_Func  modify_msg_status;
	Btsdk_CreateBMsgFile_Func  	create_bmsg_file;
	Btsdk_OpenBMsgFile_Func    	open_bmsg_file;
	Btsdk_PushMsg_Func          push_msg;
} BtSdkMAPMsgIORoutinesStru, *PBtSdkMAPMsgIORoutinesStru;

/* Callback functions used to get or change MSE server status. */
typedef BTBOOL (*Btsdk_RegisterNotification_Func)(BTCONNHDL mns_conn_hdl, BTBOOL turn_on);
typedef BTBOOL (*Btsdk_UnpdateInbox_Func)(void);
typedef BTBOOL (*Btsdk_GetMSETime_Func)(BTUINT8 *mse_time);
typedef struct _BtSdkMAPMSEStatusRoutinesStru {
    Btsdk_RegisterNotification_Func register_notification;
    Btsdk_UnpdateInbox_Func         update_inbox;
    Btsdk_GetMSETime_Func           get_mse_time;
} BtSdkMAPMSEStatusRoutinesStru, *PBtSdkMAPMSEStatusRoutinesStru;

typedef struct _BtSdkMASSvrCBStru
{
	BtSdkMAPFindFolderRoutinesStru  find_folder_rtns;
	BtSdkMAPFindMsgRoutinesStru     find_msg_rtns;
	BtSdkMAPFileIORoutinesStru	    file_io_rtns;
	BtSdkMAPMsgIORoutinesStru       msg_io_rtns;
	BtSdkMAPMSEStatusRoutinesStru   mse_status_rtns;
} BtSdkMASSvrCBStru, *PBtSdkMASSvrCBStru;

typedef void (Btsdk_MAP_STATUS_INFO_CB)(
										BTUINT8 first, 
										BTUINT8 last, 
										BTUINT8* filename, 
										BTUINT32 filesize, 
										BTUINT32 cursize);

typedef void (*Btsdk_MNS_MessageNotification_Func)(
													BTSVCHDL svc_hdl, 
													PBtSdkMAPEvReportObjStru ev_obj);

/************************************************************************/
/* MAP support                                                                     */
/************************************************************************/
BTSVCHDL Btsdk_RegisterMNSService(BTUINT8* svc_name, Btsdk_MNS_MessageNotification_Func st_func, PBtSdkMAPFileIORoutinesStru file_ios);
BTSVCHDL Btsdk_RegisterMASService(BTUINT8 *svc_name, PBtSdkLocalMASServerAttrStru svr_attr, PBtSdkMASSvrCBStru cb_funcs);
BTINT32 Btsdk_MAPRegisterFileIORoutines(BTCONNHDL conn_hdl, PBtSdkMAPFileIORoutinesStru cb_funcs);
BTINT32 Btsdk_MAPSetFolder(BTCONNHDL conn_hdl, BTUINT8 *folder);
BTINT32 Btsdk_MAPGetFolderList(BTCONNHDL conn_hdl, PBtSdkMAPGetFolderListParamStru param, BTSDKHANDLE file_hdl);
BTSDKHANDLE Btsdk_MAPStartEnumFolderList(Btsdk_ReadFile_Func func_read, Btsdk_RewindFile_Func func_rewind, BTSDKHANDLE file_hdl);
PBtSdkMAPFolderObjStru Btsdk_MAPEnumFolderList(BTSDKHANDLE enum_hdl, PBtSdkMAPFolderObjStru item);
void Btsdk_MAPEndEnumFolderList(BTSDKHANDLE enum_hdl);
BTINT32 Btsdk_MAPGetMessageList(BTCONNHDL conn_hdl, PBtSdkMAPGetMsgListParamStru param, BTSDKHANDLE file_hdl);
BTINT32 Btsdk_MAPGetMessage(BTCONNHDL conn_hdl, PBtSdkMAPGetMsgParamStru param, BTSDKHANDLE file_hdl);
BTINT32 Btsdk_MAPSetMessageStatus(BTCONNHDL conn_hdl, BTUINT8 *msg_hdl, BTUINT8 status);
BTINT32 Btsdk_MAPPushMessage(BTCONNHDL conn_hdl, PBtSdkMAPPushMsgParamStru param, BTSDKHANDLE file_hdl);
BTINT32 Btsdk_MAPUpdateInbox(BTCONNHDL conn_hdl);
BTINT32 Btsdk_MAPCancelTransfer(BTCONNHDL conn_hdl);
BTINT32 Btsdk_MAPSetNotificationRegistration(BTCONNHDL conn_hdl, BTBOOL turn_on);
BTSDKHANDLE Btsdk_MAPStartEnumMessageList(Btsdk_ReadFile_Func func_read, Btsdk_RewindFile_Func func_rewind, BTSDKHANDLE file_hdl);
PBtSdkMAPMsgObjStru Btsdk_MAPEnumMessageList(BTSDKHANDLE enum_hdl, PBtSdkMAPMsgObjStru item);
void Btsdk_MAPEndEnumMessageList(BTSDKHANDLE enum_hdl);

#endif