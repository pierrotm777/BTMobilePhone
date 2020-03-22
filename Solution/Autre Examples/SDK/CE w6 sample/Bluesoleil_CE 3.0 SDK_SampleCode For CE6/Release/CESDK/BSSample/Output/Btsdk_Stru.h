/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2008 IVT Corporation
*
* All rights reserved.
* 
---------------------------------------------------------------------------*/

/////////////////////////////////////////////////////////////////////////////
// Module Name:
//     Btsdk_Stru.h
// Abstract:
//     This Module defines struct used by APIs.
// Usage:
//     #include "Btsdk_Stru.h"
// 
// Author:    
//     chenjinfeng
// Revision History:
//     2008-8-4		Created
// 
/////////////////////////////////////////////////////////////////////////////
#ifndef _BTSDK_STRU_H
#define _BTSDK_STRU_H

#include "Btsdk_Macro.h"


//-----------------------------------------------------------------------------------------------------
//ini begin--------------------------------------------------------------------------------------------
typedef struct  _BtSdkCallbackStru
{
	BTUINT16  type;					// type of callback
	void      *func;				// callback function
}BtSdkCallbackStru, *PBtSdkCallbackStru;
//ini end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//sec begin--------------------------------------------------------------------------------------------

//sec end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//svc begin--------------------------------------------------------------------------------------------
typedef struct _BtSdkUUIDStru
{
	BTUINT32 Data1;
	BTUINT16 Data2;
	BTUINT16 Data3;
	BTUINT8  Data4[8];
} BtSdkUUIDStru, *PBtSdkUUIDStru;

typedef struct _BtSdkSDPSearchPatternStru
{
	BTUINT32 mask;					// Specifies the valid bytes in the uuid
	BtSdkUUIDStru uuid;				// UUID value
} BtSdkSDPSearchPatternStru, *PBtSdkSDPSearchPatternStru;

typedef struct _BtSdkRemoteServiceAttrStru
{
	BTUINT16 mask;									// Decide which parameter to be retrieved
	union
	{
		BTUINT16 svc_class;							//  For Compatibility
		BTUINT16 service_class;
	};												// Type of this service record
	BTDEVHDL dev_hdl;								// Handle to the remote device which provides this service.
	BTUINT8 svc_name[BTSDK_SERVICENAME_MAXLENGTH];	// Service name in UTF-8
	BTLPVOID ext_attributes;						// Free by the APP,maybe point to bellow
	//SDAP_DIInfoStru(service_class==CLS_PNP_INFO); 
	//SDAP_HIDInfoStru(service_class==CLS_HID);
	//BtSdkRmtSPPSvcExtAttrStru(service_class==CLS_SERIAL_PORT);
	//null(other)
	BTUINT16 status;                                // = PRemoteServiceStru.status & 0xFF, 
	// The lower 1byte of the PRemoteServiceStru.status contains the status, 
	// because the PRemoteServiceStru.status is not define, so no use this field
} BtSdkRemoteServiceAttrStru, *PBtSdkRemoteServiceAttrStru;

typedef struct _BtSdkRmtSPPSvcExtAttrStru 
{
	BTUINT32 size;						/*Size of BtSdkRmtSPPSvcExtAttrStru*/
	BTUINT8  server_channel;			/*Server channel value of this SPP service record*/
} BtSdkRmtSPPSvcExtAttrStru, *PBtSdkRmtSPPSvcExtAttrStru;

struct SDAP_HIDInfoStru {
	BTUINT16 		size;				/* size of SDAP_HIDInfoStru, include additional bytes for stores_list */
	BTUINT16 		mask;				/* optional or mandatory Bool type attribute mask */
	BTUINT32 		svc_hdl;			/* service handle */
	BTUINT16		release_num;		/* HID device release number */
	BTUINT16		parser_ver;			/* HID parser version */
	BTUINT8			sub_cls;			/* HID device subclass */
	BTUINT8			country_code;		/* HID country code */
	BTUINT16		super_to;			/* HID supervision timeout */
	BTUINT16		profile_ver;		/* HID ProfileVersion attribute value*/
	BTUINT16		desc_list_size;		/* total size of the descriptor list. It also marks the start point 
										of the report list in the successive memory. */
	BTUINT8			list[1];			/* list of HID class descriptor. */
};

