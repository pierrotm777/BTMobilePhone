#include "Btsdk_API.h"
#include "sdk_tst.h"

void  PIMMenu(void);
void  PIMTestConnect(void);
void  PIMTestSyncPB(void);
void  PIMTestSyncSMS(void);
void  PIMTestSendSMS(void);
void  PIMTestDeleteSMS(void);
void  PIMTestDisconnect(void);
void  PIMTestUnInit(void);
DWORD PIMConnectThreadProc(LPVOID lpParameter);
void  PIMAsyncSMSCB(PSMSDATA pSMSData);

static TCHAR s_wcManu[MAX_PATH] = {0};
static TCHAR s_wcModel[MAX_PATH] = {0};

void TestPIMMgr(void)
{
	BTUINT8 cCmd = 0;

	PIMTestInit();

	PIMMenu();

	while (cCmd != 'q')
	{
		PRINTMSG(1, " \r\n <Please Tap 'M' to show 'PIM Manager Menu'> \r\n");
		PRINTMSG(1, " \r\n <Tap 'Q' to go back to 'Main Menu'> \r\n");

		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		cCmd = g_cExcCmd;

		// parse the command
		switch (cCmd) 
		{
		case '1':
			PIMTestConnect();
			break;
		case '2':
            PIMTestSyncPB();
			break;
		case '3':
			PIMTestSyncSMS();
			break;
		case '4':
			PIMTestSendSMS();
			break;
		case '5':
			PIMTestDeleteSMS();
			break;
		case '6':
			PIMTestDisconnect();
			break;
		case 'm':
			PIMMenu();
			break;
		case 'q':
			PIMTestUnInit();
			InterlockedDecrement(&g_NumberLevel);
			break;
		default:
			PRINTMSG(1, "PIMManager() > Invalid cCmd: %c\r\n", cCmd);
			break;
		}
	}

}

void PIMMenu(void)
{
	PRINTMSG(1, "**************************************************\r\n");
	PRINTMSG(1, "*  <PIM Manager Menu>\r\n");
	PRINTMSG(1, "**************************************************\r\n");
	PRINTMSG(1, "* <1> Connect to cell phone\r\n");
	PRINTMSG(1, "* <2> Synchronize Phone book\r\n");
	PRINTMSG(1, "* <3> Synchronize SMS\r\n");
	PRINTMSG(1, "* <4> Send SMS\r\n");
	PRINTMSG(1, "* <5> Delete SMS\r\n");
	PRINTMSG(1, "* <6> Disconnect the connection\r\n");
	PRINTMSG(1, "* <M> Show PIM Manager Menu\r\n");
	PRINTMSG(1, "* <Q> Return to the 'Main Menu'\r\n");
	PRINTMSG(1, "**************************************************\r\n");
}

void PIMTestInit()
{
	TCHAR wcFilename[MAX_PATH] = {0}; 
	TCHAR wcPIMWorkingPath[MAX_PATH] = {0}; 
	TCHAR wcPBDBWorkingPath[MAX_PATH] = {0}; 
	TCHAR wcSMSDBWorkingPath[MAX_PATH] = {0}; 
	GetModuleFileName(NULL, wcFilename, MAX_PATH -1);
	*(_tcsrchr(wcFilename, _T('\\'))) = 0;
	_tcscpy(wcPIMWorkingPath, wcFilename);
	_tcscpy(wcPBDBWorkingPath, wcFilename);
	_tcscpy(wcSMSDBWorkingPath, wcFilename);
	_tcscat(wcPIMWorkingPath, TEXT("\\PIM"));

	if (PIM_MGR_Init(wcPIMWorkingPath, wcPBDBWorkingPath, wcSMSDBWorkingPath))
	{
		PRINTMSG(1, "PIM Initialization succeeds!\r\n");
	}
	else
	{
		PRINTMSG(1, "PIM Initialization fail!\r\n");
	}
}

