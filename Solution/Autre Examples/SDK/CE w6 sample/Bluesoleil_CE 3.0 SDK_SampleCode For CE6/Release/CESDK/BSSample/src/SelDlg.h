
#ifndef _SELECT_DIALOG_H
#define _SELECT_DIALOG_H

#include <stdio.h>
#include <stdlib.h>
#include <windows.h>

#ifdef __cplusplus
extern "C" {
#endif

unsigned char OnSelDlg(HWND hWnd, LPCTSTR sTitle, LPCTSTR sPromptText, char *sExcCmd, int sExcCmdLen);

#ifdef __cplusplus
}
#endif

#endif