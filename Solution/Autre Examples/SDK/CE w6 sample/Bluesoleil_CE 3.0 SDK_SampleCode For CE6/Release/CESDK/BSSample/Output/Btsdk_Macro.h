/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2008 IVT Corporation
*
* All rights reserved.
* 
---------------------------------------------------------------------------*/

/////////////////////////////////////////////////////////////////////////////
// Module Name:
//     Btsdk_Macro.h
// Abstract:
//     This Module defines macros used by APIs and structures.
// Usage:
//     #include "Btsdk_Macro.h"
// 
// Author:    
//     chenjinfeng
// Revision History:
//     2008-8-4		Created
// 
/////////////////////////////////////////////////////////////////////////////
#ifndef _BTSDK_MACRO_H
#define _BTSDK_MACRO_H

//-----------------------------------------------------------------------------------------------------
//typedef begin----------------------------------------------------------------------------------------
typedef char BTINT8;
typedef unsigned char BTUINT8;
typedef unsigned char BTBOOL;
typedef short BTINT16;
typedef unsigned short BTUINT16;
typedef long BTINT32;
typedef unsigned long BTUINT32;
typedef void * BTLPVOID;

typedef BTUINT32 BTDEVHDL;
typedef BTUINT32 BTSVCHDL;
typedef BTUINT32 BTCONNHDL;
typedef BTUINT32 BTSHCHDL;
typedef BTUINT32 BTSDKHANDLE;

#define BTSDK_TRUE		1
#define BTSDK_FALSE		0
//typedef end------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//comm define begin------------------------------------------------------------------------------------
#define BTSDK_INVALID_HANDLE					0x00000000
#define BTSDK_OK								0x0000


/* Max size value used in service attribute structures */
#define BTSDK_SERVICENAME_MAXLENGTH				80
#define BTSDK_MAX_SUPPORT_FORMAT				6		/* OPP format number */
#define BTSDK_PATH_MAXLENGTH					256		/* Shall not be larger than FTP_MAX_PATH and OPP_MAX_PATH */
#define BTSDK_CARDNAME_MAXLENGTH				256		/* Shall not be larger than OPP_MAX_NAME */
#define BTSDK_PACKETTYPE_MAXNUM					10		/* PAN supported network packet type */

/* Max size value used in device attribute structures */
#define BTSDK_DEVNAME_LEN						64		/* Shall not be larger than MAX_NAME_LEN */
#define BTSDK_SHORTCUT_NAME_LEN					100
#define BTSDK_BDADDR_LEN						6
#define BTSDK_LINKKEY_LEN						16
#define BTSDK_PINCODE_LEN						16
/* For compatibility */
#define BTSDK_LINK_KEY_LENGTH					16
#define BTSDK_PIN_CODE_LEN						16
/* if: BT2.1 Supported */
#define BTSDK_MAX_EIR_LEN                       240

// maxsize value use in A2DP
#define BTSDK_A2DP_AUDIOCARD_NAME_LEN      	0x80
#define BTSDK_A2DP_CODECCAPS_LEN			0x10

#define OPP_MAX_NAME		                    256		// the maximum length of the file name 
#define FTP_MAX_PATH							256		// the maximum length of the path
#define MAX_NAME_LEN							64
#define MAX_VENDOR_PARAM_LEN					256
#define BTSDK_PATH_MAXLENGTH					256		// Shall not be larger than FTP_MAX_PATH and OPP_MAX_PATH 

//----------------------------------
//conn event for app begin----------
/* Type of Connection Event */
#define BTSDK_APP_EV_CONN_IND					0x01	
#define BTSDK_APP_EV_DISC_IND					0x02
#define BTSDK_APP_EV_CONN_CFM					0x07	
#define BTSDK_APP_EV_DISC_CFM					0x08
/* Definitions for Compatibility */
#define BTSDK_APP_EV_CONN						0x01	
#define BTSDK_APP_EV_DISC						0x02
//conn event for app end-----------
//----------------------------------

//---------------------
//error begin----------
// General Error 
#define BTSDK_ER_GENERAL_INDEX									0x0300
#define BTSDK_ER_HANDLE_NOT_EXIST								(BTSDK_ER_GENERAL_INDEX + 0x0001)
#define BTSDK_ER_OPERATION_FAILURE								(BTSDK_ER_GENERAL_INDEX + 0x0002)
#define BTSDK_ER_SDK_UNINIT										(BTSDK_ER_GENERAL_INDEX + 0x0003)
#define BTSDK_ER_INVALID_PARAMETER								(BTSDK_ER_GENERAL_INDEX + 0x0004)
#define BTSDK_ER_NULL_POINTER									(BTSDK_ER_GENERAL_INDEX + 0x0005)
#define BTSDK_ER_NO_MEMORY										(BTSDK_ER_GENERAL_INDEX + 0x0006)
#define BTSDK_ER_BUFFER_NOT_ENOUGH								(BTSDK_ER_GENERAL_INDEX + 0x0007)
#define BTSDK_ER_FUNCTION_NOTSUPPORT							(BTSDK_ER_GENERAL_INDEX + 0x0008)
#define BTSDK_ER_NO_FIXED_PIN_CODE								(BTSDK_ER_GENERAL_INDEX + 0x0009)
#define BTSDK_ER_CONNECTION_EXIST								(BTSDK_ER_GENERAL_INDEX + 0x000A)
#define BTSDK_ER_OPERATION_CONFLICT								(BTSDK_ER_GENERAL_INDEX + 0x000B)
#define BTSDK_ER_NO_MORE_CONNECTION_ALLOWED						(BTSDK_ER_GENERAL_INDEX + 0x000C)
#define BTSDK_ER_ITEM_EXIST										(BTSDK_ER_GENERAL_INDEX + 0x000D)
#define BTSDK_ER_ITEM_INUSE										(BTSDK_ER_GENERAL_INDEX + 0x000E)
#define BTSDK_ER_DEVICE_UNPAIRED								(BTSDK_ER_GENERAL_INDEX + 0x000F)
#define BTSDK_ER_BLUETOOTH_NOTREADY								(BTSDK_ER_GENERAL_INDEX + 0x0010)
#define BTSDK_ER_CHANGE_NOTHING  								(BTSDK_ER_GENERAL_INDEX + 0x0011)

// HCI Error
#define BTSDK_ER_HCI_INDEX										0x0400
#define BTSDK_ER_UNKNOWN_HCI_COMMAND							(BTSDK_ER_HCI_INDEX + 0x0001)
#define BTSDK_ER_NO_CONNECTION									(BTSDK_ER_HCI_INDEX + 0x0002)
#define BTSDK_ER_HARDWARE_FAILURE								(BTSDK_ER_HCI_INDEX + 0x0003)
#define BTSDK_ER_PAGE_TIMEOUT									(BTSDK_ER_HCI_INDEX + 0x0004)
#define BTSDK_ER_AUTHENTICATION_FAILURE							(BTSDK_ER_HCI_INDEX + 0x0005)
#define BTSDK_ER_KEY_MISSING									(BTSDK_ER_HCI_INDEX + 0x0006)
#define BTSDK_ER_MEMORY_FULL									(BTSDK_ER_HCI_INDEX + 0x0007)
#define BTSDK_ER_CONNECTION_TIMEOUT								(BTSDK_ER_HCI_INDEX + 0x0008)
#define BTSDK_ER_MAX_NUMBER_OF_CONNECTIONS						(BTSDK_ER_HCI_INDEX + 0x0009)
#define BTSDK_ER_MAX_NUMBER_OF_SCO_CONNECTIONS					(BTSDK_ER_HCI_INDEX + 0x000a)
#define BTSDK_ER_ACL_CONNECTION_ALREADY_EXISTS					(BTSDK_ER_HCI_INDEX + 0x000b)
#define BTSDK_ER_COMMAND_DISALLOWED								(BTSDK_ER_HCI_INDEX + 0x000c)
#define BTSDK_ER_HOST_REJECTED_LIMITED_RESOURCES				(BTSDK_ER_HCI_INDEX + 0x000d)
#define BTSDK_ER_HOST_REJECTED_SECURITY_REASONS					(BTSDK_ER_HCI_INDEX + 0x000e)
#define BTSDK_ER_HOST_REJECTED_PERSONAL_DEVICE					(BTSDK_ER_HCI_INDEX + 0x000f)
#define BTSDK_ER_HOST_TIMEOUT									(BTSDK_ER_HCI_INDEX + 0x0010)
#define BTSDK_ER_UNSUPPORTED_FEATURE							(BTSDK_ER_HCI_INDEX + 0x0011)
#define BTSDK_ER_INVALID_HCI_COMMAND_PARAMETERS					(BTSDK_ER_HCI_INDEX + 0x0012)
#define BTSDK_ER_PEER_DISCONNECTION_USER_END					(BTSDK_ER_HCI_INDEX + 0x0013)
#define BTSDK_ER_PEER_DISCONNECTION_LOW_RESOURCES				(BTSDK_ER_HCI_INDEX + 0x0014)
#define BTSDK_ER_PEER_DISCONNECTION_TO_POWER_OFF				(BTSDK_ER_HCI_INDEX + 0x0015)
#define BTSDK_ER_LOCAL_DISCONNECTION							(BTSDK_ER_HCI_INDEX + 0x0016)
#define BTSDK_ER_REPEATED_ATTEMPTS								(BTSDK_ER_HCI_INDEX + 0x0017)
#define BTSDK_ER_PAIRING_NOT_ALLOWED							(BTSDK_ER_HCI_INDEX + 0x0018)
#define BTSDK_ER_UNKNOWN_LMP_PDU								(BTSDK_ER_HCI_INDEX + 0x0019)
#define BTSDK_ER_UNSUPPORTED_REMOTE_FEATURE						(BTSDK_ER_HCI_INDEX + 0x001a)
#define BTSDK_ER_SCO_OFFSET_REJECTED							(BTSDK_ER_HCI_INDEX + 0x001b)
#define BTSDK_ER_SCO_INTERVAL_REJECTED							(BTSDK_ER_HCI_INDEX + 0x001c)
#define BTSDK_ER_SCO_AIR_MODE_REJECTED							(BTSDK_ER_HCI_INDEX + 0x001d)
#define BTSDK_ER_INVALID_LMP_PARAMETERS							(BTSDK_ER_HCI_INDEX + 0x001e)
#define BTSDK_ER_UNSPECIFIED_ERROR								(BTSDK_ER_HCI_INDEX + 0x001f)
#define BTSDK_ER_UNSUPPORTED_LMP_PARAMETER_VALUE				(BTSDK_ER_HCI_INDEX + 0x0020)
#define BTSDK_ER_ROLE_CHANGE_NOT_ALLOWED						(BTSDK_ER_HCI_INDEX + 0x0021)
#define BTSDK_ER_LMP_RESPONSE_TIMEOUT							(BTSDK_ER_HCI_INDEX + 0x0022)
#define BTSDK_ER_LMP_ERROR_TRANSACTION_COLLISION				(BTSDK_ER_HCI_INDEX + 0x0023)
#define BTSDK_ER_LMP_PDU_NOT_ALLOWED							(BTSDK_ER_HCI_INDEX + 0x0024)
#define BTSDK_ER_ENCRYPTION_MODE_NOT_ACCEPTABLE					(BTSDK_ER_HCI_INDEX + 0x0025)
#define BTSDK_ER_UNIT_KEY_USED									(BTSDK_ER_HCI_INDEX + 0x0026)
#define BTSDK_ER_QOS_IS_NOT_SUPPORTED							(BTSDK_ER_HCI_INDEX + 0x0027)
#define BTSDK_ER_INSTANT_PASSED									(BTSDK_ER_HCI_INDEX + 0x0028)
#define BTSDK_ER_PAIRING_WITH_UNIT_KEY_NOT_SUPPORTED			(BTSDK_ER_HCI_INDEX + 0x0029)
#define BTSDK_ER_DIFFERENT_TRANSACTION_COLLISION				(BTSDK_ER_HCI_INDEX + 0x002A)
#define BTSDK_ER_QOS_UNACCEPTABLE_PARAMETER						(BTSDK_ER_HCI_INDEX + 0x002C)
#define BTSDK_ER_QOS_REJECTED									(BTSDK_ER_HCI_INDEX + 0x002D)
#define BTSDK_ER_CHANNEL_CLASS_NOT_SUPPORTED					(BTSDK_ER_HCI_INDEX + 0x002E)
#define BTSDK_ER_INSUFFICIENT_SECURITY							(BTSDK_ER_HCI_INDEX + 0x002F)
#define BTSDK_ER_PARAMETER_OUT_OF_RANGE							(BTSDK_ER_HCI_INDEX + 0x0030)
#define BTSDK_ER_ROLE_SWITCH_PENDING							(BTSDK_ER_HCI_INDEX + 0x0032)
#define BTSDK_ER_RESERVED_SLOT_VIOLATION						(BTSDK_ER_HCI_INDEX + 0x0034)
#define BTSDK_ER_ROLE_SWITCH_FAILED								(BTSDK_ER_HCI_INDEX + 0x0035)

