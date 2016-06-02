using System;
using System.ComponentModel;
using System.Drawing;

using KellControls.KellTable.Editors;
using KellControls.KellTable.Models.Design;
using KellControls.KellTable.Renderers;
using KellControls.KellTable.Sorting;


namespace KellControls.KellTable.Models
{
	/// <summary>
	/// Represents a Column whose Cells are displayed as a ComboBox
	/// </summary>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class ComboBoxColumn : DropDownColumn
	{
		#region Constructor
		
		/// <summary>
		/// Creates a new ComboBoxColumn with default values
		/// </summary>
        public ComboBoxColumn(Type dataType)
            : base(dataType)
		{

		}


		/// <summary>
		/// Creates a new ComboBoxColumn with the specified header text
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
        public ComboBoxColumn(string text, Type dataType)
            : base(text, dataType)
		{

		}


		/// <summary>
		/// Creates a new ComboBoxColumn with the specified header text and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
        public ComboBoxColumn(string text, int width, Type dataType)
            : base(text, width, dataType)
		{

		}


		/// <summary>
		/// Creates a new ComboBoxColumn with the specified header text, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public ComboBoxColumn(string text, int width, bool visible, Type dataType)
            : base(text, width, visible, dataType)
		{
		
		}


		/// <summary>
		/// Creates a new ComboBoxColumn with the specified header text and image
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
        public ComboBoxColumn(string text, Image image, Type dataType)
            : base(text, image, dataType)
		{

		}


		/// <summary>
		/// Creates a new ComboBoxColumn with the specified header text, image and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
        public ComboBoxColumn(string text, Image image, int width, Type dataType)
            : base(text, image, width, dataType)
		{

		}


		/// <summary>
		/// Creates a new ComboBoxColumn with the specified header text, image, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public ComboBoxColumn(string text, Image image, int width, bool visible, Type dataType)
            : base(text, image, width, visible, dataType)
		{

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
			return "COMBOBOX";
		}


		/// <summary>
		/// Gets the Column's default CellRenderer
		/// </summary>
		/// <returns>The Column's default CellRenderer</returns>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new ComboBoxCellRenderer();
		}


		/// <summary>
		/// Gets a string that specifies the name of the Column's default CellEditor
		/// </summary>
		/// <returns>A string that specifies the name of the Column's default 
		/// CellEditor</returns>
		public override string GetDefaultEditorName()
		{
			return "COMBOBOX";
		}


		/// <summary>
		/// Gets the Column's default CellEditor
		/// </summary>
		/// <returns>The Column's default CellEditor</returns>
		public override ICellEditor CreateDefaultEditor()
		{
			return new ComboBoxCellEditor();
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the Type of the Comparer used to compare the Column's Cells when 
		/// the Column is sorting
		/// </summary>
		public override Type DefaultComparerType
		{
			get
			{
				return typeof(TextComparer);
			}
		}

		#endregion
	}
}
