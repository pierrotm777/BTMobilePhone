; ���������:
$time_to_down = 500		; ����������������� �������� ��������� (��)


; iCarDS ������� ��� ���?
$iCarDS_is_running = ProcessExists("iCarDS.exe")

; ���� iCarDS �������, ��...
If ($iCarDS_is_running) Then
   $SDK = ObjCreate("RideRunner.sdk")
   $CurrentVolume = StringReplace($SDK.getinfo("VOLUME"), "%", "")

   ; ��������� ��������� �� 0 � ������� ��������� �������
   $sleep_down = $time_to_down / $CurrentVolume
   For $n = $CurrentVolume To 0 Step -1
	  $SDK.Execute("SETVOL;Master;" & $n)
	  Sleep($sleep_down)
   Next

   ; ���������������� ��������������� �����/�����
   $SDK.Execute("PAUSE")

   ; ����� �� ������-�� ��������� ���������
   Sleep(50)

   ; ������������� ��������� �����������
   $NotifyVolume = $CurrentVolume
   $SDK.Execute("SETVOL;Master;" & $NotifyVolume)
EndIf

; ��������������� ����� �����������
_WMP_SoundPlay("C:\Windows\Media\Tinkerbell.ogg")

; ���� iCarDS �������, ��...
If ($iCarDS_is_running) Then
   ; ��������������� ���������
   $SDK.Execute("SETVOL;Master;" & $CurrentVolume)
EndIf


Func _WMP_SoundPlay($sFile)
   $oWMP = ObjCreate("WMPlayer.OCX.7")
   $oWMP.Url = $sFile
   $oWMP.Settings.Volume = 100
   $oWMP.Controls.Play

   Do
   Until $oWMP.PlayState = 1
EndFunc