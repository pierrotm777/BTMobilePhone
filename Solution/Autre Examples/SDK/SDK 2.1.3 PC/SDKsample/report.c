/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2007 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    report.c
Abstract: 
	This file implements PrintErrorMessage, which prints error messages
	on the screen.

Revision History:
2007-5-30   Guan Tengfei  Created
---------------------------------------------------------------------------*/

#include "sdk_tst.h"

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
        This function is to print the message of the error code.
Arguments:
		ulErrorCode:	[in] error code defined IVT Bluetooth stack
        is_ptr:		[in] specify if the message is printed in the screen
Return Value:
        void
---------------------------------------------------------------------------*/
void PrintErrorMessage(BTUINT32 ulErrorCode, BTUINT8 is_ptr)
{
	char *pszErrMsg = NULL;

	if (ulErrorCode != BTSDK_OK)
	{
		pszErrMsg = (char *)malloc(255);
		if (NULL == pszErrMsg)
		{
			return;
		}
		strcpy(pszErrMsg, "BTSDK ERROR: ");
		switch (ulErrorCode)
		{
			case BTSDK_ER_UNKNOWN_HCI_COMMAND:
				strcat(pszErrMsg, "Unknown HCI command");
				break;
			case BTSDK_ER_NO_CONNECTION:
				strcat(pszErrMsg, "No connection");
				break;
			case BTSDK_ER_HARDWARE_FAILURE:
				strcat(pszErrMsg, "Hardware failure");
				break;
			case BTSDK_ER_PAGE_TIMEOUT:
				strcat(pszErrMsg, "Page timeout");
				break;
			case BTSDK_ER_AUTHENTICATION_FAILURE:
				strcat(pszErrMsg, "Authentication failure");
				break;
			case BTSDK_ER_KEY_MISSING:
				strcat(pszErrMsg, "Key missing");
				break;
			case BTSDK_ER_MEMORY_FULL:
				strcat(pszErrMsg, "Memory full");
				break;
			case BTSDK_ER_CONNECTION_TIMEOUT:
				strcat(pszErrMsg, "Connection timeout");
				break;
			case BTSDK_ER_MAX_NUMBER_OF_CONNECTIONS:
				strcat(pszErrMsg, "Max number of connections");
				break;
			case BTSDK_ER_MAX_NUMBER_OF_SCO_CONNECTIONS:
				strcat(pszErrMsg, "Max number of SCO connections to a device");
				break;
			case BTSDK_ER_ACL_CONNECTION_ALREADY_EXISTS :
				strcat(pszErrMsg, "ACL connection already exists");
				break;
			case BTSDK_ER_COMMAND_DISALLOWED:
				strcat(pszErrMsg, "Command rejected");
				break;
			case BTSDK_ER_HOST_REJECTED_LIMITED_RESOURCES:
				strcat(pszErrMsg, "Host rejected due to limited resources");
				break;
			case BTSDK_ER_HOST_REJECTED_SECURITY_REASONS:
				strcat(pszErrMsg, "Host rejected due to security reasons");
				break;
			case BTSDK_ER_HOST_REJECTED_PERSONAL_DEVICE:
				strcat(pszErrMsg, "Host rejected due to remote device is only a personal device");
				break;
			case BTSDK_ER_HOST_TIMEOUT:
				strcat(pszErrMsg, "Host timeout");
				break;
			case BTSDK_ER_UNSUPPORTED_FEATURE:
				strcat(pszErrMsg, "Unsupported feature or parameter value");
				break;
			case BTSDK_ER_INVALID_HCI_COMMAND_PARAMETERS:
				strcat(pszErrMsg, "Invalid HCI command parameters");
				break;
			case BTSDK_ER_PEER_DISCONNECTION_USER_END:
				strcat(pszErrMsg, "Other end terminated connection: User ended connection");
				break;
			case BTSDK_ER_PEER_DISCONNECTION_LOW_RESOURCES:
				strcat(pszErrMsg, "Other end terminated connection: Low resources");
				break;
			case BTSDK_ER_PEER_DISCONNECTION_TO_POWER_OFF:
				strcat(pszErrMsg, "Other end terminated connection: About to power off");
				break;
			case BTSDK_ER_LOCAL_DISCONNECTION:
				strcat(pszErrMsg, "Connection terminated by local host");
				break;
			case BTSDK_ER_REPEATED_ATTEMPTS:
				strcat(pszErrMsg, "Repeated attempts");
				break;
			case BTSDK_ER_PAIRING_NOT_ALLOWED:
				strcat(pszErrMsg, "Pairing not allowed");
				break;
			case BTSDK_ER_UNKNOWN_LMP_PDU:
				strcat(pszErrMsg, "Unknown LMP PDU");
				break;
			case BTSDK_ER_UNSUPPORTED_REMOTE_FEATURE:
				strcat(pszErrMsg, "Unsupported remote feature");
				break;
			case BTSDK_ER_SCO_OFFSET_REJECTED:
				strcat(pszErrMsg, "SCO offset rejected");
				break;
			case BTSDK_ER_SCO_INTERVAL_REJECTED:
				strcat(pszErrMsg, "SCO interval rejected");
				break;
			case BTSDK_ER_SCO_AIR_MODE_REJECTED:
				strcat(pszErrMsg, "SCO air mode rejected");
				break;
			case BTSDK_ER_INVALID_LMP_PARAMETERS:
				strcat(pszErrMsg, "Invalid LMP parameters");
				break;
			case BTSDK_ER_UNSPECIFIED_ERROR:
				strcat(pszErrMsg, "Unspecified error");
				break;
			case BTSDK_ER_UNSUPPORTED_LMP_PARAMETER_VALUE:
				strcat(pszErrMsg, "Unsupported LMP parameter value");
				break;
			case BTSDK_ER_ROLE_CHANGE_NOT_ALLOWED:
				strcat(pszErrMsg, "Role change not allowed");
				break;
			case BTSDK_ER_LMP_RESPONSE_TIMEOUT:
				strcat(pszErrMsg, "LMP response timeout");
				break;
			case BTSDK_ER_LMP_ERROR_TRANSACTION_COLLISION:
				strcat(pszErrMsg, "LMP error transaction collision");
				break;
			case BTSDK_ER_LMP_PDU_NOT_ALLOWED:
				strcat(pszErrMsg, "LMP PDU not allowed");
				break;
			case BTSDK_ER_ENCRYPTION_MODE_NOT_ACCEPTABLE:
				strcat(pszErrMsg, "Encryption mode not acceptable");
				break;
			case BTSDK_ER_UNIT_KEY_USED:
				strcat(pszErrMsg, "Unit Key used");
				break;
			case BTSDK_ER_QOS_IS_NOT_SUPPORTED:
				strcat(pszErrMsg, "QoS is not supported");
				break;
			case BTSDK_ER_INSTANT_PASSED:
				strcat(pszErrMsg, "Instant passed");
				break;
			case BTSDK_ER_PAIRING_WITH_UNIT_KEY_NOT_SUPPORTED:
				strcat(pszErrMsg, "Pairing with Unit Key not supported");
				break;
			case BTSDK_ER_HANDLE_NOT_EXIST:
				strcat(pszErrMsg, "Handle not exist");
				break;
			case BTSDK_ER_OPERATION_FAILURE:
				strcat(pszErrMsg, "Operation failure");
				break;
			case BTSDK_ER_SDK_UNINIT:
				strcat(pszErrMsg, "Not initialized");
				break;
			case BTSDK_ER_INVALID_PARAMETER:
				strcat(pszErrMsg, "Invalid parameters");
				break;
			case BTSDK_ER_NULL_POINTER:
				strcat(pszErrMsg, "The paramter is a null pointer");
				break;
			case BTSDK_ER_NO_MEMORY:
				strcat(pszErrMsg, "SDK no memory");
				break;
			case BTSDK_ER_BUFFER_NOT_ENOUGH:
				strcat(pszErrMsg, "SDK buffer not enough");
				break;
			case BTSDK_ER_FUNCTION_NOTSUPPORT:
				strcat(pszErrMsg, "The function is not supported");
				break;
			case BTSDK_ER_CONNECTION_EXIST:
				strcat(pszErrMsg, "Connection to this server already exists");
				break;
			case BTSDK_ER_SERVER_IS_ACTIVE:
				strcat(pszErrMsg, "The server is active");
				break;
			case BTSDK_ER_NO_SERVICE:
				strcat(pszErrMsg, "No service");
				break;
			default:
				strcat(pszErrMsg, "Unkown error");
				break;
		}
		strcat(pszErrMsg, "!\r\n");
		printf("%s", pszErrMsg);
		free(pszErrMsg);
		pszErrMsg = NULL;
	}
}
