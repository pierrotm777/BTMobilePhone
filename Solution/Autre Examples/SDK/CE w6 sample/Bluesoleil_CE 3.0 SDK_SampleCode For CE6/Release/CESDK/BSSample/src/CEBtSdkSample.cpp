// CEBtSdkSample.cpp : Defines the entry point for the application.
//
#include <windows.h>
#include "CEBtSdkSample.h"
#include <commctrl.h>
#include "sdk_tst.h"
#include "SelDlg.h"
#include "ras.h"
#include "Sipapi.h"


#define MAX_LOADSTRING 100

#define EXCCMD_STATIC_X 5
#define EXCCMD_STATIC_WIDTH 60

#define EXCCMD_EDIT_X (EXCCMD_STATIC_WIDTH+EXCCMD_STATIC_X+5)
#define EXCCMD_EDIT_WIDTH 80

#define EXC_BUTTON_X (EXCCMD_EDIT_X+EXCCMD_EDIT_WIDTH+5)
#define EXC_BUTTON_WIDTH 50

#define EXCCMD_Y 40
#define EXCCMD_HEIGHT 23
#define EXCCMD_X EXCCMD_STATIC_X


HWND  g_hMainWndHdl= NULL;

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass	(HINSTANCE, LPTSTR);
BOOL				InitInstance	(HINSTANCE, int);
LRESULT CALLBACK	WndProc			(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK	About			(HWND, UINT, WPARAM, LPARAM);




int WINAPI WinMain(	HINSTANCE hInstance,
					HINSTANCE hPrevInstance,
					LPTSTR    lpCmdLine,
					int       nCmdShow)
{
	MSG msg;
	HACCEL hAccelTable;

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow)) 
	{
		return FALSE;
	}	
	
	hAccelTable = LoadAccelerators(hInstance, (LPCTSTR)IDC_CEBTSDKSAMPLE);

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0)) 
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg)) 
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return msg.wParam;
}

//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
//    It is important to call this function so that the application 
//    will get 'well formed' small icons associated with it.
//
ATOM MyRegisterClass(HINSTANCE hInstance, LPTSTR szWindowClass)
{
	WNDCLASS	wc;

    wc.style			= CS_HREDRAW | CS_VREDRAW;
    wc.lpfnWndProc		= (WNDPROC) WndProc;
    wc.cbClsExtra		= 0;
    wc.cbWndExtra		= 0;
    wc.hInstance		= hInstance;
    wc.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_CEBTSDKSAMPLE));
    wc.hCursor			= 0;
    wc.hbrBackground	= (HBRUSH) GetStockObject(WHITE_BRUSH);
    wc.lpszMenuName		= 0;
    wc.lpszClassName	= szWindowClass;

	return RegisterClass(&wc);
}

//
//  FUNCTION: InitInstance(HANDLE, int)
//
//  PURPOSE: Saves instance handle and creates main window
//
//  COMMENTS:
//
//    In this function, we save the instance handle in a global variable and
//    create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	HWND	hWnd;
	TCHAR	szTitle[MAX_LOADSTRING];			// The title bar text
	TCHAR	szWindowClass[MAX_LOADSTRING];		// The window class name
	UINT    uHeight = CW_USEDEFAULT;

	g_hInst = hInstance;		// Store instance handle in our global variable	

	// Initialize global strings
	LoadString(hInstance, IDC_CEBTSDKSAMPLE, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance, szWindowClass);

	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	hWnd = CreateWindow(szWindowClass, szTitle, WS_VISIBLE | WS_CAPTION,
		CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, uHeight - 10, NULL, NULL, hInstance, NULL);

	if (!hWnd)
	{	
		return FALSE;
	}

	SaveMainWndHdl(hWnd);

	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);
	if (g_hwndCB)
		CommandBar_Show(g_hwndCB, TRUE);

	return TRUE;
}

