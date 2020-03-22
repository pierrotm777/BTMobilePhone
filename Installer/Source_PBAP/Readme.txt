1-For all skin:
===============
Replace the indicator:
PHONE_CONNECTED_PHOCO
by
MOBILEPHONE_CONNECTED

but PHONE_CONNECTED_PHOCO is also accepted !

2-For DFX5
=====================
edit the execTbl.ini file:
replace:
"MOBILEPHONE","LOAD;MOBILEPHONE.skin"
by
/"MOBILEPHONE","LOAD;MOBILEPHONE.skin"

Into the file TopIndicators.txt :
replace:
B24,388,0,50,35,"LOAD;MOBILEPHONE.skin||WAIT;.5||MOBILEPHONE_PC",
by
B24,388,0,50,35,"MOBILEPHONE||WAIT;.5||MOBILEPHONE_PB",

Into the file skin.ini:
replace:
MenuExec_44=load;mobilephone.skin||WAIT;.5||mobilephone_pc
by
MenuExec_44=MOBILEPHONE||WAIT;.5||MOBILEPHONE_PB

and (option)
IND,mobilephone_connected,Indicators\Phone_Off.png,Indicators\Phone_On.png
by
IND,MOBILEPHONE_CONNECTED,Indicators\BT_00.png,Indicators\BT_01.png


2-For Reborn
=====================
edit the execTbl.ini file:
replace:
"MOBILEPHONE","LOAD;MOBILEPHONE.skin"
by
/"MOBILEPHONE","LOAD;MOBILEPHONE.skin"

3-For eLite 2
=============
RideRunner\Skins\eLite 2\elite\menus\media.applications
add:
LSTMobilePhone||MobilePhone
ICO$SKINPATH$ICONS\MENUS\Phone.PNG

RideRunner\Skins\eLite 2\elite\static\Top-round.elite
add:
I04,720,35,41,27,"MOBILEPHONE_MESSAGERECEIVED",,,"Mobile_Indicators\SMS_*.png"

!!!!!!! DELETE all MOBILEPHONE lines into your exectbl.ini file !!!!!!!!

4-GpsLockOut - see http://www.mp3car.com/rr-released-plugins/136227-gpslockout-updated-11-12-09-a.html
============
If you use this plugin for stop SMS Read or Write
[Settings]
Speed=5
cmds=dvd video mobilephone_smswrite mobilephone_smsread
skins=external_DVD.skin video_player.skin windvd_player.skin
