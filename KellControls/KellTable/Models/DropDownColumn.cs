using System;
using System.ComponentModel;
using System.Drawing;

using KellControls.KellTable.Editors;
using KellControls.KellTable.Events;
using KellControls.KellTable.Renderers;


namespace KellControls.KellTable.Models
{
	/// <summary>
	/// Represents a Column whose Cells are displayed with a drop down 
	/// button for editing
	/// </summary>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public abstract class DropDownColumn : Column
	{
		#region Class Data

		/// <summary>
		/// Specifies whether the Cells should draw a drop down button
		/// </summary>
		private bool showButton;

		#endregion


		#region Constructor
		
		/// <summary>
		/// Creates a new DropDownColumn with default values
		/// </summary>
        public DropDownColumn(Type dataType)
            : base(dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DropDownColumn with the specified header text
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
        public DropDownColumn(string text, Type dataType)
            : base(text, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DropDownColumn with the specified header text and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
        public DropDownColumn(string text, int width, Type dataType)
            : base(text, width, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DropDownColumn with the specified header text, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public DropDownColumn(string text, int width, bool visible, Type dataType)
            : base(text, width, visible, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DropDownColumn with the specified header text and image
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
        public DropDownColumn(string text, Image image, Type dataType)
            : base(text, image, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DropDownColumn with the specified header text, image and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
        public DropDownColumn(string text, Image image, int width, Type dataType)
            : base(text, image, width, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DropDownColumn with the specified header text, image, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public DropDownColumn(string text, Image image, int width, bool visible, Type dataType)
            : base(text, image, width, visible, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Initializes the DropDownColumn with default values
		/// </summary>
		private void Init()
		{
			this.showButton = true;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets whether the Column's Cells should draw a drop down button
		/// </summary>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Determines whether the Column's Cells should draw a drop down button")]
		public bool ShowDropDownButton
		{
			get
			{
				return this.showButton;
			}

			set
			{
				if(this.showButton != value)
				{
					this.showButton = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}

		#endregion
	}
}
