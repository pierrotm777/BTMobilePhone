/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2007 IVT Corporation
*
* All rights reserved.
* 
---------------------------------------------------------------------------*/
 
/////////////////////////////////////////////////////////////////////////////
// Module Name:
//     Btsdk_Stru.h
// Abstract:
//     This module defines BlueSoleil SDK structures.
// Usage:
//     #include "Btsdk_Stru.h"
// 
// Author://     
//     
// Revision History:
//     2007-12-25		Created
// 
/////////////////////////////////////////////////////////////////////////////



#ifndef _BTSDK_STRU_H
#define _BTSDK_STRU_H


/*************** Structure Definition ******************/ 

typedef struct  _BtSdkCallbackStru
{
	BTUINT16 type;					/*type of callback*/
	void *func;						/*callback function*/
}BtSdkCallbackStru, *PBtSdkCallbackStru;

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

typedef struct _BtSdkVendorCmdStru
{
	BTUINT16 ocf;			/* OCF Range (10 bits): 0x0000-0x03FF */
	BTUINT8	 param_len;		/* length of param in bytes */
	BTUINT8	 param[1];		/* Parameters to be packed in the vendor command. Little endian is adopted. */
} BtSdkVendorCmdStru, *PBtSdkVendorCmdStru;

typedef struct _BtSdkEventParamStru
{
	BTUINT8 ev_code;		/* Event code. */
	BTUINT8 param_len;		/* length of param in bytes */
	BTUINT8 param[1];		/* Event parameters. */
} BtSdkEventParamStru, *PBtSdkEventParamStru;

typedef struct  _BtSdkRemoteLMPInfoStru
{
	BTUINT8 lmp_feature[8];				/* LMP features */
	BTUINT16 manuf_name; 				/* the name of the manufacturer */
	BTUINT16 lmp_subversion;			/* the sub version of the LMP firmware */
	BTUINT8 lmp_version; 				/* the main version of the LMP firmware */
} BtSdkRemoteLMPInfoStru, *PBtSdkRemoteLMPInfoStru;

typedef struct _BtSdkRemoteDevicePropertyStru
{
	BTUINT32 mask;								/*Specifies members available.*/
	BTDEVHDL dev_hdl;							/*Handle assigned to the device record*/
	BTUINT8 bd_addr[BTSDK_BDADDR_LEN];			/*BT address of the device record*/
	BTUINT8 name[BTSDK_DEVNAME_LEN];			/*Name of the device record, must be in UTF-8*/
	BTUINT32 dev_class;							/*Device class*/
	BtSdkRemoteLMPInfoStru lmp_info;			/* LMP info */
	BTUINT8	link_key[BTSDK_LINKKEY_LEN];		/* link key for this device. */
} BtSdkRemoteDevicePropertyStru;  
typedef BtSdkRemoteDevicePropertyStru* PBtSdkRemoteDevicePropertyStru;

/* Parameters of Hold_Mode command */
typedef struct _BtSdkHoldModeStru {
	BTUINT16 conn_hdl;					/* reserved, set it to 0. */
	BTUINT16 max;						/* Hold mode max interval. */
	BTUINT16 min;						/* Hold mode min interval. */
} BtSdkHoldModeStru;
typedef BtSdkHoldModeStru* PBtSdkHoldModeStru;

/*************** Structure Definition ******************/
/* AVRCP specific structure for member 'ext_attributes' in _BtSdkRemoteServiceAttrStru */
typedef struct _BtSdkRmtAVRCPSvcExtAttrStru
{
	BTUINT32 size;						/* Size of BtSdkRmtAVRCPSvcExtAttrStru */
	BTUINT16 mask;						/* Specifies whether an optional attribute value is available */
	BTUINT16 profile_ver;				/* Profile version number, e.g. BTSDK_AVRCP_VER_1_0 */
	BTUINT16 supported_features;		/* Supported features, bit masks of e.g. BTSDK_AVRCP_FEATURES_CATEGORY_1 */
} BtSdkRmtAVRCPSvcExtAttrStru, *PBtSdkRmtAVRCPSvcExtAttrStru;

typedef struct _BtSdkPassThr {
	BTDEVHDL	dev_hdl; 	        /* Handle to the peer device */
	BTUINT8		state_flag;  		/* Button state(0: pressed 1: released) */
	BTUINT8		op_id;        		/* Pass through command ID */
	BTUINT8		length;				/* Length of op_data, Always 0 */
	BTUINT8		op_data[1]; 		/* Additional parameter data, ignored */
} BtSdkPassThrReqStru, *PBtSdkPassThrReqStru,
  BtSdkPassThrRspStru, *PBtSdkPassThrRspStru;

typedef struct _BtSdkGroupNaviReq {
	BTDEVHDL	dev_hdl; 		    /* Handle to the peer device */
    BTUINT8		state_flag;  		/* Button state(0: pressed 1: released) */
    BTUINT16	vendor_unique_id;  	/* Vendor Unique Operation IDs */
} BtSdkGroupNaviReqStru, *PBtSdkGroupNaviReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_PASSTHROUGH_IND, BTSDK_APP_EV_AVRCP_PASSTHROUGH_RSP */
typedef struct _BtSdkPassThrReqNew {
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		state_flag;  		/* Button state(0: pressed 1: released) */
	BTUINT8		op_id;        		/* Pass through command ID */
    BTUINT16	vendor_unique_id;  	/* Vendor Unique Operation IDs */
} BtSdkPassThrReqNewStru, *PBtSdkPassThrReqNewStru,
  BtSdkPassThrRspNewStru, *PBtSdkPassThrRspNewStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GENERAL_REJECT_RSP */
typedef struct _BtSdkGeneralRejectRsp {
	BTUINT32	size;               /* Size of this structure */
	BTUINT16    cmd_type;           /* Type of the command being rejected, e.g. BTSDK_APP_EV_AVRCP_GET_CAPABILITIES_IND */
	BTUINT8     error_code;         /* Error code, e.g. BTSDK_AVRCP_ERROR_INVALID_COMMAND */
} BtSdkGeneralRejectRspStru, *PBtSdkGeneralRejectRspStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_CAPABILITIES_IND, BTSDK_APP_EV_AVRCP_LIST_PLAYER_SETTING_VALUES_IND,
   BTSDK_APP_EV_AVRCP_INFORM_BATTERYSTATUS_OF_CT_IND, BTSDK_APP_EV_AVRCP_SET_ABSOLUTE_VOLUME_IND, 
   BTSDK_APP_EV_AVRCP_PLAY_ITEM_RSP, BTSDK_APP_EV_AVRCP_ADDTO_NOWPLAYING_RSP, BTSDK_APP_EV_AVRCP_SET_ABSOLUTE_VOLUME_RSP,
   BTSDK_APP_EV_AVRCP_SET_ADDRESSED_PLAYER_RSP, BTSDK_APP_EV_AVRCP_GENERAL_REJECT_RSP */
typedef struct _BtSdkID {
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		id;                 /* ID of the item requested, e.g. CapabilityID of the GetCapabilites request */
} BtSdkIDStru, *PBtSdkIDStru,
  BtSdkGetCapabilitiesReqStru, *PBtSdkGetCapabilitiesReqStru,
  BtSdkListPlayerAppSetValReqStru, *PBtSdkListPlayerAppSetValReqStru,
  BtSdkInformBattStatusReqStru, *PBtSdkInformBattStatusReqStru,
  BtSdkSetAbsoluteVolReqStru, *PBtSdkSetAbsoluteVolReqStru,
  BtSdkPlayItemRspStru, *PBtSdkPlayItemRspStru,
  BtSdkAddToNowPlayingRspStru, *PBtSdkAddToNowPlayingRspStru,
  BtSdkSetAbsoluteVolRspStru, *PBtSdkSetAbsoluteVolRspStru,
  BtSdkSetAddresedPlayerRspStru, *PBtSdkSetAddresedPlayerRspStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_SET_ADDRESSED_PLAYER_IND, BTSDK_APP_EV_AVRCP_SET_BROWSED_PLAYER_IND */
