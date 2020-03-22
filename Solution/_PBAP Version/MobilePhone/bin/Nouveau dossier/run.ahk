;deb=1
#SingleInstance force
SetBatchLines -1 
menu, tray, add, Exit, exit
menu, tray, NoStandard






;Gui, 3: -Caption  +LastFound ; +ToolWindow ;+OwnDialogs  ;+AlwaysOnTop
;Gui, 3: Show,  x200 y150   w1200 h800     ,252542



;gui, 1:+owner3


prefix = %A_ScriptDir%\setting.ini


IniRead, WidthApp, %prefix%, poz, Width 
IniRead, HeightApp, %prefix%, poz, Height 


IniRead, WidthAppd, %prefix%, poz, dWidth 
IniRead, HeightAppd, %prefix%, poz, dHeight 

rrrr_nan = CarPhone


prefix = %A_ScriptDir%\setting.ini

IniRead, xf, %prefix%, poz, xf ,10

IniRead, yf, %prefix%, poz, yf ,10

IniRead, xftt, %prefix%, poz, xf1,10

IniRead, yftt, %prefix%, poz, yf1 ,10



CountDown = 0000  ; 4 digits = mmSS 
T = 20000101%CountDown% 

zagr_stat=0
gosub,styguiyy



truff=0

 stat = 0

iconNamx=
iconpriemp=
yyhytgb=
1yyhytgb=
dd=1

nostartop=1



Menu, Tray, Icon, ico\dark.ico

;settimer,blin,20000

#Include ,include.ahk

blin:
;WinMinimize  ,CarPhone
WinRestore ,CarPhone

return
