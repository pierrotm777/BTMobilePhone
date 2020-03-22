/*=======================================================================================

	OpenNETCF.Net.DestinationInfoCollection
	Copyright © 2003, OpenNETCF.org

	This library is free software; you can redistribute it and/or modify it under 
	the terms of the OpenNETCF.org Shared Source License.

	This library is distributed in the hope that it will be useful, but 
	WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
	FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
	for more details.

	You should have received a copy of the OpenNETCF.org Shared Source License 
	along with this library; if not, email licensing@opennetcf.org to request a copy.

	If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
	email licensing@opennetcf.org.

	For general enquiries, email enquiries@opennetcf.org or visit our website at:
	http://www.opennetcf.org

=======================================================================================*/
using System;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Summary description for DestinationInfoCollection.
	/// </summary>
	public class DestinationInfoCollection : System.Collections.CollectionBase
	{
		public DestinationInfoCollection()
		{
		}

		public DestinationInfoCollection(DestinationInfo[] items)
		{
			this.AddRange(items);
		}

		public DestinationInfoCollection(DestinationInfoCollection items)
		{
			this.AddRange(items);
		}

		public virtual void AddRange(DestinationInfo[] items)
		{
			foreach (DestinationInfo item in items)
			{
				this.List.Add(item);
			}
		}

		public virtual void AddRange(DestinationInfoCollection items)
		{
			foreach (DestinationInfo item in items)
			{
				this.List.Add(item);
			}
		}


		public virtual void Add(DestinationInfo value)
		{
			this.List.Add(value);
		}

		public virtual bool Contains(DestinationInfo value)
		{
			return this.List.Contains(value);
		}

		public virtual int IndexOf(DestinationInfo value)
		{
			return this.List.IndexOf(value);
		}

		public virtual void Insert(int index, DestinationInfo value)
		{
			this.List.Insert(index, value);
		}

		public virtual DestinationInfo this[int index]
		{
			get { return (DestinationInfo)this.List[index]; }
			set { this.List[index] = value; }
		}

		public virtual void Remove(DestinationInfo value)
		{
			this.List.Remove(value);
		}

		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(DestinationInfoCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public DestinationInfo Current
			{
				get { return (DestinationInfo)(this.wrapped.Current); }
			}

			object System.Collections.IEnumerator.Current
			{
				get	{ return (DestinationInfo)(this.wrapped.Current); }
			}

			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			public void Reset()
			{
				this.wrapped.Reset();
			}
		}

		public new virtual DestinationInfoCollection.Enumerator GetEnumerator()
		{
			return new DestinationInfoCollection.Enumerator(this);
		}
	}

}
