/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-20089 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    pbap_tst.c
Abstract:
    this module is to test Phone Book Access profiles relative functionality. 
Revision History:
	2009-04-15   Chu Enlai  Created

---------------------------------------------------------------------------*/
#include "shtypes.h"
#include "ShlObj.h"
#include "sdk_tst.h"
#include "profiles_tst.h"
#include "vcard_parser\vobject.h"
#include "Btsdk_Stru.h"

/*PSE root directory*/
//const BTUINT8 g_root_pse[] = "D:\\PSE";
static BTUINT8 g_root_pse[MAX_PATH] = {0};
/* current remote device handle */
static BTDEVHDL s_currRmtPbapDevHdl = BTSDK_INVALID_HANDLE;
/* current remote device's PBAP service handle */
static BTSVCHDL s_currPbapSvcHdl = BTSDK_INVALID_HANDLE;
/* current remote device's HF service handle */
static BTCONNHDL s_currPbapConnHdl = BTSDK_INVALID_HANDLE;
/*PSE current directory*/
static BTUINT8 g_curdir_pse[MAX_PATH] = {0};

const BTUINT8 g_strRoot[] = "root";
const BTUINT8 g_strTelecom[] = "telecom";
const BTUINT8 g_strPB[] = "pb";
const BTUINT8 g_strDelimiter[] = "\\";

/*
	Implement the mandatory callback functions to deal with 
	file I/O. They are usually platform dependent.
*/

char* file_open(const char* filename);
PMB_ContactItemEx analyseVcardEntry(char* pbuffer);

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function searches a directory for a file of any type other 
	than subdirectory.
Arguments:
	const BTUINT8* path
	BTUINT8* name
Return:
	BTSDKHANDLE 
