#include "sdk_tst.h"
#include "profiles_tst.h"
#include "Btsdk_Stru.h"
#include "Btsdk_API.h"
/* current remote audio device handle */
static BTDEVHDL s_currAudioRmtDevHdl = BTSDK_INVALID_HANDLE;
/* current remote audio device AV service handle */
static BTSHCHDL s_currAudioSvcHdl = BTSDK_INVALID_HANDLE;
/* current remote audio device connection handle */
static BTCONNHDL s_currAudioConnHdl = BTSDK_INVALID_HANDLE;
static BTSVCHDL s_currAVRCPSvcHdl = BTSDK_INVALID_HANDLE;
////AVRCP 
static BTUINT16 s_AVRCPSvcClass = 0;
static BTUINT8 s_musicName[10] = "believe";
static BTUINT8 s_artistName[15] = "artist name";
static BTUINT8 s_albumName[10] = "album name";
static BTUINT32 s_musicPos = 0;
static char s_current_folder[MAX_PATH] = TEXT("Root");
HANDLE g_AVRCPTestEvent = NULL;
BTUINT8 s_cPlayingTime[MAX_PATH] = {0};
void AVRCP_App_RegNotifReq(BTUINT8 event_id);
void AVRCP_App_GeneralRejectRsp(BTDEVHDL hdl, BTUINT8 tl, BTUINT16 cmd_type, BTUINT8 error_code);
void AVRCP_App_GetElementAttrReq();
void AVRCP_App_GetPlayStatusInd(BTDEVHDL hdl, BTUINT8 tl);
void AVRCP_App_GetCurPlayerAppSetValInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetCurPlayerAppSetValReqStru in);
void AVRCP_App_SetCurPlayerAppSetValInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkSetCurPlayerAppSetValReqStru in);
void AVRCP_Exp_GetPlayerAppSetAttrTextInd(BTDEVHDL dev_hdl, BTUINT8 tl, PBtSdkGetPlayerAppSetAttrTxtReqStru in);
void AVRCP_call_Back_events_reg();
void AVRCP_call_Back_events_dereg();
void AVRCP_Exp_GetFolderItemInd(BTDEVHDL dev_hdl, BTUINT8 tl, PBtSdkGetFolderItemReqStru in);
void AVRCP_App_InformBattStatusCfm(PBtSdkInformBattStatusReqStru pBattChanged);
void AVRCP_App_ListPlayerAppSetAttrCfm(PBtSdkListPlayerAppSetAttrRspStru pListPlAppSetAttr);
void AVRCP_App_ListPlayerAppSetValCfm(PBtSdkListPlayerAppSetValRspStru pListPlAppSetValRsp);
void AVRCP_App_SetAddressedPlayerCfm(PBtSdkSetAddresedPlayerReqStru pSetAddressedPlayerRsp);
void AVRCP_APP_AddToNowPlayingInd();
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is to select expected remote device. 
Arguments:
Return:
void 
---------------------------------------------------------------------------*/
void TestSelectRmtAudioDev()
{
	//s_currAudioRmtDevHdl = SelectRemoteDevice(BTSDK_DEVCLS_MASK(BTSDK_AV_HEADSET));
	s_currAudioRmtDevHdl = SelectRemoteDevice(0);
	if (BTSDK_INVALID_HANDLE == s_currAudioRmtDevHdl)
	{
		printf("Please make sure that the expected device is in discoverable state and search again.\n");
	}
	else
	{
		printf("Select remote audio device successfully.\n");
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is to get service handle according to given device handle.
Arguments:
Return:
void 
---------------------------------------------------------------------------*/
void TestSelectAudioSvc()
{
	s_currAudioSvcHdl = SelectRemoteService(s_currAudioRmtDevHdl);
	if (BTSDK_INVALID_HANDLE == s_currAudioSvcHdl)
	{
		printf("Can't get expected service handle.\n");
	}
	else
	{
		printf("Select remote audio device's service successfully.\n");
	}
}

void TestShowAVRCPServerMenu()
{
	printf("*****************************************\n");
	printf("*    Please select the Server class     *\n");
	printf("* <1>    AVRCP_TG  (Local is CT)        *\n");
	printf("* <2>    AVRCP_CT  (Local is TG)        *\n");
	printf("* <r>    Return to the upper menu       *\n");
	printf("*****************************************\n");
	printf(">>");
}

void TestShowPassThroughMenu()
{
	printf("*****************************************\n");
	printf("*    Please select the choice you want: *\n");
	printf("* <1>   POWER                           *\n");
	printf("* <2>   VOLUME UP                       *\n");
	printf("* <3>   VOLUME DOWN                     *\n");
	printf("* <4>   MUTE                            *\n");
	printf("* <5>   PLAY                            *\n");
	printf("* <6>   STOP                            *\n");
	printf("* <7>   PAUSE                           *\n");
	printf("* <8>   RECORD                          *\n");
	printf("* <9>   REWIND                          *\n");
	printf("* <a>   FAST_FORWARD                    *\n");
	printf("* <b>   EJECT                           *\n");
	printf("* <c>   FORWEAD                         *\n");
	printf("* <d>   BACKWARD                        *\n");
	printf("* <r>   Return the upper menu           *\n");
	printf("*****************************************\n");
	printf(">>");
}

void TestChooseAVRCPPassThroughCommand(BTUINT8 *index)
{
	BTUINT8 ch = 0;
	TestShowPassThroughMenu();
	while (ch != 'r')
	{		
		scanf(" %c", &ch);	
		getchar();
		if (ch == '\n')
		{
			printf(">>");
		}
		else
		{   
			if(('0'<ch && ch<'10') || ('a'<=ch && ch<= 'd'))
			{
				*index = ch;
			}else
			{
				printf("Invalid command.\n");
			}
			break;
		}
	}
}

void TestSelectAVRCPSvc()
{
	BtSdkRemoteServiceAttrStru attribute = {0};
	s_currAVRCPSvcHdl = SelectRemoteService(s_currAudioRmtDevHdl);
	if (BTSDK_INVALID_HANDLE == s_currAVRCPSvcHdl)
	{
		printf("Cann't get expected service handle.\n");
		return;
	}
	if (BTSDK_OK == Btsdk_GetRemoteServiceAttributes(s_currAVRCPSvcHdl, &attribute))
	{
		if (attribute.service_class == BTSDK_CLS_AVRCP_TG)
		{
			s_AVRCPSvcClass = BTSDK_CLS_AVRCP_TG;
			printf("Remote is AVRCP_TG  (Local is CT)\n");
		}
		else if (attribute.service_class == BTSDK_CLS_AVRCP_CT)
		{
			s_AVRCPSvcClass = BTSDK_CLS_AVRCP_CT;
			printf("Remote is AVRCP_CT  (Local is TG)\n");
		}
		else
		{
			printf("Select wrong service handle.\n");
		}
	}
	else
	{
		printf("Invalid service handle.\n");
		return;
	}
}

void TestAVRCPShowMenu(BTUINT16 service_class)
{
	if(BTSDK_CLS_AVRCP_CT == service_class)
	{
		printf("*****************************************\n");
		printf("*    Please select the choice you want: *\n");
		printf("* <1>   Track changed                   *\n");
		printf("* <2>   Position changed                *\n");
		printf("* <3>   Play status changed             *\n");
		printf("* <4>   Media Player setting changed.   *\n");
		printf("* <5>   Track reached the end           *\n");
		printf("* <6>   Track reached the start         *\n");
		printf("* <7>   Battery status changed          *\n");
		printf("* <8>   System status changed           *\n");
		printf("* <9>   Volume changed                  *\n");
		printf("* <r>   Return to the upper menu        *\n");
		printf("*****************************************\n");
		printf(">>");
	}
	else if (BTSDK_CLS_AVRCP_TG == service_class)
	{
		printf("*****************************************\n");
		printf("*    Please select the choice you want: *\n");
		printf("* <1>   CHOOSE PASS THROUGH COMMAND     *\n");
		printf("* <2>   GET MUSIC INFORMATION           *\n");
		printf("* <3>   BROWSE                          *\n");
		printf("* <4>   SEND BATTERY STATUS TO TG       *\n");
		printf("* <5>   SET PLAYER SETTING VALUE        *\n");
		printf("* <6>   SET ABSOLUTE VOLUME		        *\n");
		printf("* <r>   Return the upper menu           *\n");
		printf("*****************************************\n");
		printf(">>");
	}	 
}

BTUINT32 SendPassThroughCommand(BTUINT8 op_id)
{
	BOOL bRet = FALSE;
	BTINT32 lRet1 = BTSDK_FALSE;
	BTINT32 lRet2 =BTSDK_FALSE;
	
	BtSdkPassThrReqStru CommandStru = {0};
	CommandStru.dev_hdl = s_currAudioRmtDevHdl;
	CommandStru.state_flag = BTSDK_AVRCP_BUTTON_STATE_PRESSED;
	CommandStru.op_id = op_id;
	CommandStru.length = 0;
	lRet1 = Btsdk_AVRCP_PassThroughReq(&CommandStru);
	
	CommandStru.state_flag = BTSDK_AVRCP_BUTTON_STATE_RELEASED;
	lRet2 = Btsdk_AVRCP_PassThroughReq(&CommandStru);
	
	if ((BTSDK_OK == lRet1)&&(BTSDK_OK == lRet2))
	{
		bRet = TRUE;
	}
	return bRet;
}

void TestAVRCPCTFunc(BTUINT8 choiceID)
{
	switch(choiceID)
	{
	case '1':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_POWER);
		break;
	case '2':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_UP);
		break;
	case '3':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_DOWN);
		break;
	case '4':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_MUTE);
		break;
	case '5':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_PLAY);
		break;
	case '6':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_STOP);
		break;
	case '7':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_PAUSE);
		break;
	case '8':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_RECORD);
		break;
	case '9':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_REWIND);
		break;
	case 'a':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_FAST_FORWARD);
		break;
	case 'b':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_EJECT);
		break;
	case 'c':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_FORWARD);
		break;
	case 'd':
		SendPassThroughCommand(BTSDK_AVRCP_OPID_AVC_PANEL_BACKWARD);
		break;
	case 'e':
		{
			AVRCP_App_GetElementAttrReq();
		}
		break;
	default:
		printf("Invalid command.\n");
		break;
	}
}

void TestAVRCPTGFunc(BTUINT8 choiceID)
{
	PBtSdkTrackChangedStru rsp_notif = NULL;
	PBtSdkPlayPosChangedStru rsp_notif2 = NULL;
	PBtSdkTrackReachEndStru rsp_notif3 = NULL;
	PBtSdkTrackReachStartStru rsp_notif4 = NULL;
	BTUINT16 size = 0;
	BTUINT32 ret = 0;
	
	switch(choiceID)
	{
	case '1':
		rsp_notif = NULL;
		size = sizeof(BtSdkTrackChangedStru);
		rsp_notif = (PBtSdkTrackChangedStru)malloc(size);
		if (rsp_notif != NULL)
		{
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = BTSDK_AVRCP_RSP_CHANGED;
			memset(rsp_notif->identifier, 0xFF, 8);
			ret = Btsdk_AVRCP_EventTrackChanged((PBtSdkTrackChangedStru)rsp_notif);
			if(BTSDK_OK == ret)
			{
				printf("Change the music name successful!");
			}
			
			if(strcmp(s_musicName, "believe"))
			{
				strcpy(s_musicName, "believe");
			}
			else
			{
				strcpy(s_musicName, "onlyyou");
			}
			free(rsp_notif);
			rsp_notif = NULL;
			printf("TestAVRCPTGFunc function  Case 1.\n");
		}
		break;
	case'2':
		rsp_notif2 = NULL;
		size = sizeof(BtSdkPlayPosChangedStru);
		rsp_notif2 = (PBtSdkPlayPosChangedStru)malloc(size);
		if(rsp_notif2 != NULL)
		{
			memset(rsp_notif2, 0, size);
			rsp_notif2->size = size;
			rsp_notif2->rsp_code = BTSDK_AVRCP_RSP_CHANGED;
			if(s_musicPos+10<169)
			{
				s_musicPos = s_musicPos +10;
			}
			else
			{
				s_musicPos = 0;
			}
			rsp_notif2->pos = s_musicPos*1000;
			ret = Btsdk_AVRCP_EventPlayPosChanged(rsp_notif2);
			if(BTSDK_OK == ret)
			{
				printf("Change the music position successful!");
			}
			free(rsp_notif2);
			rsp_notif2 = NULL;
			printf("TestAVRCPTGFunc function  Case 2.\n");
		}
		break;
	case '3':
		{
			//This tell the CT current status of media player.
			PBtSdkPlayStatusChangedStru play_statuschaned = NULL;
			BTUINT32 size = sizeof(BtSdkPlayStatusChangedStru);
			play_statuschaned = (PBtSdkPlayStatusChangedStru)malloc(size);
			if (NULL != play_statuschaned)
			{
				memset(play_statuschaned, 0, size);
				play_statuschaned->size = size;
				play_statuschaned->rsp_code = BTSDK_AVRCP_RSP_CHANGED;
				play_statuschaned->id = BTSDK_AVRCP_PLAYSTATUS_STOPPED; /* If no track currently selected, then return 0xFFFFFFFF in the INTERIM response. */
				Btsdk_AVRCP_EventPlayStatusChanged(play_statuschaned);
				free(play_statuschaned);
				play_statuschaned = NULL;
			}
			printf("TestAVRCPTGFunc function  Case 3.\n");
		}
		break;
	case '4':
		{
			//This tell the CT current media player setting value changed.
			BTUINT8 num = 1;
			BTUINT8 k = 0;
			PBtSdkPlayerAppSetChangedStru rsp_notif = NULL;
			
			BTUINT32 size = sizeof(BtSdkPlayerAppSetChangedStru) + (num - 1) * sizeof(BtSdkIDPairStru);
			rsp_notif = (PBtSdkPlayerAppSetChangedStru)malloc(size);
			if (NULL == rsp_notif) 
			{
				return;
			}
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = BTSDK_AVRCP_RSP_CHANGED;
			rsp_notif->num = num;
			rsp_notif->id[0].attr_id = BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS;
			rsp_notif->id[0].value_id = BTSDK_AVRCP_REPEAT_MODE_ALL_TRACK_REPEAT;
			Btsdk_AVRCP_EventPlayerAppSetChanged(rsp_notif);
			free(rsp_notif);
			rsp_notif = NULL;
			printf("TestAVRCPTGFunc function  Case 4.\n");
		}
		break;
	case '5':
		{
			//This tells CT track has reached the end.
			BTUINT32 size = sizeof(BtSdkTrackReachEndStru); 
			rsp_notif3 = (PBtSdkTrackReachEndStru)malloc(size);
			if (NULL != rsp_notif3)
			{
				memset(rsp_notif3, 0, size);
				rsp_notif3->size = size;
				rsp_notif3->rsp_code = BTSDK_AVRCP_RSP_CHANGED;
				ret = Btsdk_AVRCP_EventTrackReachEnd(rsp_notif3);
				if(BTSDK_OK == ret)
				{
					printf("track has reached the end.\n");
					printf("TestAVRCPTGFunc function  Case 5.\n");
				}
				free(rsp_notif3);
			}
			break;
		}
	case '6':
		{
			//This tells CT track has reached the start.
			BTUINT32 size4 = sizeof(BtSdkTrackReachStartStru); 
			rsp_notif4 = (PBtSdkTrackReachStartStru)malloc(size4);
			if (NULL != rsp_notif4)
			{
				memset(rsp_notif4, 0, size4);
				rsp_notif4->size = size4;
				rsp_notif4->rsp_code = BTSDK_AVRCP_RSP_CHANGED;
				ret = Btsdk_AVRCP_EventTrackReachStart(rsp_notif4);
				if(BTSDK_OK == ret)
				{
					printf("track has reached the start.\n");
					printf("TestAVRCPTGFunc function  Case 6.\n");
				}
				free(rsp_notif4);
			}
			break;
		}
	case '7':
		{
			//This tells CT battery status has changed.
			PBtSdkBattStatusChangedStru rsp_notif_batt = NULL;
			BTUINT32 size_batt = sizeof(BtSdkBattStatusChangedStru);
			rsp_notif_batt = (PBtSdkBattStatusChangedStru)malloc(size_batt);
			if (NULL != rsp_notif_batt)
			{
				memset(rsp_notif_batt, 0, size_batt);
				rsp_notif_batt->size = sizeof(BtSdkBattStatusChangedStru);
				rsp_notif_batt->rsp_code = BTSDK_AVRCP_RSP_CHANGED;
				rsp_notif_batt->id = BTSDK_AVRCP_BATTERYSTATUS_FULL_CHARGE;
				ret = Btsdk_AVRCP_EventBattStatusChanged(rsp_notif_batt);
				if(BTSDK_OK == ret)
				{
					printf("TG battery is full charge.\n");
					printf("TestAVRCPTGFunc function  Case 7.\n");
				}
				free(rsp_notif4);
			}
			break;
		}
	case '8':
		{
			//This tells CT system status has changed.  
			PBtSdkSysStatusChangedStru rsp_notif_sys = NULL;
			BTUINT32 size_sys = sizeof(BtSdkSysStatusChangedStru); 
			rsp_notif_sys = (PBtSdkSysStatusChangedStru)malloc(size_sys);
			if (NULL != rsp_notif_sys)
			{
				rsp_notif_sys->size = sizeof(BtSdkSysStatusChangedStru);
				rsp_notif_sys->rsp_code = BTSDK_AVRCP_RSP_CHANGED;
				rsp_notif_sys->id = 0x00; //power_on
				ret = Btsdk_AVRCP_EventSysStatusChanged(rsp_notif_sys);
				if(BTSDK_OK == ret)
				{
					printf("TG system is power on.\n");
					printf("TestAVRCPTGFunc function  Case 8.\n");
				}
			}
			break;
		}
	case '9':
		{
			//This tells CT volume has changed.
			PBtSdkVolChangedStru rsp_notif_vol = NULL;
			BTUINT32 size_vol = sizeof(BtSdkVolChangedStru);
			rsp_notif_vol = (PBtSdkVolChangedStru)malloc(size_vol);
			if ( NULL != rsp_notif_vol)
			{
				rsp_notif_vol->size = sizeof(BtSdkVolChangedStru);
				rsp_notif_vol->rsp_code = BTSDK_AVRCP_RSP_CHANGED;
				rsp_notif_vol->id = BTSDK_AVRCP_ABSOLUTE_VOLUME_MIN;
				ret = Btsdk_AVRCP_EventVolChanged(rsp_notif_vol);
				if(BTSDK_OK == ret)
				{
					printf("TG volume is min.\n");
					printf("TestAVRCPTGFunc function  Case 9.\n");
				}
			}
			break;
		}
	default:
		printf("TestAVRCPTGFunc function  Default.\n");
		break;
	}
}

void TestConnectAVRCPSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;
	BTCONNHDL conn_hdl = BTSDK_INVALID_HANDLE;
	BTUINT8 ch = 0;
	if (s_AVRCPSvcClass == BTSDK_CLS_AVRCP_TG)
	{
		ulRet = Btsdk_ConnectEx(s_currAudioRmtDevHdl, s_AVRCPSvcClass, 0, &conn_hdl);
		if (BTSDK_OK != ulRet)
		{
			printf("Please make sure that the expected device is powered on and connectable.\n");
			return;
		}
	}	
	else
	{
		printf("Please let the remote device connect local's AVRCP TG service.\n");
		
	}
	TestAVRCPShowMenu(s_AVRCPSvcClass);
	while('r' != ch)
	{
		scanf("%c", &ch);
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
			if(s_AVRCPSvcClass == BTSDK_CLS_AVRCP_TG)
			{
				if(ch == '1')
				{
					TestChooseAVRCPPassThroughCommand(&ch);
					if('r' == ch)
					{
						TestAVRCPShowMenu(s_AVRCPSvcClass);
						ch = 0;
						continue;
					}
					TestAVRCPCTFunc(ch);
					
				}
				else if(ch == '2')
				{
					TestAVRCPCTFunc('e');
				}
				else if(ch == '3')
				{
					printf("Browsing\n");
					TestAVRCPBrowsing();
				}
				else if(ch == '4')
				{
					printf("start battery funcion\n");
					TestAVRCPInformBattStatusFunc();
				}
				else if (ch == '5')
				{
					printf("set PlayerSetting Attributes.\n");
					TestAVRCPSetCurPlayerSettingValFunc();
				}
				else if (ch == '6')
				{
					printf("set Absolute Vol.\n");
					TestAVRCPSetAbsoluteVolFunc();
				}
			}
			else if(s_AVRCPSvcClass == BTSDK_CLS_AVRCP_CT)
			{
				BOOL bIsLocalBeTG = FALSE;
				BTCONNHDL conn_hdl = BTSDK_INVALID_HANDLE;
				BTSDKHANDLE enum_handle = BTSDK_INVALID_HANDLE;
				BtSdkConnectionPropertyStru conn_prop = {0};
				enum_handle = Btsdk_StartEnumConnection();
				if (enum_handle != BTSDK_INVALID_HANDLE)
				{
					while(BTSDK_INVALID_HANDLE != (conn_hdl = Btsdk_EnumConnection(enum_handle, &conn_prop)))
					{
						if (conn_prop.service_class == BTSDK_CLS_AVRCP_TG
							&& conn_prop.role == BTSDK_CONNROLE_SERVER)
						{
							bIsLocalBeTG = TRUE;
							break;
						}
					}
					Btsdk_EndEnumConnection(enum_handle);
				}
				if (bIsLocalBeTG)
				{
					TestAVRCPTGFunc(ch);
				}
				else
				{
					printf("Please let the remote device connect local's AVRCP TG service.\n");
					continue;
				}
			}
			printf("\n");	
			TestAVRCPShowMenu(s_AVRCPSvcClass);
		}			
	}
}

void TestAVRCPBattStatusMenu()
{
	printf("*****************************************\n");
	printf("*    Please select the choice you want: *\n");
	printf("* <1>   Battery in normal state         *\n");
	printf("* <2>   Unable to operate soon.         *\n");
	printf("* <3>   Can not operate any more.       *\n");
	printf("* <4>   Connect external power supply   *\n");
	printf("* <5>   device is completely charged    *\n");
	printf("* <r>   Return the upper menu           *\n");
	printf("*****************************************\n");
	printf(">>");
}

void TestAVRCPBrowseShowMenu()
{
	printf("*****************************************\n");
	printf("*    Please select the choice you want: *\n");
	printf("* <1>   Get folder Item                 *\n");
	printf("* <2>   Set address player req          *\n");
	printf("* <3>   Set browse player               *\n");
	printf("* <4>   Change Path                     *\n");
	printf("* <5>   Get folder item                 *\n");
	printf("* <6>   Get item attributes             *\n");
	printf("* <7>   Search request                  *\n");
	printf("* <8>   Play item request               *\n");
	printf("* <9>   NowPlaying req                  *\n");
	printf("* <r>   Return the upper menu           *\n");
	printf("*****************************************\n");
	printf(">>");
}

void AVRCPNowPlayingReq()
{
	BtSdkAddToNowPlayingReqStru addToNowPlaying;
}

void AVRCPPlayItemReq()
{
	
	PBtSdkPlayItemReqStru pram=NULL;
	
	int len=0,count=4,ret=0;
	len=sizeof(BtSdkPlayItemReqStru);
	pram=(PBtSdkPlayItemReqStru)malloc(len);
	
	memset(pram,0,len);
	pram->size = sizeof(BtSdkPlayItemReqStru);
	pram->scope=BTSDK_AVRCP_SCOPE_MEDIAPLAYER_VIRTUAL_FILESYSTEM|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_LIST|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_SEARCH|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_NOWPLAYING;
	sprintf(pram->uid,"0x0");
	pram->uid_counter=1;
	
	ret=Btsdk_AVRCP_PlayItemReq(s_currAudioRmtDevHdl,pram);
	if(ret==BTSDK_OK)
	{
		printf("Get folder Btsdk_AVRCP_PlayItemReq success\n");
	}
	else
	{
		printf("Get folder Btsdk_AVRCP_PlayItemReq fail error code %0x\n",ret);
	}
}

void AVRCPGetItemAttrReq()
{
	PBtSdkGetItemAttrReqStru pGIAreq=NULL;
	int len=0,count=4,ret=0;
	
	len=sizeof(BtSdkGetItemAttrReqStru)+count*1;
	pGIAreq=(PBtSdkGetItemAttrReqStru)malloc(len);
	memset(pGIAreq,0,len);
	pGIAreq->size = sizeof(BtSdkSetBrowsedPlayerReqStru);
	pGIAreq->scope=BTSDK_AVRCP_SCOPE_MEDIAPLAYER_VIRTUAL_FILESYSTEM|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_LIST|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_SEARCH|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_NOWPLAYING;
	sprintf(pGIAreq->uid,"0x0");
	
	pGIAreq->uid_counter=0;
	pGIAreq->attr_num=0;
	pGIAreq->attr_id[0]=BTSDK_AVRCP_MA_TOTALNUMBEROF_MEDIA;
	pGIAreq->attr_id[1]=BTSDK_AVRCP_MA_NAMEOF_ARTIST;
	pGIAreq->attr_id[2]=BTSDK_AVRCP_MA_NAMEOF_ALBUM;
	pGIAreq->attr_id[3]=BTSDK_AVRCP_MA_NUMBEROF_MEDIA;
	
	ret=Btsdk_AVRCP_GetItemAttrReq(s_currAudioRmtDevHdl,pGIAreq);
	if(ret==BTSDK_OK)
	{
		printf("Get folder Btsdk_AVRCP_GetItemAttrReq success\n");
	}
	else
	{
		printf("Get folder Btsdk_AVRCP_GetItemAttrReq fail error code %0x\n",ret);
	}
}

void AVRCPSetBrowsedPlayerReq()
{
	PBtSdkGetFolderItemReqStru PFldrItem=NULL;
	PBtSdkSetBrowsedPlayerReqStru pnpl = NULL;
	
	int len=0,count=4,ret=0;
	len=sizeof(BtSdkSetBrowsedPlayerReqStru);
	pnpl=(PBtSdkGetFolderItemReqStru)malloc(len);
	memset(pnpl,0,len);
	pnpl->size = sizeof(BtSdkSetBrowsedPlayerReqStru);
	pnpl->id = 0x0;
	
	ret=Btsdk_AVRCP_SetBrowsedPlayerReq(s_currAudioRmtDevHdl,pnpl);
	if(ret==BTSDK_OK)
	{
		printf("Get folder Btsdk_AVRCP_SetBrowsedPlayerReq success\n");
	}
	else
	{
		printf("Get folder Btsdk_AVRCP_SetBrowsedPlayerReq fail error code %0x\n",ret);
	}
}

void AVRCPGetFolderItem()
{
	PBtSdkGetFolderItemReqStru PFldrItem = NULL;
	BtSdkGetFolderItemReqStru tobj; 
	int len = 0;
	int count = 4;
	int ret = 0;
	
	/*
	PFldrItem->size=sizeof(BtSdkGetFolderItemReqStru);
	PFldrItem->scope=BTSDK_AVRCP_SCOPE_MEDIAPLAYER_LIST;//|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_VIRTUAL_FILESYSTEM|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_SEARCH|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_NOWPLAYING;
	PFldrItem->start_item=1;
	PFldrItem->end_item = 1;
	PFldrItem->attr_count=0x01;
	*/
	//	PFldrItem->attr_id[0]=BTSDK_AVRCP_MA_TOTALNUMBEROF_MEDIA;
	//	PFldrItem->attr_id[1]=BTSDK_AVRCP_MA_NAMEOF_ARTIST;
	//	PFldrItem->attr_id[2]=BTSDK_AVRCP_MA_NAMEOF_ALBUM;
	//	PFldrItem->attr_id[3]=BTSDK_AVRCP_MA_NUMBEROF_MEDIA;
	//	PFldrItem->size=sizeof(BtSdkGetFolderItemReqStru);
	
    tobj.scope = BTSDK_AVRCP_SCOPE_MEDIAPLAYER_LIST;//|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_VIRTUAL_FILESYSTEM|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_SEARCH|BTSDK_AVRCP_SCOPE_MEDIAPLAYER_NOWPLAYING;
	tobj.start_item = 1;
	tobj.end_item = 1;
	tobj.attr_count = 0x01;
	tobj.attr_id[0] = BTSDK_AVRCP_MA_TOTALNUMBEROF_MEDIA;
	tobj.size = sizeof(tobj);
		
	len = sizeof(tobj);
	PFldrItem = (PBtSdkGetFolderItemReqStru)malloc(len);
	memset(PFldrItem, 0, len);
    memcpy((void*)PFldrItem, &tobj, sizeof(tobj));
	
	ret = Btsdk_AVRCP_GetFolderItemsReq(s_currAudioRmtDevHdl,PFldrItem);
	if(ret == BTSDK_OK)
	{
		printf("Get folder item success\n");
	}
	else
	{
		printf("Get folder item fail error code %0x\n",ret);
	}
	free(PFldrItem);
}

void AVRCPSetAbsoluteVolCfm(PBtSdkSetAbsoluteVolRspStru pSetAbsVol)
{
	printf("----------The absolute volume is %d----------\n",pSetAbsVol->id);
}

void AVRCPVolumeChangedReq(PBtSdkVolChangedStru pVolumeChanged)
{
	if (pVolumeChanged->id == 0x00)
	{
		printf("TG volume has changed to min.\n");
	}
	else
	{
		printf("no TG volume changed.\n");
	}
	
}

void AVRCPSystemStatusChangedReq(PBtSdkSysStatusChangedStru pSysStatusChanged)
{
	if (pSysStatusChanged->id == 0x00)
	{
		printf("TG system status changed to power on.\n");
	}
	else
	{
		printf("no TG system status changed.\n");
	}
}

void AVRCPBattStatusChangedReq(PBtSdkBattStatusChangedStru pBattStatusChanged)
{
	if (pBattStatusChanged->id == BTSDK_AVRCP_BATTERYSTATUS_FULL_CHARGE)
	{
		printf("TG battery status is full charged.\n");
	}
	else if(pBattStatusChanged->id < 0)
	{
		printf("no TG battery status changed.\n");
	}
}

void AVRCPTrackReachedStart(PBtSdkTrackReachEndStru pTrackReachStart)
{
	if (pTrackReachStart == NULL)
	{
		printf("----------none-------\n");
		return;
	}
	printf("-----------reached start---------------\n");
//	AVRCP_App_RegNotifReq(BTSDK_AVRCP_EVENT_TRACK_REACHED_START);
	printf("AVRCP_App_TrackReachedStart function.\n");
}

void AVRCPTrackReachedEnd(PBtSdkTrackReachEndStru pTrackReachEnd)
{
	if (pTrackReachEnd == NULL)
	{		
		printf("----------none-------\n");
		return;
	}
	printf("-----------reached end-----------------\n");
//	AVRCP_App_RegNotifReq(BTSDK_AVRCP_EVENT_TRACK_REACHED_END);
	printf("AVRCP_App_TrackReachedEnd function.\n");
}

void AVRCPSetAddressedPlayerReq()
{
	int ret=0;
	PBtSdkSetAddresedPlayerReqStru setplayeraddress = NULL;
	setplayeraddress=(PBtSdkSetAddresedPlayerReqStru)malloc(sizeof(BtSdkSetAddresedPlayerReqStru));
	if (setplayeraddress != NULL)
	{
		memset(setplayeraddress, 0, sizeof(BtSdkSetAddresedPlayerReqStru));
		setplayeraddress->size = sizeof(BtSdkSetAddresedPlayerReqStru);
		setplayeraddress->id=0x00;
		ret = Btsdk_AVRCP_SetAddressedPlayerReq(s_currAudioRmtDevHdl, setplayeraddress);
		if(BTSDK_OK == ret)
		{
			printf("set player address success\n");
		}
		else
		{
			printf("set player address fail error code %0x\n",ret);
		}
		free(setplayeraddress);
	}
	AVRCP_App_RegNotifReq(BTSDK_AVRCP_EVENT_ADDRESSED_PLAYER_CHANGED);
}

void AVRCPChangePathReq()
{
	int ret=0;
	PBtSdkChangePathReqStru chngPath = NULL;
	chngPath=(PBtSdkChangePathReqStru)malloc(sizeof(BtSdkChangePathReqStru));
	if (chngPath != NULL)
	{
		memset(chngPath, 0, sizeof(BtSdkChangePathReqStru));
		chngPath->size = sizeof(BtSdkChangePathReqStru);
		chngPath->uid_counter = 0x01;
		chngPath->direction = BTSDK_AVRCP_DIRECTION_FOLDER_UP;
		//memcpy(chngPath->folder_uid, folder_uid, 8); //Please refer to 6.10.3
		ret = Btsdk_AVRCP_ChangePathReq(s_currAudioRmtDevHdl, chngPath);
		if(BTSDK_OK == ret)
		{
			printf("change path success\n");
		}
		else
		{
			printf("change path fail error code %0x\n",ret);
		}
		free(chngPath);
	}
}

void AVRCPSearchReq()
{
	int ret=0;
	PBtSdkSearchReqStru search = NULL;
	char name[16] = {0};
	BTUINT16 len = 0;
	sprintf(name, "songs");
	len = strlen(name); 
	search=(PBtSdkSearchReqStru)malloc(sizeof(BtSdkSearchReqStru) + len -1);
	if (search != NULL)
	{
		memset(search,0,sizeof(BtSdkSearchReqStru));	
		search->size= sizeof(BtSdkSearchReqStru);
		search->characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;
		search->len = len;
		strcpy(search->string, name);	
		ret = Btsdk_AVRCP_SearchReq(s_currAudioRmtDevHdl, search);
		if(BTSDK_OK == ret)
		{	
			printf("search request success\n");
		}
		else
		{	
			printf("search request error code %0x\n",ret);
		}
		free(search);
	}
}

void TestBrowsingCommand(char ch)
{
	switch(ch)
	{
	case '1':
		AVRCPGetFolderItem();
		break;
	case '2':
		AVRCPSetAddressedPlayerReq();
		break;
	case '3':
		AVRCPSetBrowsedPlayerReq();
		break;
	case '4':
		AVRCPChangePathReq();
		break;
	case '5':
		AVRCPGetFolderItem();
		break;
	case '6':
		AVRCPGetItemAttrReq();
		break;
	case '7':
		AVRCPSearchReq();
		break;
	case '8':
		AVRCPPlayItemReq();
		break;
	case '9':
		AVRCPNowPlayingReq();
		break;
	}
}

void AVRCPStatusNormal()
{
	BTUINT32 ret = 0;
	BtSdkInformBattStatusReqStru battStatus = {0};
	battStatus.size = sizeof(BtSdkInformBattStatusReqStru);
	battStatus.id = BTSDK_AVRCP_BATTERYSTATUS_NORMAL;
	ret = Btsdk_AVRCP_InformBattStatusReq(s_currAudioRmtDevHdl, &battStatus);
	if (BTSDK_OK == ret)
	{
		printf("AVRCP Battery Status Normal has send to TG.\n");
	}
}

void AVRCPStatusWarning()
{
	BTUINT32 ret = 0;
	BtSdkInformBattStatusReqStru battStatus = {0};
	battStatus.size = sizeof(BtSdkInformBattStatusReqStru);
	battStatus.id = BTSDK_AVRCP_BATTERYSTATUS_WARNING;
	ret = Btsdk_AVRCP_InformBattStatusReq(s_currAudioRmtDevHdl, &battStatus);
	if (BTSDK_OK == ret)
	{
		printf("AVRCP Battery Status Warning has send to TG.\n");
	}
}

void AVRCPStatusCritical()
{
	BTUINT32 ret = 0;
	BtSdkInformBattStatusReqStru battStatus = {0};
	battStatus.size = sizeof(BtSdkInformBattStatusReqStru);
	battStatus.id = BTSDK_AVRCP_BATTERYSTATUS_CRITICAL;
	ret = Btsdk_AVRCP_InformBattStatusReq(s_currAudioRmtDevHdl, &battStatus);
	if (BTSDK_OK == ret)
	{
		printf("AVRCP Battery Status Critical has send to TG.\n");
	}
}

void AVRCPStatusExternal()
{
	BTUINT32 ret = 0;
	BtSdkInformBattStatusReqStru battStatus = {0};
	battStatus.size = sizeof(BtSdkInformBattStatusReqStru);
	battStatus.id = BTSDK_AVRCP_BATTERYSTATUS_EXTERNAL;
	ret = Btsdk_AVRCP_InformBattStatusReq(s_currAudioRmtDevHdl, &battStatus);
	if (BTSDK_OK == ret)
	{
		printf("AVRCP Battery Status External has send to TG.\n");
	}
}

void AVRCPStatusFullCharge()
{
	BTUINT32 ret = 0;
	BtSdkInformBattStatusReqStru battStatus = {0};
	battStatus.size = sizeof(BtSdkInformBattStatusReqStru);
	battStatus.id = BTSDK_AVRCP_BATTERYSTATUS_FULL_CHARGE;
	ret = Btsdk_AVRCP_InformBattStatusReq(s_currAudioRmtDevHdl, &battStatus);
	if (BTSDK_OK == ret)
	{
		printf("AVRCP Battery Status full charge has send to TG.\n");
	}
}

void TestBattStatusCommand(char ch)
{ 
	switch (ch)
	{
	case '1':
		AVRCPStatusNormal();
		break;
	case '2':
		AVRCPStatusWarning();
		break;
	case '3':
		AVRCPStatusCritical();
		break;
	case '4':
		AVRCPStatusExternal();
		break;
	case '5':
		AVRCPStatusFullCharge();
		break;
	default:
		break;
	}
}

BOOL TestAVRCPInformBattStatusFunc()
{
	BTUINT8 ch = 0;
	printf("Please select the browsing option\n");
	TestAVRCPBattStatusMenu();
	while('r' != ch)
	{
		scanf("%c", &ch);
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
			TestBattStatusCommand(ch);
		}
		printf("\n");				
		TestAVRCPBattStatusMenu();
	}
	return 1;
}

