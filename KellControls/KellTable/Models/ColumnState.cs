using System;


namespace KellControls.KellTable.Models
{
	/// <summary>
	/// Specifies the state of a Column
	/// </summary>
	public enum ColumnState
	{
		/// <summary>
		/// Column is in its normal state
		/// </summary>
		Normal = 1,

		/// <summary>
		/// Mouse is over the Column
		/// </summary>
		Hot = 2,

		/// <summary>
		/// Column is being pressed
		/// </summary>
		Pressed = 3
	}
}
