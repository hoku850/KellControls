using System;


namespace KellControls.KellTable.Models
{
	/// <summary>
	/// Specifies how selected Cells are drawn by a Table
	/// </summary>
	public enum SelectionStyle
	{
		/// <summary>
		/// The first visible Cell in the selected Cells Row is drawn as selected
		/// </summary>
		ListView = 0,

		/// <summary>
		/// The selected Cells are drawn as selected
		/// </summary>
		Grid = 1
	}
}
