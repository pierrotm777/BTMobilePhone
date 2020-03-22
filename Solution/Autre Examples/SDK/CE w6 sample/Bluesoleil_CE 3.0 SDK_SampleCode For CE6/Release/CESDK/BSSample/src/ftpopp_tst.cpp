/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2008 IVT Corporation
*
* All rights reserved.
* 
---------------------------------------------------------------------------*/

/////////////////////////////////////////////////////////////////////////////
// Module Name:
//    
// Abstract:
//     
// Usage:
//     
// 
// Author:    
//     chenjinfeng
// Revision History:
//     2008-10-15		Created
// 
/////////////////////////////////////////////////////////////////////////////
#include "sdk_tst.h"


#define MAX_FTP_FOLDER_LIST 128
#define MAX_FILENAME    256

static BTINT32 g_CurrentFolderNum = 0;
static BTINT32 g_CurrentSelectNum = -1;
static BTUINT8 g_szRemoteCurrentDir[BTSDK_PATH_MAXLENGTH] = { 0 };

typedef struct tagFILEINFO
{
    UCHAR   Filename[BTSDK_PATH_MAXLENGTH];
	int	    nsize;
	int		ntype; 
} FILEINFO;

static FILEINFO rmtFileInfo[MAX_FTP_FOLDER_LIST];


/*******************************************************************
*																	*
********************************************************************/
void TransferStatusCallback(UCHAR first, UCHAR last, UCHAR* filename, DWORD filesize, DWORD cursize)
{
	static BTUINT32 s_cur_size = 0;
	static BTUINT32 s_total_size = 0;
	if (!last) {
		if (first) {
			s_total_size = filesize;
			PRINTMSG(1, "\nstart transferring %s:\n", filename);
			PRINTMSG(1, "%10d byte / %d transferred", s_cur_size, s_total_size);
		}
		else {
			s_cur_size += cursize;
			PRINTMSG(1, "\r%10d bytes", s_cur_size);
		}

	}
	else {
		s_cur_size += cursize;
		PRINTMSG(1, "\r%10d byte / %d transferred\n", s_cur_size, s_total_size);
		s_cur_size = 0;
		s_total_size = 0;
	}
}

/*******************************************************************
*																	*
********************************************************************/
BTBOOL AppOBEXServerReceiveFileInd(PBtSdkFileTransferReqStru pFileInfo)
{
	BTUINT8 strDevName[BTSDK_DEVNAME_LEN] = {0};

	Btsdk_GetRemoteDeviceName(pFileInfo->dev_hdl, strDevName, NULL);
	switch (pFileInfo->flag)
	{
	case BTSDK_ER_CONTINUE:
		switch (pFileInfo->operation)
		{
		case BTSDK_APP_EV_FTP_PUT:
			PRINTMSG(1, "%s request to upload file %s.\n", strDevName, pFileInfo->file_name);
			/* If allowed, keep pFileInfo->flag unmodified. Otherwise, modify pFileInfo->flag to the error code
			   reflecting the reason. */
			break;
		case BTSDK_APP_EV_FTP_DEL_FILE:
			PRINTMSG(1, "%s request to delete file %s.\n", strDevName, pFileInfo->file_name);
			/* If allowed, keep pFileInfo->flag unmodified. Otherwise, modify pFileInfo->flag to the error code
			   reflecting the reason. */
			break;
		case BTSDK_APP_EV_FTP_DEL_FOLDER:
			PRINTMSG(1, "%s request to delete folder %s.\n", strDevName, pFileInfo->file_name);
			/* If allowed, keep pFileInfo->flag unmodified. Otherwise, modify pFileInfo->flag to the error code
			   reflecting the reason. */
			break;
		case BTSDK_APP_EV_FTP_CREATE:
			PRINTMSG(1, "%s request to create folder %s.\n", strDevName, pFileInfo->file_name);
			/* If allowed, keep pFileInfo->flag unmodified. Otherwise, modify pFileInfo->flag to the error code
			   reflecting the reason. */
			break;
		case BTSDK_APP_EV_OPP_PUSH:
			PRINTMSG(1, "%s request to push object %s.\n", strDevName, pFileInfo->file_name);
			/* If allowed, keep pFileInfo->flag unmodified. Otherwise, modify pFileInfo->flag to the error code
			   reflecting the reason. */
			break;
		case BTSDK_APP_EV_OPP_PULL:
			PRINTMSG(1, "%s request to pull local business card.\n", strDevName);
			break;
		}
		break;
	case BTSDK_ER_SUCCESS:
		switch (pFileInfo->operation)
		{
		case BTSDK_APP_EV_FTP_PUT:
			PRINTMSG(1, "%s finish uploading file %s.\n", strDevName, pFileInfo->file_name);
			break;
		case BTSDK_APP_EV_OPP_PUSH:
			PRINTMSG(1, "%s finish pushing object %s.\n", strDevName, pFileInfo->file_name);
			break;
		}
		break;
	default:
		break;
	}
	return BTSDK_TRUE;
}