typedef struct _BtSdkSize2ID {
	BTUINT32	size;               /* Size of this structure */
	BTUINT16	id;                 /* Player ID */
} BtSdkSize2IDStru, *PBtSdkSize2IDStru,
  BtSdkSetAddresedPlayerReqStru, *PBtSdkSetAddresedPlayerReqStru,
  BtSdkSetBrowsedPlayerReqStru, *PBtSdkSetBrowsedPlayerReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_CAPABILITIES_RSP */
typedef struct _BtSdkGetCapabilitiesRsp {
	BTUINT32	size;                   /* Size of this structure, including the full content of capability */
	BTUINT8 	capability_id;			/* ID of capability requested */
	BTUINT8 	count;					/* Number of CompanyID or EventID */
	BTUINT8 	capability[1];			/* List of CompanyID or EventID
										   3...3*n for COMPANY_ID, 
										   2...n for EVENTS_SUPPORTED
										*/
} BtSdkGetCapabilitiesRspStru, *PBtSdkGetCapabilitiesRspStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_LIST_PLAYER_SETTING_ATTR_RSP, BTSDK_APP_EV_AVRCP_LIST_PLAYER_SETTING_VALUES_RSP,
   BTSDK_APP_EV_AVRCP_GET_CURRENTPLAYER_SETTING_VALUE_RSP, BTSDK_APP_EV_AVRCP_GET_PLAYER_SETTING_ATTR_TEXT_RSP */
typedef struct _BtSdkNumID {
	BTUINT32	size;                   /* Size of this structure, including the full content of id */
	BTUINT8 	num;					/* Number of ID provided */
	BTUINT8 	id[1];					/* List of ID, 2...n for ID */
} BtSdkNumIDStru, *PBtSdkNumIDStru,
  BtSdkListPlayerAppSetAttrRspStru, *PBtSdkListPlayerAppSetAttrRspStru,
  BtSdkListPlayerAppSetValRspStru, *PBtSdkListPlayerAppSetValRspStru,
  BtSdkGetCurPlayerAppSetValReqStru, *PBtSdkGetCurPlayerAppSetValReqStru,
  BtSdkGetPlayerAppSetAttrTxtReqStru, *PBtSdkGetPlayerAppSetAttrTxtReqStru;

/* Player Application Setting Parameter */
typedef struct _BtSdkIDPair {
	BTUINT8 	attr_id;				/* Player Application Setting Arrtribute ID */
	BTUINT8 	value_id;				/* Player Application Setting Value ID */
} BtSdkIDPairStru, *PBtSdkIDPairStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_SET_CURRENTPLAYER_SETTING_VALUE_IND, BTSDK_APP_EV_AVRCP_GET_CURRENTPLAYER_SETTING_VALUE_RSP */
typedef struct _BtSdkNumIDPair {
	BTUINT32	size;                   /* Size of this structure, including the full content of id */
	BTUINT8 	num;					/* Number of attributes */
	BtSdkIDPairStru id[1];				/* List of the requested attribues ID and value ID */
} BtSdkNumIDPairStru, *PBtSdkNumIDPairStru,
  BtSdkSetCurPlayerAppSetValReqStru, *PBtSdkSetCurPlayerAppSetValReqStru,
  BtSdkGetCurPlayerAppSetValRspStru, *PBtSdkGetCurPlayerAppSetValRspStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_PLAYER_SETTING_VALUE_TEXT_IND */
typedef struct _BtSdkIDNumID {
	BTUINT32	size;                   /* Size of this structure, including the full content of value_id */
	BTUINT8 	attr_id;				/* Attribute ID */
	BTUINT8 	num;					/* Number of Value IDs */
	BTUINT8 	value_id[1];			/* List of Value IDs */
} BtSdkIDNumIDStru, *PBtSdkIDNumIDStru,
  BtSdkGetPlayerAppSetValTxtReqStru, *PBtSdkGetPlayerAppSetValTxtReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_INFORM_CHARACTERSET_IND */
typedef struct _BtSdkNumCSID {
	BTUINT32	size;
	BTUINT8 	num;					/* number of CharacterSet IDs */
	BTUINT16 	characterset_id[1];		/* List of CharacterSet IDs */
} BtSdkNumCSIDStru, *PBtSdkNumCSIDStru,
  BtSdkInformCharSetReqStru, *PBtSdkInformCharSetReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_ELEMENT_ATTRIBUTES_IND */
typedef struct _BtSdkIDNum4ID {
	BTUINT32	size;
	BTUINT8 	identifier[8];			/* Unique identifier to identify an element on TG */
	BTUINT8 	num;					/* Number of attr_id */
	BTUINT32 	attr_id[1];				/* Attributes ID */
} BtSdkIDNum4IDStru, *PBtSdkIDNum4IDStru,
  BtSdkGetElementAttrReqStru, *PBtSdkGetElementAttrReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_PLAY_STATUS_RSP */
