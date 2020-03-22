/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2005 IVT Corporation
*
* All rights reserved.
*
---------------------------------------------------------------------------*/
 
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Module Name:
    hfp_tst.c
Abstract:

Revision History:


---------------------------------------------------------------------------*/

#include "sdk_tst.h"
#include "profiles_tst.h"
#include "Btsdk_Stru.h"

/* current remote HF device handle */ 
static BTDEVHDL s_currRmtHFDevHdl = BTSDK_INVALID_HANDLE;
/* current remote device's HF service handle */
static BTSHCHDL s_currHFSvcHdl = BTSDK_INVALID_HANDLE;
/* current remote HF AG connection handle */
static BTCONNHDL s_currAGConnHdl = BTSDK_INVALID_HANDLE;
/* current HF connection handle */
static BTCONNHDL s_currHFConnHdl = BTSDK_INVALID_HANDLE;
/* current HF AG's state */
static BTUINT8 s_currHFstate = BTSDK_AGAP_ST_IDLE;
/* voice tag status */
static BTUINT8 s_hf_bvra_enable = 0; 

static BTSVCHDL s_hfag_svc = BTSDK_INVALID_HANDLE;
static BTSVCHDL s_hsag_svc = BTSDK_INVALID_HANDLE;	// Headset AG service
//static BTSVCHDL s_cur_ag_hconn = BTSDK_INVALID_HANDLE;

static BTSVCHDL s_hf_svc = BTSDK_INVALID_HANDLE;
static BTSVCHDL s_hs_svc = BTSDK_INVALID_HANDLE;


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the event processing callback function of the application.
Arguments:
	event [in] event ID
    *pArg [in] *pArg is different according to different event.   
    dwArg [in] the length of pArg.
Return:
	void 
---------------------------------------------------------------------------*/