// SDP error 
#define BTSDK_ER_SDP_INDEX										0x00C0
#define BTSDK_ER_SERVER_IS_ACTIVE								(BTSDK_ER_SDP_INDEX + 0x0000)
#define BTSDK_ER_NO_SERVICE										(BTSDK_ER_SDP_INDEX + 0x0001)
#define BTSDK_ER_SERVICE_RECORD_NOT_EXIST						(BTSDK_ER_SDP_INDEX + 0x0002)

// CTP/ICP error 
#define BTSDK_ER_CTP_INDEX										0x0500
#define BTSDK_ER_CTP_GW_EXIST									(BTSDK_ER_CTP_INDEX + 0x0000)	
#define BTSDK_ER_CTP_GW_NONEXIST								(BTSDK_ER_CTP_INDEX + 0x0001)
#define BTSDK_ER_USER_HANGUP									(BTSDK_ER_CTP_INDEX + 0x0002)
#define BTSDK_ER_REMOTE_HANGUP									(BTSDK_ER_CTP_INDEX + 0x0003)

// OBEX error 
#define BTSDK_ER_OBEX_INDEX										0x0600
#define BTSDK_ER_CONTINUE										(BTSDK_ER_OBEX_INDEX + 0x0090)
#define BTSDK_ER_SUCCESS										(BTSDK_ER_OBEX_INDEX + 0x00A0)
#define BTSDK_ER_CREATED										(BTSDK_ER_OBEX_INDEX + 0x00A1)
#define BTSDK_ER_ACCEPTED										(BTSDK_ER_OBEX_INDEX + 0x00A2)
#define BTSDK_ER_NON_AUTH_INFO									(BTSDK_ER_OBEX_INDEX + 0x00A3)
#define BTSDK_ER_NO_CONTENT										(BTSDK_ER_OBEX_INDEX + 0x00A4)
#define BTSDK_ER_RESET_CONTENT									(BTSDK_ER_OBEX_INDEX + 0x00A5)
#define BTSDK_ER_PARTIAL_CONTENT								(BTSDK_ER_OBEX_INDEX + 0x00A6)
#define BTSDK_ER_MULT_CHOICES									(BTSDK_ER_OBEX_INDEX + 0x00B0)
#define BTSDK_ER_MOVE_PERM										(BTSDK_ER_OBEX_INDEX + 0x00B1)
#define BTSDK_ER_MOVE_TEMP										(BTSDK_ER_OBEX_INDEX + 0x00B2)
#define BTSDK_ER_SEE_OTHER										(BTSDK_ER_OBEX_INDEX + 0x00B3)
#define BTSDK_ER_NOT_MODIFIED									(BTSDK_ER_OBEX_INDEX + 0x00B4)
#define BTSDK_ER_USE_PROXY										(BTSDK_ER_OBEX_INDEX + 0x00B5)
#define BTSDK_ER_BAD_REQUEST									(BTSDK_ER_OBEX_INDEX + 0x00C0)
#define BTSDK_ER_UNAUTHORIZED									(BTSDK_ER_OBEX_INDEX + 0x00C1)
#define BTSDK_ER_PAY_REQ										(BTSDK_ER_OBEX_INDEX + 0x00C2)
#define BTSDK_ER_FORBIDDEN										(BTSDK_ER_OBEX_INDEX + 0x00C3)
#define BTSDK_ER_NOTFOUND										(BTSDK_ER_OBEX_INDEX + 0x00C4)
#define BTSDK_ER_METHOD_NOT_ALLOWED								(BTSDK_ER_OBEX_INDEX + 0x00C5)
#define BTSDK_ER_NOT_ACCEPTABLE									(BTSDK_ER_OBEX_INDEX + 0x00C6)
#define BTSDK_ER_PROXY_AUTH_REQ									(BTSDK_ER_OBEX_INDEX + 0x00C7)
#define BTSDK_ER_REQUEST_TIMEOUT								(BTSDK_ER_OBEX_INDEX + 0x00C8)
#define BTSDK_ER_CONFLICT										(BTSDK_ER_OBEX_INDEX + 0x00C9)
#define BTSDK_ER_GONE											(BTSDK_ER_OBEX_INDEX + 0x00CA)
#define BTSDK_ER_LEN_REQ										(BTSDK_ER_OBEX_INDEX + 0x00CB)
#define BTSDK_ER_PREC_FAIL										(BTSDK_ER_OBEX_INDEX + 0x00CC)
#define BTSDK_ER_REQ_ENTITY_TOO_LARGE							(BTSDK_ER_OBEX_INDEX + 0x00CD)
#define BTSDK_ER_URL_TOO_LARGE									(BTSDK_ER_OBEX_INDEX + 0x00CE)
#define BTSDK_ER_UNSUPPORTED_MEDIA_TYPE							(BTSDK_ER_OBEX_INDEX + 0x00CF)
#define BTSDK_ER_SVR_ERR										(BTSDK_ER_OBEX_INDEX + 0x00D0)
#define BTSDK_ER_NOTIMPLEMENTED									(BTSDK_ER_OBEX_INDEX + 0x00D1)
#define BTSDK_ER_BAD_GATEWAY									(BTSDK_ER_OBEX_INDEX + 0x00D2)
#define BTSDK_ER_SERVICE_UNAVAILABLE							(BTSDK_ER_OBEX_INDEX + 0x00D3)
#define BTSDK_ER_GATEWAY_TIMEOUT								(BTSDK_ER_OBEX_INDEX + 0x00D4)
#define BTSDK_ER_HTTP_NOTSUPPORT								(BTSDK_ER_OBEX_INDEX + 0x00D5)
#define BTSDK_ER_DATABASE_FULL									(BTSDK_ER_OBEX_INDEX + 0x00E0)
#define BTSDK_ER_DATABASE_LOCK									(BTSDK_ER_OBEX_INDEX + 0x00E1)

// Serial wrap error
#define BTSDK_ER_SWRAPINDX										0X0700
#define BTSDK_ER_COM_INUSED										(BTSDK_ER_SWRAPINDX + 0x0001)
#define BTSDK_ER_COM_OPENNOTCOMPLETED							(BTSDK_ER_SWRAPINDX + 0x0002)

// APP extended error base
#define BTSDK_ER_APPEXTEND_INDEX								0x4000
//error end------------
//---------------------

#define SERVICE_CLASS_MASK		0xFFE000
#define	MAJOR_DEVICE_CLASS_MASK	0x001F00
#define MINOR_DEVICE_CLASS_MASK 0x0000FC

//---------------------------
//device class begin---------
// major device classes			                                    
#define BTSDK_DEVCLS_MISC						0x000000
#define BTSDK_DEVCLS_COMPUTER					0x000100
#define BTSDK_DEVCLS_PHONE						0x000200
#define BTSDK_DEVCLS_LAP						0x000300
#define BTSDK_DEVCLS_AUDIO						0x000400
#define BTSDK_DEVCLS_PERIPHERAL					0x000500
#define BTSDK_DEVCLS_IMAGE             			0x000600
#define BTSDK_DEVCLS_WEARABLE          			0x000700
#define BTSDK_DEVCLS_UNCLASSIFIED				0x001F00
#define BTSDK_DEVCLS_MASK(a)					(((BTUINT32)(a) >> 8) & 0x1F)
#define BTSDK_MINDEVCLS_MASK(a)					(((BTUINT32)(a) >> 2) & 0x3F)

// the minor device class field - computer major class 
#define BTSDK_COMPCLS_UNCLASSIFIED				(BTSDK_DEVCLS_COMPUTER | 0x000000) 
#define BTSDK_COMPCLS_DESKTOP					(BTSDK_DEVCLS_COMPUTER | 0x000004)
#define BTSDK_COMPCLS_SERVER            		(BTSDK_DEVCLS_COMPUTER | 0x000008)
#define BTSDK_COMPCLS_LAPTOP            		(BTSDK_DEVCLS_COMPUTER | 0x00000C)
#define BTSDK_COMPCLS_HANDHELD				  	(BTSDK_DEVCLS_COMPUTER | 0x000010)
#define BTSDK_COMPCLS_PALMSIZED					(BTSDK_DEVCLS_COMPUTER | 0x000014)
#define BTSDK_COMPCLS_WEARABLE					(BTSDK_DEVCLS_COMPUTER | 0x000018)

// the minor device class field - phone major class
#define BTSDK_PHONECLS_UNCLASSIFIED   			(BTSDK_DEVCLS_PHONE | 0x000000) 
#define BTSDK_PHONECLS_CELLULAR         		(BTSDK_DEVCLS_PHONE | 0x000004)
#define BTSDK_PHONECLS_CORDLESS        			(BTSDK_DEVCLS_PHONE | 0x000008)
#define BTSDK_PHONECLS_SMARTPHONE    			(BTSDK_DEVCLS_PHONE | 0x00000C)
#define BTSDK_PHONECLS_WIREDMODEM    			(BTSDK_DEVCLS_PHONE | 0x000010)
#define BTSDK_PHONECLS_COMMONISDNACCESS			(BTSDK_DEVCLS_PHONE | 0x000014)
#define BTSDK_PHONECLS_SIMCARDREADER			(BTSDK_DEVCLS_PHONE | 0x000018)