/*******************************************************************
*																	*
********************************************************************/
BTCONNHDL Connect2OBEXService(BTUINT16 svc_class)
{
	BTINT32 result;
	BTDEVHDL rmt_dev;
	BTCONNHDL conn_hdl;

	conn_hdl = SelectConnection(svc_class);
	if (conn_hdl != BTSDK_INVALID_HANDLE)
		return conn_hdl;

	rmt_dev = SelectRemoteDevice(0);
	if (rmt_dev == BTSDK_INVALID_HANDLE)
	{
		PRINTMSG(1, "No device selected\r\n");
		return BTSDK_INVALID_HANDLE;
	}

	result = Btsdk_StartClientEx(rmt_dev, svc_class, 0, &conn_hdl);
	if (result == BTSDK_OK)
		PRINTMSG(1, "Connect to %04x service successfully\n", svc_class);
	else
//		Btsdk_PrintErrorMessage(result, BTSDK_TRUE);
		PRINTMSG(1, "Connect to %04x service Failed\n", svc_class);
	return conn_hdl;
}

/*******************************************************************
*																	*
********************************************************************/
void DisconnectOBEXConnection(BTUINT16 svc_class)
{
	BTCONNHDL conn_hdl = SelectConnection(svc_class);
	if (BTSDK_INVALID_HANDLE != conn_hdl)
		Btsdk_DisconnectConnection(conn_hdl);
}

/*******************************************************************
*																	*
********************************************************************/
void ListRemoteOneFile(BTUINT8 *pFileInfo)
{
	WIN32_FIND_DATA *pFindData = NULL;
	if (NULL == pFileInfo)
		return;
	pFindData = (WIN32_FIND_DATA *)pFileInfo;	

	//strcpy(rmtFileInfo[g_CurrentFolderNum].Filename,pFindData->cFileName);
	WideCharToMultiByte( CP_ACP, 0, pFindData->cFileName , -1, (LPSTR)rmtFileInfo[g_CurrentFolderNum].Filename , BTSDK_PATH_MAXLENGTH, NULL, NULL);

	rmtFileInfo[g_CurrentFolderNum].nsize =pFindData->nFileSizeLow;	
	rmtFileInfo[g_CurrentFolderNum].ntype = pFindData->dwFileAttributes;		
	if (rmtFileInfo[g_CurrentFolderNum].ntype & FILE_ATTRIBUTE_DIRECTORY)
	{
		PRINTMSG(1, " <%d> %-28hs*****************DIR***********\n",g_CurrentFolderNum + 1,rmtFileInfo[g_CurrentFolderNum].Filename);
	}
	else
		PRINTMSG(1, " <%d> %-28hs*****************%d***********\n",g_CurrentFolderNum + 1,rmtFileInfo[g_CurrentFolderNum].Filename,rmtFileInfo[g_CurrentFolderNum].nsize);		
	
	g_CurrentFolderNum++;
	
//	Btsdk_FreeMemory(pFileInfo);
	pFindData = NULL;
	return;
}

