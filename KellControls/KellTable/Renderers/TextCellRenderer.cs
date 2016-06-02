using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using KellControls.KellTable.Editors;
using KellControls.KellTable.Events;
using KellControls.KellTable.Models;


namespace KellControls.KellTable.Renderers
{
	/// <summary>
	/// A CellRenderer that draws Cell contents as strings
	/// </summary>
	public class TextCellRenderer : CellRenderer
	{
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the TextCellRenderer class with 
		/// default settings
		/// </summary>
		public TextCellRenderer() : base()
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

			string text = e.Cell.Text;

			if (text != null && text.Length != 0)
			{
				if (e.Enabled)
				{
					e.Graphics.DrawString(text, this.Font, this.ForeBrush, this.ClientRectangle, this.StringFormat);
				}
				else
				{
					e.Graphics.DrawString(text, this.Font, this.GrayTextBrush, this.ClientRectangle, this.StringFormat);
				}
			}
			
			if (e.Focused && e.Enabled)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, this.ClientRectangle);
			}
		}

		#endregion

		#endregion
	}
}
