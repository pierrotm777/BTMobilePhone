#include "sdk_tst.h"


#define HFP_MEM_PHONE_NUMBERS	600		/* Represents number of the contacts stored in the SIM card */
#define HFP_SUBMENU_MASK        0x0100
enum HFP_MENU_ID {
	HFPMENU_HIDDEN,
	HFPMENU_MAIN,
	HFPMENU_HF,
	HFPMENU_AG,

	HFMENU_IDLE,
	HFMENU_STANDBY,
	HFMENU_RINGING,
	HFMENU_OUTGOINGCALL,
	HFMENU_ONGOINGCALL,
	HFMENU_CALLWAITING,
	HFMENU_HELDINCOMINGCALL,
};

#ifndef USINGLIBVERSION
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
#endif
BTDEVHDL g_cur_hf_hdev = BTSDK_INVALID_HANDLE;
BTSVCHDL g_cur_hf_hconn = BTSDK_INVALID_HANDLE;

static BTSVCHDL s_hf_svc = BTSDK_INVALID_HANDLE;
static BTSVCHDL s_hs_svc = BTSDK_INVALID_HANDLE;
static BTUINT8 s_cur_hf_spk_vol = 6;
static BTUINT8 s_cur_hf_mic_vol = 6;

static BTSVCHDL s_ag_svc = BTSDK_INVALID_HANDLE;
static BTSVCHDL s_vg_svc = BTSDK_INVALID_HANDLE;	// Headset AG service
static BTSVCHDL s_cur_ag_hconn = BTSDK_INVALID_HANDLE;

static BTUINT16 s_cur_hfp_menu = HFPMENU_HIDDEN;
static BTUINT16 s_cur_hf_menu = HFMENU_IDLE;
static BTUINT8 s_cur_ag_spk_vol = 6;
static BTUINT8 s_cur_ag_mic_vol = 6;

static BTUINT8 s_hf_bvra_enable = 0;
static BTUINT8 s_ag_bvra_enable = 0;
static BTUINT8 s_ag_inband_ring_enable = 0;

// For simplification, this sample only stores the number and status of the existing calls. 
// Only one setup call is allowed at a time.
static BTUINT8 s_ag_active_calls = 0; // Number of active calls
static BTUINT8 s_ag_held_calls = 0; // Number of held calls
static BTUINT8 s_ag_setup_call_status = 0; // Status of the setup call: 0 - no setup call;
// 2 - Dialing; 3 - Alerting; 4 - incoming; 5 - Waiting;
static BTUINT8 s_ag_last_dial_number = 0; // 1 - A last call number exists. 0 - No call has been made before.

// BTRH held incoming call is a ambiguous state. According to the HFP 1.5 Spec, if an incoming call is 
// put on hold, the indicator call is set to 1 and callsetup is set to 0, while callheld is not specified.
// So we can't differentiate an active call and a held incoming call when creating a new HFP connection.

void HFChangeMenu(BTUINT16 new_menu);

void HfpMainMenu();
void HfpCmdInMainMenu(BTUINT8 *choice);

void HfpShowMenu();

void HfpExecCmd(BTUINT8 *choice);
void HfpCmdInHFMenu(BTUINT8 *choice);


/* 
Accessorial functions 
*/


/*******************************************************************
*																	*
********************************************************************/
void TestHfp(void)
{
	BTUINT8 ch = 0;

	s_cur_hfp_menu = HFPMENU_MAIN;
	HfpShowMenu();

	while (ch != 'q')
	{
		WaitForSingleObject (g_hFuncExcCmdEvt, INFINITE);
		ch = g_cExcCmd;
		PRINTMSG(1, "TestHfp: g_iNum(%d), g_cExcCmd(%c), g_NumberLevel(%d)\r\n", g_iNum, g_cExcCmd,g_NumberLevel);

		if (ch == '\n')
		{
			PRINTMSG(1, ">");
		}
		else
		{
			HfpExecCmd(&ch);
		}
	}
	s_cur_hfp_menu = HFPMENU_HIDDEN;
}


/*******************************************************************
*																	*
********************************************************************/
void HfpExecCmd(BTUINT8 *choice)
{
	switch (s_cur_hfp_menu)
	{
	case HFPMENU_MAIN:
		HfpCmdInMainMenu(choice);
		break;
	case HFPMENU_HF:
		HfpCmdInHFMenu(choice);
		break;
	default:
		PRINTMSG(1,"Invalid command.\r\n >");
		break;
	}
}


void HFGetCmdStr(BTUINT16 cmd_idx, BTUINT8 *cmd_str)
{
	switch (cmd_idx)
	{
	case BTSDK_HFP_CMD_BRSF:
		strcpy((char *)cmd_str, "AT+BRSF=<val>");
		break;
	case BTSDK_HFP_CMD_CIND_T:
		strcpy((char *)cmd_str, "AT+CIND=?");
		break;
	case BTSDK_HFP_CMD_CIND_R:
		strcpy((char *)cmd_str, "AT+CIND?");
		break;
	case BTSDK_HFP_CMD_CMER:
		strcpy((char *)cmd_str, "AT+CMER=3,0,0,1");
		break;
	case BTSDK_HFP_CMD_CHLD_T:
		strcpy((char *)cmd_str, "AT+CHLD=?");
		break;
	case BTSDK_HFP_CMD_COPS_SET_FORMAT:
		strcpy((char *)cmd_str, "AT+COPS=3,0");
		break;
	case BTSDK_HFP_CMD_CCWA_ACTIVATE:
		strcpy((char *)cmd_str, "AT+CCWA=1");
		break;
	case BTSDK_HFP_CMD_CMEE:
		strcpy((char *)cmd_str, "AT+CMEE=1");
		break;
	case BTSDK_HFP_CMD_CLIP:
		strcpy((char *)cmd_str, "AT+CLIP=<n>");
		break;
	case BTSDK_HFP_CMD_BVRA_ENABLE:
		strcpy((char *)cmd_str, "AT+BVRA=1");
		break;
	case BTSDK_HFP_CMD_BVRA_DISABLE:
		strcpy((char *)cmd_str, "AT+BVRA=0");
		break;
	case BTSDK_HFP_CMD_VTS:
		strcpy((char *)cmd_str, "AT+VTS=<n>");
		break;
	case BTSDK_HFP_CMD_VGS:
		strcpy((char *)cmd_str, "AT+VGS=<n>");
		break;
	case BTSDK_HFP_CMD_VGM:
		strcpy((char *)cmd_str, "AT+VGM=<n>");
		break;
	case BTSDK_HFP_CMD_NREC:
		strcpy((char *)cmd_str, "AT+NREC=<n>");
		break;
	case BTSDK_HFP_CMD_CHLD_0:
		strcpy((char *)cmd_str, "AT+CHLD=0");
		break;
	case BTSDK_HFP_CMD_CHLD_1:
		strcpy((char *)cmd_str, "AT+CHLD=1[<idx>]");
		break;
	case BTSDK_HFP_CMD_CHLD_2:
		strcpy((char *)cmd_str, "AT+CHLD=2[<idx>]");
		break;
	case BTSDK_HFP_CMD_CHLD_3:
		strcpy((char *)cmd_str, "AT+CHLD=3");
		break;
	case BTSDK_HFP_CMD_CHLD_4:
		strcpy((char *)cmd_str, "AT+CHLD=4");
		break;
	case BTSDK_HFP_CMD_CANCELCALL:
		strcpy((char *)cmd_str, "AT+CHUP");
		break;
	case BTSDK_HFP_CMD_ANSWERCALL:
		strcpy((char *)cmd_str, "ATA");
		break;
	case BTSDK_HFP_CMD_DIAL:
		strcpy((char *)cmd_str, "ATD<n>");
		break;
	case BTSDK_HFP_CMD_MEMDIAL:
		strcpy((char *)cmd_str, "ATD><n>");
		break;
	case BTSDK_HFP_CMD_BLDN:
		strcpy((char *)cmd_str, "AT+BLDN");
		break;
	case BTSDK_HFP_CMD_CKPD:
		strcpy((char *)cmd_str, "AT+CKPD=200");
		break;
	case BTSDK_HFP_CMD_CKPD_REJ_CALL:
		strcpy((char *)cmd_str, "AT+CKPD=\"e\"");
		break;
	case BTSDK_HFP_CMD_BIA:
		strcpy((char *)cmd_str, "AT+BIA=x,x,x,x,x");
		break;
	case BTSDK_HFP_CMD_COPS_READ:
		strcpy((char *)cmd_str, "AT+COPS?");
		break;
	case BTSDK_HFP_CMD_BINP:
		strcpy((char *)cmd_str, "AT+BINP=1");
		break;
	case BTSDK_HFP_CMD_BTRH_QUERY:
		strcpy((char *)cmd_str, "AT+BTRH?");
		break;
	case BTSDK_HFP_CMD_BTRH:
		strcpy((char *)cmd_str, "AT+BTRH=0");
		break;
	case BTSDK_HFP_CMD_CNUM:
		strcpy((char *)cmd_str, "AT+CNUM");
		break;
	case BTSDK_HFP_CMD_CLCC:
		strcpy((char *)cmd_str, "AT+CLCC");
		break;
	case BTSDK_HFP_CMD_CGMI:
		strcpy((char *)cmd_str, "AT+CGMI");
		break;
	case BTSDK_HFP_CMD_CGMM:
		strcpy((char *)cmd_str, "AT+CGMM");
		break;
	default:
		strcpy((char *)cmd_str, "Extended Command");
		break;
	}
}