/*******************************************************************
*																	*
********************************************************************/
BTINT32 FTPBrowseFolder(BTUINT8 op_code)
{
	BTCONNHDL conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_FILE_TRANS);
	BTUINT8 CurrentRemoteFolder[BTSDK_PATH_MAXLENGTH] = {0};
	BTINT32 iErrorCode = 0;	
	BTINT32 iret = 0;

	if (conn_hdl != BTSDK_INVALID_HANDLE)
	{
		/////////////////////////////////////////
		iret = Btsdk_FTPGetRmtDir(conn_hdl, CurrentRemoteFolder);
		if (BTSDK_OK !=  iret)
		{
			PRINTMSG(1, "Get remote folder return error code = 0x%x \n",iret);
		}
		///////////////////////////////////////

		g_CurrentFolderNum = 0;
		PRINTMSG(1, "\n*********Name****************************************size**********\n");
		if (op_code == FTP_OP_NEXT)
		{
			iErrorCode = Btsdk_FTPBrowseFolder(conn_hdl, g_szRemoteCurrentDir, ListRemoteOneFile, op_code);		
		}
		else
		{
			iErrorCode = Btsdk_FTPBrowseFolder(conn_hdl, CurrentRemoteFolder, ListRemoteOneFile, op_code);
		}

		if (iErrorCode != BTSDK_OK)
		{
			PRINTMSG(1, "Btsdk_FTPBrowseFolder error code = 0x%x \n",iErrorCode);
		}
		return iErrorCode;
	}
	else
	{
		return BTSDK_ER_NO_CONNECTION;
	}
}

/*******************************************************************
*																	*
********************************************************************/
BTINT32 SelectFileNumFromList(BTCONNHDL conn_hdl)
{
	BTINT32 iErrorCode = 0;	
	BTUINT8 s[10] = {0};
	BTUINT8 CurrentRemoteFolder[BTSDK_PATH_MAXLENGTH] = {0};
	BTINT32 iret = 0;

	if (conn_hdl == BTSDK_INVALID_HANDLE)
	{
		conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_FILE_TRANS);
		if (conn_hdl == BTSDK_INVALID_HANDLE)
			return BTSDK_ER_NO_CONNECTION;
	}

	/////////////////////////////////////////
	iret = Btsdk_FTPGetRmtDir(conn_hdl, CurrentRemoteFolder);
	if (BTSDK_OK !=  iret)
	{
		PRINTMSG(1, "Get remote folder return error code = 0x%x \n",iret);
	}
	///////////////////////////////////////

	g_CurrentFolderNum = 0;
	PRINTMSG(1, "\n*********Name****************************************size**********\n");
	//iErrorCode = Btsdk_FTPBrowseFolder(conn_hdl, g_szRemoteCurrentDir, ListRemoteOneFile, FTP_OP_REFRESH);
	iErrorCode = Btsdk_FTPBrowseFolder(conn_hdl, CurrentRemoteFolder, ListRemoteOneFile, FTP_OP_REFRESH);
	if (iErrorCode != BTSDK_OK)
	{
		memset(g_szRemoteCurrentDir, 0, sizeof(g_szRemoteCurrentDir));
		PRINTMSG(1, "Btsdk_FTPBrowseFolder error code = 0x%x \n",iErrorCode);
		return -1;
	}
	PRINTMSG(1, "Select the target:\n"); 
	PRINTMSG(1, "(Press key 'q' to return to the function list.)\n");
	do
	{
		PRINTMSG(1, "Target number = ");

		OnSelDlg(g_hWnd, _T("Select Target number"), _T("Please enter number"), (char *)s, 10);
		//scanf("%s", s);
		if((s[0] == 'q') || (s[0] == 'Q'))
		{
			PRINTMSG(1, "\nUser abort the operation.\n");
			return -1;
		}
		g_CurrentSelectNum = atoi((char *)s);
		if ((g_CurrentSelectNum <= 0) || (g_CurrentSelectNum > MAX_FTP_FOLDER_LIST))
		{
			PRINTMSG(1, "%d is not a valid datum, please select again.\n", g_CurrentSelectNum);
			continue;
		}
		else
		{
			PRINTMSG(1, "%d  \r\n", g_CurrentSelectNum);
			break;
		}
	}while(1);
	return g_CurrentSelectNum;
}

