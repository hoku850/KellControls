using System;


namespace KellControls.KellTable.Themes
{
	/// <summary>
	/// Represents the different states of a ListView
	/// </summary>
	public enum ListViewStates
	{
		/// <summary>
		/// The ListView is in its normal state
		/// </summary>
		Normal = 1,
		
		/// <summary>
		/// The ListView is highlighted
		/// </summary>
		Hot = 2,
		
		/// <summary>
		/// The ListView is selected
		/// </summary>
		Selected = 3,
		
		/// <summary>
		/// The ListView is disabled
		/// </summary>
		Disabled = 4,
		
		/// <summary>
		/// The ListView is selected but does not have focus
		/// </summary>
		SelectedNotFocus = 5
	}
}