struct SDAP_DIInfoStru {
	BTUINT16		size;				/* size of SDAP_DIInfoStru, include additional bytes for str_url_list */
	BTUINT16		mask;				/* optional or mandatory Bool type attribute mask */
	BTUINT32		svc_hdl;			/* service handle */
	BTUINT16		spec_id;			/* value of SpecificationID attribute */
	BTUINT16		vendor_id;			/* value of VendorID attriubte */
	BTUINT16		product_id;			/* value of ProductID attribute */
	BTUINT16		version;			/* value of Version attribute */
	BTUINT16		vendor_id_src;		/* value of VendorIDSource attribute */
	BTUINT16		list_size;			/* size of the text string list */
	BTUINT8			str_url_list[1];	/* List of ClientExecutableURL, DocumentationURL and 
										ServiceDescription attributes. */
};

typedef struct  _BtSdkLocalServerAttrStru
{
	BTUINT16 mask;					                 // Decide which parameter to be modified or retrieved
	BTUINT16 service_class;			                 // Service class, 16bit UUID
	BTUINT8 svc_name[BTSDK_SERVICENAME_MAXLENGTH];	 // must in UTF-8
	BTUINT16 security_level;		                 // Authorization, Authentication, Encryption, None
	BTUINT16 author_method;			                 // Accept, Prompt, Reject (untrusted device), combined with security level "Authorization(BTSDK_SSL_AUTHENTICATION)"
	BTLPVOID ext_attributes;		                 // Profile specific attributes//app should delete it
	BTUINT32 app_param;				                 // User defined parameters
} BtSdkLocalServerAttrStru, *PBtSdkLocalServerAttrStru;

//ext_attributes of the BtSdkLocalServerAttrStru
typedef struct _BtSdkLocalSPPServerAttrStru 
{
	BTUINT32 size;
	BTUINT16 mask;
	BTUINT8 com_index;
} BtSdkLocalSPPServerAttrStru, *PBtSdkLocalSPPServerAttrStru;
typedef struct _BtSdkLocalSPPServerAttrStru BtSdkLocalDUNServerAttrStru;
typedef struct _BtSdkLocalSPPServerAttrStru *PBtSdkLocalDUNServerAttrStru;
typedef struct _BtSdkLocalSPPServerAttrStru BtSdkLocalFAXServerAttrStru;
typedef struct _BtSdkLocalSPPServerAttrStru *PBtSdkLocalFAXServerAttrStru;

//ext_attributes of the BtSdkLocalServerAttrStru
typedef struct  _BtSdkLocalFTPServerAttrStru
{
	BTUINT32 size;
	BTUINT16 mask;
	BTUINT16 desired_access;
	BTUINT8 root_dir[BTSDK_PATH_MAXLENGTH];
} BtSdkLocalFTPServerAttrStru, *PBtSdkLocalFTPServerAttrStru;

//ext_attributes of the BtSdkLocalServerAttrStru
typedef struct  _BtSdkLocalOPPServerAttrStru
{
	BTUINT32 size;									/*Size of this structure, use for verification and versioning.*/
	BTUINT16 mask; 									/*Decide which parameter to be modified or retrieved*/
	BTUINT16 vcard_support;							/*Specify vCard version supported and operation allowed*/
	BTUINT16 vcal_support;							/*Specify vCal version supported and operation allowed*/
	BTUINT16 vnote_support;							/*Specify operation on vNote allowed*/
	BTUINT16 vmessage_support;						/*Specify operation on vMessage allowed*/
	BTUINT8 inbox_path[BTSDK_PATH_MAXLENGTH];		/*must in UTF-8*/
	BTUINT8 outbox_path[BTSDK_PATH_MAXLENGTH];		/*must in UTF-8*/
	BTUINT8 own_card[BTSDK_CARDNAME_MAXLENGTH]; 	/*must in UTF-8*/
} BtSdkLocalOPPServerAttrStru, *PBtSdkLocalOPPServerAttrStru;