//
//  FUNCTION: WndProc(HWND, unsigned, WORD, LONG)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	HDC hdc;
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	TCHAR szHello[MAX_LOADSTRING];
	RASCONNSTATE Ras_state = RASCS_OpenPort;
	WORD idItem, wNotifyCode ;
	HWND hWndCtl ;

	idItem		= LOWORD(wParam) ; 
	wNotifyCode = HIWORD(wParam) ; 
	hWndCtl		= (HWND) lParam ;

	switch (message) 
	{
		case WM_COMMAND:
			wmId    = LOWORD(wParam); 
			wmEvent = HIWORD(wParam); 
			//PRINTMSG(1, "wmId(0x%x), wmEvent(0x%x)\r\n", wmId, wmEvent);
			// Parse the menu selections:
			switch (wmId)
			{
				case IDM_HELP_ABOUT:
				   DialogBox(g_hInst, (LPCTSTR)IDD_ABOUTBOX, hWnd, (DLGPROC)About);
				   break;
				case IDM_FILE_EXIT:
				   DestroyWindow(hWnd);
				   break;
				case IDC_EXCCMD_EDIT:
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
					}
					break;
				case IDC_EXC_BUTTON:
					PRINTMSG(1, "Click Exc button, g_NumberLevel(%d)\r\n", g_NumberLevel);
					{
						if(GetExcCmd(&g_iNum, &g_cExcCmd))
						{
							PRINTMSG(1, "g_iNum(%d), g_cExcCmd(%c)\r\n", g_iNum, g_cExcCmd);
							switch(g_NumberLevel)
							{
								case 0:
									SetEvent(g_hSdkExcCmdEvt);
									break;
								case 1:
									SetEvent(g_hFuncExcCmdEvt);
									break;
								case 2:
									SetEvent(g_h3LevelCmdEvent);
									break;
								default:
									PRINTMSG(1, "g_NumberLevel(%d) is wrong\r\n", g_NumberLevel);
									break;
							}
							
						}
					}
					break;
				case IDC_CLEAR_BUTTON:
					PRINTMSG(1, "Click Clear button, clear all log\r\n");
					{
						SendDlgItemMessage (g_hWnd, IDC_LOG_EDIT, LB_RESETCONTENT, 0,0);
					}
					break;
				default:
				   return DefWindowProc(hWnd, message, wParam, lParam);
			}
			break;
		case WM_CREATE:
			{
				g_hwndCB = CommandBar_Create(g_hInst, hWnd, 1);			
				CommandBar_InsertMenubar(g_hwndCB, g_hInst, IDM_MENU, 0);
				CommandBar_AddAdornments(g_hwndCB, 0, 0);
				g_hWnd = hWnd;

				{		
					int cx;
					int cy;
					cx=GetSystemMetrics(SM_CXSCREEN);
					cy=GetSystemMetrics(SM_CYSCREEN);
					PRINTMSG(1, "cx(%d), cy(%d)\r\n", cx, cy);

					CreateWindow(TEXT("EDIT"),TEXT(""),WS_BORDER |WS_CHILD|WS_VISIBLE,EXCCMD_EDIT_X,EXCCMD_Y,EXCCMD_EDIT_WIDTH,EXCCMD_HEIGHT,hWnd,(HMENU)IDC_EXCCMD_EDIT,g_hInst,NULL);
					CreateWindow(TEXT("STATIC"),TEXT("Exc Cmd"),WS_BORDER |WS_CHILD|WS_VISIBLE,EXCCMD_STATIC_X,EXCCMD_Y,EXCCMD_STATIC_WIDTH,EXCCMD_HEIGHT,hWnd,(HMENU)IDC_EXCCMD_STATIC,g_hInst,NULL);
					CreateWindow(TEXT("BUTTON"),TEXT("Exc"),WS_BORDER |WS_CHILD|WS_VISIBLE,EXC_BUTTON_X,EXCCMD_Y,EXC_BUTTON_WIDTH,EXCCMD_HEIGHT,hWnd,(HMENU)IDC_EXC_BUTTON,g_hInst,NULL);
					CreateWindow(TEXT("BUTTON"),TEXT("ClearLog"),WS_BORDER |WS_CHILD|WS_VISIBLE,EXC_BUTTON_X+2*EXC_BUTTON_WIDTH,EXCCMD_Y,2*EXC_BUTTON_WIDTH,EXCCMD_HEIGHT,hWnd,(HMENU)IDC_CLEAR_BUTTON,g_hInst,NULL);
					CreateWindowEx (WS_EX_CLIENTEDGE, TEXT ("listbox"),TEXT (""), WS_VISIBLE | WS_CHILD | WS_VSCROLL |LBS_USETABSTOPS | LBS_NOINTEGRALHEIGHT,EXCCMD_STATIC_X,EXCCMD_Y+EXCCMD_HEIGHT+5 ,cx - (EXCCMD_X+EXCCMD_X),cy - (EXCCMD_Y+EXCCMD_HEIGHT+10+20),hWnd,(HMENU)IDC_LOG_EDIT,g_hInst,NULL);

					PRINTMSG(1, "WM_CREATE: hWnd(0x%x)\r\n", hWnd);
					SetFocus(GetDlgItem(hWnd,IDC_EXCCMD_EDIT));			
				}

				InitSdk(g_hInst);
			}
			break;
		case WM_PAINT:
			{
				RECT rt;
				hdc = BeginPaint(hWnd, &ps);
				GetClientRect(hWnd, &rt);
				LoadString(g_hInst, IDS_HELLO, szHello, MAX_LOADSTRING);
				DrawText(hdc, szHello, _tcslen(szHello), &rt, 
					DT_SINGLELINE | DT_VCENTER | DT_CENTER);
				EndPaint(hWnd, &ps);
			}
			break;
		case WM_DESTROY:
			{
				DeinitSdk();

				CommandBar_Destroy(g_hwndCB);
				PostQuitMessage(0);
			}
			break;
		case WM_RASDIALEVENT :
			{
				Ras_state = (RASCONNSTATE)wParam;
				switch ( Ras_state )
				{
				case RASCS_OpenPort:
					PRINTMSG(1,"Ras_state = RASCS_OpenPort \r\n");
					break;
				case RASCS_PortOpened:
					PRINTMSG(1,"Ras_state = RASCS_PortOpened \r\n");
					break;
				case RASCS_Connected:
					PRINTMSG(1,"Ras_state = RASCS_Connected \r\n");
					break;
				case RASCS_Disconnected:
					PRINTMSG(1,"Ras_state = RASCS_Disconnected \r\n");
					break;
				case RASCS_Authenticate:
					PRINTMSG(1,"Ras_state = RASCS_Authenticate \r\n");
					break;
				case RASCS_Authenticated:
					PRINTMSG(1,"Ras_state = RASCS_Authenticated \r\n");
					break;
				default:
					PRINTMSG(1,"Ras_state = %d \r\n", Ras_state);
					break;
				}
			}
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
   }
   return 0;
}

// Mesage handler for the About box.
LRESULT CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	RECT rt, rt1;
	int DlgWidth, DlgHeight;	// dialog width and height in pixel units
	int NewPosX, NewPosY;

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
			return TRUE;

		case WM_COMMAND:
			if ((LOWORD(wParam) == IDOK) || (LOWORD(wParam) == IDCANCEL))
			{
				EndDialog(hDlg, LOWORD(wParam));
				return TRUE;
			}
			break;
	}
    return FALSE;
}


void SaveMainWndHdl(HWND hWnd)
{
	g_hMainWndHdl = hWnd;
}

HWND GetMainWndHdl()
{
    return g_hMainWndHdl;
}