/*******************************************************************
*	                             				         			*
********************************************************************/
void HFGetErrStr(BTUINT8 err_code, BTUINT8 *err_str)
{
	switch (err_code)
	{
	case BTSDK_HFP_CMEERR_AGFAILURE:
		strcpy((char *)err_str, "+CME ERROR:0 - AG failure");
		break;
	case BTSDK_HFP_CMEERR_NOCONN2PHONE:
		strcpy((char *)err_str, "+CME ERROR:1 - no connection to phone");
		break;
	case BTSDK_HFP_CMEERR_OPERATION_NOTALLOWED:
		strcpy((char *)err_str, "+CME ERROR:3 - operation not allowed");
		break;
	case BTSDK_HFP_CMEERR_OPERATION_NOTSUPPORTED:
		strcpy((char *)err_str, "+CME ERROR:4 - operation not supported");
		break;
	case BTSDK_HFP_CMEERR_PHSIMPIN_REQUIRED:
		strcpy((char *)err_str, "+CME ERROR:5 - PH-SIM PIN required");
		break;
	case BTSDK_HFP_CMEERR_SIMNOT_INSERTED:
		strcpy((char *)err_str, "+CME ERROR:10 - SIM not inserted");
		break;
	case BTSDK_HFP_CMEERR_SIMPIN_REQUIRED:
		strcpy((char *)err_str, "+CME ERROR:11 - SIM PIN required");
		break;
	case BTSDK_HFP_CMEERR_SIMPUK_REQUIRED:
		strcpy((char *)err_str, "+CME ERROR:12 - SIM PUK required");
		break;
	case BTSDK_HFP_CMEERR_SIM_FAILURE:
		strcpy((char *)err_str, "+CME ERROR:13 - SIM failure");
		break;
	case BTSDK_HFP_CMEERR_SIM_BUSY:
		strcpy((char *)err_str, "+CME ERROR:14 - SIM busy");
		break;
	case BTSDK_HFP_CMEERR_INCORRECT_PASSWORD:
		strcpy((char *)err_str, "+CME ERROR:16 - incorrect password");
		break;
	case BTSDK_HFP_CMEERR_SIMPIN2_REQUIRED:
		strcpy((char *)err_str, "+CME ERROR:17 - SIM PIN2 required");
		break;
	case BTSDK_HFP_CMEERR_SIMPUK2_REQUIRED:
		strcpy((char *)err_str, "+CME ERROR:18 - SIM PUK2 required");
		break;
	case BTSDK_HFP_CMEERR_MEMORY_FULL:
		strcpy((char *)err_str, "+CME ERROR:20 - memory full");
		break;
	case BTSDK_HFP_CMEERR_INVALID_INDEX:
		strcpy((char *)err_str, "+CME ERROR:21 - invalid index");
		break;
	case BTSDK_HFP_CMEERR_MEMORY_FAILURE:
		strcpy((char *)err_str, "+CME ERROR:23 - memory failure");
		break;
	case BTSDK_HFP_CMEERR_TEXTSTRING_TOOLONG:
		strcpy((char *)err_str, "+CME ERROR:24 - text string too long");
		break;
	case BTSDK_HFP_CMEERR_INVALID_CHAR_INTEXTSTRING:
		strcpy((char *)err_str, "+CME ERROR:25 - invalid characters in text string");
		break;
	case BTSDK_HFP_CMEERR_DIAL_STRING_TOOLONG:
		strcpy((char *)err_str, "+CME ERROR:26 - dial string too long");
		break;
	case BTSDK_HFP_CMEERR_INVALID_CHAR_INDIALSTRING:
		strcpy((char *)err_str, "+CME ERROR:27 - invalid characters in dial string");
		break;
	case BTSDK_HFP_CMEERR_NETWORK_NOSERVICE:
		strcpy((char *)err_str, "+CME ERROR:30 - no network service");
		break;
	case BTSDK_HFP_CMEERR_NETWORK_TIMEOUT:
		strcpy((char *)err_str, "+CME ERROR:31 - network timeout");
		break;
	case BTSDK_HFP_CMEERR_EMERGENCYCALL_ONLY:
		strcpy((char *)err_str, "+CME ERROR:32 - Network not allowed, emergency calls only");
		break;
	case BTSDK_HFP_APPERR_TIMEOUT:
		strcpy((char *)err_str, "Wait for AG response timeout");
		break;
	case BTSDK_HFP_STDERR_ERROR:
		strcpy((char *)err_str, "ERROR");
		break;
	case BTSDK_HFP_STDRR_NOCARRIER:
		strcpy((char *)err_str, "NO CARRIER");
		break;
	case BTSDK_HFP_STDERR_BUSY:
		strcpy((char *)err_str, "BUSY");
		break;
	case BTSDK_HFP_STDERR_NOANSWER:
		strcpy((char *)err_str, "NO ANSWER");
		break;
	case BTSDK_HFP_STDERR_DELAYED:
		strcpy((char *)err_str, "DELAYED");
		break;
	case BTSDK_HFP_STDERR_BLACKLISTED:
		strcpy((char *)err_str, "BLACKLISTED");
		break;
	case BTSDK_HFP_OK:
		strcpy((char *)err_str, "OK");
		break;
	default:
		strcpy((char *)err_str, "Unknown error result code");
		break;
	}
}