//ext_attributes of the BtSdkLocalServerAttrStru
typedef struct  _BtSdkLocalGNServerAttrStru
{
	BTUINT32 size;
	BTUINT16 mask;
	BTUINT16 security_description;
	BTUINT16 packet_type_num;
	BTUINT16 packet_type_list[BTSDK_PACKETTYPE_MAXNUM];
} BtSdkLocalGNServerAttrStru, *PBtSdkLocalGNServerAttrStru;

//ext_attributes of the BtSdkLocalServerAttrStru
typedef struct  _BtSdkLocalNAPServerAttrStru
{
	BTUINT32 size;
	BTUINT16 mask;
	BTUINT16 security_description;
	BTUINT32 max_access_rate;
	BTUINT16 net_access_type;
	BTUINT16 packet_type_num;
	BTUINT16 packet_type_list[BTSDK_PACKETTYPE_MAXNUM];
} BtSdkLocalNAPServerAttrStru, *PBtSdkLocalNAPServerAttrStru;

//ext_attributes of the BtSdkLocalServerAttrStru
//BtSdkLocalA2DPServerAttrStru//have define in conn section

//svc end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//loc begin--------------------------------------------------------------------------------------------
typedef struct _BtSdkCommSettingStru
{
	BTUINT32 baud_rate;		//  Baud rate at which the communications device operates. 
	BTUINT8  byte_size;		//  Number of bits in the bytes transmitted and received. 
	BTUINT8  parity;		//  Parity scheme to be used. 
	BTUINT8  stop_bits;		//  Number of stop bits to be used. 
	BTUINT8  flow_control;	//  Flow control scheme to be used. 
} BtSdkCommSettingStru, *PBtSdkCommSettingStru;

typedef struct _BtSdkEventParamStru
{
	BTUINT8 ev_code;		          // Event code. 
	BTUINT8 param_len;		          // length of param in bytes 
	BTUINT8 param[1];		          // Event parameters.
} BtSdkEventParamStru, *PBtSdkEventParamStru;

typedef struct _BtSdkVendorCmdStru
{
	BTUINT16 ocf;			          // OCF Range (10 bits): 0x0000-0x03FF
	BTUINT8	 param_len;		          // length of param in bytes
	BTUINT8	 param[1];		          // Parameters to be packed in the vendor command. Little endian is adopted. 
} BtSdkVendorCmdStru, *PBtSdkVendorCmdStru;

typedef struct _BtSdkLocalLMPInfoStru
{
	BTUINT8 lmp_feature[8];				/* LMP features */
	BTUINT16 manuf_name;				/* the name of the manufacturer */
	BTUINT16 lmp_subversion;			/* the sub version of the LMP firmware */
	BTUINT8 lmp_version;				/* the main version of the LMP firmware */
	BTUINT8 hci_version;				/* HCI version */
	BTUINT16 hci_revision;				/* HCI revision */
	BTUINT8 country_code;				/* country code */
} BtSdkLocalLMPInfoStru, *PBtSdkLocalLMPInfoStru;
//loc end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//rmt begin--------------------------------------------------------------------------------------------
typedef struct  _BtSdkRemoteLMPInfoStru
{
	BTUINT8 lmp_feature[8];				/* LMP features */
	BTUINT16 manuf_name; 				/* the name of the manufacturer */
	BTUINT16 lmp_subversion;			/* the sub version of the LMP firmware */
	BTUINT8 lmp_version; 				/* the main version of the LMP firmware */
} BtSdkRemoteLMPInfoStru, *PBtSdkRemoteLMPInfoStru;

typedef struct _BtSdkRemoteDevicePropertyStru
{
	BTUINT32 mask;								// Specifies members available.
	BTDEVHDL dev_hdl;							// Handle assigned to the device record
	BTUINT8 bd_addr[BTSDK_BDADDR_LEN];			// BT address of the device record
	BTUINT8 name[BTSDK_DEVNAME_LEN];			// Name of the device record, must be in UTF-8
	BTUINT32 dev_class;							// Device class
	BtSdkRemoteLMPInfoStru lmp_info;			//  LMP info 
	BTUINT8	link_key[BTSDK_LINKKEY_LEN];		//  link key for this device. 
}BtSdkRemoteDevicePropertyStru, *PBtSdkRemoteDevicePropertyStru;