typedef struct _BtSdkGetPlayStatusRsp {
	BTUINT32	size;
	BTUINT32 	song_length;			/* The total length of the playing song in milliseconds */
	BTUINT32 	song_position;			/* The current position of the playing in milliseconds elapsed */
	BTUINT8 	play_status;			/* Current status of Playing */
} BtSdkPlayStatusRspStru, *PBtSdkPlayStatusRspStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_CHANGE_PATH_IND */
typedef struct _BtSdkChangePathReq {
	BTUINT32	size;
	BTUINT16 	uid_counter;
	BTUINT8 	direction;
	BTUINT8 	folder_uid[8];
} BtSdkChangePathReqStru, *PBtSdkChangePathReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_CHANGE_PATH_RSP */
typedef struct _BtSdkChangePathRsp {
	BTUINT32	size;
	BTUINT8 	status;					/* The Result of the SetBrowsedPlayer operation. If an error has occurred then this is the only field present in the response. */
	BTUINT32 	items_num;				/* The number of items in the folder which has been changed to, ie the new current folder */
} BtSdkChangePathRspStru, *PBtSdkChangePathRspStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_FOLDER_ITEMS_IND */
typedef struct _BtSdkGetFolderItemsReq {
	BTUINT32	size;
	BTUINT8 	scope;					/* e.g AVRCP_SCOPE_MEDIAPLAYER_LIST */
	BTUINT32 	start_item;				/* The offset within the listing of the item which should be the first returned item. The first media element in the listing is at offset 0. */
	BTUINT32 	end_item;				/* The offset within the listing of the item which should be the final returned. */
	BTUINT8 	attr_count;				/* The number of the requested attributes.
										   0x00			All	attributes are requested. There is no following Attribute List.
										   0x01-0xFE	The following Attribute List contains this number of attributes.
										   0xFF			No attributes are requested. There is no following Attribute List.
										*/
	BTUINT32 	attr_id[1];				/* Attributes which are requested to be returned for each item returned. */
} BtSdkGetFolderItemReqStru, *PBtSdkGetFolderItemReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_ITEM_ATTRIBUTES_IND */
typedef struct _BtSdkGetItemAttributesReq {
	BTUINT32	size;
	BTUINT8 	scope;					/* The scope in which the UID of the media element item or folder item is valid. */
	BTUINT8 	uid[8];					/* The UID of the media element item or folder item to returned. */
	BTUINT16 	uid_counter;			
	BTUINT8 	attr_num;				/* The number of attributes IDs in the following Attribute ID list. If this calue is 0 then all attributes are requested. */
	BTUINT32 	attr_id[1];				/* e.g list AVRCP_MA_TITLEOF_MEDIA */
} BtSdkGetItemAttrReqStru, *PBtSdkGetItemAttrReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_SEARCH_IND */
typedef struct _BtSdkSearchReq {
	BTUINT32	size;
	BTUINT16 	characterset_id;		/* CharacterSet ID. 
										   0x006A				The calue of UTF-8 as defined in IANA character set document.
										   All other values		reserved.
										*/
	BTUINT16 	len;					/* The length of the search string in octets. */
	BTUINT8 	string[1];				/* The stirng to search on in the specified character set. */
} BtSdkSearchReqStru, *PBtSdkSearchReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_SEARCH_RSP */
typedef struct _BtSdkSearchRsp {
	BTUINT32	size;
	BTUINT8 	status;					/* The Result of the GetItemAttributes operation. If an error has occurred then this is the only field present in the response. */
	BTUINT16 	uid_counter;
	BTUINT32 	items_num;				/* The number of media element items found in the search */
} BtSdkSearchRspStru, *PBtSdkSearchRspStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_PLAY_ITEM_IND */
typedef struct _BtSdkPlayItemReq {
	BTUINT32	size;
	BTUINT8 	scope;					/* The scope in which the UID of the media elemnt item or folder item, if supported, is valid. */
	BTUINT8 	uid[8];					/* The UID of the media element item or folder item. */
	BTUINT16 	uid_counter;
} BtSdkPlayItemReqStru, *PBtSdkPlayItemReqStru,
  BtSdkAddToNowPlayingReqStru, *PBtSdkAddToNowPlayingReqStru;

typedef struct _BtSdkIDString {
	BTUINT8 	id;						/* AttributeID or ValueID */
	BTUINT16 	characterset_id;		/* Character set ID */
	BTUINT8 	len;					/* Length of the player application setting attribute string */
	BTUINT8 	string[1];				/* Player application setting attribute string in specified character set */
} BtSdkIDStringStru, *PBtSdkIDStringStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_PLAYER_APPLICATION_SETTING_ATTRIBUTES_TEXT_RSP,
BTSDK_APP_EV_AVRCP_GET_PLAYER_APPLICATION_SETTING_VALUE_TEXT_RSP */
typedef struct _BtSdkGetPlayerSettingTxtRsp {
	BTUINT32	size;
	BTUINT32	subpacket_type;
	union {
		BTUINT8 id_num;					/* The number of Items, BTSDK_AVRCP_PACKET_HEAD */
		BtSdkIDStringStru id_string;	/* BTSDK_AVRCP_SUBPACKET */
	};
} BtSdkIDStringRspStru, *PBtSdkIDStringRspStru,
  BtSdkGetPlayerAppSettingAttrTxtRspStru, *PBtSdkGetPlayerAppSettingAttrTxtRspStru,
  BtSdkGetPlayerAppSettingValTxtRspStru, *PBtSdkGetPlayerAppSettingValTxtRspStru;

typedef struct _BtSdk4IDString {/* Attribute Value Entry list */
	BTUINT32 	attr_id;				/* Attributes ID */
	BTUINT16 	characterset_id;		/* Character set ID */
	BTUINT16 	len;					/* Length of value */
	BTUINT8 	value[1];				/* Value of the specified attribute */
} BtSdk4IDStringStru, *PBtSdk4IDStringStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_ELEMENT_ATTRIBUTES_RSP */
typedef struct _BtSdkGetElementAttrRsp{
	BTUINT32	size;
	BTUINT32	subpacket_type;
	union {
		BTUINT8	id_num;					/* The number of Items, BTSDK_AVRCP_PACKET_HEAD */
		BtSdk4IDStringStru id_value;	/* BTSDK_AVRCP_SUBPACKET */
	};
} BtSdkGetElementAttrRspStru, *PBtSdkGetElementAttrRspStru;

typedef struct _BtSdkGetItemAttrRspHead{
	BTUINT8		status;
	BTUINT8		attr_num;		/* The number of Item */
} BtSdkGetItemAttrRspHeadStru, *PBtSdkGetItemAttrRspHeadStru;
		
/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_ITEM_ATTRIBUTES_RSP */
typedef struct _BtSdkGetItemAttrRsp {
	BTUINT32	size;
	BTUINT32	subpacket_type;
	union {
		BtSdkGetItemAttrRspHeadStru packet_head;	/* BTSDK_AVRCP_PACKET_HEAD */
		BtSdk4IDStringStru entry;					/* BTSDK_AVRCP_SUBPACKET */
	};
} BtSdkGetItemAttrRspStru, *PBtSdkGetItemAttrRspStru;

typedef struct _BtsdkSetBrowsedPlayerRspHead{						
	BTUINT8 	status;					/* The Result of the SetBrowsedPlayer operation. If an error has occurred then this is the only field present in the response. */
	BTUINT16 	uid_counter;		
	BTUINT32 	items_num;
	BTUINT16 	characterset_id;		/* The character set ID to be displayed on CT. */
	BTUINT8 	folder_depth;			/* The number of Folder Name Length/Folder Name pairs which follow. */
} BtsdkSetBrowsedPlayerRspHeadStru, *PBtsdkSetBrowsedPlayerRspHeadStru;

typedef struct _BtsdkSetBrowsedPlayerRspItem{									
	BTUINT16 	folder_name_len;		/* The length in octets of the folder name which follows. */
	BTUINT8 	folder_name[1];
} BtsdkSetBrowsedPlayerRspItemStru, *PBtsdkSetBrowsedPlayerRspItemStru;
		
/* Type of the Parameter of BTSDK_APP_EV_AVRCP_SET_BROWSED_PLAYER_RSP */
typedef struct _BtsdkSetBrowsedPlayerRsp {
	BTUINT32	size;
	BTUINT32	subpacket_type;
	union {
		BtsdkSetBrowsedPlayerRspHeadStru packet_head;	/* BTSDK_AVRCP_PACKET_HEAD */
		BtsdkSetBrowsedPlayerRspItemStru folder_item;	/* BTSDK_AVRCP_SUBPACKET */
	};
} BtSdkSetBrowsedPlayerRspStru, *PBtSdkSetBrowsedPlayerRspStru;

typedef struct _BtSdkMediaPlayerItem {
	BTUINT16 	player_id;
	BTUINT8 	play_status;
	BTUINT8 	major_player_type;
	BTUINT32	player_subtype;
	BTUINT8 	feature_bitmask[16];
	BTUINT16 	characterset_id;		/* e.g AVRCP_CHARACTERSETID_UTF8 */
	BTUINT16 	name_len;
	BTUINT8 	name[1];
} BtSdkMediaPlayerItemStru, *PBtSdkMediaPlayerItemStru;

typedef struct _BtSdkFolderItem {
	BTUINT8 	folder_uid[8];
	BTUINT8 	folder_type;
	BTUINT8 	is_playable;
	BTUINT16 	characterset_id;
	BTUINT16 	name_len;
	BTUINT8 	name[1];
} BtSdkFolderItemStru, *PBtSdkFolderItemStru;

