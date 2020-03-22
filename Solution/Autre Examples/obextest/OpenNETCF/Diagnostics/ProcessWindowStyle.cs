//==========================================================================================
//
//		OpenNETCF.Diagnostics.ProcessWindowStyle
//		Copyright (c) 2004, OpenNETCF.org
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

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Specified how a new window should appear when the system starts a process.
	/// </summary>
	public enum ProcessWindowStyle
	{
		/// <summary>
		/// The hidden window style.
		/// A window can be either visible or hidden.
		/// The system displays a hidden window by not drawing it.
		/// If a window is hidden, it is effectively disabled.
		/// A hidden window can process messages from the system or from other windows, but it cannot process input from the user or display output.
		/// Frequently, an application may keep a new window hidden while it customizes the window's appearance, and then make the window style Normal .
		/// </summary>
		Hidden = 0,

		/// <summary>
		/// The maximized window style.
		/// By default, the system enlarges a maximized window so that it fills the screen or, in the case of a child window, the parent window's client area.
		/// If the window has a title bar, the system automatically moves it to the top of the screen or to the top of the parent window's client area.
		/// Also, the system disables the window's sizing border and the window-positioning capability of the title bar so that the user cannot move the window by dragging the title bar.
		/// </summary>
		Maximized = 3,
		/// <summary>
		/// The minimized window style.
		/// By default, the system reduces a minimized window to the size of its taskbar button and moves the minimized window to the taskbar.
		/// </summary>
		Minimized = 2,
		

		/// <summary>
		/// The normal, visible window style.
		/// The system displays a window with Normal style on the screen, in a default location.
		/// If a window is visible, the user can supply input to the window and view the window's output.
		/// Frequently, an application may initialize a new window to the Hidden style while it customizes the window's appearance, and then make the window style Normal.
		/// </summary>
		Normal = 1,
	}
}
 

 