/*******************************************************************
*																	*
********************************************************************/
void FTPEnterSubFolder()
{
	BTINT32 iErrorCode = 0;
	
	SelectFileNumFromList(BTSDK_INVALID_HANDLE);
	
	if (g_CurrentSelectNum <= 0)
	{
		PRINTMSG(1, "SelectFileNumFromList fail:\n"); 
		return;
	}
	strcpy((char *)g_szRemoteCurrentDir, (char *)rmtFileInfo[g_CurrentSelectNum - 1].Filename);				
	if (rmtFileInfo[g_CurrentSelectNum - 1].ntype & FILE_ATTRIBUTE_DIRECTORY)
	{			
		iErrorCode = FTPBrowseFolder(FTP_OP_NEXT);
	}
	else
		PRINTMSG(1, "Your selection is not a folder, Please select another one\n");
	
	if (iErrorCode != BTSDK_OK)
	{					
		PRINTMSG(1, "Enter one remote folder return error code = 0x%x \n", iErrorCode);
	}		
	g_CurrentSelectNum = -1;
}

/*******************************************************************
*																	*
********************************************************************/
void FTPGoUpOneFolder()
{
	//BTUINT8 *p;
	BTINT32 iStrLen = 0;
	BTINT32 iErrorCode = 0;

	BTUINT8 CurrentRemoteFolder[BTSDK_PATH_MAXLENGTH] = {0};
	BTINT32 iret = 0;

	BTCONNHDL conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_FILE_TRANS);

	if (conn_hdl == BTSDK_INVALID_HANDLE)
		return;

	/////////////////////////////////////////
	iret = Btsdk_FTPGetRmtDir(conn_hdl, CurrentRemoteFolder);
	if (BTSDK_OK !=  iret)
	{
		PRINTMSG(1, "Get remote folder return error code = 0x%x \n",iret);
	}
	/////////////////////////////////////////

	memset(g_szRemoteCurrentDir, 0, sizeof(g_szRemoteCurrentDir));
	g_CurrentFolderNum = 0;
	iErrorCode = Btsdk_FTPBrowseFolder(conn_hdl, g_szRemoteCurrentDir, ListRemoteOneFile, FTP_OP_UPDIR);		
	if (iErrorCode != BTSDK_OK)
		PRINTMSG(1, "Up one level remote folder return error code = 0x%x \n",iErrorCode);			
	//free(CurrentRemoteFolder);
}

/*******************************************************************
*																	*
********************************************************************/
void FTPDownload(void)
{
	BTUINT8 *CurrentLocalPath = NULL;
	BTUINT8 *CurrentRemoteFolder = NULL;
	BTINT32 iErrorCode = 0;
	DWORD dTicks = 0;//temp test
	BTCONNHDL conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_FILE_TRANS);

	if (conn_hdl == BTSDK_INVALID_HANDLE)
		return;

	SelectFileNumFromList(conn_hdl);
	CurrentLocalPath = (BTUINT8 *)malloc(BTSDK_PATH_MAXLENGTH);
	CurrentRemoteFolder = (BTUINT8 *)malloc(BTSDK_PATH_MAXLENGTH);
	PRINTMSG(1, "Enter the folder (e.g. c:\\test_download\\) to save the downloaded file:\n");

	OnSelDlg(g_hWnd, _T("Enter the folder"), _T("Please Enter the folder"), (char *)CurrentLocalPath, BTSDK_PATH_MAXLENGTH);
	//scanf("%s", CurrentLocalPath);
	strcat((char *)CurrentLocalPath, (char *)rmtFileInfo[g_CurrentSelectNum - 1].Filename);
	strcpy((char *)CurrentRemoteFolder, (char *)rmtFileInfo[g_CurrentSelectNum - 1].Filename);
	//register the status callback function.
	Btsdk_FTPRegisterStatusCallback(conn_hdl, TransferStatusCallback);
	dTicks = GetTickCount();//temp test
	if (rmtFileInfo[g_CurrentSelectNum - 1].ntype & FILE_ATTRIBUTE_DIRECTORY)
	{			
		iErrorCode = Btsdk_FTPGetDir(conn_hdl, CurrentRemoteFolder, CurrentLocalPath);
	}
	else
		iErrorCode = Btsdk_FTPGetFile(conn_hdl, CurrentRemoteFolder, CurrentLocalPath);
	dTicks = GetTickCount() - dTicks;//temp test
			
	if (iErrorCode != BTSDK_OK)
	{					 
		PRINTMSG(1, "Get Remote file/folder return error code = 0x%x \n", iErrorCode);
	}
	else
	{
		PRINTMSG(1, "Get Remote file/folder %s successful \n", CurrentRemoteFolder);
		PRINTMSG(1, "Speed is %d Bps \n", rmtFileInfo[g_CurrentSelectNum - 1].nsize*1000/dTicks);
	}
	Btsdk_FTPRegisterStatusCallback(conn_hdl, NULL);
	g_CurrentSelectNum = -1;
	free(CurrentLocalPath);
	free(CurrentRemoteFolder);
}

