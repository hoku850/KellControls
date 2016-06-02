using System;


namespace KellControls.KellTable.Editors
{
	/// <summary>
	/// Specifies the DropDownCellEditor style
	/// </summary>
	public enum DropDownStyle
	{
		/// <summary>
		/// The text portion is editable. The user must click the arrow 
		/// button to display the list portion
		/// </summary>
		DropDown = 1,

		/// <summary>
		/// The user cannot directly edit the text portion. The user must 
		/// click the arrow button to display the list portion
		/// </summary>
		DropDownList = 2
	}
}