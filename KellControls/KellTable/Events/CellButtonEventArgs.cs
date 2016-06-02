using System;

using KellControls.KellTable.Models;


namespace KellControls.KellTable.Events
{
	#region Delegates

	/// <summary>
	/// Represents the method that will handle the CellButtonClicked event of a Table
	/// </summary>
	public delegate void CellButtonEventHandler(object sender, CellButtonEventArgs e);

	#endregion



	#region CellButtonEventArgs
	
	/// <summary>
	/// Provides data for the CellButtonClicked event of a Table
	/// </summary>
	public class CellButtonEventArgs : CellEventArgsBase
	{
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the CellButtonEventArgs class with 
		/// the specified Cell source, row index and column index
		/// </summary>
		/// <param name="source">The Cell that raised the event</param>
		/// <param name="column">The Column index of the Cell</param>
		/// <param name="row">The Row index of the Cell</param>
		public CellButtonEventArgs(Cell source, int column, int row) : base(source, column, column)
		{
			
		}

		#endregion
	}

	#endregion
}