/* 
Server callbacks 
*/

/*******************************************************************
*	Callback to deal with HF/HS events								*
********************************************************************/
void App_HFCbkFunc(BTCONNHDL hdl, BTUINT16 event, BTUINT8 *param, BTUINT16 param_len)
{
	//BTUINT8 *buf = NULL;
	//BTUINT8 *extend_recode = NULL;

	//buf = malloc(33);
	//extend_recode = malloc(257);

	BTUINT8 buf[33];
	BTUINT8 extend_recode[257];

	switch (event)
	{
		// Connection created indications
	case BTSDK_HFP_EV_SPP_ESTABLISHED_IND:
		/* SPP connection connected. This event is received before sending any AT commands. */
		g_cur_hf_hconn = hdl;
		{
			PBtsdk_HFP_ConnInfoStru conn_info = (PBtsdk_HFP_ConnInfoStru)param;
			PRINTMSG(1,"HF SPP connection setup, local role: %04x\n>", conn_info->role);
		}
		break;
	case BTSDK_HFP_EV_SLC_ESTABLISHED_IND:
		/* Service Level Connection connected. This event is received after sending all mandatory
		AT commands defined in the HFP Spec 1.5 service Level Connection procedure. */
		g_cur_hf_hconn = hdl;
		{
			PBtsdk_HFP_ConnInfoStru conn_info = (PBtsdk_HFP_ConnInfoStru)param;
			PRINTMSG(1,"HF SLC connection setup, local role: %04x\n>", conn_info->role);
			g_cur_hf_hdev = conn_info->dev_hdl;
		}
		break;
		// State changed indications
	case BTSDK_HFP_EV_SLC_RELEASED_IND:
		g_cur_hf_hconn = BTSDK_INVALID_HANDLE;
		g_cur_hf_hdev  = BTSDK_INVALID_HANDLE;
		{
			PBtsdk_HFP_ConnInfoStru conn_info = (PBtsdk_HFP_ConnInfoStru)param;
			BTUINT8 *name = (BTUINT8*)malloc(BTSDK_DEVNAME_LEN);
			memset(name, 0, BTSDK_DEVNAME_LEN);
			//Btsdk_UpdateRemoteDeviceName(conn_info->dev_hdl, name, NULL);
			PRINTMSG(1,"HF connection with %s released, local role: %04x\n>", name, conn_info->role);
			free(name);
		}
		HFChangeMenu(HFMENU_IDLE);
		break;
	case BTSDK_HFP_EV_STANDBY_IND:
		HFChangeMenu(HFMENU_STANDBY);
		break;
	case BTSDK_HFP_EV_ONGOINGCALL_IND:
		HFChangeMenu(HFMENU_ONGOINGCALL);
		break;
	case BTSDK_HFP_EV_RINGING_IND:
		if (s_cur_hf_menu == HFMENU_RINGING) {
			PRINTMSG(1,"HF-->Ringing...\n>");
		} else { 
			HFChangeMenu(HFMENU_RINGING);
			if (*param) {  /* in-band */
				PRINTMSG(1,"HF-->in-band ring tone!\n>");
			} else {
				PRINTMSG(1,"HF-->out-band ring tone!\n>");
				/*
				To Do: Generate local ring tone 
				*/
			}
		}
		break;
	case BTSDK_HFP_EV_OUTGOINGCALL_IND:
		HFChangeMenu(HFMENU_OUTGOINGCALL);
		break;
	case BTSDK_HFP_EV_CALLHELD_IND:
		PRINTMSG(1,"HF--> The incoming call is on hold!\n>");
		HFChangeMenu(HFMENU_HELDINCOMINGCALL);
		break;
	case BTSDK_HFP_EV_CALL_WAITING_IND:
		{
			PBtsdk_HFP_PhoneInfoStru call_info = (PBtsdk_HFP_PhoneInfoStru)param;
			if (call_info->num_len != 0) {
				memcpy(buf, call_info->number, call_info->num_len);
				buf[call_info->num_len] = 0;
				PRINTMSG(1,"HF-->Calling number is: %s\n>", buf);
			}
			if (call_info->name_len != 0) {
				memcpy(buf, call_info->alpha_str, call_info->name_len);
				buf[call_info->name_len] = 0;
				PRINTMSG(1,"HF-->Calling name is: %s\n>", buf);
			}
		}
		HFChangeMenu(HFMENU_CALLWAITING);
		break;

		// Confirmation to the requests (AT Commands) from APP
	case BTSDK_HFP_EV_ATCMD_RESULT:
		{
			PBtsdk_HFP_ATCmdResultStru cmd_result = (PBtsdk_HFP_ATCmdResultStru)param;
			HFGetCmdStr(cmd_result->cmd_code, buf);
			HFGetErrStr(cmd_result->result_code, extend_recode);
			if (cmd_result->cmd_code == BTSDK_HFP_CMD_CGMI)
			{
				PRINTMSG(1,"Hands free ready, you can set up PIM connection now.\r\n");
			}
			if (cmd_result->result_code == BTSDK_HFP_OK)
				PRINTMSG(1,"HF-->Possitive confirmation to the command: %s\n>", buf);
			else
				PRINTMSG(1,"HF-->Negative confirmation to the command: %s, response is: %s\n>", buf, extend_recode);
		}
		break;

		// Local indications
	case BTSDK_HFP_EV_AUDIO_CONN_ESTABLISHED_IND:
		PRINTMSG(1,"HF-->SCO Audio Connection Established, SCO connection handle %04x.\n>", *(BTUINT16*)param);
		/*
		To Do: Switch local audio path to Bluetooth SCO/eSCO link
		*/
		break;
	case BTSDK_HFP_EV_AUDIO_CONN_RELEASED_IND:
		PRINTMSG(1,"HF-->SCO Audio Connection Released, SCO connection handle %04x.\n>", *(BTUINT16*)param);
		/*
		To Do: Swith local audio path to local voice adapter
		*/
		break;
	case BTSDK_HFP_EV_TERMINATE_LOCAL_RINGTONE_IND:
		PRINTMSG(1,"HF-->Terminate local ring tone and un-mute the audio link!\n>");
		/*
		To Do:  
		The application shall terminate its local ring tone here if it has been started before. 
		And, the application should unmute the audio link here if it has been muted before.
		*/
		break;

		// Indications or responses from AG
	case BTSDK_HFP_EV_VOICE_RECOGN_ACTIVATED_IND:
		PRINTMSG(1,"HF-->Voice recognition activated locally or by the AG!\n>");
		s_hf_bvra_enable = 1;
		break;
	case BTSDK_HFP_EV_VOICE_RECOGN_DEACTIVATED_IND:
		PRINTMSG(1,"HF-->Voice recognition deactivated locally or by the AG!\n>");
		s_hf_bvra_enable = 0;
		break;
	case BTSDK_HFP_EV_NETWORK_AVAILABLE_IND:
		PRINTMSG(1,"HF-->Cellular Network is available!\n>");
		break;
	case BTSDK_HFP_EV_NETWORK_UNAVAILABLE_IND:
		PRINTMSG(1,"HF-->Cellular Network is unavailable!\n>");
		break;
	case BTSDK_HFP_EV_ROAMING_RESET_IND:
		PRINTMSG(1,"HF-->Roaming is not active.\n>");
		break;
	case BTSDK_HFP_EV_ROAMING_ACTIVE_IND:
		PRINTMSG(1,"HF-->Roaming is active.\n>");
		break;
	case BTSDK_HFP_EV_SIGNAL_STRENGTH_IND:
		PRINTMSG(1,"HF-->The current signal strength is: %d\n>", *param);
		break;
	case BTSDK_HFP_EV_BATTERY_CHARGE_IND:
		PRINTMSG(1,"HF-->The battery level of AG is: %d\n>", *param);
		break;
	case BTSDK_HFP_EV_CHLDHELD_ACTIVATED_IND:
		PRINTMSG(1,"HF-->An active call is put on hold.\n>");
		break;
	case BTSDK_HFP_EV_CHLDHELD_RELEASED_IND:
		PRINTMSG(1,"HF-->A held call is activated or released.\n>");
		// Whether the call is activated or released is determined by the indication previous to
		// BTSDK_HFP_EV_CHLDHELD_RELEASED_IND.
		break;
	case BTSDK_HFP_EV_MICVOL_CHANGED_IND:
		PRINTMSG(1,"HF-->New microphone gain is %d\n>", *param);
		s_cur_hf_mic_vol = *param;
		/*
		To Do: Change the local microphone volume.
		*/
		break;
	case BTSDK_HFP_EV_SPKVOL_CHANGED_IND:
		PRINTMSG(1,"HF-->New speaker gain is %d\n>", *param);
		s_cur_hf_spk_vol = *param;
		/*
		To Do: Change the local speaker volume.
		*/
		break;
	case BTSDK_HFP_EV_CLIP_IND:
		{
			PBtsdk_HFP_PhoneInfoStru call_info = (PBtsdk_HFP_PhoneInfoStru)param;
			if (call_info->num_len != 0) {
				memcpy(buf, call_info->number, call_info->num_len);
				buf[call_info->num_len] = 0;
				PRINTMSG(1,"HF-->Calling number is: %s\n>", buf);
			}
			if (call_info->name_len != 0) {
				memcpy(buf, call_info->alpha_str, call_info->name_len);
				buf[call_info->name_len] = 0;
				PRINTMSG(1,"HF-->Calling name is: %s\n>", buf);
			}
		}
		break;
	case BTSDK_HFP_EV_CURRENT_CALLS_IND:
		{			
			PBtsdk_HFP_CLCCInfoStru clcc = (PBtsdk_HFP_CLCCInfoStru)param;

			UCHAR str[32 + 1] = {0};
			memcpy(str, clcc->number, clcc->num_len);
			PRINTMSG(1,"HF-->Call Information: <idx:%d>:<Num:%s><Len:%d><Type:%d><dir:%d><status:%d><mode:%d><mpty:%d>\n>", 
				clcc->idx, str, clcc->num_len, clcc->type, clcc->dir, clcc->status, clcc->mode, clcc->mpty);
		}
		break;
	case BTSDK_HFP_EV_NETWORK_OPERATOR_IND:
		{
			PBtsdk_HFP_COPSInfoStru network_operator = (PBtsdk_HFP_COPSInfoStru)param;
			memcpy(buf, network_operator->operator_name, network_operator->operator_len); /*the max value of param_len is 16.*/
			buf[network_operator->operator_len] = 0;
			PRINTMSG(1,"HF-->The current network operator name is: %s\n>", buf);
		}
		break;
	case BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND:
		{
			PBtsdk_HFP_PhoneInfoStru subscriber_info = (PBtsdk_HFP_PhoneInfoStru)param;
			memcpy(buf, subscriber_info->number, subscriber_info->num_len); 
			buf[subscriber_info->num_len] = 0;
			PRINTMSG(1,"HF-->The subscriber number is: %s\n>", buf);
		}
		break;
	case BTSDK_HFP_EV_VOICETAG_PHONE_NUM_IND:
		{
			PBtsdk_HFP_PhoneInfoStru subscriber_info = (PBtsdk_HFP_PhoneInfoStru)param;
			memcpy(buf, subscriber_info->number, subscriber_info->num_len); 
			buf[subscriber_info->num_len] = 0;
			PRINTMSG(1,"HF-->The phone number for voice tag is: %s\n>", buf);
			/*
			To Do: Attach the phone number to a voice tag.
			*/
		}			
		break;

		// Extended result codes from AG
	case BTSDK_HFP_EV_EXTEND_CMD_IND:
		memcpy(extend_recode, param, param_len);
		extend_recode[param_len] = 0;
		PRINTMSG(1,"HF-->Receiving extended result code: %s\n>", extend_recode);
		/*
		To Do: Deal with the result code if possible.
		*/
// 		{
// 			// Relay the result code to the COM port bound with this HFP connection
// 			Btsdk_HFSwrap_ResultCodeInd(hdl, param, param_len);
// 		}
		break;
	default:
		break;
	}
	//free(buf);
	//free(extend_recode);
}