BOOL TestAVRCPSetCurPlayerSettingValFunc()
{
	PBtSdkSetCurPlayerAppSetValReqStru pSetCurPlayerAppVal = NULL;
	BTUINT32 ret = BTSDK_OK;
	BTUINT32 size = 0;
	BTUINT8 id_len = 1;
	BTUINT8 num = 4;
	int k = 0;
	printf("AVRCP_APP_SetCurPlayerSettingValRep func.\n");
	size = sizeof(BtSdkSetCurPlayerAppSetValReqStru) + ((num-1) * sizeof(BtSdkIDPairStru));
	pSetCurPlayerAppVal = (PBtSdkSetCurPlayerAppSetValReqStru)malloc(size);
	if (NULL == pSetCurPlayerAppVal)
	{
		return;
	}
	memset(pSetCurPlayerAppVal, 0, size);
	pSetCurPlayerAppVal->size = size;
	pSetCurPlayerAppVal->num = num;
	pSetCurPlayerAppVal->id[0].attr_id = BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS;
	pSetCurPlayerAppVal->id[1].attr_id = BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS;
	pSetCurPlayerAppVal->id[2].attr_id = BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS;
	pSetCurPlayerAppVal->id[3].attr_id = BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS;
	while (k < num)
	{
		switch (pSetCurPlayerAppVal->id[k].attr_id)
		{
		case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
			{
				pSetCurPlayerAppVal->id[k].value_id = BTSDK_AVRCP_EQUALIZER_OFF;
				break;
			}
		case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
			{
				pSetCurPlayerAppVal->id[k].value_id = BTSDK_AVRCP_REPEAT_MODE_OFF;
				break;
			}
		case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
			{
				pSetCurPlayerAppVal->id[k].value_id = BTSDK_AVRCP_SHUFFLE_ALL_TRACKS_SHUFFLE;
				break;
			}
		case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
			{
				pSetCurPlayerAppVal->id[k].value_id = BTSDK_AVRCP_SCAN_OFF;
				break;
			}
		}
		k++;
	}
	ret = Btsdk_AVRCP_SetCurPlayerAppSetValReq(s_currAudioRmtDevHdl, pSetCurPlayerAppVal);
	if (BTSDK_OK == ret)
	{
		printf("CurPlayer setting has been set.\n");
	}
	free(pSetCurPlayerAppVal);
	return 1;
}

BOOL TestAVRCPSetAbsoluteVolFunc()
{
	BtSdkSetAbsoluteVolReqStru setAbsVolReq = {0};
	BTUINT32 ret = BTSDK_OK;
	BTUINT32 size = 0;
	size = sizeof(BtSdkSetAbsoluteVolReqStru);
	setAbsVolReq.size = size;
	setAbsVolReq.id = BTSDK_AVRCP_ABSOLUTE_VOLUME_MAX;	//0x7F sample 100%volume
	ret = Btsdk_AVRCP_SetAbsoluteVolReq(s_currAudioRmtDevHdl, &setAbsVolReq);
	if (BTSDK_OK == ret)
	{
		printf("100% volume has been set.\n");
	}
}

BOOL TestAVRCPBrowsing()
{
	BTUINT8 ch = 0;
	printf("Please select the browsing option\n");
	TestAVRCPBrowseShowMenu();
	while('r' != ch)
	{
		scanf("%c", &ch);
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
			TestBrowsingCommand(ch);
		}
		printf("\n");				
		TestAVRCPBrowseShowMenu();
	}
	return 1;
}