// the minor device class field - LAN/Network access point major class
#define BTSDK_LAP_FULLY             	    	(BTSDK_DEVCLS_LAP | 0x000000)
#define BTSDK_LAP_17              		       	(BTSDK_DEVCLS_LAP | 0x000020)
#define BTSDK_LAP_33              		       	(BTSDK_DEVCLS_LAP | 0x000040)
#define BTSDK_LAP_50                		   	(BTSDK_DEVCLS_LAP | 0x000060)
#define BTSDK_LAP_67              		       	(BTSDK_DEVCLS_LAP | 0x000080)
#define BTSDK_LAP_83              		       	(BTSDK_DEVCLS_LAP | 0x0000A0)
#define BTSDK_LAP_99               		      	(BTSDK_DEVCLS_LAP | 0x0000C0)
#define BTSDK_LAP_NOSRV       			       	(BTSDK_DEVCLS_LAP | 0x0000E0)

// the minor device class field - Audio/Video major class
#define BTSDK_AV_UNCLASSIFIED        		   	(BTSDK_DEVCLS_AUDIO | 0x000000)
#define BTSDK_AV_HEADSET              		  	(BTSDK_DEVCLS_AUDIO | 0x000004)
#define BTSDK_AV_HANDSFREE           		  	(BTSDK_DEVCLS_AUDIO | 0x000008)
#define BTSDK_AV_HEADANDHAND           			(BTSDK_DEVCLS_AUDIO | 0x00000C)
#define BTSDK_AV_MICROPHONE            			(BTSDK_DEVCLS_AUDIO | 0x000010) 
#define BTSDK_AV_LOUDSPEAKER           			(BTSDK_DEVCLS_AUDIO | 0x000014)
#define BTSDK_AV_HEADPHONES            			(BTSDK_DEVCLS_AUDIO | 0x000018)
#define BTSDK_AV_PORTABLEAUDIO     				(BTSDK_DEVCLS_AUDIO | 0x00001C)
#define BTSDK_AV_CARAUDIO          		     	(BTSDK_DEVCLS_AUDIO | 0x000020)
#define BTSDK_AV_SETTOPBOX       		       	(BTSDK_DEVCLS_AUDIO | 0x000024)
#define BTSDK_AV_HIFIAUDIO          		   	(BTSDK_DEVCLS_AUDIO | 0x000028)
#define BTSDK_AV_VCR                     		(BTSDK_DEVCLS_AUDIO | 0x00002C)
#define BTSDK_AV_VIDEOCAMERA      		     	(BTSDK_DEVCLS_AUDIO | 0x000030)
#define BTSDK_AV_CAMCORDER           	  		(BTSDK_DEVCLS_AUDIO | 0x000034)
#define BTSDK_AV_VIDEOMONITOR	        	 	(BTSDK_DEVCLS_AUDIO | 0x000038)
#define BTSDK_AV_VIDEODISPANDLOUDSPK		 	(BTSDK_DEVCLS_AUDIO | 0x00003C) 
#define BTSDK_AV_VIDEOCONFERENCE     	  		(BTSDK_DEVCLS_AUDIO | 0x000040)
#define BTSDK_AV_GAMEORTOY             			(BTSDK_DEVCLS_AUDIO | 0x000048)

// the minor device class field - peripheral major class
#define BTSDK_PERIPHERAL_UNCLASSIFIED			(BTSDK_DEVCLS_PERIPHERAL | 0x000000) 
#define BTSDK_PERIPHERAL_JOYSTICK				(BTSDK_DEVCLS_PERIPHERAL | 0x000004)
#define BTSDK_PERIPHERAL_GAMEPAD				(BTSDK_DEVCLS_PERIPHERAL | 0x000008)
#define BTSDK_PERIPHERAL_REMCONTROL				(BTSDK_DEVCLS_PERIPHERAL | 0x00000C)
#define BTSDK_PERIPHERAL_SENSE				 	(BTSDK_DEVCLS_PERIPHERAL | 0x000010)
#define BTSDK_PERIPHERAL_TABLET				 	(BTSDK_DEVCLS_PERIPHERAL | 0x000014)
#define BTSDK_PERIPHERAL_SIMCARDREADER			(BTSDK_DEVCLS_PERIPHERAL | 0x000018)
#define BTSDK_PERIPHERAL_KEYBOARD			 	(BTSDK_DEVCLS_PERIPHERAL | 0x000040)    
#define BTSDK_PERIPHERAL_POINT			    	(BTSDK_DEVCLS_PERIPHERAL | 0x000080)
#define BTSDK_PERIPHERAL_KEYORPOINT			 	(BTSDK_DEVCLS_PERIPHERAL | 0x0000C0)

// the minor device class field - imaging major class
#define BTSDK_IMAGE_DISPLAY					   	(BTSDK_DEVCLS_IMAGE | 0x000010)
#define BTSDK_IMAGE_CAMERA					   	(BTSDK_DEVCLS_IMAGE | 0x000020)
#define BTSDK_IMAGE_SCANNER					   	(BTSDK_DEVCLS_IMAGE | 0x000040)
#define BTSDK_IMAGE_PRINTER					   	(BTSDK_DEVCLS_IMAGE | 0x000080)

// the minor device class field - wearable major class
#define BTSDK_WERABLE_WATCH					   	(BTSDK_DEVCLS_WEARABLE | 0x000004)
#define BTSDK_WERABLE_PAGER					   	(BTSDK_DEVCLS_WEARABLE | 0x000008)
#define BTSDK_WERABLE_JACKET				   	(BTSDK_DEVCLS_WEARABLE | 0x00000C)
#define BTSDK_WERABLE_HELMET				   	(BTSDK_DEVCLS_WEARABLE | 0x000010)
#define BTSDK_WERABLE_GLASSES				   	(BTSDK_DEVCLS_WEARABLE | 0x000014)
//device class end-----------
//---------------------------

//---------------------------
//service class begin---------
// major service classes
#define BTSDK_SRVCLS_LDM						0x002000
#define BTSDK_SRVCLS_POSITION					0x010000
#define BTSDK_SRVCLS_NETWORK					0x020000
#define BTSDK_SRVCLS_RENDER						0x040000
#define BTSDK_SRVCLS_CAPTURE					0x080000
#define BTSDK_SRVCLS_OBJECT						0x100000
#define BTSDK_SRVCLS_AUDIO						0x200000
#define BTSDK_SRVCLS_TELEPHONE					0x400000
#define BTSDK_SRVCLS_INFOR						0x800000
#define BTSDK_SRVCLS_MASK(a)					(((BTUINT32)(a) >> 13) & 0x7FF)

//  Class of Service 
#define BTSDK_CLS_SERIAL_PORT					0x1101
#define BTSDK_CLS_LAN_ACCESS					0x1102
#define BTSDK_CLS_DIALUP_NET					0x1103
#define BTSDK_CLS_IRMC_SYNC						0x1104
#define BTSDK_CLS_OBEX_OBJ_PUSH					0x1105
#define BTSDK_CLS_OBEX_FILE_TRANS				0x1106
#define BTSDK_CLS_IRMC_SYNC_CMD					0x1107
#define BTSDK_CLS_HEADSET						0x1108
#define BTSDK_CLS_CORDLESS_TELE					0x1109
#define BTSDK_CLS_AUDIO_SOURCE					0x110A	
#define BTSDK_CLS_AUDIO_SINK					0x110B
#define BTSDK_CLS_AVRCP_TG						0x110C
#define BTSDK_CLS_ADV_AUDIO_DISTRIB				0x110D
#define BTSDK_CLS_AVRCP_CT						0x110E
#define BTSDK_CLS_VIDEO_CONFERENCE				0x110F
#define BTSDK_CLS_INTERCOM						0x1110
#define BTSDK_CLS_FAX							0x1111
#define BTSDK_CLS_HEADSET_AG					0x1112
#define BTSDK_CLS_WAP							0x1113
#define BTSDK_CLS_WAP_CLIENT					0x1114
#define BTSDK_CLS_PAN_PANU						0x1115
#define BTSDK_CLS_PAN_NAP						0x1116
#define BTSDK_CLS_PAN_GN						0x1117
#define BTSDK_CLS_DIRECT_PRINT					0x1118
#define BTSDK_CLS_REF_PRINT						0x1119
#define BTSDK_CLS_IMAGING						0x111A
#define BTSDK_CLS_IMAG_RESPONDER				0x111B
#define BTSDK_CLS_IMAG_AUTO_ARCH				0x111C
#define BTSDK_CLS_IMAG_REF_OBJ					0x111D
#define BTSDK_CLS_HANDSFREE						0x111E
#define BTSDK_CLS_HANDSFREE_AG					0x111F
#define BTSDK_CLS_DPS_REF_OBJ			        0x1120
#define BTSDK_CLS_REFLECTED_UI			        0x1121
#define BTSDK_CLS_BASIC_PRINT			        0x1122
#define BTSDK_CLS_PRINT_STATUS			        0x1123
#define BTSDK_CLS_HID							0x1124
#define BTSDK_CLS_HCRP							0x1125
#define BTSDK_CLS_HCR_PRINT						0x1126
#define BTSDK_CLS_HCR_SCAN						0x1127
#define BTSDK_CLS_SIM_ACCESS					0x112D
#define BTSDK_CLS_PBAP_PCE					    0x112E
#define BTSDK_CLS_PBAP_PSE					    0x112F
#define BTSDK_CLS_PHONEBOOK_ACCESS			    0x1130
#define BTSDK_CLS_PNP_INFO						0x1200
//service class end-----------
//---------------------------
//comm define end------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//ini begin--------------------------------------------------------------------------------------------
// Type of Callback Indication 
#define BTSDK_PIN_CODE_REQ_IND			0x00
#define BTSDK_LINK_KEY_REQ_IND			0x01
#define BTSDK_LINK_KEY_NOTIF_IND		0x02
#define BTSDK_AUTHENTICATION_FAIL_IND  	0x03
#define BTSDK_INQUIRY_RESULT_IND  		0x04
#define BTSDK_INQUIRY_COMPLETE_IND 		0x05
#define BTSDK_AUTHORIZATION_IND  		0x06
#define BTSDK_CONNECTION_REQUEST_IND	0x07
#define BTSDK_CONNECTION_COMPLETE_IND	0x08
#define BTSDK_CONNECTION_EVENT_IND  	0x09
#define BTSDK_AUTHORIZATION_ABORT_IND	0x0A
#define BTSDK_VENDOR_EVENT_IND			0x0B
// if: BT2.1 Supported 
#define BTSDK_IOCAP_REQ_IND             0x0C
#define BTSDK_USR_CFM_REQ_IND           0x0D
#define BTSDK_PASSKEY_REQ_IND           0x0E
#define BTSDK_REM_OOBDATA_REQ_IND       0x0F
#define BTSDK_PASSKEY_NOTIF_IND         0x10
#define BTSDK_SIMPLE_PAIR_COMPLETE_IND  0x11
// endif BT2.1 Supported 
#define BTSDK_OBEX_AUTHEN_REQ_IND       0x12
#define BTSDK_SHORTCUT_EVENT_IND	 	0x13

#define MAX_APICBKTYPE_COUNT (BTSDK_SHORTCUT_EVENT_IND+1)//for cbktype value for 0 to 0x12(12)

