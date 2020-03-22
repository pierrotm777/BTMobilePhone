/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2005 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    loc_dev_tst.c
Abstract:
	Sample codes of local device manager.

Revision History:
2007-5-30   Guan Tengfei Created
---------------------------------------------------------------------------*/

#include "sdk_tst.h"

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function sets local device name
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void Test_Btsdk_SetLocalName(void)
{
	BTUINT8 szLocalDevName[BTSDK_DEVNAME_LEN] = {0};
	BTUINT16 usLen = 0;
	BTUINT32 ulRet = 0;
    
	printf("Please input a string of which length is smaller than BTSDK_DEVNAME_LEN as the local device's name.\n");
	printf("Local Name = ");
	scanf(" %s", szLocalDevName);
	usLen  = strlen(szLocalDevName) +1; 
	ulRet = Btsdk_SetLocalName(szLocalDevName, usLen);
	if (BTSDK_OK == ulRet)
	{
		printf("You have set the name of this local device successfully.\n");
	}
	else
	{
		PrintErrorMessage(ulRet, BTSDK_TRUE);
	}	
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function sets discovery mode of local device 
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void Test_Btsdk_SetDiscoveryMode(void)
{
	BTUINT16 usMode = 1;
	BTUINT32 ulReturn = 0;
	
	printf("Discovery Modes:\n");
	printf("1. GENERAL_DISCOVERABLE\n");
	printf("2. LIMITED_DISCOVERABLE\n");
	printf("4. CONNECTABLE\n");
	printf("8. PAIRABLE\n");
	printf("The discovery mode can be the binary combination of the upper modes.\n");
	printf("For example, Inputting 9 indicates 'GENERAL_DISCOVERABLE|PAIRABLE'.\n");
	printf("             Inputting 5 indicates 'GENERAL_DISCOVERABLE|CONNECTABLE', and so on.\n");
	printf("Your choice is: "); 
	scanf("%d", &usMode);
	ulReturn = Btsdk_SetDiscoveryMode(usMode);
	if (BTSDK_OK == ulReturn)
	{
		printf("You have set the discovery mode of this local device successfully.\n");
		Btsdk_GetDiscoveryMode(&usMode);
		printf("The discovery mode after set is:\n");
		if (usMode & BTSDK_GENERAL_DISCOVERABLE)
		{
			printf("GENERAL_DISCOVERABLE |");
		}
		if (usMode & BTSDK_LIMITED_DISCOVERABLE)
		{
			printf(" LIMITED_DISCOVERABLE |");
		}
		if (usMode & BTSDK_CONNECTABLE)
		{
			printf(" CONNECTABLE |");
		}
		if (usMode & BTSDK_PAIRABLE)
		{
			printf(" PAIRABLE |");
		}
		printf("\n");	
	}
	else
	{
		PrintErrorMessage(ulReturn, BTSDK_TRUE);
	}	
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function sets the device class of local device 
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void Test_Btsdk_SetLocalDeviceClass(void)
{
	BTUINT32 ulDeviceClass = 0;
	BTUINT32 ulReturn = 0;

	printf("You can input '0X000104' for class of 'Desktop workstation'.\n");
	printf("You can input '0X00010C' for class of 'Laptop computer'.\n");
	printf("You can input '0X240404' for class of 'Headset', possibly used in Handsfree/Headset sample test.\n");
	printf("Please refer to the documents we provide for more information about device's class.\n");
	printf("Device's Class = ");
	scanf("%X", &ulDeviceClass);
	ulReturn = Btsdk_SetLocalDeviceClass(ulDeviceClass);
	if (BTSDK_OK == ulReturn)
	{
		Btsdk_GetLocalDeviceClass(&ulDeviceClass);
		printf("The modified local device's class is 0X%06X.\n", ulDeviceClass);
	}
	else
	{
		PrintErrorMessage(ulReturn, BTSDK_TRUE);
	}	
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function prints LMP information of local device
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void Test_Btsdk_GetLocalLMPInfo(void)
{
	int j = 0;
	BtSdkLocalLMPInfoStru struLMPInfo = {0};

	printf("\n");
	PrintErrorMessage(Btsdk_GetLocalLMPInfo(&struLMPInfo), BTSDK_TRUE);

	printf("LMP feature=");
	for(j = 0; j < 7; j++)
	{
		printf("%02X:", struLMPInfo.lmp_feature[j]);
	}
	printf("%02X\n", struLMPInfo.lmp_feature[j]);
	
	printf("The name of the manufacturer = %04X\n", struLMPInfo.manuf_name);
	printf("The sub version of the LMP firmware = %04X\n", struLMPInfo.lmp_subversion);
	printf("The main version of the LMP firmware = %02X\n", struLMPInfo.lmp_version);
	printf("HCI version = %02X\n", struLMPInfo.hci_version);
	printf("HCI revision = %04X\n", struLMPInfo.hci_revision);
	printf("Country code = %02X\n", struLMPInfo.country_code);
	printf("\n\n");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function prints information of local device
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void Test_Print_Local_Info(void)
{
	int j = 0;
	BTUINT8 szName[BTSDK_DEVNAME_LEN] = {0};
	BTUINT8 szBDAddr[BTSDK_BDADDR_LEN] = {0};
	BTUINT32 ulDevClass = 0;
	BTUINT16 usMode = 0;
	BTUINT16 usLen = BTSDK_DEVNAME_LEN;

	printf("\n");

	/* display the local device name */
	PrintErrorMessage(Btsdk_GetLocalName(szName, &usLen), BTSDK_TRUE);
	printf("Local Name = \"%s\"\n", szName);

	/* display the Bluetooth Address of the local device */
	PrintErrorMessage(Btsdk_GetLocalDeviceAddress(szBDAddr), BTSDK_TRUE);
	printf("BD Addr: ");
	for(j = 5; j > 0; j--)
	{
		printf("%02X:", szBDAddr[j]);
	}
	printf("%02X\n", szBDAddr[0]);

	/* display the device class of the local device */
	PrintErrorMessage(Btsdk_GetLocalDeviceClass(&ulDevClass), BTSDK_TRUE);
	printf("Device Class: %08lX\n", ulDevClass);

	/* display the discovery mode of the local device */
	PrintErrorMessage(Btsdk_GetDiscoveryMode(&usMode), BTSDK_TRUE);
	printf("Discovery Mode: ");
	if (usMode & BTSDK_GENERAL_DISCOVERABLE)
	{
		printf("GENERAL_DISCOVERABLE |");
	}
	if (usMode & BTSDK_LIMITED_DISCOVERABLE)
	{
		printf(" LIMITED_DISCOVERABLE |");
	}
	if (usMode & BTSDK_CONNECTABLE)
	{
		printf(" CONNECTABLE |");
	}
	if (usMode & BTSDK_PAIRABLE)
	{
		printf(" PAIRABLE\n");
	}
	printf("\n");	
}

typedef struct _GetLocalOOBDataOutStru
{
	BTUINT8 status;
	BTUINT8 c_value[16];
	BTUINT8 r_value[16];
}GetLocalOOBDataOutStru, *PGetLocalOOBDataOutStru;

void Test_Get_Local_OOB_Info()
{
	BTUINT32 event_len = sizeof(GetLocalOOBDataOutStru);
	GetLocalOOBDataOutStru oob = {0};
	Btsdk_ExecuteHCICommandEx(IDX_READ_LOCAL_OOBDATA_CMD, 0, NULL, event_len, &oob);
	if (oob.status == 0)
	{
		//User please save the OOB data to some place. And transfer to the remote device with other
		//media. During simple pair, if the remote device declare had got local device's OOB data, it should
		//handle the BTSDK_REM_OOBDATA_REQ_IND event.
		printf("+++++ Read Local OOB Data Command succeed.\n");
	}
	else
	{
		printf("+++++ Read Local OOB Data Command failure, error code: %d\n", oob.status);
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function shows local device manager menu
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void LocDevShowMenu()
{
	printf("*****************************************\n");
	printf("*   Local Device Manager Testing Menu   *\n");
	printf("* <1> Set the local device's name       *\n");
	printf("* <2> Set a discovery mode              *\n");
	printf("* <3> Set the local device's class      *\n");
	printf("* <4> Get local LMP Info                *\n");
	printf("* <5> Print the local device's Info     *\n");
	printf("* <6> Get local device OOB data info    *\n");
	printf("* <r> Return to the upper menu          *\n");
	printf("*****************************************\n");
	printf(">");
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to execute user's choice
Arguments:
	BTUINT8 choice: [in] user's choice
Return:
	void 
---------------------------------------------------------------------------*/
void LocDevExecCmd(BTUINT8 choice)
{
	switch (choice) 
	{
		case '1':
			Test_Btsdk_SetLocalName();
			break;
		case '2':
			Test_Btsdk_SetDiscoveryMode();
			break;
		case '3':
			Test_Btsdk_SetLocalDeviceClass();
			break;
		case '4':
			Test_Btsdk_GetLocalLMPInfo();
			break;
		case '5':
			Test_Print_Local_Info();
			break;
		case '6':
			Test_Get_Local_OOB_Info();
			break;
		default:
			printf("Invalid command.\n");
			break;
	}
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the main function of local device manger
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestLocDevMgr(void)
{
	BTUINT8 ucChoice = 0;
	LocDevShowMenu();
	while (ucChoice != 'r')
	{
		scanf(" %c", &ucChoice);
		getchar();		
		if (ucChoice == '\n')
		{
			printf(">>");
		}
		else if('r' == ucChoice)
		{
			break;
		}
		else
		{
			LocDevExecCmd(ucChoice);
			printf("\n");
			LocDevShowMenu();
		}
	}
}