void Test_HfpAgCallbackFunc(BTCONNHDL hdl, BTUINT16 event, BTUINT8 *param, BTUINT16 param_len)
{
	BTUINT16 usAGStatus = BTSDK_FALSE;
	PBtsdk_HFP_ConnInfoStru conn_info = NULL;
	BTUINT8 *name = NULL;
	
	switch (event)
	{
	case BTSDK_HFP_EV_SLC_ESTABLISHED_IND:
		s_currAGConnHdl = hdl;
		printf("The SLC link hasn't been established yet!\n >");
		conn_info = (PBtsdk_HFP_ConnInfoStru)param;
		name = (BTUINT8*)malloc(BTSDK_DEVNAME_LEN);
		memset(name, 0, BTSDK_DEVNAME_LEN);
		Btsdk_UpdateRemoteDeviceName(conn_info->dev_hdl, name, NULL);
		printf("HF connection with %s is created, local role: %04x\n>", name, conn_info->role);
		free(name);
		break;
	case BTSDK_HFP_EV_SLC_RELEASED_IND:
		s_currAGConnHdl = BTSDK_INVALID_HANDLE;
		printf("The SLC link has not been established yet!\n >");
		conn_info = (PBtsdk_HFP_ConnInfoStru)param;
		name = (BTUINT8*)malloc(BTSDK_DEVNAME_LEN);
		memset(name, 0, BTSDK_DEVNAME_LEN);
		Btsdk_UpdateRemoteDeviceName(conn_info->dev_hdl, name, NULL);
		printf("HF connection with %s is released, local role: %04x\n>", name, conn_info->role);
		free(name);
		break;
	case BTSDK_HFP_EV_AUDIO_CONN_ESTABLISHED_IND:
		printf("AG-->SCO Audio Connection is Established. SCO connection handle %04x.\n>", *(BTUINT16*)param);
		/*
		To Do: Switch local audio path to Bluetooth SCO/eSCO link
		*/
		break;
	case BTSDK_HFP_EV_AUDIO_CONN_RELEASED_IND:
		printf("AG-->SCO Audio Connection is released. SCO connection handle %04x.\n>", *(BTUINT16*)param);
		/*
		To Do: Switch local audio path to local voice adapter
		*/
		break;
	 case BTSDK_HFP_EV_STANDBY_IND:
		 printf("ACL link has been established, but SCO link has not been yet.\n>");
		 s_currHFstate = BTSDK_AGAP_ST_STANDBY;
		break;
	case BTSDK_HFP_EV_RINGING_IND:
		printf("AG is in ringing state.\n>");
		s_currHFstate = BTSDK_AGAP_ST_RINGING;
		break;
	case BTSDK_HFP_EV_OUTGOINGCALL_IND:
		printf("AG is in outgoing state.\n>");
		s_currHFstate = BTSDK_AGAP_ST_OUTGOINGCALL;
		break;
	case BTSDK_HFP_EV_ANSWER_CALL_REQ:
		if (*param == BTSDK_HFP_TYPE_INCOMING_CALL)
		{
			printf("AG-->HF requests the AG to accept the incoming call.\n>");
			/*
			To Do: Accept the incoming call.
			*/
		}
		else // BTSDK_HFP_TYPE_HELDINCOMING_CALL
		{
			printf("AG-->HF requests the AG to accept the held incoming call.\n>");
			/*
			To Do: Accept the held incoming call.
			*/
		}
		s_currHFstate = BTSDK_AGAP_ST_ONGOINGCALL;
		break;
	case BTSDK_HFP_EV_DTMF_REQ:
		printf("Notifies AG to transmit the specific DTMF code\n");
		break;
	case BTSDK_HFP_EV_CANCEL_CALL_REQ:
		printf("SCO connection is disconnected\n");
		switch (*param)
		{
		case BTSDK_HFP_TYPE_ALL_CALLS:
			printf("AG-->HF requests the AG to release all the existing calls except the held one.\n>");
			/*
			To Do: Release all the existing (active, incoming, waiting and outgoing) calls.
			*/
			break;
		case BTSDK_HFP_TYPE_INCOMING_CALL:
			printf("AG-->HF requests the AG to reject the incoming call.\n>");
			/*
			To Do: Reject the incoming call.
			*/
			break;
		case BTSDK_HFP_TYPE_HELDINCOMING_CALL:
			printf("AG-->HF requests the AG to reject the held incoming call.\n>");
			/*
			To Do: Reject the held incoming call.
			*/
			break;
		case BTSDK_HFP_TYPE_OUTGOING_CALL:
			printf("AG-->HF requests the AG to terminate the outgoing call.\n>");
			/*
			To Do: Terminate the outgoing call.
			*/
			break;
		case BTSDK_HFP_TYPE_ONGOING_CALL:
			printf("AG-->HF requests the AG to release the ongoing call.\n>");
			/*
			To Do: Release the ongoing call.
			*/
			break;
		default:
			break;
		}
		s_currHFstate = BTSDK_AGAP_ST_IDLE;
		break;
	case BTSDK_HFP_EV_HF_DIAL_REQ: /* HF/HS calls out */
		if (*param != 0)
		{
			printf("phoneNum = %s\n", param);
		}
		printf("AG-->HF requests to dial a specific number.\n>");
		Btsdk_AGAP_DialRsp(hdl, BTSDK_HFP_OK);	// A simple demo only, assuming that OK is received from network
		s_currHFstate = BTSDK_AGAP_ST_OUTGOINGCALL;
		break;
	case BTSDK_HFP_EV_HF_MEM_DIAL_REQ:
		printf("AG-->HF requests to dial the number stored in the specific location.\n>");
		Btsdk_AGAP_DialRsp(hdl, BTSDK_HFP_OK);	// A simple demo only, assuming that OK is received from network
		break;
	case BTSDK_HFP_EV_NETWORK_AVAILABLE_IND:
		printf("HF-->Cellular Network is available!\n>");
		break;
	case BTSDK_HFP_EV_NETWORK_UNAVAILABLE_IND:
		printf("HF-->Cellular Network is unavailable!\n>");
		break;
	case BTSDK_HFP_EV_EXTEND_CMD_IND:
		printf("Extend command is received\n");
		Btsdk_HFP_ExtendCmd(hdl,"\r\nERROR\r\n", (BTUINT16)strlen("\r\nERROR\r\n"), 500);
		break;
	default:
		printf("This event is not handled in this sample\n");
		break;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the event processing callback function of the application.
Arguments:
	msgid [in] event ID
    *pArg [in] *pArg is different according to different event.   
    dwArg [in] the length of pArg.
Return:
	void 
---------------------------------------------------------------------------*/
void Test_HfpAPCallbackFunc(BTCONNHDL hdl, BTUINT16 event, BTUINT8 *param, BTUINT16 param_len)
{
	PBtsdk_HFP_ATCmdResultStru res = (PBtsdk_HFP_ATCmdResultStru)param;

	BTUINT8 *buf = NULL;
	buf = malloc(MAX_PATH);

	switch (event)
	{
	case BTSDK_HFP_EV_SLC_ESTABLISHED_IND:
		{
			PBtsdk_HFP_ConnInfoStru pHFAPConnInfo = (PBtsdk_HFP_ConnInfoStru)param;
			s_currHFConnHdl = hdl;
			printf("HF connection is created. \n>");		
			break;
		}
	case BTSDK_HFP_EV_SLC_RELEASED_IND:
		{
			s_currHFConnHdl = BTSDK_INVALID_HANDLE;
			printf("HF connection is released.\n>");
			break;
		}
	case BTSDK_HFP_EV_AUDIO_CONN_ESTABLISHED_IND:
		{
			printf("The SCO Link has been established. The SCO connection handle is %04x.\n>", *(BTUINT16*)param);
			break;
		}
	case BTSDK_HFP_EV_AUDIO_CONN_RELEASED_IND:
		{
			printf("The SCO Link has been released.\n>");
			break;
		}
	case BTSDK_HFP_EV_STANDBY_IND:
		{
			printf("\n>");
			break;
		}
	case BTSDK_HFP_EV_RINGING_IND:
		{
			printf("HF AG is Ringing...\n>");
			if (*param == 1)
			{
				printf("Headset connection\n>");
			}
			else
			{
				printf("Handsfree connection\n>");
			}
			break;
		}
	case BTSDK_HFP_EV_ONGOINGCALL_IND:
		{
			printf("Ongoing Call...\n>");
			break;
		}

	case BTSDK_HFP_EV_OUTGOINGCALL_IND:
		{
			printf("Outgoing Call...\n>");
			break;
		}
	case BTSDK_HFP_EV_TERMINATE_LOCAL_RINGTONE_IND:
		{
			printf("Terminate local ringtone Call\n>");
			break;
		}
	case BTSDK_HFP_EV_VOICE_RECOGN_ACTIVATED_IND:
		{
			printf("HF-->Voice recognition is activated locally or by the AG!\n>");
			s_hf_bvra_enable = 1;
			break;
		}
	case BTSDK_HFP_EV_VOICE_RECOGN_DEACTIVATED_IND:
		{
			printf("HF-->Voice recognition is deactivated locally or by the AG!\n>");
			s_hf_bvra_enable = 0;
			break;
		}
	case BTSDK_HFP_EV_NETWORK_AVAILABLE_IND:
		{
			printf("Cellular Network is available!\n>");
			break;
		}
	case BTSDK_HFP_EV_NETWORK_UNAVAILABLE_IND:
		{
			printf("Cellular Network is unavailable!\n>");
			break;
		}	
	case BTSDK_HFP_EV_SPKVOL_CHANGED_IND:
		{
			printf("New speaker gain is %d\n>", *param);
			break;
		}
	case BTSDK_HFP_EV_MICVOL_CHANGED_IND:
		{
			printf("New micvol gain is %d\n>", *param);
			break;
		}
	case BTSDK_HFP_EV_VOICETAG_PHONE_NUM_IND:
		{
			PBtsdk_HFP_PhoneInfoStru subscriber_info = (PBtsdk_HFP_PhoneInfoStru)param;
			memcpy(buf, subscriber_info->number, subscriber_info->num_len); 
			buf[subscriber_info->num_len] = 0;
			printf("HF-->The phone number for this voice tag is: %s\n>", buf);
			break;
		}
	case BTSDK_HFP_EV_EXTEND_CMD_IND:
		{
			printf("Receiving extended AT command: %s\n>", param);
			break;
		}
	case BTSDK_HFP_EV_ATCMD_RESULT:
		{
			printf("Receiving  AT command: 0x%04x, 0x%04x\n>", res->cmd_code, res->result_code);
			break;
		}
	case BTSDK_HFP_EV_CLIP_IND:
		{
			PBtsdk_HFP_PhoneInfoStru call_info = (PBtsdk_HFP_PhoneInfoStru)param;
			if (call_info->num_len != 0)
			{
				memcpy(buf, call_info->number, call_info->num_len);
				buf[call_info->num_len] = 0;
				printf("HF-->Calling number is: %s\n>", buf);
			}
			if (call_info->name_len != 0)
			{
				memcpy(buf, call_info->alpha_str, call_info->name_len);
				buf[call_info->name_len] = 0;
				printf("HF-->Calling name is: %s\n>", buf);
			}
			break;
		}
	case BTSDK_HFP_EV_NETWORK_OPERATOR_IND:
		{
			PBtsdk_HFP_COPSInfoStru network_operator = (PBtsdk_HFP_COPSInfoStru)param;
			memcpy(buf, network_operator->operator_name, network_operator->operator_len); /*the max value of param_len is 16.*/
			buf[network_operator->operator_len] = 0;
			printf("HF-->The current network operator name is: %s\n>", buf);
			break;
		}
	case BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND:
		{
			PBtsdk_HFP_PhoneInfoStru subscriber_info = (PBtsdk_HFP_PhoneInfoStru)param;
			memcpy(buf, subscriber_info->number, subscriber_info->num_len); 
			buf[subscriber_info->num_len] = 0;
			printf("HF-->The subscriber number is: %s\n>", buf);
			break;
		}
	case BTSDK_HFP_EV_CURRENT_CALLS_IND:
		{
			PBtsdk_HFP_CLCCInfoStru clcc = (PBtsdk_HFP_CLCCInfoStru)param;
			BTUINT8 str[32 + 1] = {0};
			memcpy(str, clcc->number, clcc->num_len);
			printf("HF-->Call Information: <idx:%d>:<Num:%s><Len:%d><Type:%d><dir:%d><status:%d><mode:%d><mpty:%d>\n>", 
				clcc->idx, str, clcc->num_len, clcc->type, clcc->dir, clcc->status, clcc->mode, clcc->mpty);
			break;
		}
	case BTSDK_HFP_EV_BATTERY_CHARGE_IND:
		{
			printf("battery: 0x%04x\n>", *param);
			break;
		}
				
	default:
		break;
	}
	free(buf);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show HFP user interface.
Arguments:
    void
Return:
	void 
---------------------------------------------------------------------------*/
void HfpShowMenu()
{
	printf("*****************************************\n");
	printf("*         HFP/HSP Testing Menu          *\n");
	printf("* <1> HFP AG service -- default service *\n");
	printf("* <2> HFP Device service                *\n");
	printf("* <r> Return to upper menu              *\n");
	printf("*****************************************\n");
	printf(">");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show Handsfree AG testing menu.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void HfpAGShowMenu()
{
	printf("*****************************************\n");
	printf("*         HFP/HSP AG Testing Menu       *\n");
	printf("* <1> Mute on/mute off HFP              *\n");
	printf("* <2> HFP Disconnect                    *\n");
	printf("* <r> Return to upper menu              *\n");
	printf("*****************************************\n");
	printf(">");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to show Handsfree device testing menu.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void HfpAPShowMenu()
{
	printf("***********************************************\n");
	printf("*      HFP/HSP Device Testing Menu            *\n");
	printf("* <1> Answer the incoming call                *\n");
	printf("* <2> Reject/Terminate/Release the call       *\n");
	printf("* <3> Call with a phone number                *\n");
	printf("* <4> Last number redial                      *\n");
	printf("* <5> Memory Call                             *\n");
	printf("* <6> DTMF                                    *\n");
	printf("* <7> Turn microphone volume to the max       *\n");
	printf("* <8> Turn speaker volume to the max          *\n");
	printf("* <9> Disable the EC and NR on AG             *\n");
	printf("* <a> Request Current Network Operator Name   *\n");
	printf("* <b> SCO Audio Transfer                      *\n");
	printf("* <c> Request Subscriber Info                 *\n");
	printf("* <d> Request current call list               *\n");
	printf("* <e> Request manufacturer and model ID       *\n");
	printf("* <f> AT command                              *\n");
	printf("* <g> Attach a phone number to a voice tag    *\n");
	printf("* <h> Activate or deactivate voice recognition*\n");
	printf("* <i> HFP Disconnect                          *\n");
	printf("* <r> Return to upper menu                    *\n");
	printf("***********************************************\n");
	printf(">");
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select device handle.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectRmtHFDev()
{   
	/* many Headset devices provide both HF and AV services */
	//s_currRmtHFDevHdl = SelectRemoteDevice(BTSDK_DEVCLS_MASK(BTSDK_AV_HEADSET));
    s_currRmtHFDevHdl = SelectRemoteDevice(0);
	if (BTSDK_INVALID_HANDLE == s_currRmtHFDevHdl)
	{
		printf("Please make sure that the expected device is in discoverable state and search again.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to select service handle of HF service.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestSelectHFSvc()
{
	s_currHFSvcHdl = SelectRemoteService(s_currRmtHFDevHdl);
	if (BTSDK_INVALID_HANDLE == s_currHFSvcHdl)
	{
		printf("Cannot get expected service handle.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to connect HF service by its service handle.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestConnectAGSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;
	
	ulRet = Btsdk_Connect(s_currHFSvcHdl, 0, &s_currAGConnHdl);
	if (BTSDK_OK != ulRet)
	{
		printf("Please make sure that the expected device is powered on and connectable.\n");
	}
	else
	{
		printf("Please switch your audio card to Bluetooth SCO card, then you can hear music with the headset device.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to connect HF service by its service handle.
Arguments:
Return:
	void 
---------------------------------------------------------------------------*/
void TestConnectHFSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;
	
	ulRet = Btsdk_Connect(s_currHFSvcHdl, 0, &s_currHFConnHdl);
	if (BTSDK_OK != ulRet)
	{
		printf("Please make sure that the expected device is powered on and connectable.\n");
	}
	else
	{
		printf("Connect to Audio GateWay successfully.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution function for HFP AG sample.
Arguments:
	choice: [in] choice you have selected.
Return:
	void 
---------------------------------------------------------------------------*/
void HfpAGExecCmd(BTUINT8 choice)
{
	BTBOOL bSCOExist = BTSDK_FALSE;	
	switch (choice)
	{	
	case '1': 
	       /* bSCOExist == TRUE indicates that initial HFP connection is in mute off state. 
		      bSCOExist == FALSE indicates that initial HFP connection is in mute on state. */
	       Btsdk_AGAP_IsAudioConnExisted(&bSCOExist);	
		   if (BTSDK_TRUE == bSCOExist)
		   {	
			   if (BTSDK_OK == Btsdk_AGAP_AudioConnTrans(s_currAGConnHdl))
			   {
				   printf("Mute on HFP connection successfully!\n");				   
			   }
			   else
			   {
				   printf("Cannot mute on HFP connection and an error occurs!\n");
			   }
		   }
		   else
		   {
			   if (BTSDK_OK == Btsdk_AGAP_AudioConnTrans(s_currAGConnHdl))
			   {
				   printf("Mute off HFP connection successfully and please pay attention to audio card switching.\n");
			   }
			   else
			   {
				   printf("Cannot mute off HFP connection and an error occurs.\n");
			   }
		   } 
		   break;

	case '2':
		if (BTSDK_INVALID_HANDLE == s_currAGConnHdl)
		{
			printf("If there is not a connection existing, please establish a connection first.\n");
			break;
		}
		PrintErrorMessage(Btsdk_Disconnect(s_currAGConnHdl), BTSDK_TRUE);
        s_currHFConnHdl = BTSDK_INVALID_HANDLE;
		break;
		
	default:
		printf("Invalid command.\n");
		break;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution function for HFP device sample.
Arguments:
	BTUINT8 choice
Return:
	void 
---------------------------------------------------------------------------*/
void HfpAPExecCmd(BTUINT8 choice)
{
	BTUINT8 buf[32];
	BTUINT32 res = 0;
	memset(buf, 0, 32);

	switch (choice)
	{
	case '1': // Answer the incoming call
		Btsdk_HFAP_AnswerCall(s_currHFConnHdl);
		break;
	case '2': // Reject the incoming call or terminate an outgoing call or release an ongoing call
		Btsdk_HFAP_CancelCall(s_currHFConnHdl);
		break;
	case '3': // Please dial with a phone number
		printf("Input the phone number: ");
		scanf("%s", buf);
		if (strlen(buf) != 0)
		{
			Btsdk_HFAP_Dial(s_currHFConnHdl, buf, (BTUINT16)strlen(buf));
		}
		break;
	case '4': // Last number redial
		Btsdk_HFAP_LastNumRedial(s_currHFConnHdl);
		break;
	case '5': // Memory dialing
		printf("Input the memory location: ");
		scanf("%s", buf);
		if (strlen(buf) != 0)
			Btsdk_HFAP_MemNumDial(s_currHFConnHdl, buf, (BTUINT16)strlen(buf));
		break;
	case '6': // Transmit DTMF
		printf("Input the DTMF digit: ");
		do {
			scanf("%c", buf);
		} while (buf[0] == '\n');
		if ((buf[0] >= '0' && buf[0] <= '9') || buf[0] == '*' || buf[0] == '#')
			Btsdk_HFAP_TxDTMF(s_currHFConnHdl, buf[0]);
		break;
	case '7':
		printf("Turn microphone volume of cell phone to the Max.");
		Btsdk_HFAP_SetMicVol(s_currHFConnHdl, '15');
		break;
	case '8':
		printf("Turn speaker volume of cell phone to the Max.");
		Btsdk_HFAP_SetSpkVol(s_currHFConnHdl, '15');
		break;
	case '9': // Disable the EC and NR on AG
		Btsdk_HFAP_DisableNREC(s_currHFConnHdl);
		break;
	case 'a': // Request for Current Network Operator Name
		Btsdk_HFAP_NetworkOperatorReq(s_currHFConnHdl);
		// The network operator name will be returned by BTSDK_HFP_EV_NETWORK_OPERATOR_IND event.
		break;
	case 'b': // SCO Audio Switching
		Btsdk_HFAP_AudioConnTrans(s_currHFConnHdl);
		break;
	case 'c': // Request for Subscriber Info
		res = Btsdk_HFAP_GetSubscriberNumber(s_currHFConnHdl);
		// The subscriber info will be returned by BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND event.
		break;
	case 'd': // Request for current call list
		res = Btsdk_HFAP_GetCurrentCalls(s_currHFConnHdl);
		// The information of each existing call will be returned by BTSDK_HFP_EV_CURRENT_CALLS_IND event.
		break;
	case 'e': // Request for manufacturer and model ID
		{
			BTUINT8 *m_id;
			BTUINT16 m_len = 0;
			res = Btsdk_HFAP_GetManufacturerID(s_currHFConnHdl, NULL, &m_len);
			if (m_len != 0)
			{
				m_id = (BTUINT8*)malloc(m_len);
				memset(m_id, 0, m_len);
				Btsdk_HFAP_GetManufacturerID(s_currHFConnHdl, m_id, &m_len);
				printf("HF-->Manufacturer of the AG is: %s\n>", m_id);
				free(m_id);
			}
			m_len = 0;
			Btsdk_HFAP_GetModelID(s_currHFConnHdl, NULL, &m_len);
			if (m_len != 0)
			{
				m_id = (BTUINT8*)malloc(m_len);
				memset(m_id, 0, m_len);
				Btsdk_HFAP_GetModelID(s_currHFConnHdl, m_id, &m_len);
				printf("HF-->Model ID of the AG is: %s\n>", m_id);
				free(m_id);
			}
			break;
		}
	case 'f': //AT command
		{
			BTUINT8 *ext_cmd = (BTUINT8*)malloc(256);
			printf("Input the AT command: ");
			scanf("%s", ext_cmd);
			if (strlen(ext_cmd) != 0)
			{
				strcat(ext_cmd, "\r");
				Btsdk_HFP_ExtendCmd(s_currHFConnHdl, ext_cmd, (BTUINT16)strlen(ext_cmd), 6000);
				// After this command is transmited and before receiving one of "OK", "ERROR" or "+CMER", 
				// all the result codes responded by AG will be returned by BTSDK_HFP_EV_EXTEND_CMD_IND event.
				// The ending "OK", "ERROR" or "+CMER" will also be returned by BTSDK_HFP_EV_EXTEND_CMD_IND event.
			}
			free(ext_cmd);
			break;
		}
	case 'g': // Attach a phone number to a voice tag
		res = Btsdk_HFAP_VoiceTagPhoneNumReq(s_currHFConnHdl);
		// The phone number will be returned by BTSDK_HFP_EV_VOICETAG_PHONE_NUM_IND event.
		break;
	case 'h': // Activate or deactivate voice recognition
		s_hf_bvra_enable = s_hf_bvra_enable ^ 1;
		Btsdk_HFAP_VoiceRecognitionReq(s_currHFConnHdl, s_hf_bvra_enable);
		break;
	case 'i':
		if (BTSDK_INVALID_HANDLE == s_currHFConnHdl)
		{
			printf("If there is not a connection existing, please establish a connection first.\n");
			break;
		}
		PrintErrorMessage(Btsdk_Disconnect(s_currHFConnHdl), BTSDK_TRUE);
        s_currHFConnHdl = BTSDK_INVALID_HANDLE;
		break;
	default:
		printf("**Invalid Command!**\n>");
		break;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the execution for HFP sample.
Arguments:
	BTUINT8 choice
Return:
	void 
---------------------------------------------------------------------------*/
void HfpExecCmd(BTUINT8 choice)
{
	BTINT8 ch = 0;

	if (choice == '1')
	{
		HfpInit();
		TestSelectRmtHFDev();
		TestSelectHFSvc();
		TestConnectAGSvc();
		/* the initial state is in mute on state, so switch it to mute off state to hear music */
		HfpAGShowMenu();
		while (ch != 'r')
		{
			scanf(" %c", &ch);
			getchar();		
			if (ch == '\n')
			{
				printf(">>");
			}
			else if('r' == ch)
			{
				break;
			}
			else
			{
				HfpAGExecCmd(ch);
				printf("\n");
				HfpAGShowMenu();
			}
		}
	}
	else if (choice == '2')
	{
		HfpInit();
		TestSelectRmtHFDev();
		TestSelectHFSvc();
		TestConnectHFSvc();
		HfpAPShowMenu();
		while (ch != 'r')
		{
			scanf(" %c", &ch);
			getchar();		
			if (ch == '\n')
			{
				printf(">>");
			}
			else if('r' == ch)
			{
				break;
			}
			else
			{
				HfpAPExecCmd(ch);
				printf("\n");
				HfpAPShowMenu();
			}
		}
	}
	else
	{
		printf("Invalid command.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to initialize HFP profile.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void HfpInit(void)
{
	Btsdk_HFAP_APPRegCbk4ThirdParty(Test_HfpAPCallbackFunc);
	s_hf_svc = Btsdk_RegisterHFPService("Hands-free unit", BTSDK_CLS_HANDSFREE, BTSDK_HF_BRSF_3WAYCALL|
		BTSDK_HF_BRSF_CLIP|BTSDK_HF_BRSF_BVRA|BTSDK_HF_BRSF_RMTVOLCTRL|
		BTSDK_HF_BRSF_ENHANCED_CALLSTATUS|BTSDK_HF_BRSF_ENHANCED_CALLCONTROL);
	s_hs_svc = Btsdk_RegisterHFPService("Headset unit", BTSDK_CLS_HEADSET, 0);
	
	Btsdk_AGAP_APPRegCbk4ThirdParty(Test_HfpAgCallbackFunc);
	s_hfag_svc = Btsdk_RegisterHFPService("Hands-free audio gateway", BTSDK_CLS_HANDSFREE_AG, BTSDK_AG_BRSF_3WAYCALL|
		BTSDK_AG_BRSF_BVRA|BTSDK_AG_BRSF_BINP|BTSDK_AG_BRSF_REJECT_CALL|
		BTSDK_AG_BRSF_ENHANCED_CALLSTATUS|BTSDK_AG_BRSF_ENHANCED_CALLCONTROL|
		BTSDK_AG_BRSF_EXTENDED_ERRORRESULT);
	s_hsag_svc = Btsdk_RegisterHFPService("Headset audio gateway", BTSDK_CLS_HEADSET_AG, 0);

}

void HfpDone(void)
{
	Btsdk_UnregisterHFPService(s_hf_svc);
	Btsdk_UnregisterHFPService(s_hs_svc);
	
	Btsdk_UnregisterHFPService(s_hfag_svc);
	Btsdk_UnregisterHFPService(s_hsag_svc);
	
	Btsdk_HFAP_APPRegCbk4ThirdParty(NULL);
	Btsdk_AGAP_APPRegCbk4ThirdParty(NULL);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is the entry function for HFP sample.
Arguments:
	void
Return:
	void 
---------------------------------------------------------------------------*/
void TestHfpFunc(void)
{
	BTUINT8 ch = 0;

	HfpShowMenu();	
	while (ch != 'r')
	{
		scanf(" %c", &ch);
		getchar();		
		if (ch == '\n')
		{
			printf(">>");
		}
		else if('r' == ch)
		{
			break;
		}
		else
		{
			HfpExecCmd(ch);
			printf("\n");
			HfpShowMenu();
		}
	}

	if (BTSDK_INVALID_HANDLE != s_currHFConnHdl)
	{
		Btsdk_Disconnect(s_currHFConnHdl);
		s_currHFConnHdl = BTSDK_INVALID_HANDLE;
	}
	HfpDone();
}


				




