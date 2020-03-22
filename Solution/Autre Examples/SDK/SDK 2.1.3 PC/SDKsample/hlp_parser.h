
#ifndef _HLP_PARSER_H_
#define _HLP_PARSER_H_
#define BMSG_FILE_HDL		DWORD
#define IVALIDE_FILE_HDL 0

#define FILE_BUF_SIZE 1024

/*error code define*/
#define PARSER_OK 0
#define ERR_IVALID_FILEHDL 1
#define ERR_EMPTY_FILE	 2
#define ERR_INVALID_MSGFMT 3

/*hlp functions*/
char* BMsgTokenParser_Init(pBMsgObj pMsgObj);
char* BMsgGrammerParser_Init(pBMsgObj pMsgObj);
void  hlp_BMsgTokenParser_DeInit(char* pTokParser);
void  hlp_BMsgGrammerParser_DeInit(char* pGramParser);
void hlp_BMsgUserData_DeInit(char* pObj);
void BMsgObjUserData_Init(pBMsgObj pMsgObj);
void  BMsgTokenParser_ParseFile(char* TokParser, BT_FILE_HDL file_hdl);
BT_FILE_HDL BMsg_OpenFile(const UINT16* path);
DWORD BMsg_WriteFile(BT_FILE_HDL file_hdl, char* buf, DWORD buf_len);
BT_FILE_HDL BMsg_CreateFile(UINT16* f_path);
DWORD BMsg_ReadFile(BT_FILE_HDL file_hdl, char* buf, DWORD buf_len, BOOL* is_end);
DWORD BMsg_GetFileSize(BT_FILE_HDL file_hdl);
void BMsg_CloseFile(BT_FILE_HDL file_hdl);
char* hlp_GetJmpFunFrom_TokenMatrix(char* pTParser, UINT uEvt);
char* hlp_GetJmpFunFrom_GramMatrix(char* pGParser, UINT uEvent);
BOOL hlp_AddChar2LineBuf(pTokenParser pTParser, char c);
void hlp_RecordePropNamePos(pTokenParser pTParser);
void hlp_CalcPropNameAddress(pTokenParser pTParser);
void hlp_CalcPropPrmsAddress(pTokenParser pTParser);
void hlp_CalcPropDataAddress(pTokenParser pTParser);
BOOL hlp_TokParserProcessElem_CB(char* tparser, char* pPropName, DWORD nNameLen, void** pPrams, WORD nPrams, char* pPropData, DWORD nDataLen);
void hlp_SetElemNodeData(pPropElemNode pElemNode, char* prop_name, DWORD name_len, void** p_params, int n_params, char* prop_value, DWORD value_size);
void hlp_ReleaseTokParserBuf(pTokenParser pTParser);
void hlp_RecordParamNamePos(pTokenParser pTParser);
void hlp_RecordParamValuePos(pTokenParser pTParser);
void hlp_RecordPropDataPos(pTokenParser pTParser);
void hlp_FreeElemNodeList(pPropElemNode pList);
void hlp_FreeParamNodeList(pStru_ParamNode pList);
UINT GetParserResultCode(char* obj);
char* hlp_GetBMsgPropertyValue(pPropElemNode pListElem, UINT uPropId, DWORD* len);
char* hlp_GetBMsgContentInfoValue(pBMsgEnvelope pListEnve, UINT uPropId, DWORD* len);
char* hlp_GetBMsgBodyData(pPropElemNode pListElem, UINT uPropId, DWORD* len);
void hlp_InsertNode2ListFind(pBMsgFindData pList_fd, char* pNode);
char* hlp_FindFirstBMsgPropValue(pPropElemNode pHead, UINT uPropId, char** pFindStru, DWORD* pLen);
void hlp_CreateFindHdlFromId(pPropElemNode pHead, UINT uPropId, char** pFindStru);
char* hlp_FindFirstBMsgContentPropValue(pBMsgEnvelope pHeadEnve, UINT uPropId, char** FindHdl, DWORD* pLen);
char* hlp_FindFirstBMsgContentData(pBMsgEnvelope pHeadEnve, UINT uPropId, char** FindHdl, DWORD* pLen);
char* hlp_FindFirstBMsgRecipientPropValue(pBMsgEnvelope pHeadContent, UINT uPropId, char** findhdl, DWORD* pLen);
void hlp_FreeStruList(pStru_List plist);
void hlp_SetBMsgContentPropertyValue(pBMsgContent pListHead, UINT uPropId, char* pDataValue, WORD wDataSize);
BOOL hlp_SetBMsgPropertyValue(char* vBMsgObj, UINT uPropId, char* pPropData, WORD wDataLen);
BOOL hlp_SetBMsgSenderPropValue(char* vBMsgObj, UINT uPropId, char* pPropData, WORD wDataLen);;
void hlp_SetBMsgBodyData(pBMsgContent pListHead, UINT uPropId, char* pDataValue, WORD wDataSize);
void hlp_WritePropElemToFile(BT_FILE_HDL f_hdl, pPropElemNode pElemList);
void hlp_WriteEnvlopeToFile(BT_FILE_HDL f_hdl, pBMsgEnvelope pEnv);
char* hlp_GetBMsgRecipentInfoValue(pBMsgEnvelope pEnv, UINT uPropId, DWORD* pLen);
void hlp_TokenParser_ParseBuf(char* tparser, char* pBuf, DWORD dwBufLen);
char* GetPropNameFromId(UINT uId);
void hlp_FreeBMsgEnvelop(pBMsgEnvelope pEnve);
BOOL hlp_setBMsgRecepientValue(pBMsgEnvelope pEnveNode, UINT uPropId, char* pDataValue, WORD wDataSize);
#endif

