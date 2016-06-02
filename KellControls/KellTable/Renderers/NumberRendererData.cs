using System;
using System.ComponentModel;
using System.Drawing;

using KellControls.KellTable.Themes;


namespace KellControls.KellTable.Renderers
{
	/// <summary>
	/// Contains information about the current state of a number Cell's 
	/// up and down buttons
	/// </summary>
	public class NumberRendererData
	{
		#region Class Data

		/// <summary>
		/// The current state of the up button
		/// </summary>
		private UpDownStates upState;

		/// <summary>
		/// The current state of the down button
		/// </summary>
		private UpDownStates downState;
		
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
		/// Initializes a new instance of the NumberRendererData class
		/// </summary>
		public NumberRendererData()
		{
			this.upState = UpDownStates.Normal;
			this.downState = UpDownStates.Normal;
			this.clickX = -1;
			this.clickY = -1;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the current state of the up button
		/// </summary>
		public UpDownStates UpButtonState
		{
			get
			{
				return this.upState;
			}

			set
			{
				if (!Enum.IsDefined(typeof(UpDownStates), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(UpDownStates));
				}
					
				this.upState = value;
			}
		}


		/// <summary>
		/// Gets or sets the current state of the down button
		/// </summary>
		public UpDownStates DownButtonState
		{
			get
			{
				return this.downState;
			}

			set
			{
				if (!Enum.IsDefined(typeof(UpDownStates), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(UpDownStates));
				}
					
				this.downState = value;
			}
		}
		

		/// <summary>
		/// Gets or sets the Point that the mouse was last clicked in a button
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
