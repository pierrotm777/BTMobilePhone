; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
; To add a localization:
; 1) Add the language to the [Languages] section
; 2) Add localizations to the [Messages] section
; 3) Add localizations to the [CustomMessages] section
; 4) Add License Agreement localization
; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

[Setup]
; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
; BEGIN CUSTOMIZATION SECTION: Customize these constants
; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

; Ensure that you use YOUR OWN APP_ID; DO NOT REUSE THIS ONE
#define APP_ID "{{58534807-17E7-4254-8E2A-AAA932A00320}"

#define APP_NAME "My Add-In"
#define APP_VERSION "1.0.0.0"
#define DEFAULT_GROUP_NAME "{cm:MyAddInDefaultGroupName}"
#define HELP_SHORTCUT "{cm:MyAddInHelpFile}"
#define DEST_SUB_DIR "MyAddIn"
#define HELP_FILE_NAME "MyAddIn.chm"
#define LICENSE_AGREEMENT_FILE_NAME "LicenseAgreement.rtf"
#define DLL_FILE_NAME_VS2005 "MyAddInVS2005.dll"
#define DLL_FILE_NAME_VS2008 "MyAddInVS2008.dll"
#define DLL_FILE_NAME_VS2010 "MyAddInVS2010.dll"
#define DLL_FILE_NAME_VS2012 "MyAddInVS2012.dll"
#define ADDIN_XML_FILE_NAME "MyAddIn.AddIn"
#define MY_COMPANY_NAME "My Company"
#define MY_COMPANY_WEB_SITE "www.mycompany.com"
#define OUTPUT_FOLDER_NAME ".\Setup"
#define OUTPUT_BASE_FILE_NAME "MyAddInSetup"
#define COPYRIGHT_YEAR "2008"

; Ensure that these values are used for the Connect class
#define CONNECT_CLASS_FULL_NAME_VS_2005 "MyAddInVS2005.Connect"
#define CONNECT_CLASS_FULL_NAME_VS_2008 "MyAddInVS2008.Connect"
#define CONNECT_CLASS_FULL_NAME_VS_2010 "MyAddInVS2010.Connect"
#define CONNECT_CLASS_FULL_NAME_VS_2012 "MyAddInVS2012.Connect"
; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
; END CUSTOMIZATION SECTION 
; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