void TestDisconnectAVRCPSvc()
{
	BTSDKHANDLE enum_handle = Btsdk_StartEnumConnection();
	if (enum_handle != BTSDK_INVALID_HANDLE)
	{
		BtSdkConnectionPropertyStru conn_prop = {0};
		BTCONNHDL conn_hdl = BTSDK_INVALID_HANDLE;
		while (BTSDK_INVALID_HANDLE != (conn_hdl = Btsdk_EnumConnection(enum_handle, &conn_prop)))
		{
			if (conn_prop.service_class == s_AVRCPSvcClass)
			{
				Btsdk_Disconnect(conn_hdl);
				break;
			}
		}
		Btsdk_EndEnumConnection(enum_handle);
	}
	
}
/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is to connect specified device's service with it's service handle.
Arguments:
Return:
void 
---------------------------------------------------------------------------*/
void TestConnectAudioSvc()
{
	BTINT32 ulRet = BTSDK_FALSE;
	ulRet = Btsdk_Connect(s_currAudioSvcHdl, 0, &s_currAudioConnHdl);
	if (BTSDK_OK != ulRet)
	{
		printf("Please make sure that the expected device is powered on and connectable.\n");
		return;
	}
	if (BTSDK_INVALID_HANDLE != s_currAudioConnHdl)
	{
		GetConnectionInfo(s_currAudioRmtDevHdl);
	}
	return;
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is a callback function for AVRCP events
Arguments:
event: [in] AVRCP event
param: [in] profile specified parameter
Return:
void 
---------------------------------------------------------------------------*/
void AVRCP_Event_CbkFunc(BTUINT16 event, BTUINT8 *param)
{
	/* param is always a NULL pointer, reserved for later use. */
	switch (event)
	{
	case BTSDK_APP_EV_AVRCP_IND_CONN:
		printf("AVRCP connect successful.\n");
		break;
	case BTSDK_APP_EV_AVRCP_IND_DISCONN:
		printf("AVRCP disconnected.\n");
		break;
	default:
		printf("AVRCP_Event_CbkFunc Default case event %d  data %s \n",event,param);
		break;
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is to deal with the event generated by a user's operation.
Arguments:
op_id:      [in] operation code ID
state_flag: [in] states flag
Return:
void 
---------------------------------------------------------------------------*/
void AVRCP_PassThr_Cmd_CbkFunc(BTUINT8 op_id, BTUINT8 state_flag)
{
	//******************************************************
    /* state_flag indicates the button status (up or down). */
	printf("Inside AVRCP_PassThr_Cmd_CbkFunc function.\n");
	
	
	if (BTSDK_AVRCP_BUTTON_STATE_PRESSED != state_flag)
	{
		printf("AVRCP:DATA:BTSDK_AVRCP_BUTTON_STATE_PRESSED event\n");
		
		return;
	}
    /* op_id could be one of the following status. please add code for each case. */
	switch (op_id)
	{
	case BTSDK_AVRCP_OPID_AVC_PANEL_PLAY:
		printf("The user has pressed down 'Play' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_STOP:
		printf("The user has pressed down 'Stop' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_PAUSE:
		printf("The user has pressed down 'Pause' button on the AV device's panel.\n ");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_FAST_FORWARD:
		printf("The user has pressed down 'Fast forward' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_FORWARD:
		printf("The user has pressed down 'Forward' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_BACKWARD:
		printf("The user has pressed down 'Backward' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_UP:
		printf("The user has pressed down 'Volume up' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_VOLUME_DOWN:
		printf("The user has pressed down 'Volume down' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_RECORD:
		printf("The user has pressed down 'Record' button on the AV device's panel.\n");
		break;	
	case BTSDK_AVRCP_OPID_AVC_PANEL_POWER:
		printf("The user has pressed down 'POWER' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_MUTE:
		printf("The user has pressed down 'MUTE' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_REWIND:
		printf("The user has pressed down 'REWIND' button on the AV device's panel.\n");
		break;
	case BTSDK_AVRCP_OPID_AVC_PANEL_EJECT:
		printf("The user has pressed down 'EJECT' button on the AV device's panel.\n");
		break;
	default:
		printf("AVRCP_PassThr_Cmd_CbkFunc  Default %d.\n",op_id); 
		break;		
	}
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is to show user interface of AV test.
Arguments:
void
Return:
void 
---------------------------------------------------------------------------*/
void TestAVShowMenu(void)
{
    printf("*****************************************\n");
	printf("*    Please select the choice you want: *\n");
	printf("* <1>   Select A Remote Audio Device    *\n");
	printf("* <2>   Select Service's Handle         *\n");
	printf("* <3>   Connect Remote Audio Service    *\n");
	printf("* <4>   Disconnect                      *\n");
	printf("* <r>   Return to the upper menu        *\n");
	printf("*****************************************\n");
	printf(">>");
}

void TestAVRCPShowWorkMenu()
{
	printf("*****************************************\n");
	printf("*    Please select the choice you want: *\n");
	printf("* <1>   Select A Remote Audio Device    *\n");
	printf("* <2>   Select Service's Handle         *\n");
	printf("* <3>   Connect Remote AVRCP Service    *\n");
	printf("* <4>   Disconnect                      *\n");
	printf("* <r>   Return to the upper menu        *\n");
	printf("*****************************************\n");
	printf(">>");
}

void AVRCP_App_GetElementAttrInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetElementAttrReqStru in)
{
	PBtSdkGetElementAttrRspStru prsp = NULL;
	BTUINT32 attr_id;
	BTUINT32 size = 0;
	BTUINT32 ret = 100;
	BTUINT8 string[MAX_PATH] = {0};
	BTUINT16 len = 0;
	BTUINT8 num = (0 != in->num) ? in->num : 7;
	BTUINT8 k = 0;
	size = 2 * sizeof(BTUINT32) + sizeof(BTUINT8);
	prsp = (PBtSdkGetElementAttrRspStru)malloc(size);
	if (NULL == prsp)
	{
		return;
	}
	memset(prsp, 0, size);
	prsp->size = size;
	prsp->subpacket_type = BTSDK_AVRCP_PACKET_HEAD;
	prsp->id_num = num;
	Btsdk_AVRCP_GetElementAttrRsp(hdl, tl, prsp);
	free(prsp);
	
	while (k < num) 
	{
		memset(string, 0, MAX_PATH);
		if (in->num == 0) 
		{
			attr_id = BTSDK_AVRCP_MA_TITLEOF_MEDIA + k;
		} 
		else 
		{
			attr_id = in->attr_id[k];
		}
		switch (attr_id) 
		{
		case BTSDK_AVRCP_MA_TITLEOF_MEDIA:  // called...
			{
				strcpy(string, s_musicName);
				break;
			}
		case BTSDK_AVRCP_MA_NAMEOF_ARTIST:  // called...
			{
				strcpy(string, s_artistName);
				break;
			}
		case BTSDK_AVRCP_MA_NAMEOF_ALBUM:  // called...
			{
				strcpy(string, s_albumName);
				break;
			}
		case BTSDK_AVRCP_MA_PLAYING_TIME:  // called...
			{
				strcpy(string, "169000");
				break;
			}
		default:
			printf("AVRCP_App_GetElementAttrInd %0x\n",attr_id);
			break;
		}
		len = strlen(string);
		size = sizeof(BtSdkGetElementAttrRspStru) + (len + 1) * sizeof(BTUINT8);
		prsp = (PBtSdkGetElementAttrRspStru)malloc(size);
		if (NULL != prsp)
		{
			memset(prsp, 0, size);
			prsp->size = size;
			prsp->subpacket_type = BTSDK_AVRCP_SUBPACKET;
			prsp->id_value.attr_id = attr_id;
			prsp->id_value.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;
			prsp->id_value.len = (BTUINT8)len + 1;
			memcpy(prsp->id_value.value, string, len);
			ret = Btsdk_AVRCP_GetElementAttrRsp(hdl, tl, prsp);
			free(prsp);
		}
		k++;
	}
}

void AVRCP_App_GetCapabilitiesInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetCapabilitiesReqStru in)
{
	BTUINT32 len_in = 0;
	BTUINT32 id_len = 0;
	BTUINT8 count = 4;/* sample */
	
	printf("Inside AVRCP_App_GetCapabilitiesInd function.\n");
	
	switch (in->id) 
	{
	case BTSDK_AVRCP_CAPABILITYID_COMPANY_ID:
		{
			count = 1;
			id_len = 3;
			printf("Get Capabilities Indication of CAPABILITY_ID COMPANY_ID.\n");
			break;
		}
	case BTSDK_AVRCP_CAPABILITYID_EVENTS_SUPPORTED:
		{
			count = 9;
			//count = 5;
			id_len = 1;
			printf("Get Capabilities Indication of CAPABILITY_ID EVENTS_SUPPORTED.\n");
			break;
		}
	default:
		printf("AVRCP_App_GetCapabilitiesInd function Default %d.\n",in->id);
		/* error */
		break;
	}
	if (id_len > 0) 
	{
		PBtSdkGetCapabilitiesRspStru prsp = NULL;
		
		len_in = sizeof(BtSdkGetCapabilitiesRspStru) + count * id_len;
		prsp = (PBtSdkGetCapabilitiesRspStru)malloc(len_in);
		memset(prsp, 0, len_in);
		prsp->size = len_in;
		prsp->capability_id = in->id;
		prsp->count = count;
		
		switch (in->id) 
		{
		case BTSDK_AVRCP_CAPABILITYID_COMPANY_ID:
			{
				prsp->capability[0] = 0x00;
				prsp->capability[1] = 0x19;
				prsp->capability[2] = 0x58;	
				printf("Get Capabilities Indication of CAPABILITY_ID COMPANY_ID.\n");
				break;
			}
		case BTSDK_AVRCP_CAPABILITYID_EVENTS_SUPPORTED:
			{
				prsp->capability[0] = BTSDK_AVRCP_EVENT_PLAYBACK_STATUS_CHANGED;
				prsp->capability[1] = BTSDK_AVRCP_EVENT_TRACK_CHANGED;
				prsp->capability[2] = BTSDK_AVRCP_EVENT_PLAYBACK_POS_CHANGED;
				prsp->capability[3] = BTSDK_AVRCP_EVENT_PLAYER_APPLICATION_SETTING_CHANGED;
				prsp->capability[4] = BTSDK_AVRCP_EVENT_TRACK_REACHED_END;
				prsp->capability[5] = BTSDK_AVRCP_EVENT_TRACK_REACHED_START;
				prsp->capability[6] = BTSDK_AVRCP_EVENT_BATT_STATUS_CHANGED;
				prsp->capability[7] = BTSDK_AVRCP_EVENT_SYSTEM_STATUS_CHANGED;
				prsp->capability[8] = BTSDK_AVRCP_EVENT_VOLUME_CHANGED;
				//prsp->capability[4] = BTSDK_APP_EV_AVRCP_GET_PLAY_STATUS_IND;
				
				printf("Get Capabilities Indication of CAPABILITY_ID EVENTS_SUPPORTED.\n");
				break;
			}
		default:/* error */
			printf("AVRCP_App_GetCapabilitiesInd 2 %0x\n",in->id);
			break;
		}	
		Btsdk_AVRCP_GetCapabilitiesRsp(hdl, tl, prsp);
		
		if (NULL != prsp) 
		{
			free(prsp);
		}
	} 
	else 
	{
		AVRCP_App_GeneralRejectRsp(hdl, tl, BTSDK_APP_EV_AVRCP_GET_CAPABILITIES_IND, BTSDK_AVRCP_ERROR_INVALID_PARAMETER);
	}
}

void AVRCP_App_ListPlayerAppSetAttrInd(BTDEVHDL hdl, BTUINT8 tl)
{
	BTUINT32 len_in = 0;
	BTUINT32 id_len = 1;
	BTUINT8 num = 4;
	PBtSdkListPlayerAppSetAttrRspStru prsp = NULL;
	
	printf("Inside AVRCP_App_ListPlayerAppSetAttrInd function.\n");
	
	len_in = sizeof(BtSdkListPlayerAppSetAttrRspStru) + (num -1)* id_len;
	prsp = (PBtSdkListPlayerAppSetAttrRspStru)malloc(len_in);
	if (prsp)
	{
		memset(prsp, 0, len_in);
		prsp->size = len_in;
		prsp->num= num;		
		prsp->id[0] = BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS;
		prsp->id[1] = BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS;
		prsp->id[2]	= BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS;
		prsp->id[3] = BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS;
		Btsdk_AVRCP_ListPlayerAppSetAttrRsp(hdl, tl, prsp);		
		printf("total support %d attrs.\n",prsp->num);
		free(prsp);
	}
}


void AVRCP_App_ListPlayerAppSetValInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkListPlayerAppSetValReqStru in)
{
	BTUINT32 len_in = 0;
	BTUINT32 id_len = 0;
	BTUINT8 num = 4;/* sample */
	
	printf("Inside AVRCP_App_ListPlayerAppSetValInd.\n");
	
	switch (in->id) 
	{
	case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
		{
			num = 2;
			id_len = 1;
			printf("Get Indication of EQUALIZER_ONOFF_STATUS----1--num = %d.\n",num);
			break;
		}
	case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
		{
			num = 4;
			id_len = 1;
			printf("Get Indication of REPEAT_MODE_STATUS----1--num = %d.\n",num);
			break;
		}
	case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
		{
			num = 3;
			id_len = 1;
			printf("Get Indication of SHUFFLE_ONOFF_STATUS----1--num = %d.\n",num);
			break;
		}
	case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
		{
			num = 3;
			id_len = 1;
			printf("Get Indication of SCAN_ONOFF_STATUS----1--num = %d.\n",num);
			break;
		}
	default:
		printf("AVRCP_App_ListPlayerAppSetValInd function Default %d.\n",in->id);
		/* error */
		break;
	}
	if (id_len > 0) 
	{
		PBtSdkListPlayerAppSetValRspStru prsp = NULL;
		
		len_in = sizeof(BtSdkListPlayerAppSetValRspStru) + num * id_len;
		prsp = (PBtSdkListPlayerAppSetValRspStru)malloc(len_in);
		memset(prsp, 0, len_in);
		prsp->size = len_in;
		prsp->num = num;
		
		switch (in->id) 
		{
		case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
			{
				prsp->id[0] = BTSDK_AVRCP_EQUALIZER_OFF;
				prsp->id[1] = BTSDK_AVRCP_EQUALIZER_ON;	
				printf("Get Indication of EQUALIZER_ONOFF_STATUS----2--num = %d.\n",prsp->num);
				break;
			}
		case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
			{
				prsp->id[0] = BTSDK_AVRCP_REPEAT_MODE_OFF;
				prsp->id[1] = BTSDK_AVRCP_REPEAT_MODE_SINGLE_TRACK_REPEAT;
				prsp->id[2] = BTSDK_AVRCP_REPEAT_MODE_ALL_TRACK_REPEAT;
				prsp->id[3] = BTSDK_AVRCP_REPEAT_MODE_GROUP_REPEAT;
				//prsp->capability[4] = BTSDK_APP_EV_AVRCP_GET_PLAY_STATUS_IND;
				
				printf("Get Indication of REPEAT_MODE_STATUS----2--num = %d.\n",prsp->num);
				break;
			}
		case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
			{
				prsp->id[0] = BTSDK_AVRCP_SHUFFLE_OFF;
				prsp->id[1] = BTSDK_AVRCP_SHUFFLE_ALL_TRACKS_SHUFFLE;	
				prsp->id[2] = BTSDK_AVRCP_SHUFFLE_GROUP_SHUFFLE;
				printf("Get Indication of SHUFFLE_ONOFF_STATUS----2--num = %d.\n",prsp->num);
				break;
			}
		case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
			{
				prsp->id[0] = BTSDK_AVRCP_SCAN_OFF;
				prsp->id[1] = BTSDK_AVRCP_SCAN_ALL_TRACKS_SCAN;	
				prsp->id[2] = BTSDK_AVRCP_SCAN_GROUP_SCAN;
				printf("Get Indication of SCAN_ONOFF_STATUS----2--num = %d.\n",prsp->num);
				break;
			}
		default:/* error */
			printf("AVRCP_App_ListPlayerAppSetValInd 2 %0x\n",in->id);
			break;
		}	
		Btsdk_AVRCP_ListPlayerAppSetValRsp(hdl, tl, prsp);
		if (NULL != prsp) 
		{
			free(prsp);
		}
	} 
	else 
	{
		AVRCP_App_GeneralRejectRsp(hdl, tl, BTSDK_APP_EV_AVRCP_LIST_PLAYER_SETTING_VALUES_IND, BTSDK_AVRCP_ERROR_INVALID_PARAMETER);
	}
}

void AVRCP_App_GetPlayerAppSetAttrTxtInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetPlayerAppSetAttrTxtReqStru in)
{
	PBtSdkGetPlayerAppSettingAttrTxtRspStru prsp = NULL;
	BTUINT32 size = 0;
	BTUINT8 k = 0;
	
	printf("Inside AVRCP_App_GetPlayerAppSetAttrTxtInd.\n");
	
	size = 2 * sizeof(BTUINT32) + sizeof(BTUINT8);
	prsp = (PBtSdkGetPlayerAppSettingAttrTxtRspStru)malloc(size);
	if (NULL == prsp)
	{
		return;
	}
	memset(prsp, 0, size);
	prsp->size = size;
	prsp->subpacket_type = BTSDK_AVRCP_PACKET_BROWSABLE_ITEM;
	prsp->id_num = in->num;
	Btsdk_AVRCP_GetPlayerAppSetAttrTxtRsp(hdl, tl, prsp);
	free(prsp);
	prsp = NULL;
	while (k < in->num) {
		BTUINT8 str[MAX_PATH] = {0};
		BTUINT8 id = in->id[k];
		BTUINT16 len = 0;
		
		switch (id) 
		{
		case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
			{
				strcpy(str, "Equalizer ON/OFF Status");
				break;
			}
		case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
			{
				strcpy(str, "Repeat Mode Status");
				break;
			}
		case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
			{
				strcpy(str, "Shuffle Status ON/OFF Status");
				break;
			}
		case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
			{
				strcpy(str, "Scan Status ON/OFF Status");
				break;
			}
		default:
			break;
		}
		len = strlen(str);
		size = sizeof(BtSdkGetPlayerAppSettingAttrTxtRspStru) + (len - 1) * sizeof(BTUINT8);
		prsp = (PBtSdkGetPlayerAppSettingAttrTxtRspStru)malloc(size);
		if (prsp != NULL)
		{
			memset(prsp, 0, size);
			prsp->size = size;
			prsp->subpacket_type = BTSDK_AVRCP_PACKET_MEDIA_ATTR;//?
			prsp->id_string.id = in->id[k];
			prsp->id_string.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;
			prsp->id_string.len = len;
			strcpy(prsp->id_string.string, str);
			Btsdk_AVRCP_GetPlayerAppSetAttrTxtRsp(hdl, tl, prsp); /* Sent attribute */			
			free(prsp);
		}
		k++;
	}
}

void AVRCP_App_GetPlayerAppSetValTxtInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetPlayerAppSetValTxtReqStru in)
{
	PBtSdkGetPlayerAppSettingValTxtRspStru prsp = NULL;
	BTUINT32 size = 0;
	BTUINT8 k = 0;
	printf("Inside AVRCP_App_GetPlayerAppSetValTxtInd\n");
	size = 2 * sizeof(BTUINT32) + sizeof(BTUINT8);
	prsp = (PBtSdkGetPlayerAppSettingValTxtRspStru)malloc(size);
	prsp->size = size;
	prsp->subpacket_type = BTSDK_AVRCP_PACKET_BROWSABLE_ITEM;
	prsp->id_num = in->num;
	Btsdk_AVRCP_GetPlayerAppSetValTxtRsp(hdl, tl, prsp); 	/* Specifies the number of values */
	free(prsp);

	while (k < in->num) {
		BTUINT8 *str = NULL;
		BTUINT8 id = in->value_id[k];
		BTUINT16 len = 0;
		switch (in->attr_id)
		{
		case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
			{			
				switch (id)
				{
				case BTSDK_AVRCP_EQUALIZER_OFF:
					str = "Equalizer Off";
					break;
				case BTSDK_AVRCP_EQUALIZER_ON:
					str = "Equalizer On";
					break;
				default:
					break;
				}
				break;
			}
		case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
			switch (id)
			{
			case BTSDK_AVRCP_REPEAT_MODE_OFF:
				str = "Repeat Mode Off";
				break;
			case BTSDK_AVRCP_REPEAT_MODE_SINGLE_TRACK_REPEAT:
				str = "Repeat Mode Single Track Repeat";
				break;
			case BTSDK_AVRCP_REPEAT_MODE_ALL_TRACK_REPEAT:
				str = "Repeat Mode All Track Repeat";
				break;
			case BTSDK_AVRCP_REPEAT_MODE_GROUP_REPEAT:
				str = "Repeat Mode Group Repeat";
				break;
			default:
				break;
			}
			break;
		case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
			switch (id)
			{
			case BTSDK_AVRCP_SHUFFLE_OFF:
				str = "Shuffle Off";
				break;
			case BTSDK_AVRCP_SHUFFLE_ALL_TRACKS_SHUFFLE:
				str = "Shuffle All Tracks Shuffle";
				break;
			case BTSDK_AVRCP_SHUFFLE_GROUP_SHUFFLE:
				str = "Shuffle Group Shuffle";
				break;
			default:
				break;
			}
			break;
		case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
			switch (id)
			{
			case BTSDK_AVRCP_SCAN_OFF:
				str = "Scan Off";
				break;
			case BTSDK_AVRCP_SCAN_ALL_TRACKS_SCAN:
				str = "Scan All Tracks Scan";
				break;
			case BTSDK_AVRCP_SCAN_GROUP_SCAN:
				str = "Scan Group Scan";
				break;
			default:
				break;
			}
			break;
		default:
				break;
		}
		len = strlen(str);
		size = sizeof(BtSdkGetPlayerAppSettingValTxtRspStru) + (len - 1) * sizeof(BTUINT8);
		prsp = (PBtSdkGetPlayerAppSettingValTxtRspStru)malloc(size);
		prsp->size = size;
		prsp->subpacket_type = BTSDK_AVRCP_PACKET_MEDIA_ATTR;
		prsp->id_string.id = id;
		prsp->id_string.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;
		prsp->id_string.len = len;
		strcpy(prsp->id_string.string,  str);
		Btsdk_AVRCP_GetPlayerAppSetValTxtRsp(hdl, tl, prsp); /* Sent value */
		free(prsp);
		k++;
	}
}

void AVRCP_App_SetAddressedPlayerInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkSetAddresedPlayerReqStru in)
{	
	BTUINT32 len_in;
	PBtSdkSetAddresedPlayerRspStru prsp = NULL;
	printf("Inside AVRCP_App_SetAddressedPlayerInd function.\n");
	
	len_in = sizeof(BtSdkSetAddresedPlayerRspStru);
	prsp=(PBtSdkSetAddresedPlayerRspStru)malloc(len_in);
	if (prsp != NULL)
	{
		memset(prsp, 0, len_in);
		prsp->size=len_in;
		prsp->id=in->id;
		printf("----------%x---------\n",prsp->id);
		Btsdk_AVRCP_SetAddressedPlayerRsp(hdl, tl, prsp);
		printf("----------%x---------\n",prsp->id);
		free(prsp);
	}
}

void AVRCP_App_InformBattStatusInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkInformBattStatusReqStru in)
{
	printf("Inside AVRCP_App_InformBattStatusInd function.\n");
	switch (in->id) 
	{
	case BTSDK_AVRCP_BATTERYSTATUS_NORMAL:
		{
			printf("Battery operation is in normal state.\n");
			break;
		}
	case BTSDK_AVRCP_BATTERYSTATUS_WARNING:
		{
			printf("Unable to operate soon. Specified when battery going down.\n");
			break;
		}
	case BTSDK_AVRCP_BATTERYSTATUS_CRITICAL:
		{
			printf("Can not operate any more. Specified when battery going down.\n");
			break;
		}
	case BTSDK_AVRCP_BATTERYSTATUS_EXTERNAL:
		{
			printf("Connecting to external power supply.\n");
			break;
		}
	case BTSDK_AVRCP_BATTERYSTATUS_FULL_CHARGE:
		{
			printf("When the device is completely charged.\n");
			break;
		}
	default:
		printf("AVRCP_App_InformBattStatusInd function Default %d.\n",in->id);
		/* error */
		break;
	}
	Btsdk_AVRCP_InformBattStatusRsp(hdl, tl);
}

void AVRCP_App_SetAbsoluteVolRsp(BTDEVHDL hdl, BTUINT8 tl, PBtSdkSetAbsoluteVolReqStru in)
{
	PBtSdkSetAbsoluteVolRspStru prsp = NULL;
	BTUINT32 size = in->size;
	BTUINT8 id = in->id;

	printf("Inside AVRCP_App_SetAbsoluteVolRsp function.\n");

	prsp = (PBtSdkSetAbsoluteVolRspStru)malloc(sizeof(BtSdkSetAbsoluteVolRspStru));
	prsp->size = size;
	prsp->id = id;
	
	Btsdk_AVRCP_SetAbsoluteVolRsp(hdl, tl, prsp);
	printf("TG has been set absolute volume: %d\n",prsp->id);
	free(prsp);
}

void *AVRCP_App_NotifSet(BTUINT8 event_id, BTUINT8 rsp_code)
{
	void *prsp = NULL;
	BTUINT16 size = 0;
	
	switch (event_id) 
	{
	case BTSDK_AVRCP_EVENT_PLAYBACK_STATUS_CHANGED:
		{
			PBtSdkPlayStatusChangedStru rsp_notif;
			size = sizeof(BtSdkPlayStatusChangedStru);
			rsp_notif = (PBtSdkPlayStatusChangedStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			rsp_notif->id = BTSDK_AVRCP_PLAYSTATUS_STOPPED;/* Indicates the current status of playback */
			prsp = rsp_notif;
			printf("Notification of PLAYBACK_STATUS_CHANGED Event.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_TRACK_CHANGED:									/* 0x02 *///called....
		{
			PBtSdkTrackChangedStru rsp_notif;
			size = sizeof(BtSdkTrackChangedStru);
			rsp_notif = (PBtSdkTrackChangedStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			memset(rsp_notif->identifier, 0xFF, 8);
			/* If a track is selected, then return 0x0 in the response. If no track is currently selected, 
			then return 0xFFFFFFFFFFFFFFFF in the INTERIM response. */
			prsp = rsp_notif;
			printf("Notification of TRACK_CHANGED Event.\n");
			//		//		if(process_start==0)
			//		//		{
			//			//ShellExecute(NULL,NULL,"WmpML.exe",NULL,"C:\\Users\\Public\\Documents\\GM NGI Stability Test Suite",SW_HIDE);
			//		//			process_start=1;
			//		//		}
			break;
		}
	case BTSDK_AVRCP_EVENT_TRACK_REACHED_END:								/* 0x03 */
		{
			PBtSdkTrackReachEndStru rsp_notif;
			size = sizeof(BtSdkTrackReachEndStru);
			rsp_notif = (PBtSdkTrackReachEndStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			prsp = rsp_notif;
			printf("Notification of track has reached the end.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_TRACK_REACHED_START:	
		{							/* 0x04 */
			PBtSdkTrackReachStartStru rsp_notif;
			size = sizeof(BtSdkTrackReachStartStru);
			rsp_notif = (PBtSdkTrackReachStartStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			prsp = rsp_notif;
			printf("Notification of track has reached the start.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_NOW_PLAYING_CONTENT_CHANGED:					/* 0x09 */
		{
			printf("playing content has changed.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_AVAILABLE_PLAYERS_CHANGED:						/* 0x0A */
		{
			PBtSdkNotifNullStru rsp_notif;
			size = sizeof(BtSdkNotifNullStru);
			rsp_notif = (PBtSdkNotifNullStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			prsp = rsp_notif;
			printf("Notification of AVAILABLE_PLAYERS_CHANGED Event.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_PLAYBACK_POS_CHANGED:								/* 0x05 */ // called...
		{
			PBtSdkPlayPosChangedStru rsp_notif;
			size = sizeof(BtSdkPlayPosChangedStru);
			rsp_notif = (PBtSdkPlayPosChangedStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			rsp_notif->pos = 0xFFFFFFFF; /* If no track currently selected, then return 0xFFFFFFFF in the INTERIM response. */
			prsp = rsp_notif;
			printf("Notification of PLAYBACK_POS_CHANGED Event.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_BATT_STATUS_CHANGED:								/* 0x06 */
		{
			PBtSdkBattStatusChangedStru rsp_notif;
			size = sizeof(BtSdkBattStatusChangedStru);
			rsp_notif = (PBtSdkBattStatusChangedStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			rsp_notif->id = BTSDK_AVRCP_BATTERYSTATUS_NORMAL;/* Battery status */
			prsp = rsp_notif;
			printf("Notification of BATT_STATUS_CHANGED Event.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_SYSTEM_STATUS_CHANGED:							/* 0x07 */
		{
			PBtSdkSysStatusChangedStru rsp_notif;
			size = sizeof(BtSdkSysStatusChangedStru);
			rsp_notif = (PBtSdkSysStatusChangedStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			rsp_notif->id = BTSDK_AVRCP_SYSTEM_POWER_ON;/* Indicates the current System status. */
			prsp = rsp_notif;
			printf("Notification of SYSTEM_STATUS_CHANGED Event.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_PLAYER_APPLICATION_SETTING_CHANGED:				/* 0x08 */
		{
			BTUINT8 num = 4;/* sample */
			BTUINT8 k = 0;
			BTUINT8 attr_id = BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS;
			PBtSdkPlayerAppSetChangedStru rsp_notif;
			
			size = sizeof(BtSdkPlayerAppSetChangedStru) + ((num-1)*sizeof(BtSdkIDPairStru));
			rsp_notif = (PBtSdkPlayerAppSetChangedStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			rsp_notif->num = num;
			while (k < num) 
			{
				rsp_notif->id[k].attr_id = attr_id;
				switch (attr_id++) 
				{
				case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
					rsp_notif->id[k].value_id = BTSDK_AVRCP_EQUALIZER_OFF;/* Sample */
					break;
				case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
					rsp_notif->id[k].value_id = BTSDK_AVRCP_REPEAT_MODE_OFF;/* Sample */
					break;
				case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
					rsp_notif->id[k].value_id = BTSDK_AVRCP_SHUFFLE_OFF;/* Sample */
					break;
				case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
					rsp_notif->id[k].value_id = BTSDK_AVRCP_SCAN_OFF;/* Sample */
					break;							
				default :
					printf("BTSDK_AVRCP_EVENT_PLAYER_APPLICATION_SETTING_CHANGED default %0x\n",attr_id);
				}
				k++;
			}
			prsp = rsp_notif;

			printf("Notification of PLAYER_APPLICATION_SETTING_CHANGED Event.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_ADDRESSED_PLAYER_CHANGED:						/* 0x0B */
		{
			printf("Notification of ADDRESSED_PLAYER_CHANGED Event.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_UIDS_CHANGED:										/* 0x0C */
		{
			PBtSdkUIDSChangedStru rsp_notif;
			size = sizeof(BtSdkUIDSChangedStru);
			rsp_notif = (PBtSdkUIDSChangedStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			//rsp_notif->uid_counter = app_avrcp.local_element_uidcounter;/* The UID Counter of the currently browsed player as defined in 6.10.3 */
			prsp = rsp_notif;
			printf("Notification of UIDS_CHANGED Event.\n");
			break;
		}
	case BTSDK_AVRCP_EVENT_VOLUME_CHANGED:									/* 0x0D */
		{
			PBtSdkVolChangedStru rsp_notif;
			size = sizeof(BtSdkVolChangedStru);
			rsp_notif = (PBtSdkVolChangedStru)malloc(size);
			memset(rsp_notif, 0, size);
			rsp_notif->size = size;
			rsp_notif->rsp_code = rsp_code;
			rsp_notif->id = 0x00; //0音量
			//rsp_notif->id = app_avrcp.current_volume;
			prsp = rsp_notif;
			printf("Notification of VOLUME_CHANGED Event.\n");
			break;
		}
	default:/* unknown event */
		printf("AVRCP_App_NotifSet Event Default  Id %d.\n",event_id);
		break;
	}
	
	return prsp;
}

void AVRCP_App_RegNotifInd(void *param)
{
	void *prsp = NULL;
	BTUINT8	event_id = 0;
	
	PBtSdkTrackChangedStru rsp_notif = NULL;
	PBtSdkPlayPosChangedStru rsp_notif2 = NULL;
	BTUINT16 size = 0;
	BTUINT32 ret = 0;
	
	if (param == NULL)
	{
		return;
	}
	event_id = ((PBtSdkRegisterNotifReqStru)param)->event_id;
	
	prsp = AVRCP_App_NotifSet(event_id, BTSDK_AVRCP_RSP_INTERIM);
	if (prsp) 
	{
		switch (event_id) 
		{
		case BTSDK_AVRCP_EVENT_PLAYBACK_STATUS_CHANGED:
			Btsdk_AVRCP_EventPlayStatusChanged((PBtSdkPlayStatusChangedStru)prsp);
			printf("Register Notification of PLAYBACK_STATUS_CHANGED event.\n");
			break;
		case BTSDK_AVRCP_EVENT_TRACK_CHANGED:  //  called...
			Btsdk_AVRCP_EventTrackChanged((PBtSdkTrackChangedStru)prsp);
			printf("Register Notification of TRACK_CHANGED event.\n");
			break;
		case BTSDK_AVRCP_EVENT_TRACK_REACHED_END:
			Btsdk_AVRCP_EventTrackReachEnd((PBtSdkTrackReachEndStru)prsp);
			printf("Register Notification of TRACK_REACHED_END event.\n");
			break;
		case BTSDK_AVRCP_EVENT_TRACK_REACHED_START:
			Btsdk_AVRCP_EventTrackReachStart((PBtSdkTrackReachStartStru)prsp);
			printf("Register Notification of TRACK_REACHED_START event.\n");
			break;
		case BTSDK_AVRCP_EVENT_PLAYBACK_POS_CHANGED:  //  called...
			Btsdk_AVRCP_EventPlayPosChanged((PBtSdkPlayPosChangedStru)prsp);
			printf("Register Notification of PLAYBACK_POS_CHANGED event.\n");
			
			break;
		case BTSDK_AVRCP_EVENT_BATT_STATUS_CHANGED:
			Btsdk_AVRCP_EventBattStatusChanged((PBtSdkBattStatusChangedStru)prsp);
			printf("Register Notification of BATT_STATUS_CHANGED event.\n");
			break;
		case BTSDK_AVRCP_EVENT_SYSTEM_STATUS_CHANGED:
			Btsdk_AVRCP_EventSysStatusChanged((PBtSdkSysStatusChangedStru)prsp);
			printf("Register Notification of SYSTEM_STATUS_CHANGED event.\n");
			break;
		case BTSDK_AVRCP_EVENT_PLAYER_APPLICATION_SETTING_CHANGED:
			Btsdk_AVRCP_EventPlayerAppSetChanged((PBtSdkPlayerAppSetChangedStru)prsp);
			printf("Register Notification of PLAYER_APPLICATION_SETTING_CHANGED event.\n");
			break;
		case BTSDK_AVRCP_EVENT_NOW_PLAYING_CONTENT_CHANGED:
			Btsdk_AVRCP_EventNowPlayingContentChanged(prsp);
			printf("Register Notification of NOW_PLAYING_CONTENT_CHANGED event.\n");
			break;
		case BTSDK_AVRCP_EVENT_AVAILABLE_PLAYERS_CHANGED:
			Btsdk_AVRCP_EventAvailablePlayerChanged(prsp);
			printf("Register Notification of AVAILABLE_PLAYERS_CHANGED event.\n");
			break;
		case BTSDK_AVRCP_EVENT_ADDRESSED_PLAYER_CHANGED:
			Btsdk_AVRCP_EventAddrPlayerChanged(prsp);
			printf("Register Notification of ADDRESSED_PLAYER_CHANGED event.\n");
			break;
		case BTSDK_AVRCP_EVENT_UIDS_CHANGED:
			Btsdk_AVRCP_EventUIDSChanged(prsp);
			printf("Register Notification of UIDS_CHANGED event.\n");
			break;
		case BTSDK_AVRCP_EVENT_VOLUME_CHANGED:
			Btsdk_AVRCP_EventVolChanged((PBtSdkVolChangedStru)prsp);
			printf("Register Notification of VOLUME_CHANGED event.\n");
			break;
		default:
			printf("RegNotifInd event id Default  %d.\n",event_id);
			break;
		}
		free(prsp);
	}
}

BTBOOL App_AVRCP_TG_Cmd_Cbk(BTDEVHDL hdl, BTUINT8 tl, BTUINT16 cmd_type, BTUINT8 *param)
{
	PBtSdkTrackChangedStru rsp_notif = NULL;
	PBtSdkPlayPosChangedStru rsp_notif2 = NULL;
	BTUINT16 size = 0;
	BTUINT32 ret = 0;
	
	BTBOOL bRet = BTSDK_TRUE;
	switch (cmd_type)
	{
	case BTSDK_APP_EV_AVRCP_GET_CAPABILITIES_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_GetCapabilitiesInd(hdl, tl, (PBtSdkGetCapabilitiesReqStru)param);
			printf("TG callback event of GET_CAPABILITIES_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_LIST_PLAYER_SETTING_ATTR_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_ListPlayerAppSetAttrInd(hdl, tl);
			printf("TG callback event of LIST_PLAYER_SETTING_ATTR_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_LIST_PLAYER_SETTING_VALUES_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_ListPlayerAppSetValInd(hdl, tl,(PBtSdkListPlayerAppSetValReqStru)param);
			printf("TG callback event of LIST_PLAYER_SETTING_VALUES_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_PLAYER_SETTING_ATTR_TEXT_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_GetPlayerAppSetAttrTxtInd(hdl, tl,(PBtSdkGetPlayerAppSetAttrTxtReqStru)param);
			printf("TG callback event of GET_PLAYER_SETTING_ATTR_TEXT_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_PLAYER_SETTING_VALUE_TEXT_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_GetPlayerAppSetValTxtInd(hdl, tl, (PBtSdkGetPlayerAppSetValTxtReqStru)param);
			printf("TG callback event of GET_PLAYER_SETTING_VALUE_TEXT_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_CURRENTPLAYER_SETTING_VALUE_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_GetCurPlayerAppSetValInd(hdl, tl, (PBtSdkGetCurPlayerAppSetValReqStru)param);
			printf("TG callback event of GET_CURRENTPLAYER_SETTING_VALUE_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_SET_CURRENTPLAYER_SETTING_VALUE_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_SetCurPlayerAppSetValInd(hdl, tl, (PBtSdkSetCurPlayerAppSetValReqStru)param);
			printf("TG callback event of SET_CURRENTPLAYER_SETTING_VALUE_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_INFORM_CHARACTERSET_IND:
		{
			bRet = BTSDK_FALSE;
			Btsdk_AVRCP_InformCharSetRsp(hdl, tl);
			printf("TG callback event of INFORM_CHARACTERSET_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_INFORM_BATTERYSTATUS_OF_CT_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_InformBattStatusInd(hdl, tl, (PBtSdkInformBattStatusReqStru)param);
			printf("TG callback event of INFORM_BATTERYSTATUS_OF_CT_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_SET_ABSOLUTE_VOLUME_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_SetAbsoluteVolRsp(hdl, tl, (PBtSdkSetAbsoluteVolReqStru)param);
			printf("TG callback event of SET_ABSOLUTE_VOLUME_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_SET_ADDRESSED_PLAYER_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_SetAddressedPlayerInd(hdl, tl, (PBtSdkSetAddresedPlayerReqStru)param);
			printf("TG callback event of SET_ADDRESSED_PLAYER_IND.\n"); 
			break;	
		}
	case BTSDK_APP_EV_AVRCP_SET_BROWSED_PLAYER_IND:
		{
			BtsdkSetBrowsedPlayerRspHeadStru playerhead={0};
			BtsdkSetBrowsedPlayerRspItemStru folderitem={0};
			
			PBtSdkSetBrowsedPlayerRspStru browsedplayer=NULL;
			int size=0;
			
			bRet = BTSDK_FALSE;
			size = 2 * sizeof(BTUINT32) + sizeof(BtsdkSetBrowsedPlayerRspHeadStru);
			browsedplayer=(PBtSdkSetBrowsedPlayerRspStru)malloc(size);
			
			browsedplayer->size = size;
			browsedplayer->subpacket_type = BTSDK_AVRCP_PACKET_HEAD;
			browsedplayer->packet_head.status = BTSDK_AVRCP_ERROR_SUCCESSFUL;
			browsedplayer->packet_head.uid_counter = 0x1357;
			browsedplayer->packet_head.items_num = 0x0005;
			browsedplayer->packet_head.characterset_id = 3;
			browsedplayer->packet_head.folder_depth = 3;
			
			Btsdk_AVRCP_SetBrowsedPlayerRsp(hdl, tl, browsedplayer);
			printf("TG callback event of SET_BROWSED_PLAYER_IND.\n"); 
			free(browsedplayer);
		}
		break;
	case BTSDK_APP_EV_AVRCP_GET_FOLDER_ITEMS_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_Exp_GetFolderItemInd(hdl,tl,(PBtSdkGetFolderItemReqStru)param);
			//Btsdk_AVRCP_GetFolderItemsRsp(hdl, tl,(PBtSdkGetFolderItemRspStru)param);
			printf("TG callback event of GET_FOLDER_ITEMS_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_CHANGE_PATH_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_Exp_GetFolderItemInd(hdl,tl,(PBtSdkGetFolderItemReqStru)param);
			//Btsdk_AVRCP_ChangePathRsp(hdl, tl,(PBtSdkChangePathRspStru)param);
			printf("TG callback event of CHANGE_PATH_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_ITEM_ATTRIBUTES_IND:
		{
			bRet = BTSDK_FALSE;
			Btsdk_AVRCP_GetItemAttrRsp(hdl, tl,(PBtSdkGetItemAttrRspStru)param);
			
			printf("TG callback event of GET_ITEM_ATTRIBUTES_IND.\n"); 
		}
		break;
	case BTSDK_APP_EV_AVRCP_PLAY_ITEM_IND:
		{
			PBtSdkPlayItemRspStru playitem;
			bRet = BTSDK_FALSE;
			playitem=(PBtSdkPlayItemRspStru)malloc(sizeof(BtSdkPlayItemRspStru));
			playitem->size=sizeof(BtSdkPlayItemRspStru);
			playitem->id=BTSDK_AVRCP_ERROR_SUCCESSFUL;
			
			Btsdk_AVRCP_PlayItemRsp(hdl, tl,(PBtSdkPlayItemRspStru)playitem);
			printf("TG callback event of PLAY_ITEM_IND.\n"); 
			free(playitem);
		}
		break;
	case BTSDK_APP_EV_AVRCP_SEARCH_IND:
		{
			bRet = BTSDK_FALSE;
			Btsdk_AVRCP_SearchRsp(hdl, tl,(PBtSdkSearchRspStru)param);
			printf("TG callback event of SEARCH_IND.\n"); 
		}
		break;
	case BTSDK_APP_EV_AVRCP_ADDTO_NOWPLAYING_IND:
		{
			bRet = BTSDK_FALSE;
			AVRCP_APP_AddToNowPlayingInd();
			Btsdk_AVRCP_AddToNowPlayingRsp(hdl, tl,(PBtSdkAddToNowPlayingRspStru)param);
			printf("TG callback event of ADDTO_NOWPLAYING_IND.\n"); 
		}
		break;
	case BTSDK_APP_EV_AVRCP_REGISTER_NOTIFICATION_IND:  // called...
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_RegNotifInd(param);
			printf("TG callback event of REGISTER_NOTIFICATION_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_ELEMENT_ATTR_IND:  // called...
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_GetElementAttrInd(hdl, tl, (PBtSdkGetElementAttrReqStru)param);
			printf("TG callback event of GET_ELEMENT_ATTR_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_PLAY_STATUS_IND:  // called...
		{
			bRet = BTSDK_FALSE;
			AVRCP_App_GetPlayStatusInd(hdl, tl);
			printf("TG callback event of GET_PLAY_STATUS_IND.\n"); 
			break;
		}
	case BTSDK_APP_EV_AVRCP_PASSTHROUGH_IND:  // called...
		{
			//should write a response to CT.
			printf("TG callback event of PASSTHROUGH_IND.\n");  
			break;
		}
	default:
		{
			printf("App_AVRCP_TG_Cmd_Cbk Default cmd type id %d.\n",cmd_type); 
			break;
		}
	}
	return bRet;
}
void AVRCP_App_GetCapabilitiesCfm(PBtSdkGetCapabilitiesRspStru pStruGetCapRsp)
{
	printf("AVRCP_App_GetCapabilitiesCfm function.\n");
	if (NULL == pStruGetCapRsp)
	{
		return;
	}
	
	switch (pStruGetCapRsp->capability_id) 
	{
	case BTSDK_AVRCP_CAPABILITYID_COMPANY_ID:
		{
			break;
		}
	case BTSDK_AVRCP_CAPABILITYID_EVENTS_SUPPORTED:
		{
			BTUINT8 count = 0;
			while (count < pStruGetCapRsp->count) 
			{
				switch (pStruGetCapRsp->capability[count])
				{
				case BTSDK_AVRCP_EVENT_PLAYBACK_STATUS_CHANGED:
				case BTSDK_AVRCP_EVENT_TRACK_CHANGED:
				case BTSDK_AVRCP_EVENT_PLAYBACK_POS_CHANGED:
				case BTSDK_AVRCP_EVENT_PLAYER_APPLICATION_SETTING_CHANGED:
				case BTSDK_AVRCP_EVENT_TRACK_REACHED_END:
				case BTSDK_AVRCP_EVENT_TRACK_REACHED_START:
				case BTSDK_AVRCP_EVENT_BATT_STATUS_CHANGED:
				case BTSDK_AVRCP_EVENT_SYSTEM_STATUS_CHANGED:
				case BTSDK_AVRCP_EVENT_VOLUME_CHANGED:
					{
						AVRCP_App_RegNotifReq(pStruGetCapRsp->capability[count]);
						break;
					}
				default:
					break;
				}
				count++;
			}
			break;
		}
	default:
		break;
	}
}

void AVRCP_App_GetElementAttrCfm(PBtSdkGetElementAttrRspStru pStruGetElementAttrRsp)
{
	static BTUINT8 count = 0;
	char tcTmpName[MAX_PATH] = {0};
	WCHAR wcName[MAX_PATH] = {0};
	char szName[MAX_PATH] = {0};
	switch (pStruGetElementAttrRsp->subpacket_type) 
	{
	case BTSDK_AVRCP_PACKET_HEAD:
		{
			count = 0;
			break;
		}
	case BTSDK_AVRCP_SUBPACKET:
		{
			count++;
			if (BTSDK_AVRCP_MA_TITLEOF_MEDIA == pStruGetElementAttrRsp->id_value.attr_id)
			{
				memcpy(tcTmpName, pStruGetElementAttrRsp->id_value.value, pStruGetElementAttrRsp->id_value.len);
				MultiByteToWideChar(CP_UTF8, 0, tcTmpName, -1, wcName, MAX_PATH);
				WideCharToMultiByte(CP_ACP, 0, wcName, -1, szName, MAX_PATH, NULL, NULL);
				printf("the music name is :");
				printf(szName);
				printf("\n");
			}
			else if (BTSDK_AVRCP_MA_NAMEOF_ARTIST == pStruGetElementAttrRsp->id_value.attr_id)
			{
				memcpy(tcTmpName, pStruGetElementAttrRsp->id_value.value, pStruGetElementAttrRsp->id_value.len);
				MultiByteToWideChar(CP_UTF8, 0, tcTmpName, -1, wcName, MAX_PATH);
				WideCharToMultiByte(CP_ACP, 0, wcName, -1, szName, MAX_PATH, NULL, NULL);
				printf("the artist is :");
				printf(szName);
				printf("\n");				
			}
			else if (BTSDK_AVRCP_MA_NAMEOF_ALBUM == pStruGetElementAttrRsp->id_value.attr_id)
			{
				memcpy(tcTmpName, pStruGetElementAttrRsp->id_value.value, pStruGetElementAttrRsp->id_value.len);
				MultiByteToWideChar(CP_UTF8, 0, tcTmpName, -1, wcName, MAX_PATH);
				WideCharToMultiByte(CP_ACP, 0, wcName, -1, szName, MAX_PATH, NULL, NULL);
				printf("the album is :");
				printf(szName);
				printf("\n");				
			}
			else if (BTSDK_AVRCP_MA_PLAYING_TIME == pStruGetElementAttrRsp->id_value.attr_id)
			{
				memcpy(tcTmpName, pStruGetElementAttrRsp->id_value.value, pStruGetElementAttrRsp->id_value.len);
				strcpy(s_cPlayingTime, tcTmpName);
				MultiByteToWideChar(CP_UTF8, 0, tcTmpName, -1, wcName, MAX_PATH);
				WideCharToMultiByte(CP_ACP, 0, wcName, -1, szName, MAX_PATH, NULL, NULL);
				printf("the time is :");
				printf(szName);
				printf("\n");
			}			
			break;
		}
	default:
		break;
	}
}

void AVRCP_App_RegNotifReq(BTUINT8      event_id)
{
	BtSdkRegisterNotifReqStru struNotifReq = {0}; 
	printf("AVRCP_App_RegNotifReq function.\n");
	if (BTSDK_INVALID_HANDLE == s_currAudioRmtDevHdl)
	{
		return;
	}
	struNotifReq.size = sizeof(BtSdkRegisterNotifReqStru);
	struNotifReq.event_id = event_id;
	struNotifReq.playback_interval = 0;
	Btsdk_AVRCP_RegNotifReq(s_currAudioRmtDevHdl, &struNotifReq);
}

void AVRCP_App_GetElementAttrReq()
{
	BTUINT16 len_in;
	BTUINT8 num = 4;
	PBtSdkGetElementAttrReqStru pStruGetEleAttrReq = NULL;
	len_in = sizeof(BtSdkGetElementAttrReqStru) + (num * sizeof(BTUINT32));
	pStruGetEleAttrReq = (PBtSdkGetElementAttrReqStru)malloc(len_in);
	printf("AVRCP_App_GetElementAttrReq function.\n");
	if (NULL == pStruGetEleAttrReq)
	{
		return;
	}
	memset(pStruGetEleAttrReq, 0, len_in);
	pStruGetEleAttrReq->size = len_in;
	pStruGetEleAttrReq->num = num;
	pStruGetEleAttrReq->attr_id[0] = BTSDK_AVRCP_MA_TITLEOF_MEDIA; // title name
	pStruGetEleAttrReq->attr_id[1] = BTSDK_AVRCP_MA_NAMEOF_ARTIST; // artist name
	pStruGetEleAttrReq->attr_id[2] = BTSDK_AVRCP_MA_NAMEOF_ALBUM;	 // album name
	pStruGetEleAttrReq->attr_id[3] = BTSDK_AVRCP_MA_PLAYING_TIME;  // playing time(in milliseconds)
	
	Btsdk_AVRCP_GetElementAttrReq(s_currAudioRmtDevHdl, pStruGetEleAttrReq);
	if (NULL != pStruGetEleAttrReq)
	{
		free(pStruGetEleAttrReq);
		pStruGetEleAttrReq = NULL;
	}
	AVRCP_App_RegNotifReq(BTSDK_AVRCP_EVENT_TRACK_CHANGED);
}

void AVRCP_APP_InformCharSetReq()
{
	PBtSdkInformCharSetReqStru pInformCharSetReq = NULL;
	BTUINT32 size = 0;
	BTUINT8 num = 1;
	BTUINT32 ret = BTSDK_OK;
	size = sizeof(BtSdkInformCharSetReqStru) + ((num-1) *sizeof(BTUINT16));
	pInformCharSetReq = (PBtSdkInformCharSetReqStru)malloc(size);
	if (NULL != pInformCharSetReq)
	{
		memset(pInformCharSetReq, 0 ,size);
		pInformCharSetReq->size = size;
		pInformCharSetReq->num = num;
		pInformCharSetReq->characterset_id[0] = BTSDK_AVRCP_CHARACTERSETID_UTF8;
		ret = Btsdk_AVRCP_InformCharSetReq(s_currAudioRmtDevHdl, pInformCharSetReq);
		if (ret == BTSDK_OK)
		{
			printf("AVRCP_APP_InformCharSetReq function.\n");
		}
		free(pInformCharSetReq);
	}

}

void AVRCP_App_ListPlayerAppSetAttrReq()
{
	BTUINT32 ret = 0;
	ret = Btsdk_AVRCP_ListPlayerAppSetAttrReq(s_currAudioRmtDevHdl);
	if (ret == BTSDK_OK)
	{
		printf("AVRCP_App_ListPlayerAppSetAttrReq function.\n");
	}
}

void AVRCP_APP_GetPlayerAppSetAttrTxtReq()
{
	PBtSdkGetPlayerAppSetAttrTxtReqStru pGetPlayerAppSetAttrTxt = NULL;
	BTUINT32 ret = 0;
	BTUINT32 size = 0;
	BTUINT8 num = 4;
	BTUINT8 id_len = 1;

	size = sizeof(BtSdkGetPlayerAppSetAttrTxtReqStru) + (num * sizeof(BTUINT8));
	pGetPlayerAppSetAttrTxt = (PBtSdkGetPlayerAppSetAttrTxtReqStru)malloc(size);
	if (NULL == pGetPlayerAppSetAttrTxt)
	{
		return;
	}
	memset(pGetPlayerAppSetAttrTxt, 0, size);
	pGetPlayerAppSetAttrTxt->size = size;
	pGetPlayerAppSetAttrTxt->num = num;
	pGetPlayerAppSetAttrTxt->id[0] = BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS;
	pGetPlayerAppSetAttrTxt->id[1] = BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS;
	pGetPlayerAppSetAttrTxt->id[2] = BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS;
	pGetPlayerAppSetAttrTxt->id[3] = BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS;
	ret = Btsdk_AVRCP_GetPlayerAppSetAttrTxtReq(s_currAudioRmtDevHdl, pGetPlayerAppSetAttrTxt);
	if (BTSDK_OK == ret)
	{
		printf("AVRCP_APP_GetPlayerAppSetAttrTxtReq func.\n");
	}
}
			
void AVRCP_APP_GetPlayerAppSetValTxtReq()
{
	PBtSdkGetPlayerAppSetValTxtReqStru  pGetPlayAppSetValTxt = NULL;
	BTUINT32 ret = 0;
	BTUINT32 size = 0;
	BTUINT8 id_len = 1;
	BTUINT8 num = 4; // sample  选取shuffle mode

	printf("AVRCP_APP_GetPlayerAppSetValTxtReq func.\n");
	
	size = sizeof(BtSdkGetPlayerAppSetValTxtReqStru) + (num * sizeof(BTUINT8));
	pGetPlayAppSetValTxt = (PBtSdkGetPlayerAppSetValTxtReqStru)malloc(size);
	if (NULL == pGetPlayAppSetValTxt)
	{
		return;
	}
	memset(pGetPlayAppSetValTxt,0,size);
	pGetPlayAppSetValTxt->size = size;
//	pGetPlayAppSetValTxt->attr_id = BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS;
//	pGetPlayAppSetValTxt->attr_id = BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS;
	pGetPlayAppSetValTxt->attr_id = BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS;
//	pGetPlayAppSetValTxt->attr_id = BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS;
	switch (pGetPlayAppSetValTxt->attr_id)
		{
		case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
			{
				num = 2;
				pGetPlayAppSetValTxt->num = num;
				pGetPlayAppSetValTxt->value_id[0] = BTSDK_AVRCP_EQUALIZER_OFF;
				pGetPlayAppSetValTxt->value_id[1] = BTSDK_AVRCP_EQUALIZER_ON;
				break;
			}
		case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
			{
				num = 4;
				pGetPlayAppSetValTxt->num = num;
				pGetPlayAppSetValTxt->value_id[0] = BTSDK_AVRCP_REPEAT_MODE_OFF;
				pGetPlayAppSetValTxt->value_id[1] = BTSDK_AVRCP_REPEAT_MODE_SINGLE_TRACK_REPEAT;
				pGetPlayAppSetValTxt->value_id[2] = BTSDK_AVRCP_REPEAT_MODE_ALL_TRACK_REPEAT;
				pGetPlayAppSetValTxt->value_id[3] = BTSDK_AVRCP_REPEAT_MODE_GROUP_REPEAT;
				break;
			}
		case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
			{
				num = 3;
				pGetPlayAppSetValTxt->num = num;
				pGetPlayAppSetValTxt->value_id[0] = BTSDK_AVRCP_SHUFFLE_OFF;
				pGetPlayAppSetValTxt->value_id[1] = BTSDK_AVRCP_SHUFFLE_ALL_TRACKS_SHUFFLE;
				pGetPlayAppSetValTxt->value_id[2] = BTSDK_AVRCP_SHUFFLE_GROUP_SHUFFLE;
				break;
			}
		case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
			{
				num = 3;
				pGetPlayAppSetValTxt->num = num;
				pGetPlayAppSetValTxt->value_id[0] = BTSDK_AVRCP_SCAN_OFF;
				pGetPlayAppSetValTxt->value_id[1] = BTSDK_AVRCP_SCAN_ALL_TRACKS_SCAN;
				pGetPlayAppSetValTxt->value_id[2] = BTSDK_AVRCP_SCAN_GROUP_SCAN;
				break;
			}
		default:
			printf("failed getPlayerAppSetValTxt Req.\n");
			break;
		}
	ret = Btsdk_AVRCP_GetPlayerAppSetValTxtReq(s_currAudioRmtDevHdl, pGetPlayAppSetValTxt);
	if (BTSDK_OK == ret)
	{
		printf("CurPlayer setting has been set.\n");
		free(pGetPlayAppSetValTxt);
	}

}
void AVRCP_App_ListPlayerAppSetValReq()
{
	BTUINT32 ret = 0;
	BtSdkListPlayerAppSetValReqStru listAppSetVal = {0};
	listAppSetVal.size = sizeof(BtSdkListPlayerAppSetValReqStru);
	
	listAppSetVal.id = BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS;
	ret = Btsdk_AVRCP_ListPlayerAppSetValReq(s_currAudioRmtDevHdl, &listAppSetVal);
	if (ret == BTSDK_OK)
	{
		printf("AVRCP List player app set EQUALIZER val success.\n");
	}
	
	listAppSetVal.id = BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS;
	ret = Btsdk_AVRCP_ListPlayerAppSetValReq(s_currAudioRmtDevHdl, &listAppSetVal);
	if (ret == BTSDK_OK)
	{
		printf("AVRCP List player app set REPEAT val success.\n");
	}
	
	listAppSetVal.id = BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS;
	ret = Btsdk_AVRCP_ListPlayerAppSetValReq(s_currAudioRmtDevHdl, &listAppSetVal);
	if (ret == BTSDK_OK)
	{
		printf("AVRCP List player app set SHUFFLE val success.\n");
	}
	
	listAppSetVal.id = BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS;
	ret = Btsdk_AVRCP_ListPlayerAppSetValReq(s_currAudioRmtDevHdl, &listAppSetVal);
	if (ret == BTSDK_OK)
	{
		printf("AVRCP List player app set SCAN val success.\n");
	}
	//此时id值为BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS
}

void AVRCP_App_GetCurPlayerAppSetValReq()
{
	PBtSdkGetCurPlayerAppSetValReqStru preq = NULL;
	BTUINT16 len_in = 0;
	BTUINT8 num = 4;
	BtSdkRegisterNotifReqStru struNotifReq = {0};
	len_in = sizeof(BtSdkGetCurPlayerAppSetValReqStru) + (num * sizeof(BTUINT8));
	preq = (PBtSdkGetCurPlayerAppSetValReqStru)malloc(len_in);
	printf("AVRCP_App_GetCurPlayerAppSetValReq function.\n");
	if (preq != NULL)
	{
		memset(preq, 0, len_in);
		preq->size = len_in;
		preq->num = num;
		preq->id[0] = BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS;
		preq->id[1] = BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS;
		preq->id[2] = BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS;
		preq->id[3] = BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS;
		Btsdk_AVRCP_GetCurPlayerAppSetValReq(s_currAudioRmtDevHdl, preq);
		free(preq);
		preq = NULL;
	}	 
	AVRCP_App_RegNotifReq(BTSDK_AVRCP_EVENT_PLAYER_APPLICATION_SETTING_CHANGED);
}

void AVRCP_APP_GetPlayerAppSetValTxtCfm(PBtSdkGetPlayerAppSettingValTxtRspStru pGetPlayerAppSetValTxt)
{
	printf("AVRCP_APP_GetPlayerAppSetValTxtCfm func\n");
	switch (pGetPlayerAppSetValTxt->id_string.id)
	{
	case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
		{
			printf("----%s----\n",pGetPlayerAppSetValTxt->id_string.string);
			break;
		}
	case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
		{
			printf("----%s----\n",pGetPlayerAppSetValTxt->id_string.string);
			break;
		}
	case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
		{
			printf("----%s----\n",pGetPlayerAppSetValTxt->id_string.string);
			break;
		}
	case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
		{
			printf("----%s----\n",pGetPlayerAppSetValTxt->id_string.string);
			break;
		}
	default:
		printf("-------no str----------\n");
	}
}

void AVRCP_APP_GetPlayerAppSetAttrTxtCfm(PBtSdkGetPlayerAppSettingAttrTxtRspStru pGetPlayerAppSetAttrTxt)
{
	printf("AVRCP_APP_GetPlayerAppSetAttrTxtCfm func\n");
	switch (pGetPlayerAppSetAttrTxt->id_string.id)
	{
	case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
		{
			printf("----%s----\n",pGetPlayerAppSetAttrTxt->id_string.string);
			break;
		}
	case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
		{
			printf("----%s----\n",pGetPlayerAppSetAttrTxt->id_string.string);
			break;
		}
	case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
		{
			printf("----%s----\n",pGetPlayerAppSetAttrTxt->id_string.string);
			break;
		}
	case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
		{
			printf("----%s----\n",pGetPlayerAppSetAttrTxt->id_string.string);
			break;
		}
	default:
		printf("-------no str----------\n");
	}
}

void AVRCP_APP_SetCurPlayerAppSetValCfm()
{
	printf("--------set success--------\n");
}

void AVRCP_App_GetCurPlayerAppSetValChangedCfm(PBtSdkPlayerAppSetChangedStru pStruPlaySetValChanged)
{
	if (NULL == pStruPlaySetValChanged)
	{
		return;
	}
	if (pStruPlaySetValChanged->id[0].attr_id == BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS 
		&&  pStruPlaySetValChanged->id[0].value_id == BTSDK_AVRCP_REPEAT_MODE_ALL_TRACK_REPEAT)
	{
		printf("---------reapeat mode is all track repeat.-------------\n");
	}
}

void AVRCP_App_PlaybackPosChangedCfm(PBtSdkPlayPosChangedStru pStruPlayPosChanged)
{
	BTUINT32 pos = 0;
	WCHAR positionStr[10] = {0};
	int nPlayingTime = 0;
	printf("AVRCP_App_PlaybackPosChangedCfm function.\n");
	if (NULL == pStruPlayPosChanged)
	{
		return;
	}
	pos = pStruPlayPosChanged->pos;
	nPlayingTime = atoi((const char*)s_cPlayingTime);
	printf("The progress of music is %d, total is %d", pos, nPlayingTime);
	printf("\n");
}

void AVRCP_App_GetCapabilitiesReq()
{
	BTUINT32 ret = 0;
	BtSdkGetCapabilitiesReqStru cap = {0};
	cap.size = sizeof(BtSdkGetCapabilitiesReqStru);
	cap.id = BTSDK_AVRCP_CAPABILITYID_EVENTS_SUPPORTED;
	ret = Btsdk_AVRCP_GetCapabilitiesReq(s_currAudioRmtDevHdl, &cap);
	printf("AVRCP_App_GetCapabilitiesReq function.\n");
}

void AVRCP_App_PlaybackStatusChangedCfm(PBtSdkPlayStatusChangedStru pPlayStatusChanged)
{
	if (pPlayStatusChanged == NULL)
	{
		return;
	}
	//BTSDK_AVRCP_PLAYSTATUS_STOPPED
	printf("Remote Status Changed, current status is %x\n", pPlayStatusChanged->id);
	AVRCP_App_RegNotifReq(BTSDK_AVRCP_EVENT_PLAYBACK_STATUS_CHANGED);	
	printf("AVRCP_App_PlaybackStatusChangedCfm function.\n");
}

typedef struct _PlayerSettingResStru
{
	BTUINT8 attr_id;					// attribute id
	BTUINT8 value_id;					// attribute value
}PlayerSettingResStru, *PPlayerSettingResStru;

PlayerSettingResStru struPlayerSettingRes[] = 
{
	// attr_id									// value_id	
	{BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS,	BTSDK_AVRCP_EQUALIZER_OFF},
	{BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS,	BTSDK_AVRCP_EQUALIZER_ON},
	{BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS,		BTSDK_AVRCP_REPEAT_MODE_OFF},
	{BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS,		BTSDK_AVRCP_REPEAT_MODE_SINGLE_TRACK_REPEAT},
	{BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS,		BTSDK_AVRCP_REPEAT_MODE_ALL_TRACK_REPEAT},
	{BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS,		BTSDK_AVRCP_REPEAT_MODE_GROUP_REPEAT},
	{BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS,		BTSDK_AVRCP_SHUFFLE_OFF},
	{BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS,		BTSDK_AVRCP_SHUFFLE_ALL_TRACKS_SHUFFLE},
	{BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS,		BTSDK_AVRCP_SHUFFLE_GROUP_SHUFFLE},
	{BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS,		BTSDK_AVRCP_SCAN_OFF},
	{BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS,		BTSDK_AVRCP_SCAN_ALL_TRACKS_SCAN},
	{BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS,		BTSDK_AVRCP_SCAN_GROUP_SCAN},
};

/*PlayerSettingResStru struPlayerSettingRes[] = 
{
// attr_id								// value_id										// image to show		// tool tip text
	{BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS,	BTSDK_AVRCP_REPEAT_MODE_OFF},
	{BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS,	BTSDK_AVRCP_REPEAT_MODE_SINGLE_TRACK_REPEAT},
	{BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS,	BTSDK_AVRCP_REPEAT_MODE_ALL_TRACK_REPEAT},
	{BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS,	BTSDK_AVRCP_REPEAT_MODE_GROUP_REPEAT},
	{BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS,	BTSDK_AVRCP_SHUFFLE_OFF},
	{BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS,	BTSDK_AVRCP_SHUFFLE_ALL_TRACKS_SHUFFLE},
	{BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS,	BTSDK_AVRCP_SHUFFLE_GROUP_SHUFFLE},
};*/

int const NUM_PLAYER_SETTING_RES = sizeof(struPlayerSettingRes) / sizeof(PlayerSettingResStru);
void AVRCP_App_GetCurPlayerAppSetValCfm(PBtSdkGetCurPlayerAppSetValRspStru pStruGetCurPlayerAppSetValRsp)
{
	BTUINT8 num = pStruGetCurPlayerAppSetValRsp->num;
	BTUINT8 nCurRepeatValID = 0;
	BTUINT8 nCurShuffleValID = 0;
	BTUINT8 nCurEqualizerValID = 0;
	BTUINT8 nCurScanValID = 0;
	BTUINT8 index = 0;
	int n = 0;
	printf("AVRCP_App_GetCurPlayerAppSetValCfm function.\n");
	if (NULL == pStruGetCurPlayerAppSetValRsp)
	{
		return;
	}
	for (index = 0; index < num; index++)
	{
		BTUINT8 attr_id = pStruGetCurPlayerAppSetValRsp->id[index].attr_id;
		BTUINT8 value_id = pStruGetCurPlayerAppSetValRsp->id[index].value_id;
		
		// 1. record the value id of app setting value
		if (BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS == attr_id)
		{
			nCurRepeatValID = value_id;
		}
		else if (BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS == attr_id)
		{
			nCurShuffleValID = value_id;
		}
		else if(BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS == attr_id)
		{
			nCurEqualizerValID = value_id;
		}
		else if(BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS == attr_id)
		{
			nCurScanValID = value_id;
		}
		
		// 2. make the app setting value button to show corresponding image
		// and tool tip text
		for (n = 0; n < NUM_PLAYER_SETTING_RES; n++)
		{
			if ((attr_id == struPlayerSettingRes[n].attr_id) &&
				(value_id == struPlayerSettingRes[n].value_id))
			{
				if (BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS == attr_id)
				{
					printf("repeart mode status.\n");
					if (BTSDK_AVRCP_REPEAT_MODE_ALL_TRACK_REPEAT == struPlayerSettingRes[n].value_id)
					{
						printf("-----repeat mode is all track repeat.---------\n");
					}
					else if (BTSDK_AVRCP_REPEAT_MODE_OFF == struPlayerSettingRes[n].value_id)
					{
						printf("-----repeat mode is off.----------------------\n");
					}
					break;
				}
				else if (BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS == attr_id)
				{
					printf("shuffle mode status.\n");
					break;
				}
				else if (BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS == attr_id)
				{
					printf("equalizer mode status.\n");
					break;
				}
				else if (BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS == attr_id)
				{
					printf("scan mode status.\n");
					break;
				}
			}
		}
	}
}

BTBOOL AVRCP_CT_Response_Cbk_Func(BTDEVHDL hdl, BTUINT16 rsp_type, BTUINT8 *param)
{
	switch (rsp_type)
	{
	case BTSDK_APP_EV_AVRCP_GET_CAPABILITIES_RSP: // GetCapabilities
		{
			AVRCP_App_GetCapabilitiesCfm((PBtSdkGetCapabilitiesRspStru)param);
			printf("CT Response Callback GET_CAPABILITIES_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_ELEMENT_ATTR_RSP: // GetElementAttributes
		{
			AVRCP_App_GetElementAttrCfm((PBtSdkGetElementAttrRspStru)param);
			printf("CT Response Callback GET_ELEMENT_ATTR_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_TRACK_CHANGED_NOTIF:
		{
			AVRCP_App_GetElementAttrReq();
			printf("CT Response Callback TRACK_CHANGED_NOTIF.\n");
			break;
		}	
	case BTSDK_APP_EV_AVRCP_PLAYBACK_POS_CHANGED_NOTIF:
		{
			if (((PBtSdkPlayPosChangedStru)param)->rsp_code == BTSDK_AVRCP_RSP_CHANGED)
			{
				AVRCP_App_PlaybackPosChangedCfm((PBtSdkPlayPosChangedStru)param);
			}
			printf("CT Response Callback PLAYBACK_POS_CHANGED_NOTIF.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_PLAYBACK_STATUS_CHANGED_NOTIF:
		{
			if (((PBtSdkPlayStatusChangedStru)param)->rsp_code == BTSDK_AVRCP_RSP_CHANGED)
			{
				AVRCP_App_PlaybackStatusChangedCfm((PBtSdkPlayStatusChangedStru)param);
			}
			printf("CT Response Callback PLAYBACK_STATUS_CHANGED_NOTIF.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_FOLDER_ITEMS_RSP:
		{
			PBtSdkGetFolderItemRspStru pRsp = (PBtSdkGetFolderItemRspStru)param;
			if (pRsp != NULL && pRsp->subpacket_type == BTSDK_AVRCP_PACKET_BROWSABLE_ITEM)
			{
				PBtSdkBrowsableItemStru pBrowseItem = (PBtSdkBrowsableItemStru)(&pRsp->item);
				if (pBrowseItem != NULL)
				{
					BTUINT32 size = sizeof(BtSdkMediaPlayerItemStru) + pBrowseItem->player_item.name_len;
					PBtSdkMediaPlayerItemStru pPlayerItem = NULL;
					pPlayerItem = (PBtSdkMediaPlayerItemStru)malloc(size);
					memset(pPlayerItem, 0, size);
					memcpy(pPlayerItem, &pBrowseItem->player_item, size);
				}
			}
			else if (pRsp != NULL && pRsp->subpacket_type == BTSDK_AVRCP_PACKET_MEDIA_ATTR)
			{
				break;
			}
		}
	case BTSDK_APP_EV_AVRCP_INFORM_CHARACTERSET_RSP:
		{
			printf("CT Response Carllback AVRCP_INFORM_CHARACTERSET_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_INFORM_BATTERYSTATUS_OF_CT_RSP:
		{
			AVRCP_App_InformBattStatusCfm((PBtSdkInformBattStatusReqStru)param);
			printf("CT Response Callback AVRCP_INFORM_BATTERYSTATUS_OF_CT_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_LIST_PLAYER_SETTING_ATTR_RSP:
		{
			AVRCP_App_ListPlayerAppSetAttrCfm((PBtSdkListPlayerAppSetAttrRspStru)param);
			printf("CT Response Callback AVRCP_LIST_PLAYER_SETTING_ATTR_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_LIST_PLAYER_SETTING_VALUES_RSP:
		{
			AVRCP_App_ListPlayerAppSetValCfm((PBtSdkListPlayerAppSetValRspStru)param);
			printf("CT Response Callback AVRCP_LIST_PLAYER_SETTING_Val_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_CURRENTPLAYER_SETTING_VALUE_RSP:
		{
			AVRCP_App_GetCurPlayerAppSetValCfm((PBtSdkGetCurPlayerAppSetValRspStru)param);
			printf("CT Response Callback GET_CURRENTPLAYER_SETTING_VALUE_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_PLAYER_APPLICATION_SETTING_CHANGED_NOTIF:
		{
			AVRCP_App_GetCurPlayerAppSetValChangedCfm((PBtSdkPlayerAppSetChangedStru)param);
			printf("CT Response Callback PLAYER_APPLICATION_SETTING_CHANGED_NOTIF.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_SET_CURRENTPLAYER_SETTING_VALUE_RSP:
		{
			AVRCP_APP_SetCurPlayerAppSetValCfm();
			printf("CT Response Callback SET_CURRENTPLAYER_SETTING_VALUE_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_PLAYER_SETTING_ATTR_TEXT_RSP:
		{
			AVRCP_APP_GetPlayerAppSetAttrTxtCfm((PBtSdkGetPlayerAppSettingAttrTxtRspStru)param);
			printf("CT Response Callback GET_PLAYER_SETTING_ATTR_TEXT_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_GET_PLAYER_SETTING_VALUE_TEXT_RSP:
		{
			AVRCP_APP_GetPlayerAppSetValTxtCfm((PBtSdkGetPlayerAppSettingValTxtRspStru)param);
			printf("CT Response Callback GET_PLAYER_SETTING_VALUE_TEXT_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_SET_ADDRESSED_PLAYER_RSP:
		{
			AVRCP_App_SetAddressedPlayerCfm((PBtSdkSetAddresedPlayerReqStru)param);
			printf("CT Response Callback AVRCP_SET_ADDRESSED_PLAYER_RSP.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_ADDRESSED_PLAYER_CHANGED_NOTIF:
		{
			AVRCPSetAddressedPlayerReq();
			printf("CT Response Callback AVRCP_ADDRESSED_PLAYER_CHANGED_NOTIF.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_TRACK_REACHED_END_NOTIF:
		{
			AVRCPTrackReachedEnd((PBtSdkTrackReachEndStru)param);
			printf("CT Response Callback AVRCP_TRACK_REACHED_END_NOTIF.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_TRACK_REACHED_START_NOTIF:
		{
			AVRCPTrackReachedStart((PBtSdkTrackReachStartStru)param);
			printf("CT Response Callback AVRCP_TRACK_REACHED_START_NOTIF.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_BATT_STATUS_CHANGED_NOTIF:
		{
			AVRCPBattStatusChangedReq((PBtSdkBattStatusChangedStru)param);
			printf("CT Response Callback AVRCP_BATT_STATUS_CHANGED_NOTIF.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_SYSTEM_STATUS_CHANGED_NOTIF:
		{
			AVRCPSystemStatusChangedReq((PBtSdkSysStatusChangedStru)param);
			printf("CT Response Callback AVRCP_SYSTEM_STATUS_CHANGED_NOTIF.\n");
			break;
		}	
	case BTSDK_APP_EV_AVRCP_VOLUME_CHANGED_NOTIF:
		{
			AVRCPVolumeChangedReq((PBtSdkVolChangedStru)param);
			printf("CT Response Callback AVRCP_VOLUME_CHANGED_NOTIF.\n");
			break;
		}
	case BTSDK_APP_EV_AVRCP_SET_ABSOLUTE_VOLUME_RSP:
		{
			AVRCPSetAbsoluteVolCfm((PBtSdkSetAbsoluteVolRspStru)param);
			break;
		}
	default:
		printf("AVRCP_CT_Response_Cbk_Func function.  Default  %d\n",rsp_type);
		break;
	}
	return BTSDK_TRUE;
}

void AVRCP_App_SetAddressedPlayerCfm(PBtSdkSetAddresedPlayerReqStru pSetAddressedPlayerRsp){
	if (pSetAddressedPlayerRsp == NULL)
	{
		return;
	}
	printf("--------%x--------\n",pSetAddressedPlayerRsp->id);
	printf("AVRCP_App_SetAddressedPlayerCfm function.\n");
}

void AVRCP_App_ListPlayerAppSetValCfm(PBtSdkListPlayerAppSetValRspStru pListPlAppSetValRsp)
{
	if (pListPlAppSetValRsp == NULL)
	{
		return;
	}
	//	printf("**********%d******%d**********\n",BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS,
	//											  pListPlAppSetValRsp->num);
	if (pListPlAppSetValRsp->num == 3)
	{
		printf("scan status has 3 mode.\n");
	}
	printf("AVRCP_App_ListPlayerAppSetValCfm function.\n");
	
}

void AVRCP_App_ListPlayerAppSetAttrCfm(PBtSdkListPlayerAppSetAttrRspStru pListPlAppSetAttr)
{
	if (pListPlAppSetAttr == NULL)
	{
		return;
	}
	else if (pListPlAppSetAttr->num == 4)
	{
		printf("support Equalizer ON/OFF status.\n");
		printf("support Repeat Mode status.\n");
		printf("support Shuffle ON/OFF status.\n");
		printf("support Scan ON/OFF status.\n");
	}
	printf("AVRCP_App_ListPlayerAppSetAttrCfm function.\n");
}

void AVRCP_App_InformBattStatusCfm(PBtSdkInformBattStatusReqStru pBattChanged)
{
	if (pBattChanged == NULL)
	{
		return;
	}
	printf("AVRCP_App_InformBattStatusCfm function.\n");
}

void TestAVRCPFunc(void)
{
	BTUINT8 ch = 0;
	Btsdk_AVRCP_RegPassThrCmdCbk4ThirdParty(AVRCP_PassThr_Cmd_CbkFunc);
	Btsdk_AVRCP_RegIndCbk4ThirdParty(AVRCP_Event_CbkFunc);
	Btsdk_AVRCP_TGRegCommandCbk((Btsdk_AVRCP_TG_Command_Cbk_Func *)App_AVRCP_TG_Cmd_Cbk);
	Btsdk_AVRCP_CTRegResponseCbk(AVRCP_CT_Response_Cbk_Func);
	
	TestAVRCPShowWorkMenu();
	while (ch != 'r')
	{		
		scanf(" %c", &ch);	
		getchar();
		if (ch == '\n')
		{
			printf(">>");
		}
		else
		{   
			switch (ch)
			{
			case '1':
				TestSelectRmtAudioDev();
				break;
			case '2':
				TestSelectAVRCPSvc();
				break;
			case '3':
				TestConnectAVRCPSvc();
				break;
			case '4':
				TestDisconnectAVRCPSvc();
				break;
			case 'r':
				break;  
			default:
				printf("Invalid command.\n");
				break;
			}			
			printf("\n");				
			TestAVRCPShowWorkMenu();			
		}		
	}
	Btsdk_AVRCP_RegPassThrCmdCbk4ThirdParty(NULL);
	Btsdk_AVRCP_RegIndCbk4ThirdParty(NULL);
	Btsdk_AVRCP_TGRegCommandCbk(NULL);
	Btsdk_AVRCP_CTRegResponseCbk(NULL);
}


/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
This function is the entry function for AV APIs test.
Arguments:
void
Return:
void 
---------------------------------------------------------------------------*/
void TestAVFunc(void)
{
	BTUINT8 ch = 0;
	s_currAudioRmtDevHdl = BTSDK_INVALID_HANDLE;
	s_currAudioSvcHdl = BTSDK_INVALID_HANDLE;
	TestAVShowMenu();	
	while (ch != 'r')
	{		
		scanf(" %c", &ch);	
		getchar();
		if (ch == '\n')
		{
			printf(">>");
		}
		else
		{   
			switch (ch)
			{
			case '1':
				TestSelectRmtAudioDev();
				break;
			case '2':
				TestSelectAudioSvc();
				break;
			case '3':
				TestConnectAudioSvc();
				break;
			case '4':
				if (BTSDK_INVALID_HANDLE != s_currAudioConnHdl)
				{
					Btsdk_Disconnect(s_currAudioConnHdl);
				}
				break;
			case 'r':
				break;  
			default:
				printf("Invalid command.\n");
				break;
			}
			
			printf("\n");				
			TestAVShowMenu();			
		}		
	}
}

void AVRCP_App_GeneralRejectRsp(BTDEVHDL hdl, BTUINT8 tl, BTUINT16 cmd_type, BTUINT8 error_code)
{
	BtSdkGeneralRejectRspStru rsp;	
	rsp.size = sizeof(BtSdkGeneralRejectRspStru);
	rsp.cmd_type = cmd_type;
	rsp.error_code = error_code;
	Btsdk_AVRCP_GeneralRejectRsp(hdl, tl, &rsp);
	printf("AVRCP_App_GeneralRejectRsp function.\n");
}

void AVRCP_APP_AddToNowPlayingInd()
{

}

void AVRCP_App_GetPlayStatusInd(BTDEVHDL hdl, BTUINT8 tl)
{
	//	int getlocal_sec=0;
	//	SYSTEMTIME t_log;
	//	FILE *nowplaying;
	//	int curr_p_m=0,curr_p_s=0;
	//	static int PT_updation=0;
	//	char CurrentPlayTime[10]={0};
	BtSdkPlayStatusRspStru rsp = {0};	
	
	//	GetLocalTime( &t_log );
	
	rsp.size = 3 * sizeof(BTUINT32) + sizeof(BTUINT8);
	
	//	if((t_log.wSecond-getcurrent_sec)!=0)
	//	{
	//		pos=pos+1;
	//		if(pos>leg)
	//			pos=0;
	//		pos_mili=pos*1000;
	//		getcurrent_sec=t_log.wSecond;
	//		PT_updation++;
	//	}
	//
	//	if(PT_updation==3)
	//	{
	//		nowplaying=fopen("C:\\Users\\Public\\Documents\\MrMX\\nowplaying.txt","r+");
	//
	//		if(nowplaying!=NULL)
	//		{
	//			fgets(CurrentPlayTime,10,nowplaying);
	//			fclose(nowplaying);
	//
	//			sscanf(CurrentPlayTime,"%d:%d",&curr_p_m,&curr_p_s);
	//			pos=(curr_p_m*60)+(curr_p_s);
	//			pos_mili=pos*1000;
	//		}
	//		PT_updation=0;
	//	}
	
	
	rsp.song_length = 0x12345678;// The total length of the playing song in milliseconds
	rsp.song_position = 0x98765432;// The current position of the playing in milliseconds elapsed
	
	rsp.play_status = BTSDK_AVRCP_PLAYSTATUS_PLAYING;
	Btsdk_AVRCP_GetPlayStatusRsp(hdl, tl, &rsp);
	//	sprintf(dbuf,"Rx:AVRCP:DATA:Get Play Status Indication  %d %d \n",rsp.song_length,rsp.song_position);
	//	Log_IT(dbuf);
}

void AVRCP_App_GetCurPlayerAppSetValInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkGetCurPlayerAppSetValReqStru in)
{
	PBtSdkGetCurPlayerAppSetValRspStru pGetCurplayerAppSetValRsp = NULL;
	BTUINT16 size = 0;
	BTUINT8 num = in->num;
	BTUINT8 k = 0;
	printf("Inside AVRCP_App_GetCurPlayerAppSetValInd function.\n");
	
	if (in == NULL)
	{
		return;
	}	
	size = sizeof(BtSdkGetCurPlayerAppSetValRspStru) + (num * sizeof(BtSdkIDPairStru));
	pGetCurplayerAppSetValRsp = (PBtSdkGetCurPlayerAppSetValRspStru)malloc(size);
	if (NULL == pGetCurplayerAppSetValRsp)
	{
		return;
	}
	memset(pGetCurplayerAppSetValRsp, 0, size);
	pGetCurplayerAppSetValRsp->size = size;
	pGetCurplayerAppSetValRsp->num = num;
	
	while (k < num) 
	{
		pGetCurplayerAppSetValRsp->id[k].attr_id = in->id[k];
		switch (in->id[k]) 
		{
		case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
			pGetCurplayerAppSetValRsp->id[k].value_id = BTSDK_AVRCP_EQUALIZER_OFF;
			printf("Get current player setting value of EQUALIZER_ONOFF_STATUS.\n");
			break;
		case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
			pGetCurplayerAppSetValRsp->id[k].value_id = BTSDK_AVRCP_REPEAT_MODE_OFF; //BTSDK_AVRCP_REPEAT_MODE_OFF
			printf("Get current player setting value of REPEAT_MODE_STATUS.\n");
			break;
		case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
			pGetCurplayerAppSetValRsp->id[k].value_id = BTSDK_AVRCP_SHUFFLE_ALL_TRACKS_SHUFFLE; //BTSDK_AVRCP_SHUFFLE_OFF
			printf("Get current player setting value of SHUFFLE_ONOFF_STATUS.\n");
			break;
		case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
			pGetCurplayerAppSetValRsp->id[k].value_id = BTSDK_AVRCP_SCAN_OFF;
			printf("Get current player setting value of SCAN_ONOFF_STATUS.\n");
			break;
		default:
			pGetCurplayerAppSetValRsp->num--;
			printf("AVRCP_App_GetCurPlayerAppSetValInd function.Default  %d\n",in->id[k]);
			break;				
		}
		k++;
	}
	
	if (pGetCurplayerAppSetValRsp->num > 0) 
	{
		Btsdk_AVRCP_GetCurPlayerAppSetValRsp(hdl, tl, pGetCurplayerAppSetValRsp);
	} 
	else 
	{
		AVRCP_App_GeneralRejectRsp(hdl, tl, BTSDK_APP_EV_AVRCP_SET_CURRENTPLAYER_SETTING_VALUE_IND, BTSDK_AVRCP_ERROR_INVALID_PARAMETER);
	}
	
	if (NULL != pGetCurplayerAppSetValRsp) 
	{
		free(pGetCurplayerAppSetValRsp);
		pGetCurplayerAppSetValRsp = NULL;
	}
}

void AVRCP_App_SetCurPlayerAppSetValInd(BTDEVHDL hdl, BTUINT8 tl, PBtSdkSetCurPlayerAppSetValReqStru in)
{
	BTUINT8 num = in->num;
	BTUINT8 k = 0;
	BTUINT8 rsp_result = BTSDK_OK;
	printf("AVRCP_App_SetCurPlayerAppSetValInd function.\n");
	if (in == NULL)
	{
		return;
	}
	
	while (k < num) 
	{
		switch (in->id[k].attr_id) 
		{
		case BTSDK_AVRCP_PASA_EQUALIZER_ONOFF_STATUS:
			printf("-----EQUALIZER MODE is %d-------\n",in->id[k].value_id);			//1
			printf("Set current player setting value indication EQUALIZER_ONOFF_STATUS.\n");
			break;
		case BTSDK_AVRCP_PASA_REPEAT_MODE_STATUS:
			//SetCurPlayerAppSetVal(in->id[k].attr_id, in->id[k].value_id);
			printf("-----REPEAT MODE is %d-------\n",in->id[k].value_id);				//1
			printf("Set current player setting value indication REPEAT_MODE_STATUS.\n");
			break;
		case BTSDK_AVRCP_PASA_SHUFFLE_ONOFF_STATUS:
			//SetCurPlayerAppSetVal(in->id[k].attr_id, in->id[k].value_id);
			printf("-----SHUFFLE MODE is %d-------\n",in->id[k].value_id);				//2
			printf("Set current player setting value indication SHUFFLE_ONOFF_STATUS.\n");
			break;
		case BTSDK_AVRCP_PASA_SCAN_ONOFF_STATUS:
			printf("-----SCAN MODE is %d-------\n",in->id[k].value_id);					//1
			printf("Set current player setting value indication SCAN_ONOFF_STATUS.\n");
			break;
		default:// 0x80-0xFF is provided for TG driven static media player menu extension by CT
			printf("AVRCP_App_SetCurPlayerAppSetValInd function. Default  %d\n",in->id[k].attr_id);
			break;				
		}
		k++;
	}
	
	if (rsp_result == BTSDK_OK) 
	{
		Btsdk_AVRCP_SetCurPlayerAppSetValRsp(hdl, tl);
	} 
	else 
	{
		AVRCP_App_GeneralRejectRsp(hdl, tl, BTSDK_APP_EV_AVRCP_SET_CURRENTPLAYER_SETTING_VALUE_IND
			, BTSDK_AVRCP_ERROR_INVALID_PARAMETER);
	}
}
void AVRCP_call_Back_events_reg()
{
	Btsdk_AVRCP_RegPassThrCmdCbk4ThirdParty(AVRCP_PassThr_Cmd_CbkFunc);
	Btsdk_AVRCP_RegIndCbk4ThirdParty(AVRCP_Event_CbkFunc);
	Btsdk_AVRCP_TGRegCommandCbk((Btsdk_AVRCP_TG_Command_Cbk_Func *)App_AVRCP_TG_Cmd_Cbk);
	Btsdk_AVRCP_CTRegResponseCbk(AVRCP_CT_Response_Cbk_Func);
}

void AVRCP_call_Back_events_dereg()
{
    Btsdk_AVRCP_RegPassThrCmdCbk4ThirdParty(NULL);
	Btsdk_AVRCP_RegIndCbk4ThirdParty(NULL);
	Btsdk_AVRCP_TGRegCommandCbk(NULL);
	Btsdk_AVRCP_CTRegResponseCbk(NULL);
}

void AVRCP_Exp_GetPlayerAppSetAttrTextInd(BTDEVHDL dev_hdl, BTUINT8 tl, PBtSdkGetPlayerAppSetAttrTxtReqStru in)
{
	PBtSdkGetPlayerAppSettingValTxtRspStru prsp = NULL; 
	BTUINT32 size = 0; 
	BTUINT8 k = 0; 
	size = 2 * sizeof(BTUINT32) + sizeof(BTUINT8);
	prsp = (PBtSdkGetPlayerAppSettingValTxtRspStru)malloc(size); 
	prsp->size = size; 
	prsp->subpacket_type = BTSDK_AVRCP_PACKET_HEAD;
	prsp->id_num = in->num; 
	Btsdk_AVRCP_GetPlayerAppSetAttrTxtRsp(dev_hdl, tl, prsp); /* Specifies the number of attributes */ 
	free(prsp);
	
	while (k < in->num) 
	{
		BTUINT8 str = NULL; 
		BTUINT8 id = in->id[k]; 
		BTUINT8 len = 0; 
		switch (id)
		{ 
		case 0: str = "Equalizer Status";  break; 
		case 1: str = "Repeat Mode Status"; break; 
		case 2: str = "Shuffle Status"; break;
		case 3: str = "Scan Status"; break; 
		default:  
			break;
		}
		printf("\n  AVRCP_Exp_GetPlayerAppSetAttrTextInd %d",id);
		len = strlen(str); 
		size = sizeof(PBtSdkGetPlayerAppSettingValTxtRspStru) + (len - 1) * sizeof(BTUINT8); 
		prsp = (PBtSdkGetPlayerAppSettingValTxtRspStru)malloc(size); 
		prsp->size = size;
		prsp->subpacket_type = BTSDK_AVRCP_SUBPACKET;
		prsp->id_string.id = in->id[k]; 
		prsp->id_string.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8; 
		prsp->id_string.len = len; 
		//strcpy(prsp->id_string.string, str[in->id[k]]);
		Btsdk_AVRCP_GetPlayerAppSetAttrTxtRsp(dev_hdl, tl, prsp); /* Sent attribute */ 
		free(prsp); 
		k++; 
	} 
}


void AVRCP_Exp_GetFolderItemInd(BTDEVHDL dev_hdl, BTUINT8 tl, PBtSdkGetFolderItemReqStru in)
{
	PBtSdkGetFolderItemRspStru prsp = NULL;
	BTUINT32 size = 0;
	BTUINT8 *str_name = NULL;
	BTUINT16 item_len = 0;
	BTUINT16 name_len = 0;
	BTUINT8 k = 0;
	if (in == NULL)
	{
		return;
	}
	/* If in->attr_count is set to zero, all attribute information shall be returned, else attribute information for the specified attribute IDs shall be returned by TG. */
	/* Specify the folder 揑VT_Folder?*/ 
	switch (in->scope)
	{
	case BTSDK_AVRCP_SCOPE_MEDIAPLAYER_LIST:
		{
			BTUINT32 item_num = in->end_item - in->start_item + 1;
			BTUINT32 index = 0;
			
			while (index++ < item_num) 
			{
				str_name = "IVT_Folder";
				name_len = strlen(str_name);
				item_len = 4 * sizeof(BTUINT16) + sizeof(BtSdkMediaPlayerItemStru) + (name_len - 1) * sizeof(BTUINT8); // size of BtSdkBrowsableItemStru
				size = 2 * sizeof(BTUINT32) + item_len; // size of BtSdkGetFolderItemRspStru
				prsp = malloc(size);
				if (prsp != NULL)
				{
					memset(prsp, 0, size);
					prsp->size = size;
					prsp->subpacket_type = BTSDK_AVRCP_PACKET_BROWSABLE_ITEM;
					
					prsp->item.items_num = item_num;
					prsp->item.uid_counter = 0x00;
					prsp->item.item_len = item_len;
					prsp->item.item_type = BTSDK_AVRCP_ITEMTYPE_MEDIAPLAYER_ITEM;
					prsp->item.status = BTSDK_AVRCP_ERROR_SUCCESSFUL;
					
					prsp->item.player_item.player_id = 1;
					prsp->item.player_item.play_status = BTSDK_AVRCP_PLAYSTATUS_STOPPED;
					prsp->item.player_item.major_player_type = BTSDK_AVRCP_MAJORPLAYERTYPE_AUDIO;
					prsp->item.player_item.player_subtype = BTSDK_AVRCP_PLAYERSUBTYPE_NONE;
					memset(prsp->item.player_item.feature_bitmask, 0xFF, 16); /* Please refer to AVRCP 1.4 Specification 6.10.2.1 */
					prsp->item.player_item.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;		/* e.g AVRCP_CHARACTERSETID_UTF8 */
					prsp->item.player_item.name_len = name_len;
					memcpy(prsp->item.player_item.name, str_name, name_len);
					
					Btsdk_AVRCP_GetFolderItemsRsp(dev_hdl, tl, prsp);
					free(prsp);
				}
			}
		}
		break;
	case BTSDK_AVRCP_SCOPE_MEDIAPLAYER_NOWPLAYING:
		{
			BTUINT32 item_num = in->end_item - in->start_item + 1;
			BTUINT32 index = 0;
			BTUINT8 song_name[MAX_PATH] = {0};
			BTUINT8 attr_num = (in->attr_count) ? in->attr_count : 7; 
			
			while (index++ < item_num) 
			{
				BTUINT8 attr_index = 0;
				sprintf(song_name, "Song%02d", index);
				name_len = strlen(song_name);
				item_len = 4 * sizeof(BTUINT16) + sizeof(BtSdkMediaElementItemStru) + (name_len - 1) * sizeof(BTUINT8); // size of BtSdkBrowsableItemStru
				size = 2 * sizeof(BTUINT32) + item_len; // size of 
				prsp = malloc(size);
				if (prsp != NULL)
				{
					memset(prsp, 0, size);
					prsp->size = size;
					prsp->subpacket_type = BTSDK_AVRCP_PACKET_BROWSABLE_ITEM;
					
					prsp->item.items_num = item_num;
					prsp->item.uid_counter = 0x00;
					prsp->item.item_len = item_len;
					prsp->item.item_type = BTSDK_AVRCP_ITEMTYPE_MEDIAELEMENT_ITEM;
					prsp->item.status = BTSDK_AVRCP_ERROR_SUCCESSFUL;
					
					prsp->item.element_item.element_uid[8];			/* UID as defined in 6.10.3 */
					prsp->item.element_item.media_type = BTSDK_AVRCP_MEDIATYPE_AUDIO;				/* e.g AVRCP_MEDIATYPE_AUDIO */
					prsp->item.element_item.attr_num = attr_num;
					prsp->item.element_item.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;		/* e.g AVRCP_CHARACTERSETID_UTF8 */
					prsp->item.element_item.name_len = name_len;
					memcpy(prsp->item.element_item.name, song_name, name_len);
					
					Btsdk_AVRCP_GetFolderItemsRsp(dev_hdl, tl, prsp);
					free(prsp);
				}
				
				while (attr_index < attr_num)
				{
					switch (in->attr_id[attr_index])
					{ 
					case BTSDK_AVRCP_MA_TITLEOF_MEDIA:
						str_name = "Give Peace a Chance";
						break;
					case BTSDK_AVRCP_MA_NAMEOF_ARTIST:
						str_name = "Maggie";
						break;
					case BTSDK_AVRCP_MA_NAMEOF_ALBUM:
						str_name = "One World";
						break;
					case BTSDK_AVRCP_MA_NUMBEROF_MEDIA:
						str_name = "10";
						break;
					case BTSDK_AVRCP_MA_TOTALNUMBEROF_MEDIA:
						str_name = "16";
						break;
					case BTSDK_AVRCP_MA_GENRE: 
						str_name = "Rock";
						break;
					case BTSDK_AVRCP_MA_PLAYING_TIME:
						str_name = "103000";
						break; 
					default:
						break;
					}
					name_len = strlen(str_name);
					item_len = 4 * sizeof(BTUINT32) + sizeof(BtSdk4IDStringStru) + (name_len - 1) * sizeof(BTUINT8); // size of BtSdkBrowsableItemStru
					size = 2 * sizeof(BTUINT32) + item_len; // size of BtSdkGetFolderItemRspStru
					prsp = malloc(size);
					if (prsp)
					{
						memset(prsp, 0, size);
						prsp->size = size;
						prsp->subpacket_type = BTSDK_AVRCP_PACKET_MEDIA_ATTR;
						
						prsp->element_attr.attr_id = in->attr_id[attr_index];				/* Attributes ID */
						prsp->element_attr.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;		/* Character set ID */
						prsp->element_attr.len = name_len;					/* Length of value */
						memcpy(prsp->element_attr.value, str_name, name_len);				/* Value of the specified attribute */
						
						Btsdk_AVRCP_GetFolderItemsRsp(dev_hdl, tl, prsp);
						free(prsp);
					}
					attr_index++;
				}
			}
		}
		break;
	case BTSDK_AVRCP_SCOPE_MEDIAPLAYER_VIRTUAL_FILESYSTEM:	//refer to 6.10.1.2 
		{
			PBtSdkGetFolderItemRspStru prsp_file = NULL;
			BTUINT16 item_num = in->end_item - in->end_item + 1;//the number of CT wants 
			BTUINT32 folderItem_len = 0;
			BTUINT8 folder_name[MAX_PATH] = {0};
			BTUINT32 index = 0;
			BTUINT8 folder_type = 0;
			BTUINT8 song_name[MAX_PATH] = {0};
			
			if (stricmp(s_current_folder, "Root"))
			{
				index = 0;
				item_num = 2;			//the number of TG have and response
				while(index++ < item_num)
				{
					if (index == 0)
					{
						strcpy(folder_name, "Songlists");
						folder_type = BTSDK_AVRCP_FOLDERTYPE_PLAYLISTS;
					}
					else if (index == 1)
					{
						strcpy(folder_name, "bands");
						folder_type = BTSDK_AVRCP_FOLDERTYPE_ARTISTS;
					}
					folderItem_len = sizeof(BtSdkFolderItemStru) + ((name_len - 1) * sizeof(BTUINT8));
					item_len = 4 * (sizeof(BTUINT16)) + folderItem_len;
					size = 2 * (sizeof(BTUINT32)) + item_len;
					prsp_file = (PBtSdkGetFolderItemRspStru)malloc(size);
					
					name_len = strlen(folder_name);			
					if (NULL != prsp_file)
					{
						memset(prsp_file, 0, size);
						prsp_file->size = size;
						prsp_file->subpacket_type = BTSDK_AVRCP_PACKET_BROWSABLE_ITEM;
						prsp_file->item.items_num = item_num;
						prsp_file->item.uid_counter = 0x00;
						prsp_file->item.item_len = item_len;
						prsp_file->item.item_type = BTSDK_AVRCP_ITEMTYPE_FOLDER_ITEM;
						prsp_file->item.status = BTSDK_AVRCP_ERROR_SUCCESSFUL;
						prsp_file->item.folder_item.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;
						prsp_file->item.folder_item.folder_uid;
						prsp_file->item.folder_item.folder_type = folder_type;
						prsp_file->item.folder_item.is_playable = BTSDK_AVRCP_ISPLAYABLE_CAN;
						prsp_file->item.folder_item.name_len = name_len;
						strcpy(prsp_file->item.folder_item.name, folder_name);
						
						Btsdk_AVRCP_GetFolderItemsRsp(dev_hdl, tl, prsp_file);
						free(prsp_file);
					}
				}
			}
			else if (stricmp(s_current_folder, "Root\\Songlists"))
			{
				index = 0;
				item_num = 2;
				while(index++ < item_num)
				{
					if (index == 0)
					{
						strcpy(folder_name, "Monday");
						folder_type = BTSDK_AVRCP_FOLDERTYPE_TITLES;
					}
					else if (index == 1)
					{
						strcpy(folder_name, "Tuesday");
						folder_type = BTSDK_AVRCP_FOLDERTYPE_TITLES;
					}
					folderItem_len = sizeof(BtSdkFolderItemStru) + ((name_len - 1) * sizeof(BTUINT8));
					item_len = 4 * (sizeof(BTUINT16)) + folderItem_len;
					size = 2 * (sizeof(BTUINT32)) + item_len;
					prsp_file = (PBtSdkGetFolderItemRspStru)malloc(size);
					
					name_len = strlen(folder_name);			
					if (NULL != prsp_file)
					{
						memset(prsp_file, 0, size);
						prsp_file->size = size;
						prsp_file->subpacket_type = BTSDK_AVRCP_PACKET_HEAD;
						prsp_file->item.items_num = 1;
						prsp_file->item.uid_counter = 0x00;
						prsp_file->item.item_len = item_len;
						prsp_file->item.item_type = BTSDK_AVRCP_ITEMTYPE_FOLDER_ITEM;
						prsp_file->item.status = BTSDK_AVRCP_ERROR_SUCCESSFUL;
						prsp_file->item.folder_item.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;
						prsp_file->item.folder_item.folder_uid;
						prsp_file->item.folder_item.folder_type = folder_type;
						prsp_file->item.folder_item.is_playable = BTSDK_AVRCP_ISPLAYABLE_CAN;
						prsp_file->item.folder_item.name_len = name_len;
						strcpy(prsp_file->item.folder_item.name, folder_name);
						Btsdk_AVRCP_GetFolderItemsRsp(dev_hdl, tl, prsp_file);
						free(prsp_file);
					}
				}
			}
			else if (stricmp(s_current_folder, "Root\\Songlists\\Monday"))
			{
				BTUINT8 attr_num = (in->attr_count) ? in->attr_count : 7;
				index = 0;
				item_num = 1;
				while (index++ < item_num) 
				{
					BTUINT8 attr_index = 0;
					strcpy(song_name, "SongABC");
					name_len = strlen(song_name);
					item_len = 4 * sizeof(BTUINT16) + sizeof(BtSdkMediaElementItemStru) + (name_len - 1) * sizeof(BTUINT8); // size of BtSdkBrowsableItemStru
					size = 2 * sizeof(BTUINT32) + item_len; // size of 
					prsp = malloc(size);
					if (prsp != NULL)
					{
						memset(prsp, 0, size);
						prsp->size = size;
						prsp->subpacket_type = BTSDK_AVRCP_PACKET_BROWSABLE_ITEM;
						
						prsp->item.items_num = item_num;//?
						prsp->item.uid_counter = 0x00;
						prsp->item.item_len = item_len;
						prsp->item.item_type = BTSDK_AVRCP_ITEMTYPE_MEDIAELEMENT_ITEM;
						prsp->item.status = BTSDK_AVRCP_ERROR_SUCCESSFUL;
						
						prsp->item.element_item.element_uid[8];			/* UID as defined in 6.10.3 */
						prsp->item.element_item.media_type = BTSDK_AVRCP_MEDIATYPE_AUDIO;				/* e.g AVRCP_MEDIATYPE_AUDIO*/ 
						prsp->item.element_item.attr_num = attr_num;
						prsp->item.element_item.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;		/* e.g AVRCP_CHARACTERSETID_UTF8 */
						prsp->item.element_item.name_len = name_len;
						memcpy(prsp->item.element_item.name, song_name, name_len);
						
						Btsdk_AVRCP_GetFolderItemsRsp(dev_hdl, tl, prsp);
						free(prsp);
					}
					while (attr_index < attr_num)
					{
						switch (in->attr_id[attr_index])
						{ 
						case BTSDK_AVRCP_MA_TITLEOF_MEDIA:
							str_name = "SongABC";
							break;
						case BTSDK_AVRCP_MA_NAMEOF_ARTIST:
							str_name = "Maggie";
							break;
						case BTSDK_AVRCP_MA_NAMEOF_ALBUM:
							str_name = "One World";
							break;
						case BTSDK_AVRCP_MA_NUMBEROF_MEDIA:
							str_name = "10";
							break;
						case BTSDK_AVRCP_MA_TOTALNUMBEROF_MEDIA:
							str_name = "16";
							break;
						case BTSDK_AVRCP_MA_GENRE: 
							str_name = "Rock";
							break;
						case BTSDK_AVRCP_MA_PLAYING_TIME:
							str_name = "103000";
							break; 
						default:
							break;
						}
						name_len = strlen(str_name);
						item_len = 4 * sizeof(BTUINT32) + sizeof(BtSdk4IDStringStru) + (name_len - 1) * sizeof(BTUINT8); // size of BtSdkBrowsableItemStru
						size = 2 * sizeof(BTUINT32) + item_len; // size of BtSdkGetFolderItemRspStru
						prsp = malloc(size);
						if (prsp)
						{
							memset(prsp, 0, size);
							prsp->size = size;
							prsp->subpacket_type = BTSDK_AVRCP_PACKET_MEDIA_ATTR;
							
							prsp->element_attr.attr_id = in->attr_id[attr_index];				/* Attributes ID */
							prsp->element_attr.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;		/* Character set ID */
							prsp->element_attr.len = name_len;					/* Length of value */
							memcpy(prsp->element_attr.value, str_name, name_len);				/* Value of the specified attribute */
							
							Btsdk_AVRCP_GetFolderItemsRsp(dev_hdl, tl, prsp);
							free(prsp);
						}
						attr_index++;
					}
				}
			}		
			break;
		}
	case BTSDK_AVRCP_SCOPE_MEDIAPLAYER_SEARCH:
		{
			PBtSdkGetFolderItemRspStru prsp_search = NULL;
			BTUINT32 item_num_get = in->end_item - in->start_item + 1;
			BTUINT32 item_num_back = 0;
			BTUINT32 index = 0;
			BTUINT8 song_name[MAX_PATH] = {0};
			BTUINT8 attr_num = (in->attr_count) ? in->attr_count : 7;
			if (1) //refer to 6.11 p87 support search condition.
			{
				if (item_num_get < 1)
				{
					strcpy(song_name, "");
					item_num_back = 0;
				}
				else
				{
					strcpy(song_name, "SongABC");
					item_num_back = item_num_get;
				}
				index = 0;
				while (index++ < item_num_get) 
				{
					BTUINT8 attr_index = 0;
					name_len = strlen(song_name);
					item_len = 4 * sizeof(BTUINT16) + sizeof(BtSdkMediaElementItemStru) + (name_len - 1) * sizeof(BTUINT8); // size of BtSdkBrowsableItemStru
					size = 2 * sizeof(BTUINT32) + item_len; // size of 
					prsp_search = malloc(size);
					if (prsp != NULL)
					{
						memset(prsp, 0, size);
						prsp_search->size = size;
						prsp_search->subpacket_type = BTSDK_AVRCP_PACKET_BROWSABLE_ITEM;
						prsp_search->item.items_num = item_num_back;//?
						prsp_search->item.uid_counter = 0x00;
						prsp_search->item.item_len = item_len;
						prsp_search->item.item_type = BTSDK_AVRCP_ITEMTYPE_MEDIAELEMENT_ITEM;
						prsp_search->item.status = BTSDK_AVRCP_ERROR_SUCCESSFUL;
						
						prsp_search->item.element_item.element_uid[8];			/* UID as defined in 6.10.3 */
						prsp_search->item.element_item.media_type = BTSDK_AVRCP_MEDIATYPE_AUDIO;				/* e.g AVRCP_MEDIATYPE_AUDIO*/ 
						prsp_search->item.element_item.attr_num = attr_num;
						prsp_search->item.element_item.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;		/* e.g AVRCP_CHARACTERSETID_UTF8 */
						prsp_search->item.element_item.name_len = name_len;
						memcpy(prsp_search->item.element_item.name, song_name, name_len);
						
						Btsdk_AVRCP_GetFolderItemsRsp(dev_hdl, tl, prsp_search);
						free(prsp_search);
					}
					while (attr_index < attr_num)
					{
						switch (in->attr_id[attr_index])
						{ 
						case BTSDK_AVRCP_MA_TITLEOF_MEDIA:
							str_name = "SongAbC";
							break;
						case BTSDK_AVRCP_MA_NAMEOF_ARTIST:
							str_name = "Maggie";
							break;
						case BTSDK_AVRCP_MA_NAMEOF_ALBUM:
							str_name = "One World";
							break;
						case BTSDK_AVRCP_MA_NUMBEROF_MEDIA:
							str_name = "10";
							break;
						case BTSDK_AVRCP_MA_TOTALNUMBEROF_MEDIA:
							str_name = "16";
							break;
						case BTSDK_AVRCP_MA_GENRE: 
							str_name = "Rock";
							break;
						case BTSDK_AVRCP_MA_PLAYING_TIME:
							str_name = "103000";
							break; 
						default:
							break;
						}
						name_len = strlen(str_name);
						item_len = 4 * sizeof(BTUINT32) + sizeof(BtSdk4IDStringStru) + (name_len - 1) * sizeof(BTUINT8); // size of BtSdkBrowsableItemStru
						size = 2 * sizeof(BTUINT32) + item_len; // size of BtSdkGetFolderItemRspStru
						prsp_search = malloc(size);
						if (prsp)
						{
							memset(prsp, 0, size);
							prsp_search->size = size;
							prsp_search->subpacket_type = BTSDK_AVRCP_PACKET_MEDIA_ATTR;
							
							prsp_search->element_attr.attr_id = in->attr_id[attr_index];				/* Attributes ID */
							prsp_search->element_attr.characterset_id = BTSDK_AVRCP_CHARACTERSETID_UTF8;		/* Character set ID */
							prsp_search->element_attr.len = name_len;					/* Length of value */
							memcpy(prsp_search->element_attr.value, str_name, name_len);				/* Value of the specified attribute */
							
							Btsdk_AVRCP_GetFolderItemsRsp(dev_hdl, tl, prsp_search);
							free(prsp_search);
						}
						attr_index++;
					}
				}
			}
			else 
			{
				AVRCP_App_GeneralRejectRsp(dev_hdl, tl, BTSDK_APP_EV_AVRCP_GET_FOLDER_ITEMS_IND, BTSDK_AVRCP_ERROR_INVALID_PARAMETER);
			}
			break;
		}
	default:
		break;
	}
}