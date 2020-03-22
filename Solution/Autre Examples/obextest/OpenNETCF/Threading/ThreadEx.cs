//==========================================================================================
//
//		OpenNETCF.Threading.ThreadEx
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
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using OpenNETCF.Win32;

namespace OpenNETCF.Threading
{
	/// <summary>
	/// Creates and controls a thread, sets its priority, and gets its status.
	/// <para><b>New in v1.1</b></para>
	/// </summary>
	public class ThreadEx
	{
		Thread				m_thread;
		ThreadStart			m_start;
		IntPtr				m_handle;
		IntPtr				m_terminationEvent;
		string				m_name;
		ThreadState			m_state;

		#region static methods
		// these method maintain the Microsoft Thread interface so users don't have to use both Thread classes

		/// <summary>
		/// Blocks the current thread for the specified number of milliseconds.
		/// </summary>
		/// <param name="millisecondsTimeout">Amount of time to block</param>
		public static void Sleep(int millisecondsTimeout)
		{
			Thread.Sleep(millisecondsTimeout);
		}

		/// <summary>
		/// Blocks the current thread for the specified span of time.
		/// </summary>
		/// <param name="timeout">Amount of time to block</param>
		public static void Sleep(TimeSpan timeout)
		{
			Sleep(Convert.ToInt32(timeout.TotalMilliseconds));
		}
		
		/// <summary>
		/// Allocates an unnamed data slot on all the threads.
		/// </summary>
		/// <returns>A <see cref="LocalDataStoreSlot"/>.</returns>
		public static System.LocalDataStoreSlot AllocateDataSlot()
		{
			return Thread.AllocateDataSlot();
		}

		/// <summary>
		/// Allocates a named data slot on all threads.
		/// </summary>
		/// <param name="name">The name of the data slot to be allocated.</param>
		/// <returns>A <see cref="LocalDataStoreSlot"/>.</returns>
		public static System.LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			return Thread.AllocateNamedDataSlot(name);
		}

		/// <summary>
		/// Eliminates the association between a name and a slot, for all threads in the process.
		/// </summary>
		/// <param name="name">The name of the data slot to be freed.</param>
		public static void FreeNamedDataSlot(string name)
		{
			Thread.FreeNamedDataSlot(name);
		}

		/// <summary>
		/// Retrieves the value from the specified slot on the current thread.
		/// </summary>
		/// <param name="slot">The <see cref="LocalDataStoreSlot"/> from which to get the value.</param>
		/// <returns>The value retrieved</returns>
		public static object GetData(LocalDataStoreSlot slot)
		{
			return Thread.GetData(slot);
		}

