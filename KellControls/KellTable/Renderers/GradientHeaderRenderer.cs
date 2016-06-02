using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using KellControls.KellTable.Events;
using KellControls.KellTable.Models;
using KellControls.KellTable.Themes;


namespace KellControls.KellTable.Renderers
{
	/// <summary>
	/// A HeaderRenderer that draws gradient Column headers
	/// </summary>
	public class GradientHeaderRenderer : HeaderRenderer
	{
		#region Class Data

		/// <summary>
		/// The start Color of the gradient
		/// </summary>
		private Color startColor;

		/// <summary>
		/// The ned Color of the gradient
		/// </summary>
		private Color endColor;

		/// <summary>
		/// The Color of the Column header when it is pressed
		/// </summary>
		private Color pressedColor; 

		#endregion

		
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the GradientHeaderRenderer class 
		/// with default settings
		/// </summary>
		public GradientHeaderRenderer() : base()
		{
			// steel blue gradient
			this.startColor = Color.FromArgb(200, 209, 215);
			this.endColor = Color.FromArgb(239, 239, 239);
			this.pressedColor = Color.Empty;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the start Color of the gradient
		/// </summary>
		[Category("Appearance"),
		Description("The start color of a ColumnHeaders gradient")]
		public Color StartColor
		{
			get
			{
				return this.startColor;
			}

			set
			{
				if (this.startColor != value)
				{
					this.startColor = value;
				}
			}
		}
		

		/// <summary>
		/// Gets or sets the end Color of the gradient
		/// </summary>
		[Category("Appearance"),
		Description("The end color of a ColumnHeaders gradient")]
		public Color EndColor
		{
			get
			{
				return this.endColor;
			}

			set
			{
				if (this.endColor != value)
				{
					this.endColor = value;
				}
			}
		}
		

		/// <summary>
		/// Gets or sets the Color of the Column header when it is pressed
		/// </summary>
		[Category("Appearance"),
		Description("The color of a ColumnHeader when it is in a pressed state")]
		public Color PressedColor
		{
			get
			{
				return this.pressedColor;
			}

			set
			{
				if (this.pressedColor != value)
				{
					this.pressedColor = value;
				}
			}
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

			if (e.Column == null || e.Column.ColumnState != ColumnState.Pressed)
			{
				using (LinearGradientBrush brush = new LinearGradientBrush(e.HeaderRect, this.StartColor, this.EndColor, LinearGradientMode.Vertical))
				{
					e.Graphics.FillRectangle(brush, e.HeaderRect);
				}

				using (Pen pen = new Pen(this.EndColor))
				{
					e.Graphics.DrawLine(pen, e.HeaderRect.Left, e.HeaderRect.Top, e.HeaderRect.Right-2, e.HeaderRect.Top);
					e.Graphics.DrawLine(pen, e.HeaderRect.Left, e.HeaderRect.Top, e.HeaderRect.Left, e.HeaderRect.Bottom-1);
				}

				using (Pen pen = new Pen(this.StartColor))
				{
					e.Graphics.DrawLine(pen, e.HeaderRect.Right-1, e.HeaderRect.Top, e.HeaderRect.Right-1, e.HeaderRect.Bottom-1);
					e.Graphics.DrawLine(pen, e.HeaderRect.Left+1, e.HeaderRect.Bottom-1, e.HeaderRect.Right-1, e.HeaderRect.Bottom-1);
				}
			}
			else
			{
				Color pressed = this.PressedColor;

				if (pressed == Color.Empty)
				{
					pressed = ControlPaint.Light(this.startColor);
				}
				
				using (SolidBrush brush = new SolidBrush(pressed))
				{
					e.Graphics.FillRectangle(brush, e.HeaderRect);
				}
				
				using (Pen pen = new Pen(this.StartColor))
				{
					e.Graphics.DrawRectangle(pen, e.HeaderRect.X, e.HeaderRect.Y, e.HeaderRect.Width-1, e.HeaderRect.Height-1);
				}
			}
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

				if (e.Column.ColumnState == ColumnState.Pressed)
				{
					imageRect.X += 1;
					imageRect.Y += 1;
				}

				this.DrawColumnHeaderImage(e.Graphics, e.Column.Image, imageRect, e.Column.Enabled);
			}

			if (e.Column.ColumnState == ColumnState.Pressed)
			{
				textRect.X += 1;
				textRect.Y += 1;
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