/*******************************************************************
*																	*
********************************************************************/
void FTPDeleteRemote(void)
{
	BTUINT8 *CurrentRemoteFolder = NULL;
	BTINT32 iErrorCode = 0;
	BTCONNHDL conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_FILE_TRANS);

	if (conn_hdl == BTSDK_INVALID_HANDLE)
		return;

	SelectFileNumFromList(conn_hdl);
	CurrentRemoteFolder = (BTUINT8 *)malloc(BTSDK_PATH_MAXLENGTH);
	strcpy((char *)CurrentRemoteFolder, (char *)rmtFileInfo[g_CurrentSelectNum - 1].Filename);
	Btsdk_FTPRegisterStatusCallback(conn_hdl, TransferStatusCallback);
	if (rmtFileInfo[g_CurrentSelectNum - 1].ntype & FILE_ATTRIBUTE_DIRECTORY)
	{			
		iErrorCode = Btsdk_FTPDeleteDir(conn_hdl, (char *)CurrentRemoteFolder);
	}
	else
		iErrorCode = Btsdk_FTPDeleteFile(conn_hdl, (char *)CurrentRemoteFolder);
			
	if (iErrorCode != BTSDK_OK)
	{					
		PRINTMSG(1, "Delete Remote file/folder return error code = 0x%x \n", iErrorCode);
	}
	else
	{
		PRINTMSG(1, "Delete Remote file/folder %s successful \n", CurrentRemoteFolder);
		g_CurrentFolderNum = 0;
		PRINTMSG(1, "\n*********Name****************************************size**********\n");
		Btsdk_FTPBrowseFolder(conn_hdl, g_szRemoteCurrentDir, ListRemoteOneFile, FTP_OP_REFRESH);
	}
	g_CurrentSelectNum = -1;
	free(CurrentRemoteFolder);
}

/*******************************************************************
*																	*
********************************************************************/
void FTPPutFile(void)
{
	BTINT32 iErrorCode = BTSDK_OK;
	BTUINT8 szLocFile[BTSDK_PATH_MAXLENGTH] = {0};
	BTUINT8 *szFileName = szLocFile, *szDelimiter;
	BTCONNHDL conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_FILE_TRANS);
	if (conn_hdl == BTSDK_INVALID_HANDLE)
		return;
	PRINTMSG(1, "Enter the file (e.g. c:\\test_push.dat) to be put:\n");

	OnSelDlg(g_hWnd, _T("Enter the file"), _T("Please Enter the file "), (char *)szLocFile, BTSDK_PATH_MAXLENGTH);
	//scanf("%s", szLocFile);
	szDelimiter = (BTUINT8 *)strstr((char *)szFileName, "\\");
	while (szDelimiter != NULL)
	{
		szFileName = szDelimiter + 1;
		szDelimiter = (BTUINT8 *)strstr((char *)szFileName, "\\");
	}
	Btsdk_FTPRegisterStatusCallback(conn_hdl, TransferStatusCallback);
	iErrorCode = Btsdk_FTPPutFile(conn_hdl, szLocFile, szFileName);
	if (iErrorCode != BTSDK_OK)
	{					
		PRINTMSG(1, "Put file return error code = 0x%x \n", iErrorCode);
	}
	else
	{
		PRINTMSG(1, "Put file %s successful \n", szLocFile);
	}
	Btsdk_FTPRegisterStatusCallback(conn_hdl, NULL);
}

