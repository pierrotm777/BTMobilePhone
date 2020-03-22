/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2005 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    profiles_tst.c
Abstract:
	                                     
Revision History:
2007-3-20   Huyi Created

---------------------------------------------------------------------------*/
#include "profiles_tst.h"

#define FMTBD2STR(bd) (TEXT("%02X:%02X:%02X:%02X:%02X:%02X"), bd[5],bd[4],bd[3],bd[2],bd[1],bd[0])

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show profile test menu.
Arguments:
    void
Return:
	void 
---------------------------------------------------------------------------*/
void ProfilesTestShowMenu(void)
{
   	printf("*****************************************\n");
	printf("*         Profile Testing Menu          *\n");
	printf("* <1> A2DP Profile                      *\n");
	printf("* <2> FTP Profile                       *\n");
	printf("* <3> OPP Profile                       *\n");
	printf("* <4> PAN Profile                       *\n");
	printf("* <5> HFP/HSP Profile                   *\n");
	printf("* <6> SPP Profile                       *\n");
	printf("* <7> HID Profile                       *\n");
	printf("* <8> Local SPP Service                 *\n");
	printf("* <9> PBAP profile                      *\n");
	printf("* <a> MAP profile                       *\n");
	printf("* <b> GATT profile                      *\n");
	printf("* <c> AVRCP profile                     *\n");
	printf("* <r> Return to the upper menu          *\n");
	printf("*****************************************\n");
	printf(">");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function prints other information of remote device
Arguments:
	BTDEVHDL dev_hdl: [in] device handle
Return:
	void 
---------------------------------------------------------------------------*/
void GetConnectionInfo(BTDEVHDL dev_hdl)
{
	BTUINT8 ucRssi = 0;
	BTUINT16 usRole = 0;
	BTUINT16 usTimeout = 0;
	BTUINT32 ulResult = BTSDK_OK;
	/*Before calling this function, please make sure a connection has been established.*/
	ulResult = Btsdk_GetRemoteRSSI(dev_hdl, &ucRssi);
	PrintErrorMessage(ulResult, BTSDK_TRUE);
	if (BTSDK_OK == ulResult)
	{
		printf("RSSI: %04X\n", ucRssi);
		ulResult = Btsdk_GetRemoteDeviceRole(dev_hdl, &usRole);
		PrintErrorMessage(ulResult, BTSDK_TRUE);
		printf("Role: %04X\n", usRole);
		ulResult = Btsdk_GetSupervisionTimeout(dev_hdl, &usTimeout);
		PrintErrorMessage(ulResult, BTSDK_TRUE);
		printf("Super Timeout: %04X\n", usTimeout);
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the entry function for profiles test.
Arguments:
	BTUINT8 chChoice
Return:
	void 
---------------------------------------------------------------------------*/
void ProfilesMgrExc(BTUINT8 chChoice)
{
	switch(chChoice)
	{
		case '1':
			TestAVFunc();
			break;
		case '2':
			TestFTPFunc();
			break;
		case '3':
			TestOPPFunc();
			break;
		case '4':
            TestPANFunc();
		    break;
		case '5':
            TestHfpFunc();
		    break;
		case '6':
			TestSPPFunc();
			break;
		case '7':
			TestHidFunc();
			break;
		case '8':
			TestLocalSPPServiceFunc();
			break;
		case '9':
			TestPBAPFunc();
			break;	
		case 'a':
			TestMAPFunc();
			break;
		case 'b':
			TestGATT();
			break;
		case 'c':
			TestAVRCPFunc();
			break;
		default:
			printf("Invalid command.\n");
			break;
	}
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the entry function for profiles test.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestProfiles(void)
{
	BTUINT8 ucChoice = 0;	
	ProfilesTestShowMenu();
	while (ucChoice != 'r')
	{
		scanf(" %c", &ucChoice);
		getchar();
		if ('\n' == ucChoice)
		{
			printf(">>");
		}
		else if('r' == ucChoice)
		{
			break;
		}
		else
		{
			ProfilesMgrExc(ucChoice);
			printf("\n");
			ProfilesTestShowMenu();
		}
	}
}
		

