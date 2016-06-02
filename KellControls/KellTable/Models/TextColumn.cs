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
	/// Summary description for TextColumn.
	/// </summary>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class TextColumn : Column
	{
		#region Constructor
		
		/// <summary>
		/// Creates a new TextColumn with default values
		/// </summary>
        public TextColumn(Type dataType)
            : base(dataType)
		{

		}


		/// <summary>
		/// Creates a new TextColumn with the specified header text
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
        public TextColumn(string text, Type dataType)
            : base(text, dataType)
		{

		}


		/// <summary>
		/// Creates a new TextColumn with the specified header text and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
        public TextColumn(string text, int width, Type dataType)
            : base(text, width, dataType)
		{

		}


		/// <summary>
		/// Creates a new TextColumn with the specified header text, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public TextColumn(string text, int width, bool visible, Type dataType)
            : base(text, width, visible, dataType)
		{

		}


		/// <summary>
		/// Creates a new TextColumn with the specified header text and image
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
        public TextColumn(string text, Image image, Type dataType)
            : base(text, image, dataType)
		{

		}


		/// <summary>
		/// Creates a new TextColumn with the specified header text, image and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
        public TextColumn(string text, Image image, int width, Type dataType)
            : base(text, image, width, dataType)
		{

		}


		/// <summary>
		/// Creates a new TextColumn with the specified header text, image, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public TextColumn(string text, Image image, int width, bool visible, Type dataType)
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
			return "TEXT";
		}


		/// <summary>
		/// Gets the Column's default CellRenderer
		/// </summary>
		/// <returns>The Column's default CellRenderer</returns>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new TextCellRenderer();
		}


		/// <summary>
		/// Gets a string that specifies the name of the Column's default CellEditor
		/// </summary>
		/// <returns>A string that specifies the name of the Column's default 
		/// CellEditor</returns>
		public override string GetDefaultEditorName()
		{
			return "TEXT";
		}


		/// <summary>
		/// Gets the Column's default CellEditor
		/// </summary>
		/// <returns>The Column's default CellEditor</returns>
		public override ICellEditor CreateDefaultEditor()
		{
			return new TextCellEditor();
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