---------------------------------------------------------------------------*/
BTSDKHANDLE APP_FindFirstFile(const BTUINT8* path, BTUINT8* name)
{
	BTUINT8 *full_path;
	WIN32_FIND_DATA data_info;	 
	HANDLE hdl;

	full_path = malloc(strlen(path) + strlen("\\*.*") + 1);
	strcpy(full_path, path);
	if (path[strlen(path) - 1] != '\\')
	{
		strcat(full_path, "\\*.*");
	}
	else
	{
		strcat(full_path, "*.*");
	}
	if ((hdl = FindFirstFile(full_path, &data_info)) != INVALID_HANDLE_VALUE) 
	{
		while (!strcmp(data_info.cFileName, ".") ||
			   !strcmp(data_info.cFileName, "..") ||
			   strlen(data_info.cFileName) > BTSDK_PATH_MAXLENGTH) 
		{
			if (!FindNextFile(hdl, &data_info))
			{
				FindClose(hdl);
				hdl = (HANDLE)NULL;
				break;
			}
		}
		strcpy(name, data_info.cFileName);
	}
	free(full_path);
	return (BTSDKHANDLE)hdl;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to continue a file search from a previous call to the 
	APP_FindFirstFile function.
Arguments:
	BTSDKHANDLE hdl
	BTUINT8* name
Return:
	int 
---------------------------------------------------------------------------*/
int APP_FindNextFile(BTSDKHANDLE hdl, BTUINT8* name)
{
	WIN32_FIND_DATA data_info;	
	int r = -1;

	if (FindNextFile((HANDLE)hdl,&data_info))
	{
		strcpy(name, data_info.cFileName);
		r = 0;
	}
	return r;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to close the specified search handle. 
Arguments:
	BTSDKHANDLE hdl
Return:
	void  
---------------------------------------------------------------------------*/
void  APP_FindFileClose(BTSDKHANDLE hdl)
{
	FindClose((HANDLE)hdl);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to change the current working directory.
Arguments:
	const char* path
Return:
	int 
---------------------------------------------------------------------------*/
int APP_ChangeDir(const char* path)
{
	return (_chdir(path));
}

int PSE_ChangeDir(const char* path)
{
	strcpy(g_curdir_pse, path);
	return (_chdir(path));
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to create a new directory.
Arguments:
	const char* path
Return:
	int 
---------------------------------------------------------------------------*/
int APP_CreateDir(const char* path)
{
	return (_mkdir(path));
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to open an existing file for reading.
Arguments:
	const BTUINT8* file_name
Return:
	BTSDKHANDLE 
---------------------------------------------------------------------------*/
BTSDKHANDLE APP_OpenFile(const BTUINT8* file_name)
{
	FILE *filehdl = fopen(file_name, "rb");
	if (filehdl != NULL)
	{
		return (BTSDKHANDLE)filehdl;
	}
}

void PSE_GetFullPath(const BTUINT8* file_name, BTUINT8 strpath[MAX_PATH])
{
	ZeroMemory(strpath, MAX_PATH);
	strcpy(strpath, g_curdir_pse);

	if (strpath[strlen(strpath) - 1] != '\\')
	{
		strcat(strpath, "\\");
	}
	strcat(strpath, file_name);
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to open an existing file for reading, the file is in the
	current directory of PSE.
Arguments:
	const BTUINT8* file_name
Return:
	BTSDKHANDLE 
---------------------------------------------------------------------------*/
BTSDKHANDLE PSE_OpenFile(const BTUINT8* file_name)
{
	FILE *filehdl = NULL;
	BTUINT8 strfullpath[MAX_PATH] = {0};

	PSE_GetFullPath(file_name, strfullpath);
	filehdl = fopen(strfullpath, "rb");
	if (filehdl != NULL)
	{
		return (BTSDKHANDLE)filehdl;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to open an empty file for both reading and writing. 
	If the given file exists, its contents are destroyed.
	If file_name is a NULL pointer, creates a temporary file.
Arguments:
	const BTUINT8* file_name
Return:
	BTSDKHANDLE 
---------------------------------------------------------------------------*/
BTSDKHANDLE APP_CreateFile(const BTUINT8* file_name)
{
	return (BTSDKHANDLE)(file_name!=NULL ? fopen(file_name,"w+b") : tmpfile());
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is close a file stream.
Arguments:
	BTSDKHANDLE file_hdl
Return:
	void 
---------------------------------------------------------------------------*/
void APP_CloseFile(BTSDKHANDLE file_hdl)
{
	if (file_hdl != BTSDK_INVALID_HANDLE) 
	{
		fclose((FILE*)file_hdl);
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to write data to a file stream.
Arguments:
	BTSDKHANDLE file_hdl
	BTUINT8* buf
	BTUINT32 bytes_to_write
Return:
	BTUINT32 
---------------------------------------------------------------------------*/
BTUINT32 APP_WriteFile(BTSDKHANDLE file_hdl, BTUINT8* buf, BTUINT32 bytes_to_write)
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

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is read data from a file stream.
Arguments:
	BTSDKHANDLE file_hdl
	BTUINT8* buf
	BTUINT32 len
Return:
	BTUINT32 
---------------------------------------------------------------------------*/
BTUINT32 APP_ReadFile(BTSDKHANDLE file_hdl, BTUINT8* buf, BTUINT32 len, BTBOOL *is_end)
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

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to get the size of a file.
Arguments:
	BTSDKHANDLE file_hdl
Return:
	BTUINT32 
---------------------------------------------------------------------------*/
BTUINT32 APP_GetFilesize(BTSDKHANDLE file_hdl)
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

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is move the file pointer to a specified location that is 
	offset bytes from the beginning of the file.
Arguments:
	BTSDKHANDLE file_hdl
	BTUINT32 offset
Return:
	int 
---------------------------------------------------------------------------*/
int APP_Rewind(BTSDKHANDLE file_hdl, BTUINT32 offset)
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

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is return the number of missed calls that have not been 
	checked on the PSE at the point of this function is called.
Arguments:
	BTUINT8 *path
Return:
	int 
---------------------------------------------------------------------------*/
int APP_GetNewMissedCalls(BTUINT8 *path)
{
	BTUINT8 *full_path;
	WIN32_FIND_DATA data_info;	 
	HANDLE hdl;
	BTINT32 num = 0;

	full_path = malloc(strlen(path) + strlen("\\*.vcf") + 1);
	strcpy(full_path, path);
	if (path[strlen(path) - 1] != '\\')
	{
		strcat(full_path, "\\*.vcf");
	}
	else
	{
		strcat(full_path, "*.vcf");
	}
	if ((hdl = FindFirstFile(full_path, &data_info)) != INVALID_HANDLE_VALUE) 
	{
		while (!FindNextFile(hdl, &data_info))
		{
			num++;
		}
		FindClose(hdl);
	}

	if (full_path)
	{
		free(full_path);
	}
	return num;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select expected remote PBAP device according to device class. 
Arguments:
Return:
	None. 
---------------------------------------------------------------------------*/
void TestSelectRmtPBAPDev()
{
	s_currRmtPbapDevHdl = SelectRemoteDevice(0);
	if (BTSDK_INVALID_HANDLE == s_currRmtPbapDevHdl)
	{
		printf("Please make sure that the expected device is in discoverable state and search again.\n");
	}	
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to get PBAP service handle according to given device handle. 
Arguments:
Return:
	None. 
---------------------------------------------------------------------------*/
void TestSelectPBAPSvc()
{
	s_currPbapSvcHdl = SelectRemoteService(s_currRmtPbapDevHdl);
	if (BTSDK_INVALID_HANDLE == s_currPbapSvcHdl)
	{
		printf("Cann't get expected service handle.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to connect specified device's PBAP service with it's service handle.
Arguments:
Return:
	None. 
---------------------------------------------------------------------------*/
void TestConnectPBAPSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;
	ulRet = Btsdk_Connect(s_currPbapSvcHdl, 0, &s_currPbapConnHdl);
	if (BTSDK_OK != ulRet)
	{
		printf("Please make sure that the expected device is powered on and connectable.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to get PSE's attributes.
Arguments:
	BTCONNHDL conn_hdl
	PBtSdkRmtPSESvcAttrStru pattr
Return:
	None.
---------------------------------------------------------------------------*/
void GetRmtPSEAttribute(BTCONNHDL conn_hdl, PBtSdkRmtPSESvcAttrStru pattr)
{
	BtSdkConnectionPropertyStru conn_prop;
	pattr->repositories = BTSDK_PBAP_REPO_LOCAL;
	if (Btsdk_GetConnectionProperty(conn_hdl, &conn_prop) == BTSDK_OK)
	{
		BtSdkRemoteServiceAttrStru svc_attr;
		svc_attr.mask = BTSDK_RSAM_EXTATTRIBUTES;
		if (Btsdk_GetRemoteServiceAttributes(conn_prop.service_handle, &svc_attr) == BTSDK_OK)
		{
			memcpy(pattr, svc_attr.ext_attributes, sizeof(BtSdkRmtPSESvcAttrStru));
			Btsdk_FreeMemory(svc_attr.ext_attributes);
		}
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to register PSE service.
Arguments:
Return:
	None. 
---------------------------------------------------------------------------*/
void RegisterPbapService(void)
{
	BTSVCHDL svc_hdl = BTSDK_INVALID_HANDLE;
	BtSdkLocalPSEServerAttrStru svr_info = {0};
	BtSdkPBAPSvrCBStru st_pbapsvr_cb = {0};
	
	svr_info.size = sizeof(BtSdkLocalPSEServerAttrStru);
	strcpy(g_root_pse,  "D:\\PSE");
	strcpy(svr_info.root_dir, g_root_pse);
	strcpy(svr_info.path_delimiter, "\\");
	svr_info.repositories = BTSDK_PBAP_REPO_LOCAL;
	
	/*those functions come from versit project.*/
	st_pbapsvr_cb.cardparser_rtns.parse_open = (Btsdk_vCardParser_Open_Func)vCardParse;
	st_pbapsvr_cb.cardparser_rtns.get_prop = (Btsdk_vCardParser_GetProperty_Func)GetProperty;
	st_pbapsvr_cb.cardparser_rtns.parse_close = (Btsdk_vCardParser_Close_Func)vCardClose;
	st_pbapsvr_cb.cardparser_rtns.get_prop = (Btsdk_vCardParser_GetProperty_Func)GetProperty;
	st_pbapsvr_cb.cardparser_rtns.parse_free = (Btsdk_vCardParser_FreeProperty_Func)FreevCardBuff;
	st_pbapsvr_cb.cardparser_rtns.parse_findfirst = (Btsdk_vCardParser_FindFirstProperty_Func)FindFirstProperty;
	st_pbapsvr_cb.cardparser_rtns.parse_findnext = (Btsdk_vCardParser_FindNextProperty_Func)FindNextProperty;
	st_pbapsvr_cb.cardparser_rtns.parse_findclose = (Btsdk_vCardParser_FindPropertyClose_Func)EndFindProperty;
	
	/*those functions are usually platform dependent.*/
	st_pbapsvr_cb.findfile_rtns.find_first =  (Btsdk_FindFirstFile_Func)APP_FindFirstFile;
	st_pbapsvr_cb.findfile_rtns.find_next = (Btsdk_FindNextFile_Func)APP_FindNextFile;
	st_pbapsvr_cb.findfile_rtns.find_close = (Btsdk_FindFileClose_Func)APP_FindFileClose;
	
	st_pbapsvr_cb.fileio_rtns.close_file = (Btsdk_CloseFile_Func)APP_CloseFile;
	st_pbapsvr_cb.fileio_rtns.create_file = (Btsdk_CreateFile_Func)APP_CreateFile;
	st_pbapsvr_cb.fileio_rtns.get_file_size = (Btsdk_GetFileSize_Func)APP_GetFilesize;
	st_pbapsvr_cb.fileio_rtns.open_file = (Btsdk_OpenFile_Func)APP_OpenFile;
	st_pbapsvr_cb.fileio_rtns.read_file = (Btsdk_ReadFile_Func)APP_ReadFile;
	st_pbapsvr_cb.fileio_rtns.rewind_file = (Btsdk_RewindFile_Func)APP_Rewind;
	st_pbapsvr_cb.fileio_rtns.write_file = (Btsdk_WriteFile_Func)APP_WriteFile;
	
	st_pbapsvr_cb.dirctrl_rtns.change_dir = (Btsdk_ChangDir_Func)APP_ChangeDir;
	st_pbapsvr_cb.dirctrl_rtns.create_dir = (Btsdk_CreateDir_Func)APP_CreateDir;
		  
	st_pbapsvr_cb.get_new_missedcalls = (Btsdk_PBAP_GetMissedCalls_Func)APP_GetNewMissedCalls;
		   
	svc_hdl = Btsdk_RegisterPBAPService("Phone Book Access Server", &svr_info, &st_pbapsvr_cb);
	if (svc_hdl != BTSDK_INVALID_HANDLE)
	{
	  printf("Register pbap service successfully!\n");
	}
	else
	{
	  printf("Register pbap service Failed!\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is to register PSE service.
Arguments:
Return:
None. 
---------------------------------------------------------------------------*/
void PSERegisterPbapService(void)
{
	BTSVCHDL svc_hdl = BTSDK_INVALID_HANDLE;
	BtSdkLocalPSEServerAttrStru svr_info = {0};
	BtSdkPBAPSvrCBStru st_pbapsvr_cb = {0};
	
	BOOL bRet = PSEGetDefaultRootDir(g_root_pse);
	svr_info.size = sizeof(BtSdkLocalPSEServerAttrStru);
	strcpy(svr_info.root_dir, g_root_pse);
	strcpy(svr_info.path_delimiter, "\\");
	svr_info.repositories = BTSDK_PBAP_REPO_LOCAL;
	
	/*those functions come from versit project.*/
	st_pbapsvr_cb.cardparser_rtns.parse_open = (Btsdk_vCardParser_Open_Func)vCardParse;
	st_pbapsvr_cb.cardparser_rtns.get_prop = (Btsdk_vCardParser_GetProperty_Func)GetProperty;
	st_pbapsvr_cb.cardparser_rtns.parse_close = (Btsdk_vCardParser_Close_Func)vCardClose;
	st_pbapsvr_cb.cardparser_rtns.get_prop = (Btsdk_vCardParser_GetProperty_Func)GetProperty;
	st_pbapsvr_cb.cardparser_rtns.parse_free = (Btsdk_vCardParser_FreeProperty_Func)FreevCardBuff;
	st_pbapsvr_cb.cardparser_rtns.parse_findfirst = (Btsdk_vCardParser_FindFirstProperty_Func)FindFirstProperty;
	st_pbapsvr_cb.cardparser_rtns.parse_findnext = (Btsdk_vCardParser_FindNextProperty_Func)FindNextProperty;
	st_pbapsvr_cb.cardparser_rtns.parse_findclose = (Btsdk_vCardParser_FindPropertyClose_Func)EndFindProperty;
	
	/*those functions are usually platform dependent.*/
	/****************************Begin***********************************/
	st_pbapsvr_cb.findfile_rtns.find_first =  (Btsdk_FindFirstFile_Func)APP_FindFirstFile;
	st_pbapsvr_cb.findfile_rtns.find_next = (Btsdk_FindNextFile_Func)APP_FindNextFile;
	st_pbapsvr_cb.findfile_rtns.find_close = (Btsdk_FindFileClose_Func)APP_FindFileClose;
	
	st_pbapsvr_cb.fileio_rtns.close_file = (Btsdk_CloseFile_Func)APP_CloseFile;
 	st_pbapsvr_cb.fileio_rtns.create_file = (Btsdk_CreateFile_Func)APP_CreateFile;
	st_pbapsvr_cb.fileio_rtns.get_file_size = (Btsdk_GetFileSize_Func)APP_GetFilesize;
	st_pbapsvr_cb.fileio_rtns.open_file = (Btsdk_OpenFile_Func)PSE_OpenFile;
	st_pbapsvr_cb.fileio_rtns.read_file = (Btsdk_ReadFile_Func)APP_ReadFile;
	st_pbapsvr_cb.fileio_rtns.rewind_file = (Btsdk_RewindFile_Func)APP_Rewind;
	st_pbapsvr_cb.fileio_rtns.write_file = (Btsdk_WriteFile_Func)APP_WriteFile;
	
	st_pbapsvr_cb.dirctrl_rtns.change_dir = (Btsdk_ChangDir_Func)PSE_ChangeDir;
	st_pbapsvr_cb.dirctrl_rtns.create_dir = (Btsdk_CreateDir_Func)APP_CreateDir;
		  
	st_pbapsvr_cb.get_new_missedcalls = (Btsdk_PBAP_GetMissedCalls_Func)APP_GetNewMissedCalls;
	/******************************End*********************************/

	svc_hdl = Btsdk_RegisterPBAPService("Phone Book Access Server", &svr_info, &st_pbapsvr_cb);
	if (svc_hdl != BTSDK_INVALID_HANDLE)
	{
		printf("Register pbap service successfully!\n");
	}
	else
	{
		printf("Register pbap service Failed!\n");
	}
}

BOOL PSEGetDefaultRootDir(BTUINT8* pRootPse)
{
	LPITEMIDLIST pIDL = NULL;
	TCHAR szMyDocumentPath[MAX_PATH]={0};
	SHGetSpecialFolderLocation(NULL, CSIDL_PERSONAL, &pIDL);
	if(pIDL != NULL)
	{
		SHGetPathFromIDList(pIDL, szMyDocumentPath);
	}
	strcpy(pRootPse, szMyDocumentPath);
	strcat(pRootPse, "\\Bluetooth");
	_mkdir(pRootPse);
	strcat(pRootPse, "\\PBAP\\");
	_mkdir(pRootPse);
	strcat(pRootPse, g_strRoot);
	return (_mkdir(pRootPse));
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to set the work folder.
Arguments:
Return:
	None. 
---------------------------------------------------------------------------*/
void TestSetPhonebookPath(void)
{
	BTUINT32 result;
	BTCONNHDL hconn = s_currPbapConnHdl;
	BTUINT8 *strFoler = NULL;
	
	if (hconn == BTSDK_INVALID_HANDLE)
	{
		printf("No PBAP connection selected!\n");
		return;
	}
	printf("Enter the folder to change to,\n");
	printf("(.. represents parent;  \\ or / represents root;  otherwise, it is a sub-folder):\n>>");
	strFoler = malloc(BTSDK_PATH_MAXLENGTH);
	scanf("%s", strFoler);

	result = Btsdk_PBAPSetPath(hconn, strFoler); //input ".." back up to previous folder

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

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is a callback function to show transferred file information. 
Arguments:
	BTUINT8 first
	BTUINT8 last
	BTUINT8* filename
	BTUINT32 filesize
	BTUINT32 cursize
Return:
	None. 
---------------------------------------------------------------------------*/
void PbapAppStatusCB(BTUINT8 first, BTUINT8 last, BTUINT8* filename, BTUINT32 filesize, BTUINT32 cursize)
{
	static BTUINT32 s_cur_size = 0;
	static BTUINT32 s_total_size = 0;

	if (!last)
	{
		if (first)
		{
			s_total_size = filesize;
			printf("\nstart transferring %s:\n", filename);
			printf("%10d byte / %d transferred", s_cur_size, s_total_size);
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


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to get phone book from PSE.
Arguments:
Return:
	None. 
---------------------------------------------------------------------------*/
void TestGetPhoneBook(void)
{
	BTSDKHANDLE hFile;
	BTUINT8 pb[32];
	BtSdkPBAPParamStru param;
    
	BTUINT8 *strPath = NULL;
	BTUINT32 result = BTSDK_OK;
	BtSdkRmtPSESvcAttrStru pse_attr;
	BTCONNHDL hconn = s_currPbapConnHdl;
	BTUINT8 ch = 0;
	BtSdkPBAPFileIORoutinesStru fileio_rtns = {0};
	memset(&param, 0, sizeof(BtSdkPBAPParamStru));

	if (hconn == BTSDK_INVALID_HANDLE)
	{
		printf("No PBAP connection selected!\n");
		return;
	}
	
	fileio_rtns.write_file = (Btsdk_WriteFile_Func)APP_WriteFile;
	Btsdk_PBAPRegisterFileIORoutines(hconn, &fileio_rtns);
	Btsdk_PBAPRegisterStatusCallback(hconn, PbapAppStatusCB);
	
	param.mask = BTSDK_PBAP_PM_MAXCOUNT;
    param.max_count = 0xFFFF;//如果只是想得到个数，这个参数设置成0

	GetRmtPSEAttribute(hconn, &pse_attr);
	if (pse_attr.repositories == BTSDK_PBAP_REPO_LOCAL)
	{
		strcpy(pb, "telecom/");
	}
	else if (pse_attr.repositories == BTSDK_PBAP_REPO_SIM)
	{
		strcpy(pb, "SIM1/telecom/");
	}
	else
	{
		printf("Select the repositories:\n");
		printf("1) Local memory\n");
		printf("2) SIM card\n");
		printf(">>");
		scanf("%c", &ch);
		getchar();
		if (ch == '1')
		{
			strcpy(pb, "telecom/");
		}
		else if (ch == '2')
		{
			strcpy(pb, "SIM1/telecom/");
		}
		else
		{
			printf("Invalid input, cancel the operation.\n");
			return;
		}
	}

	printf("Select the phone book:\n");
	printf("1) Main Phone book\n");
	printf("2) Incoming Calls History\n");
	printf("3) Outgoing Calls History\n");
	printf("4) Missed Calls History\n");
	printf("5) Combined Calls History\n");
	printf(">>");
	scanf("%c", &ch);
	switch (ch)
	{
	case '1':
		strcat(pb, "pb.vcf");
		break;
	case '2':
		strcat(pb, "ich.vcf");
		break;
	case '3':
		strcat(pb, "och.vcf");
		break;
	case '4':
		strcat(pb, "mch.vcf");
		break;
	case '5':
		strcat(pb, "cch.vcf");
		break;
	default:
		printf("Invalid input, cancel the operation.\n");
		return;
	}
	if (ch != '1')
	{
		Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_X_IRMC_CALL_DATETIME);
	}
	printf("Enter the file name (e.g. c:\\gpb.vcf) to store the phone book:\n>>");
	strPath = malloc(BTSDK_PATH_MAXLENGTH);
	scanf("%s", strPath);

	hFile = APP_CreateFile(strPath);

	result = Btsdk_PBAPPullPhoneBook(hconn, pb, &param, hFile);

	if (result == BTSDK_OK)
	{
		printf("The phone book retrieved is saved to %s\n", strPath);
	}
	else
	{
		printf("0X%04X\n",result);
	}
	free(strPath);
	APP_CloseFile(hFile);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to get vCard list.
Arguments:
Return:
	None.
---------------------------------------------------------------------------*/
void TestGetvCardListing(void)
{
	BTSDKHANDLE hFile;
	BTUINT8 pb[4];
	BTUINT8 *ppb = pb;
	BTUINT8 *strPath = NULL;
	BTUINT32 result = BTSDK_OK;
	BTUINT8 ch = 0;
	BtSdkPBAPParamStru param = {0};
	BTCONNHDL hconn = s_currPbapConnHdl;
	BtSdkPBAPFileIORoutinesStru fileio_rtns = {0};
	
	if (hconn == BTSDK_INVALID_HANDLE)
	{
		printf("No PBAP connection selected!\n");
		return;
	}

	fileio_rtns.write_file = (Btsdk_WriteFile_Func)APP_WriteFile;
	Btsdk_PBAPRegisterFileIORoutines(hconn, &fileio_rtns);
	Btsdk_PBAPRegisterStatusCallback(hconn, PbapAppStatusCB);
	
	param.mask = BTSDK_PBAP_PM_LISTOFFSET|BTSDK_PBAP_PM_MAXCOUNT|BTSDK_PBAP_PM_ORDER;
	param.list_offset = 0;
	param.max_count = 0xFFFF;
	param.order = BTSDK_PBAP_ORDER_NAME;
	
	printf("Select the phone book:\n");
	printf("1) Main Phone book\n");
	printf("2) Incoming Calls History\n");
	printf("3) Outgoing Calls History\n");
	printf("4) Missed Calls History\n");
	printf("5) Combined Calls History\n");
	printf(">>");
	scanf("%c", &ch);
	switch (ch)
	{
	case '1':
		strcpy(pb, "pb");
		break;
	case '2':
		strcpy(pb, "ich");
		break;
	case '3':
		strcpy(pb, "och");
		break;
	case '4':
		strcpy(pb, "mch");
		break;
	case '5':
		strcpy(pb, "cch");
		break;
	default:
		ppb = NULL;
		break;
	}
	
	printf("Enter the file name (e.g. c:\\gl.vcf) to store the vCard Listing Object:\n>>");
	strPath = malloc(BTSDK_PATH_MAXLENGTH);
	scanf("%s", strPath);
	
	hFile = APP_CreateFile(strPath);

	result = Btsdk_PBAPPullCardList(hconn, ppb, &param, hFile);
	if (result == BTSDK_OK)
	{
		printf("The vCard Listing Object retrieved is saved to %s\n", strPath);
	}
	else
	{
		printf("0X%04X\n",result);
	}
	free(strPath);
	APP_CloseFile(hFile);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to get vCard object entry.
Arguments:
Return:
	None.
---------------------------------------------------------------------------*/
void TestGetvCardEntry(void)
{
	char* pbuffer = NULL;
	PMB_ContactItemEx vcardContactItem;

	BTSDKHANDLE hFile;
	BTUINT8 *strCard = NULL;
	BTUINT8 *strPath = NULL;
	BTUINT32 result = BTSDK_OK;
	BtSdkPBAPParamStru param = {0};
	BtSdkPBAPFileIORoutinesStru fileio_rtns = {0};
	
	BTCONNHDL hconn = s_currPbapConnHdl;
	if (hconn == BTSDK_INVALID_HANDLE)
	{
		printf("No PBAP connection selected!\n");
		return;
	}

	fileio_rtns.write_file = (Btsdk_WriteFile_Func)APP_WriteFile;
	Btsdk_PBAPRegisterFileIORoutines(hconn, &fileio_rtns);
	Btsdk_PBAPRegisterStatusCallback(hconn, PbapAppStatusCB);
	
	param.mask = BTSDK_PBAP_PM_FILTER|BTSDK_PBAP_PM_FORMAT;
	param.format = BTSDK_PBAP_FMT_VCARD30;
	Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_VERSION);
	Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_FN);
	Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_N);
	Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_TEL);
	Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_SOUND);
	Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_ORG);
	Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_TITLE);
	Btsdk_PBAPFilterComposer(param.filter, BTSDK_PBAP_FILTER_EMAIL);

	printf("Enter the name of the vCard entry (e.g. 1.vcf) to be retrieved:\n");
	strCard = malloc(BTSDK_PATH_MAXLENGTH);
	scanf("%s", strCard);
	
	printf("Enter the file name (e.g. c:\\gce.vcf) to store the vCard entry:\n");
	strPath = malloc(BTSDK_PATH_MAXLENGTH);
	scanf("%s", strPath);
	
	hFile = APP_CreateFile(strPath);
	if (hFile == BTSDK_INVALID_HANDLE)
	{
		printf("Could not create file %s \n", strPath);
		if (strCard != NULL)
		{
			free(strCard);
		}
		return;
	}

	result = Btsdk_PBAPPullCardEntry(hconn, strCard, &param, hFile);

	if (result == BTSDK_OK)
	{
		printf("The vCard entry retrieved is saved to %s\n", strPath);
	}
	else
	{
		printf("0X%04X\n", result);
	}
	free(strCard);
	APP_CloseFile(hFile);

/*******add vcard analysis***********/
	if (*strPath != 0)
	{
		pbuffer = file_open(strPath);
		if (*pbuffer != 0)
		{
			vcardContactItem = analyseVcardEntry(pbuffer);
			free(pbuffer);
		}
		free(strPath);
	}	
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show PBAP user interface.
Arguments:
Return:
	None 
---------------------------------------------------------------------------*/
void PBAPTestShowMenu()
{
	printf("\n");
	printf("***********************************\n");
	printf("*         PBAP Testing Menu       *\n");
	printf("* <1> Register PSE Service        *\n");
	printf("* <2> PCE Functions               *\n");
	printf("* <3> PSE Functions               *\n");
	printf("* <r> Return to upper menu        *\n");
	printf("***********************************\n");
	printf(">");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show PCE menu.
Arguments:
Return:
	None 
---------------------------------------------------------------------------*/
void PCETestShowMenu()
{
	printf("\n");
	printf("***********************************\n");
	printf("*          PCE Testing Menu       *\n");
	printf("* <1> Set Phone Book Path in PSE  *\n");
	printf("* <2> Pull Phone Book from PSE    *\n");
	printf("* <3> Pull vCard Listing from PSE *\n");
	printf("* <4> Pull vCard Entry from PSE   *\n");
	printf("* <5> Disconnect from PSE         *\n");
	printf("* <r> Return to upper menu        *\n");
	printf("***********************************\n");
	printf(">");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution function of PCE.
Arguments:
	BTUINT8 choice
Return:
	None. 
---------------------------------------------------------------------------*/
void PCEExecCmd(BTUINT8 choice)
{
	switch (choice) {
	case '1':
		TestSetPhonebookPath();
		break;
	case '2':
		TestGetPhoneBook();
		break;
	case '3':
		TestGetvCardListing();
		break;
	case '4':
		TestGetvCardEntry();
		break;
	case '5':
		if (BTSDK_INVALID_HANDLE != s_currPbapConnHdl)
		{
			Btsdk_Disconnect(s_currPbapConnHdl);
			s_currPbapConnHdl = BTSDK_INVALID_HANDLE;
		}
		break;
	case 'r':
		break;
	default:
		printf("Invalid command.\n");
		break;
	}
}

BOOL PSELaunchServer()
{
	PSERegisterPbapService();
	return TRUE;
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution function of PBAP sample.
Arguments:
	BTUINT8 choice
Return:
	None. 
---------------------------------------------------------------------------*/
void PbapExecCmd(BTUINT8 choice)
{
	BTUINT8 ch = 0;	

	if (choice == '1')
	{
		RegisterPbapService();
	}
	else if (choice == '2')
	{
		TestSelectRmtPBAPDev();
		TestSelectPBAPSvc();
		TestConnectPBAPSvc();
		if (BTSDK_INVALID_HANDLE == s_currPbapConnHdl)
		{
			printf("Establish PBAP connection unsuccessfully.\n");
			printf("Please make sure the expected device's PABP service is connectable.\n");
			return;
		}
		PCETestShowMenu();
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
				PCEExecCmd(ch);
				printf("\n");
				PCETestShowMenu();
			}
		}
		if (BTSDK_INVALID_HANDLE != s_currPbapConnHdl)
		{
			Btsdk_Disconnect(s_currPbapConnHdl);
			s_currPbapConnHdl = BTSDK_INVALID_HANDLE;
		}
	}
	else if (choice == '3')
	{
		PSELaunchServer();
	}
	else
	{
		printf("Invalid command.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the entry function of PBAP sample.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestPBAPFunc(void)
{
	BTUINT8 ch = 0;
	
	PBAPTestShowMenu();	
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
			PbapExecCmd(ch);
			printf("\n");
			PBAPTestShowMenu();
		}
	}
}

char* file_open(const char* filename)
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

char* getSubString(const char* pbuffer, char* keyword, int* number)
{
	char* beginLocation;
	char* endLocation;
	char* forRet = NULL;
	beginLocation = strstr(pbuffer, keyword);
	if(beginLocation == NULL)
	{
		return NULL;
	}
	while(*(beginLocation-1) != '\n')
	{
		beginLocation = beginLocation + strlen(keyword);
	}

	beginLocation = beginLocation + strlen(keyword);
	endLocation = strstr(beginLocation, "\r\n");
	if (beginLocation == endLocation)
	{
		printf("return NULL\n");
		return NULL;
	}
	forRet = (char*)malloc(sizeof(char)*(endLocation-beginLocation+1));
	memset(forRet, 0, sizeof(char)*(endLocation-beginLocation+1));
	strncpy(forRet, beginLocation, endLocation-beginLocation);
	if (number != NULL)
	{
		*number = endLocation - pbuffer;
	}
	
	return forRet;
}

void setVcardName(PMB_ContactNameItem vCardNameItem, const char* content)
{
	WCHAR wstrName[MAX_PATH] = {0};
	char strName[MAX_CONTACT_NAME_LENGTH] = {0};
	char* beginLocation;
	char* endLocation;
	vCardNameItem->dwSize = sizeof(MB_ContactNameItem);

	beginLocation = content;
	endLocation = strstr(beginLocation, ";");
	if (endLocation != NULL)
	{
		strncpy(vCardNameItem->szContactLastName, beginLocation, endLocation-beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szContactLastName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("Last Name is %s\n", strName);
	} 
	else
	{
		strcpy(vCardNameItem->szContactLastName, beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szContactLastName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("Last Name is %s\n", strName);
		return;
	}	
	
	ZeroMemory(wstrName, MAX_PATH * sizeof(WCHAR));
	ZeroMemory(strName, MAX_CONTACT_NAME_LENGTH * sizeof(char));
	beginLocation = endLocation + 1;
	endLocation= strstr(beginLocation, ";");
	if (endLocation != NULL)
	{
		strncpy(vCardNameItem->szContactFirstName, beginLocation, endLocation-beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szContactFirstName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("First Name is %s\n", strName);
	} 
	else
	{
		strcpy(vCardNameItem->szContactFirstName, beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szContactFirstName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("First Name is %s\n", strName);
		return;
	}

	ZeroMemory(wstrName, MAX_PATH * sizeof(WCHAR));
	ZeroMemory(strName, MAX_CONTACT_NAME_LENGTH * sizeof(char));
	beginLocation = endLocation + 1;
	endLocation= strstr(beginLocation, ";");
	if (endLocation != NULL)
	{
		strncpy(vCardNameItem->szMiddleName, beginLocation, endLocation-beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szMiddleName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("Middle Name is %s\n", strName);
	} 
	else
	{
		strcpy(vCardNameItem->szMiddleName, beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szMiddleName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("Middle Name is %s\n", strName);
		return;
	}

	ZeroMemory(wstrName, MAX_PATH * sizeof(WCHAR));
	ZeroMemory(strName, MAX_CONTACT_NAME_LENGTH * sizeof(char));
	beginLocation = endLocation + 1;
	endLocation= strstr(beginLocation, ";");
	if (endLocation != NULL)
	{
		strncpy(vCardNameItem->szContactPrefixName, beginLocation, endLocation-beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szContactPrefixName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("Prefix Name is %s\n", strName);
	} 
	else
	{
		strcpy(vCardNameItem->szContactPrefixName, beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szContactPrefixName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("Prefix Name is %s\n", strName);
		return;
	}

	ZeroMemory(wstrName, MAX_PATH * sizeof(WCHAR));
	ZeroMemory(strName, MAX_CONTACT_NAME_LENGTH * sizeof(char));
	beginLocation = endLocation + 1;
	endLocation= strstr(beginLocation, ";");
	if (endLocation != NULL)
	{
		strncpy(vCardNameItem->szContactNickName, beginLocation, endLocation-beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szContactNickName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("Nick Name is %s\n", strName);
	} 
	else
	{
		strcpy(vCardNameItem->szContactNickName, beginLocation);
		MultiByteToWideChar(CP_UTF8, 0, vCardNameItem->szContactNickName, -1, wstrName, MAX_PATH);
		WideCharToMultiByte(CP_ACP, 0, wstrName, -1, strName, MAX_CONTACT_NAME_LENGTH, NULL, NULL);
		printf("Nick Name is %s\n", strName);
		return;
	}

}

void setVcardTeleNum(PMB_ContactTelephoneItem telephoneItem, char* content)
{
	char* beginLocation;
	char* endLocation;
	char teleNumType[20];
	memset(teleNumType, 0, sizeof(teleNumType));
	telephoneItem->dwSize = sizeof(MB_ContactTelephoneItem);

	beginLocation = content;
	beginLocation = strstr(beginLocation, "TYPE=");
	if (beginLocation == NULL)
	{
		return;
	} 
	else
	{
		beginLocation = beginLocation + strlen("TYPE=");
		endLocation = strstr(beginLocation, ":");
		if (endLocation != NULL)
		{
			strncpy(teleNumType, beginLocation, endLocation - beginLocation);
		} 
		else
		{
			strcpy(teleNumType, beginLocation);
		}
		
		printf("%s number : ", teleNumType);
	}

	beginLocation = content;
	beginLocation = strstr(beginLocation, ":");
	if (beginLocation != NULL)
	{
		beginLocation = beginLocation + strlen(":");
		strcpy(telephoneItem->szTelephone, beginLocation);
		printf("%s\n", telephoneItem->szTelephone);
	}
}

PMB_ContactItemEx analyseVcardEntry(char* pbuffer)
{
	char* subString;
	char* tempLoc;
	int count = 0;
	int i;
	int offNum;
	const char* pszVBegin = "BEGIN:VCARD";
	const char* pszVEnd = "END:VCARD";
	PMB_ContactItemEx vcardContactItem;
	PMB_ContactNameItem vCardNameItem;
	vcardContactItem = (PMB_ContactItemEx)malloc(sizeof(MB_ContactItemEx));
	vCardNameItem = (PMB_ContactNameItem)malloc(sizeof(MB_ContactNameItem));
	memset(vcardContactItem, 0, sizeof(MB_ContactItemEx));
	memset(vCardNameItem, 0, sizeof(MB_ContactNameItem));

	subString = getSubString(pbuffer, "N:", NULL);
	setVcardName(vCardNameItem, subString);
	vcardContactItem->itemName = vCardNameItem;
	free(subString);
	
	tempLoc = pbuffer;
	offNum = 0;
	while (NULL != (subString = getSubString(tempLoc, "TEL;", &offNum)))
	{
		count++;
		free(subString);
		if(offNum != 0)
		{
			tempLoc = tempLoc + offNum;
		}
		else
		{
			break;
		}
	}
	vcardContactItem->arrayCountTelephone = count;

 	count = 0;
	tempLoc = pbuffer;
	offNum = 0;
	vcardContactItem->arrayContactTelephone = (PMB_ContactTelephoneItem)malloc(sizeof(MB_ContactTelephoneItem)*(int)(vcardContactItem->arrayCountTelephone));
	while (NULL != (subString = getSubString(tempLoc, "TEL;", &offNum)))
	{
		setVcardTeleNum(&vcardContactItem->arrayContactTelephone[count], subString);
		free(subString);
		subString = NULL;
		if(offNum != 0)
		{
			tempLoc = tempLoc + offNum;
		}
		else
		{
			break;
		}
		count++;
	}

	free(vCardNameItem);
	free(vcardContactItem->arrayContactTelephone);
	free(vcardContactItem);
	return NULL;
}