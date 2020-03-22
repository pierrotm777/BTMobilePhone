//==========================================================================================
//
//		OpenNETCF.BestPracticeViolationException
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

namespace OpenNETCF
{
	/// <summary>
	/// This exception is thrown in situations where calling code violates a "best practice".  If you get this exception, it is advised that you consider rearchitecting the calling code.
	/// <para><b>New in v1.1</b></para>
	/// </summary>
	/// <seealso cref="Threading.ThreadEx.Abort"/>
	public class BestPracticeViolationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the BestPracticeViolationException class. 
		/// </summary>
		/// <param name="message">Message to set for the Exception</param>
		public BestPracticeViolationException(string message) : base(message)
		{
		}
	}
}
