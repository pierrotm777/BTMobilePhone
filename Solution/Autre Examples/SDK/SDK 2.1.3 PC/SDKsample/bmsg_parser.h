#ifndef _BMSG_PARSER_H_
#define _BMSG_PARSER_H_
#include <Windows.h>

#define BT_FILE_HDL UINT8*

typedef struct PropElemNodeStru
{
	char* pPropName;
	DWORD  wNameLen;
	char* pPropData;
	DWORD  wDataSize;
	char* pListParams;
	struct PropElemNodeStru* next;
}PropElemNode,*pPropElemNode;

typedef struct Stru_ParamNode
{
	char* param_name;
	char* param_val;
	struct Stru_ParamNode* next;
}Stru_ParamNode,*pStru_ParamNode;

typedef struct BMsgContentStru
{
	pPropElemNode pListBodyProp;
	pPropElemNode pListMsgData;			/*Parse operation will be list(pairs of <name, value> ElemDate, and List of data blocks;*/
}BMsgContent,*pBMsgContent;
/*A BMessage file will be parsed into BMsgEnvelopeStru structure*/
typedef struct BMsgEnvelopeStru
{
	pPropElemNode pListReciever;
	struct BMsgEnvelopeStru* pChild;  /*self nested*/
	pBMsgContent pContent; /*pChild and pStruContent, only one pointer is not NULL,mutually exclusive*/
}BMsgEnvelope, *pBMsgEnvelope;

/*UserData of Gramer BMParser*/
typedef struct BMsgObjectStru
{
	UINT16* msg_handle;
	pPropElemNode pListMsgProp;
	pPropElemNode  pListSender;
	pBMsgEnvelope pEnvelope;
}BMsgObjectStru,*pBMsgObjectStru;

typedef struct TokenParserStru
{
	char* pDataLineBuf;
	DWORD dwLbSize;
	DWORD dwLbLen;
	void** pParams;
	WORD nParams;
	char* pPropName;
	DWORD dwNameSize;
	char* pPropData;
	DWORD dwDataSize;
	char* pMsgObjCB;
	UINT uCurState;
	BOOL bIsDataStarted;
	BOOL bIsWrap;
	BOOL bIsDataOver;
	BOOL bIsEsc;
}TokenParser,*pTokenParser;
 typedef struct GrammerParserStru
 {
	 UINT uCurState;
	 char* PMsgObjCB;
	 UINT nNestedDepth; /*bMessage envelope nested depth*/
 }GrammerParser, *pGrammerParser;
typedef struct BMsgObjStru
{
	pBMsgObjectStru pUserData; /*Pointer to  BMsgObjectStru*/
	char* pParserToken;
	char* pParserGrammer;
	UINT uErrorCode;
}BMsgObj,*pBMsgObj;

char* BMessageParser(BT_FILE_HDL file_hdl);  /*Return VBMsgObj*/
char* GetPropertValue(char* vBMsgObj, UINT uPropId, DWORD* len);
char* FindFirstPropertyValue(char* vBMsgObj, UINT uPropId, char** FindHdl, DWORD* len);
char* FindNextPropertyValue(char* FindHdl, DWORD* len);
void EndFindPropertyValue(char* FindHdl);
BOOL SetPropertyValue(char* vBMsgObj, UINT uPropId, char* pPropData, WORD wDataLen);
BOOL SetPropertyValueEx(char* vBMsgObj, int nLayer, UINT uPropId, char* pPropData, WORD wDataLen);
void WriteVBMsgToFile(BT_FILE_HDL f_hdl, char* vBMsgObj);
void BMessageClose(char* VObj);
char* GetDateTime(char* szMsg);

#define IDX_PROP_BMSG_VERSION	0x01
#define IDX_RPOP_BMSG_READSTATUS	0x02
#define IDX_RPOP_BMSG_TYPE		0x03
#define IDX_RPOP_BMSG_FOLDER		0x04
#define IDX_RPOP_BMSG_SENDER_VESION  0x05
#define IDX_RPOP_BMSG_SENDER_N		0x06
#define IDX_RPOP_BMSG_SENDER_FN		0x07
#define IDX_RPOP_BMSG_SENDER_TEL	0x08
#define IDX_RPOP_BMSG_SENDER_EMAIL	0x09
#define IDX_RPOP_BMSG_RECIPIENT_VESION	0x0A
#define IDX_RPOP_BMSG_RECIPIENT_N		0x0B
#define IDX_RPOP_BMSG_RECIPIENT_FN		0x0C
#define IDX_RPOP_BMSG_RECIPIENT_TEL		0x0D
#define IDX_RPOP_BMSG_RECIPIENT_EMAIL	0x0E

#define IDX_RPOP_BMSG_PART_ID		0x0F
#define IDX_RPOP_BMSG_BODY_ENCODING	 0x10
#define IDX_RPOP_BMSG_BODY_CHARSET		0x11
#define IDX_RPOP_BMSG_BODY_LANGUAGE  0x12
#define IDX_RPOP_BMSG_BODY_LENGTH	0x13
#define IDX_PROP_BMSG_BODY_CONTENT		0x14  /*BMessage content*/

typedef struct BMsgPropertyInfoStru
{
	char* pPropName;
	UINT uPropId;
}BMsgPropertyInfo;
static const BMsgPropertyInfo BMsgPropMapTbl[] = 
{
	{"VERSION",		IDX_PROP_BMSG_VERSION},
	{"STATUS",		IDX_RPOP_BMSG_READSTATUS},
	{"TYPE",		IDX_RPOP_BMSG_TYPE},
	{"FOLDER",		IDX_RPOP_BMSG_FOLDER},
	{"VERSION",		IDX_RPOP_BMSG_SENDER_VESION},
	{"N",			IDX_RPOP_BMSG_SENDER_N},
	{"FN",			IDX_RPOP_BMSG_SENDER_FN},
	{"TEL",			IDX_RPOP_BMSG_SENDER_TEL},
	{"EMAIL",		IDX_RPOP_BMSG_SENDER_EMAIL},
	{"VERSION",		IDX_RPOP_BMSG_RECIPIENT_VESION},
	{"N",			IDX_RPOP_BMSG_RECIPIENT_N},
	{"FN",			IDX_RPOP_BMSG_RECIPIENT_FN},
	{"TEL",			IDX_RPOP_BMSG_RECIPIENT_TEL},
	{"EMAIL",		IDX_RPOP_BMSG_RECIPIENT_EMAIL},
	{"PARTID",		IDX_RPOP_BMSG_PART_ID},
	{"ENCODING",	IDX_RPOP_BMSG_BODY_ENCODING},
	{"CHARSET",		IDX_RPOP_BMSG_BODY_CHARSET},
	{"LANGUAGE",	IDX_RPOP_BMSG_BODY_LANGUAGE},
	{"LENGTH",		IDX_RPOP_BMSG_BODY_LENGTH},
	{"MSG-CONTENT",			IDX_PROP_BMSG_BODY_CONTENT}, 
};
#define PROP_MAPTBL_SIZE (sizeof(BMsgPropMapTbl)/sizeof(BMsgPropertyInfo))

typedef struct Stru_List
{
	void* pData;
	struct Stru_List* next;
}Stru_List,*pStru_List;
typedef struct BMsgFindDataStru{
	UINT uPropId;
	char* pListHead;  /*Pointer to Stru_Find*/
	char* pCurNode;
}BMsgFindData, *pBMsgFindData;
#endif