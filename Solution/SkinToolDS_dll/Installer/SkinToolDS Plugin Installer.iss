﻿; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

; EDIT THESE LINES
#define iCarDSExtPluginName "SkinToolDS"
#define iCarDSExtPluginFriendlyName "SkinToolDS"
#define iCarDSExtPluginVersion "V0.0.1"
#define iCarDSExtPluginPublisher "P. Montet"
#define iCarDSExtURL ""

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{90AEE502-68E6-4A27-9AED-66662AB55F04}
AppName=iCarDS Plugin for {#iCarDSExtPluginFriendlyName}
AppVerName=iCarDS Plugin for {#iCarDSExtPluginFriendlyName} {#iCarDSExtPluginVersion}
AppPublisher={#iCarDSExtPluginPublisher}
AppPublisherURL={#iCarDSExtURL}
AppSupportURL={#iCarDSExtURL}
AppUpdatesURL={#iCarDSExtURL}
DefaultDirName={reg:HKLM\Software\iCarDS,Path|{pf}\TipTop software\iCar DS}\Extentions\{#iCarDSExtPluginName}
OutputDir=..\Installer Output
OutputBaseFilename=iCarDS Plugin {#iCarDSExtPluginFriendlyName} {#iCarDSExtPluginVersion} Setup
SourceDir=Source
Compression=lzma
SolidCompression=yes
Uninstallable=yes
;LicenseFile=License.txt
;PrivilegesRequired=admin
UsePreviousTasks=no

[Languages]
Name: en;    MessagesFile: "compiler:Default.isl"
Name: fr;   MessagesFile: "compiler:Languages\French.isl"
Name: de;    MessagesFile: "compiler:Languages\German.isl"
Name: fi;    MessagesFile: "compiler:Languages\Finnish.isl"
Name: it;    MessagesFile: "compiler:Languages\Italian.isl"
Name: pl;    MessagesFile: "compiler:Languages\Polish.isl"
Name: pt_br; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: ru;    MessagesFile: "compiler:Languages\Russian.isl"

#include <idp.iss>
; Language files must be included after idp.iss and after [Languages] section
#include <idplang\French.iss>
#include <idplang\German.iss>
#include <idplang\Finnish.iss>
#include <idplang\Italian.iss>
#include <idplang\Polish.iss>
#include <idplang\BrazilianPortuguese.iss>
#include <idplang\Russian.iss>

; Let's change some of standard strings:
[CustomMessages]
; en.InstallChameleon=Install the Sample Skin into
; fr.InstallChameleon=Installer les écrans dans
; de.InstallChameleon=Installieren Sie die Beispiel Haut in
; fi.InstallChameleon=Asenna Sample Skin osaksi
; it.InstallChameleon=Installare AddOns ( di lingua francese e un altro impostazioni dello schermo ) in
; pl.InstallChameleon=Zainstalować skórki próbki do
; pt_br.InstallChameleon=Instale a pele da amostra em
; ru.InstallChameleon=Установка скина MobilePhone в

; en.InstallAddOnsChameleon=Install AddOns(French Language and another settings screen) into
; fr.InstallAddOnsChameleon=Installer les ajouts(Language français et écrans config. supp.) dans
; de.InstallAddOnsChameleon=Installieren Sie AddOns ( Französisch Sprache und weitere Einstellungen Bildschirm ) in
; fi.InstallAddOnsChameleon=Asenna AddOns ( Ranskan kieli ja toinen asetusruutu ) osaksi
; it.InstallAddOnsChameleon=Installare AddOns ( di lingua francese e un altro impostazioni dello schermo ) in
; pl.InstallAddOnsChameleon=Instalowanie AddOns ( język francuski i kolejne ustawienia ekranu ) do
; pt_br.InstallAddOnsChameleon=Instalar complementos ( língua francesa e outras definições do ecrã ) para
; ru.InstallAddOnsChameleon=Установка дополнений (Русского языка и экрана настроек) в

en.UpdateChameleonIniFiles=Update Files .ini(setting.ini, skin.ini and ExecTBL.ini) into
fr.UpdateChameleonIniFiles=Mise à jour fichiers .ini(setting.ini, skin.ini and ExecTBL.ini) dans
de.UpdateChameleonIniFiles=Installieren Sie AddOns (setting.ini, skin.ini and ExecTBL.ini) in
fi.UpdateChameleonIniFiles=Asenna AddOns (setting.ini, skin.ini and ExecTBL.ini) osaksi
it.UpdateChameleonIniFiles=Installare AddOns (setting.ini, skin.ini and ExecTBL.ini) in
pl.UpdateChameleonIniFiles=Instalowanie AddOns (setting.ini, skin.ini and ExecTBL.ini) do
pt_br.UpdateChameleonIniFiles=Instalar complementos (setting.ini, skin.ini и ExecTBL.ini) para
ru.UpdateChameleonIniFiles=Обновление файлов конфигурации (setting.ini, skin.ini и ExecTBL.ini) в

en.FullInstall=Full installation
fr.FullInstall=Installation complète
de.FullInstall=Volledige installatie
fi.FullInstall=Täysi asennus
it.FullInstall=Installazione completa
pl.FullInstall=Pełna instalacja
pt_br.FullInstall=Instalação completa
ru.FullInstall=Полная установка
;*********************************************************
; en.SkinOnly=Install the Sample Skin into
; fr.SkinOnly=Installer les écrans dans
; de.SkinOnly=Installieren Sie die Beispiel Haut in
; fi.SkinOnly=Asenna Sample Skin osaksi
; it.SkinOnly=Installare AddOns in
; pl.SkinOnly=Zainstalować skórki próbki do
; pt_br.SkinOnly=Instale a pele da amostra em
; ru.SkinOnly=Установка скина MobilePhone в

; en.AddOnOnly=Install AddOns (French Language and another settings screen) into
; fr.AddOnOnly=Installer les ajouts (Language français et écrans config. supp.) dans
; de.AddOnOnly=Installieren Sie AddOns ( Französisch Sprache und weitere Einstellungen Bildschirm ) in
; fi.AddOnOnly=Asenna AddOns ( Ranskan kieli ja toinen asetusruutu ) osaksi
; it.AddOnOnly=Installare AddOns ( di lingua francese e un altro impostazioni dello schermo ) in
; pl.AddOnOnly=Instalowanie AddOns ( język francuski i kolejne ustawienia ekranu ) do
; pt_br.AddOnOnly=Instalar complementos ( língua francesa e outras definições do ecrã ) para
; ru.AddOnOnly=Установка дополнений (Русского языка и экрана настроек) в

en.EditSettingsOnly=Update .ini files (setting.ini, skin.ini and ExecTBL.ini) into
fr.EditSettingsOnly=Mise à jour fichiers .ini(setting.ini, skin.ini et ExecTBL.ini) dans
de.EditSettingsOnly=Update bestanden .ini (setting.ini, skin.ini en ExecTBL.ini) in
fi.EditSettingsOnly=Päivitys tiedostoja .ini (setting.ini, skin.ini ja ExecTBL.ini) osaksi
it.EditSettingsOnly=I file di aggiornamento .ini (setting.ini, skin.ini e ExecTBL.ini) in
pl.EditSettingsOnly=Aktualizacja plików .ini (setting.ini, skin.ini i ExecTBL.ini) w
pt_br.EditSettingsOnly=Arquivos de atualização .ini (setting.ini, skin.ini и ExecTBL.ini) para
ru.EditSettingsOnly=Обновление файлов конфигурации (setting.ini, skin.ini и ExecTBL.ini) в

[Types]
Name: "full"; Description: "{cm:FullInstall}"; Languages: de en fi fr it pl pt_br ru
; Name: "skinfiles"; Description: "{cm:SkinOnly} Chameleon"; Languages: de en fi fr it pl pt_br ru
; Name: "addonfiles"; Description: "{cm:AddOnOnly} Chameleon"; Languages: de en fi fr it pl pt_br ru
Name: "updateini"; Description: "{cm:EditSettingsOnly} Chameleon"; Languages: de en fi fr it pl pt_br ru

[Components]
Name: "full"; Description: "{cm:FullInstall}"; Types: full updateini
; Name: "skinfiles"; Description: "{cm:SkinOnly}"; Types: skinfiles
; Name: "addonfiles"; Description: "{cm:AddOnOnly}"; Types: addonfiles
Name: "updateini"; Description: "{cm:EditSettingsOnly}"; Types: updateini

[Tasks]
; Name: "SSCDL"; Description: "{cm:InstallChameleon} Chameleon"; Languages: de en fi fr it pl pt_br ru; Components: skinfiles
; Name: "SSCDL2"; Description: "{cm:InstallAddOnsChameleon} Chameleon"; Languages: de en fi fr it pl pt_br ru; Components: addonfiles
Name: "SSCDL3"; Description: "{cm:UpdateChameleonIniFiles} Chameleon"; Languages: de en fi fr it pl pt_br ru; Components: updateini

[Files]
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "SkinToolDS.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "Readme.txt"; DestDir: "{app}"; Flags: ignoreversion isreadme
Source: "SkinToolDS Skin Commands.txt"; DestDir: "{app}"; Flags: ignoreversion isreadme
;Source: "History.txt"; DestDir: "{app}"; Flags: ignoreversion isreadme
;Source: "SkinToolDS.exe"; DestDir: "{app}"; Flags: ignoreversion

;Tools
; Source: "Tools\*"; DestDir: "{app}\Tools"; Flags: ignoreversion recursesubdirs createallsubdirs
; Source: "Photo\*"; DestDir: "{app}\Photo"; Flags: ignoreversion recursesubdirs createallsubdirs
; Source: "Messages\*"; DestDir: "{app}\Messages"; Flags: ignoreversion recursesubdirs createallsubdirs
; Source: "Languages\*"; DestDir: "{app}\Languages"; Flags: ignoreversion recursesubdirs createallsubdirs

;skin files
; Source: "Sample Skin_PBAP_DS\*"; DestDir: "{app}\Sample Skin"; Flags: ignoreversion recursesubdirs createallsubdirs; Components: full skinfiles; Languages: de en fi fr it pl pt_br ru
; Source: "Sample Skin_PBAP_DS\Chameleon\*"; DestDir: "{code:DataPath}\Skins\Chameleon"; Flags: ignoreversion recursesubdirs createallsubdirs; Components: full addonfiles; Languages: de en fi fr it pl pt_br ru
; Source: "Sample Skin_PBAP_DS\Chameleon_AddOns\*"; DestDir: "{code:DataPath}\Skins\Chameleon"; Flags: ignoreversion recursesubdirs createallsubdirs; Components: full addonfiles; Languages: de en fi fr it pl pt_br ru
; Source: "Sample Skin_PBAP_DS\iCarDS_AddOns\*"; DestDir: "{app}\..\..\Language"; Flags: ignoreversion recursesubdirs createallsubdirs; Components: full addonfiles; Languages: de en fi fr it pl pt_br ru

[Run]
;Regasm
Filename: "{dotnet20}\Regasm.exe"; Parameters: "/tlb /codebase SkinToolDS.dll"; WorkingDir: "{app}"; Flags: runhidden; StatusMsg: "Registering Plugin..."
;update settings chameleon files
Filename: "{code:DataPath}\Skins\Chameleon\ExecTBL.ini"; Flags: runhidden shellexec postinstall; StatusMsg: "{cm:UpdateChameleonIniFiles} Chameleon"; Languages: de en fi fr it pl pt_br ru; Components: updateini
Filename: "{code:DataPath}\Skins\Chameleon\setting.ini"; Flags: runhidden shellexec postinstall; StatusMsg: "{cm:UpdateChameleonIniFiles} Chameleon"; Languages: de en fi fr it pl pt_br ru; Components: updateini
Filename: "{code:DataPath}\Skins\Chameleon\skin.ini"; Flags: runhidden shellexec postinstall; StatusMsg: "{cm:UpdateChameleonIniFiles} Chameleon"; Languages: de en fi fr it pl pt_br ru; Components: updateini

[UninstallRun]
; Regasm
Filename: {dotnet20}\Regasm.exe; Parameters: /u /tlb SkinToolDS.dll; WorkingDir: {app}; StatusMsg: Un-Registering Plugin...; Flags: runhidden


[UninstallDelete]
Name: {app}\SkinToolDS.tlb; Type: files
Name: {app}\SkinToolDS.exe; Type: files


[Code]
/////////////////////////////////
// Get PLUG-IN Installation Path
/////////////////////////////////
function InstallPath(p: String): String;
begin
	// look for iCarDS.exe?
	if FileExists(ExpandConstant('{app}\TipTop software\iCar DS\iCarDS.exe')) then
	begin
		if DirExists(ExpandConstant('{app}\TipTop software\iCar DS\Extensions')) then
			Result := ExpandConstant('{app}\TipTop software\iCar DS\Extensions\{#iCarDSExtPluginName}')
		else
			Result := ExpandConstant('{app}')
	end
	else
		Result := ExpandConstant('{app}')
end;


/////////////////
// Get DATA Path
/////////////////
function DataPath(p: String): String;
begin
	// look for rr.ini?
	//if FileExists(ExpandConstant('{userdocs}\iCarDS\iCarDS.lic')) then
	//begin
	//	if DirExists(ExpandConstant('{userdocs}\iCarDS')) then
			Result := ExpandConstant('{userdocs}\iCarDS')
	//	else
	//		Result := ExpandConstant('{reg:HKLM\Software\iCarDS,Path|{pf}\iCarDS}')
	//end
	//else
	//	Result := ExpandConstant('{reg:HKLM\Software\iCarDS,Path|{pf}\iCarDS}')
end;


function FileReplaceString(const FileName, SearchString, ReplaceString: string):boolean;
var
  MyFile : TStrings;
  MyText : string;
begin
  MyFile := TStringList.Create;

  try
    result := true;

    try
      MyFile.LoadFromFile(FileName);
      MyText := MyFile.Text;

      if StringChangeEx(MyText, SearchString, ReplaceString, True) > 0 then //Only save if text has been changed.
      begin;
        MyFile.Text := MyText;
        MyFile.SaveToFile(FileName);
      end;
    except
      result := false;
    end;
  finally
    MyFile.Free;
  end;
end;


const
   LF = #10;
   CR = #13;
   CRLF = CR + LF;

function UpdateExecTBLIni(): boolean;
var
  fileName : string;
  lines : TArrayOfString;
begin
  Result := true;
  fileName := ExpandConstant('{code:DataPath}\Skins\Chameleon\ExecTBL.ini');
  FileCopy(fileName, fileName + '.bakBySkinToolDS',False);

  SetArrayLength(lines, 2);
  lines[0] := CRLF + '/,--------------- SkinToolDS ------------------------';
  lines[1] := '"LoadPlugins","BYVAR;plugin_SkinToolDS_is;<<LoadExt;SkinToolDS"';
  Result := SaveStringsToFile(filename,lines,true);
  exit;
end;

function UpdateSettingIni(): boolean;
var
  fileName : string;
  lines : TArrayOfString;
begin
  Result := true;
  fileName := ExpandConstant('{code:DataPath}\Skins\Chameleon\setting.ini');
  FileCopy(fileName, fileName + '.bakBySkinToolDS',False);
  SetArrayLength(lines, 1);
  lines[0] := 'plugin_SkinToolDS_is=1';
  Result := SaveStringsToFile(filename,lines,true);
  exit;
end;

function UpdateSkinIni(): boolean;
var
  fileName : string;
  lines : TArrayOfString;
begin
  Result := true;
  fileName := ExpandConstant('{code:DataPath}\Skins\Chameleon\skin.ini');  FileCopy(fileName, fileName + '.bakBySkinToolDS',False);
  FileReplaceString(fileName, '[END]', '');
  SetArrayLength(lines, 2);
  lines[0] := 'plugin_SkinToolDS_is=1';
  lines[1] := '[END]';
  Result := SaveStringsToFile(filename,lines,true);
  exit;
end;

//run update settings files after  postinstall
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if  CurStep=ssPostInstall then
    begin
         UpdateExecTBLIni();//update ExecTBL.ini file
         UpdateSettingIni();//update setting.ini file         UpdateSkinIni();//update skin.ini file
    end
end;


//check .net framework version 3.5
const
  dotnet35URL = 'http://download.microsoft.com/download/7/0/3/703455ee-a747-4cc8-bd3e-98a615c3aedb/dotNetFx35setup.exe';
 
function InitializeSetup(): Boolean;
//var
  //msgRes : integer;
  //errCode : integer;
 
begin
  Result := true;
  // Check for required dotnetfx 3.5 installation
  if (Not RegKeyExists(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5')) then begin
    MsgBox('The MobilePhone plugin for iCarDS requires'#13 
            'Microsoft .NET Framework 3.5.'#13#13
            'Please use Windows Update to install this version,'#13
            'and then re-run the Setup program.', mbInformation, MB_OK);
//    msgRes := MsgBox(CustomMessage('DotNET'), mbError, MB_OKCANCEL);
//     if(msgRes = 1) then begin
//       ShellExec('Open', dotnet35URL, '', '', SW_SHOW, ewNoWait, errCode);
//     end;
     Abort();

  end;
end;