void PIMTestConnect(void)
{ 
	if (g_cur_hf_hconn == BTSDK_INVALID_HANDLE)
	{
		PRINTMSG(1, "Please set up the hands free connection first\r\n");
        return ;
	}

	DWORD id = 0;
	CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)PIMConnectThreadProc,NULL, 0, &id);
}

void PIMTestSyncPB()
{
	int nCount = PIM_MGR_SyncContacts(NULL);
	PRINTMSG(1, "%d contacts stored in the connected cell phone!\r\n", nCount);

	if(nCount >0)
	{
		const INT MAX_TEL_COUNT  =10;
		const INT MAX_ADD_COUNT  =3;
		const INT MAX_EMAIL_COUNT=3;
		const INT MAX_URL_COUNT  =3;
		const INT MAX_IM_COUNT   =3;

		PBDATA        *oContactEx = new PBDATA [nCount];
		PB_ContactNameItem      *oLinkman = new PB_ContactNameItem[nCount];
		PB_ContactOrgItem       *oOrganization = new PB_ContactOrgItem[nCount];
		PB_ContactPhotoItem     *oPhoto = new PB_ContactPhotoItem[nCount];
		PB_ContactTelephoneItem *oTel = new PB_ContactTelephoneItem[nCount * MAX_TEL_COUNT];
		PB_ContactAddressItem   *oAddress = new PB_ContactAddressItem[nCount * MAX_ADD_COUNT];
		PB_ContactEmailItem     *oEmail = new PB_ContactEmailItem[nCount * MAX_EMAIL_COUNT];
		PB_ContactURLItem       *oUrl = new PB_ContactURLItem[nCount * MAX_URL_COUNT];
		PB_ContactIMItem        *oIm = new PB_ContactIMItem[nCount * MAX_IM_COUNT];

		ZeroMemory(oContactEx,nCount * sizeof(PBDATA));
		ZeroMemory(oLinkman,nCount * sizeof(PB_ContactNameItem));
		ZeroMemory(oOrganization,nCount * sizeof(PB_ContactOrgItem));
		ZeroMemory(oPhoto,nCount * sizeof(PB_ContactPhotoItem));
		ZeroMemory(oTel,nCount * MAX_TEL_COUNT*sizeof(PB_ContactTelephoneItem));
		ZeroMemory(oAddress,nCount * MAX_ADD_COUNT*sizeof(PB_ContactAddressItem));
		ZeroMemory(oEmail,nCount * MAX_EMAIL_COUNT*sizeof(PB_ContactEmailItem));
		ZeroMemory(oUrl,nCount * MAX_URL_COUNT*sizeof(PB_ContactURLItem));
		ZeroMemory(oIm,nCount * MAX_IM_COUNT*sizeof(PB_ContactIMItem));

		DWORD *dwTelCount		= new DWORD[nCount];
		DWORD *dwAddressCount	= new DWORD[nCount];
		DWORD *dwEmailCount		= new DWORD[nCount];
		DWORD *dwUrlCount		= new DWORD[nCount];
		DWORD *dwImCount		= new DWORD[nCount];

		for(DWORD i = 0; i < (DWORD)nCount; ++i) 
		{
			DWORD k;

			for(k=0;k<MAX_TEL_COUNT;++k)
			{
				oTel[k + i * MAX_TEL_COUNT].dwSize     = sizeof(PB_ContactTelephoneItem);
			}
			for(k=0;k<MAX_ADD_COUNT;++k)
			{
				oAddress[k + i * MAX_ADD_COUNT].dwSize = sizeof(PB_ContactAddressItem);
			}
			for(k=0;k<MAX_EMAIL_COUNT;++k)
			{
				oEmail[k + i * MAX_EMAIL_COUNT].dwSize   = sizeof(PB_ContactEmailItem);
			}
			for(k=0;k<MAX_URL_COUNT;++k)
			{
				oUrl[k + i * MAX_URL_COUNT].dwSize     = sizeof(PB_ContactURLItem);
			}
			for(k=0;k<MAX_IM_COUNT;++k)
			{
				oIm[k + i * MAX_IM_COUNT].dwSize      = sizeof(PB_ContactIMItem);
			}
			oContactEx[i].dwSize    = sizeof(PBDATA);
			oLinkman[i].dwSize      = sizeof(PB_ContactNameItem);
			oOrganization[i].dwSize = sizeof(PB_ContactOrgItem);
			oContactEx[i].itemName              = oLinkman+ i;
			oContactEx[i].itemOrg               = oOrganization + i;
			oContactEx[i].itemPhoto             = oPhoto + i;

			dwTelCount[i]		= MAX_TEL_COUNT;
			dwAddressCount[i]	= MAX_ADD_COUNT;
			dwEmailCount[i]		= MAX_EMAIL_COUNT;
			dwUrlCount[i]		= MAX_URL_COUNT;
			dwImCount[i]		= MAX_IM_COUNT;

			oContactEx[i].arrayContactTelephone = oTel + i * MAX_TEL_COUNT;
			oContactEx[i].arrayContactAddress   = oAddress + i * MAX_ADD_COUNT;
			oContactEx[i].arrayContactEmail     = oEmail + i * MAX_EMAIL_COUNT;
			oContactEx[i].arrayContactURL       = oUrl + i * MAX_URL_COUNT;
			oContactEx[i].arrayContactIM        = oIm + i * MAX_IM_COUNT;
			oContactEx[i].arrayCountTelephone   = dwTelCount+ i;
			oContactEx[i].arrayCountAddress     = dwAddressCount + i;
			oContactEx[i].arrayCountEmail       = dwEmailCount + i;
			oContactEx[i].arrayCountURL         = dwUrlCount + i;
			oContactEx[i].arrayCountIM          = dwImCount + i;
		}

		PRINTMSG(1, "Syncing, please wait...\r\n");
		//解析vcf文件，获得VCard数据，并且会对nCount重新赋值。
		if (PIM_MGR_SyncContacts(oContactEx) >= 0)
		{
			PRINTMSG(1, "Sync succeeds!\r\n", nCount);
		}
		else
		{
			PRINTMSG(1, "Sync fails!\r\n", nCount);
		}

		//Release.
		delete []dwTelCount;
		delete []dwAddressCount;
		delete []dwEmailCount;
		delete []dwUrlCount;
		delete []dwImCount;

		delete []oLinkman;
		delete []oOrganization;
		delete []oPhoto;
		delete []oTel;
		delete []oAddress;
		delete []oEmail;
		delete []oUrl;
		delete []oIm;

		delete []oContactEx;
	}
}