/*******************************************************************
*																	*
********************************************************************/
void FTPPutFolder(void)
{
	BTINT32 iErrorCode = BTSDK_OK;
	BTUINT8 szLocDir[BTSDK_PATH_MAXLENGTH] = {0};
	BTUINT8 *szDirName = szLocDir, *szDelimiter;
	BTCONNHDL conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_FILE_TRANS);
	if (conn_hdl == BTSDK_INVALID_HANDLE)
		return;
	PRINTMSG(1, "Enter the folder (e.g. c:\\test_push) to be put:\n");

	OnSelDlg(g_hWnd, _T("Enter the folder"), _T("Please Enter the folder "), (char *)szLocDir, BTSDK_PATH_MAXLENGTH);
	//scanf("%s", szLocDir);
	szDelimiter = (BTUINT8 *)strstr((char *)szDirName, "\\");
	while (szDelimiter != NULL)
	{
		szDirName = szDelimiter + 1;
		szDelimiter = (BTUINT8 *)strstr((char *)szDirName, "\\");
	}
	Btsdk_FTPRegisterStatusCallback(conn_hdl, TransferStatusCallback);
	iErrorCode = Btsdk_FTPPutDir(conn_hdl, szLocDir, szDirName);
	if (iErrorCode != BTSDK_OK)
	{					
		PRINTMSG(1, "Put folder return error code = 0x%x \n", iErrorCode);
	}
	else
	{
		PRINTMSG(1, "Put folder %s successful \n", szLocDir);
	}
	Btsdk_FTPRegisterStatusCallback(conn_hdl, NULL);
}

/*******************************************************************
*																	*
********************************************************************/
void FTPTestShowMenu(void)
{
	PRINTMSG(1, "\n*********************************************\n");
	PRINTMSG(1, "*     FTP Function Testing Menu             *\n");
	PRINTMSG(1, "* <1> Connect FTP server                    *\n");
	PRINTMSG(1, "* <2> Disconnect the current ftp connection *\n");
	PRINTMSG(1, "* <3> Browse remote folder list             *\n");
	PRINTMSG(1, "* <4> Enter one remote folder               *\n");
	PRINTMSG(1, "* <5> Up one level remote folder            *\n");
	PRINTMSG(1, "* <6> Get one remote file/folder            *\n");
	PRINTMSG(1, "* <7> Delete one remote file/folder         *\n");
	PRINTMSG(1, "* <8> Put one file                          *\n");
	PRINTMSG(1, "* <9> Put one folder                        *\n");
	PRINTMSG(1, "* <m> Show this menu                        *\n");
	PRINTMSG(1, "* <q> Quit                                  *\n");
	PRINTMSG(1, "*********************************************\n");
	PRINTMSG(1, ">");
}

/*******************************************************************
*																	*
********************************************************************/
void FTPExecCmd(BTUINT8 choice)
{
	BTINT32 iErrorCode = 0;

	switch (choice) 
	{		
	case '1': //Create a ftp connection
		Connect2OBEXService(BTSDK_CLS_OBEX_FILE_TRANS);
		break;
	case '2': //Disconnect the current ftp connection
		DisconnectOBEXConnection(BTSDK_CLS_OBEX_FILE_TRANS);
		break;
	case '3': //Browse remote folder list 	
		iErrorCode = FTPBrowseFolder(FTP_OP_REFRESH);			
		if (iErrorCode != BTSDK_OK)
			PRINTMSG(1, "Browse remote folder failed for error code = 0x%x \n", iErrorCode);
		break;
	case '4': //Enter one remote folder
		FTPEnterSubFolder();
		break;
	case '5': //Up one level remote folder
		FTPGoUpOneFolder();
		break;
	case '6': //Get one file or folder form the remote server.
		FTPDownload();
		break;
	case '7':
		FTPDeleteRemote();
		break;
	case '8':
		FTPPutFile();
		break;
	case '9':
		FTPPutFolder();
		break;
	case 'm':
//		system("cls");
		FTPTestShowMenu();
		return;
	case 'q':
		InterlockedDecrement(&g_NumberLevel);
		return;
	default:
		PRINTMSG(1, "Invalid command.\n");
		break;
	}
	PRINTMSG(1, "\n");
}

