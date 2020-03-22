//==========================================================================================
//
//		OpenNETCF.Win32.Win32Window
//		Copyright (c) 2003-2004, OpenNETCF.org
//
//		This library is free software; you can redistribute it and/or modify it under 
//		the terms of the OpenNETCF.org Shared Source License.
//
//		This library is distributed in the hope that it will be useful, but 
//		WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
//		FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
//		for more details.
//
//		You should have received a copy of the OpenNETCF.org Shared Source License 
//		along with this library; if not, email licensing@opennetcf.org to request a copy.
//
//		If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
//		email licensing@opennetcf.org.
//
//		For general enquiries, email enquiries@opennetcf.org or visit our website at:
//		http://www.opennetcf.org
//
//==========================================================================================
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// A helper class for working with native windows.
	/// </summary>
	public class Win32Window
	{
		//native window handle
		IntPtr hWnd;

		private Win32Window()
		{
		}

		private Win32Window(IntPtr hWnd)
		{
			this.hWnd = hWnd;
		}

		public static Win32Window FromControl(Control c)
		{
			c.Capture = true;
			IntPtr hWnd = GetCapture();
			c.Capture = false;

			if ( hWnd == IntPtr.Zero )
				throw new Exception("Unable to capture window");
			return new Win32Window(hWnd);
		}

		static public implicit operator IntPtr(Win32Window wnd)
		{
			return wnd.hWnd;
		}

		/// <summary>
		/// 
		/// </summary>
		public enum GetWindowParam: int
		{
			/// <summary>
			/// 
			/// </summary>
			GW_HWNDFIRST  =      0,
			/// <summary>
			/// 
			/// </summary>
			GW_HWNDLAST   =      1,
			/// <summary>
			/// 
			/// </summary>
			GW_HWNDNEXT   =      2,
			/// <summary>
			/// 
			/// </summary>
			GW_HWNDPREV   =      3,
			/// <summary>
			/// 
			/// </summary>
			GW_OWNER      =      4,
			/// <summary>
			/// 
			/// </summary>
			GW_CHILD      =      5,
		}

		/// <summary>
		/// 
		/// </summary>
		public enum GetWindowLongParam
		{
			/// <summary>
			/// 
			/// </summary>
			GWL_WNDPROC   =      (-4),
			/// <summary>
			/// 
			/// </summary>
			GWL_HINSTANCE =      (-6),
			/// <summary>
			/// 
			/// </summary>
			GWL_HWNDPARENT=      (-8),
			/// <summary>
			/// 
			/// </summary>
			GWL_STYLE     =      (-16),
			/// <summary>
			/// 
			/// </summary>
			GWL_EXSTYLE   =      (-20),
			/// <summary>
			/// 
			/// </summary>
			GWL_USERDATA  =      (-21),
			/// <summary>
			/// 
			/// </summary>
			GWL_ID        =      (-12),
		}

		#region Window Styles
		
		/// <summary>
		/// Specifies the extended style of the window. This parameter can be one of the values
		/// </summary>
		public enum WindowStyleExtended
		{
			/// <summary>
			/// Specifies that a window created with this style accepts drag-drop files. 
			/// </summary>
			WS_EX_ACCEPTFILES,

			/// <summary>
			/// Forces a top-level window onto the taskbar when the window is visible.  
			/// </summary>
			WS_EX_APPWINDOW,

			/// <summary>
			/// Specifies that a window has a border with a sunken edge. 
			/// </summary>
			WS_EX_CLIENTEDGE = 0x00000200, 
			
			/// <summary>
			/// Creates a window that has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter. 
			/// </summary>
			WS_EX_DLGMODALFRAME = 0x00000001,
	
			/// <summary>
			/// Creates a window that has generic "left-aligned" properties. This is the default. 
			/// </summary>
			WS_EX_LEFT = 0,
	
			/// <summary>
			/// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area. For other languages, the style is ignored. 
			/// </summary>
			WS_EX_LEFTSCROLLBAR = 0x00004000,
	
			/// <summary>
			/// The window text is displayed using left-to-right reading-order properties. This is the default. 
			/// </summary>
			WS_EX_LTRREADING = 0,
	
			/// <summary>
			/// Creates an MDI child window. 
			/// </summary>
			WS_EX_MDICHILD = 0x00000040,
	
			/// <summary>
			/// A top-level window created with this style cannot be activated. If a child window has this style, tapping it does not cause its top-level parent to be activated. A window that has this style receives stylus events, but neither it nor its child windows can get the focus. Supported in Windows CE versions 2.0 and later. 
			/// </summary>
			WS_EX_NOACTIVATE = 0x08000000,
	
			/// <summary>
			/// A window created with this style does not show animated exploding and imploding rectangles, and does not have a button on the taskbar. Supported in Windows CE versions 2.0 and later. 
			/// </summary>
			WS_EX_NOANIMATION = 0x04000000,
	
			/// <summary>
			/// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed. 
			/// </summary>
			WS_EX_NOPARENTNOTIFY = 0x00000004,
	
			/// <summary>
			/// Combines the WS_EX_CLIENTEDGE and WS_EX_WINDOWEDGE styles. 
			/// </summary>
			WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
	
			/// <summary>
			/// Combines the WS_EX_WINDOWEDGE, WS_EX_TOOLWINDOW, and WS_EX_TOPMOST styles. 
			/// </summary>
			WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
	
			/// <summary>
			/// The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored. 
			/// </summary>
			WS_EX_RIGHT = 0x00001000,
	
			/// <summary>
			/// Vertical scroll bar (if present) is to the right of the client area. This is the default. 
			/// </summary>
			WS_EX_RIGHTSCROLLBAR = 0, // Cannot find value in header file.
	
			/// <summary>
			/// If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties. For other languages, the style is ignored. 
			/// </summary>
			WS_EX_RTLREADING = 0x00002000,
	
			/// <summary>
			/// Creates a window with a three-dimensional border style intended to be used for items that do not accept user input. 
			/// </summary>
			WS_EX_STATICEDGE = 0x00020000,
	
			/// <summary>
			/// Creates a tool window; that is, a window intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.  
			/// </summary>
			WS_EX_TOOLWINDOW = 0x00000080,
	
			/// <summary>
			/// Specifies that a window created with this style should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function. 
			/// </summary>
			WS_EX_TOPMOST = 0x00000008,
	
			/// <summary>
			/// Specifies that a window created with this style should not be painted until siblings beneath the window (that were created by the same thread) have been painted. The window appears transparent because the bits of underlying sibling windows have already been painted. To achieve transparency without these restrictions, use the SetWindowRgn function.
			/// </summary>
			WS_EX_TRANSPARENT = 0x00000020,
	
			/// <summary>
			/// Specifies that a window has a border with a raised edge. 
			/// </summary>
			WS_EX_WINDOWEDGE = 0x00000100,

			/// <summary>
			/// Additional value for Completeness
			/// </summary>
			WS_EX_NONE = 0x00000000
		}

		//General
		public enum WindowStyle : int
		{
			/// <summary>
			/// Creates a window that has a thin-line border. 
			/// </summary>
			WS_BORDER = 0x00800000,
 
			/// <summary>
			/// Creates a window that has a title bar (includes the WS_BORDER style). 
			/// </summary>
			WS_CAPTION = 0x00C00000,
 
			/// <summary>
			/// Creates a child window. This style cannot be used with the WS_POPUP style. 
			/// </summary>
			WS_CHILD = 0x40000000,
 
			/// <summary>
			/// Same as the WS_CHILD style. 
			/// </summary>
			WS_CHILDWINDOW = 0x40000000,
			/// <summary>
			/// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated. If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window. 
			/// </summary>
			WS_CLIPSIBLINGS = 0x04000000,
 
			/// <summary>
			/// Creates a window that is initially disabled. A disabled window cannot receive input from the user. 
			/// </summary>
			WS_DISABLED = 0x08000000,
 
			/// <summary>
			/// Creates a window that has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar. 
			/// </summary>
			WS_DLGFRAME = 0x00400000,
 
			/// <summary>
			/// Specifies the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style. The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys. 
			/// </summary>
			WS_GROUP = 0x00020000,
 
			/// <summary>
			/// Creates a window that has a horizontal scroll bar. 
			/// </summary>
			WS_HSCROLL = 0x00100000,
 
			/// <summary>
			/// Creates a window that is initially minimized. Same as the WS_MINIMIZE style. 
			/// </summary>
			WS_ICONIC,
 
			/// <summary>
			/// Creates a window that is initially maximized. 
			/// </summary>
			WS_MAXIMIZE,
 
			/// <summary>
			/// Creates a window that has a Maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style.  
			/// </summary>
			WS_MAXIMIZEBOX = 0x00020000,
 
			/// <summary>
			/// Creates a window that is initially minimized. Same as the WS_ICONIC style. 
			/// </summary>
			WS_MINIMIZE,
 
			/// <summary>
			/// Creates a window that has a Minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style.  
			/// </summary>
			WS_MINIMIZEBOX = 0x00010000,
 
			/// <summary>
			/// Creates an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style. 
			/// </summary>
			WS_OVERLAPPED = WS_BORDER | WS_CAPTION,
 
			/// <summary>
			/// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU, WS_THICKFRAME, WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles. Same as the WS_TILEDWINDOW style.  
			/// </summary>
			WS_OVERLAPPEDWINDOW = WS_OVERLAPPED,
 
			/// <summary>
			/// Creates a pop-up window. This style cannot be used with the WS_CHILD style. 
			/// </summary>
			WS_POPUP = unchecked((int)0x80000000),
 
			/// <summary>
			/// Creates a pop-up window with WS_BORDER, WS_POPUP, and WS_SYSMENU styles. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible. 
			/// </summary>
			WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
 
			/// <summary>
			/// Creates a window that has a sizing border. Same as the WS_THICKFRAME style. 
			/// </summary>
			WS_SIZEBOX = 0x00040000,
 
			/// <summary>
			/// Creates a window that has a Close (X) button in the non-client area. 
			/// </summary>
			WS_SYSMENU = 0x00080000,
 
			/// <summary>
			/// Specifies a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style. 
			/// </summary>
			WS_TABSTOP = 0x00010000,
 
			/// <summary>
			/// Creates a window that has a sizing border. Same as the WS_SIZEBOX style. 
			/// </summary>
			WS_THICKFRAME = 0x00040000,
 
			/// <summary>
			/// Creates an overlapped window. An overlapped window has a title bar and a border. Same as the WS_OVERLAPPED style.  
			/// </summary>
			WS_TILED = WS_OVERLAPPED,
 
			/// <summary>
			/// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU, WS_THICKFRAME, WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles. Same as the WS_OVERLAPPEDWINDOW style.  
			/// </summary>
			WS_TILEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU,
 
			/// <summary>
			/// Creates a window that is initially visible. 
			/// </summary>
			WS_VISIBLE = 0x10000000,
 
			/// <summary>
			/// Creates a window that has a vertical scroll bar. 
			/// </summary>
			WS_VSCROLL = 0x00200000
		}

		//Listbox
		public enum ListBoxStyle : int
		{
			
			LBS_NOTIFY           = 0x0001,
			LBS_SORT             = 0x0002,
			LBS_NOREDRAW         = 0x0004,
			LBS_MULTIPLESEL      = 0x0008,
			LBS_HASSTRINGS       = 0x0040,
			LBS_USETABSTOPS      = 0x0080,
			LBS_NOINTEGRALHEIGHT = 0x0100,
			LBS_MULTICOLUMN      = 0x0200,
			LBS_WANTKEYBOARDINPUT= 0x0400,
			LBS_EXTENDEDSEL      = 0x0800,
			LBS_DISABLENOSCROLL  = 0x1000,
			LBS_NODATA           = 0x2000,
			LBS_STANDARD         = (LBS_NOTIFY | LBS_SORT | WindowStyle.WS_VSCROLL | WindowStyle.WS_BORDER)
		}

		public enum ComboBoxStyle
		{
			CBS_DROPDOWN          = 0x0002,
			CBS_DROPDOWNLIST      = 0x0003,
			CBS_AUTOHSCROLL       = 0x0040,
			CBS_OEMCONVERT        = 0x0080,
			CBS_SORT              = 0x0100,
			CBS_HASSTRINGS        = 0x0200,
			CBS_NOINTEGRALHEIGHT  = 0x0400,
			CBS_DISABLENOSCROLL   = 0x0800,
			CBS_UPPERCASE         = 0x2000,
			CBS_LOWERCASE         = 0x4000,		
		}

		public enum EditStyle
		{
			ES_LEFT        = 0x0000,
			ES_CENTER      = 0x0001,
			ES_RIGHT       = 0x0002,
			ES_MULTILINE   = 0x0004,
			ES_UPPERCASE   = 0x0008,
			ES_LOWERCASE   = 0x0010,
			ES_PASSWORD    = 0x0020,
			ES_AUTOVSCROLL = 0x0040,
			ES_AUTOHSCROLL = 0x0080,
			ES_NOHIDESEL   = 0x0100,
			ES_COMBOBOX    = 0x0200,
			ES_OEMCONVERT  = 0x0400,
			ES_READONLY    = 0x0800,
			ES_WANTRETURN  = 0x1000,
			ES_NUMBER      = 0x2000,
		}
	
		
		#endregion

		#region Windows messages

		public enum WindowMessage: int
		{
			WM_PAINT				= 0x000F,
			WM_NOTIFY				= 0x004E,
			WM_HELP					= 0x0053,
			WM_CTLCOLOREDIT			= 0x0133,
			WM_CTLCOLORLISTBOX		= 0x0134,
			WM_CTLCOLORBTN			= 0x0135,
			WM_HIBERNATE			= 0x03ff,
			WM_SETTEXT				= 0xC
	}

		public enum ListViewMessage: int
		{
			LVM_FIRST = 0x1000,
			LVM_GETHEADER = LVM_FIRST + 31,
			LVM_SETICONSPACING = LVM_FIRST + 53,
		}

		public enum EditMessage: int
		{
			EM_GETSEL              = 0x00B0,
			EM_SETSEL              = 0x00B1,
			EM_GETRECT             = 0x00B2,
			EM_SETRECT             = 0x00B3,
			EM_SETRECTNP           = 0x00B4,
			EM_SCROLL              = 0x00B5,
			EM_LINESCROLL          = 0x00B6,
			EM_SCROLLCARET         = 0x00B7,
			EM_GETMODIFY           = 0x00B8,
			EM_SETMODIFY           = 0x00B9,
			EM_GETLINECOUNT        = 0x00BA,
			EM_LINEINDEX           = 0x00BB,
			EM_LINELENGTH          = 0x00C1,
			EM_REPLACESEL          = 0x00C2,
			EM_GETLINE             = 0x00C4,
			EM_LIMITTEXT           = 0x00C5,
			EM_CANUNDO             = 0x00C6,
			EM_UNDO                = 0x00C7,
			EM_FMTLINES            = 0x00C8,
			EM_LINEFROMCHAR        = 0x00C9,
			EM_SETTABSTOPS         = 0x00CB,
			EM_SETPASSWORDCHAR     = 0x00CC,
			EM_EMPTYUNDOBUFFER     = 0x00CD,
			EM_GETFIRSTVISIBLELINE = 0x00CE,
			EM_SETREADONLY         = 0x00CF,
			EM_GETPASSWORDCHAR     = 0x00D2,
			EM_SETMARGINS          = 0x00D3,
			EM_GETMARGINS          = 0x00D4,
			EM_SETLIMITTEXT        = EM_LIMITTEXT,
			EM_GETLIMITTEXT        = 0x00D5,
			EM_POSFROMCHAR         = 0x00D6,
			EM_CHARFROMPOS         = 0x00D7
		}

		public enum ComboBoxMessage: int
		{
			CB_GETEDITSEL              = 0x0140,
			CB_LIMITTEXT               = 0x0141,
			CB_SETEDITSEL              = 0x0142,
			CB_ADDSTRING               = 0x0143,
			CB_DELETESTRING            = 0x0144,
			CB_GETCOUNT                = 0x0146,
			CB_GETCURSEL               = 0x0147,
			CB_GETLBTEXT               = 0x0148,
			CB_GETLBTEXTLEN            = 0x0149,
			CB_INSERTSTRING            = 0x014A,
			CB_RESETCONTENT            = 0x014B,
			CB_FINDSTRING              = 0x014C,
			CB_SELECTSTRING            = 0x014D,
			CB_SETCURSEL               = 0x014E,
			CB_SHOWDROPDOWN            = 0x014F,
			CB_GETITEMDATA             = 0x0150,
			CB_SETITEMDATA             = 0x0151,
			CB_GETDROPPEDCONTROLRECT   = 0x0152,
			CB_SETITEMHEIGHT           = 0x0153,
			CB_GETITEMHEIGHT           = 0x0154,
			CB_SETEXTENDEDUI           = 0x0155,
			CB_GETEXTENDEDUI           = 0x0156,
			CB_GETDROPPEDSTATE         = 0x0157,
			CB_FINDSTRINGEXACT         = 0x0158,
			CB_SETLOCALE               = 0x0159,
			CB_GETLOCALE               = 0x015A,
			CB_GETTOPINDEX             = 0x015b,
			CB_SETTOPINDEX             = 0x015c,
			CB_GETHORIZONTALEXTENT     = 0x015d,
			CB_SETHORIZONTALEXTENT     = 0x015e,
			CB_GETDROPPEDWIDTH         = 0x015f,
			CB_SETDROPPEDWIDTH         = 0x0160,
			CB_INITSTORAGE             = 0x0161
		}


		public enum NotificationMessage: int
		{
			TCN_FIRST		= (0-550),
			TCN_SELCHANGED  = (TCN_FIRST - 1),
			TCN_SELCHANGING  = (TCN_FIRST - 2),
	}

		#endregion

		#region WindowPos enumerations
		//WindowPosZOrder
		public enum SetWindowPosZOrder 
		{
			HWND_TOP        = 0,
			HWND_BOTTOM     = 1,
			HWND_TOPMOST    = -1,
			HWND_NOTOPMOST  = -2,
			HWND_MESSAGE = -3
		}

		public enum SetWindowPosFlags 
		{
			SWP_NOSIZE          = 0x0001,
			SWP_NOMOVE          = 0x0002,
			SWP_NOZORDER        = 0x0004,
			SWP_NOREDRAW        = 0x0008,
			SWP_NOACTIVATE      = 0x0010,
			SWP_FRAMECHANGED    = 0x0020,
			SWP_SHOWWINDOW      = 0x0040,
			SWP_HIDEWINDOW      = 0x0080,
			SWP_NOCOPYBITS      = 0x0100,
			SWP_NOOWNERZORDER   = 0x0200, 
			SWP_NOSENDCHANGING  = 0x0400,
			SWP_DRAWFRAME       = SWP_FRAMECHANGED,
			SWP_NOREPOSITION    = SWP_NOOWNERZORDER,
			SWP_DEFERERASE      = 0x2000,
			SWP_ASYNCWINDOWPOS  = 0x4000
		}

		#endregion

		[DllImport("coredll.dll")] 
		public static extern IntPtr CreateWindowExW(IntPtr dwExStyle, string lpClassName,
			string lpWindowName,IntPtr dwStyle, int X, int Y, int cx, int cy,
			IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		/// <summary>
		/// This function destroys the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to the window to be destroyed.</param>
		/// <returns>Nonzero indicates success.</returns>
		[DllImport("coredll.dll")] 
		public static extern IntPtr DestroyWindow(IntPtr hWnd);

		/// <summary>
		/// This function retrieves the handle to the top-level window whose class name and window name match the specified strings.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="wndName">The window name (the window's title). If this parameter is NULL, all window names match.</param>
		/// <returns>A handle to the window that has the specified class name and window name indicates success. NULL indicates failure.</returns>
		[DllImport("coredll")]
		public static extern IntPtr FindWindow(string className, string wndName);

		/// <summary>
		/// Retrieves the handle to a window that has the specified relationship to the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to a window.</param>
		/// <param name="nCmd">Specifies the relationship between the specified window and the window whose handle is to be retrieved.</param>
		/// <returns>A window handle indicates success. NULL indicates that no window exists with the specified relationship to the specified window.</returns>
		[DllImport("coredll")]
		public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowParam nCmd);

		/// <summary>
		/// Retrieves the handle to a window that has the specified relationship to the Win32Window.
		/// </summary>
		/// <param name="nCmd">Specifies the relationship between the specified window and the window which is to be retrieved.</param>
		/// <returns></returns>
		public Win32Window GetWindow(GetWindowParam nCmd)
		{
			IntPtr hWndNext = GetWindow(hWnd, nCmd);
			return new Win32Window(hWndNext);	
		}
		
		[DllImport("coredll")]
		internal static extern int GetWindowText(IntPtr hWnd, StringBuilder sb, int maxCount);

		/// <summary>
		/// Copies the text of the specified window's title bar or controls body.
		/// </summary>
		/// <returns>The window text.</returns>
		public string GetWindowText()
		{
			StringBuilder sb = new StringBuilder(256);
			GetWindowText(hWnd, sb, sb.Capacity);
			return sb.ToString();
		}
		
		#region GetWindowRect

		[DllImport("coredll")]
		public static extern bool GetWindowRect(IntPtr hWnd, byte[] rect);
		
		/// <summary>
		/// Returns a Rectangle representing the bounds of the window
		/// </summary>
		/// <param name="hWnd">a valid window handle</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle"/> representing the bounds of the specified window.</returns>
		public static Rectangle GetWindowRect(IntPtr hWnd)
		{
			byte[] rect = new byte[16];

			if(!GetWindowRect(hWnd, rect))
			{
				throw new WinAPIException("Failed getting window rectangle");
			}

			int x = BitConverter.ToInt32(rect, 0);
			int y = BitConverter.ToInt32(rect, 4);
			int width = BitConverter.ToInt32(rect, 8) - x;
			int height = BitConverter.ToInt32(rect, 12) - y;
			Rectangle result = new Rectangle(x, y, width, height);

			return result;
		}
		#endregion

		/// <summary>
		/// Retrieves the window handle to the active window associated with the thread that calls the function.
		/// </summary>
		/// <returns>The handle to the active window associated with the calling thread's message queue indicates success. NULL indicates failure.</returns>
		[DllImport("coredll")]
		public static extern IntPtr GetActiveWindow();

		/// <summary>
		/// Retrieves the handle to the keyboard focus window associated with the thread that called the function.
		/// </summary>
		/// <returns>The handle to the window with the keyboard focus indicates success. NULL indicates that the calling thread's message queue does not have an associated window with the keyboard focus.</returns>
		[DllImport("coredll")]
		public static extern IntPtr GetFocus();

		/// <summary>
		/// Retrieves the handle to the window, if any, that has captured the mouse or stylus input. Only one window at a time can capture the mouse or stylus input.
		/// </summary>
		/// <returns>The handle of the capture window associated with the current thread indicates success. NULL indicates that no window in the current thread has captured the mouse.</returns>
		[DllImport("coredll")]
		public static extern IntPtr GetCapture();

		/// <summary>
		/// Retrieves information about the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="nItem">Specifies the zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus four; for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer.</param>
		/// <returns>The requested 32-bit value indicates success. Zero indicates failure.</returns>
		[DllImport("coredll")]
		public static extern int GetWindowLong(IntPtr hWnd, GetWindowLongParam nItem);

		/// <summary>
		/// This function changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="X">Specifies the new position of the left side of the window.</param>
		/// <param name="Y">Specifies the new position of the top of the window.</param>
		/// <param name="cx">Specifies the new width of the window.</param>
		/// <param name="cy">Specifies the new height of the window.</param>
		/// <returns>Nonzero indicates success. Zero indicates failure.</returns>
		[DllImport("coredll.dll")] 
		public static extern IntPtr MoveWindow(IntPtr hWnd, int X, int Y, int cx, int cy);

		/// <summary>
		/// Places a message in the message queue associated with the thread that created the specified window and then returns without waiting for the thread to process the message.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure is to receive the message.</param>
		/// <param name="msg">Specifies the message to be posted.</param>
		/// <param name="wParam">Specifies additional message-specific information.</param>
		/// <param name="lParam">Specifies additional message-specific information.</param>
		/// <returns>Nonzero indicates success. Zero indicates failure.</returns>
		[DllImport("coredll.dll")] 
		public static extern IntPtr PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Changes an attribute of the specified window.
		/// </summary>
		/// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="GetWindowLongParam">Specifies the zero-based offset to the value to be set.</param>
		/// <param name="nValue">Specifies the replacement value.</param>
		[DllImport("coredll")]
		public static extern void SetWindowLong(IntPtr hWnd, int GetWindowLongParam, int nValue);
		
		[DllImport("coredll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
		
		[DllImport("coredll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

		[DllImport("coredll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

		[DllImport("coredll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, String lParam);

		[DllImport("coredll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, String lParam);

		[DllImport("coredll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, byte[] lParam);

		[DllImport("coredll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, byte[] lParam);

		/// <summary>
		/// enables or disables mouse and keyboard input to the specified window or control. When input is disabled, the window does not receive input such as mouse clicks and key presses. When input is enabled, the window receives all input.
		/// </summary>
		/// <param name="hWnd">Handle to the window to be enabled or disabled.</param>
		/// <param name="bEnable">Boolean that specifies whether to enable or disable the window. If this parameter is TRUE, the window is enabled. If the parameter is FALSE, the window is disabled.</param>
		/// <returns>Nonzero indicates that the window was previously disabled. Zero indicates that the window was not previously disabled.</returns>
		[DllImport("coredll")]
		public static extern bool EnableWindow( IntPtr hWnd, bool bEnable );

		/// <summary>
		/// Defines a new window message that is guaranteed to be unique throughout the system. The returned message value can be used when calling the SendMessage or PostMessage function.
		/// </summary>
		/// <param name="lpMsg">String that specifies the message to be registered.</param>
		/// <returns>A message identifier in the range 0xC000 through 0xFFFF indicates that the message is successfully registered. Zero indicates failure.</returns>
		[DllImport("coredll")]
		public static extern int RegisterWindowMessage(string lpMsg);

		[DllImport("coredll")]
		public static extern IntPtr CallWindowProc(IntPtr pfn, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("coredll")]
		public static extern IntPtr CallWindowProc(IntPtr pfn, IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Sets the keyboard focus to the specified window. All subsequent keyboard input is directed to this window. The window, if any, that previously had the keyboard focus loses it.
		/// </summary>
		/// <param name="hWnd">Handle to the window that will receive the keyboard input.
		/// If this parameter is NULL, keystrokes are ignored.</param>
		/// <returns>The handle to the window that previously had the keyboard focus indicates success. NULL indicates that the hWnd parameter is invalid or the window is not associated with the calling thread's message queue.</returns>
		[DllImport("coredll.dll")]
		public static extern bool SetFocus(IntPtr hWnd);

		/// <summary>
		/// Puts the thread that created the specified window into the foreground and activates the window.
		/// </summary>
		/// <param name="hWnd">Handle to the window that should be activated and brought to the foreground.</param>
		/// <returns>Nonzero indicates that the window was brought to the foreground. Zero indicates that the window was not brought to the foreground.</returns>
		[DllImport("coredll.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd ); 

		[DllImport("coredll.dll")]
		public extern static int SetWindowPos (
			IntPtr hWnd, SetWindowPosZOrder pos,
			int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

		public static void UpdateWindowStyle(IntPtr hwnd, int RemoveStyle, int AddStyle) 
		{
			int style = GetWindowLong(hwnd, GetWindowLongParam.GWL_STYLE);
			style &= ~RemoveStyle;
			style |= AddStyle;
			SetWindowLong(hwnd, (int)GetWindowLongParam.GWL_STYLE, style);
			SetWindowPos(hwnd, 0, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOMOVE |
				SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOSIZE |
				SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_FRAMECHANGED);
		}

		public static void SetWindowStyle(IntPtr hwnd, int style) 
		{
			SetWindowLong(hwnd, (int)GetWindowLongParam.GWL_STYLE, style);
			SetWindowPos(hwnd, 0, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOMOVE |
				SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOSIZE |
				SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_FRAMECHANGED);
		}

		[DllImport("coredll.dll", EntryPoint="SetParent", SetLastError=true)] 
		public static extern IntPtr SetParent(IntPtr hWChild, IntPtr hwParent);

		[DllImport("coredll", EntryPoint="CreateWindowExW", SetLastError=true)]
		public static extern IntPtr CreateWindowEx(
			IntPtr dwExStyle, 
			string lpClassName, 
			string lpWindowName, 
			IntPtr dwStyle, 
			int x, 
			int y, 
			int nWidth, 
			int nHeight, 
			IntPtr hWndParent, 
			IntPtr hMenu, 
			IntPtr hInstance, 
			IntPtr lpParam 
			); 
	}

	/// <summary>
	/// Notification header returned in WM_NOTIFY lParam
	/// </summary>
	public struct NMHDR
	{
		/// <summary>
		/// Window that has sent WM_NOTIFY
		/// </summary>
		public IntPtr  hwndFrom;
		/// <summary>
		/// Control ID of the window that sent the notification
		/// </summary>
		public int  idFrom;
		/// <summary>
		/// Notification code
		/// </summary>
		public Win32Window.NotificationMessage  code;         // NM_ code
	}


}

/* ============================== Change Log ==============================
 * ---------------------
 * Oct 20, Chris Tacke
 * - Added changes submitted by Chris Schletter
 *   - Added WS_VISIBLE
 *   - Added WM_SETTEXT
 *   - Added CreateWindow, SetWindowStyle
 * ---------------------
 *
 * ---------------------
 * Aug 22, Peter Foot
 * ---------------------
 * - Namespace changed to Win32
 * 
 * ---------------------
 * May 13, Alex Feinman
 * ---------------------
 * - Added CallWindowProc, GetCapture
 * - Added enum WindowMessage
 * ---------------------
 * ---------------------
 * May 12, Alex Yakhnin
 * ---------------------
 * - Added SetFocus, SetWindowPos and UpdateWindowStyle SetWindowStyle helper methods
 * ---------------------
 * ---------------------
 * May 9, Alex Feinman
 * ---------------------
 * - Added RegisterWindowMessage
 * ---------------------
 * ======================================================================== */
