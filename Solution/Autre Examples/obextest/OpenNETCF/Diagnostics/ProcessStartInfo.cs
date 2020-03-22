//==========================================================================================
//
//		OpenNETCF.Diagnostics.ProcessStartInfo
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
using System;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Specifies a set of values used when starting a process.
	/// </summary>
	/// <remarks><see cref="ProcessStartInfo"/> is used in conjunction with the <see cref="Process"/> component.
	/// When you start a process using the Process class, you have access to process information in addition to that available when attaching to a running process.
	/// You can use the <see cref="ProcessStartInfo"/> class for greater control over the process you start.
	/// You must at least set the <see cref="ProcessStartInfo.FileName"/> property, either manually or using the constructor.
	/// The file name is any application or document.
	/// Here a document is defined to be any file type that has an open or default action associated with it.</remarks>
	public sealed class ProcessStartInfo
	{
		private string m_filename;
		private string m_arguments;
		private bool m_shellexecute;
		private string m_verb;
		private ProcessWindowStyle m_style;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessStartInfo"/> class without specifying a file name with which to start the process.
		/// </summary>
		/// <remarks>You must set at least the <see cref="ProcessStartInfo.FileName"/> property before starting the process.
		/// The file name is any application or document.
		/// In this case, a document is defined to be any file type that has an open or default action associated with it.
		/// <para>Optionally, you can also set other properties before starting the process.
		/// The <see cref="Verb"/> property supplies actions to take, such as "print", with the file indicated in the <see cref="FileName"/> property.
		/// The <see cref="Arguments"/> property supplies a way to pass command line arguments to the file when the system opens it.</para></remarks>
		public ProcessStartInfo()
		{
			m_shellexecute = true;
			m_style = ProcessWindowStyle.Normal;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="T:OpenNETCF.Diagnostics.ProcessStartInfo"/> class and specifies a file name such as an application or document with which to start the process.
		/// </summary>
		/// <param name="fileName">An application or document with which to start a process.</param>
		/// <remarks>The file name is any application or document.
		/// In this case, a document is defined to be any file type that has an open or default action associated with it.</remarks>
		public ProcessStartInfo(string fileName) : this()
		{
			m_filename = fileName;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessStartInfo"/> class and specifies an application file name with which to start the process, as well as a set of command line arguments to pass to the application.
		/// </summary>
		/// <param name="fileName">An application with which to start a process.</param>
		/// <param name="arguments">Command line arguments to pass to the application when the process starts.</param>
		/// <remarks>The file name is any application or document.
		/// In this case, a document is defined to be any file type that has an open or default action associated with it.
		/// <para>You can change the <see cref="FileName"/> or <see cref="Arguments"/> properties after calling this constructor, up to the time that the process starts.
		/// After you start the process, changing these values has no effect.</para></remarks>
		public ProcessStartInfo(string fileName, string arguments) : this(fileName)
		{
			m_arguments = arguments;
		}
		#endregion

		#region Arguments
		/// <summary>
		/// Gets or sets the set of command line arguments to use when starting the application.
		/// </summary>
		/// <value>File type-specific arguments that the system can associate with the application specified in the <see cref="ProcessStartInfo.FileName"/> property.
		/// The default is an empty string ("").</value>
		public string Arguments
		{
			get
			{
				return m_arguments;
			}
			set
			{
				m_arguments = value;
			}
		}
		#endregion

		#region File Name
		/// <summary>
		///  Gets or sets the application or document to start.
		/// </summary>
		/// <value>The name of the application to start, or the name of a document of a file type that is associated with an application and that has a default open action available to it.
		/// The default is an empty string ("").</value>
		/// <remarks>You must set at least the <b>FileName</b> property before starting the process.
		/// The file name is any application or document.
		/// Here a document is defined to be any file type that has an open or default action associated with it.
		/// <para>The set of file types available to you depends in part on the value of the <see cref="ProcessStartInfo.UseShellExecute"/> property.
		/// If <see cref="ProcessStartInfo.UseShellExecute"/> is true, you are able to start any document and perform operations on the file, such as printing, with the <see cref="Process"/> component.
		/// When <see cref="ProcessStartInfo.UseShellExecute"/> is false, you are able to start only executables with the <see cref="Process"/> component.</para></remarks>
		public string FileName
		{
			get
			{
				return m_filename;
			}
			set
			{
				m_filename = value;
			}
		}
		#endregion

		#region Use Shell Execute
		/// <summary>
		/// Gets or sets a value indicating whether to use the operating system shell to start the process.
		/// </summary>
		/// <value><b>true</b> to use the shell when starting the process; otherwise, the process is created directly from the executable file.
		/// The default is <b>true</b>.</value>
		/// <remarks>When you use the operating system shell to start processes, you are able to start any document (which is any registered file type associated with an executable that has a default open action) and perform operations on the file, such as printing, with the <see cref="T:System.Diagnostics.Process"/> component.
		/// When <b>UseShellExecute</b> is <b>false</b>, you are able to start only executables with the <see cref="T:System.Diagnostics.Process"/> component.</remarks>
		public bool UseShellExecute
		{
			get
			{
				return m_shellexecute;
			}
			set
			{
				m_shellexecute = value;
			}
		}
		#endregion

		#region Verb
		/// <summary>
		/// Gets or sets the verb to use when opening the application or document specified by the <see cref="P:OpenNETCF.Diagnostics.ProcessStartInfo.FileName"/> property.
		/// </summary>
		/// <value>The action to take with the file that the process opens.
		/// The default is an empty string ("").</value>
		/// <remarks>Each file name extension has its own set of verbs.
		/// For example, the "print" verb will print a document specified using <see cref="ProcessStartInfo.FileName"/>.
		/// The default verb can be specified using an empty string ("").</remarks>
		public string Verb
		{
			get
			{
				return m_verb;
			}
			set
			{
				m_verb = value;
			}
		}
		#endregion

		#region Window Style
		/// <summary>
		/// Gets or sets the window state to use when the process is started.
		/// <para><b>Not currently supported.</b></para>
		/// </summary>
		/// <value>A <see cref="ProcessWindowStyle"/> that indicates whether the process is started in a window that is maximized, minimized, normal (neither maximized nor minimized), or not visible.
		/// The default is normal.</value>
		public ProcessWindowStyle WindowStyle
		{
			get
			{
				return m_style;
			}
			set
			{
				m_style = value;
			}
		}
		#endregion

	}
}
