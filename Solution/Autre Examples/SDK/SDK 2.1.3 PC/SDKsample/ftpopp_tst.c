/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2005 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    ftpopp_tst.c
Abstract:
    this module is to test FTP and OPP profiles relative functionality. 
Revision History:
2007-3-19   Huyi  Created


---------------------------------------------------------------------------*/
#include "sdk_tst.h"
#include "profiles_tst.h"

#define MAX_FTP_FOLDER_LIST 128
#define MAX_FILENAME    256

typedef struct tagFILEINFO
{
    UCHAR   ucFileName[MAX_FILENAME];
	int	    iSize;
	int		iType; 
} FILEINFOSTRU;
/* current folder number. */
static BTINT32 s_CurrentFolderNum = 0;

/* current selected file/folder number. */
static BTINT32 s_CurrentSelectNum = -1;

/* FTP current remote shared directory. */
static BTUINT8 s_szRemoteCurrentDir[MAX_FILENAME] = { 0 };

/* current file size */
static BTUINT32 s_CurrentFileSize = 0;
static BTUINT32 s_TotalFileSize = 0;
static FILEINFOSTRU rmtFileInfoStru[MAX_FTP_FOLDER_LIST];
static BTDEVHDL s_currRmtFTPDevHdl = BTSDK_INVALID_HANDLE;
static BTSHCHDL s_currFTPSvcHdl = BTSDK_INVALID_HANDLE;
static BTCONNHDL s_currFTPConnHdl = BTSDK_INVALID_HANDLE;
static BTDEVHDL s_currRmtOPPDevHdl = BTSDK_INVALID_HANDLE;
static BTSHCHDL s_currOPPSvcHdl = BTSDK_INVALID_HANDLE;
static BTCONNHDL s_currOPPConnHdl = BTSDK_INVALID_HANDLE;

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select expected remote FTP device according to device class. 
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectRmtFTPDev()
{
	s_currRmtFTPDevHdl = SelectRemoteDevice(0);
	if (BTSDK_INVALID_HANDLE == s_currRmtFTPDevHdl)
	{
		printf("Please make sure that the expected device is in discoverable state and search again.\n");
	}	
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select expected remote OPP device according to device class. 
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectRmtOPPDev()
{
	s_currRmtOPPDevHdl = SelectRemoteDevice(0);
	if (BTSDK_INVALID_HANDLE == s_currRmtOPPDevHdl)
	{
		printf("Please make sure that the expected device is in discoverable state and search again.\n");
	}	
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to get FTP service handle according to given device handle. 
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectFTPSvc()
{
	s_currFTPSvcHdl = SelectRemoteService(s_currRmtFTPDevHdl);
	if (BTSDK_INVALID_HANDLE == s_currFTPSvcHdl)
	{
		printf("Cann't get expected service handle.\n");
	}
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to get OPP service handle according to given device handle. 
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectOPPSvc()
{
	s_currOPPSvcHdl = SelectRemoteService(s_currRmtOPPDevHdl);
	if (BTSDK_INVALID_HANDLE == s_currOPPSvcHdl)
	{
		printf("Cann't get expected service handle.\n");
	}
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to connect specified device's FTP service with it's service handle.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestConnectFTPSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;
	ulRet = Btsdk_Connect(s_currFTPSvcHdl, 0, &s_currFTPConnHdl);
	if (BTSDK_OK != ulRet)
	{
		printf("Please make sure that the expected device is powered on and connectable.\n");
		return;
	}
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to connect specified device's OPP service with it's service handle.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestConnectOPPSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;
	ulRet = Btsdk_Connect(s_currOPPSvcHdl, 0, &s_currOPPConnHdl);
	if (BTSDK_OK != ulRet)
	{
		printf("Please make sure that the expected device is powered on and connectable.\n");
		return;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to list browsed files and folders.
Arguments:
	pFileInfo: [in] pointer to WIN32_FIND_DATA.
Return:
	void 
---------------------------------------------------------------------------*/
void ListRemoteOneFile(BTUINT8 *pFileInfo)
{
	WIN32_FIND_DATA *pFindData = NULL;
	UCHAR ucFileType[MAX_FILENAME]= {0}; 
	
	if (NULL == pFileInfo)
	{
		return;
	}

	pFindData = (WIN32_FIND_DATA *)pFileInfo;	
	strcpy(rmtFileInfoStru[s_CurrentFolderNum].ucFileName, pFindData->cFileName);
	rmtFileInfoStru[s_CurrentFolderNum].iSize =pFindData->nFileSizeLow;	
	rmtFileInfoStru[s_CurrentFolderNum].iType = pFindData->dwFileAttributes;
	
	if (rmtFileInfoStru[s_CurrentFolderNum].iType & FILE_ATTRIBUTE_DIRECTORY)
	{
		strcpy(ucFileType, "DIR");
		printf(" <%d> %-28hs************%s***********\n",s_CurrentFolderNum + 1,
			    rmtFileInfoStru[s_CurrentFolderNum].ucFileName,ucFileType);
		s_CurrentFolderNum++;
	}
	else
	{
		printf(" <%d> %-28hs************%d***********\n",s_CurrentFolderNum + 1,
		        rmtFileInfoStru[s_CurrentFolderNum].ucFileName,rmtFileInfoStru[s_CurrentFolderNum].iSize);
		s_CurrentFolderNum++;
	}
	return;
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is for you to select file or folder to be operated.
Arguments:
	hConnHdl : [in] connection handle
Return:
	BTINT32 : File number you select 
---------------------------------------------------------------------------*/
BTINT32 SelectFileNumFromList(BTCONNHDL hConnHdl)
{
	BTINT32 iErrorCode = 0;	
	char szChoice[8] = { 0 };		
	BTUINT32 ucCurrentFolderNum = 0;
	
    printf("*********Name**************************************Size(B)**********\n");
	s_CurrentFolderNum = 0;
	iErrorCode = Btsdk_FTPBrowseFolder(hConnHdl, s_szRemoteCurrentDir, ListRemoteOneFile, FTP_OP_REFRESH);
	if (iErrorCode != BTSDK_OK)
	{
		if (iErrorCode ==0X6a4)
		{
			printf("The current remote folder is empty.\n");		
		}
		else
		{
			printf("Browsing remote folder list returns error code = 0x%x \n",iErrorCode);			
		}
		return 0;
	}
	printf("Select the target:\n"); 
	printf("(Press key 'q' to return to the function testing menu.)\n");
	do
	{
		printf("Target number = ");
		scanf(" %s", szChoice);
		getchar();

		if (('q' == szChoice[0]) || ('Q' == szChoice[0]))
		{
			printf("\nUser abort the operation.\n");
			break;
		}

		ucCurrentFolderNum = atoi(szChoice);

		if ((ucCurrentFolderNum <= 0) || (ucCurrentFolderNum > MAX_FTP_FOLDER_LIST))
		{
			printf("%d is not a valid datum, please select again.\n", ucCurrentFolderNum);
			continue;
		}
		else
		{
			break;
		}
		
	} while (1);
	
	return ucCurrentFolderNum;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is a callback function to show transferred file information.
Arguments:
	first:		[in]first callback flag 
	last:		[in]last callback flag 
	filename:	[in]file name, only valid when first flag is setted.
	filesize:	[in]total transfer file size, only valid when first flag is setted.
	cursize:	[in]current transfer size
Return:
	void 
---------------------------------------------------------------------------*/
void FTPStatusCallback(UCHAR ucFirst, UCHAR ucLast, UCHAR* ucFileName, DWORD dwFilesize, DWORD dwCursize)
{
	if (1 == ucFirst)
	{
		s_TotalFileSize = dwFilesize;
		printf("*******It is going to transfer file %s, and total size = %d ****\n",
			   rmtFileInfoStru[s_CurrentSelectNum-1].ucFileName,s_TotalFileSize);
	}
	
	printf("*******It is transfering file %s, and current size = %d****\n",
		    rmtFileInfoStru[s_CurrentSelectNum-1].ucFileName,s_CurrentFileSize += dwCursize);

	if (1 == ucLast)
	{
		printf("*******It has finished to transfer file %s,and total size = %d ****\n",
			   rmtFileInfoStru[s_CurrentSelectNum-1].ucFileName,s_TotalFileSize);
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show FTP user interface.
Arguments:
    void
Return:
	void 
---------------------------------------------------------------------------*/
void FTPTestShowMenu(void)
{	
	printf("***********************************************************\n");
	printf("*     FTP Function Testing Menu                           *\n");
	printf("* <1> Get shared remote directory and set working folders *\n");
	printf("* <2> Browse remote folder list                           *\n");
	printf("* <3> Enter one remote folder	                          *\n");
	printf("* <4> Get one remote file/folder                          *\n");
	printf("* <5> Put one file/folder to remote server                *\n");
	printf("* <6> Delete one remote file/folder                       *\n");
	printf("* <7> Return to upper working directory or root directory *\n");
	printf("* <r> Return to the upper menu                            *\n");
	printf("***********************************************************\n");
	printf(">>");
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution function of FTP sample.
Arguments:
    chChoice:  [in] the choice you have selected.
Return:
	void 
---------------------------------------------------------------------------*/
void FTPExecCmd(BTUINT8 chChoice)
{
	BTINT32 iErrorCode = 0;
	BTINT32 iStrLen = 0;
	BTUINT8 szChoice[10] = { 0 };
	BTUINT8 s[10] = {0};
	BTUINT8 *pPosition = NULL;
	BTUINT8 szDirectory[MAX_PATH] = { 0 };
	BTUINT8 szTemp[MAX_PATH] = { 0 };
	BTUINT8 szRoot[MAX_PATH] = {'\\'};
	BTUINT8 ucCurrentLocalPath[MAX_FILENAME] = {0};	
	BTUINT8 ucCurrentRemoteFolder[MAX_FILENAME] = {0};
	BTINT8	szNewFolder[MAX_FILENAME] = { 0 };
	BTINT8	szSavePath[MAX_PATH] = { 0 };
	WIN32_FIND_DATA pFindData;
	HANDLE hFile;
	switch (chChoice) 
	{		
	case '1':   /* set remote directory */
		Btsdk_FTPGetRmtDir(s_currFTPConnHdl, s_szRemoteCurrentDir);
		printf("The remote current working directory you get is: %s\n",s_szRemoteCurrentDir);
		printf("Do you want to create another working directory? \n");
		printf("Enter 'y' for yes, while others for no.\n");
		printf("Your choice is:");
		scanf(" %s",szChoice);
		if (('y' == szChoice[0]) || ('Y' == szChoice[0]))
		{
			printf("Create directory in remote device:");
			scanf("%s",szNewFolder);
			strcat(s_szRemoteCurrentDir, szNewFolder);
			iErrorCode = Btsdk_FTPCreateDir(s_currFTPConnHdl, s_szRemoteCurrentDir);	
			Btsdk_FTPSetRmtDir(s_currFTPConnHdl, s_szRemoteCurrentDir);
			Btsdk_FTPGetRmtDir(s_currFTPConnHdl, s_szRemoteCurrentDir);
			printf("The remote current working directory after modification is: %s", s_szRemoteCurrentDir);
		}
		break;	
		
	case '2':	/* browse remote folder list */			
		Btsdk_FTPGetRmtDir(s_currFTPConnHdl, s_szRemoteCurrentDir);
		printf("The current folder you are browsing is %s\n", s_szRemoteCurrentDir);
		printf("\n*********Name**************************************Size(B)**********\n");
		s_CurrentFolderNum = 0;
		iErrorCode = Btsdk_FTPBrowseFolder(s_currFTPConnHdl, szDirectory, ListRemoteOneFile,  FTP_OP_NEXT);
		if (iErrorCode != BTSDK_OK)
		{
			if (iErrorCode ==0X6a4)
			{
				printf("The remote folder is empty.\n");
			}
			else
			{
				printf("Browsing remote folder list returns error code = 0x%x \n",iErrorCode);
			}
		}
		break;	
		
	case '3': /* Enter one remote folder */
        s_CurrentFolderNum = SelectFileNumFromList(s_currFTPConnHdl)-1;
		if (-1 == s_CurrentFolderNum)
		{
			break;
		}
		strcpy(s_szRemoteCurrentDir,rmtFileInfoStru[s_CurrentFolderNum].ucFileName);				
		if (rmtFileInfoStru[s_CurrentFolderNum].iType & FILE_ATTRIBUTE_DIRECTORY)
		{			
			printf("The content of the selected remote folder is:\n");
			s_CurrentFolderNum = 0;
			iErrorCode = Btsdk_FTPBrowseFolder(s_currFTPConnHdl, s_szRemoteCurrentDir, ListRemoteOneFile, FTP_OP_NEXT);
		}
		else
		{
			printf("Your selection is not a folder,Please select a folder\n");
		}
		if (iErrorCode != BTSDK_OK)
		{
			if (iErrorCode ==0X6a4)
			{
				printf("The remote folder is empty.\n");
			}
			else
			{
				printf("Browsing remote folder list returns error code = 0x%x \n",iErrorCode);
			}
		}		
		s_CurrentSelectNum = -1;
		break;
		
	case '4':  /* get one file or folder from the remote server */
		s_CurrentFileSize = 0;
		s_TotalFileSize = 0;
		s_CurrentSelectNum = SelectFileNumFromList(s_currFTPConnHdl)-1;
		if (-1 == s_CurrentFolderNum)
		{
			break;
		}

		/* register the status callback function */
		ZeroMemory(ucCurrentLocalPath, strlen(ucCurrentLocalPath));
		printf("Please input the path < e.g. c:\\ > where you want to save:");
		scanf("%s",szSavePath);
		strcpy(ucCurrentLocalPath, szSavePath);
		strcat(ucCurrentLocalPath, rmtFileInfoStru[s_CurrentSelectNum].ucFileName);
		strcpy(ucCurrentRemoteFolder, rmtFileInfoStru[s_CurrentSelectNum].ucFileName);
		Btsdk_FTPRegisterStatusCallback4ThirdParty(s_currFTPConnHdl, FTPStatusCallback);
		if (rmtFileInfoStru[s_CurrentSelectNum].iType & FILE_ATTRIBUTE_DIRECTORY)
		{			
			iErrorCode = Btsdk_FTPGetDir(s_currFTPConnHdl, ucCurrentRemoteFolder,ucCurrentLocalPath);
		}
		else
			iErrorCode = Btsdk_FTPGetFile(s_currFTPConnHdl, ucCurrentRemoteFolder,ucCurrentLocalPath);
				
		if (iErrorCode != BTSDK_OK)
		{					
			printf("Getting Remote file/folder returns error code = 0x%x \n",iErrorCode);
		}
		else
		{
			printf("Get Remote file/folder %s successfully.\n",ucCurrentRemoteFolder);
		}
		Btsdk_FTPRegisterStatusCallback4ThirdParty(s_currFTPConnHdl, NULL);
		s_CurrentSelectNum = -1;
		break;

	case '5':  /*put one file or folder to the remote server */
		ZeroMemory(ucCurrentLocalPath, strlen(ucCurrentLocalPath));
		printf("please input the path of file/folder which you want to put:");
		scanf("%s",ucCurrentLocalPath);
 		hFile = FindFirstFile(ucCurrentLocalPath,&pFindData);
		/* register the status callback function */
		Btsdk_FTPRegisterStatusCallback4ThirdParty(s_currFTPConnHdl, FTPStatusCallback);
		if (pFindData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
		{			
			iErrorCode = Btsdk_FTPPutDir(s_currFTPConnHdl, ucCurrentLocalPath,pFindData.cFileName);
		}
		else
		{
			iErrorCode = Btsdk_FTPPutFile(s_currFTPConnHdl, ucCurrentLocalPath,pFindData.cFileName);
		}				
		if (iErrorCode != BTSDK_OK)
		{					
			printf("Putting Remote file/folder returns error code = 0x%x \n",iErrorCode);
		}
		else
		{
			printf("Put Remote file/folder %s successfully \n",ucCurrentRemoteFolder);
		}
		Btsdk_FTPRegisterStatusCallback4ThirdParty(s_currFTPConnHdl, NULL);
		break;

	case '6':  /* delete one file or folder on the remote server */
		s_CurrentSelectNum = SelectFileNumFromList(s_currFTPConnHdl)-1;
		if (-1 == s_CurrentFolderNum)
		{
			break;
		}

		/* register the status callback function */
		strcpy(ucCurrentRemoteFolder, rmtFileInfoStru[s_CurrentSelectNum].ucFileName);
		Btsdk_FTPRegisterStatusCallback4ThirdParty(s_currFTPConnHdl, FTPStatusCallback);
		if (rmtFileInfoStru[s_CurrentSelectNum].iType & FILE_ATTRIBUTE_DIRECTORY)
		{			
			iErrorCode = Btsdk_FTPDeleteDir(s_currFTPConnHdl, ucCurrentRemoteFolder);
		}
		else
		{
			iErrorCode = Btsdk_FTPDeleteFile(s_currFTPConnHdl, ucCurrentRemoteFolder);
		}		
		if (iErrorCode != BTSDK_OK)
		{					
			printf("Deleting Remote file/folder returns error code = 0x%x \n",iErrorCode);
		}
		else
		{
			printf("Delete Remote file/folder %s successfully \n",ucCurrentRemoteFolder);
		}
		Btsdk_FTPRegisterStatusCallback4ThirdParty(s_currFTPConnHdl, NULL);
		s_CurrentSelectNum = -1;
		break;

	case '7':	/*return to upper working directory or root directory*/
		Btsdk_FTPGetRmtDir(s_currFTPConnHdl, s_szRemoteCurrentDir);
		printf("The current folder you are browsing is %s\n", s_szRemoteCurrentDir);
		printf("Return to upper directory enter 'u',others return to root directory.\n");
		printf("Your choice is:");
		scanf("%s",szChoice);
		if (('U' == szChoice[0]) || ('u' == szChoice[0]))
		{
			Btsdk_FTPBackDir(s_currFTPConnHdl);
			Btsdk_FTPGetRmtDir(s_currFTPConnHdl, s_szRemoteCurrentDir);			
		}
		else
		{
			Btsdk_FTPGetRmtDir(s_currFTPConnHdl, s_szRemoteCurrentDir);
			while (0 != strcmp(s_szRemoteCurrentDir,"\\"))
			{
				Btsdk_FTPBackDir(s_currFTPConnHdl);
				Btsdk_FTPGetRmtDir(s_currFTPConnHdl, s_szRemoteCurrentDir);
			}
		}
		break;
		
	default:
		printf("Invalid command.\n");
		break;
	}
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the entry function of ftp sample.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestFTPFunc(void)
{
	BTUINT8 ch = 0;	
	
	TestSelectRmtFTPDev();
	TestSelectFTPSvc();
	TestConnectFTPSvc();
	if (BTSDK_INVALID_HANDLE == s_currFTPConnHdl)
	{
		printf("Establish FTP connection unsuccessfully.\n");
		printf("Please make sure the expected device's FTP service is connectable.\n");
		return;
	}	
	FTPTestShowMenu();
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
			FTPExecCmd(ch);
			printf("\n");
			FTPTestShowMenu();
		}
	}
	if (BTSDK_INVALID_HANDLE != s_currFTPConnHdl)
	{
		Btsdk_Disconnect(s_currFTPConnHdl);
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show OPP user interface.
Arguments:
    void
Return:
	void 
---------------------------------------------------------------------------*/
void OPPTestShowMenu()
{
	printf("******************************************\n");
	printf("*     OPP Function Testing Menu          *\n");
	printf("* <1> Push my default business card      *\n");
	printf("* <2> Pull default business card         *\n");
	printf("* <3> Exchange business card             *\n");
	printf("* <r> Return to the upper menu           *\n");
	printf("******************************************\n");
	printf(">>");
}

void InitOPP()
{
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution function of OPP sample.
Arguments:
    chChoice: [in] the choice you have selected.
Return:
	void 
---------------------------------------------------------------------------*/
void OPPExecCmd(BTUINT8 chChoice)
{
	BTUINT8 strCardPath[MAX_FILENAME] = { 0 };
	BTUINT8 strCardPathPull[MAX_FILENAME] = { 0 };
	BTUINT8 szInboxDirPath[MAX_FILENAME] = { 0 };
	BTUINT8 szOutboxDirPath[MAX_FILENAME] = { 0 };
	BTUINT32 iRetCode = 0;
	BTUINT32 iRetCodePush = 0;
	BTUINT32 iRetCodePull = 0;

    /* create directories as Bluetooth inbox and outbox */
	strcpy(szInboxDirPath, "C:\\Bluetooth");
	CreateDirectory(szInboxDirPath, NULL);
	strcpy(szInboxDirPath, "C:\\Bluetooth\\inbox");	
	CreateDirectory(szInboxDirPath, NULL);
	strcpy(szOutboxDirPath, "C:\\Bluetooth\\outbox");
    CreateDirectory(szOutboxDirPath, NULL);
	printf("Create two directories:\n");
	printf("Inbox: C:\\Bluetooth\\inbox\n");
	printf("Outbox: C:\\Bluetooth\\outbox\n");
	printf("If there isn't a default card in outbox, please create one !\n");
	printf("For example: a card 'Alvin.vcf'.\n");
	switch (chChoice) 
	{
	case '1': //send card to the remote device. 
		strcpy(strCardPath, "C:\\Bluetooth\\outbox\\Alvin.vcf");		
		iRetCode = Btsdk_OPPPushObj(s_currOPPConnHdl,strCardPath);
		if (BTSDK_OK == iRetCode)
		{
			printf("The default business card has been pushed successfully\n");
		}
		else
			printf("Pushing the default business card returns error code:0x%x\n", iRetCode);
		break;
	case '2': //get card from the remote device.
		strcpy(strCardPathPull, "C:\\Bluetooth\\inbox");
		iRetCode = Btsdk_OPPPullObj(s_currOPPConnHdl, strCardPathPull);
		if (BTSDK_OK == iRetCode)
		{
			printf("The default business card has been pulled successfully\n");
		}
		else
			printf("Pulling the default business card returns error code:0x%x\n",iRetCode);
		break;		
	case '3'://exchange card with the remote device.
		strcpy(strCardPath, "C:\\Bluetooth\\outbox\\Alvin.vcf");
		strcpy(strCardPathPull, "C:\\Bluetooth\\inbox");
		iRetCode = Btsdk_OPPExchangeObj(s_currOPPConnHdl, strCardPath, strCardPathPull, &iRetCodePush, &iRetCodePull);
		if(BTSDK_OK == iRetCode)
		{
			printf("The default business card has been exchanged successfully\n");			
		}
		else
		{
			printf("Pushing the default business card returns error code:0x%x\n", iRetCodePush);
			printf("Pulling the default business card returns error code:0x%x\n", iRetCodePull);
		}
		break;
	default:
		printf("Invalid command.\n");
		break;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the entry function of OPP sample.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestOPPFunc(void)
{
	BTUINT8 ch = 0;	
	
	TestSelectRmtOPPDev();
	TestSelectOPPSvc();
	TestConnectOPPSvc();
	OPPTestShowMenu();	
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
			OPPExecCmd(ch);
			printf("\n");
			OPPTestShowMenu();
		}
	}
	if (BTSDK_INVALID_HANDLE != s_currOPPConnHdl)
	{
		Btsdk_Disconnect(s_currOPPConnHdl);
	}
}

