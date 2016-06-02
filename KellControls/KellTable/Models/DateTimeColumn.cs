using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using KellControls.KellTable.Editors;
using KellControls.KellTable.Events;
using KellControls.KellTable.Models.Design;
using KellControls.KellTable.Renderers;
using KellControls.KellTable.Sorting;


namespace KellControls.KellTable.Models
{
	/// <summary>
	/// Represents a Column whose Cells are displayed as a DateTime
	/// </summary>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class DateTimeColumn : DropDownColumn
	{
		#region Class Data

		/// <summary>
		/// Default long date format
		/// </summary>
		public static readonly string LongDateFormat = DateTimeFormatInfo.CurrentInfo.LongDatePattern;

		/// <summary>
		/// Default short date format
		/// </summary>
		public static readonly string ShortDateFormat = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

		/// <summary>
		/// Default time format
		/// </summary>
		public static readonly string TimeFormat = DateTimeFormatInfo.CurrentInfo.LongTimePattern;

		/// <summary>
		/// The format of the date and time displayed in the Cells
		/// </summary>
		private DateTimePickerFormat dateFormat;

		/// <summary>
		/// The custom date/time format string
		/// </summary>
		private string customFormat;

		#endregion
		
		
		#region Constructor
		
		/// <summary>
		/// Creates a new DateTimeColumn with default values
		/// </summary>
        public DateTimeColumn(Type dataType)
            : base(dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DateTimeColumn with the specified header text
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
        public DateTimeColumn(string text, Type dataType)
            : base(text, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DateTimeColumn with the specified header text and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
        public DateTimeColumn(string text, int width, Type dataType)
            : base(text, width, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DateTimeColumn with the specified header text, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public DateTimeColumn(string text, int width, bool visible, Type dataType)
            : base(text, width, visible, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DateTimeColumn with the specified header text and image
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
        public DateTimeColumn(string text, Image image, Type dataType)
            : base(text, image, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DateTimeColumn with the specified header text, image and width
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
        public DateTimeColumn(string text, Image image, int width, Type dataType)
            : base(text, image, width, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Creates a new DateTimeColumn with the specified header text, image, width and visibility
		/// </summary>
		/// <param name="text">The text displayed in the column's header</param>
		/// <param name="image">The image displayed on the column's header</param>
		/// <param name="width">The column's width</param>
		/// <param name="visible">Specifies whether the column is visible</param>
        public DateTimeColumn(string text, Image image, int width, bool visible, Type dataType)
            : base(text, image, width, visible, dataType)
		{
			this.Init();
		}


		/// <summary>
		/// Initializes the DateTimeColumn with default values
		/// </summary>
		internal void Init()
		{
			this.dateFormat = DateTimePickerFormat.Long;
			this.customFormat = DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + DateTimeFormatInfo.CurrentInfo.LongTimePattern;
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
			return "DATETIME";
		}


		/// <summary>
		/// Gets the Column's default CellRenderer
		/// </summary>
		/// <returns>The Column's default CellRenderer</returns>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new DateTimeCellRenderer();
		}


		/// <summary>
		/// Gets a string that specifies the name of the Column's default CellEditor
		/// </summary>
		/// <returns>A string that specifies the name of the Column's default 
		/// CellEditor</returns>
		public override string GetDefaultEditorName()
		{
			return "DATETIME";
		}


		/// <summary>
		/// Gets the Column's default CellEditor
		/// </summary>
		/// <returns>The Column's default CellEditor</returns>
		public override ICellEditor CreateDefaultEditor()
		{
			return new DateTimeCellEditor();
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the format of the date and time displayed in the Column's Cells
		/// </summary>
		[Category("Appearance"),
		DefaultValue(DateTimePickerFormat.Long),
		Description("The format of the date and time displayed in the Column's Cells")]
		public DateTimePickerFormat DateTimeFormat
		{
			get
			{
				return this.dateFormat;
			}

			set
			{
				if (!Enum.IsDefined(typeof(DateTimePickerFormat), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(DateTimePickerFormat));
				}
					
				if (this.dateFormat != value)
				{
					this.dateFormat = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}


		/// <summary>
		/// Gets or sets the custom date/time format string
		/// </summary>
		[Category("Appearance"),
		Description("The custom date/time format string")]
		public string CustomDateTimeFormat
		{
			get
			{
				return this.customFormat;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("CustomFormat cannot be null");
				}

				if (!this.customFormat.Equals(value))
				{
					this.customFormat = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}

				DateTime.Now.ToString(DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
			}
		}


		/// <summary>
		/// Specifies whether the CustomDateTimeFormat property should be serialized at 
		/// design time
		/// </summary>
		/// <returns>true if the CustomDateTimeFormat property should be serialized, 
		/// false otherwise</returns>
		private bool ShouldSerializeCustomDateTimeFormat()
		{
			return !this.customFormat.Equals(DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + DateTimeFormatInfo.CurrentInfo.LongTimePattern);
		}


		/// <summary>
		/// Gets or sets the string that specifies how the Column's Cell contents 
		/// are formatted
		/// </summary>
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string Format
		{
			get
			{
				return this.CustomDateTimeFormat;
			}

			set
			{
				this.CustomDateTimeFormat = value;
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
				return typeof(DateTimeComparer);
			}
		}

		#endregion
	}
}
