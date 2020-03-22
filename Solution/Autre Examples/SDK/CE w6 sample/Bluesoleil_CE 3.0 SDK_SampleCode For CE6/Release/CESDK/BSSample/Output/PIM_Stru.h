#ifndef _PIM_STRU_H
#define _PIM_STRU_H

#include "PIM_Macro.h"
#include "windef.h"

///////////////////////////////////////////
//PPBDATA
typedef struct _PB_ContactNameItem{
	DWORD dwSize;
	CHAR szContactFirstName[PIM_CONTACT_NAME_LENGTH];
	CHAR szContactLastName[PIM_CONTACT_NAME_LENGTH];
	CHAR szMiddleName[PIM_CONTACT_NAME_LENGTH];
	CHAR szContactPrefixName[PIM_CONTACT_NAME_LENGTH];
	CHAR szContactNickName[PIM_CONTACT_NAME_LENGTH];
} PB_ContactNameItem, *PPB_ContactNameItem;

typedef struct _PB_ContactOrgItem{
	DWORD dwSize;
	CHAR szOrgName [PIM_CONTACT_ADDRESS_LENGTH];
	CHAR szDepartmentName [PIM_CONTACT_ADDRESS_LENGTH];
	CHAR szRole [PIM_CONTACT_ADDRESS_LENGTH];
	CHAR szTitle [PIM_CONTACT_ADDRESS_LENGTH];

} PB_ContactOrgItem, *PPB_ContactOrgItem;

typedef struct _PB_ContactPhotoItem{
	INT nPhotoType;
	CHAR szPhotoURL[PIM_CONTACT_URL_LENGTH];
	DWORD dwPhotoSize;
	BYTE* dataPhoto;
} PB_ContactPhotoItem, *PPB_ContactPhotoItem;

typedef struct _PB_ContactTelephoneItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nTelephoneType;
	CHAR szTelephone [PIM_CONTACT_TELEPHONE_LENGTH];
} PB_ContactTelephoneItem, *PPB_ContactTelephoneItem;

typedef struct _PB_ContactAddressItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nAddressType;
	CHAR szNation [PIM_CONTACT_ADDRESS_LENGTH];
	CHAR szRegion [PIM_CONTACT_ADDRESS_LENGTH];
	CHAR szCity [PIM_CONTACT_ADDRESS_LENGTH];
	CHAR szStreet [PIM_CONTACT_ADDRESS_LENGTH];
	CHAR szPostBOX [PIM_CONTACT_ADDRESS_LENGTH];
	CHAR szPostolCODE [PIM_CONTACT_ADDRESS_LENGTH];
	CHAR szAddressExtended [PIM_CONTACT_ADDRESS_LENGTH];
} PB_ContactAddressItem, *PPB_ContactAddressItem;

typedef struct _PB_ContactEmailItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nEmailType;
	CHAR szEmail [PIM_CONTACT_TELEPHONE_LENGTH];
} PB_ContactEmailItem, *PPB_ContactEmailItem;

typedef struct _PB_ContactURLItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nURLType;
	CHAR szURL[PIM_CONTACT_URL_LENGTH];
} PB_ContactURLItem, *PPB_ContactURLItem;

typedef struct _PB_ContactIMItem{
	DWORD dwSize;
	BOOL bPreferred;
	INT nIMType;
	CHAR szIMURI[PIM_CONTACT_IM_LENGTH];
} PB_ContactIMItem, *PPB_ContactIMItem;

typedef struct _PBDATA{
	DWORD dwSize;
	CHAR szContactID[PIM_CONTACT_ID_LENGTH];
	PB_ContactNameItem * itemName;
	CHAR szContactBirthday[PIM_CONTACT_BIRTHDAY_LENGTH];
	CHAR szContactAnniversary [PIM_CONTACT_BIRTHDAY_LENGTH];
	CHAR szContactGroup [PIM_CONTACT_GROUP_LENGTH];
	CHAR szContactMemo[PIM_CONTACT_MEMO_LENGTH];
	PB_ContactOrgItem * itemOrg;
	PB_ContactPhotoItem *itemPhoto;
	DWORD* arrayCountTelephone;
	PB_ContactTelephoneItem* arrayContactTelephone;
	DWORD* arrayCountAddress;
	PB_ContactAddressItem* arrayContactAddress;
	DWORD* arrayCountEmail;
	PB_ContactEmailItem* arrayContactEmail;
	DWORD* arrayCountURL;
	PB_ContactURLItem* arrayContactURL;
	DWORD* arrayCountIM;
	PB_ContactIMItem* arrayContactIM;
} PBDATA, *PPBDATA;
///////////////////////////////////////////

typedef struct _SMSDataInfo{
	int	 ID;	
	int  index;										//the id of SMS in phone
	int	 type;										//the type of SMS in phone: SIM or ME
	int	 readState;									//the state of SMS:SMS_UNREAD or SMS_READ
	int  locked;									//Locked message could not be deleted
	int  position;                                  //SIM or Phone
	TCHAR szSmsBody[PIM_SMS_BODY_LENGTH];			//content of SMS
	TCHAR szSmsCallerID[PIM_PHONE_NUMBER_LENGTH];	//phone number of caller
	TCHAR timeStamp[PIM_TIME_STAMP_LENGTH];			//time
	int wBoxNumber;									//state of box, 0:inbox, 1:outbox
}SMSDATA, *PSMSDATA;

typedef struct _PHONELIST 
{
	TCHAR manu[64];
	TCHAR model[64];
}PHONELIST , *PPHONELIST ;

typedef void (*PPIMCB)(PSMSDATA pSMSData);

#endif