/*******************************************************************
*																	*
********************************************************************/
void HFConnectAG(void)
{
	BTDEVHDL dev_hdl;
	BTINT32 result;
	BtSdkHFPConnParamStru param;

	dev_hdl = SelectRemoteDevice(0);
	if (dev_hdl == BTSDK_INVALID_HANDLE)
		return;

	// If param is not provided, the default feature set is BTSDK_HF_BRSF_ALL
	param.size = sizeof(BtSdkHFPConnParamStru);
	param.mask = 0;
	param.features = BTSDK_HF_BRSF_ALL; // Local HF supported features

	// Tries to connect to Hands-free AG first
	result = Btsdk_ConnectEx(dev_hdl, BTSDK_CLS_HANDSFREE_AG, (BTUINT32)&param, &g_cur_hf_hconn);
	if (result == BTSDK_ER_NO_SERVICE)
	{
		// If Hands-free AG service is unavailable, tries to connect to Headset AG
		result = Btsdk_ConnectEx(dev_hdl, BTSDK_CLS_HEADSET_AG, 0, &g_cur_hf_hconn);
	}

	PRINTMSG(1,"Btsdk_ConnectEx result %d\r\n>", result);
	
}

/*******************************************************************
*																	*
********************************************************************/
void HFPReleaseConnection(BTBOOL loc_ag)
{
	BTSDKHANDLE hEnum = BTSDK_INVALID_HANDLE;
	BTCONNHDL hConn = BTSDK_INVALID_HANDLE;
	BTINT32 result;

	hEnum = Btsdk_StartEnumConnection();
	if (hEnum != BTSDK_INVALID_HANDLE)
	{
		BtSdkConnectionPropertyStru prop;
		BTUINT32 role1 = BTSDK_CONNROLE_SERVER, role2 = BTSDK_CONNROLE_CLIENT;

		if (loc_ag)
		{
			role1 = BTSDK_CONNROLE_CLIENT;
			role2 = BTSDK_CONNROLE_SERVER;
		}
		while ((hConn = Btsdk_EnumConnection(hEnum, &prop)) != BTSDK_INVALID_HANDLE)
		{
			if ((prop.role == role1 && (prop.service_class == BTSDK_CLS_HANDSFREE || prop.service_class == BTSDK_CLS_HEADSET)) ||
				(prop.role == role2 && (prop.service_class == BTSDK_CLS_HANDSFREE_AG || prop.service_class == BTSDK_CLS_HEADSET_AG)))
				break;
		}
		Btsdk_EndEnumConnection(hEnum);
	}

	if (hConn == BTSDK_INVALID_HANDLE)
	{
		PRINTMSG(1,"No HFP connection exists\r\n>");
		return;
	}
	result = Btsdk_Disconnect(hConn);
	if (result == BTSDK_OK)
		PRINTMSG(1,"Release connection successfully\n>");
}


