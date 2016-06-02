using System;
using System.Drawing;
using System.Windows.Forms;

using KellControls.KellTable.Events;
using KellControls.KellTable.Models;
using KellControls.KellTable.Win32;


namespace KellControls.KellTable.Editors
{
	/// <summary>
	/// A class for editing Cells that contain strings
	/// </summary>
	public class TextCellEditor : CellEditor
	{
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the TextCellEditor class with default settings
		/// </summary>
		public TextCellEditor() : base()
		{
			TextBox textbox = new TextBox();
			textbox.AutoSize = false;
			textbox.BorderStyle = BorderStyle.None;

			this.Control = textbox;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Sets the location and size of the CellEditor
		/// </summary>
		/// <param name="cellRect">A Rectangle that represents the size and location 
		/// of the Cell being edited</param>
		protected override void SetEditLocation(Rectangle cellRect)
		{
			this.TextBox.Location = cellRect.Location;
			this.TextBox.Size = new Size(cellRect.Width-1, cellRect.Height-1);
		}


		/// <summary>
		/// Sets the initial value of the editor based on the contents of 
		/// the Cell being edited
		/// </summary>
		protected override void SetEditValue()
		{
			this.TextBox.Text = this.EditingCell.Text;
		}


		/// <summary>
		/// Sets the contents of the Cell being edited based on the value 
		/// in the editor
		/// </summary>
		protected override void SetCellValue()
		{
			this.EditingCell.Text = this.TextBox.Text;
		}


		/// <summary>
		/// Starts editing the Cell
		/// </summary>
		public override void StartEditing()
		{
			this.TextBox.KeyPress += new KeyPressEventHandler(OnKeyPress);
			this.TextBox.LostFocus += new EventHandler(OnLostFocus);
			
			base.StartEditing();

			this.TextBox.Focus();
		}


		/// <summary>
		/// Stops editing the Cell and commits any changes
		/// </summary>
		public override void StopEditing()
		{
			this.TextBox.KeyPress -= new KeyPressEventHandler(OnKeyPress);
			this.TextBox.LostFocus -= new EventHandler(OnLostFocus);
			
			base.StopEditing();
		}


		/// <summary>
		/// Stops editing the Cell and ignores any changes
		/// </summary>
		public override void CancelEditing()
		{
			this.TextBox.KeyPress -= new KeyPressEventHandler(OnKeyPress);
			this.TextBox.LostFocus -= new EventHandler(OnLostFocus);
			
			base.CancelEditing();
		}


		#endregion


		#region Properties

		/// <summary>
		/// Gets the TextBox used to edit the Cells contents
		/// </summary>
		public TextBox TextBox
		{
			get
			{
				return this.Control as TextBox;
			}
		}

		#endregion


		#region Events

		/// <summary>
		/// Handler for the editors TextBox.KeyPress event
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">A KeyPressEventArgs that contains the event data</param>
		protected virtual void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == AsciiChars.CarriageReturn /*Enter*/)
			{
				if (this.EditingTable != null)
				{
					this.EditingTable.StopEditing();
				}
			}
			else if (e.KeyChar == AsciiChars.Escape)
			{
				if (this.EditingTable != null)
				{
					this.EditingTable.CancelEditing();
				}
			}
		}


		/// <summary>
		/// Handler for the editors TextBox.LostFocus event
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">An EventArgs that contains the event data</param>
		protected virtual void OnLostFocus(object sender, EventArgs e)
		{
			if (this.EditingTable != null)
			{
				this.EditingTable.StopEditing();
			}
		}

		#endregion
	}
}