/* Parameters of Hold_Mode command */
typedef struct _BtSdkHoldModeStru {
	BTUINT16 conn_hdl;					/* reserved, set it to 0. */
	BTUINT16 max;						/* Hold mode max interval. */
	BTUINT16 min;						/* Hold mode min interval. */
} BtSdkHoldModeStru, *PBtSdkHoldModeStru;

/* Parameters of Sniff_Mode command */
typedef struct _BtSdkSniffModeStru {
	BTUINT16 conn_hdl;					/* reserved, set it to 0. */
	BTUINT16 max;						/* Sniff mode max interval. */
	BTUINT16 min;						/* Sniff mode min interval. */
	BTUINT16 attempt;					/* Sniff mode attempt value. */
	BTUINT16 timeout;					/* Sniff mode timeout value. */
} BtSdkSniffModeStru, *PBtSdkSniffModeStru;

/* Parameters of Park_Mode (V1.1) or Park_State (V1.2) command */
typedef struct _BtSdkParkModeStru {
	BTUINT16 conn_hdl;					/* reserved, set it to 0. */
	BTUINT16 max;						/* Beacon max interval. */
	BTUINT16 min;						/* Beacon min interval. */
} BtSdkParkModeStru, *PBtSdkParkModeStru;
//rmt end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------



//-----------------------------------------------------------------------------------------------------
//shc begin--------------------------------------------------------------------------------------------
typedef struct  tag_BtSdkShortCutPropertyStru
{
	BTSHCHDL shc_hdl;						        // handle assigned to the shortcut instance
	BTCONNHDL conn_hdl;						        // handle assigned to the connection instance
	BTCONNHDL dev_hdl;						        // handle assigned to the device instance 
	BTSVCHDL svc_hdl;						        // handle assigned to the service instance 
	BTBOOL	by_dev_hdl;						        // BTSDK_TRUE: Specify device by dev_hdl. Otherwise by bd_addr. 
	BTBOOL	by_svc_hdl;						        // BTSDK_TRUE: Specify service type by svc_hdl. Otherwise by svc_class. 
	BTUINT8 bd_addr[BTSDK_BDADDR_LEN];		        // bluetooth address of local device 
	BTUINT16 svc_class;						        // service class 
	BTUINT16 mask;							        // Specified which member is to be set or get.  
	BTUINT8 shc_name[BTSDK_SHORTCUT_NAME_LEN];	    // name of the shortcut, must in UTF-8 
	BTUINT8 dev_name[BTSDK_DEVNAME_LEN];	        // Name of the device record, must be in UTF-8 
	BTUINT8 svc_name[BTSDK_SERVICENAME_MAXLENGTH];	// must in UTF-8
	BTUINT32 dev_class;						        // device class of the remote device
	BTBOOL is_default;						        // is default shortcut 
	BTUINT8 sec_level;						        // Security level of this shortcut. Authentication/Encryption. 
	BTUINT16 shc_attrib_len;				        // the length of shortcut attribute 
	BTUINT8 *pshc_attrib;					        // shortcut attribute //will be string which store pshc_attrib values
} BtSdkShortCutPropertyStru, *PBtSdkShortCutPropertyStru;
//shc end----------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//------------------------------------------------------------------------------------------------------
//conn begin--------------------------------------------------------------------------------------------
typedef struct _BtSdkConnectionPropertyStru
{
	BTUINT32 role : 2;
	BTUINT32 result : 30;
	BTDEVHDL device_handle;
	BTSVCHDL service_handle;
	BTUINT16 service_class;
	BTUINT32 duration;
	BTUINT32 received_bytes;
	BTUINT32 sent_bytes;
} BtSdkConnectionPropertyStru, *PBtSdkConnectionPropertyStru;