/* 
HFP Menus 
*/

/*******************************************************************
*																	*
********************************************************************/
void HfpMainMenu()
{
	PRINTMSG(1,"*****************************************\n");
	PRINTMSG(1,"*           HFP Testing Menu           *\n");
	PRINTMSG(1,"* <1> Unit Test (HF)                   *\n");
	PRINTMSG(1,"* <m> Show this menu                   *\n");
	PRINTMSG(1,"* <q> Quit                             *\n");
	PRINTMSG(1,"*****************************************\n");
	PRINTMSG(1,">");
}

/*******************************************************************
*																	*
********************************************************************/
void HFIdleMenu()
{
	PRINTMSG(1,"*****************************************\n");
	PRINTMSG(1,"*           HFP-HF Idle Menu            *\n");
	PRINTMSG(1,"* <1> Connect AG                        *\n");
	PRINTMSG(1,"* <m> Show this menu                    *\n");
	PRINTMSG(1,"* <q> Quit                              *\n");
	PRINTMSG(1,"*****************************************\n");
	PRINTMSG(1,">");
}

/*******************************************************************
*																	*
********************************************************************/
void HFStandbyMenu()
{
	PRINTMSG(1,"***********************************************************\n");
	PRINTMSG(1,"*                       HFP-HF Standby Menu               *\n");
	PRINTMSG(1,"* <1> Originate Last Number Redial - BLDN                 *\n");
	PRINTMSG(1,"* <2> Originate a Call - ATD¡­                            *\n");
	PRINTMSG(1,"* <3> Originate a Memory Call - ATD>¡­                    *\n");
	PRINTMSG(1,"* <4> Request Phone Number From AG for a voice tag - BINP *\n");
	PRINTMSG(1,"* <5> Activate/Deactivate Voice Recognition - BVRA        *\n");
	PRINTMSG(1,"* <6> Disconnect Link to AG                               *\n");
	PRINTMSG(1,"* <s> Show the Common Sub-Menu                            *\n");
	PRINTMSG(1,"* <m> Show this menu                                      *\n");
	PRINTMSG(1,"* <q> Quit                                                *\n");
	PRINTMSG(1,"***********************************************************\n");
	PRINTMSG(1,">");
}

/*******************************************************************
*																	*
********************************************************************/
void HFRingingMenu()
{
	PRINTMSG(1,"****************************************************\n");
	PRINTMSG(1,"*                HFP-HF Ringing Menu               *\n");
	PRINTMSG(1,"* <1> Answer the Incoming Call (ATA)               *\n");
	PRINTMSG(1,"* <2> Reject the Incoming Call (CHUP)              *\n");
	//PRINTMSG(1,"* <3> BTRH Hold the Incoming Call (BTRH=0)         *\n");
	PRINTMSG(1,"* <s> Show the Common Sub-Menu                     *\n");
	PRINTMSG(1,"* <m> Show this menu                               *\n");
	PRINTMSG(1,"* <q> Quit                                         *\n");
	PRINTMSG(1,"****************************************************\n");
	PRINTMSG(1,">");
}

/*******************************************************************
*																	*
********************************************************************/
void HFOutgoingCallMenu()
{
	PRINTMSG(1,"****************************************************\n");
	PRINTMSG(1,"*                HFP-HF Outgoing Call Menu         *\n");
	PRINTMSG(1,"* <1> Terminate the Outgoing Call - CHUP           *\n");
	PRINTMSG(1,"* <s> Show the Common Sub-Menu                     *\n");
	PRINTMSG(1,"* <m> Show this menu                               *\n");
	PRINTMSG(1,"* <q> Quit                                         *\n");
	PRINTMSG(1,"****************************************************\n");
	PRINTMSG(1,">");
}

/*******************************************************************
*																	*
********************************************************************/
void HFOngoingCallMenu()
{
	PRINTMSG(1,"**************************************************************\n");
	PRINTMSG(1,"*                HFP-HF Ongoing Call Menu                    *\n");
	PRINTMSG(1,"* <1> Terminate the Ongoing Call - CHUP                      *\n");
	PRINTMSG(1,"* <2> make AG Transmit DTMF Code to GSM Network - VTS        *\n");
	PRINTMSG(1,"* <3> Originate Last Number Redial - BLDN                    *\n");
	PRINTMSG(1,"* <4> Originate a Call - ATD¡­                               *\n");
	PRINTMSG(1,"* <5> Originate a Memory Call - ATD>¡­                       *\n");
	PRINTMSG(1,"* <6> Release active call, accept held call - AT+CHLD=1      *\n");
	PRINTMSG(1,"* <7> Release the specified active call - AT+CHLD=1<idx>     *\n");
	PRINTMSG(1,"* <8> Hold Calls Except the Specified Call - AT+CHLD=2<idx>  *\n");
	PRINTMSG(1,"* <9> Hold active calls, accept held call - AT+CHLD=2        *\n");
	PRINTMSG(1,"* <a> Adds a held call to the conversation - AT+CHLD=3       *\n");
	PRINTMSG(1,"* <b> Leaves the 3-way calling - AT+CHLD=4                   *\n");
	PRINTMSG(1,"* <s> Show the Common Sub-Menu                               *\n");
	PRINTMSG(1,"* <m> Show this menu                                         *\n");
	PRINTMSG(1,"* <q> Quit                                                   *\n");
	PRINTMSG(1,"**************************************************************\n");
	PRINTMSG(1,">");
}

/*******************************************************************
*																	*
********************************************************************/
void HFCallWaitingMenu()/* CCWA 3Way-Call Incoming */
{
	PRINTMSG(1,"********************************************************************\n");
	PRINTMSG(1,"*                HFP-HF Call Waiting Menu                          *\n");
	PRINTMSG(1,"* <1> Hold active call(s), answer the waiting Call - AT+CHLD=2     *\n");
	PRINTMSG(1,"* <2> Release active call(s), answer the waiting Call - AT+CHLD=1  *\n");
	PRINTMSG(1,"* <3> Reject the waiting call - AT+CHLD=0                          *\n");
	PRINTMSG(1,"* <s> Show the Common Sub-Menu                                     *\n");
	PRINTMSG(1,"* <m> Show this menu                                               *\n");
	PRINTMSG(1,"* <q> Quit                                                         *\n");
	PRINTMSG(1,"********************************************************************\n");
	PRINTMSG(1,">");
}