		/// <summary>
		/// Looks up a named data slot.
		/// </summary>
		/// <param name="name">The name of the local data slot.</param>
		/// <returns>A <see cref="LocalDataStoreSlot"/> allocated for this thread.</returns>
		public static System.LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			return Thread.GetNamedDataSlot(name);
		}

		/// <summary>
		/// Sets the data in the specified slot on the currently running thread, for that thread's current domain.
		/// </summary>
		/// <param name="slot">The <see cref="LocalDataStoreSlot"/> in which to set the value.</param>
		/// <param name="data">The value to be set.</param>
		public static void SetData(LocalDataStoreSlot slot, object data)
		{
			Thread.SetData(slot, data);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name of the thread.
		/// </summary>
		/// <value>A string containing the name of the thread, or a null reference (Nothing in Visual Basic) if no name was set.</value>
		public string Name
		{
			get { return m_name; }
			set
			{
				if(m_name != null)
					throw new InvalidOperationException("Thread Name already set");

				m_name = value;
			}
		}

		/// <summary>
		/// Gets a value indicating the execution status of the current thread.
		/// </summary>
		/// <value><b>true</b> if this thread has been started and has not terminated normally or aborted; otherwise, <b>false</b>.</value>
		public bool IsAlive
		{
			get
			{ 
				if((m_state == ThreadState.Stopped)	| (m_state == ThreadState.Unstarted))
					return false;

				return true;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating the scheduling priority of a thread.
		/// </summary>
		/// <value>One of the <see cref="System.Threading.ThreadPriority"/> values. The default value is Normal.</value>
		public System.Threading.ThreadPriority Priority
		{
			get{ return m_thread.Priority; }
			set
			{ 
				if(m_state == ThreadState.Stopped)
					throw new ThreadStateException();

				m_thread.Priority = value; 
			}
		}

		/// <summary>
		/// Returns the ThreadEx instance's current <see cref="ThreadState"/>
		/// </summary>
		public ThreadState State
		{
			get { return m_state; }
		}

		#endregion

		/// <summary>
		/// Returns a <see cref="System.Threading.Thread"/> equivalent for the ThreadEx instance
		/// </summary>
		/// <param name="threadEx">The ThreadEx to convert</param>
		/// <returns>A <see cref="System.Threading.Thread"/></returns>
		public static implicit operator Thread (ThreadEx threadEx)
		{
			return threadEx.m_thread;
		}

		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="start">A <see cref="System.Threading.ThreadStart"/> delegate that references the methods to be invoked when this thread begins executing.</param>
		public ThreadEx(System.Threading.ThreadStart start)
		{
			if(start == null)
				throw new ArgumentNullException();

			m_terminationEvent = OpenNETCF.Win32.Core.CreateEvent(true, false, OpenNETCF.GuidEx.NewGuid().ToString());

			m_start = start;
			m_thread = new Thread(new ThreadStart(ShimProc));
			m_name = null;
			m_state = ThreadState.Unstarted;
		}

		/// <summary>
		/// Causes the operating system to change the state of the current instance to <see cref="ThreadState.Running"/>.
		/// </summary>
		public void Start()
		{
			lock(this)
			{
				m_thread.Start();
				m_handle = GetThreadHandle(m_thread);
				m_state = ThreadState.Running;
			}
		}

		/// <summary>
		/// Either suspends the thread, or if the thread is already suspended, has no effect.
		/// </summary>
		public void Suspend()
		{
			if((m_state == ThreadState.Unstarted) || (m_state == ThreadState.Stopped))
				throw new ThreadStateException();

			m_state = ThreadState.SuspendRequested;

			lock(this)
			{
				SuspendThread(m_handle);
			}

			m_state = ThreadState.Suspended;
		}

		/// <summary>
		/// Resumes a thread that has been suspended.
		/// </summary>
		public void Resume()
		{
			if(m_state != ThreadState.Suspended)
				throw new ThreadStateException();

			lock(this)
			{
				ResumeThread(m_handle);
			}
			m_state = ThreadState.Running;
		}

		private static IntPtr GetThreadHandle(Thread thread)
		{
			// ah, the magic of reflection!  Thanks to Alex Feinman for helping me on this
			return ((IntPtr)typeof(Thread).GetField("m_ThreadId", BindingFlags.NonPublic|BindingFlags.Instance).GetValue(thread));
		}

		/// <summary>
		/// Terminates the running thread.  This method will <b>always</b> throw a <see cref="BestPracticeViolationException"/>
		/// </summary>
		public void Abort()
		{	
			lock(this)
			{
				// terminate the thread
				TerminateThread(m_handle, 0);

				// set the termination event to notify Joins
				Core.SetEvent(m_terminationEvent);

				// set the state
				m_state = ThreadState.Stopped;

				// yield
				Thread.Sleep(0);
			}

			throw new BestPracticeViolationException("Aborting a thread is almost always bad practice.  Consider rearchitecting the calling code.");
		}

		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses.
		/// </summary>
		/// <returns><b>true</b> if the thread has terminated;</returns>
		public bool Join()
		{
			return Join(Core.INFINITE);
		}

		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait for the thread to terminate.</param>
		/// <returns><b>true</b> if the thread has terminated; <b>false</b> if the thread has not terminated after the amount of time specified by the <i>millisecondsTimeout</i> parameter has elapsed.</returns>
		public bool Join(int millisecondsTimeout)
		{
			return Join((uint)millisecondsTimeout);
		}

		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses.
		/// </summary>
		/// <param name="timeout"></param>
		/// <returns><b>true</b> if the thread has terminated; <b>false</b> if the thread has not terminated after the amount of time specified by the <i>timeout</i> parameter has elapsed.</returns>
		public bool Join(TimeSpan timeout)
		{
			return Join(Convert.ToInt32(timeout.TotalMilliseconds));
		}

		private bool Join(uint timeout)
		{
			// mimic full framework functionality
			if((m_state == ThreadState.Unstarted) || (m_state == ThreadState.Stopped))
				throw new ThreadStateException();

			lock(this)
			{
				m_state = ThreadState.WaitSleepJoin;

				if(Core.WaitForSingleObject(m_terminationEvent, timeout) == Core.Wait.Object)
				{
					m_state = ThreadState.Stopped;
					return true;
				}

				return false;
			}
		}

		private void ShimProc()
		{
			// call into actual thread proc
			m_start();

			// set the termination event to notify Joins
			Core.SetEvent(m_terminationEvent);

			// set the thread to stopped
			m_state = ThreadState.Stopped;

			// yield
			Thread.Sleep(0);
		}

		[DllImport("coredll.dll", EntryPoint="TerminateThread", SetLastError = true)]
		private static extern bool TerminateThread(IntPtr hThread, uint dwExitCode);

		[DllImport("coredll.dll", EntryPoint="SuspendThread", SetLastError = true)]
		private static extern uint SuspendThread(IntPtr hThread);

		[DllImport("coredll.dll", EntryPoint="ResumeThread", SetLastError = true)]
		private static extern uint ResumeThread(IntPtr hThread);
	}

	/// <summary>
	/// Specifies the execution states of a <see cref="ThreadEx"/>.
	/// </summary>
	public enum ThreadState : int
	{
		/// <summary>
		/// Thread is unstarted
		/// </summary>
		Unstarted,
		/// <summary>
		/// Thread is running
		/// </summary>
		Running,
		/// <summary>
		/// Thread is waiting in a Join
		/// </summary>
		WaitSleepJoin,
		/// <summary>
		/// Suspend has been called but not acted upon
		/// </summary>
		SuspendRequested,
		/// <summary>
		/// Thread is suspended
		/// </summary>
		Suspended,
		/// <summary>
		/// Thread has either terminated or been Aborted
		/// </summary>
		Stopped
	}
}