// lParam for LAP
typedef struct _BtSdkLAPConnParamStru
{
	BTUINT32 size;
	BTUINT16 mask;	//Reserved set 0
	BTUINT8 com_index;
} BtSdkLAPConnParamStru, *PBtSdkLAPConnParamStru;

// lParam for DUN
typedef struct _BtSdkDUNConnParamStru
{
	BTUINT32 size;
	BTUINT16 mask;	//Reserved set 0
	BTUINT8 com_index;
} BtSdkDUNConnParamStru, *PBtSdkDUNConnParamStru;

// lParam for FAX
typedef struct _BtSdkFAXConnParamStru 
{
	BTUINT32 size;
	BTUINT16 mask;	//Reserved set 0
	BTUINT8 com_index;
} BtSdkFAXConnParamStru, *PBtSdkFAXConnParamStru;

// lParam for SPP
typedef struct _BtSdkSPPConnParamStru
{
	BTUINT32 size;
	BTUINT16 mask;	//Reserved set 0
	BTUINT8 com_index;
} BtSdkSPPConnParamStru, *PBtSdkSPPConnParamStru;

// lParam for OPP.
typedef struct _BtSdkOPPConnParamStru
{
	BTUINT32 size;									// Size of this structure, use for verification and tell version
	BTUINT8 inbox_path[BTSDK_PATH_MAXLENGTH];		// must in UTF-8
	BTUINT8 outbox_path[BTSDK_PATH_MAXLENGTH];		// must in UTF-8
	BTUINT8 own_card[BTSDK_CARDNAME_MAXLENGTH]; 	// must in UTF-8
} BtSdkOPPConnParamStru,BtSdkOPPClientParamStru, *PBtSdkOPPConnParamStru,*PBtSdkOPPClientParamStru;

// lParam for A2DP SINK.
struct BTSDK_A2DP_CodecCapsStru{
	BTUINT8   codec_type;        						// Codec type 
	BTUINT8   codec_priority;							// the priority of the Codec 
	BTUINT8   codec_caps[BTSDK_A2DP_CODECCAPS_LEN];     // indicates the Codec information,be configured such as struct BTSDK_A2DP_SBCUserInfoStru or  BTSDK_A2DP_MPEG12UserInfoStru or BTSDK_A2DP_MPEG24UserInfoStru,depend on codec_type
};
typedef struct _BtSdkLocalA2DPServerAttrStru 
{
	BTUINT32 size;
	BTUINT16 mask;
	BTUINT16 dev_type;								// the local device type, can be BTSDK_A2DP_PLAYER or BTSDK_A2DP_HEADPHONE
	BTUINT16 trans_mask;         					// transport service mask */
	BTUINT16 content_protect;    					// the content protection type RFD
	BTUINT8	 sep_type;								// BTSDK_AUDIOSRC or BTSDK_AUDIOSNK 
	BTUINT8   codec_num;   				
	BTUINT8   audio_card[BTSDK_A2DP_AUDIOCARD_NAME_LEN];	// the audio card name used 
	struct BTSDK_A2DP_CodecCapsStru codec[1];
} BtSdkLocalA2DPServerAttrStru, *PBtSdkLocalA2DPServerAttrStru;

// lParam for HID.  this is not clear for this time
//struct SDAP_DIInfoStru 

typedef struct _BtSdkBIPImgConnParamStru
{
	BTUINT32 	size;
	BTUINT16	mask;             // this is not clear
	BTUINT16	feature;
} BtSdkBIPImgConnParamStru, *PBtSdkBIPImgConnParamStru;


typedef struct _BtSdkBIPObjConnParamStru
{
	BTUINT32 	    size;
	BTUINT16	    mask;             // Specifies the valid bytes in the uuid
	BTSVCHDL	    loc_svc_hdl;
	BtSdkUUIDStru	obj_svcid;
} BtSdkBIPObjConnParamStru, *PBtSdkBIPObjConnParamStru;