/* Possible return value of Btsdk_Connection_Request_Ind_Func */
#define BTSDK_CONNREQ_DEFAULT			0x00	/* Upper layer wants HCI to process the connection request. */
#define BTSDK_CONNREQ_PENDING			0x01	/* Upper layer will process (accept or reject) the request later itself. */
#define BTSDK_CONNREQ_ACCEPT			0x02	/* Upper layer wants HCI to accept the connection request. */
#define BTSDK_CONNREQ_REJECT			0x03	/* Upper layer wants HCI to reject the connection request. */

/* OBEX authentication information MAX length */
#define BTSDK_MAX_USERID_LEN            20  /* Shall be the same as MAX_USER_ID_LEN. */
#define BTSDK_MAX_PWD_LEN               64  /* Shall be the same as MAX_PWD_LEN. */

//ini end--------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//sec begin--------------------------------------------------------------------------------------------
//  Security Level, for member 'role' in sec_level BtSdkShortCutPropertyStru
#define BTSDK_SSL_NO_SECURITY					0x00
#define BTSDK_SSL_AUTHENTICATION				0x01
#define BTSDK_SSL_AUTHORIZATION					0x02
#define BTSDK_SSL_ENCRYPTION					0x04
#define BTSDK_DEFAULT_SECURITY					(BTSDK_SSL_AUTHORIZATION | BTSDK_SSL_AUTHENTICATION)

//  Authorization Method 
#define BTSDK_AUTHORIZATION_ACCEPT				0x01
#define BTSDK_AUTHORIZATION_REJECT				0x02
#define BTSDK_AUTHORIZATION_PROMPT				0x03 

// Security Mode
#define BTSDK_SECURITY_LOW						0x01
#define BTSDK_SECURITY_MEDIUM					0x02
#define BTSDK_SECURITY_HIGH						0x03
#define BTSDK_SECURITY_ENCRYPT_MODE1			0x04

//  Authorization Result 
#define BTSDK_AUTHORIZATION_GRANT				0x01
#define BTSDK_AUTHORIZATION_DENY				0x02

/* Trust Level */
#define BTSDK_TRUSTED 							0x01
#define BTSDK_UNTRUSTED							0x00

//sec end--------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//svc begin--------------------------------------------------------------------------------------------
// Possible flags for member 'mask' in _BtSdkSDPSearchPatternStru 
#define BTSDK_SSPM_UUID16					0x0001
#define BTSDK_SSPM_UUID32					0x0002
#define BTSDK_SSPM_UUID128					0x0004

// Possible flags for member 'mask' in _BtSdkRemoteServiceAttrStru 
#define BTSDK_RSAM_SERVICENAME					0x0001
#define BTSDK_RSAM_EXTATTRIBUTES				0x0002

// Possible flags for member 'mask' in _BtSdkLocalServerAttrStru 
#define BTSDK_LSAM_SERVICENAME					0x0001
#define BTSDK_LSAM_SECURITYLEVEL				0x0002
#define BTSDK_LSAM_AUTHORMETHOD					0x0004
#define BTSDK_LSAM_EXTATTRIBUTES				0x0008
#define BTSDK_LSAM_APPPARAM						0x0010

/* Type of Data Element */
#define BTSDK_DETYPE_NULL						0x00
#define BTSDK_DETYPE_UINT						0x01
#define BTSDK_DETYPE_INT						0x02
#define BTSDK_DETYPE_UUID						0x03
#define BTSDK_DETYPE_STRING						0x04
#define BTSDK_DETYPE_BOOL						0x05
#define BTSDK_DETYPE_DESEQ						0x06
#define BTSDK_DETYPE_DEALT						0x07
#define BTSDK_DETYPE_URL						0x08

/* Server Status */
#define BTSDK_SERVER_STARTED					0x01
#define BTSDK_SERVER_STOPPED					0x02
#define BTSDK_SERVER_CONNECTED					0x03


/* Status of Remote Service */
#define BTSDK_RMTSVC_IDLE						0x00
#define BTSDK_RMTSVC_CONNECTED					0x01

#define BTSDK_MAX_SEARCH_PATTERNS				12
//svc end--------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//loc begin--------------------------------------------------------------------------------------------
/* Discovery Mode for Btsdk_SetDiscoveryMode() and Btsdk_GetDiscoveryMode() */
#define BTSDK_GENERAL_DISCOVERABLE				0x01
#define BTSDK_LIMITED_DISCOVERABLE				0x02
#define BTSDK_DISCOVERABLE						BTSDK_GENERAL_DISCOVERABLE
#define BTSDK_CONNECTABLE						0x04
#define BTSDK_PAIRABLE							0x08
#define BTSDK_DISCOVERY_DEFAULT_MODE			(BTSDK_DISCOVERABLE | BTSDK_CONNECTABLE | BTSDK_PAIRABLE)

// Possible values of 'flow_control' member in BtSdkCommSettingStru: 
#define BTSDK_FC_NONE         0x00
#define BTSDK_FC_DTRDSR       0x01
#define BTSDK_FC_RTSCTS       0x02
#define BTSDK_FC_XONXOFF      0x04
//loc end--------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//rmt begin--------------------------------------------------------------------------------------------
/* Possible values for "mask" member of BtSdkRemoteDevicePropertyStru structure. */
#define BTSDK_RDPM_HANDLE			0x0001
#define BTSDK_RDPM_ADDRESS			0x0002
#define BTSDK_RDPM_NAME				0x0004
#define BTSDK_RDPM_CLASS			0x0008
#define BTSDK_RDPM_LMPINFO			0x0010
#define BTSDK_RDPM_LINKKEY			0x0020

/* Default role of local device when creating a new ACL connection. */
#define BTSDK_MASTER_ROLE						0x00
#define BTSDK_SLAVE_ROLE						0x01
#define BTSDK_MASTER_SLAVE						0x02

/* Possible power mode of an ACL link */
#define BTSDK_LPM_ACTIVE_MODE	0x00
#define BTSDK_LPM_HOLD_MODE		0x01
#define BTSDK_LPM_SNIFF_MODE	0x02
#define BTSDK_LPM_PARK_MODE		0x03

// Possible link type
#define BTSDK_SCO_LINK			0x00
#define BTSDK_ACL_LINK			0x01
#define BTSDK_ESCO_LINK			0x02

// Possible values for link policy setting 
#define BTSDK_LP_ENABLE_ROLESWITCH	0x0001
#define BTSDK_LP_ENABLE_HOLDMODE	0x0002
#define BTSDK_LP_ENABLE_SNIFFMODE	0x0004
#define BTSDK_LP_ENABLE_PARKMODE	0x0008
//rmt end--------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//shc begin--------------------------------------------------------------------------------------------
//  Possible flags for member 'mask' in _BtSdkShortCutPropertyStru 
#define BTSDK_SCPM_SHCNAME					0x0001
#define BTSDK_SCPM_DEVNAME					0x0002
#define BTSDK_SCPM_SVCNAME					0x0004
#define BTSDK_SCPM_DEVCLASS					0x0008
#define BTSDK_SCPM_ISDEFAULT				0x0010
#define BTSDK_SCPM_SECLEVEL					0x0020
#define BTSDK_SCPM_SHCATTR					0x0040
#define BTSDK_SCPM_ALL						0x0FFF

//  Possible flags for member 'mask' in _BtSdkShortCutPropertyStru 
#define BTSDK_SCPM_SHCNAME					0x0001
#define BTSDK_SCPM_DEVNAME					0x0002
#define BTSDK_SCPM_SVCNAME					0x0004
#define BTSDK_SCPM_DEVCLASS					0x0008
#define BTSDK_SCPM_ISDEFAULT				0x0010
#define BTSDK_SCPM_SECLEVEL					0x0020
#define BTSDK_SCPM_SHCATTR					0x0040
#define BTSDK_SCPM_ALL						0x0FFF
//shc end--------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//conn begin--------------------------------------------------------------------------------------------
// Possible roles for member 'role' in _BtSdkConnectionPropertyStru
#define BTSDK_CONNROLE_INITIATOR				0x2
#define BTSDK_CONNROLE_ACCEPTOR					0x1
// Definitions for Compatibility 
#define BTSDK_CONNROLE_CLIENT					0x2
#define BTSDK_CONNROLE_SERVER					0x1

// feature for BtSdkBIPImgConnParamStru
#define BTSDK_IMG_FEA_PUSH		    (1<<0x00)
#define BTSDK_IMG_FEA_PUSH_STORE	(1<<0x01)
#define BTSDK_IMG_FEA_PUSH_PRINT	(1<<0x02)
#define BTSDK_IMG_FEA_PUSH_DISP	    (1<<0x03)
#define BTSDK_IMG_FEA_PULL		    (1<<0x04)
#define BTSDK_IMG_FEA_ADV_PRINTING	(1<<0x05)
#define BTSDK_IMG_FEA_AUTO_ARCHIVE	(1<<0x06)
#define BTSDK_IMG_FEA_REM_CAMERA	(1<<0x07)
#define BTSDK_IMG_FEA_REM_DISPLAY	(1<<0x08)
//conn end--------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//ui begin--------------------------------------------------------------------------------------------
// definitions  for reason why to reject the connection request
//#define  ER_UNKNOWN_HCI_CMD            0x01            // Unknown HCI Command
//#define  ER_NO_CONNECT                 0x02            // No Connection
//#define  ER_HARDWARE_FAILURE           0x03            // Hardware Failure 
//#define  ER_PAGE_TIMEOUT               0x04            // Page Timeout
//#define  ER_AUTH_FAILURE               0x05            // Authentication Failure
//#define  ER_KEY_MISSING		           0x06            // Key Missing
//#define  ER_MEMORY_FULL                0x07            // Memory Full
//#define  ER_CONN_TIMEOUT               0x08            // Connection Timeout
//#define  ER_MAX_NUM_CONN               0x09            // Max Number Of Connections
//#define  ER_Max_NUM_SCO_CONN           0x0A            // Max Number Of SCO Connections To A Device
//#define  ER_ACL_CONN_EXIST             0x0B            // ACL connection already exists
//#define  ER_COMMAND_DISALLOW           0x0C            // Command Disallowed
//#define  ER_HOST_REJ_lIMI_RESOURCE     0x0D            // Host Rejected due to limited resources
//#define  ER_HOST_REJ_SECU_REASON       0x0E            // Host Rejected due to security reasons
//#define  ER_HOST_REJ_REMOTE_PERS_DEV   0x0F            // Host Rejected due to remote device is only a personal device 
//#define  ER_HOST_TIMEOUT               0x10            // Host Timeout
//#define  ER_UNSUPP_FEAT_OR_PARA_VALUE  0x11            // Unsupported Feature or Parameter Value
//#define  ER_INVALID_HCI_COMM_PARA      0x12            // Invalid HCI Command Parameters
//#define  ER_OTHER_END_USER_END_CONN    0x13            // Other End Terminated Connection: User Ended Connection
//#define  ER_OTHER_END_LOW_RESOURCE     0x14            // Other End Terminated Connection: Low Resources
//#define  ER_OTHER_END_POWER_OFF        0x15            // Other End Terminated Connection: About to Power Off
#define    ER_LOCAL_HOST_TERMI_CONN        0x16            // Connection Terminated by Local Host
//#define  ER_REPEATED_ATTEMPT           0x17            //  Repeated Attempts
//#define  ER_PAIRING_NOT_ALLOW          0x18            // Pairing Not Allowed
//#define  ER_UNKNOWN_LMP_PDU            0x19            // Unknown LMP PDU
//#define  ER_UNSUPP_REMOTE_FEATURE      0x1A            // Unsupported Remote Feature
//#define  ER_SCO_OFF_REJECT             0x1B            // SCO Offset Rejected
//#define  ER_SCO_INTERVAL_REJECT        0x1C            // SCO Interval Rejected
//#define  ER_SCO_AIR_MODE_REJECT        0x1D            // SCO Air Mode Rejected
//#define  ER_INVALID_LMP_PARA           0x1E            // Invalid LMP Parameters
//#define  ER_UNSPEC_ERROR               0x1F            // Unspecified Error
//#define  ER_UNSUPP_LMP_PARA_VALUE      0x20            // Unsupported LMP Parameter Value
//#define  ER_ROLE_CHANGE_NOT_ALLOW      0x21            // Role Change Not Allowed
//#define  ER_LMP_RESPONSE_TIMEOUT       0x22            // LMP Response Timeout
//#define  ER_LMP_ERROR_TRANS_COLLISION  0x23            // LMP Error Transaction Collision
//#define  ER_LMP_PDU_NOT_ALLOW          0x24            // LMP PDU Not Allowed
//#define  ER_ENCRYPT_MODE_NOT_ACCEPT    0x25            // Encryption Mode Not Acceptable
//#define  ER_UNIT_KEY_USED              0x26            // Unit Key Used
//#define  ER_QOS_NOT_SUPPORT            0x27            // QoS is Not Supported
//#define  ER_INSTANT_PASSED             0x28            // Instant Passed
//#define  ER_UNIT_KEY_PAIR_NOT_SUPPORT  0x29            // Pairing with Unit Key Not Supported*/
//ui end  --------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------