OutputDir={#OUTPUT_FOLDER_NAME}
OutputBaseFilename={#OUTPUT_BASE_FILE_NAME}
AppID={#APP_ID}
VersionInfoVersion={#APP_VERSION}
PrivilegesRequired=admin
MinVersion=0,5.0.2195
AppName={#APP_NAME}
AppVerName={#APP_NAME}
DefaultGroupName={#DEFAULT_GROUP_NAME}
AppPublisher={#MY_COMPANY_NAME}
AppPublisherURL={#MY_COMPANY_WEB_SITE}
AppSupportURL={#MY_COMPANY_WEB_SITE}
AppUpdatesURL={#MY_COMPANY_WEB_SITE}
DefaultDirName={pf}\{#DEST_SUB_DIR}
LicenseFile={#LICENSE_AGREEMENT_FILE_NAME}
Compression=lzma
SolidCompression=true
DisableReadyPage=true
ShowLanguageDialog=no
UninstallLogMode=append
DisableProgramGroupPage=true
VersionInfoCompany={#MY_COMPANY_NAME}
AppCopyright=Copyright © {#COPYRIGHT_YEAR} {#MY_COMPANY_NAME}
AlwaysUsePersonalGroup=false
InternalCompressLevel=ultra
AllowNoIcons=true
LanguageDetectionMethod=locale
DisableDirPage=true

[Languages]
; USE ENGLISH AS THE FIRST LANGUAGE!!!
Name: English; MessagesFile: compiler:Default.isl
Name: Spanish; MessagesFile: compiler:Languages\Spanish.isl

[Types]
Name: Custom; Description: Custom; Flags: iscustom

[Files]
Source: {#DLL_FILE_NAME_VS2005}; DestDir: {app}; Flags:ignoreversion; AfterInstall:CreateAddInXMLFileVS2005(); Check: IsMyAddInVS2005Selected()
Source: {#DLL_FILE_NAME_VS2008}; DestDir: {app}; Flags:ignoreversion; AfterInstall:CreateAddInXMLFileVS2008(); Check: IsMyAddInVS2008Selected()
Source: {#DLL_FILE_NAME_VS2010}; DestDir: {app}; Flags:ignoreversion; AfterInstall:CreateAddInXMLFileVS2010(); Check: IsMyAddInVS2010Selected()
Source: {#DLL_FILE_NAME_VS2012}; DestDir: {app}; Flags:ignoreversion; AfterInstall:CreateAddInXMLFileVS2012(); Check: IsMyAddInVS2012Selected()
Source: {#HELP_FILE_NAME}; DestDir: {app}; Flags: ignoreversion

[Icons]
Name: {group}\{cm:MyAddInWebSite}; Filename: http://www.mycompany.com
Name: {group}\{cm:UninstallProgram,{#APP_NAME}}; Filename: {uninstallexe}
Name: {group}\{#HELP_SHORTCUT}; Filename: {app}\{#HELP_FILE_NAME}; IconFilename: {app}\{#HELP_FILE_NAME}

[Messages]
English.FinishedLabel=Setup has finished installing [name] on your computer.
Spanish.FinishedLabel=La instalación de [name] en su ordenador ha finalizado.

English.UninstallStatusLabel=The uninstallation can take up to a minute while the commands of the add-in are removed from Visual Studio.
Spanish.UninstallStatusLabel=La desinstalación puede tardar hasta un minuto mientras los comandos del complemento se eliminan de Visual Studio.

[CustomMessages]
English.MyAddInWebSite={{#APP_NAME} Web Site
Spanish.MyAddInWebSite=Sitio web de {#APP_NAME}

English.VSNeedsToBeClosed=Close Visual Studio before continuing.
Spanish.VSNeedsToBeClosed=Cierre Visual Studio antes de continuar.

English.RegisteringAddIn=Registering the add-in...
Spanish.RegisteringAddIn=Registrando el complemento...

English.MyAddInDefaultGroupName={#APP_NAME} for Visual Studio
Spanish.MyAddInDefaultGroupName={#APP_NAME} para Visual Studio

English.MyAddInHelpFile={#APP_NAME} Help
Spanish.MyAddInHelpFile=Ayuda de {#APP_NAME} (en inglés)

English.VSRequired=Visual Studio Standard Edition or higher is required to install this product.
Spanish.VSRequired=Se requiere Visual Studio Standard Edition o superior para instalar este producto.

[Code]
var
   IDEsPage: TWizardPage;
   IDEsCheckListBox: TNewCheckListBox;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function GetXMLAddInFullFolderName(Param: String): String;
var
   sCommonAppDataFolder: String;
   sResult: String;
begin
   sCommonAppDataFolder := ExpandConstant('{commonappdata}');

   (* Param is the version such as '8.0' or '9.0' *)
   sResult := sCommonAppDataFolder + '\Microsoft\VisualStudio\' + Param + '\Addins\'

   Result := sResult;
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
procedure CreateAddInXMLFile(sVersion: String; sConnectClassFullName: String; sDLLFileName: String);
var
   sLines: TArrayOfString;
   sXMLAddInFullFileName: String;
   sAddInDLLFullFileName: String;
   sInstallationFolder: String;
   sFolder: String;
begin

   (* Compose the full name of the add-in DLL *)
   sInstallationFolder := ExpandConstant('{app}');
   sAddInDLLFullFileName := sInstallationFolder + '\' + sDLLFileName;

   (* Get the folder where to put the .AddIn XML registration file *)
   sFolder := GetXMLAddInFullFolderName(sVersion);

   (* Ensure that the folder is created *)
   CreateDir(sFolder);

   (* Compose the full name of the .AddIn XML registration file *)
   sXMLAddInFullFileName := sFolder + '{#ADDIN_XML_FILE_NAME}';

   (* Create the .AddIn XML registration file *)
   SetArrayLength(sLines,16);
   
   sLines[0]  := '<?xml version="1.0" encoding="windows-1252" standalone="no"?>';
   sLines[1]  := '<Extensibility xmlns="http://schemas.microsoft.com/AutomationExtensibility">';
   sLines[2]  := '   <HostApplication>';
   sLines[3]  := '      <Name>Microsoft Visual Studio</Name>';
   sLines[4]  := '      <Version>' + sVersion + '</Version>';
   sLines[5]  := '   </HostApplication>';
   sLines[6]  := '   <Addin>';
   sLines[7]  := '      <FriendlyName>{#APP_NAME}</FriendlyName>';
   sLines[8]  := '      <Description>{#APP_NAME}</Description>';
   sLines[9]  := '      <Assembly>' + sAddInDLLFullFileName + '</Assembly>';
   sLines[10] := '      <FullClassName>' + sConnectClassFullName + '</FullClassName>';
   sLines[11] := '      <LoadBehavior>3</LoadBehavior>';
   sLines[12] := '      <CommandPreload>0</CommandPreload>';
   sLines[13] := '      <CommandLineSafe>0</CommandLineSafe>';
   sLines[14] := '   </Addin>';
   sLines[15] := '</Extensibility>';

   SaveStringsToFile(sXMLAddInFullFileName, sLines, False);

end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
procedure CreateAddInXMLFileVS2005();
begin
   CreateAddInXMLFile('8.0','{#CONNECT_CLASS_FULL_NAME_VS_2005}', '{#DLL_FILE_NAME_VS2005}')
end ;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
procedure CreateAddInXMLFileVS2008();
begin
   CreateAddInXMLFile('9.0','{#CONNECT_CLASS_FULL_NAME_VS_2008}', '{#DLL_FILE_NAME_VS2008}')
end ;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
procedure CreateAddInXMLFileVS2010();
begin
   CreateAddInXMLFile('10.0','{#CONNECT_CLASS_FULL_NAME_VS_2010}', '{#DLL_FILE_NAME_VS2010}')
end ;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
procedure CreateAddInXMLFileVS2012();
begin
   CreateAddInXMLFile('11.0','{#CONNECT_CLASS_FULL_NAME_VS_2012}', '{#DLL_FILE_NAME_VS2012}')
end ;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
procedure ShowCloseVisualStudioMessage();
var
   sMsg: String;
begin
   sMsg := CustomMessage('VSNeedsToBeClosed');
   MsgBox(sMsg, mbCriticalError, mb_Ok)
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
procedure ShowVisualStudioRequiredMessage();
var
   sMsg: String;
begin
   sMsg := CustomMessage('VSRequired');
   MsgBox(sMsg, mbCriticalError, mb_Ok)
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsVSInstalledByProgID(ProgID: String):Boolean;
begin

   if RegKeyExists(HKCR, ProgID) then
      Result := True
   else
      Result := False

end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsVSRunningByProgID(ProgID: String):Boolean;
var
   IDE: Variant;
begin

   try
      IDE := GetActiveOleObject(ProgID);
   except
   end;

   if VarIsEmpty(IDE) then
      Result := False
   else
      Result := True
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsVS2005Installed():Boolean;
begin
   if IsVSInstalledByProgID('VisualStudio.DTE.8.0') then
      Result := True
   else
      Result := False
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsVS2008Installed():Boolean;
begin
   if IsVSInstalledByProgID('VisualStudio.DTE.9.0') then
      Result := True
   else
      Result := False
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsVS2010Installed():Boolean;
begin
   if IsVSInstalledByProgID('VisualStudio.DTE.10.0') then
      Result := True
   else
      Result := False
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsVS2012Installed():Boolean;
begin
   if IsVSInstalledByProgID('VisualStudio.DTE.11.0') then
      Result := True
   else
      Result := False
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsVSInstalled():Boolean;
begin

   if IsVS2005Installed() then
      Result := True
   else if IsVS2008Installed() then
      Result := True
   else if IsVS2010Installed() then
      Result := True
   else if IsVS2012Installed() then
      Result := True
   else
      begin
         Result := False
         ShowVisualStudioRequiredMessage();
      end

end;
(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsSomeVSRunning():Boolean;
begin

   if IsVSRunningByProgID('VisualStudio.DTE.8.0') then
      Result := True
   else if IsVSRunningByProgID('VisualStudio.DTE.9.0') then
        Result := True
   else if IsVSRunningByProgID('VisualStudio.DTE.10.0') then
      Result := True
   else if IsVSRunningByProgID('VisualStudio.DTE.11.0') then
      Result := True
   else
      Result := False

end;
(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
procedure UnregisterAddInIfInstalled(sVersion: String; sConnectClassFullName: String);
var
   sXMLAddInFullFileName: String;
   sFolder: String;
   sIDEFullFileName: String;
   iResultCode: Integer;
begin

   (* Compose the full name of the .AddIn XML registration file *)
   sFolder := GetXMLAddInFullFolderName(sVersion);
   sXMLAddInFullFileName := sFolder + '{#ADDIN_XML_FILE_NAME}';

   if FileExists(sXMLAddInFullFileName) then
      begin
         (* Delete the .AddIn XML registration file to unregister the add-in *)
         DeleteFile(sXMLAddInFullFileName);

         (* Compose the full name of the Visual Studio IDE *)
         sIDEFullFileName := ExpandConstant('{reg:HKLM\SOFTWARE\Microsoft\VisualStudio\' + sVersion + ',InstallDir}') + 'devenv.exe'

         (* Remove the commands of the IDE *)
         Exec(sIDEFullFileName, '/ResetAddin ' + sConnectClassFullName + ' /command File.Exit', '',SW_HIDE,ewWaitUntilTerminated,iResultCode);

      end

end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsMyAddInVS2005Selected():Boolean;
begin
   Result := IDEsCheckListBox.Checked[0]
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsMyAddInVS2008Selected():Boolean;
begin
   Result := IDEsCheckListBox.Checked[1]
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsMyAddInVS2010Selected():Boolean;
begin
   Result := IDEsCheckListBox.Checked[2]
end;

(***************************************************************************************************)
(* Auxiliar function                                                                               *)
(***************************************************************************************************)
function IsMyAddInVS2012Selected():Boolean;
begin
   Result := IDEsCheckListBox.Checked[3]
end;

(***************************************************************************************************)
(* InnoSetup event function                                                                        *)
(***************************************************************************************************)
function InitializeSetup(): Boolean;
begin

   if not IsVSInstalled() then
      begin
         Result := False;
      end
   else
      begin
         if IsSomeVSRunning() then
            begin
               ShowCloseVisualStudioMessage()
               Result := False;
            end
         else
            Result := True;
      end

end;

(***************************************************************************************************)
(* InnoSetup event function                                                                        *)
(***************************************************************************************************)
procedure InitializeWizard();
var
   IDEsLabel: TLabel;
   bIsVS2005Installed: Boolean;
   bIsVS2008Installed: Boolean;
   bIsVS2010Installed: Boolean;
   bIsVS2012Installed: Boolean;

begin   IDEsPage := CreateCustomPage(wpSelectComponents, SetupMessage(msgWizardSelectComponents), SetupMessage(msgSelectComponentsDesc));

   IDEsLabel := TLabel.Create(IDEsPage);
   IDEsLabel.Caption := SetupMessage(msgSelectComponentsLabel2);
   IDEsLabel.Width := IDEsPage.SurfaceWidth;
   IDEsLabel.Height := ScaleY(40);
   IDEsLabel.AutoSize := False;
   IDEsLabel.WordWrap := True;
   IDEsLabel.Parent := IDEsPage.Surface;

   IDEsCheckListBox := TNewCheckListBox.Create(IDEsPage);
   IDEsCheckListBox.Top := IDEsLabel.Top + IDEsLabel.Height + ScaleY(8);
   IDEsCheckListBox.Width := IDEsPage.SurfaceWidth;
   IDEsCheckListBox.Height := ScaleX(100);
   IDEsCheckListBox.Flat := True;
   IDEsCheckListBox.Parent := IDEsPage.Surface;

   bIsVS2005Installed := IsVS2005Installed();
   bIsVS2008Installed := IsVS2008Installed();
   bIsVS2010Installed := IsVS2010Installed();
   bIsVS2012Installed := IsVS2012Installed();

   IDEsCheckListBox.AddCheckBox('{#APP_NAME} -  Visual Studio 2005', '', 0, bIsVS2005Installed, bIsVS2005Installed, False, True, nil)
   IDEsCheckListBox.AddCheckBox('{#APP_NAME} -  Visual Studio 2008', '', 0, bIsVS2008Installed, bIsVS2008Installed, False, True, nil)
   IDEsCheckListBox.AddCheckBox('{#APP_NAME} -  Visual Studio 2010', '', 0, bIsVS2010Installed, bIsVS2010Installed, False, True, nil)
   IDEsCheckListBox.AddCheckBox('{#APP_NAME} -  Visual Studio 2012', '', 0, bIsVS2012Installed, bIsVS2012Installed, False, True, nil)

end;
(***************************************************************************************************)
(* InnoSetup event function                                                                        *)
(***************************************************************************************************)
function InitializeUninstall(): Boolean;
begin

   if IsSomeVSRunning() then
      begin
         ShowCloseVisualStudioMessage()
         Result := False;
      end
   else
       begin
         Result := True;
      end
end;

(***************************************************************************************************)
(* InnoSetup event function                                                                        *)
(***************************************************************************************************)
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
   if CurUninstallStep = usUninstall then
      begin
         UnregisterAddInIfInstalled('8.0', '{#CONNECT_CLASS_FULL_NAME_VS_2005}' );
         UnregisterAddInIfInstalled('9.0', '{#CONNECT_CLASS_FULL_NAME_VS_2008}' );
         UnregisterAddInIfInstalled('10.0', '{#CONNECT_CLASS_FULL_NAME_VS_2010}' );
         UnregisterAddInIfInstalled('11.0', '{#CONNECT_CLASS_FULL_NAME_VS_2012}' );
      end;
end;