void PIMTestSyncSMS()
{
	INT nCount = PIM_MGR_SyncSMS(NULL);
    PRINTMSG(1, "%d messages stored in the connected cell phone!\r\n", nCount);

	if (nCount > 0)
	{
		PSMSDATA pSMSData = new SMSDATA[nCount];
		ZeroMemory(pSMSData, sizeof(SMSDATA) * nCount);
		PSMSDATA pSMSTemp = NULL;
		if (pSMSData != NULL)
		{
			PRINTMSG(1, "Syncing, please wait...\r\n");
			if (PIM_MGR_SyncSMS(pSMSData) >= 0)
			{
				PRINTMSG(1, "Sync succeeds!\r\n");
				pSMSTemp = pSMSData;
				for (int i = 0; i < nCount; i++)
				{
					pSMSTemp = pSMSData + i;
					char cNumber[PIM_PHONE_NUMBER_LENGTH] = {0};
					char cSMSBody[PIM_SMS_BODY_LENGTH]= {0};
					WideCharToMultiByte( CP_ACP, 0, pSMSTemp->szSmsCallerID , -1, cNumber , PIM_PHONE_NUMBER_LENGTH, NULL, NULL);
					WideCharToMultiByte( CP_ACP, 0, pSMSTemp->szSmsBody , -1, cSMSBody , PIM_SMS_BODY_LENGTH, NULL, NULL);
					PRINTMSG(1, "ID = %d, Phone Number: %s, SMS Content: %s\r\n", i, cNumber, cSMSBody);
				}
			}
			else
			{
				PRINTMSG(1, "Sync fails!\r\n");
			}		
		}

		delete []pSMSData;
	}
}