typedef struct _MediaElementItem {
	BTUINT8 	element_uid[8];			/* UID as defined in 6.10.3 */
	BTUINT8 	media_type;				/* e.g AVRCP_MEDIATYPE_AUDIO */
	BTUINT8 	attr_num;
	BTUINT16 	characterset_id;		/* e.g AVRCP_CHARACTERSETID_UTF8 */
	BTUINT16 	name_len;
	BTUINT8 	name[1];
} BtSdkMediaElementItemStru, *PBtSdkMediaElementItemStru;


typedef struct _BtSdkBrowsableItem{
	BTUINT16 	items_num;				/* The total number of items returned in this listing */
	BTUINT16 	uid_counter;
	BTUINT16	item_len;				/* The size of the member in union. */
	BTUINT8		item_type;
	BTUINT8 	status;					/* The Result of the GetFolderItem operation. If an error has occurred then this is the only field present in the response. */
	union {
		BtSdkMediaPlayerItemStru	player_item;	/* item_type == AVRCP_ITEMTYPE_MEDIAPLAYER_ITEM */
		BtSdkFolderItemStru			folder_item;	/* item_type == AVRCP_ITEMTYPE_FOLDER_ITEM */
		BtSdkMediaElementItemStru	element_item;	/* item_type == AVRCP_ITEMTYPE_MEDIAELEMENT_ITEM */
	};
} BtSdkBrowsableItemStru, *PBtSdkBrowsableItemStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_GET_FOLDER_ITEMS_RSP */
typedef struct _BtSdkGetFolderItemsRsp {
	BTUINT32	size;
	BTUINT32	subpacket_type;
	union {
		BtSdkBrowsableItemStru	item;			/* subpacket_type = BTSDK_AVRCP_PACKET_HEAD */
		BtSdk4IDStringStru		element_attr;	/* subpacket_type = BTSDK_AVRCP_SUBPACKET,
												when item.item_type == AVRCP_ITEMTYPE_MEDIAELEMENT_ITEM
												*/
	};
} BtSdkGetFolderItemRspStru, *PBtSdkGetFolderItemRspStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_REGISTER_NOTIFICATION_IND */
typedef struct _BtSdkRegisterNotifReq {
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		event_id;           /* ID of the event requires notification, e.g. BTSDK_AVRCP_EVENT_PLAYBACK_STATUS_CHANGED */
	BTUINT32	playback_interval;  /* Playback interval in seconds */
} BtSdkRegisterNotifReqStru, *PBtSdkRegisterNotifReqStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_TRACK_REACHED_END_NOTIF, BTSDK_APP_EV_AVRCP_TRACK_REACHED_START_NOTIF
BTSDK_APP_EV_AVRCP_NOW_PLAYING_CONTENT_CHANGED_NOTIF, BTSDK_APP_EV_AVRCP_AVAILABLE_PLAYERS_CHANGED_NOTIF */
typedef struct _BtSdkNotifNull { 
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		rsp_code;            /* Response code, e.g. BTSDK_AVRCP_RSP_NOT_IMPLEMENTED */
} BtSdkNotifNullStru, *PBtSdkNotifNullStru,
  BtSdkTrackReachEndStru, *PBtSdkTrackReachEndStru,
  BtSdkTrackReachStartStru, *PBtSdkTrackReachStartStru,
  BtSdkNowPlayingContentChangedStru, *PBtSdkNowPlayingContentChangedStru,
  BtSdkAvailablePlayerChangedStru, *PBtSdkAvailablePlayerChangedStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_VOLUME_CHANGED_NOTIF, BTSDK_APP_EV_AVRCP_PLAYBACK_STATUS_CHANGED_NOTIF
BTSDK_APP_EV_AVRCP_BATT_STATUS_CHANGED_NOTIF, BTSDK_APP_EV_AVRCP_SYSTEM_STATUS_CHANGED_NOTIF */
typedef struct _BtSdkNotifID {
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		rsp_code;
	BTUINT8		id;
} BtSdkNotifIDStru, *PBtSdkNotifIDStru,
  BtSdkVolChangedStru, *PBtSdkVolChangedStru,
  BtSdkPlayStatusChangedStru, *PBtSdkPlayStatusChangedStru,
  BtSdkBattStatusChangedStru, *PBtSdkBattStatusChangedStru,
  BtSdkSysStatusChangedStru, *PBtSdkSysStatusChangedStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_UIDS_CHANGED_NOTIF */
typedef struct _BtSdkUIDSChanged {
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		rsp_code;
	BTUINT16	uid_counter;
} BtSdkUIDSChangedStru, *PBtSdkUIDSChangedStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_PLAYBACK_POS_CHANGED_NOTIF */
typedef struct _BtSdkPlaybackPosChanged {
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		rsp_code;
	BTUINT32	pos;				/* Current playback position in millisecond. */
} BtSdkPlayPosChangedStru, *PBtSdkPlayPosChangedStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_AVAILABLE_PLAYERS_CHANGED_NOTIF */
typedef struct _BtSdkAddrPlayerChanged {
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		rsp_code;
	BTUINT16	player_id;			/* Unique Media Player ID */
	BTUINT16	uid_counter;
} BtSdkAddrPlayerChangedStru, *PBtSdkAddrPlayerChangedStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_TRACK_CHANGED_NOTIF */
typedef struct _BtSdkTrackChanged {
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		rsp_code;
	BTUINT8		identifier[8];		/* Unique Identifier to identify an element on TG, as is used for the GetElementAttributes command. */
} BtSdkTrackChangedStru, *PBtSdkTrackChangedStru;

/* Type of the Parameter of BTSDK_APP_EV_AVRCP_PLAYER_APPLICATION_SETTING_CHANGED_NOTIF */
typedef struct _BtSdkPlayerAppSetChanged {
	BTUINT32	size;               /* Size of this structure */
	BTUINT8		rsp_code;
	BTUINT8 	num;				/* Number of attributes */
	BtSdkIDPairStru id[1];			/* List of the requested attribues ID and value ID */
} BtSdkPlayerAppSetChangedStru, *PBtSdkPlayerAppSetChangedStru;

/* Parameters of Sniff_Mode command */
typedef struct _BtSdkSniffModeStru {
	BTUINT16 conn_hdl;					/* reserved, set it to 0. */
	BTUINT16 max;						/* Sniff mode max interval. */
	BTUINT16 min;						/* Sniff mode min interval. */
	BTUINT16 attempt;					/* Sniff mode attempt value. */
	BTUINT16 timeout;					/* Sniff mode timeout value. */
} BtSdkSniffModeStru;
typedef BtSdkSniffModeStru* PBtSdkSniffModeStru;

/* Parameters of Park_Mode (V1.1) or Park_State (V1.2) command */
typedef struct _BtSdkParkModeStru {
	BTUINT16 conn_hdl;					/* reserved, set it to 0. */
	BTUINT16 max;						/* Beacon max interval. */
	BTUINT16 min;						/* Beacon min interval. */
} BtSdkParkModeStru;
typedef BtSdkParkModeStru* PBtSdkParkModeStru;

/* Basic SDP Element */
typedef struct _BtSdkUUIDStru
{
    BTUINT32 Data1;
    BTUINT16 Data2;
    BTUINT16 Data3;
    BTUINT8  Data4[8];
} BtSdkUUIDStru, *PBtSdkUUIDStru;