/*******************************************************************
*																	*
********************************************************************/
void TestFTPFunc(void)
{
	BTUINT8 ch = 0;

	FTPTestShowMenu();
	while (ch != 'q')
	{
		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TestFTPFunc: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd,g_NumberLevel);
		//scanf("%c", &ch);
		
		if (ch == '\n')
		{
			PRINTMSG(1, ">");
		}
		else
		{
			FTPExecCmd(ch);
		}
	}
}

/*******************************************************************
*																	*
********************************************************************/
void OppPutCard(void)
{
	BTCONNHDL conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_OBJ_PUSH);
	if (conn_hdl != BTSDK_INVALID_HANDLE)
	{
		BTUINT32 iRetCode = BTSDK_OK;
		BTUINT8 *strCardPath = (BTUINT8 *)malloc(BTSDK_PATH_MAXLENGTH);
		PRINTMSG(1, "Enter the vCard (e.g. c:\\mycard\\card.vcf) to be put:\n");
		
		OnSelDlg(g_hWnd, _T("Enter the vCard"), _T("Please Enter the vCard "), (char *)strCardPath, BTSDK_PATH_MAXLENGTH);		
		//scanf("%s", strCardPath);
		Btsdk_OPPRegisterStatusCallback(conn_hdl, TransferStatusCallback);
		iRetCode = Btsdk_OPPPushObj(conn_hdl, strCardPath);
		Btsdk_OPPRegisterStatusCallback(conn_hdl, NULL);
		if (BTSDK_OK == iRetCode)
			PRINTMSG(1, "It has pushed the default business card successfully\n");
		else
			PRINTMSG(1, "Push the default business card return error code:0x%x\n", iRetCode);
		free(strCardPath);
	}
}

/*******************************************************************
*																	*
********************************************************************/
void OppDealPullFile(void)
{
	BTCONNHDL conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_OBJ_PUSH);
	if (conn_hdl != BTSDK_INVALID_HANDLE)
	{
		BTUINT32 iRetCode = 0;
		BTUINT8 *strCardPathPull = (BTUINT8 *)malloc(BTSDK_PATH_MAXLENGTH);
		PRINTMSG(1, "Enter the folder (e.g. c:\\inbox\\) to save the downloaded vCard:\n");
		
		OnSelDlg(g_hWnd, _T("Enter the folder"), _T("Please Enter the folder "), (char *)strCardPathPull, BTSDK_PATH_MAXLENGTH);			
		//scanf("%s", strCardPathPull);
		Btsdk_OPPRegisterStatusCallback(conn_hdl, TransferStatusCallback);
		iRetCode = Btsdk_OPPPullObj(conn_hdl, strCardPathPull);
		Btsdk_OPPRegisterStatusCallback(conn_hdl, NULL);
		if (BTSDK_OK == iRetCode)
			PRINTMSG(1, "Remote business card has been saved to %s as remote.vcf.\n", strCardPathPull);
		else
			PRINTMSG(1, "Pull the default business card return error code:0x%x\n",iRetCode);
		free(strCardPathPull);
	}
}

