using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace KellControls.KellTable.Models
{
	/// <summary>
	/// Stores Image related properties for a Cell
	/// </summary>
	internal class CellImageStyle
	{
		#region Class Data

		/// <summary>
		/// The Image displayed in the Cell
		/// </summary>
		private Image image;

		/// <summary>
		/// Determines how Images are sized in the Cell
		/// </summary>
		private ImageSizeMode imageSizeMode;

		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the CellImageStyle class with default settings
		/// </summary>
		public CellImageStyle()
		{
			this.image = null;
			this.imageSizeMode = ImageSizeMode.Normal;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the image that is displayed in the Cell
		/// </summary>
		public Image Image
		{
			get
			{
				return this.image;
			}

			set
			{
				this.image = value;
			}
		}


		/// <summary>
		/// Gets or sets how the Cells image is sized within the Cell
		/// </summary>
		public ImageSizeMode ImageSizeMode
		{
			get
			{
				return this.imageSizeMode;
			}

			set
			{
				if (!Enum.IsDefined(typeof(ImageSizeMode), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(ImageSizeMode));
				}
				
				if (this.imageSizeMode != value)
				{
					this.imageSizeMode = value;
				}
			}
		}

		#endregion
	}
}