typedef struct _BtSdkSDPSearchPatternStru
{
	BTUINT32 mask;					/*Specifies the valid bytes in the uuid*/
	BtSdkUUIDStru uuid;				/*UUID value*/
} BtSdkSDPSearchPatternStru, *PBtSdkSDPSearchPatternStru;

/* Remote service record attributes */
typedef struct _BtSdkRemoteServiceAttrStru
{
	BTUINT16 mask;									/*Decide which parameter to be retrieved*/
	union
	{
		BTUINT16 svc_class;							/* For Compatibility */
		BTUINT16 service_class;
	};												/*Type of this service record*/
	BTDEVHDL dev_hdl;								/*Handle to the remote device which provides this service.*/
	BTUINT8 svc_name[BTSDK_SERVICENAME_MAXLENGTH];	/*Service name in UTF-8*/
	BTLPVOID ext_attributes;						/*Free by the APP*/
	BTUINT16 status;
} BtSdkRemoteServiceAttrStru, *PBtSdkRemoteServiceAttrStru;

typedef struct _BtSdkRmtSPPSvcExtAttrStru 
{
	BTUINT32 size;						/*Size of BtSdkRmtSPPSvcExtAttrStru*/
	BTUINT8  server_channel;			/*Server channel value of this SPP service record*/
} BtSdkRmtSPPSvcExtAttrStru, *PBtSdkRmtSPPSvcExtAttrStru;

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

typedef struct _BtSdkFileTransferReqStru
{
	BTDEVHDL dev_hdl;		/* Handle to the remote device tries to upload/delete the file. */
	BTUINT16 operation;		/* Specify the operation on the file. 
							It can be one of the following values:
								BTSDK_APP_EV_FTP_PUT: The remote device request to upload the file.
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

typedef struct _BtSdkAppExtSPPAttrStru
{
	BTUINT32			size;									/* Size of this structure */
	BTUINT32			sdp_record_handle;						/* 32bit interger specifies the SDP service record handle */
	BtSdkUUIDStru 		service_class_128;						/* 128bit UUID specifies the service class of this service record */
	BTUINT8				svc_name[BTSDK_SERVICENAME_MAXLENGTH];	/* Service name, in UTF-8 */
	BTUINT8				rf_svr_chnl;							/* RFCOMM server channel assigned to this service record */
	BTUINT8				com_index;								/* Index of the local COM port assigned to this service record */
} BtSdkAppExtSPPAttrStru, *PBtSdkAppExtSPPAttrStru;

/* lParam for SPP */
typedef struct _BtSdkSPPConnParamStru
{
	BTUINT32 size;
	BTUINT16 mask;	//Reserved set 0
	BTUINT8 com_index;
} BtSdkSPPConnParamStru, *PBtSdkSPPConnParamStru;

/* lParam for OPP */
typedef struct _BtSdkOPPConnParamStru
{
	BTUINT32 size;									/*Size of this structure, use for verification and versioning.*/
	BTUINT8 inbox_path[BTSDK_PATH_MAXLENGTH];		/*must in UTF-8*/
	BTUINT8 outbox_path[BTSDK_PATH_MAXLENGTH];		/*must in UTF-8*/
	BTUINT8 own_card[BTSDK_CARDNAME_MAXLENGTH]; 	/*must in UTF-8*/
} BtSdkOPPConnParamStru, *PBtSdkOPPConnParamStru;

/* lParam for DUN */
typedef struct _BtSdkDUNConnParamStru
{ 
	BTUINT32 size;
	BTUINT16 mask;	//Reserved set 0
	BTUINT8 com_index;
} BtSdkDUNConnParamStru, *PBtSdkDUNConnParamStru;

/* lParam for FAX */
typedef struct _BtSdkFAXConnParamStru 
{
	BTUINT32 size;
	BTUINT16 mask;	//Reserved set 0
	BTUINT8 com_index;
} BtSdkFAXConnParamStru, *PBtSdkFAXConnParamStru;

/* Used By +COPS */
typedef struct Btsdk_HFP_COPSInfo {
	BTUINT8 	mode;				/* current mode and provides no information with regard to the name of the operator */
	BTUINT8 	format;				/* the format of the operator parameter string */
	BTUINT8		operator_len;
	BTINT8 	operator_name[1];	/* the string in alphanumeric format representing the name of the network operator */	
} Btsdk_HFP_COPSInfoStru, *PBtsdk_HFP_COPSInfoStru;

/* Used By +BINP, +CNUM, +CLIP, +CCWA */
typedef struct Btsdk_HFP_PhoneInfo {
	BTUINT8 	type;				/* the format of the phone number provided */
	BTUINT8 	service;			/* Indicates which service this phone number relates to. Shall be either 4 (voice) or 5 (fax). */
	BTUINT8 	num_len;			/* the length of the phone number provided */	
	BTINT8 		number[32];			/* subscriber number, the length shall be PHONENUM_MAX_DIGITS */	
	BTUINT8 	name_len;			/* length of subaddr */
	BTINT8 		alpha_str[1];		/* string type subaddress of format specified by <cli_validity> */	
} Btsdk_HFP_PhoneInfoStru, *PBtsdk_HFP_PhoneInfoStru;

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

/* Parameter of the BTSDK_HFP_EV_SLC_ESTABLISHED_IND and BTSDK_HFP_EV_SLC_RELEASED_IND events */
typedef struct Btsdk_HFP_ConnInfo {
    BTUINT16 role; /* 16bit UUID specifies the local role of the connection:
                          BTSDK_CLS_HANDSFREE - Local device acts as a HF. 
                          BTSDK_CLS_HANDSFREE_AG - Local device acts as a Hands-free AG. 
                          BTSDK_CLS_HEADSET - Local device acts as a HS. 
                          BTSDK_CLS_HEADSET_AG - Local device acts as a Headset AG. */
	BTDEVHDL dev_hdl;  /* Handle to the remote device. */
} Btsdk_HFP_ConnInfoStru, *PBtsdk_HFP_ConnInfoStru;

/* Used by BTSDK_HFP_EV_ATCMD_RESULT */
typedef struct Btsdk_HFP_ATCmdResult {
	BTUINT16	cmd_code;			/* Which AT command code got an error */
	BTUINT8	    result_code;		/* What result occurs, BTSDK_HFP_APPERR_TIMEOUT, CME Error Code or standard error result code */
} Btsdk_HFP_ATCmdResultStru, *PBtsdk_HFP_ATCmdResultStru;

/* lParam of Btsdk_StartClient, Btsdk_StartClientEx and Btsdk_ConnectShortCutEx; and,
   ext_attributes of BtSdkLocalServerAttrStru. */
typedef struct _BtSdkHFPUIParam {
	BTUINT32 size;      /* Must set to sizeof(BtSdkHFPConnParamStru) */
	BTUINT16 mask;	    /* Reserved, set to 0 */
	BTUINT16 features;  /* Local supported features.
	                       1) For HSP, it shall be 0.
	                       2) For HFP-HF, it can be the bit OR operation of following values:
	                          BTSDK_HF_BRSF_NREC, BTSDK_HF_BRSF_3WAYCALL, BTSDK_HF_BRSF_CLIP,
	                          BTSDK_HF_BRSF_BVRA, BTSDK_HF_BRSF_RMTVOLCTRL, BTSDK_HF_BRSF_ENHANCED_CALLSTATUS,
	                          BTSDK_HF_BRSF_ENHANCED_CALLCONTROL.
	                       3) For HFP-AG, it can be the bit OR operation of following values:
	                          BTSDK_AG_BRSF_3WAYCALL, BTSDK_AG_BRSF_NREC, BTSDK_AG_BRSF_BVRA,
	                          BTSDK_AG_BRSF_INBANDRING, BTSDK_AG_BRSF_BINP, BTSDK_AG_BRSF_REJECT_CALL,
	                          BTSDK_AG_BRSF_ENHANCED_CALLSTATUS, BTSDK_AG_BRSF_ENHANCED_CALLCONTROL,
	                          BTSDK_AG_BRSF_EXTENDED_ERRORRESULT.
	                     */
} BtSdkHFPUIParamStru, *PBtSdkHFPUIParamStru,
  BtSdkHFPConnParamStru, *PBtSdkHFPConnParamStru,
  BtSdkLocalHFPServerAttrStru, *PBtSdkHFPLocalHFPServerAttrStru;