typedef struct _BtSdkBPPConnParamStru
{
	BTUINT32 	    size;
	BTUINT16	    mask;            // this is not clear
	BTUINT16	    target;
} BtSdkBPPConnParamStru, *PBtSdkBPPConnParamStru;
//conn end---------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//HF15 begin--------------------------------------------------------------------------------------------
/* Parameter of the BTSDK_HFP_EV_SLC_ESTABLISHED_IND and BTSDK_HFP_EV_SLC_RELEASED_IND events */
typedef struct Btsdk_HFP_ConnInfo {
	BTUINT16 role; /* 16bit UUID specifies the local role of the connection:
				   BTSDK_CLS_HANDSFREE - Local device acts as a HF. 
				   BTSDK_CLS_HANDSFREE_AG - Local device acts as a Hands-free AG. 
				   BTSDK_CLS_HEADSET - Local device acts as a HS. 
				   BTSDK_CLS_HEADSET_AG - Local device acts as a Headset AG. */
	BTDEVHDL dev_hdl;  /* Handle to the remote device. */
} Btsdk_HFP_ConnInfoStru, *PBtsdk_HFP_ConnInfoStru;

/* Used By +BINP, +CNUM, +CLIP, +CCWA */
typedef struct Btsdk_HFP_PhoneInfo {
	BTUINT8 	type;				/* the format of the phone number provided */
	BTUINT8 	service;			/* Indicates which service this phone number relates to. Shall be either 4 (voice) or 5 (fax). */
	BTUINT8 	num_len;			/* the length of the phone number provided */	
	BTINT8 		number[32];			/* subscriber number, the length shall be PHONENUM_MAX_DIGITS */	
	BTUINT8 	name_len;			/* length of subaddr */
	BTINT8 		alpha_str[1];		/* string type subaddress of format specified by <cli_validity> */	
} Btsdk_HFP_PhoneInfoStru, *PBtsdk_HFP_PhoneInfoStru;

/* Used by BTSDK_HFP_EV_ATCMD_RESULT */
typedef struct Btsdk_HFP_ATCmdResult {
	BTUINT16	cmd_code;			/* Which AT command code got an error */
	BTUINT8	    result_code;		/* What result occurs, BTSDK_HFP_APPERR_TIMEOUT, CME Error Code or standard error result code */
} Btsdk_HFP_ATCmdResultStru, *PBtsdk_HFP_ATCmdResultStru;

/* Used By +CLCC */
typedef struct Btsdk_HFP_CLCCInfo {
	BTUINT8 	idx;				/* The numbering (start with 1) of the call given by the sequence of setting up or receiving the calls */
	BTUINT8 	dir;				/* Direction, 0=outgoing, 1=incoming */
	BTUINT8 	status;				/* 0=active, 1=held, 2=dialling(outgoing), 3=alerting(outgoing), 4=incoming(incoming), 5=waiting(incoming) */
	BTUINT8 	mode;				/* 0=voice, 1=data, 2=fax */
	BTUINT8 	mpty;				/* 0=not multiparty, 1=multiparty */
	BTUINT8		type;				/* the format of the phone number provided */
	BTUINT8		num_len;			/* the length of the phone number provided */	
	BTINT8		number[1];			/* phone number */	
} Btsdk_HFP_CLCCInfoStru, *PBtsdk_HFP_CLCCInfoStru;

/* Used By +COPS */
typedef struct Btsdk_HFP_COPSInfo {
	BTUINT8 	mode;				/* current mode and provides no information with regard to the name of the operator */
	BTUINT8 	format;				/* the format of the operator parameter string */
	BTUINT8		operator_len;
	BTINT8 	operator_name[1];	/* the string in alphanumeric format representing the name of the network operator */	
} Btsdk_HFP_COPSInfoStru, *PBtsdk_HFP_COPSInfoStru;

/* Used by BTSDK_HFP_EV_ATCMD_RESULT */
typedef struct Btsdk_AGAP_PreSCOConnInd {
	BTUINT16 pkt_type; /* [in] Packet type of the new SCO connection. */
	BTUINT8  blocked;  /* [in] Reserved for compatibility, always set to 0. */
	BTUINT8  result;   /* [out] SCO created by the application or not. Possible values:
					   BTSDK_AGAP_CONN_SCO_DEFAULT, BTSDK_AGAP_CONN_SCO_PENDING. */
} Btsdk_AGAP_PreSCOConnIndStru, *PBtsdk_AGAP_PreSCOConnIndStru;

