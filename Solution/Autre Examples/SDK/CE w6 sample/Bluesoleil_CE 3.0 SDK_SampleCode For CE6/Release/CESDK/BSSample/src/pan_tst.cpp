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


/* current remote device handle which provides PAN service. */
static BTDEVHDL s_currRmtPANDevHdl = BTSDK_INVALID_HANDLE;
/* current remote PAN service handle. */ 
static BTSHCHDL s_currPANSvcHdl = BTSDK_INVALID_HANDLE;
/* current PAN connection handle.*/
static BTCONNHDL s_currPANConnHdl = BTSDK_INVALID_HANDLE;

void TestPANShowMenu(void)
{
    PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, "*     PAN Testing Menu                  *\n");
	PRINTMSG(1, "* <1> Select A Remote Device            *\n");
	PRINTMSG(1, "* <2> Select Service's Handle           *\n");
	PRINTMSG(1, "* <3> Connect Remote PAN Service        *\n");
	PRINTMSG(1, "* <4> Disconnect PAN Service            *\n");
	PRINTMSG(1, "* <q> Return to The Upper Menu          *\n");
	PRINTMSG(1, "*****************************************\n");
	PRINTMSG(1, ">>");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select expected remote device according to device class.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectRmtPANDev()
{
	/* if the remote device provides PAN GN service, you can select the device's classes as argument */
	s_currRmtPANDevHdl = SelectRemoteDevice(BTSDK_DEVCLS_MASK(BTSDK_COMPCLS_DESKTOP)); 
	if (BTSDK_INVALID_HANDLE == s_currRmtPANDevHdl)
	{
		PRINTMSG(1, "Please make sure that the expected device is in discoverable state and search again.\n");
	}
	else
	{
		PRINTMSG(1, "Select remote device successfully.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to PAN get service handle according to given device handle. 
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectPANSvc()
{
	s_currPANSvcHdl = SelectRmtService(s_currRmtPANDevHdl);
	if (BTSDK_INVALID_HANDLE != s_currPANSvcHdl)
	{
		PRINTMSG(1, "select remote device's service handle successfully.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to connect specified device's service with it's service handle.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestConnectPANSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;

	ulRet = Btsdk_Connect(s_currPANSvcHdl, 0, &s_currPANConnHdl);
	if (BTSDK_OK != ulRet)
	{
		PRINTMSG(1, "Please make sure that the expected device is powered on and connectable.\n");
	}
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to register callback function for PAN events.
Arguments:
	event  [in] event ID
    len    [in] length of param
    *param [in] pointer to the address wherein IP address was storaged.
Return:
	void 
---------------------------------------------------------------------------*/
void Btsdk_PAN_Event_CbkFunc(BTUINT16 event, BTUINT16 len, BTUINT8 *param)
{	
	char *pszInfoFormat = NULL;

	if ((event == BTSDK_PAN_EV_IP_CHANGE) && (0 != (BTUINT32)(*param))) 
	{	
		pszInfoFormat = (char*)malloc(128);
		if (NULL == pszInfoFormat)
		{
			printf("Can't allocate proper memory and return.");
			return;
		}
		memset(pszInfoFormat, 0, 128);
		sprintf(pszInfoFormat, "%d.%d.%d.%d", 
			      *(param), *(param + 1), *(param + 2), *(param + 3));
		PRINTMSG(1, "The ip address allocated is: %s\n", pszInfoFormat);
		free(pszInfoFormat);
		pszInfoFormat = NULL;
	}		
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the entry function of PAN sample.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestPANFunc(void)
{
	BTUINT8 ch = 0;

	//Btsdk_PAN_RegIndCbk4ThirdParty(Btsdk_PAN_Event_CbkFunc);
	Btsdk_PAN_RegIndCbk(Btsdk_PAN_Event_CbkFunc);

    TestPANShowMenu();

	while (ch != 'q')
	{	
		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TestAVFunc: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd,g_NumberLevel);

		if (ch == '\n')
		{
			PRINTMSG(1,">>");
		}
		else
		{   
			switch (ch)
			{
				case '1':
					TestSelectRmtPANDev();
					break;
				case '2':
					TestSelectPANSvc();
					break;
				case '3':
					TestConnectPANSvc();
					break;
				case '4':
					if (BTSDK_INVALID_HANDLE != s_currPANConnHdl)
					{
						Btsdk_Disconnect(s_currPANConnHdl);
					}
					break;
				case 'q':
					InterlockedDecrement(&g_NumberLevel); 
					break;  
				default:
					PRINTMSG(1,"Invalid command.\n");
					break;
			}

			PRINTMSG(1,"\n");

			TestPANShowMenu();				
		}		
	}

	//Btsdk_PAN_RegIndCbk4ThirdParty(NULL);
	Btsdk_PAN_RegIndCbk(NULL);
}