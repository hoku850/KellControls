using System;


namespace KellControls.KellTable.Themes
{
	/// <summary>
	/// Represents the different states of a Column Header
	/// </summary>
	public enum ColumnHeaderStates
	{
		/// <summary>
		/// The Column Header is in its normal state
		/// </summary>
		Normal = 1,
		
		/// <summary>
		/// The Column Header is highlighted
		/// </summary>
		Hot = 2,
		
		/// <summary>
		/// The Column Header is being pressed by the mouse
		/// </summary>
		Pressed = 3
	}
}
