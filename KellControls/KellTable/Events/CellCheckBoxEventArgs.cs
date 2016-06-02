using System;

using KellControls.KellTable.Models;


namespace KellControls.KellTable.Events
{
	#region Delegates

	/// <summary>
	/// Represents the method that will handle the CellCheckChanged event of a Table
	/// </summary>
	public delegate void CellCheckBoxEventHandler(object sender, CellCheckBoxEventArgs e);

	#endregion



	#region CellCheckBoxEventArgs
	
	/// <summary>
	/// Provides data for the CellCheckChanged event of a Table
	/// </summary>
	public class CellCheckBoxEventArgs : CellEventArgsBase
	{
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the CellButtonEventArgs class with 
		/// the specified Cell source, row index and column index
		/// </summary>
		/// <param name="source">The Cell that Raised the event</param>
		/// <param name="column">The Column index of the Cell</param>
		/// <param name="row">The Row index of the Cell</param>
		public CellCheckBoxEventArgs(Cell source, int column, int row) : base(source, column, column)
		{
			
		}

		#endregion
	}

	#endregion
}
