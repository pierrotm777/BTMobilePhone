/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2005 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    sdk_tst.c
Abstract:
	Samples codes of IVT Bluetooth API
Revision History:
2007-5-30   Yang Songhua  Created

---------------------------------------------------------------------------*/

#include "sdk_tst.h"
#include "profiles_tst.h"

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function shows the main menu
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void SdkTestShowMenu()
{
	printf("\n\n");
	printf("  BlueSoleil SDK Sample App Ver 2.0.5    \n");
	printf("*****************************************\n");
	printf("*         BTSDK Testing Menu            *\n");
	printf("* <1> Local Device Manager              *\n");
	printf("* <2> Remote Device Manager             *\n");
	printf("* <3> Profile Manager                   *\n");
	printf("* <m> Return to This Menu Again         *\n");
	printf("* <q> Quit                              *\n");
	printf("*****************************************\n");
	printf(">");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is a callback function to get status from COM Server
Arguments:
	usMsgType: [in] message type
	pucData:   [in] message base on message type
	param:     [in] device or service handle
	arg:       [in] not used now
Return:
	void 
---------------------------------------------------------------------------*/
void BsStatusCBKFuc(ULONG usMsgType, ULONG pucData, ULONG param, BTUINT8 *arg)
{
	/* message received */
	switch(usMsgType)
	{
	case BTSDK_BLUETOOTH_STATUS_FLAG:
		{
			switch(pucData)
			{
			case BTSDK_BTSTATUS_TURNON:
				{
					//printf("MSG: Bluetooth is turned on.\n");
					break;
				}		
			case BTSDK_BTSTATUS_TURNOFF:
				{
					//printf("MSG: Bluetooth is turned off.\n");
					break;
				}
			case BTSDK_BTSTATUS_HWPLUGGED:
				{
					//printf("MSG: Bluetooth hardware is plugged.\n");
					break;
				}
				
			case BTSDK_BTSTATUS_HWPULLED:
				{
					//printf("MSG: Bluetooth hardware is pulled out.\n");
					break;
				}
			default:
				{
					//printf("MSG: Others.\n");
					break;
				}			
			}		
			break;
		}
	default:
		{
			//printf("MSG Received. Type: OTHER MESSAGES.\n");
			break;
		}		
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to execute user's choice.
Arguments:
	BTUINT8 choice: [in] user's choice
Return:
	void 
---------------------------------------------------------------------------*/
void ExecInputCmd(BTUINT8 choice)
{
	switch (choice) 
	{
		case '1':
			TestLocDevMgr();
			break;
		case '2':
			TestRmtDevMgr();
			break;
		case '3':
			TestProfiles();			
			break;
		case 'm':
			system("cls");
			SdkTestShowMenu();
			break;
		case 'q':
			break;
		default:
			printf("Invalid command.\n");
			break;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function registers callback function to get status change of BlueSoleil.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void Test_RegisterGetStatusCBK(void)
{
	/* register callback function to get the status change of BlueSoleil. */
	Btsdk_RegisterGetStatusInfoCB4ThirdParty(BsStatusCBKFuc);
	Btsdk_SetStatusInfoFlag(BTSDK_BLUETOOTH_STATUS_FLAG);		
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to conduct some initialization operations.
Arguments:
Return:
	BOOL 
---------------------------------------------------------------------------*/
BOOL InitBlueSoleilForSample()
{
	if (BTSDK_TRUE != Btsdk_IsServerConnected()) /* not connected with BlueSoleil */
	{
		if (BTSDK_OK == Btsdk_Init())
		{
			printf("Connected to BlueSoleil Server successfully.\n\n");		
		}
		else
		{
			printf("Fail to connect to BlueSoleil Server.\n\n");
			return FALSE;
		}			
	}
	return TRUE;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the main function
Arguments:
	void
Return:
	always return 0 
---------------------------------------------------------------------------*/
int main(void)
{
	BTUINT8 chInputCmd = 0;
	BTUINT8 chEnterChoice = 0;
	
	printf("IVT BlueSoleil SDK is being Initialized....\n");
	if (FALSE == InitBlueSoleilForSample())
	{
		printf("Fail to initialize BlueSoleil, assure BlueSoleil is installed!\n");
		printf("Press any key to exit this application please.\n");
		scanf(" %c", &chEnterChoice);
		getchar();
		return 1;
	}
	else
	{		
		RegAppIndCallback();
		Test_RegisterGetStatusCBK();
		
		if (BTSDK_TRUE != Btsdk_IsBluetoothHardwareExisted())
		{
			printf("There isn't any Bluetooth hardware detected.\n");
	        printf("1. Enter 'N' to exit this application.\n");
			printf("2. Plug a Bluetooth hardware and enter 'Y' to continue.\n");
			
			while (TRUE)
			{
				scanf(" %c",&chEnterChoice);
				getchar();

				if (('y'==chEnterChoice)||('Y'==chEnterChoice))
				{
					if (BTSDK_TRUE == Btsdk_IsBluetoothHardwareExisted())
					{
						printf("Bluetooth hardware is detected.\n");
						break;
					}
					else
					{
						printf("Bluetooth hardware isn't detected and plug it again please.\n");
						printf("Enter 'Y' to try again, Enter 'N' to exit this application.\n");
						printf(">");
					}
				}
				else if(('n'==chEnterChoice)||('N'== chEnterChoice))
				{
					return 1;
				}
				else
				{
					printf("You have entered into an invalid character.\n");
				}				
			}
		}		
		
		if (BTSDK_FALSE == Btsdk_IsBluetoothReady())
		{
			Btsdk_StartBluetooth();
		}			
		
		if (BTSDK_TRUE == Btsdk_IsBluetoothReady())
		{
		/*we default expect this application runs on desktop platform. 
			of course, you can set another device class according to your need. */
			Btsdk_SetLocalDeviceClass(BTSDK_COMPCLS_DESKTOP);				
			
			SdkTestShowMenu();
			while (chInputCmd != 'q')
			{
				scanf(" %c", &chInputCmd);
				getchar();
				if ('\n' == chInputCmd)
				{
					printf(">>");
				}
				else
				{
					ExecInputCmd(chInputCmd);
					printf("\n");
					if (chInputCmd != 'q')
					{
						SdkTestShowMenu();
					}
				}
			}
			
		}
		else
		{
			printf("BlueSoleil fail to reset hardware...\n");
		}
		
		printf("IVT BlueSoleil SDK is being quitted....\n");
		Btsdk_RegisterGetStatusInfoCB4ThirdParty(NULL);
		UnRegAppIndCallback();	
		Btsdk_Done();
		return 0;
		
	}	
}