/* current state mask code for function HFP_AG_SetCurIndicatorVal */
typedef struct Btsdk_HFP_CINDInfo {
	BTUINT8		service;			/* 0=unavailable, 1=available */
	BTUINT8		call;				/* 0=no active call, 1=have active call */
	BTUINT8		callsetup;			/* 0=no callsetup, 1=incoming, 2=outgoing, 3=outalert */
	BTUINT8 	callheld;			/* 0=no callheld, 1=active-hold, 2=onhold */
	BTUINT8 	signal;				/* 0~5 */
	BTUINT8		roam;				/* 0=no roam, 1= roam */
	BTUINT8		battchg;			/* 0~5 */
} Btsdk_HFP_CINDInfoStru, *PBtsdk_HFP_CINDInfoStru;
//HF15 end  --------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------


//-----------------------------------------------------------------------------------------------------
//FTP begin--------------------------------------------------------------------------------------------
//for ftp-----
typedef struct _BtSdkFileTransferReqStru
{
	BTSVCHDL svc_hdl;		/* Handle to the local FTP service receiving the request. */
	BTDEVHDL dev_hdl;		/* Handle to the remote device tries to upload/delete the file. */
	BTUINT16 operation;		/* Specify the operation on the file. 
							It can be one of the following values:
							BTSDK_APP_EV_FTP_PUT: The remote device request to upload the file.
							BTSDK_APP_EV_FTP_CREATE: The remote device request to create a folder.
							BTSDK_APP_EV_FTP_DEL_FILE: The remote device request to delete the file.
							BTSDK_APP_EV_FTP_DEL_FOLDER: The remote device request to delete the folder. In this case,
							file_name specify the name of the folder to be deleted.
							*/
	BTUINT16 flag;			/* Flag specifies the current status of uploading/deleting. 
							It can be one of the following values:
							BTSDK_ER_CONTINUE: The remote device request to upload/delete the file. 
							BTSDK_ER_SUCCESS: The remote device uploads/deletes the file successfully. 
							Other value: Error code specifies the reason of uploading/deleting failure. 
							*/
	BTUINT8	 file_name[BTSDK_PATH_MAXLENGTH];	/* the name of the file uploaded/deleted or to be uploaded/deleted */
} BtSdkFileTransferReqStru, *PBtSdkFileTransferReqStru;

//for opp-----------struct is the same as ftp BtSdkFileTransferReqStru, but every param's meaning is different.
//typedef struct _BtSdkFileTransferReqStru
//{
//	BTSVCHDL svc_hdl;		/* Handle to the local OPP service receiving the request. */
//	BTDEVHDL dev_hdl;		/* Handle to the remote device tries to upload the file */
//	BTUINT16 operation;		/* Reserved for future extension. */
//	BTUINT16 flag;			/* Flag specifies the current status of uploading. 
//							It can be following values:
//							BTSDK_ER_CONTINUE: The remote device request to upload the file. 
//							BTSDK_ER_SUCCESS: The remote device uploads the file successfully. 
//							Other value: Error code specifies the reason of uploading failure. 
//							*/
//	BTUINT8	 file_name[BTSDK_PATH_MAXLENGTH];	/* the name of the file uploaded or to be uploaded */
//} BtSdkFileTransferReqStru, *PBtSdkFileTransferReqStru;
//FTP end  --------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//CTP/ICP begin  --------------------------------------------------------------------------------------

