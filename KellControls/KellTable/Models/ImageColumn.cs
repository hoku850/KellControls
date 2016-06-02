using System;
using System.ComponentModel;
using System.Drawing;

using KellControls.KellTable.Editors;
using KellControls.KellTable.Events;
using KellControls.KellTable.Models.Design;
using KellControls.KellTable.Renderers;
using KellControls.KellTable.Sorting;


namespace KellControls.KellTable.Models
{
	/// <summary>
	/// Represents a Column whose Cells are displayed as an Image
	/// </summary>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class ImageColumn : Column
	{
		#region Class Data

		/// <summary>
		/// Specifies whether any text contained in the Cell should be drawn
		/// </summary>
		private bool drawText;

		#endregion
		
		
		#region Constructor
		
		/// <summary>
		/// Creates a new ImageColumn with default values
		/// </summary>
        public ImageColumn(Type dataType)
            : base(dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ImageColumn with the specified header text
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
        public ImageColumn(string text, Type dataType)
            : base(text, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ImageColumn with the specified header text and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
        public ImageColumn(string text, int width, Type dataType)
            : base(text, width, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ImageColumn with the specified header text, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public ImageColumn(string text, int width, bool visible, Type dataType)
            : base(text, width, visible, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ImageColumn with the specified header text and image
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
        public ImageColumn(string text, Image image, Type dataType)
            : base(text, image, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ImageColumn with the specified header text, image and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
        public ImageColumn(string text, Image image, int width, Type dataType)
            : base(text, image, width, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ImageColumn with the specified header text, image, width 
		/// and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public ImageColumn(string text, Image image, int width, bool visible, Type dataType)
            : base(text, image, width, visible, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Initializes the ImageColumn with default values
		/// </summary>
		private void Init()
		{
			this.drawText = true;
			this.Editable = false;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Gets a string that specifies the name of the Column's default CellRenderer
		/// </summary>
		/// <returns>A string that specifies the name of the Column's default 
		/// CellRenderer</returns>
		public override string GetDefaultRendererName()
		{
			return "IMAGE";
		}


		/// <summary>
		/// Gets the Column's default CellRenderer
		/// </summary>
		/// <returns>The Column's default CellRenderer</returns>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new ImageCellRenderer();
		}


		/// <summary>
		/// Gets a string that specifies the name of the Column's default CellEditor
		/// </summary>
		/// <returns>A string that specifies the name of the Column's default 
		/// CellEditor</returns>
		public override string GetDefaultEditorName()
		{
			return null;
		}


		/// <summary>
		/// Gets the Column's default CellEditor
		/// </summary>
		/// <returns>The Column's default CellEditor</returns>
		public override ICellEditor CreateDefaultEditor()
		{
			return null;
		}

		#endregion


		#region Properties
		
		/// <summary>
		/// Gets or sets whether any text contained in the Column's Cells should be drawn
		/// </summary>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Determines whether any text contained in the Column's Cells should be drawn")]
		public bool DrawText
		{
			get
			{
				return this.drawText;
			}

			set
			{
				if (this.drawText != value)
				{
					this.drawText = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}


		/// <summary>
		/// Gets the Type of the Comparer used to compare the Column's Cells when 
		/// the Column is sorting
		/// </summary>
		public override Type DefaultComparerType
		{
			get
			{
				return typeof(ImageComparer);
			}
		}


		/// <summary>
		/// Gets or sets a value indicating whether the Column's Cells contents 
		/// are able to be edited
		/// </summary>
		[Category("Appearance"),
		DefaultValue(false),
		Description("Controls whether the column's cell contents are able to be changed by the user")]
		public new bool Editable
		{
			get
			{
				return base.Editable;
			}

			set
			{
				base.Editable = value;
			}
		}

		#endregion
	}
}