//-----------------------------------------------------------------------------------------------------
//HF AG/HF begin--------------------------------------------------------------------------------------------
/* HSP/HFP AG specific event. */
#define BTSDK_APP_EV_AGAP_BASE 							 0x900
enum BTSDK_HFP_APP_EventCodeEnum {
	/* HFP_SetState Callback to Application Event Code */
	/* SLC - Both AG and HF */
	BTSDK_HFP_EV_SLC_ESTABLISHED_IND = BTSDK_APP_EV_AGAP_BASE + 0x01,	/* HFP Service Level connection established. Parameter: Btsdk_HFP_ConnInfoStru */
	BTSDK_HFP_EV_SLC_RELEASED_IND,   	            /* SPP connection released. Parameter: Btsdk_HFP_ConnInfoStru */

	/* SCO - Both AG and HF  */
	BTSDK_HFP_EV_AUDIO_CONN_ESTABLISHED_IND,		/* SCO audio connection established */
	BTSDK_HFP_EV_AUDIO_CONN_RELEASED_IND,			/* SCO audio connection released */

	/* Status Changed Indication */
	BTSDK_HFP_EV_STANDBY_IND,      					/* STANDBY Menu, the incoming call or outgoing call or ongoing call is canceled  */
	BTSDK_HFP_EV_ONGOINGCALL_IND,					/* ONGOING-CALL Menu, a call (incoming call or outgoing call) is established (ongoing) */
	BTSDK_HFP_EV_RINGING_IND,						/* RINGING Menu, a call is incoming. Parameter: BTBOOL - in-band ring tone or not.   */
	BTSDK_HFP_EV_OUTGOINGCALL_IND,					/* OUTGOING-CALL Menu, an outgoing call is being established, 3Way in Guideline P91 */
	BTSDK_HFP_EV_CALLHELD_IND,						/* BTRH-HOLD Menu, +BTRH:0, AT+BTRH=0, incoming call is put on hold */
	BTSDK_HFP_EV_CALL_WAITING_IND,					/* Call Waiting Menu, +CCWA, When Call=Active, call waiting notification. Parameter: Btsdk_HFP_PhoneInfoStru */
	BTSDK_HFP_EV_TBUSY_IND,							/* GSM Network Remote Busy, TBusy Timer Activated */

	/* AG & HF APP General Event Indication */
	BTSDK_HFP_EV_GENERATE_INBAND_RINGTONE_IND,		/* AG Only, Generate the in-band ring tone */
	BTSDK_HFP_EV_TERMINATE_LOCAL_RINGTONE_IND,		/* Terminate local generated ring tone or the in-band ring tone */
	BTSDK_HFP_EV_VOICE_RECOGN_ACTIVATED_IND,		/* +BVRA:1, voice recognition activated indication or HF request to start voice recognition procedure */
	BTSDK_HFP_EV_VOICE_RECOGN_DEACTIVATED_IND,		/* +BVRA:0, voice recognition deactivated indication or requests AG to deactivate the voice recognition procedure */
	BTSDK_HFP_EV_NETWORK_AVAILABLE_IND,				/* +CIEV:<service><value>, cellular network is available */
	BTSDK_HFP_EV_NETWORK_UNAVAILABLE_IND,			/* +CIEV:<service><value>, cellular network is unavailable */
	BTSDK_HFP_EV_ROAMING_RESET_IND,					/* +CIEV:<roam><value>, roaming is not active */
	BTSDK_HFP_EV_ROAMING_ACTIVE_IND,				/* +CIEV:<roam><value>, a roaming is active */
	BTSDK_HFP_EV_SIGNAL_STRENGTH_IND,				/* +CIEV:<signal><value>, signal strength indication. Parameter: BTUINT8 - indicator value */	
	BTSDK_HFP_EV_BATTERY_CHARGE_IND,				/* +CIEV:<battchg><value>, battery charge indication. Parameter: BTUINT8 - indicator value  */
	BTSDK_HFP_EV_CHLDHELD_ACTIVATED_IND,			/* +CIEV:<callheld><1>, Call on CHLD Held to be or has been actived. */
	BTSDK_HFP_EV_CHLDHELD_RELEASED_IND,				/* +CIEV:<callheld><0>, Call on CHLD Held to be or has been released. */	
	BTSDK_HFP_EV_MICVOL_CHANGED_IND,				/* +VGM, AT+VGM, microphone volume changed indication */
	BTSDK_HFP_EV_SPKVOL_CHANGED_IND,				/* +VGS, AT+VGS, speaker volume changed indication */

	/* OK and Error Code - HF only */
	BTSDK_HFP_EV_ATCMD_RESULT,						/* HF Received OK, Error/+CME ERROR from AG or Wait for AG Response Timeout. Parameter: Btsdk_HFP_ATCmdResultStru */

	/* To HF APP, Call Related, AG Send information to HF */
	BTSDK_HFP_EV_CLIP_IND,							/* +CLIP, Phone Number Indication. Parameter: Btsdk_HFP_PhoneInfoStru */
	BTSDK_HFP_EV_CURRENT_CALLS_IND,					/* +CLCC, the current calls of AG. Parameter: Btsdk_HFP_CLCCInfoStru */
	BTSDK_HFP_EV_NETWORK_OPERATOR_IND,				/* +COPS, the current network operator name of AG. Parameter: Btsdk_HFP_COPSInfoStru */
	BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND,				/* +CNUM, the subscriber number of AG. Parameter: Btsdk_HFP_PhoneInfoStru */
	BTSDK_HFP_EV_VOICETAG_PHONE_NUM_IND,			/* +BINP, AG inputted phone number for voice-tag; requests AG to input a phone number for the voice-tag at the HF side. Parameter: Btsdk_HFP_PhoneInfoStru */

	/* AG APP, HF Request or Indicate AG */
	BTSDK_HFP_EV_CURRENT_CALLS_REQ,					/* AT+CLCC, query the list of current calls in AG. */
	BTSDK_HFP_EV_NETWORK_OPERATOR_FORMAT_REQ,		/* AT+COPS=3,0, indicate app the network operator name should be set to long alphanumeric */
	BTSDK_HFP_EV_NETWORK_OPERATOR_REQ,				/* AT+COPS?, requests AG to respond with +COPS response indicating the currently selected operator */
	BTSDK_HFP_EV_SUBSCRIBER_NUMBER_REQ,				/* AT+CNUM, query the AG subscriber number information. */
	BTSDK_HFP_EV_VOICETAG_PHONE_NUM_REQ,			/* AT+BINP, requests AG to input a phone number for the voice-tag at the HF */
	BTSDK_HFP_EV_CUR_INDICATOR_VAL_REQ,				/* AT+CIND?, get the current indicator during the service level connection initialization procedure */
	BTSDK_HFP_EV_HF_DIAL_REQ,						/* ATD, instructs AG to dial the specific phone number. Parameter: (HFP only) BTUINT8* - phone number */
	BTSDK_HFP_EV_HF_MEM_DIAL_REQ,					/* ATD>, instructs AG to dial the phone number indexed by the specific memory location of SIM card. Parameter: BTUINT8* - memory location */
	BTSDK_HFP_EV_HF_LASTNUM_REDIAL_REQ,				/* AT+BLDN, instructs AG to redial the last dialed phone number */
	BTSDK_HFP_EV_MANUFACTURER_REQ,					/* AT+CGMI, requests AG to respond with the Manufacturer ID */
	BTSDK_HFP_EV_MODEL_REQ,							/* AT+CGMM, requests AG to respond with the Model ID */
	BTSDK_HFP_EV_NREC_DISABLE_REQ,					/* AT+NREC=0, requests AG to disable NREC function */
	BTSDK_HFP_EV_DTMF_REQ,							/* AT+VTS, instructs AG to transmit the specific DTMF code. Parameter: BTUINT8 - DTMF code */
	BTSDK_HFP_EV_ANSWER_CALL_REQ,					/* inform AG app to answer the call. Parameter: BTUINT8 - One of BTSDK_HFP_TYPE_INCOMING_CALL, BTSDK_HFP_TYPE_HELDINCOMING_CALL. */	
	BTSDK_HFP_EV_CANCEL_CALL_REQ,					/* inform AG app to cancel the call. Parameter: BTUINT8 - One of BTSDK_HFP_TYPE_ALL_CALLS, BTSDK_HFP_TYPE_INCOMING_CALL, 
														BTSDK_HFP_TYPE_HELDINCOMING_CALL, BTSDK_HFP_TYPE_OUTGOING_CALL, BTSDK_HFP_TYPE_ONGOING_CALL. */	
	BTSDK_HFP_EV_HOLD_CALL_REQ,						/* inform AG app to hold the incoming call (AT+BTRH=0) */

	/* AG APP, 3-Way Calling */
	BTSDK_HFP_EV_REJECTWAITINGCALL_REQ,				/* AT+CHLD=0, Release all held calls or reject waiting call. */	
	BTSDK_HFP_EV_ACPTWAIT_RELEASEACTIVE_REQ,		/* AT+CHLD=1, Accept the held or waiting call and release all avtive calls. Parameter: BTUINT8 - value of <idx>*/
	BTSDK_HFP_EV_HOLDACTIVECALL_REQ,					/* AT+CHLD=2, Held Specified Active Call.  Parameter: BTUINT8 - value of <idx>*/
	BTSDK_HFP_EV_ADD_ONEHELDCALL_2ACTIVE_REQ,		/* AT+CHLD=3, Add One CHLD Held Call to Active Call. */
	BTSDK_HFP_EV_LEAVE3WAYCALLING_REQ,				/* AT+CHLD=4, Leave The 3-Way Calling. */