/*******************************************************************
*																	*
********************************************************************/
void OppExchangeCard(void)
{
	BTCONNHDL conn_hdl = Connect2OBEXService(BTSDK_CLS_OBEX_OBJ_PUSH);
	if (conn_hdl != BTSDK_INVALID_HANDLE)
	{
		BTUINT32 iRetCodePush = 0;
		BTUINT32 iRetCodePull = 0;
		BTUINT8 *strCardPath = (BTUINT8 *)malloc(BTSDK_PATH_MAXLENGTH);
		BTUINT8 *strCardPathPull = (BTUINT8 *)malloc(BTSDK_PATH_MAXLENGTH);
		
		PRINTMSG(1, "Enter the vCard (e.g. c:\\mycard\\card.vcf) to be put:\n");
		
		OnSelDlg(g_hWnd, _T("Enter the vCard"), _T("Please Enter the vCard "), (char *)strCardPath, BTSDK_PATH_MAXLENGTH);		
		//scanf("%s", strCardPath);
		PRINTMSG(1, "Enter the folder (e.g. c:\\inbox\\) to save the downloaded vCard:\n");
		
		OnSelDlg(g_hWnd, _T("Enter the folder"), _T("Please Enter the folder "), (char *)strCardPathPull, BTSDK_PATH_MAXLENGTH);		
		//scanf("%s", strCardPathPull);
		Btsdk_OPPRegisterStatusCallback(conn_hdl, TransferStatusCallback);
		Btsdk_OPPExchangeObj(conn_hdl,strCardPath,strCardPathPull,(BTINT32 *)&iRetCodePush, (BTINT32 *)&iRetCodePull);
		Btsdk_OPPRegisterStatusCallback(conn_hdl, NULL);

		if (BTSDK_OK == iRetCodePush)
			PRINTMSG(1, "It has pushed the default business card successfully\n");
		else
			PRINTMSG(1, "Push the default business card return error code:0x%x\n", iRetCodePush);

		if (BTSDK_OK == iRetCodePull)
			PRINTMSG(1, "Remote business card has been saved to %s as remote.vcf.\n", strCardPathPull);
		else
			PRINTMSG(1, "Pull the default business card return error code:0x%x\n", iRetCodePull);
		free(strCardPath);
		free(strCardPathPull);
	}
}

/*******************************************************************
*																	*
********************************************************************/
void OPPTestShowMenu()
{
	PRINTMSG(1, "******************************************\n");
	PRINTMSG(1, "*     OPP function Testing Menu          *\n");
	PRINTMSG(1, "* <1> Connect OPP Server                 *\n");
	PRINTMSG(1, "* <2> Disconnect current opp connection  *\n");
	PRINTMSG(1, "* <3> Push my default business card      *\n");
	PRINTMSG(1, "* <4> Pull default business card         *\n");
	PRINTMSG(1, "* <5> Exchange business card             *\n");
	PRINTMSG(1, "* <m> Show this menu                     *\n");
	PRINTMSG(1, "* <q> Quit                               *\n");
	PRINTMSG(1, "******************************************\n");
	PRINTMSG(1, ">");
}

/*******************************************************************
*																	*
********************************************************************/
void OPPExecCmd(BTUINT8 choice)
{
	switch (choice) 
	{
	case '1':
		 Connect2OBEXService(BTSDK_CLS_OBEX_OBJ_PUSH);
		 break;
	case '2':
		 DisconnectOBEXConnection(BTSDK_CLS_OBEX_OBJ_PUSH);
		 break;
	case '3':
		OppPutCard();
		break;
	case '4':
		OppDealPullFile();
		break;
	case '5':
		OppExchangeCard();
		break;
	case 'm':
		//system("cls");
		OPPTestShowMenu();
		return;
	case 'q':
		InterlockedDecrement(&g_NumberLevel); 		
		return;
	default:
		PRINTMSG(1, "Invalid command.\n");
		break;
	}
	PRINTMSG(1, "\n");
}

/*******************************************************************
*																	*
********************************************************************/
void TestOPPFunc(void)
{
	BTUINT8 ch = 0;

	OPPTestShowMenu();
	while (ch != 'q')
	{
		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TestOPPFunc: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd, g_NumberLevel);

		//scanf("%c", &ch);
		
		if (ch == '\n')
		{
			PRINTMSG(1, ">");
		}
		else
		{
			OPPExecCmd(ch);
		}
	}
}
