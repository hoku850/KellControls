using System;
using System.ComponentModel;
using System.Drawing;

using KellControls.KellTable.Themes;


namespace KellControls.KellTable.Renderers
{
	/// <summary>
	/// Contains information about the current state of a Cell's button
	/// </summary>
	public class ButtonRendererData
	{
		#region Class Data

		/// <summary>
		/// The current state of the button
		/// </summary>
		private PushButtonStates buttonState;
		
		/// <summary>
		/// The x coordinate of the last mouse click point
		/// </summary>
		private int clickX;

		/// <summary>
		/// The y coordinate of the last mouse click point
		/// </summary>
		private int clickY;

		#endregion


		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the ButtonRendererData class
		/// </summary>
		public ButtonRendererData()
		{
			this.buttonState = PushButtonStates.Normal;
			this.clickX = -1;
			this.clickY = -1;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the current state of the button
		/// </summary>
		public PushButtonStates ButtonState
		{
			get
			{
				return this.buttonState;
			}

			set
			{
				if (!Enum.IsDefined(typeof(PushButtonStates), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(PushButtonStates));
				}
					
				this.buttonState = value;
			}
		}
		

		/// <summary>
		/// Gets or sets the Point that the mouse was last clicked in the button
		/// </summary>
		public Point ClickPoint
		{
			get
			{
				return new Point(this.clickX, this.clickY);
			}

			set
			{
				this.clickX = value.X;
				this.clickY = value.Y;
			}
		}

		#endregion
	}
}
