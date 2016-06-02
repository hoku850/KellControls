using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using KellControls.KellTable.Events;
using KellControls.KellTable.Models;
using KellControls.KellTable.Themes;


namespace KellControls.KellTable.Renderers
{
	/// <summary>
	/// A HeaderRenderer that draws flat Column headers
	/// </summary>
	public class FlatHeaderRenderer : HeaderRenderer
	{
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the XPHeaderRenderer class 
		/// with default settings
		/// </summary>
		public FlatHeaderRenderer() : base()
		{
			this.SetBackBrushColor(SystemColors.Control);
		}

		#endregion


		#region Events

		#region Paint

		/// <summary>
		/// Raises the PaintBackground event
		/// </summary>
		/// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
		protected override void OnPaintBackground(PaintHeaderEventArgs e)
		{
			base.OnPaintBackground(e);

			e.Graphics.FillRectangle(this.BackBrush, this.Bounds);
		}


		/// <summary>
		/// Raises the Paint event
		/// </summary>
		/// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
		protected override void OnPaint(PaintHeaderEventArgs e)
		{
			base.OnPaint(e);

			if (e.Column == null)
			{
				return;
			}

			Rectangle textRect = this.ClientRectangle;
			Rectangle imageRect = Rectangle.Empty;

			if (e.Column.Image != null)
			{
				imageRect = this.CalcImageRect();

				textRect.Width -= imageRect.Width;
				textRect.X += imageRect.Width;

				if (e.Column.ImageOnRight)
				{
					imageRect.X = this.ClientRectangle.Right - imageRect.Width;
					textRect.X = this.ClientRectangle.X;
				}

				this.DrawColumnHeaderImage(e.Graphics, e.Column.Image, imageRect, e.Column.Enabled);
			}

			if (e.Column.SortOrder != SortOrder.None)
			{
				Rectangle arrowRect = this.CalcSortArrowRect();
				
				arrowRect.X = textRect.Right - arrowRect.Width;
				textRect.Width -= arrowRect.Width;

				this.DrawSortArrow(e.Graphics, arrowRect, e.Column.SortOrder, e.Column.Enabled);
			}

			if (e.Column.Text == null)
			{
				return;
			}

			if (e.Column.Text.Length > 0 && textRect.Width > 0)
			{
				if (e.Column.Enabled)
				{
					e.Graphics.DrawString(e.Column.Text, this.Font, this.ForeBrush, textRect, this.StringFormat);
				}
				else
				{
					using (SolidBrush brush = new SolidBrush(SystemPens.GrayText.Color))
					{
						e.Graphics.DrawString(e.Column.Text, this.Font, brush, textRect, this.StringFormat);
					}
				}
			}
		}

		#endregion

		#endregion
	}
}
