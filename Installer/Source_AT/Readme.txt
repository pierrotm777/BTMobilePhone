1-For all skin:
===============
Replace the indicator:
PHONE_CONNECTED_PHOCO
by
MOBILEPHONE_CONNECTED


2-For DFX5 and Reborn
=====================
edit the execTbl.ini file:
replace:
"MOBILEPHONE","LOAD;MOBILEPHONE.skin"
by
/"MOBILEPHONE","LOAD;MOBILEPHONE.skin"


2-For eLite 2
=============
RideRunner\Skins\eLite 2\elite\menus\media.applications
add:
LSTMobilePhone||MobilePhone
ICO$SKINPATH$ICONS\MENUS\Phone.PNG

RideRunner\Skins\eLite 2\elite\static\Top-round.elite
add:
I04,720,35,41,27,"MOBILEPHONE_MESSAGERECEIVED",,,"Mobile_Indicators\SMS_*.png"

3-GpsLockOut - see http://www.mp3car.com/rr-released-plugins/136227-gpslockout-updated-11-12-09-a.html
============
If you use this plugin for stop SMS Read or Write
[Settings]
Speed=5
cmds=dvd video mobilephone_smswrite mobilephone_smsread
skins=external_DVD.skin video_player.skin windvd_player.skin


Documentation:
==============
FD	SIM fix-dialling-phonebook
LD	SIM last-dialling-phonebook 
ME	ME phonebook
MT	Combined ME and SIM phonebook. Not supported
SM	SIM phonebook
TA	TA phonebook
DC	ME dialled calls list
RC	ME received calls list
MC	ME missed calls list
MV	ME voice activated dialling list
GR	Group list. Ericsson specific, not supported
HP	Hierarchical phonebook. Ericsson specific
BC	Own business card. Protected by phone lock code.Ericsson specific
SM	SIM/UICC phonebook. If a SIM card is present or if 
	a UICC with an active GSM application is present, 
	the EFADN under DFTelecom is selected. If a UICC 
	with an active USIM application is present, the glo-
	bal phonebook, DFPHONEBOOK under DFTelecom is selected.
	Not supported
EN	Emergency number. Not supported
CN	SIM (or ME) own numbers (MSISDNs) list (reading 
	of this storage may be available through +CNUM 
	also). When storing information in the SIM/UICC, if 
	a SIM card is present or if a UICC with an active 
	GSM application is present, the information in 
	EFMSISDN under DFTelecom is selected. If a UICC 
	with an active USIM application is present, the 
	information in EFMSISDN under ADFUSIM is selected.
	Not supported
AP	Selected application phonebook. If a UICC with an 
	active USIM application is present, the application 
	phonebook, DFPHONEBOOK under ADFUSIM is 
	selected.
	Not supported