	/* Extended */
	BTSDK_HFP_EV_EXTEND_CMD_IND,					/* indicate app extend command received. Parameter: BTUINT8* - Full extended AT command or result code. */
	BTSDK_HFP_EV_PRE_SCO_CONNECTION_IND,			/* indicate app to create SCO connection. Parameter: Btsdk_AGAP_PreSCOConnIndStru. */
	BTSDK_HFP_EV_SPP_ESTABLISHED_IND,				/* SPP connection created. Parameter: Btsdk_HFP_ConnInfoStru. added 2008-7-3 */
	BTSDK_HFP_EV_HF_MANUFACTURERID_IND,				/* ManufacturerID indication. Parameter: BTUINT8* - Manufacturer ID of the AG device, a null-terminated ASCII string. */
	BTSDK_HFP_EV_HF_MODELID_IND,					/* ModelID indication.  Parameter: BTUINT8* - Model ID of the AG device, a null-terminated ASCII string. */

	BTSDK_HFP_EV_HF_INCOMINGCALL_IND,				/*INCOMING-CALL Menu,  a call is incoming*/
	BTSDK_HFP_EV_HF_SPECIAL_IND						/*SPECIAL Menu,  call,callsetup,or callheld indicator receive,  Parameter: BTUINT8* - indicator value(10,11;20,21,22,23;30,31,32;)*/
};

/* Spec P74, P79, P80, AT+BRSF= and +BRSF: and SDP(only 0~4 bit) Use this BRSF Features */
/* BRSF feature mask ID for HF */
#define BTSDK_HF_BRSF_NREC             			0x00000001 /* EC and/or NR function */
#define BTSDK_HF_BRSF_3WAYCALL         			0x00000002 /* Call waiting and 3-way calling */
#define BTSDK_HF_BRSF_CLIP             			0x00000004 /* CLI presentation capability */
#define BTSDK_HF_BRSF_BVRA             			0x00000008 /* Voice recognition activation */
#define BTSDK_HF_BRSF_RMTVOLCTRL       			0x00000010 /* Remote volume control */
#define BTSDK_HF_BRSF_ENHANCED_CALLSTATUS       0x00000020 /* Enhanced call status */
#define BTSDK_HF_BRSF_ENHANCED_CALLCONTROL     	0x00000040 /* Enhanced call control */
#define BTSDK_HF_BRSF_ALL						0x0000007F /* Support all the upper features */ 

/* BRSF feature mask ID for AG, defined in spec */
#define BTSDK_AG_BRSF_3WAYCALL         			0x00000001 /* Three-way calling */
#define BTSDK_AG_BRSF_NREC             			0x00000002 /* EC and/or NR function */
#define BTSDK_AG_BRSF_BVRA             			0x00000004 /* Voice recognition function */
#define BTSDK_AG_BRSF_INBANDRING       			0x00000008 /* In-band ring tone capability */
#define BTSDK_AG_BRSF_BINP             			0x00000010 /* Attach a number to a voice tag */
#define BTSDK_AG_BRSF_REJECT_CALL          		0x00000020 /* Ability to reject a call */
#define BTSDK_AG_BRSF_ENHANCED_CALLSTATUS      	0x00000040 /* Enhanced call status */
#define BTSDK_AG_BRSF_ENHANCED_CALLCONTROL      0x00000080 /* Enhanced call control */
#define BTSDK_AG_BRSF_EXTENDED_ERRORRESULT     	0x00000100 /* Extended Error Result Codes */
#define BTSDK_AG_BRSF_ALL						0x000001FF /* Support all the upper features */ 

/* Additional feature masks specify the AG supported indicators. Refer to HFP_FEA_AG_XXX macros. */
#define BTSDK_AG_IND_SERVICE		0x00010000   /* "service" indicator supported. */
#define BTSDK_AG_IND_CALL			0x00020000   /* "call" indicator supported. */
#define BTSDK_AG_IND_CALLSETUP		0x00040000   /* "callsetup" indicator supported. */
#define BTSDK_AG_IND_CALLHELD		0x00080000   /* "callheld" indicator supported. */
#define BTSDK_AG_IND_SIGNAL			0x00100000   /* "signal" indicator supported. */
#define BTSDK_AG_IND_ROAM			0x00200000   /* "roam" indicator supported. */
#define BTSDK_AG_IND_BATTCHG		0x00400000   /* "battchg" indicator supported. */

/* HF Send AT Commands to AG, AG Receive Commands, Command Code Group Mask */
#define BTSDK_HFP_CMD_GROUP1           0x8000 		/* AT Command will response directly by OK */
#define BTSDK_HFP_CMD_GROUP2           0x4000 		/* AT Command will response result 1st, then OK */
#define BTSDK_HFP_CMD_PARAM            0x1000 		/* AT Command param exist or not bit-mask */
#define BTSDK_HFP_CMD_GROUPMASK        (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_GROUP2)
/* HF AT Commands Group for SLC Establish Procedure */
#define BTSDK_HFP_CMD_BRSF             (BTSDK_HFP_CMD_GROUP2 | BTSDK_HFP_CMD_PARAM | 0x10)	/* AT+BRSF=<value> */
#define BTSDK_HFP_CMD_CIND_T  	       (BTSDK_HFP_CMD_GROUP2 | 0x11)	   					/* AT+CIND=? */
#define BTSDK_HFP_CMD_CIND_R  	       (BTSDK_HFP_CMD_GROUP2 | 0x12)	   					/* AT+CIND? */
#define BTSDK_HFP_CMD_CMER             (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_PARAM | 0x20)	/* AT+CMER=3,0,0,1 */
#define BTSDK_HFP_CMD_CHLD_T           (BTSDK_HFP_CMD_GROUP2 | 0x13)						/* AT+CHLD=? */
/* HF AT Commands Group1 */
#define BTSDK_HFP_CMD_COPS_SET_FORMAT  (BTSDK_HFP_CMD_GROUP1 | 0x01)     					/* AT+COPS=3,0 Set name format to long alphanumeric */
#define BTSDK_HFP_CMD_CCWA_ACTIVATE    (BTSDK_HFP_CMD_GROUP1 | 0x02)     					/* AT+CCWA=1 Call waiting notification Activation */
#define BTSDK_HFP_CMD_CMEE  		   (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_PARAM | 0x03)  /* AT+CMEE= Enable the use of result code +CME ERROR: <err> */
#define BTSDK_HFP_CMD_CLIP             (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_PARAM | 0x04) 	/* AT+CLIP= Calling Line Identification Presentation */
#define BTSDK_HFP_CMD_BVRA_ENABLE      (BTSDK_HFP_CMD_GROUP1 | 0x05)     					/* AT+BVRA=1 Voice Recognition Activation */
#define BTSDK_HFP_CMD_BVRA_DISABLE     (BTSDK_HFP_CMD_GROUP1 | 0x06)     					/* AT+BVRA=0 Voice Recognition Deactivation */
#define BTSDK_HFP_CMD_VTS              (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_PARAM | 0x07)	/* AT+VTS= DTMF code */
#define BTSDK_HFP_CMD_VGS              (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_PARAM | 0x08)  /* AT+VGS= Set speaker volume */
#define BTSDK_HFP_CMD_VGM 	           (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_PARAM | 0x09)  /* AT+VGM= Set microphone volume */
#define BTSDK_HFP_CMD_NREC             (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_PARAM | 0x0a)  /* AT+NREC= Noise Reduction & Echo Canceling */
#define BTSDK_HFP_CMD_CHLD_0  	       (BTSDK_HFP_CMD_GROUP1 | 0x0b)						/* AT+CHLD=0 Held Call Release */
#define BTSDK_HFP_CMD_CHLD_1 		   (BTSDK_HFP_CMD_GROUP1 | 0x0c)						/* AT+CHLD=1 Release Specified Active Call */
#define BTSDK_HFP_CMD_CHLD_2 		   (BTSDK_HFP_CMD_GROUP1 | 0x0d)						/* AT+CHLD=2 Call Held or Active/Held Position Swap */
#define BTSDK_HFP_CMD_CHLD_3 		   (BTSDK_HFP_CMD_GROUP1 | 0x0e)						/* AT+CHLD=3 Adds a held call to the conversation */
#define BTSDK_HFP_CMD_CHLD_4 		   (BTSDK_HFP_CMD_GROUP1 | 0x0f)						/* AT+CHLD=4 Connects the two calls and disconnects the subscriber from both calls */
#define BTSDK_HFP_CMD_CANCELCALL       (BTSDK_HFP_CMD_GROUP1 | 0x10)     					/* AT+CHUP Reject the Incoming Call or Terminate the Outgoing Call or Release the Ongoing Call */
#define BTSDK_HFP_CMD_ANSWERCALL       (BTSDK_HFP_CMD_GROUP1 | 0x11)     					/* ATA, Answer the Incoming Call */
#define BTSDK_HFP_CMD_DIAL             (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_PARAM | 0x12)  /* ATD, Dial the specific phone number */
#define BTSDK_HFP_CMD_MEMDIAL          (BTSDK_HFP_CMD_GROUP1 | BTSDK_HFP_CMD_PARAM | 0x13)  /* ATD>, Dial the phone number indexed by the specific memory location */
#define BTSDK_HFP_CMD_BLDN             (BTSDK_HFP_CMD_GROUP1 | 0x14)     					/* AT+BLDN, Redial the Last Dialed Phone Number */
#define BTSDK_HFP_CMD_CKPD             (BTSDK_HFP_CMD_GROUP1 | 0x15)						/* AT+CKPD=200, It is used for Headset  */
#define BTSDK_HFP_CMD_CKPD_REJ_CALL    (BTSDK_HFP_CMD_GROUP1 | 0x16)  					    /* AT+CKPD="e", It is used for Headset, used for reject call, it is not defined by HSP */
#define BTSDK_HFP_CMD_BIA         	   (BTSDK_HFP_CMD_GROUP1 | 0x17) 						/* AT+BIA=x,x,x,x,x Indicators Activation and Deactivation */
/* HF AT Commands Group2 */
#define BTSDK_HFP_CMD_COPS_READ  	   (BTSDK_HFP_CMD_GROUP2 | 0x01)     					/* AT+COPS? Find the currently selected operator */
#define BTSDK_HFP_CMD_BINP             (BTSDK_HFP_CMD_GROUP2 | 0x02)     					/* AT+BINP=1 Bluetooth Input (attach a phone number to a voice tag) */
#define BTSDK_HFP_CMD_BTRH_QUERY	   (BTSDK_HFP_CMD_GROUP2 | 0x03)     					/* AT+BTRH? Query the status of the Rsponse and Hold state of the AG. */
#define BTSDK_HFP_CMD_BTRH    		   (BTSDK_HFP_CMD_GROUP2 | BTSDK_HFP_CMD_PARAM | 0x04) 	/* AT+BTRH=0 Put the incoming call on Hold. 1 Accept, 2 Reject */
#define BTSDK_HFP_CMD_CNUM			   (BTSDK_HFP_CMD_GROUP2 | 0x07)     					/* AT+CNUM query the AG subscriber number information. */
#define BTSDK_HFP_CMD_CLCC			   (BTSDK_HFP_CMD_GROUP2 | 0x08)     					/* AT+CLCC query the list of current calls in AG. */
#define BTSDK_HFP_CMD_CGMI       	   (BTSDK_HFP_CMD_GROUP2 | 0x09)        				/* AT+CGMI Get the AG Manufacturer ID */
#define BTSDK_HFP_CMD_CGMM             (BTSDK_HFP_CMD_GROUP2 | 0x0a)    					/* AT+CGMM Get the AG Model ID */

