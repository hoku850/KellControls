using System;


namespace KellControls.KellTable.Editors
{
	/// <summary>
	/// Specifies the action that causes a Cell to start editing
	/// </summary>
	public enum EditStartAction
	{
		/// <summary>
		/// A double click will start cell editing
		/// </summary>
		DoubleClick = 1,

		/// <summary>
		/// A single click will start cell editing
		/// </summary>
		SingleClick = 2,

		/// <summary>
		/// A user defined key press will start cell editing
		/// </summary>
		CustomKey = 3
	}
}
