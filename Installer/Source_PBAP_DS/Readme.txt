**********************************************************
The plugin is installed in the folder:
C:\Program Files\TipTop software\iCar DS\Extentions\MobilePhone\
If it not the case, uninstall it and reinstall it.
**********************************************************

**********************************************************
The BTMobilephone plugin installer for iCarDS can add automatically the point 1 to 3.
You need to just install the option 'Установите образец кожи в' !

1.Under C:\Program Files\TipTop software\iCar DS\Language\
Languages files are updated.

2.Under C:\Users\pierre\Documents\iCarDS\skins\Chameleon\LABELS\
a) under LABELS folder
Languages files are updated.
b) Under C:\Users\pierre\Documents\iCarDS\skins\Chameleon\language\
Languages files are updated.

3.Under C:\Users\pierre\Documents\iCarDS\skins\Chameleon\
the files that follow are added or updated.
cf_ind.txt
1_cf_BTMPlayer.txt
cf_ind_sms.txt
cf_labelshow_sms.txt
dinamic_label.ini.fr
ExecTBL_ReadyForMobilePhone.ini
setting.skin
setting2.skin
setting3.skin
setting4.skin
setting5.skin

all MOBILEPHONE_*.skin are added.
**********************************************************

**********************************************************
You need to modify these points:
4.into setting.ini and skin.ini files:
add:(!!! VERY IMPORTANT !!!)
plugin_MobilePhone_is=1

replace:(not absolutely needed)
plugin_RRSkinTool_is=1
by 
plugin_SkinTool_is=1

5.Into ExecTBL.ini file
replace:(!!! VERY IMPORTANT !!!)
"LoadPlugins","BYVAR;plugin_SkinTool_is;<<LoadExt;SkinTools"
by
"LoadPlugins","BYVAR;plugin_SkinTool_is;<<LoadExt;SkinTool"

replace:(!!! VERY IMPORTANT !!!)
"CF_Phoco","Phoco||Set_PhocoMode"
by
"CF_Phoco","MOBILEPHONE"

add:(!!! VERY IMPORTANT !!!)
"LoadPlugins","BYVAR;plugin_MobilePhone_is;<<LoadExt;MobilePhone"
/,--------------- MobilePhone ------------------------
"ONCLCLICK","MOBILEPHONE_SETTINGS_UPDATEINFODEVICE",MobilePhone_settings.skin
"ONCLCLICK","MOBILEPHONE_UPDATECL",MobilePhone.skin
"ONCLDBLCLICK","MOBILEPHONE_FAVORITE_ADD",MobilePhone.skin
"ONCLDBLCLICK","MOBILEPHONE_ADDCOUNTRYPREFIX",MobilePhone_Info.skin
"ONCLCLICK","UPDATE_IMAGE",MobilePhone_SmsModels.skin

After have installed the plugin see if it is well installed under the folder:
C:\Program Files\TipTop software\iCar DS\Extentions\MobilePhone\

**********************************************************
**********************************************************

Кто желает попробовать - для работы плагина нужно сделать небольшие правки в трех файлах :
\Documents\iCarDS\skins\Chameleon\skin.ini
\Documents\iCarDS\skins\Chameleon\setting.ini
\Documents\iCarDS\skins\Chameleon\ExecTBL.ini

Установка MobilePhone Plugin :
1. setting.ini - (открыть в блокноте или любым текстовым редактором) :
добавить строку plugin_MobilePhone_is=1
найти поиском строку (нажав CTRL+F) plugin_RRSkinTool_is=0
заменить на plugin_SkinTool_is=1
(сохранить изменения)
skin.ini - (открыть в блокноте или любым текстовым редактором) :
добавить строку plugin_MobilePhone_is=1
(сохранить изменения)
ExecTBL.ini- (открыть в блокноте или любым текстовым редактором)
найти поиском строку (нажав CTRL+F) "LoadPlugins","BYVAR;plugin_SkinTool_is;<<LoadExt; SkinTools"
заменить на "LoadPlugins","BYVAR;plugin_SkinTool_is;<<LoadExt; SkinTool"
найти поиском строку (нажав CTRL+F) "CF_Phoco","Phoco||Set_PhocoMode"
заменить на "CF_Phoco","MOBILEPHONE"
Добавить строки :
"LoadPlugins","BYVAR;plugin_MobilePhone_is;<<LoadE xt;MobilePhone"
/,--------------- MobilePhone ------------------------
"ONCLCLICK","MOBILEPHONE_SETTINGS_UPDATEINFODEVICE ",MobilePhone_settings.skin
"ONCLCLICK","MOBILEPHONE_UPDATECL",MobilePhone.ski n
"ONCLDBLCLICK","MOBILEPHONE_FAVORITE_ADD",MobilePh one.skin
"ONCLDBLCLICK","MOBILEPHONE_ADDCOUNTRYPREFIX",Mobi lePhone_Info.skin
"ONCLCLICK","UPDATE_IMAGE",MobilePhone_SmsModels.s kin

(сохранить изменения)
2. Сделать установку из шапки :
iCarDS Plugin BTMobilePhone (PBAP & MAP services) V1.x.x DS Setup.zip
c чекбоксами .
3. Открыть "Телефон" в iCar DS => Обновить устройства "Список Устройств" , выбрать своё => "Применить" => "Обновить" базу данных
=> перезагрузить iCar DS и дождаться значка "Телефон" => Открыть "Телефон" и пользоваться.
Все последующие запуски после 10-ти секундного ожидания