#define BTSDK_HFP_EXTEND_CMD           0x88    /* Extended at-command, Both AG / HF use this */

/*-----------------------------------------------------------------------------
/* 					 CME Error Code and Standard Error Code for APP			 */
/*---------------------------------------------------------------------------*/
/* This CME ERROR Code is only for APP Reference. More Code reference to GSM Spec. */
#define BTSDK_HFP_CMEERR_AGFAILURE						0  /* +CME ERROR:0 - AG failure */
#define BTSDK_HFP_CMEERR_NOCONN2PHONE					1  /* +CME ERROR:1 - no connection to phone */
#define BTSDK_HFP_CMEERR_OPERATION_NOTALLOWED			3  /* +CME ERROR:3 - operation not allowed */
#define BTSDK_HFP_CMEERR_OPERATION_NOTSUPPORTED			4  /* +CME ERROR:4 - operation not supported */
#define BTSDK_HFP_CMEERR_PHSIMPIN_REQUIRED				5  /* +CME ERROR:5 - PH-SIM PIN required */
#define BTSDK_HFP_CMEERR_SIMNOT_INSERTED				10 /* +CME ERROR:10 - SIM not inserted */
#define BTSDK_HFP_CMEERR_SIMPIN_REQUIRED				11 /* +CME ERROR:11 - SIM PIN required */
#define BTSDK_HFP_CMEERR_SIMPUK_REQUIRED				12 /* +CME ERROR:12 - SIM PUK required */
#define BTSDK_HFP_CMEERR_SIM_FAILURE					13 /* +CME ERROR:13 - SIM failure */
#define BTSDK_HFP_CMEERR_SIM_BUSY						14 /* +CME ERROR:14 - SIM busy */
#define BTSDK_HFP_CMEERR_INCORRECT_PASSWORD				16 /* +CME ERROR:16 - incorrect password */
#define BTSDK_HFP_CMEERR_SIMPIN2_REQUIRED				17 /* +CME ERROR:17 - SIM PIN2 required */
#define BTSDK_HFP_CMEERR_SIMPUK2_REQUIRED				18 /* +CME ERROR:18 - SIM PUK2 required */
#define BTSDK_HFP_CMEERR_MEMORY_FULL					20 /* +CME ERROR:20 - memory full */
#define BTSDK_HFP_CMEERR_INVALID_INDEX					21 /* +CME ERROR:21 - invalid index */
#define BTSDK_HFP_CMEERR_MEMORY_FAILURE					23 /* +CME ERROR:23 - memory failure */
#define BTSDK_HFP_CMEERR_TEXTSTRING_TOOLONG				24 /* +CME ERROR:24 - text string too long */
#define BTSDK_HFP_CMEERR_INVALID_CHAR_INTEXTSTRING 	    25 /* +CME ERROR:25 - invalid characters in text string */
#define BTSDK_HFP_CMEERR_DIAL_STRING_TOOLONG			26 /* +CME ERROR:26 - dial string too long */
#define BTSDK_HFP_CMEERR_INVALID_CHAR_INDIALSTRING		27 /* +CME ERROR:27 - invalid characters in dial string */
#define BTSDK_HFP_CMEERR_NETWORK_NOSERVICE				30 /* +CME ERROR:30 - no network service */
#define BTSDK_HFP_CMEERR_NETWORK_TIMEOUT				31 /* +CME ERROR:31 - network timeout */
#define BTSDK_HFP_CMEERR_EMERGENCYCALL_ONLY				32 /* +CME ERROR:32 - Network not allowed, emergency calls only */

/* APP specific error code. */
#define BTSDK_HFP_APPERR_TIMEOUT						200 /* Wait for response timeout */

/* Standard error result code. */
#define BTSDK_HFP_STDERR_ERROR							201 /* result code: ERROR */
#define BTSDK_HFP_STDRR_NOCARRIER						202 /* result code: NO CARRIER */
#define BTSDK_HFP_STDERR_BUSY							203 /* result code: BUSY */
#define BTSDK_HFP_STDERR_NOANSWER						204 /* result code: NO ANSWER */
#define BTSDK_HFP_STDERR_DELAYED						205 /* result code: DELAYED */
#define BTSDK_HFP_STDERR_BLACKLISTED					206 /* result code: BLACKLISTED */
#define BTSDK_HFP_OK								    255 /* result code: OK */
//HF AG/HF end  ------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//FTP begin--------------------------------------------------------------------------------------------
// FTP specific operation , according to the struct description, this value should be define
#define BTSDK_APP_EV_FTP_BASE					0x300
#define BTSDK_APP_EV_FTP_PUT					(BTSDK_APP_EV_FTP_BASE+0)
#define BTSDK_APP_EV_FTP_GET				    (BTSDK_APP_EV_FTP_BASE+1)
#define BTSDK_APP_EV_FTP_BROWSE					(BTSDK_APP_EV_FTP_BASE+2)
#define BTSDK_APP_EV_FTP_DEL_FILE				(BTSDK_APP_EV_FTP_BASE+3)
#define BTSDK_APP_EV_FTP_DEL_FOLDER				(BTSDK_APP_EV_FTP_BASE+4)	
#define BTSDK_APP_EV_FTP_CREATE					(BTSDK_APP_EV_FTP_BASE+5)
#define BTSDK_APP_EV_FTP_SET_CUR				(BTSDK_APP_EV_FTP_BASE+6)
#define BTSDK_APP_EV_FTP_SET_ROOT				(BTSDK_APP_EV_FTP_BASE+7)
#define BTSDK_APP_EV_FTP_ABORT					 (BTSDK_APP_EV_FTP_BASE+8)
#define BTSDK_APP_EV_FTP_BACK					(BTSDK_APP_EV_FTP_BASE+9)
#define BTSDK_APP_EV_FTP_CONN					(BTSDK_APP_EV_FTP_BASE+10)

// Possible values for member 'desired_access' in _BtSdkLocalFTPServerAttrStru 
#define BTSDK_FTPDA_NOACCESS					0x0000
#define BTSDK_FTPDA_READWRITE					0x0001
#define BTSDK_FTPDA_READONLY					0x0002

// FTP Operation Code
#define FTP_OP_REFRESH	0
#define FTP_OP_UPDIR	1
#define FTP_OP_NEXT		2
//FTP end  --------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//CTP/ICP begin  --------------------------------------------------------------------------------------
// CTPICP specific event
#define BTSDK_APP_EV_CTPICP_BASE 				0x800
#define BTSDK_APP_EV_CTPICP_ATTACH				(BTSDK_APP_EV_CTPICP_BASE + 1)
#define BTSDK_APP_EV_CTPICP_DETACH				(BTSDK_APP_EV_CTPICP_BASE + 2)
#define BTSDK_APP_EV_CTPICP_CTPRING				(BTSDK_APP_EV_CTPICP_BASE + 3)
#define BTSDK_APP_EV_CTPICP_CTPREGRING			(BTSDK_APP_EV_CTPICP_BASE + 4)
#define BTSDK_APP_EV_CTPICP_ICPRING				(BTSDK_APP_EV_CTPICP_BASE + 5)
#define BTSDK_APP_EV_CTPICP_HANGUP				(BTSDK_APP_EV_CTPICP_BASE + 6)
#define BTSDK_APP_EV_CTPICP_ACTIVE				(BTSDK_APP_EV_CTPICP_BASE + 7)
#define BTSDK_APP_EV_CTPICP_CCID				(BTSDK_APP_EV_CTPICP_BASE + 8)
#define BTSDK_APP_EV_CTPICP_TLADDED				(BTSDK_APP_EV_CTPICP_BASE + 9)
//CTP/ICP end------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//A2DP begin------------------------------------------------------------------------------------------
// MPEG12 capabilities 
// chnl_mode for BTSDK_A2DP_SBCUserInfoStru and BTSDK_A2DP_MPEG12UserInfoStru
#define BTSDK_A2DP_MPEG12_JOINTSTEREO		0x01
#define BTSDK_A2DP_MPEG12_STEREO      		0x02
#define BTSDK_A2DP_MPEG12_DUAL      		0x04
#define BTSDK_A2DP_MPEG12_MONO     			0x08

// sample_frequency for BTSDK_A2DP_SBCUserInfoStru and BTSDK_A2DP_MPEG12UserInfoStru 
#define BTSDK_A2DP_MPEG12_FS48000 			0x01
#define BTSDK_A2DP_MPEG12_FS44100 			0x02
#define BTSDK_A2DP_MPEG12_FS32000 			0x04
#define BTSDK_A2DP_MPEG12_FS24000 			0x08
#define BTSDK_A2DP_MPEG12_FS22050 			0x10
#define BTSDK_A2DP_MPEG12_FS16000 			0x20

// alloc_method  for BTSDK_A2DP_SBCUserInfoStru
#define BTSDK_A2DP_SBC_LOUDNESS			0x01
#define BTSDK_A2DP_SBC_SNR				0x02

// subband for BTSDK_A2DP_SBCUserInfoStru
#define BTSDK_A2DP_SBC_SUBBAND_8		0x01
#define BTSDK_A2DP_SBC_SUBBAND_4		0x02

// block_length for BTSDK_A2DP_SBCUserInfoStru
#define BTSDK_A2DP_SBC_BLOCK_16			0x01
#define BTSDK_A2DP_SBC_BLOCK_12			0x02
#define BTSDK_A2DP_SBC_BLOCK_8			0x04
#define BTSDK_A2DP_SBC_BLOCK_4			0x08

// min_bitpool, max_bitpool for BTSDK_A2DP_SBCUserInfoStru
#define BTSDK_A2DP_SBC_MINBITPOOL		0x01
#define BTSDK_A2DP_SBC_MAXBITPOOL		0x02

#define BTSDK_A2DP_SBC_BITPOOL 			0x30

// crc for BTSDK_A2DP_MPEG12UserInfoStru
#define BTSDK_A2DP_MPEG12_CRCSUPPORT		0x01

//layer for BTSDK_A2DP_MPEG12UserInfoStru
#define BTSDK_A2DP_MPEG12_LAYER1			0x04
#define BTSDK_A2DP_MPEG12_LAYER2			0x02
#define BTSDK_A2DP_MPEG12_LAYER3 			0x01

// mpf for BTSDK_A2DP_MPEG12UserInfoStru
#define BTSDK_A2DP_MPEG12_MPF2SUPPORT	    0x01