void PIMTestSendSMS()
{
	char cPhoneNumber[PIM_PHONE_NUMBER_LENGTH] = {0};
	char cSMSBody[PIM_SMS_BODY_LENGTH] = {0};

	TCHAR szPhoneNumber[PIM_PHONE_NUMBER_LENGTH] = {0};
	TCHAR szSMSBody[PIM_SMS_BODY_LENGTH] = {0};

	OnSelDlg(g_hWnd, _T("Phone Number"), _T("Please enter the phone number in the edit"), cPhoneNumber, sizeof(cPhoneNumber));
	PRINTMSG(1, "Phone Number = %s\r\n", cPhoneNumber);
    OnSelDlg(g_hWnd, _T("SMS Content"), _T("Please enter the SMS content in the edit"), cSMSBody, sizeof(cSMSBody));
	PRINTMSG(1, "SMS Content = %s\r\n", cSMSBody);

	MultiByteToWideChar (CP_ACP, 0, cPhoneNumber, -1, szPhoneNumber , PIM_PHONE_NUMBER_LENGTH);
	MultiByteToWideChar (CP_ACP, 0, cSMSBody, -1, szSMSBody , PIM_SMS_BODY_LENGTH);

	if (strlen(cPhoneNumber) >= 5)
	{
		PRINTMSG(1, "Message is sending...\r\n" );
		if (PIM_MGR_SendSMS(szPhoneNumber, szSMSBody))
		{
			PRINTMSG(1, "Message sent!\r\n" );
		}
		else
		{
			PRINTMSG(1, "Message send fails!!\r\n");
		}
	}
}

void PIMTestDeleteSMS()
{
	PSMSDATA pSMSData = NULL;
	PSMSDATA pSMSTemp = NULL;
	INT nCount = PIM_MGR_GetSMS(NULL);
	if (nCount > 0)
	{
	    pSMSData = new SMSDATA[nCount];
		if (pSMSData != NULL)
		{
			PIM_MGR_GetSMS(pSMSData);
		}
		
		pSMSTemp = pSMSData;
		for (int i = 0; i < nCount; i++)
        {
			pSMSTemp = pSMSData + i;
			char cNumber[PIM_PHONE_NUMBER_LENGTH] = {0};
			char cSMSBody[PIM_SMS_BODY_LENGTH]= {0};
			WideCharToMultiByte( CP_ACP, 0, pSMSTemp->szSmsCallerID , -1, cNumber , PIM_PHONE_NUMBER_LENGTH, NULL, NULL);
			WideCharToMultiByte( CP_ACP, 0, pSMSTemp->szSmsBody , -1, cSMSBody , PIM_SMS_BODY_LENGTH, NULL, NULL);
			PRINTMSG(1, "ID = %d, Phone Number: %s, SMS Content: %s\r\n", i, cNumber, cSMSBody);
        }		
	}

	char cSMSID[10] = {0};
	int  SMSID = 0;
	if (0xFF == OnSelDlg(g_hWnd, _T("SMS"), _T("Please enter the SMS sequence ID to delete"), cSMSID, sizeof(cSMSID)))
	{
		PRINTMSG(1, "Delete canceled!\r\n");
		return;
	}
	PRINTMSG(1, "SMS sequence ID selected is: %S\r\n", cSMSID);

	SMSID = atoi(cSMSID);
    pSMSTemp = pSMSData + SMSID;
	PRINTMSG(1, "Deleting, please wait...\r\n");
	PIM_MGR_DelSMS(pSMSTemp);
	PRINTMSG(1, "Delete SMS succeed!\r\n");	
}

void PIMTestDisconnect()
{
	if (PIM_MGR_Disconnect())
	{
		PRINTMSG(1, "PIM disconnected!\r\n");
	}
	else
	{
		PRINTMSG(1, "PIM disconnect fails!\r\n");
	}
}

