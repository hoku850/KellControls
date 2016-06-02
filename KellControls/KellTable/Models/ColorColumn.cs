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
	/// Represents a Column whose Cells are displayed as a Color
	/// </summary>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class ColorColumn : DropDownColumn
	{
		#region Class Data
		
		/// <summary>
		/// Specifies whether the Cells should draw their Color value
		/// </summary>
		private bool showColor;

		/// <summary>
		/// Specifies whether the Cells should draw their Color name
		/// </summary>
		private bool showColorName;

		#endregion


		#region Constructor
		
		/// <summary>
		/// Creates a new ColorColumn with default values
		/// </summary>
        public ColorColumn(Type dataType)
            : base(dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ColorColumn with the specified header text
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
        public ColorColumn(string text, Type dataType)
            : base(text, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ColorColumn with the specified header text and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
        public ColorColumn(string text, int width, Type dataType)
            : base(text, width, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ColorColumn with the specified header text, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public ColorColumn(string text, int width, bool visible, Type dataType)
            : base(text, width, visible, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ColorColumn with the specified header text and image
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
        public ColorColumn(string text, Image image, Type dataType)
            : base(text, image, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ColorColumn with the specified header text, image and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
        public ColorColumn(string text, Image image, int width, Type dataType)
            : base(text, image, width, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new ColorColumn with the specified header text, image, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public ColorColumn(string text, Image image, int width, bool visible, Type dataType)
            : base(text, image, width, visible, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Initializes the ColorColumn with default values
		/// </summary>
		private void Init()
		{
			this.showColor = true;
			this.showColorName = true;
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
			return "COLOR";
		}


		/// <summary>
		/// Gets the Column's default CellRenderer
		/// </summary>
		/// <returns>The Column's default CellRenderer</returns>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new ColorCellRenderer();
		}


		/// <summary>
		/// Gets a string that specifies the name of the Column's default CellEditor
		/// </summary>
		/// <returns>A string that specifies the name of the Column's default 
		/// CellEditor</returns>
		public override string GetDefaultEditorName()
		{
			return "COLOR";
		}


		/// <summary>
		/// Gets the Column's default CellEditor
		/// </summary>
		/// <returns>The Column's default CellEditor</returns>
		public override ICellEditor CreateDefaultEditor()
		{
			return new ColorCellEditor();
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets whether the Column's Cells should draw their Color value
		/// </summary>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Specifies whether the Column's Cells should draw their Color value")]
		public bool ShowColor
		{
			get
			{
				return this.showColor;
			}

			set
			{
				if (this.showColor != value)
				{
					this.showColor = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}


		/// <summary>
		/// Gets or sets whether the Column's Cells should draw their Color name
		/// </summary>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Specifies whether the Column's Cells should draw their Color name")]
		public bool ShowColorName
		{
			get
			{
				return this.showColorName;
			}

			set
			{
				if (this.showColorName != value)
				{
					this.showColorName = value;

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
				return typeof(ColorComparer);
			}
		}

		#endregion
	}
}
