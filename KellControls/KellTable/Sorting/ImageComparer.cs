using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

using KellControls.KellTable.Models;


namespace KellControls.KellTable.Sorting
{
	/// <summary>
	/// An IComparer for sorting Cells that contain Images
	/// </summary>
	public class ImageComparer : ComparerBase
	{
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the ImageComparer class with the specified 
		/// TableModel, Column index and SortOrder
		/// </summary>
		/// <param name="tableModel">The TableModel that contains the data to be sorted</param>
		/// <param name="column">The index of the Column to be sorted</param>
		/// <param name="sortOrder">Specifies how the Column is to be sorted</param>
		public ImageComparer(TableModel tableModel, int column, SortOrder sortOrder) : base(tableModel, column, sortOrder)
		{
			
		}

		#endregion


		#region Methods
		
		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less 
		/// than, equal to or greater than the other
		/// </summary>
		/// <param name="a">First object to compare</param>
		/// <param name="b">Second object to compare</param>
		/// <returns>-1 if a is less than b, 1 if a is greater than b, or 0 if a equals b</returns>
		public override int Compare(object a, object b)
		{
			Cell cell1 = (Cell) a;
			Cell cell2 = (Cell) b;
			
			// check for null cells
			if (cell1 == null && cell2 == null)
			{
				return 0;
			}
			else if (cell1 == null)
			{
				return -1;
			}
			else if (cell2 == null)
			{
				return 1;
			}

			int retVal = 0;

			// check for null data
			if (cell1.Data == null && cell2.Data == null)
			{
				retVal = 0;
			}
			else if (cell1.Data == null)
			{
				retVal = -1;
			}
			else if (cell2.Data == null)
			{
				retVal = 1;
			}

			// since images aren't comparable, if retVal = 0 and the ImageColumn 
			// they belong to allows text drawing, compare the text properties 
			// to determine order
			if (retVal == 0 && ((ImageColumn) this.TableModel.Table.ColumnModel.Columns[this.SortColumn]).DrawText)
			{
				// check for null data
				if (cell1.Text == null && cell2.Text == null)
				{
					return 0;
				}
				else if (cell1.Text == null)
				{
					return -1;
				}
			
				retVal = cell1.Text.CompareTo(cell2.Text);
			}

			return retVal;
		}

		#endregion
	}
}
