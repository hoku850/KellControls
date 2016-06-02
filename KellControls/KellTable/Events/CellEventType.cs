using System;


namespace KellControls.KellTable.Events
{
	/// <summary>
	/// Specifies the type of event generated when the value of a 
	/// Cell's property changes
	/// </summary>
	public enum CellEventType
	{
		/// <summary>
		/// Occurs when the Cell's property change type is unknown
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Occurs when the value displayed by a Cell has changed
		/// </summary>
		ValueChanged = 1,

		/// <summary>
		/// Occurs when the value of a Cell's Font property changes
		/// </summary>
		FontChanged = 2,

		/// <summary>
		/// Occurs when the value of a Cell's BackColor property changes
		/// </summary>
		BackColorChanged = 3,

		/// <summary>
		/// Occurs when the value of a Cell's ForeColor property changes
		/// </summary>
		ForeColorChanged = 4,

		/// <summary>
		/// Occurs when the value of a Cell's CellStyle property changes
		/// </summary>
		StyleChanged = 5,

		/// <summary>
		/// Occurs when the value of a Cell's Padding property changes
		/// </summary>
		PaddingChanged = 6,

		/// <summary>
		/// Occurs when the value of a Cell's Editable property changes
		/// </summary>
		EditableChanged = 7,

		/// <summary>
		/// Occurs when the value of a Cell's Enabled property changes
		/// </summary>
		EnabledChanged = 8,

		/// <summary>
		/// Occurs when the value of a Cell's ToolTipText property changes
		/// </summary>
		ToolTipTextChanged = 9,

		/// <summary>
		/// Occurs when the value of a Cell's CheckState property changes
		/// </summary>
		CheckStateChanged = 10,

		/// <summary>
		/// Occurs when the value of a Cell's ThreeState property changes
		/// </summary>
		ThreeStateChanged = 11,

		/// <summary>
		/// Occurs when the value of a Cell's Image property changes
		/// </summary>
		ImageChanged = 12,

		/// <summary>
		/// Occurs when the value of a Cell's ImageSizeMode property changes
		/// </summary>
		ImageSizeModeChanged = 13
	}
}