typedef struct _BtSdk_SDAP_PNPINFO
{
	BTUINT16 size;				
	BTUINT16 mask;				
	BTUINT32 svc_hdl;			
	BTUINT16 spec_id;			
	BTUINT16 vendor_id;			
	BTUINT16 product_id;		
	BTUINT16 version_value;		
	BTUINT16 vendor_id_src;		
}BtSdk_SDAP_PNPINFO, *PBtSdk_SDAP_PNPINFO;

typedef struct _BtSdkRmtDISvcExtAttrStru
{
	BTUINT32 size;				
	BTUINT16 mask;				
	BTUINT16 spec_id;			
	BTUINT16 vendor_id;			
	BTUINT16 product_id;		
	BTUINT16 version;			
	BTBOOL  primary_record;		
	BTUINT16 vendor_id_source;	
	BTUINT16 list_size;			
	BTUINT8  str_url_list[1];
} BtSdkRmtDISvcExtAttrStru, *PBtSdkRmtDISvcExtAttrStru;


typedef struct  _BtSdkLocalServerAttrStru
{
	BTUINT16 mask;					/*Decide which parameter to be modified or retrieved*/
	BTUINT16 service_class;			/*Service class, 16bit UUID*/
	BTUINT8 svc_name[BTSDK_SERVICENAME_MAXLENGTH];	/*must in UTF-8*/
	BTUINT16 security_level;		/*Authorization, Authentication, Encryption, None*/
	BTUINT16 author_method;			/*Accept, Prompt, Reject (untrusted device), combined with security level "Authorization"*/
	BTLPVOID ext_attributes;		/*Profile specific attributes*/
	BTUINT32 app_param;				/*User defined parameters*/
} BtSdkLocalServerAttrStru, *PBtSdkLocalServerAttrStru;

/* Possible flags for member 'mask' in _BtSdkLocalServerAttrStru */
#define BTSDK_LSAM_SERVICENAME					0x0001
#define BTSDK_LSAM_SECURITYLEVEL				0x0002
#define BTSDK_LSAM_AUTHORMETHOD					0x0004
#define BTSDK_LSAM_EXTATTRIBUTES				0x0008
#define BTSDK_LSAM_APPPARAM						0x0010


typedef struct _BtSdkLocalSPPServerAttrStru 
{
	BTUINT32 size;
	BTUINT16 mask;
	BTUINT8 com_index;
} BtSdkLocalSPPServerAttrStru, *PBtSdkLocalSPPServerAttrStru;

/* Possible flags for member 'mask' in _BtSdkLocalSPPServerAttrStru */
#define BTSDK_LSPPSAM_COMINDEX					0x0001

typedef struct _BtSdkPBAPParamStru {
	BTUINT16 	mask;
	BTUINT8  	filter[8];
	BTUINT16 	max_count;
	BTUINT16 	list_offset; 
	BTUINT8  	order;
	BTUINT8  	format;
	BTUINT8  	*search_val;
	BTUINT8  	search_attrib; 
	BTUINT8  	missed_calls;
	BTUINT16 	pb_size;
} BtSdkPBAPParamStru, *PBtSdkPBAPParamStru;


typedef BTSDKHANDLE (*Btsdk_OpenFile_Func)(const BTUINT8* file_name);
typedef BTSDKHANDLE (*Btsdk_CreateFile_Func)(const BTUINT8* file_name);
typedef BTUINT32 (*Btsdk_WriteFile_Func)(BTSDKHANDLE 	file_hdl, BTUINT8* 	buf, BTUINT32 	bytes_to_write);
typedef BTUINT32 (*Btsdk_ReadFile_Func)(BTSDKHANDLE file_hdl, BTUINT8* buf, BTUINT32 len, BTBOOL *is_end);
typedef BTUINT32 (*Btsdk_GetFileSize_Func)(BTSDKHANDLE 	file_hdl);
typedef BTINT32 (*Btsdk_RewindFile_Func)(BTSDKHANDLE 	file_hdl, BTUINT32 		offset);
typedef void (*Btsdk_CloseFile_Func)(BTSDKHANDLE 	file_hdl);

typedef struct _BtSdkPBAPFileIORoutinesStru{
	Btsdk_OpenFile_Func    		open_file;
	Btsdk_CreateFile_Func  		create_file;
	Btsdk_WriteFile_Func   		write_file;
	Btsdk_ReadFile_Func    		read_file;
	Btsdk_GetFileSize_Func 		get_file_size;
	Btsdk_RewindFile_Func  		rewind_file;
	Btsdk_CloseFile_Func   		close_file;
} BtSdkPBAPFileIORoutinesStru, *PBtSdkPBAPFileIORoutinesStru;

typedef struct _BtSdkRmtPSESvcAttrStru
{
	BTUINT32 	size;
	BTUINT16 	mask;
	BTUINT8  	repositories;
} BtSdkRmtPSESvcAttrStru, *PBtSdkRmtPSESvcAttrStru;

typedef struct _BtSdkLocalPSEServerAttrStru
{
	BTUINT32 	size;
	BTUINT16	mask;
	BTUINT8	root_dir[BTSDK_PATH_MAXLENGTH + 1];
	BTUINT8  	path_delimiter[BTSDK_PBAP_MAX_DELIMITER + 1];
	BTUINT8  	repositories;
} BtSdkLocalPSEServerAttrStru, *PBtSdkLocalPSEServerAttrStru;

typedef BTUINT8*(*Btsdk_vCardParser_Open_Func)(BTSDKHANDLE 	file_hdl);
typedef BTUINT8* (*Btsdk_vCardParser_GetProperty_Func)(
													   BTUINT8* 	v_obj, 
													   BTUINT8* 	prop, 
													   BTINT32* 	len
													   );
typedef void (*Btsdk_vCardParser_Close_Func)(BTUINT8* 	v_obj);
typedef void (*Btsdk_vCardParser_FreeProperty_Func)(BTUINT8* 	buf);
typedef BTUINT8* (*Btsdk_vCardParser_FindFirstProperty_Func)(
															 BTUINT8*	v_obj, 
															 BTUINT8*	prop, 
															 BTSDKHANDLE*	find_hdl, 
															 BTINT32*		len
															 );
typedef BTUINT8*(*Btsdk_vCardParser_FindNextProperty_Func)(
														   BTSDKHANDLE 	find_hdl, 
														   BTINT32*		len
														   );
typedef void  (*Btsdk_vCardParser_FindPropertyClose_Func)(BTSDKHANDLE 	find_hdl);