/*******************************************************************
*																	*
********************************************************************/
void HFHeldIncomingCallMenu()
{
	PRINTMSG(1,"*******************************************************\n");
	PRINTMSG(1,"*                HFP-HF Held Incoming Call Menu       *\n");
	PRINTMSG(1,"* <1> Accept the Held Incoming Call - AT+BTRH=1       *\n");
	PRINTMSG(1,"* <2> Reject the Held Incoming Call - AT+BTRH=2       *\n");
	PRINTMSG(1,"* <s> Show the Common Sub-Menu                        *\n");
	PRINTMSG(1,"* <m> Show this menu                                  *\n");
	PRINTMSG(1,"* <q> Quit                                            *\n");
	PRINTMSG(1,"*******************************************************\n");
	PRINTMSG(1,">");
}

/*******************************************************************
*																	*
********************************************************************/
void HFCommonSubMenu()
{
	PRINTMSG(1,"*******************************************************\n");
	PRINTMSG(1,"*                HFP-HF Common Sub Menu               *\n");
	PRINTMSG(1,"* <1> SCO Audio Transfer                              *\n");
	PRINTMSG(1,"* <2> Request Subscriber Info - CNUM                  *\n");
	PRINTMSG(1,"* <3> Request Current Call List - CLCC                *\n");
	PRINTMSG(1,"* <4> Request Current Network Operator Name - COPS    *\n");
	PRINTMSG(1,"* <5> Disable the EC and NR on AG                     *\n");
	PRINTMSG(1,"* <6> Speaker Volume +1                               *\n");
	PRINTMSG(1,"* <7> Microphone Volume +1                            *\n");
	PRINTMSG(1,"* <8> Speaker Volume -1                               *\n");
	PRINTMSG(1,"* <9> Microphone Volume -1                            *\n");
	PRINTMSG(1,"* <a> Request manufacturer and model ID               *\n");
	PRINTMSG(1,"* <b> Reject waiting call or release all held calls   *\n");
	PRINTMSG(1,"* <e> Send extended AT Command                        *\n");
	PRINTMSG(1,"* <m> Show this menu                                  *\n");
	PRINTMSG(1,"* <q> Quit                                            *\n");
	PRINTMSG(1,"*******************************************************\n");
	PRINTMSG(1,">");
}


/*******************************************************************
*																	*
********************************************************************/
void HfpHFShowMenu()
{
	PRINTMSG(1,"\n");
	switch (s_cur_hf_menu)
	{
	case HFMENU_IDLE:
		HFIdleMenu();
		break;
	case HFMENU_STANDBY:
		HFStandbyMenu();
		break;
	case HFMENU_RINGING:
		HFRingingMenu();
		break;
	case HFMENU_OUTGOINGCALL:
		HFOutgoingCallMenu();
		break;
	case HFMENU_ONGOINGCALL:
		HFOngoingCallMenu();
		break;
	case HFMENU_CALLWAITING:
		HFCallWaitingMenu();
		break;
	case HFMENU_HELDINCOMINGCALL:
		HFHeldIncomingCallMenu();
		break;
	default:
		HFCommonSubMenu();
		break;
	}
}

/*******************************************************************
*																	*
********************************************************************/
void HfpShowMenu()
{
	PRINTMSG(1,"\n");
	switch (s_cur_hfp_menu)
	{
	case HFPMENU_MAIN:
		HfpMainMenu();
		break;
	case HFPMENU_HF:
		HfpHFShowMenu();
		break;
	default:
		break;
	}
}

/*******************************************************************
*																	*
********************************************************************/
void HFChangeMenu(BTUINT16 new_menu)
{
	if (s_cur_hf_menu != new_menu)
	{
		s_cur_hf_menu = new_menu;
		HfpShowMenu();
	}
}



/* 
HFP Menu Operations 
*/

/*******************************************************************
*																	*
********************************************************************/
void HfpCmdInMainMenu(BTUINT8 *choice)
{
	switch (*choice)
	{
	case '1':
		s_cur_hfp_menu = HFPMENU_HF;
		HfpShowMenu();
		break;
	case 'm':
		HfpShowMenu();
		break;
	case 'q':
		InterlockedDecrement(&g_NumberLevel); 
		break;
	default:
		PRINTMSG(1,"Invalid command.\n>");
		break;
	}
}

/*******************************************************************
*																	*
********************************************************************/
void HFCmdInIdleMenu(BTUINT8 *choice)
{
	switch (*choice)
	{
	case '1': // Setup connection with an AG
		HFConnectAG();
		break;
	case 'm': // Show the current menu
		HfpShowMenu();
		break;
	case 'q': // Back to the previous menu
		s_cur_hfp_menu = HFPMENU_MAIN;
		*choice = 'm';	// Set to any value except for 'q'.
		HfpShowMenu();
		break;
	default:
		PRINTMSG(1,"Invalid command.\n>");
		break;
	}
}


/*******************************************************************
*																	*
********************************************************************/
void HFCmdInStandbyMenu(BTUINT8 *choice)
{
	BTINT8 buf[32];

	memset(buf, 0, 32);
	switch (*choice)
	{
	case '1': // Last number redial
		Btsdk_HFAP_LastNumRedial(g_cur_hf_hconn);
		break;
	case '2': // Place a call with a phone number
		PRINTMSG(1,"Input the phone number: ");
		OnSelDlg(g_hWnd, _T("Phone number"), _T("Input the phone number: "), buf, 32);	
		if (strlen(buf) != 0)
			Btsdk_HFAP_Dial(g_cur_hf_hconn, buf, (BTUINT16)strlen(buf));
		break;
	case '3': // Memory dialing
		PRINTMSG(1,"Input the memory location: ");
		OnSelDlg(g_hWnd, _T("Memory location"), _T("Input the memory location: "), buf, 32);	
		if (strlen(buf) != 0)
			Btsdk_HFAP_MemNumDial(g_cur_hf_hconn, buf, (BTUINT16)strlen(buf));
		break;
	case '4': // Attach a phone number to a voice tag
		Btsdk_HFAP_VoiceTagPhoneNumReq(g_cur_hf_hconn);
		// The phone number will be returned by BTSDK_HFP_EV_VOICETAG_PHONE_NUM_IND event.
		break;
	case '5': // Activate or deactivate voice recognition
		s_hf_bvra_enable = s_hf_bvra_enable ^ 1;
		Btsdk_HFAP_VoiceRecognitionReq(g_cur_hf_hconn, s_hf_bvra_enable);
		break;
	case '6': // Release connection with the AG
		HFPReleaseConnection(BTSDK_FALSE);
		break;
	case 's': // Show the common sub menu
		s_cur_hf_menu |= HFP_SUBMENU_MASK;
		HfpShowMenu();
		break;
	case 'm': // Show the current menu
		HfpShowMenu();
		break;
	case 'q': // Back to the previous menu
		s_cur_hfp_menu = HFPMENU_MAIN;
		*choice = 'm';	// Set to any value except for 'q'.
		HfpShowMenu();
		break;
	default:
		PRINTMSG(1,"Invalid command.\n>");
		break;
	}
}


