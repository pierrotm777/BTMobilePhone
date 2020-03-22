#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
SetFormat, Integer, H
  Locale1=0x4090409  ; Английский (американский).
  Locale2=0x4190419  ; Русский.
  WinGet, WinID,, A
  ThreadID:=DllCall("GetWindowThreadProcessId", "Int", WinID, "Int", "0")
  InputLocaleID:=DllCall("GetKeyboardLayout", "Int", ThreadID)
  if(InputLocaleID=Locale1)
    SendMessage, 0x50,, % Locale2,, A
  else if(InputLocaleID=Locale2)
    SendMessage, 0x50,, % Locale1,, A
Exit