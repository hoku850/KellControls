using System;
using System.Collections;


namespace KellControls.KellTable.Models.Design
{
	/// <summary>
	/// 
	/// </summary>
	internal class RowComparer : IComparer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(object x, object y)
		{
			Row row1 = (Row) x;
			Row row2 = (Row) y;
			
			// check for null rows
			if (row1 == null && row2 == null)
			{
				return 0;
			}
			else if (row1 == null)
			{
				return -1;
			}
			else if (row2 == null)
			{
				return 1;
			}

			if (row1.InternalIndex < row2.InternalIndex)
			{
				return -1;
			}
			else if (row1.InternalIndex < row2.InternalIndex)
			{
				return 1;
			}

			return 0;
		}
	}
}