/*******************************************************************
*																	*
********************************************************************/
void HFCmdInRingMenu(BTUINT8 *choice)
{
	switch (*choice)
	{
	case '1': // Answer the incoming call
		Btsdk_HFAP_AnswerCall(g_cur_hf_hconn);
		break;
	case '2': // Reject the incoming call
		Btsdk_HFAP_CancelCall(g_cur_hf_hconn);
		break;
	case '3': // BTRH hold the incoming call
		//Btsdk_HFAP_HoldIncomingCall(g_cur_hf_hconn);
		break;
	case 's': // Show the common sub menu
		s_cur_hf_menu |= HFP_SUBMENU_MASK;
		HfpShowMenu();
		break;
	case 'm': // Show the current menu
		HfpShowMenu();
		break;
	case 'q': // Back to the previous menu
		s_cur_hfp_menu = HFPMENU_MAIN;
		*choice = 'm';	// Set to any value except for 'q'.
		HfpShowMenu();
		break;
	default:
		PRINTMSG(1,"Invalid command.\n>");
		break;
	}
}

/*******************************************************************
*																	*
********************************************************************/
void HFCmdInOutgoingCallMenu(BTUINT8 *choice)
{
	switch (*choice)
	{
	case '1': // Terminate the outgoing call
		Btsdk_HFAP_CancelCall(g_cur_hf_hconn);
		break;
	case 's': // Show the common sub menu
		s_cur_hf_menu |= HFP_SUBMENU_MASK;
		HfpShowMenu();
		break;
	case 'm': // Show the current menu
		HfpShowMenu();
		break;
	case 'q': // Back to the previous menu
		s_cur_hfp_menu = HFPMENU_MAIN;
		*choice = 'm';	// Set to any value except for 'q'.
		HfpShowMenu();
		break;
	default:
		PRINTMSG(1,"Invalid command.\n>");
		break;
	}
}


/*******************************************************************
*																	*
********************************************************************/
void HFCmdInOngoingCallMenu(BTUINT8 *choice)
{
	BTINT8 buf[32];

	memset(buf, 0, 32);
	switch (*choice)
	{
	case '1': // Terminate the ongoing call
		Btsdk_HFAP_CancelCall(g_cur_hf_hconn);
		break;
	case '2': // Transmit DTMF
		PRINTMSG(1,"Input the DTMF digit: ");
		do {
			OnSelDlg(g_hWnd, _T("DTMF"), _T("Input the DTMF digit: "), buf, 32);
		} while (buf[0] == 'q');
		if ((buf[0] >= '0' && buf[0] <= '9') || buf[0] == '*' || buf[0] == '#')
			Btsdk_HFAP_TxDTMF(g_cur_hf_hconn, buf[0]);
		break;
	case '3': // Last number redial
		Btsdk_HFAP_LastNumRedial(g_cur_hf_hconn);
		break;
	case '4': // Place a call with a phone number
		PRINTMSG(1,"Input the phone number: ");
		OnSelDlg(g_hWnd, _T("Phone number"), _T("Input the phone number: "), buf, 32);
		if (strlen(buf) != 0)
			Btsdk_HFAP_Dial(g_cur_hf_hconn, buf, (BTUINT16)strlen(buf));
		break;
	case '5': // Memory dialing
		PRINTMSG(1,"Input the memory location: ");
		OnSelDlg(g_hWnd, _T("Memory location"), _T("Input the memory location: "), buf, 32);
		if (strlen(buf) != 0)
			Btsdk_HFAP_MemNumDial(g_cur_hf_hconn, buf, (BTUINT16)strlen(buf));
		break;
	case '6': // Release active call, accept held call (assume one active call and one held call exist)
		Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_1, 0);
		break;
	case '7': // Release the call indicated by 1 (assume at least two calls exist, and the first one is active)
		/*
		To Do: The application shall first get the current call list, then let the user selects the
		call to be released.
		*/
		{ // A simple demo only: get call list first and select the active call indicated by 1 (assume this call exist).
			Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_1, 1);
		}
		break;
	case '8': // Hold calls except for the call indicated by <idx>
		/*
		To Do: The application shall first get the current call list, then let the user selects the
		call to be left active.
		*/
		{ // A simple demo only: get call list first and select the call indicated by 1 (assume at least two active calls exist).
			Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_2, 1);
		}
		break;
	case '9': // Hold all active calls and accept the held call with the lowest call index.
		/*
		To Do: The application shall first get the current call list.
		*/
		// A simple demo only: assume at least one held call exist.
		Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_2, 0);
		break;
	case 'a': // Adds a held call to the conversation (assume one active call and one held call exist)
		Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_3, 0);
		break;
	case 'b': // Leaves the 3-way calling (assume a conference call exists)
		Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_4, 0);
		break;
	case 's': // Show the common sub menu
		s_cur_hf_menu |= HFP_SUBMENU_MASK;
		HfpShowMenu();
		break;
	case 'm': // Show the current menu
		HfpShowMenu();
		break;
	case 'q': // Back to the previous menu
		s_cur_hfp_menu = HFPMENU_MAIN;
		*choice = 'm';	// Set to any value except for 'q'.
		HfpShowMenu();
		break;
	default:
		PRINTMSG(1,"Invalid command.\n>");
		break;
	}
}

/*******************************************************************
*																	*
********************************************************************/
void HFCmdInCallWaitingMenu(BTUINT8 *choice)
{
	BTINT8 buf[32];

	memset(buf, 0, 32);
	switch (*choice)
	{
	case '1': // Hold active call(s), answer the waiting Call
		Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_2, 0);
		break;
	case '2': // Release active call(s), answer the waiting Call
		Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_1, 0);
		break;
	case '3': // Reject the waiting call
		Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_0, 0);
		break;
	case 's': // Show the common sub menu
		s_cur_hf_menu |= HFP_SUBMENU_MASK;
		HfpShowMenu();
		break;
	case 'm': // Show the current menu
		HfpShowMenu();
		break;
	case 'q': // Back to the previous menu
		s_cur_hfp_menu = HFPMENU_MAIN;
		*choice = 'm';	// Set to any value except for 'q'.
		HfpShowMenu();
		break;
	default:
		PRINTMSG(1,"Invalid command.\n>");
		break;
	}
}

/*******************************************************************
*																	*
********************************************************************/
void HFCmdInHeldIncomingCallMenu(BTUINT8 *choice)
{
	BTINT8 buf[32];

	memset(buf, 0, 32);
	switch (*choice)
	{
	case '1': // Accept the Held Incoming Call
		Btsdk_HFAP_AcceptHeldIncomingCall(g_cur_hf_hconn);
		break;
	case '2': // Reject the Held Incoming Call
		Btsdk_HFAP_RejectHeldIncomingCall(g_cur_hf_hconn);
		break;
	case 's': // Show the common sub menu
		s_cur_hf_menu |= HFP_SUBMENU_MASK;
		HfpShowMenu();
		break;
	case 'm': // Show the current menu
		HfpShowMenu();
		break;
	case 'q': // Back to the previous menu
		s_cur_hfp_menu = HFPMENU_MAIN;
		*choice = 'm';	// Set to any value except for 'q'.
		HfpShowMenu();
		break;
	default:
		PRINTMSG(1,"Invalid command.\n>");
		break;
	}
}