typedef struct _BtSdkPBAPParserRoutinesStru{
	Btsdk_vCardParser_Open_Func              	parse_open;
	Btsdk_vCardParser_GetProperty_Func      	get_prop;
	Btsdk_vCardParser_Close_Func             	parse_close;
	Btsdk_vCardParser_FreeProperty_Func      	parse_free;
	Btsdk_vCardParser_FindFirstProperty_Func 	parse_findfirst;
	Btsdk_vCardParser_FindNextProperty_Func  	parse_findnext;
	Btsdk_vCardParser_FindPropertyClose_Func 	parse_findclose;
} BtSdkPBAPParserRoutinesStru, *PBtSdkPBAPParserRoutinesStru;

typedef BTSDKHANDLE (*Btsdk_FindFirstFile_Func)(
												const BTUINT8* 	path, 
												BTUINT8* 		file_name
												);
typedef BTINT32  (*Btsdk_FindNextFile_Func)(
											BTSDKHANDLE 	find_hdl, 
											BTUINT8* 	file_name
											);
typedef void (*Btsdk_FindFileClose_Func)(BTSDKHANDLE 	find_hdl);


typedef struct _BtSdkPBAPFindFileRoutinesStru
{
	Btsdk_FindFirstFile_Func 	find_first;
	Btsdk_FindNextFile_Func  	find_next;
	Btsdk_FindFileClose_Func 	find_close;
} BtSdkPBAPFindFileRoutinesStru, *PBtSdkPBAPFindFileRoutinesStru;

typedef BTINT32 (*Btsdk_ChangDir_Func)(const BTUINT8* 	path);
typedef BTINT32 (*Btsdk_CreateDir_Func)(const BTUINT8* 	path);


typedef struct _BtSdkPBAPDirCtrlRoutinesStru{
	Btsdk_ChangDir_Func	change_dir;
	Btsdk_CreateDir_Func 	create_dir;
} BtSdkPBAPDirCtrlRoutinesStru, *PBtSdkPBAPDirCtrlRoutinesStru;

typedef BTINT32  (*Btsdk_PBAP_GetMissedCalls_Func)(BTUINT8* 	path);


typedef struct _BtSdkPBAPSvrCBStru{
	BtSdkPBAPParserRoutinesStru    	cardparser_rtns;
	BtSdkPBAPFindFileRoutinesStru  	findfile_rtns;
	BtSdkPBAPFileIORoutinesStru	   	fileio_rtns;
	BtSdkPBAPDirCtrlRoutinesStru   	dirctrl_rtns;
	Btsdk_PBAP_GetMissedCalls_Func 	get_new_missedcalls;
} BtSdkPBAPSvrCBStru, *PBtSdkPBAPSvrCBStru;

/********************以下为解析vcard添加的结构体********************/

#define MB_MAX_DEV_NAME_LENGTH			256			// 设备的最大长度
#define MAX_SMS_VMSG_LENGTH				1024		// 短信最大body长度
#define MAX_SMS_ID_LENGTH				64			// Old:256 短信Id的最大长度
#define MAX_PHONE_NUMBER_LENGTH			40+1    	// Old:256 短信发送号码的最大长度, (40 + 1)
#define TIME_STAMP_LENGTH				32			// Old:256 时间字符串的最大长度
// 8192 -> 1024, tested min value is 512
#define MAX_CONTACT_BODY_LENGTH			1024		// 一个名片的最大长度(vCard编码)
#define MAX_CONTACT_ID_LENGTH			64			// Old:256 名片Id的最大长度  
// 8192 -> 4096
#define MAX_CALENDAR_BODY_LENGTH		4096		// 日历的最大长度(vCalendar编码)
#define MAX_CALENDAR_ID_LENGTH			64			// Old:256 日历Id的最大长度

#define SUPPORTEDWORD_MAX_LENGTH	    25			// 手机支持字段的数组的最大长度
#define ASYNC_SMS_QUEUE_MAX			    32			// 异步短信队列的最大长度
#define MAX_SYMBIAN_INI_LEN				100			// SymbianModel.ini的最大长度

#ifdef _WIN32_WCE
#define MAX_SMS_BODY_LENGTH				256			// 短信最大body长度
#define MAX_CONTACT_NAME_LENGTH			32		    // Old:512 姓名字段的最大长度
#define MAX_CONTACT_URL_LENGTH			32		    // 图片链接最大长度
#define MAX_CONTACT_TELEPHONE_LENGTH	MAX_PHONE_NUMBER_LENGTH			// Old:128 电话字段的最大长度
#define MAX_CONTACT_ADDRESS_LENGTH		32			// 地址字段的最大长度
#define MAX_CONTACT_IM_LENGTH			32			// 聊天ID
#define MAX_CONTACT_BIRTHDAY_LENGTH		16			// Old:64 生日字段的最长
#define MAX_CONTACT_GROUP_LENGTH		32			// 群组最长
#define MAX_CONTACT_MEMO_LENGTH			32			// 备注最长
#else
// 2048 -> 1024, tested min value is 256
#define MAX_SMS_BODY_LENGTH				1024		// 短信最大body长度
#define MAX_CONTACT_NAME_LENGTH			256		    // Old:512 姓名字段的最大长度
#define MAX_CONTACT_URL_LENGTH			256		    // 图片链接最大长度
#define MAX_CONTACT_TELEPHONE_LENGTH	MAX_PHONE_NUMBER_LENGTH			// Old:128 电话字段的最大长度
#define MAX_CONTACT_ADDRESS_LENGTH		256			// 地址字段的最大长度
#define MAX_CONTACT_IM_LENGTH			256			// 聊天ID
#define MAX_CONTACT_BIRTHDAY_LENGTH		16			// Old:64 生日字段的最长
#define MAX_CONTACT_GROUP_LENGTH		64			// 群组最长
#define MAX_CONTACT_MEMO_LENGTH			1024		// 备注最长
#endif

#define MAX_PRODUCT_UID_LENGTH			32			// Symbian返回地址的最大长度
#define	MAX_MANU_LENGTH					64			// Old:256 手机manufacture的最大长度
#define MAX_MODEL_LENGTH				64			// Old:256 手机model的最大长度 
#define MAX_VERSION_LENGTH				32			// Old:64 PATCH的版本最大长度
#define MAX_PLAT_LENGTH					16			// 手机软件平台的最大长度

#define MAX_CALENDAR_MEMO_LENGTH		1024		// 备注最长
#define AL_CAL_TITLE					512			// 标题最长
#define AL_CAL_DESC						2048		// 描述最长
#define AL_CAL_LOC						512			// 地点最长
#define AL_CAL_TIME						20			// 时间最长

typedef struct _MB_ContactNameItem{
	DWORD dwSize;
	CHAR szContactFirstName[MAX_CONTACT_NAME_LENGTH];
	CHAR szContactLastName[MAX_CONTACT_NAME_LENGTH];
	CHAR szMiddleName[MAX_CONTACT_NAME_LENGTH];
	CHAR szContactPrefixName[MAX_CONTACT_NAME_LENGTH];
	CHAR szContactNickName[MAX_CONTACT_NAME_LENGTH];
} MB_ContactNameItem, *PMB_ContactNameItem;

typedef struct _MB_ContactOrgItem{
	DWORD dwSize;
	CHAR szOrgName [MAX_CONTACT_ADDRESS_LENGTH];
	CHAR szDepartmentName [MAX_CONTACT_ADDRESS_LENGTH];
	CHAR szRole [MAX_CONTACT_ADDRESS_LENGTH];
	CHAR szTitle [MAX_CONTACT_ADDRESS_LENGTH];
	
} MB_ContactOrgItem, *PMB_ContactOrgItem;