// bitrate for BTSDK_A2DP_MPEG12UserInfoStru and BTSDK_A2DP_MPEG24UserInfoStru
#define BTSDK_A2DP_MPEG12_BITRATE0000		0x0001  // free 
#define BTSDK_A2DP_MPEG12_BITRATE0001		0x0002  // 32Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE0010		0x0004  // 40Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE0011		0x0008  // 48Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE0100		0x0010  // 56Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE0101		0x0020  // 64Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE0110		0x0040  // 80Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE0111		0x0080  // 96Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE1000		0x0100  // 112Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE1001		0x0200  // 128Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE1010		0x0400  // 160Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE1011		0x0800  // 192Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE1100		0x1000  // 224Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE1101		0x2000  // 256Kbps for MPEG-1 layer 3 
#define BTSDK_A2DP_MPEG12_BITRATE1110		0x4000  // 320Kbps for MPEG-1 layer 3 

// vbr for BTSDK_A2DP_MPEG12UserInfoStru
#define BTSDK_A2DP_MPEG12_VBRSUPPORT		0x01

// MPEG24(AAC) capabilities 
// MPEG AAC Sampling Frequency for BTSDK_A2DP_MPEG24UserInfoStru
#define BTSDK_A2DP_AAC_SF8000				0x0800
#define BTSDK_A2DP_AAC_SF11025				0x0400
#define BTSDK_A2DP_AAC_SF12000				0x0200
#define BTSDK_A2DP_AAC_SF16000				0x0100
#define BTSDK_A2DP_AAC_SF22050				0x0080
#define BTSDK_A2DP_AAC_SF24000				0x0040
#define BTSDK_A2DP_AAC_SF32000				0x0020
#define BTSDK_A2DP_AAC_SF44100				0x0010
#define BTSDK_A2DP_AAC_SF48000				0x0008
#define BTSDK_A2DP_AAC_SF64000				0x0004
#define BTSDK_A2DP_AAC_SF88200				0x0002
#define BTSDK_A2DP_AAC_SF96000				0x0001
#define BTSDK_A2DP_AAC_SFSEPALL				0x0FFF
// MPEG AAC Object Type for BTSDK_A2DP_MPEG24UserInfoStru
#define BTSDK_A2DP_AAC_MPEG2_LC				0x80
#define BTSDK_A2DP_AAC_MPEG4_LC				0x40
#define BTSDK_A2DP_AAC_MPEG4_LTP			0x20
#define BTSDK_A2DP_AAC_MPEG4_SCALABLE		0x10
#define BTSDK_A2DP_AAC_OBJECT_SEPALL		0xF0
// MPEG AAC Channels for BTSDK_A2DP_MPEG24UserInfoStru
#define BTSDK_A2DP_AAC_CH_1					0x02
#define BTSDK_A2DP_AAC_CH_2					0x01
// MPEG AAC VBR Support for BTSDK_A2DP_MPEG24UserInfoStru
#define BTSDK_A2DP_AAC_VBR					0x01
#define BTSDK_A2DP_AAC_NOVBR				0x00

#define BTSDK_A2DP_AUDIOCARD_NAME_LEN      	0x80
#define BTSDK_A2DP_CODECCAPS_LEN			0x10

// codec_type
#define BTSDK_A2DP_SBC					0x00
#define BTSDK_A2DP_MPEG12 				0x01
// that is AAC
#define BTSDK_A2DP_MPEG24				0x02
#define BTSDK_A2DP_SBC_PAYLOAD_TYPE 	0x60
#define BTSDK_A2DP_MPEG12_PAYLOAD_TYPE 	0x61

// codec_priority 
#define BTSDK_A2DP_CODEC_PRIORITY_1     1
#define BTSDK_A2DP_CODEC_PRIORITY_2     2
#define BTSDK_A2DP_CODEC_PRIORITY_3     3 

// Possible flags for member 'mask' in _BtSdkLocalA2DPServerAttrStru 
#define BTSDK_LA2DPSAM_DEVTYPE			0x0001
#define BTSDK_LA2DPSAM_CONTENTPROTECT	0x0002
#define BTSDK_LA2DPSAM_SEPTYPE			0x0004
#define BTSDK_LA2DPSAM_CODEC			0x0008
#define BTSDK_LA2DPSAM_AUDIOCARD        0x0010
#define BTSDK_LA2DPSAM_ALL				0x001f

// Possible flags for member 'dev_type' in _BtSdkLocalA2DPServerAttrStru 
#define BTSDK_A2DP_PLAYER	    		0x0001
#define BTSDK_A2DP_HEADPHONE		    0x0100

//  transport service mask	, service capabilities category and its parameter
#define AVDTP_SERVICE_CATEGORY_MEDIA_TRANSPORT 			    0x0001
#define AVDTP_SERVICE_CATEGORY_REPORT 				 		0x0002
#define AVDTP_SERVICE_CATEGORY_RECOVERY				 		0x0004
#define AVDTP_SERVICE_CATEGORY_CONTENT_PROTECTION 			0x0008
#define AVDTP_SERVICE_CATEGORY_HEADER_COMPRESSION			0x0010
#define AVDTP_SERVICE_CATEGORY_MULTIPLEXING					0x0020
#define AVDTP_SERVICE_CATEGORY_MEDIA_CODEC					0x0040
#define AVDTP_SERVICE_CATEGORY_NOT_DEFINED					0x0080

// Possible flags for member 'content_protect' in _BtSdkLocalA2DPServerAttrStru 
#define BTSDK_CONTENT_PROTECT_1         1 
#define BTSDK_CONTENT_PROTECT_2         2 
#define BTSDK_CONTENT_PROTECT_3         3 

// Possible flags for member 'sep_type' in _BtSdkLocalA2DPServerAttrStru 
#define BTSDK_AUDIOSRC				    0x00 
#define BTSDK_AUDIOSNK				    0x01

// Possible flags for member 'codec_num' in _BtSdkLocalA2DPServerAttrStru 
#define BTSDK_CODEC_NUM_1               1 
#define BTSDK_CODEC_NUM_2               2
#define BTSDK_CODEC_NUM_3               3

// Possible flags for member 'mask' in _BtSdkLocalA2DPServerAttrStru 
#define BTSDK_LA2DPSAM_DEVTYPE			0x0001
#define BTSDK_LA2DPSAM_CONTENTPROTECT	0x0002
#define BTSDK_LA2DPSAM_SEPTYPE			0x0004
#define BTSDK_LA2DPSAM_CODEC			0x0008
#define BTSDK_LA2DPSAM_AUDIOCARD        0x0010
#define BTSDK_LA2DPSAM_ALL				0x001f

// Possible flags for member 'dev_type' in _BtSdkLocalA2DPServerAttrStru 
#define BTSDK_A2DP_PLAYER	    		0x0001
#define BTSDK_A2DP_HEADPHONE		    0x0100
//A2DP end  --------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//AVRCP begin------------------------------------------------------------------------------------------
/* AVRCP specific event. */
#define BTSDK_APP_EV_AVRCP_BASE							0xB00
/* AVRCP TG specific event. */
#define BTSDK_APP_EV_AVTG_BASE							BTSDK_APP_EV_AVRCP_BASE
#define BTSDK_APP_EV_AVTG_ATTACHPLAYER_IND				(BTSDK_APP_EV_AVTG_BASE + 0x01)
#define BTSDK_APP_EV_AVRCP_DETACHPLAYER_IND 			(BTSDK_APP_EV_AVTG_BASE + 0x03)
#define BTSDK_APP_EV_AVRCP_IND_CONN 					BTSDK_APP_EV_AVTG_ATTACHPLAYER_IND
#define BTSDK_APP_EV_AVRCP_IND_DISCONN 					BTSDK_APP_EV_AVRCP_DETACHPLAYER_IND

/*AV/C Panel Commands operation_id*/
#define BTSDK_AVRCP_OPID_AVC_PANEL_POWER 				0x40
#define BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_UP 			0x41
#define BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_DOWN 			0x42
#define BTSDK_AVRCP_OPID_AVC_PANEL_MUTE					0x43
#define BTSDK_AVRCP_OPID_AVC_PANEL_PLAY 				0x44
#define BTSDK_AVRCP_OPID_AVC_PANEL_STOP  				0x45
#define BTSDK_AVRCP_OPID_AVC_PANEL_PAUSE 				0x46
#define BTSDK_AVRCP_OPID_AVC_PANEL_RECORD 				0x47
#define BTSDK_AVRCP_OPID_AVC_PANEL_REWIND 				0x48
#define BTSDK_AVRCP_OPID_AVC_PANEL_FAST_FORWARD 		0x49
#define BTSDK_AVRCP_OPID_AVC_PANEL_EJECT 				0x4a
#define BTSDK_AVRCP_OPID_AVC_PANEL_FORWARD 				0x4b
#define BTSDK_AVRCP_OPID_AVC_PANEL_BACKWARD 			0x4c

/*button state(0: pressed 1: released)*/
/*used by Btsdk_AVRCP_Passthrough_Cmd_Func parameter state_flag*/
#define BTSDK_AVRCP_BUTTON_STATE_PRESSED         		0
#define BTSDK_AVRCP_BUTTON_STATE_RELEASED   			1
//AVRCP end  --------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//OPP begin------------------------------------------------------------------------------------------
/* OPP specific event */
#define BTSDK_APP_EV_OPP_BASE				    0x200
#define BTSDK_APP_EV_OPP_CONN					(BTSDK_APP_EV_OPP_BASE+0)
#define BTSDK_APP_EV_OPP_DISC					(BTSDK_APP_EV_OPP_BASE+1)
#define BTSDK_APP_EV_OPP_PULL					(BTSDK_APP_EV_OPP_BASE+2)
#define BTSDK_APP_EV_OPP_PUSH					(BTSDK_APP_EV_OPP_BASE+3)
#define BTSDK_APP_EV_OPP_PUSH_CARD				(BTSDK_APP_EV_OPP_BASE+4)
#define BTSDK_APP_EV_OPP_EXCHG					(BTSDK_APP_EV_OPP_BASE+5)
//OPP end  --------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//PAN begin------------------------------------------------------------------------------------------
/*for win32 only*/
/* PAN Event */
#define BTSDK_PAN_EV_BASE			0x00000100
#define BTSDK_PAN_EV_IP_CHANGE		BTSDK_PAN_EV_BASE + 1
//PAN end  --------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//***********************************************************************************************//
//***********************************************************************************************//
//for stack macro-----------------------------------------------------------------------------
/* Possible Manufacurer ID values returned by function Btsdk_HFAP_GetDeviceVendorInfo. */
enum BTSDK_ManufacturerEnum {
	BTSDK_MANUFACTURER_SONYERICSSON = 1,
	BTSDK_MANUFACTURER_SIEMENTS,   
	BTSDK_MANUFACTURER_NOKIA,      
	BTSDK_MANUFACTURER_MOTO
};

/* Possible Modle ID values returned by function Btsdk_HFAP_GetDeviceVendorInfo. */
enum BTSDK_ModelEnum {
	BTSDK_MODEL_T610 = 1,   // for SonyEricsson T610 and T618, both of them share the same firmware.
	BTSDK_MODEL_T630,		// for SonyEricsson T630 and T628, both of them share the same firmware.
	BTSDK_MODEL_S55,		// Siements S55 
	BTSDK_MODEL_6310i,		// Nokia 6310i
	BTSDK_MODEL_V600,		// Moto V600
	BTSDK_MODEL_E1000,		// Moto E1000
	BTSDK_MODEL_6680		// Nokia 6680
};
//for stack macro-----------------------------------------------------------------------------
//***********************************************************************************************//
//***********************************************************************************************//

#endif