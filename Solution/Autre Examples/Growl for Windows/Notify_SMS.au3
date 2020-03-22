; НАСТРОЙКИ:
$time_to_down = 500		; Продолжительность снижения громкости (мс)
$time_to_up   = 1000	; Продолжительность увеличения громкости (мс)


; iCarDS запущен или нет?
$iCarDS_is_running = ProcessExists("iCarDS.exe")

; Если iCarDS запущен, то...
If ($iCarDS_is_running) Then
   $SDK = ObjCreate("RideRunner.sdk")
   $CurrentVolume = StringReplace($SDK.getinfo("VOLUME"), "%", "")

   ; Уменьшаем громкость до 0 в течение заданного времени
   $sleep_down = $time_to_down / $CurrentVolume

   For $n = $CurrentVolume To 0 Step -1
	  $SDK.Execute("SETVOL;Master;" & $n)
	  Sleep($sleep_down)
   Next

   ; Приостанавливаем воспроизведение аудио/видео
   $SDK.Execute("PAUSE")

   ; Пауза от какого-то звукового артефакта
   Sleep(50)

   ; Устанавливаем громкость уведомления
   $NotifyVolume = $CurrentVolume
   $SDK.Execute("SETVOL;Master;" & $NotifyVolume)
EndIf

; Воспроизведение звука уведомления
_WMP_SoundPlay("C:\Windows\Media\Tinkerbell.ogg")

; Если iCarDS запущен, то...
If ($iCarDS_is_running) Then
   ; Нулевая громкость
   $SDK.Execute("SETVOL;Master;0")

   ; Возобновляем воспроизведение аудио/видео
   $SDK.Execute("RESUME")

   ; Восстанавливаем громкость до исходного значения в течение заданного времени
   $sleep_up = $time_to_up / $CurrentVolume
   For $n = 0 To $CurrentVolume Step 1
	  $SDK.Execute("SETVOL;Master;" & $n)
	  Sleep($sleep_up)
   Next
EndIf


Func _WMP_SoundPlay($sFile)
   $oWMP = ObjCreate("WMPlayer.OCX.7")
   $oWMP.Url = $sFile
   $oWMP.Settings.Volume = 100
   $oWMP.Controls.Play

   Do
   Until $oWMP.PlayState = 1
EndFunc