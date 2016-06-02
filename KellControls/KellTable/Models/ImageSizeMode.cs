using System;


namespace KellControls.KellTable.Models
{
	/// <summary>
	/// Specifies how Images are sized within a Cell
	/// </summary>
	public enum ImageSizeMode
	{
		/// <summary>
		/// The Image will be displayed normally
		/// </summary>
		Normal = 0,

		/// <summary>
		/// The Image will be stretched/shrunken to fit the Cell
		/// </summary>
		SizedToFit = 1,

		/// <summary>
		/// The Image will be scaled to fit the Cell
		/// </summary>
		ScaledToFit = 2
	}
}