/*******************************************************************
*																	*
********************************************************************/
void HFCmdInCommonSubMenu(BTUINT8 *choice)
{
	switch (*choice)
	{
	case '1': // SCO Audio Transfer
		Btsdk_HFAP_AudioConnTrans(g_cur_hf_hconn);
		break;
	case '2': // Request Subscriber Info
		Btsdk_HFAP_GetSubscriberNumber(g_cur_hf_hconn);
		// The subscriber info will be returned by BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND event.
		break;
	case '3': // Request current call list
		Btsdk_HFAP_GetCurrentCalls(g_cur_hf_hconn);
		// The information of each existing call will be returned by BTSDK_HFP_EV_CURRENT_CALLS_IND event.
		break;
	case '4': // Request Current Network Operator Name
		Btsdk_HFAP_NetworkOperatorReq(g_cur_hf_hconn);
		// The network operator name will be returned by BTSDK_HFP_EV_NETWORK_OPERATOR_IND event.
		break;
	case '5': // Disable the EC and NR on AG
		Btsdk_HFAP_DisableNREC(g_cur_hf_hconn);
		break;
	case '6': // Speaker Volume +1
		if (s_cur_hf_spk_vol < 15)
		{
			s_cur_hf_spk_vol++;
			Btsdk_HFAP_SetSpkVol(g_cur_hf_hconn, s_cur_hf_spk_vol);
		}
		break;
	case '7': // Microphone Volume +1
		if (s_cur_hf_mic_vol < 15)
		{
			s_cur_hf_mic_vol++;
			Btsdk_HFAP_SetMicVol(g_cur_hf_hconn, s_cur_hf_mic_vol);
		}
		break;
	case '8': // Speaker Volume -1
		if (s_cur_hf_spk_vol != 0)
		{
			s_cur_hf_spk_vol--;
			Btsdk_HFAP_SetSpkVol(g_cur_hf_hconn, s_cur_hf_spk_vol);
		}
		break;
	case '9': // Microphone Volume -1
		if (s_cur_hf_mic_vol != 0)
		{
			s_cur_hf_mic_vol--;
			Btsdk_HFAP_SetMicVol(g_cur_hf_hconn, s_cur_hf_mic_vol);
		}
		break;
	case 'a': // Request manufacturer and model ID
		{
			BTUINT8 *m_id;
			BTUINT8 m_len = 0;
			Btsdk_HFAP_GetManufacturerID(g_cur_hf_hconn, NULL, (BTUINT16 *)&m_len);
			if (m_len != 0)
			{
				m_id = (BTUINT8*)malloc(m_len);
				memset(m_id, 0, m_len);
				Btsdk_HFAP_GetManufacturerID(g_cur_hf_hconn, m_id, (BTUINT16 *)&m_len);
				PRINTMSG(1,"HF-->Manufacturer of the AG is: %s\n>", m_id);
				free(m_id);
			}
			m_len = 0;
			Btsdk_HFAP_GetModelID(g_cur_hf_hconn, NULL, (BTUINT16 *)&m_len);
			if (m_len != 0)
			{
				m_id = (BTUINT8*)malloc(m_len);
				memset(m_id, 0, m_len);
				Btsdk_HFAP_GetModelID(g_cur_hf_hconn, m_id, (BTUINT16 *)&m_len);
				PRINTMSG(1,"HF-->Model ID of the AG is: %s\n>", m_id);
				free(m_id);
			}
		}
		break;
	case 'b': // Request AG to reject waiting call or release all held calls 
		/*
		To Do: The application shall first get the current call list.
		*/
		// A simple demo only: assume either a waiting call exists or at least one held call exists.
		Btsdk_HFAP_3WayCallingHandler(g_cur_hf_hconn, BTSDK_HFP_CMD_CHLD_0, 0);
		break;
	case 'e':
		{
			BTUINT8 *ext_cmd = (BTUINT8*)malloc(256);
			OnSelDlg(g_hWnd, _T(" AT command"), _T("Input the AT command: "), (char *)ext_cmd, 256);
			if (strlen((char *)ext_cmd) != 0)
			{
				strcat((char *)ext_cmd, "\r");
				Btsdk_HFP_ExtendCmd(g_cur_hf_hconn, (char *)ext_cmd, (BTUINT16)strlen((char *)ext_cmd), 6000);
				// After this command is transmited and before receiving one of "OK", "ERROR" or "+CMER", 
				// all the result codes responded by AG will be returned by BTSDK_HFP_EV_EXTEND_CMD_IND event.
				// The ending "OK", "ERROR" or "+CMER" will also be returned by BTSDK_HFP_EV_EXTEND_CMD_IND event.
			}
			free(ext_cmd);
		}
		break;
	case 'm': // Show the current menu
		HfpShowMenu();
		break;
	case 'q': // Back to the previous menu
		s_cur_hf_menu &= (~HFP_SUBMENU_MASK);
		*choice = 'm';	// Set to any value except for 'q'.
		HfpShowMenu();
		break;
	default:
		PRINTMSG(1,"Invalid command.\n>");
		break;
	}
}

/*******************************************************************
*																	*
********************************************************************/
void HfpCmdInHFMenu(BTUINT8 *choice)
{
	switch (s_cur_hf_menu)
	{
	case HFMENU_IDLE:
		HFCmdInIdleMenu(choice);
		break;
	case HFMENU_STANDBY:
		HFCmdInStandbyMenu(choice);
		break;
	case HFMENU_RINGING:
		HFCmdInRingMenu(choice);
		break;
	case HFMENU_OUTGOINGCALL:
		HFCmdInOutgoingCallMenu(choice);
		break;
	case HFMENU_ONGOINGCALL:
		HFCmdInOngoingCallMenu(choice);
		break;
	case HFMENU_CALLWAITING:
		HFCmdInCallWaitingMenu(choice);
		break;
	case HFMENU_HELDINCOMINGCALL:
		HFCmdInHeldIncomingCallMenu(choice);
		break;
	default:
		HFCmdInCommonSubMenu(choice);
		break;
	}
}

//----------------------------------------------------------------------------------------------------
// HFP initialization
//----------------------------------------------------------------------------------------------------
void HfpAppInit(void)
{
	// Default as HFP device (HF)
	char czSvcName[MAX_PATH] = "Hands-free unit";
	Btsdk_HFAP_APPRegCbk( App_HFCbkFunc );

	s_hf_svc = Btsdk_RegisterHFPService((BTUINT8 *)czSvcName, BTSDK_CLS_HANDSFREE, BTSDK_HF_BRSF_3WAYCALL|
		BTSDK_HF_BRSF_CLIP|BTSDK_HF_BRSF_BVRA|BTSDK_HF_BRSF_RMTVOLCTRL|
		BTSDK_HF_BRSF_ENHANCED_CALLSTATUS|BTSDK_HF_BRSF_ENHANCED_CALLCONTROL);
}

//----------------------------------------------------------------------------------------------------
// HFP deinitialization
//----------------------------------------------------------------------------------------------------
void HfpAppDone(void)
{
	// Default as HFP device (HF)
	Btsdk_UnregisterHFPService(s_hf_svc);
	Btsdk_HFAP_APPRegCbk(NULL);
}