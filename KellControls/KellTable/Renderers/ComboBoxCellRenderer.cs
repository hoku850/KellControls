using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using KellControls.KellTable.Events;
using KellControls.KellTable.Models;
using KellControls.KellTable.Themes;


namespace KellControls.KellTable.Renderers
{
	/// <summary>
	/// A CellRenderer that draws Cell contents as a ComboBox
	/// </summary>
	public class ComboBoxCellRenderer : DropDownCellRenderer
	{
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the ComboBoxCellRenderer class with 
		/// default settings
		/// </summary>
		public ComboBoxCellRenderer() : base()
		{
			
		}

		#endregion


		#region Events

		#region Paint

		/// <summary>
		/// Raises the Paint event
		/// </summary>
		/// <param name="e">A PaintCellEventArgs that contains the event data</param>
		protected override void OnPaint(PaintCellEventArgs e)
		{
			base.OnPaint(e);

			// don't bother going any further if the Cell is null 
			if (e.Cell == null)
			{
				return;
			}

			Rectangle buttonRect = this.CalcDropDownButtonBounds();

			Rectangle textRect = this.ClientRectangle;

			if (this.ShowDropDownButton)
			{
				textRect.Width -= buttonRect.Width - 1;
			}

			// draw the text
			if (e.Cell.Text != null && e.Cell.Text.Length != 0)
			{
				if (e.Enabled)
				{
					e.Graphics.DrawString(e.Cell.Text, this.Font, this.ForeBrush, textRect, this.StringFormat);
				}
				else
				{
					e.Graphics.DrawString(e.Cell.Text, this.Font, this.GrayTextBrush, textRect, this.StringFormat);
				}
			}
			
			if (e.Focused && e.Enabled)
			{
				Rectangle focusRect = this.ClientRectangle;

				if (this.ShowDropDownButton)
				{
					focusRect.Width -= buttonRect.Width;
				}
				
				ControlPaint.DrawFocusRectangle(e.Graphics, focusRect);
			}
		}

		#endregion

		#endregion
	}
}