void PIMTestUnInit()
{
	if (PIM_MGR_Uninit())
	{
		PRINTMSG(1, "PIM removed!\r\n");
	}
	else
	{
		PRINTMSG(1, "PIM removes fail!\r\n");
	}
}

DWORD PIMConnectThreadProc(LPVOID lpParameter)
{
	PRINTMSG(1, "Connecting, please wait...!\r\n");

	INT iRet = -1;
	iRet = PIM_MGR_Connect(g_cur_hf_hdev, g_cur_hf_hconn, s_wcManu, s_wcModel, ST_PB_SMS);
	if (wcslen(s_wcManu) >0)
	{
		ZeroMemory(s_wcManu, MAX_PATH);
		ZeroMemory(s_wcModel, MAX_PATH);
	}
	if (iRet == PIM_CONNECT_OK)
	{
		PRINTMSG(1, "PIM connect ok!\r\n");
		if (PIM_MGR_SetAsyncSMSCB(PIMAsyncSMSCB))
		{
			PRINTMSG(1, "New message supported!\r\n");
		}
		else
		{
			PRINTMSG(1, "New message not supported!\r\n");
		}
		return iRet;
	}
	else if (iRet == PIM_CONNECT_FAIL)
	{
		PRINTMSG(1, "PIM connect Fail!\r\n");
		return iRet;
	}
	else if (iRet == PIM_CONNECT_NEEDPATCH)
	{
		PRINTMSG(1, "In order to connect PIM successfully, a patch will be needed to upload to you phone,please wait......!\r\n");
		if (PIM_MGR_UpdatePatch())
		{
			PRINTMSG(1, "The patch has sent to you phone , please restart your phone and connect PIM again.\r\n");
		}
		else
		{
			PRINTMSG(1, "Upload patch failed.\r\n");

		}
	}
	else if (iRet == PIM_CONNECT_NEEDPHONEINFO)
	{
		/*		//Get all supported phone list,you can put these data into ListView,and select the correct model.
		PPHONELIST phoneList = NULL;
		int nCount = PIM_MGR_GetPhoneList(NULL);
		if (nCount > 0)
		{
		phoneList = new PHONELIST[nCount]; 
		PIM_MGR_GetPhoneList(phoneList);
		}

		delete [] phoneList;
		*/
		char cManu[16] = {0};
		char cModel[64] = {0};

		OnSelDlg(g_hWnd, _T("Manufacture"), _T("Please enter the Manufacture in the edit"), cManu, sizeof(cManu));
		PRINTMSG(1, "Manufacture = %s\r\n", cManu);
		OnSelDlg(g_hWnd, _T("Model"), _T("Please enter the Model in the edit"), cModel, sizeof(cModel));
		PRINTMSG(1, "Model = %s\r\n", cModel);

		MultiByteToWideChar (CP_ACP, 0, cManu, -1, s_wcManu , MAX_PATH);
		MultiByteToWideChar (CP_ACP, 0, cModel, -1,s_wcModel , MAX_PATH);
		PRINTMSG(1, "Please connect PIM again.\r\n");
	}

	return iRet;
}

void  PIMAsyncSMSCB(PSMSDATA pSMSData)
{
	PRINTMSG(1, "There is a new message!\r\n");

	char cNumber[PIM_PHONE_NUMBER_LENGTH] = {0};
	char cSMSBody[PIM_SMS_BODY_LENGTH]= {0};
	WideCharToMultiByte( CP_ACP, 0, pSMSData->szSmsCallerID , -1, cNumber , PIM_PHONE_NUMBER_LENGTH, NULL, NULL);
	WideCharToMultiByte( CP_ACP, 0, pSMSData->szSmsBody , -1, cSMSBody , PIM_SMS_BODY_LENGTH, NULL, NULL);

	PRINTMSG(1, "Phone Number: %s\r\n", cNumber);
	PRINTMSG(1, "SMS Content: %s\r\n", cSMSBody);
}
