﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace fsbBlueToothCallerID
{
    public partial class Form1 : Form
    {
        //Defines the api's used to connect to the BSWrapper dll.
        [DllImport("BSWrapper.dll")]
        private static extern uint Initialise(IntPtr bd_addr);
        [DllImport("BSWrapper.dll")]
        private static extern uint ShutDown();
        [DllImport("BSWrapper.dll")]
        private static extern string ReturnCallBacks(ushort levent);
        [DllImport("BSWrapper.dll")]
        private static extern uint Commands (char levent, string variable);

        //Variables to request data from BSWrapper. Kept the same as the BlueSoleil sample program.
        ushort BTSDK_APP_EV_AGAP_BASE = 0x900;
        enum BTSDK_HFP_APP_EventCodeEnum
        {
            ///* HFP_SetState Callback to Application Event Code */
            ///* SLC - Both AG and HF */
            BTSDK_HFP_EV_SLC_ESTABLISHED_IND = 0x900 + 0x1,     ///* HFP Service Level connection established. Parameter: Btsdk_HFP_ConnInfoStru */
            BTSDK_HFP_EV_SLC_RELEASED_IND,                      ///* SPP connection released. Parameter: Btsdk_HFP_ConnInfoStru */

            ///* SCO - Both AG and HF  */
            BTSDK_HFP_EV_AUDIO_CONN_ESTABLISHED_IND,            ///* SCO audio connection established */
            BTSDK_HFP_EV_AUDIO_CONN_RELEASED_IND,               ///* SCO audio connection released */

            ///* Status Changed Indication */
            BTSDK_HFP_EV_STANDBY_IND,                           ///* STANDBY Menu, the incoming call or outgoing call or ongoing call is canceled  */
            BTSDK_HFP_EV_ONGOINGCALL_IND,                       ///* ONGOING-CALL Menu, a call (incoming call or outgoing call) is established (ongoing) */
            BTSDK_HFP_EV_RINGING_IND,                           ///* RINGING Menu, a call is incoming. Parameter: BTBOOL - in-band ring tone or not.   */
            BTSDK_HFP_EV_OUTGOINGCALL_IND,                      ///* OUTGOING-CALL Menu, an outgoing call is being established, 3Way in Guideline P91 */
            BTSDK_HFP_EV_CALLHELD_IND,                          ///* BTRH-HOLD Menu, +BTRH:0, AT+BTRH=0, incoming call is put on hold */
            BTSDK_HFP_EV_CALL_WAITING_IND,                      ///* Call Waiting Menu, +CCWA, When Call=Active, call waiting notification. Parameter: Btsdk_HFP_PhoneInfoStru */
            BTSDK_HFP_EV_TBUSY_IND,                             ///* GSM Network Remote Busy, TBusy Timer Activated */

            ///* AG & HF APP General Event Indication */
            BTSDK_HFP_EV_GENERATE_INBAND_RINGTONE_IND,          ///* AG Only, Generate the in-band ring tone */
            BTSDK_HFP_EV_TERMINATE_LOCAL_RINGTONE_IND,          ///* Terminate local generated ring tone or the in-band ring tone */
            BTSDK_HFP_EV_VOICE_RECOGN_ACTIVATED_IND,            ///* +BVRA:1, voice recognition activated indication or HF request to start voice recognition procedure */
            BTSDK_HFP_EV_VOICE_RECOGN_DEACTIVATED_IND,          ///* +BVRA:0, voice recognition deactivated indication or requests AG to deactivate the voice recognition procedure */
            BTSDK_HFP_EV_NETWORK_AVAILABLE_IND,                 ///* +CIEV:<service><value>, cellular network is available */
            BTSDK_HFP_EV_NETWORK_UNAVAILABLE_IND,               ///* +CIEV:<service><value>, cellular network is unavailable */
            BTSDK_HFP_EV_ROAMING_RESET_IND,                     ///* +CIEV:<roam><value>, roaming is not active */
            BTSDK_HFP_EV_ROAMING_ACTIVE_IND,                    ///* +CIEV:<roam><value>, a roaming is active */
            BTSDK_HFP_EV_SIGNAL_STRENGTH_IND,                   ///* +CIEV:<signal><value>, signal strength indication. Parameter: BTUINT8 - indicator value */	
            BTSDK_HFP_EV_BATTERY_CHARGE_IND,                    ///* +CIEV:<battchg><value>, battery charge indication. Parameter: BTUINT8 - indicator value  */
            BTSDK_HFP_EV_CHLDHELD_ACTIVATED_IND,                ///* +CIEV:<callheld><1>, Call on CHLD Held to be or has been actived. */
            BTSDK_HFP_EV_CHLDHELD_RELEASED_IND,                 ///* +CIEV:<callheld><0>, Call on CHLD Held to be or has been released. */	
            BTSDK_HFP_EV_MICVOL_CHANGED_IND,                    ///* +VGM, AT+VGM, microphone volume changed indication */
            BTSDK_HFP_EV_SPKVOL_CHANGED_IND,                    ///* +VGS, AT+VGS, speaker volume changed indication */

            ///* OK and Error Code - HF only */
            BTSDK_HFP_EV_ATCMD_RESULT,                          ///* HF Received OK, Error/+CME ERROR from AG or Wait for AG Response Timeout. Parameter: Btsdk_HFP_ATCmdResultStru */

            ///* To HF APP, Call Related, AG Send information to HF */
            BTSDK_HFP_EV_CLIP_IND,                              ///* +CLIP, Phone Number Indication. Parameter: Btsdk_HFP_PhoneInfoStru */
            BTSDK_HFP_EV_CURRENT_CALLS_IND,                     ///* +CLCC, the current calls of AG. Parameter: Btsdk_HFP_CLCCInfoStru */
            BTSDK_HFP_EV_NETWORK_OPERATOR_IND,                  ///* +COPS, the current network operator name of AG. Parameter: Btsdk_HFP_COPSInfoStru */
            BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND,                 ///* +CNUM, the subscriber number of AG. Parameter: Btsdk_HFP_PhoneInfoStru */
            BTSDK_HFP_EV_VOICETAG_PHONE_NUM_IND,                ///* +BINP, AG inputted phone number for voice-tag, requests AG to input a phone number for the voice-tag at the HF side. Parameter: Btsdk_HFP_PhoneInfoStru */

            ///* AG APP, HF Request or Indicate AG */
            BTSDK_HFP_EV_CURRENT_CALLS_REQ,                     ///* AT+CLCC, query the list of current calls in AG. */
            BTSDK_HFP_EV_NETWORK_OPERATOR_FORMAT_REQ,           ///* AT+COPS=3,0, indicate app the network operator name should be set to long alphanumeric */
            BTSDK_HFP_EV_NETWORK_OPERATOR_REQ,                  ///* AT+COPS?, requests AG to respond with +COPS response indicating the currently selected operator */
            BTSDK_HFP_EV_SUBSCRIBER_NUMBER_REQ,                 ///* AT+CNUM, query the AG subscriber number information. */
            BTSDK_HFP_EV_VOICETAG_PHONE_NUM_REQ,                ///* AT+BINP, requests AG to input a phone number for the voice-tag at the HF */
            BTSDK_HFP_EV_CUR_INDICATOR_VAL_REQ,                 ///* AT+CIND?, get the current indicator during the service level connection initialization procedure */
            BTSDK_HFP_EV_HF_DIAL_REQ,                           ///* ATD, instructs AG to dial the specific phone number. Parameter: (HFP only) BTUINT8* - phone number */
            BTSDK_HFP_EV_HF_MEM_DIAL_REQ,                       ///* ATD>, instructs AG to dial the phone number indexed by the specific memory location of SIM card. Parameter: BTUINT8* - memory location */
            BTSDK_HFP_EV_HF_LASTNUM_REDIAL_REQ,                 ///* AT+BLDN, instructs AG to redial the last dialed phone number */
            BTSDK_HFP_EV_MANUFACTURER_REQ,                      ///* AT+CGMI, requests AG to respond with the Manufacturer ID */
            BTSDK_HFP_EV_MODEL_REQ,                             ///* AT+CGMM, requests AG to respond with the Model ID */
            BTSDK_HFP_EV_NREC_DISABLE_REQ,                      ///* AT+NREC=0, requests AG to disable NREC function */
            BTSDK_HFP_EV_DTMF_REQ,                              ///* AT+VTS, instructs AG to transmit the specific DTMF code. Parameter: BTUINT8 - DTMF code */
            BTSDK_HFP_EV_ANSWER_CALL_REQ,                       ///* inform AG app to answer the call. Parameter: BTUINT8 - One of BTSDK_HFP_TYPE_INCOMING_CALL, BTSDK_HFP_TYPE_HELDINCOMING_CALL. */	
            BTSDK_HFP_EV_CANCEL_CALL_REQ,                       ///* inform AG app to cancel the call. Parameter: BTUINT8 - One of BTSDK_HFP_TYPE_ALL_CALLS, BTSDK_HFP_TYPE_INCOMING_CALL,BTSDK_HFP_TYPE_HELDINCOMING_CALL, BTSDK_HFP_TYPE_OUTGOING_CALL, BTSDK_HFP_TYPE_ONGOING_CALL. */	
            BTSDK_HFP_EV_HOLD_CALL_REQ,                         ///* inform AG app to hold the incoming call (AT+BTRH=0) */

            ///* AG APP, 3-Way Calling */
            BTSDK_HFP_EV_REJECTWAITINGCALL_REQ,                 ///* AT+CHLD=0, Release all held calls or reject waiting call. */	
            BTSDK_HFP_EV_ACPTWAIT_RELEASEACTIVE_REQ,            ///* AT+CHLD=1, Accept the held or waiting call and release all avtive calls. Parameter: BTUINT8 - value of <idx>*/
            BTSDK_HFP_EV_HOLDACTIVECALL_REQ,                    ///* AT+CHLD=2, Held Specified Active Call.  Parameter: BTUINT8 - value of <idx>*/
            BTSDK_HFP_EV_ADD_ONEHELDCALL_2ACTIVE_REQ,           ///* AT+CHLD=3, Add One CHLD Held Call to Active Call. */
            BTSDK_HFP_EV_LEAVE3WAYCALLING_REQ,                  ///* AT+CHLD=4, Leave The 3-Way Calling. */

            ///* Extended */
            BTSDK_HFP_EV_EXTEND_CMD_IND,                        ///* indicate app extend command received. Parameter: BTUINT8* - Full extended AT command or result code. */
            BTSDK_HFP_EV_PRE_SCO_CONNECTION_IND,                ///* indicate app to create SCO connection. Parameter: Btsdk_AGAP_PreSCOConnIndStru. */
            BTSDK_HFP_EV_SPP_ESTABLISHED_IND,                   ///* SPP connection created. Parameter: Btsdk_HFP_ConnInfoStru. added 2008-7-3 */
            BTSDK_HFP_EV_HF_MANUFACTURERID_IND,                 ///* ManufacturerID indication. Parameter: BTUINT8* - Manufacturer ID of the AG device, a null-terminated ASCII string. */
            BTSDK_HFP_EV_HF_MODELID_IND,                        ///* ModelID indication.  Parameter: BTUINT8* - Model ID of the AG device, a null-terminated ASCII string. */
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            byte[] bd_addr;
            bd_addr = new byte[6];
            IntPtr ptr;
            ptr = Marshal.AllocHGlobal(6);

            //make byte arrary of device address
            bd_addr = Utility.HexEncoding.GetBytes(deviceAddress.Text, i);
            for (i = 0; i<=5; i++)
                Marshal.WriteByte(ptr, i, bd_addr[5 - i]);

            Connected.Text = Initialise(ptr).ToString();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ShutDown();
            Connected.Text = "False";
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            NetAvail.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_NETWORK_AVAILABLE_IND); // (BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_NETWORK_AVAILABLE_IND);
            CallerId.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_CLIP_IND);
            Ringing.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_RINGING_IND);
            NetOperator.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_NETWORK_OPERATOR_IND);
            Subscriber.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SUBSCRIBER_NUMBER_IND);
            SignalStrength.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_SIGNAL_STRENGTH_IND);
            BatteryCharge.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_BATTERY_CHARGE_IND);
            ManufacturerId.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_HF_MANUFACTURERID_IND);
            ModelId.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_HF_MODELID_IND);
            OutgoingCall.Text = ReturnCallBacks((ushort)BTSDK_HFP_APP_EventCodeEnum.BTSDK_HFP_EV_OUTGOINGCALL_IND);
        }

        private void Answer_Click(object sender, EventArgs e)
        {
            string result;
            result = Commands('1', "0").ToString();
            MessageBox.Show(result);
        }

        private void Hangup_Click(object sender, EventArgs e)
        {
            string result;
            result = Commands('2', "0").ToString();
            MessageBox.Show(result);
        }

        private void Dial_Click(object sender, EventArgs e)
        {
            string result;
            result = Commands('3', PhoneNumber.Text).ToString();
            MessageBox.Show(result);
        }

        private void GetNetwork_Click(object sender, EventArgs e)
        {
            string result;
            result = Commands('a', "0").ToString();
            MessageBox.Show(result);
        }

        private void SubscriberBut_Click(object sender, EventArgs e)
        {
            string result;
            result = Commands('c', "0").ToString();
            MessageBox.Show(result);
        }
    }
}