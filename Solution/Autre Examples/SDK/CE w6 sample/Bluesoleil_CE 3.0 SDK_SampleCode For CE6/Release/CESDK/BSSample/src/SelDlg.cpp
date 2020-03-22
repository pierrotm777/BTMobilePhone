/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
*
* Copyright (c) 1999-2008 IVT Corporation
*
* All rights reserved.
* 
---------------------------------------------------------------------------*/

/////////////////////////////////////////////////////////////////////////////
// Module Name:
//    
// Abstract:
//     
// Usage:
//     
// 
// Author:    
//     chenjinfeng
// Revision History:
//     2008-10-15		Created
// 
/////////////////////////////////////////////////////////////////////////////

#include <windows.h>
#include "resource.h"
#include "sdk_tst.h"
#include "Sipapi.h"

static TCHAR sDlgTitle[MAX_NAME_LEN];
static TCHAR sDlgPromptText[MAX_PATH];
static BTUINT8 g_cSel = 0;
static char g_sPopExcCmd[MAX_PATH];

// Mesage handler for the TestDialog box.
LRESULT CALLBACK TestDialogProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	RECT rt, rt1;
	int DlgWidth, DlgHeight;	// dialog width and height in pixel units
	int NewPosX, NewPosY;
	TCHAR cSelNumEdit[MAX_PATH];
	WORD idItem, wNotifyCode ;
	HWND hWndCtl ;

	idItem		= LOWORD(wParam) ; 
	wNotifyCode = HIWORD(wParam) ; 
	hWndCtl		= (HWND) lParam ;


	switch (message)
	{
		case WM_INITDIALOG:
			// trying to center the About dialog
			if (GetWindowRect(hDlg, &rt1)) {
				GetClientRect(GetParent(hDlg), &rt);
				DlgWidth	= rt1.right - rt1.left;
				DlgHeight	= rt1.bottom - rt1.top ;
				NewPosX		= (rt.right - rt.left - DlgWidth)/2;
				NewPosY		= (rt.bottom - rt.top - DlgHeight)/2;
				
				// if the About box is larger than the physical screen 
				if (NewPosX < 0) NewPosX = 0;
				if (NewPosY < 0) NewPosY = 0;
				SetWindowPos(hDlg, 0, NewPosX, NewPosY,
					0, 0, SWP_NOZORDER | SWP_NOSIZE);
			}
			SetWindowText(hDlg, sDlgTitle);
			SetDlgItemText(hDlg, IDC_SEL_NUM_PROMPT, sDlgPromptText);
			//SetDlgItemText(hDlg, IDC_SEL_NUM_EDIT, _T("This is Test"));
			return TRUE;

		case WM_COMMAND:
			switch(LOWORD(wParam))
			{
				case IDCANCEL:
					g_cSel = 0xFF;
					EndDialog(hDlg, LOWORD(wParam));
					return TRUE;
				case IDC_SEL_NUM_EDIT:
					{
						switch(wNotifyCode)
						{
						case EN_SETFOCUS:
							SipShowIM(SIPF_ON);
							break;
						case EN_KILLFOCUS:
							SipShowIM(SIPF_OFF);
							break;
						default:
							break;
						}
						return TRUE;
					}
				case IDOK:
					GetDlgItemText(hDlg, IDC_SEL_NUM_EDIT, cSelNumEdit, MAX_PATH);
					//PRINTMSG(1, "cSelNumEdit(%S)\r\n", cSelNumEdit);
					WideCharToMultiByte( CP_ACP, 0, cSelNumEdit , -1, g_sPopExcCmd , MAX_PATH, NULL, NULL);
					g_cSel = (BTUINT8)(_ttoi(cSelNumEdit));
					EndDialog(hDlg, LOWORD(wParam));
					return TRUE;										
			}
			break;
	}
    return FALSE;
}


unsigned char OnSelDlg(HWND hWnd, LPCTSTR sTitle, LPCTSTR sPromptText, char *sExcCmd, int sExcCmdLen)
{
//	PRINTMSG(1, "+ OnTestDialog, title(%S)\r\n", sTitle);
	memset(g_sPopExcCmd, 0x0, sizeof(char)*MAX_PATH);
	_tcscpy(sDlgTitle, sTitle);
	_tcscpy(sDlgPromptText, sPromptText);
	DialogBox(g_hInst, (LPCTSTR)IDD_SELECT_FLOW, hWnd, (DLGPROC)TestDialogProc);
	if(sExcCmd)
	{
		strncpy(sExcCmd, g_sPopExcCmd, (sExcCmdLen > MAX_PATH)?MAX_PATH:sExcCmdLen);
		memset(g_sPopExcCmd, 0x0, sizeof(char)*MAX_PATH);
	}
//	PRINTMSG(1, "- OnTestDialog\r\n");
	return g_cSel;
}