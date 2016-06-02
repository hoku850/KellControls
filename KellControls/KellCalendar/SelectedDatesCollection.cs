using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;   

namespace KellControls
{

	/// <summary>
	/// Represents a collection of DateItem objects
	/// </summary>
	public class SelectedDatesCollection : ReadOnlyCollectionBase 
	{
		#region Class Data

		/// <summary>
		/// The Calendar that owns this DateItemCollection
		/// </summary>
		private KellCalendar owner;

		#endregion
		

		#region Constructor
				
		public SelectedDatesCollection(KellCalendar owner) : base()
		{
			if (owner == null)
				throw new ArgumentNullException("owner");
							
			this.owner = owner;
		}
			
		public SelectedDatesCollection(KellCalendar owner, SelectedDatesCollection dates) : this(owner)
		{
			
			if (owner == null)
				throw new ArgumentNullException("owner");
		
			this.owner = owner;
			
			this.Add(dates);
		}

		#endregion

		#region Methods
		

		public void Add(DateTime value)
		{
			int index;
	
			index = this.IndexOf(value);
			if (index == -1)
				this.InnerList.Add(value);
			else
				this.InnerList[index] = value;
		}

		public void AddRange(DateTime[] dates)
		{
			if (dates == null)
				throw new ArgumentNullException("dates");
			
			for (int i=0; i<dates.Length; i++)
			{				
				this.Add(dates[i]);
			}
		}

		public void Add(SelectedDatesCollection dates)
		{
			if (dates == null)
				throw new ArgumentNullException("dates");
			
			for (int i=0; i<dates.Count; i++)
			{
				this.Add(dates[i]);
			}
		}
			
		public void Clear()
		{
			while (this.Count > 0)
			{
				this.RemoveAt(0);
			}
		}

		public bool Contains(DateTime date)
		{
			return (this.IndexOf(date) != -1);
		}

		public int IndexOf(DateTime date)
		{
							
			for (int i=0; i<this.Count; i++)
			{
				if (this[i] == date)
				{
					return i;
				}
			}

			return -1;
		}
			
		public void Remove(DateTime value)
		{
			
			this.InnerList.Remove(value);
		
		}
			
		public void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		public void Move(DateTime value, int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			else if (index > this.Count)
			{
				index = this.Count;
			}

			if (!this.Contains(value) || this.IndexOf(value) == index)
			{
				return;
			}

			this.InnerList.Remove(value);

			if (index > this.Count)
			{
				this.InnerList.Add(value);
			}
			else
			{
				this.InnerList.Insert(index, value);
			}

		}

		public void MoveToTop(DateTime value)
		{
			this.Move(value, 0);
		}


		public void MoveToBottom(DateTime value)
		{
			this.Move(value, this.Count);
		}

		#endregion

		#region Properties

		public virtual DateTime this[int index]
		{
			get
			{
				DateTime d = (DateTime)this.InnerList[index];
				return d;
			}
		}

		#endregion

	}

}