//CTP/ICP end------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//OPP begin-------------------------------------------------------------------------------------------
typedef struct _BtSdkObexAuthInfoStru {
	BTDEVHDL dev_hdl;     /* [in] Device handle of the OBEX server */
	BTUINT8  *target;     /* [in] Target value of the OBEX server */
	BTUINT8  target_len;  /* [in] Length of the target buffer */
	BTBOOL   pwd_only;    /* [in] whether the user id is needed */
	BTUINT8  user_id[BTSDK_MAX_USERID_LEN+1];		/* [out] the user id string	*/
	BTUINT8	 pwd[BTSDK_MAX_PWD_LEN+1];				/* [out] the password string */
} BtSdkObexAuthInfoStru, *PBtSdkObexAuthInfoStru;
//OPP end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//DUN begin-------------------------------------------------------------------------------------------

//DUN end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//PAN begin-------------------------------------------------------------------------------------------

//PAN end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//SPP begin-------------------------------------------------------------------------------------------
typedef struct _BtSdkAppExtSPPAttrStru
{
	BTUINT32			size;									/* Size of this structure */
	BTUINT32			sdp_record_handle;						/* 32bit interger specifies the SDP service record handle */
	BtSdkUUIDStru 		service_class_128;						/* 128bit UUID specifies the service class of this service record */
	BTUINT8				svc_name[BTSDK_SERVICENAME_MAXLENGTH];	/* Service name, in UTF-8 */
	BTUINT8				rf_svr_chnl;							/* RFCOMM server channel assigned to this service record */
	BTUINT8				com_index;								/* Index of the local COM port assigned to this service record */
} BtSdkAppExtSPPAttrStru, *PBtSdkAppExtSPPAttrStru;
//SPP end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//A2DP begin-------------------------------------------------------------------------------------------
struct BTSDK_A2DP_SBCUserInfoStru{	// the structure of SBC codec_caps
	BTUINT8 chnl_mode;
	BTUINT8 sample_frequency;
	BTUINT8 alloc_method;	
	BTUINT8 subband;	
	BTUINT8 block_length;
	BTUINT8 min_bitpool;
	BTUINT8 max_bitpool;
};

struct BTSDK_A2DP_MPEG12UserInfoStru{	// the structure of MPEG-1,2 codec_caps 
	BTUINT8 chnl_mode;					// channel mode, can be MPEG12_JOINTSTEREO,MPEG12_STEREO,MPEG12_DUAL,MPEG12_MONO or the combination of them 
	BTUINT8 crc;						// CRC protection flag, can be MPEG12_CRCSUPPORT or zero 
	BTUINT8 layer;						// MPEG layer, can be MPEG12_LAYER1,MPEG12_LAYER2,MPEG12_LAYER3 or the combination of them
	BTUINT8 sample_frequency;			// sample frequency, can be MPEG12_FS48000,MPEG12_FS44100,MPEG12_FS32000,MPEG12_FS24000,MPEG12_FS22050,MPEG12_FS16000 or the combination of them 
	BTUINT8 mpf;						// media payload format MPF-2 flag, can be MPEG12_MPF2SUPPORT or zero 
	BTUINT8 rfa;						// reserved for future additions
	BTUINT16 bitrate;					// bit rate index, can be MPEG12_BITRARE0000,..., MPEG12_BITRARE1110 or the combination of them 
	BTUINT8 vbr;						// variable bit rate flag, can be MPEG12_VBRSUPPORT or zero
};

struct BTSDK_A2DP_MPEG24UserInfoStru {		// the structure of MPEG4 AAC ISO 14496-3 codec_caps 
	BTUINT16  sample_frequency;
	BTUINT8   object_type;
	BTUINT8   channels;
	BTUINT32  bit_rate;
	BTUINT8   rfa;
	BTUINT8   vbr;
};
//A2DP end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------
//AVRCP begin-------------------------------------------------------------------------------------------
typedef struct _BtSdkPassThrReqStru
{
	BTDEVHDL dev_hdl; 	        /* Handle to the peer device */
	BTUINT8  state_flag;  		/* Button state(0: pressed 1: released) */
	BTUINT8  op_id;        		/* Pass through command ID */
	BTUINT8  length;			/* Length of op_data, Always 0 */
	BTUINT8  op_data[1]; 		/* Additional parameter data, ignored */
} BtSdkPassThrReqStru, *PBtSdkPassThrReqStru;
//AVRCP end  -------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------



#endif