typedef struct _MB_ContactPhotoItem{
	INT nPhotoType;
	CHAR szPhotoURL[MAX_CONTACT_URL_LENGTH];
	DWORD dwPhotoSize;
	BYTE* dataPhoto;
} MB_ContactPhotoItem, *PMB_ContactPhotoItem;

typedef struct _MB_ContactTelephoneItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nTelephoneType;
	CHAR szTelephone [MAX_CONTACT_TELEPHONE_LENGTH];
} MB_ContactTelephoneItem, *PMB_ContactTelephoneItem;

typedef struct _MB_ContactAddressItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nAddressType;
	CHAR szNation [MAX_CONTACT_ADDRESS_LENGTH];
	CHAR szRegion [MAX_CONTACT_ADDRESS_LENGTH];
	CHAR szCity [MAX_CONTACT_ADDRESS_LENGTH];
	CHAR szStreet [MAX_CONTACT_ADDRESS_LENGTH];
	CHAR szPostBOX [MAX_CONTACT_ADDRESS_LENGTH];
	CHAR szPostolCODE [MAX_CONTACT_ADDRESS_LENGTH];
	CHAR szAddressExtended [MAX_CONTACT_ADDRESS_LENGTH];
} MB_ContactAddressItem, *PMB_ContactAddressItem;

typedef struct _MB_ContactEmailItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nEmailType;
	CHAR szEmail [MAX_CONTACT_TELEPHONE_LENGTH];
} MB_ContactEmailItem, *PMB_ContactEmailItem;

typedef struct _MB_ContactURLItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nURLType;
	CHAR szURL[MAX_CONTACT_URL_LENGTH];
} MB_ContactURLItem, *PMB_ContactURLItem;

typedef struct _MB_ContactIMItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nIMType;
	CHAR szIMURI[MAX_CONTACT_IM_LENGTH];
} MB_ContactIMItem, *PMB_ContactIMItem;

typedef struct _MB_ContactItemEx{
	DWORD dwSize;
	CHAR szContactID[MAX_CONTACT_ID_LENGTH];
	MB_ContactNameItem * itemName;
	CHAR szContactBirthday[MAX_CONTACT_BIRTHDAY_LENGTH];
	CHAR szContactAnniversary [MAX_CONTACT_BIRTHDAY_LENGTH];
	CHAR szContactGroup [MAX_CONTACT_GROUP_LENGTH];
	CHAR szContactMemo[MAX_CONTACT_MEMO_LENGTH];
	MB_ContactOrgItem * itemOrg;
	MB_ContactPhotoItem *itemPhoto;
	DWORD* arrayCountTelephone;
	MB_ContactTelephoneItem* arrayContactTelephone;
	DWORD* arrayCountAddress;
	MB_ContactAddressItem* arrayContactAddress;
	DWORD* arrayCountEmail;
	MB_ContactEmailItem* arrayContactEmail;
	DWORD* arrayCountURL;
	MB_ContactURLItem* arrayContactURL;
	DWORD* arrayCountIM;
	MB_ContactIMItem* arrayContactIM;
} MB_ContactItemEx, *PMB_ContactItemEx;

/*************** Structure Definition ******************/
#define BTSDK_MAX_USERID_LEN 20
#define BTSDK_MAX_PWD_LEN 64
typedef struct _BtSdkObexAuthInfoStru {
    BTDEVHDL dev_hdl;     /* [in] Device handle of the OBEX server */
    BTUINT8  *target;     /* [in] Target value of the OBEX server */
    BTUINT8  target_len;  /* [in] Length of the target buffer */
    BTBOOL   pwd_only;    /* [in] whether the user id is needed */
	BTUINT8  user_id[BTSDK_MAX_USERID_LEN+1];		/* [out] the user id string	*/
	BTUINT8	 pwd[BTSDK_MAX_PWD_LEN+1];				/* [out] the password string */
} BtSdkObexAuthInfoStru, *PBtSdkObexAuthInfoStru;


//LE Structure
typedef enum _BTSDK_GATT_DESCRIPTOR_TYPE {
	CharacteristicExtendedProperties,
		CharacteristicUserDescription,
		ClientCharacteristicConfiguration,
		ServerCharacteristicConfiguration,
		CharacteristicFormat,
		CharacteristicAggregateFormat,
		CustomDescriptor  
} BTSDK_GATT_DESCRIPTOR_TYPE;

typedef enum _BTSDK_GATT_EVENT_TYPE {
	CharacteristicValueChangedEvent
} BTSDK_GATT_EVENT_TYPE;

typedef struct _BtsdkGATTUUIDStru {
	BTINT32 	IsShortUuid;
	BTUINT16  ShortUuid;
	BtSdkUUIDStru	LongUuid;
} BtsdkGATTUUIDStru, *PBtsdkGATTUUIDStru;

typedef struct _BtsdkGATTServiceStru {
	BtsdkGATTUUIDStru 	ServiceUuid;
	BTUINT16		    AttributeHandle;
} BtsdkGATTServiceStru, *PBtsdkGATTServiceStru;

typedef struct _BtsdkGATTCharacteristicStru {
	BTUINT16      ServiceHandle;
	BtsdkGATTUUIDStru CharacteristicUuid;
	BTUINT16      AttributeHandle;
	BTUINT16      CharacteristicValueHandle;
	BTINT32     IsBroadcastable;
	BTINT32     IsReadable;
	BTINT32     IsWritable;
	BTINT32     IsWritableWithoutResponse;
	BTINT32     IsSignedWritable;
	BTINT32     IsNotifiable;
	BTINT32     IsIndicatable;
	BTINT32     HasExtendedProperties;
} BtsdkGATTCharacteristicStru, *PBtsdkGATTCharacteristicStru;

typedef struct _BtsdkGATTCharacteristicValueStru {
	BTUINT32 DataSize;
	BTUCHAR Data[1];
} BtsdkGATTCharacteristicValueStru, *PBtsdkGATTCharacteristicValueStru;

typedef struct _BtsdkGATTDescriptorStru {
	BTUINT16                      ServiceHandle;
	BTUINT16                      CharacteristicHandle;
	BTSDK_GATT_DESCRIPTOR_TYPE DescriptorType;
	BtsdkGATTUUIDStru                 DescriptorUuid;
	BTUINT16                      AttributeHandle;
} BtsdkGATTDescriptorStru, *PBtsdkGATTDescriptorStru;

typedef struct _BtsdkGATTDescriptorValueStru {
	BTSDK_GATT_DESCRIPTOR_TYPE DescriptorType;
	BtsdkGATTUUIDStru                 DescriptorUuid;
	union {
		struct {
			BTINT32 IsReliableWriteEnabled;
			BTINT32 IsAuxiliariesWritable;
		} CharacteristicExtendedProperties;
		struct {
			BTINT32 IsSubscribeToNotification;
			BTINT32 IsSubscribeToIndication;
		} ClientCharacteristicConfiguration;
		struct {
			BTINT32 IsBroadcast;
		} ServerCharacteristicConfiguration;
		struct {
			BTUCHAR       Format;
			BTUCHAR       Exponent;
			BtsdkGATTUUIDStru Unit;
			BTUCHAR       NameSpace;
			BtsdkGATTUUIDStru Description;
		} CharacteristicFormat;
	};
	BTUINT32                       DataSize;
	BTUCHAR                       Data[1];
} BtsdkGATTDescriptorValueStru, *PBtsdkGATTDescriptorValueStru;

#endif