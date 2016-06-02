using System;
using System.ComponentModel;

using KellControls.KellTable.Themes;


namespace KellControls.KellTable.Renderers
{
	/// <summary>
	/// Contains information about the current state of a Cell's check box
	/// </summary>
	public class CheckBoxRendererData
	{
		#region Class Data

		/// <summary>
		/// The current state of the Cells check box
		/// </summary>
		private CheckBoxStates checkState;

		#endregion


		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the ButtonRendererData class with the 
		/// specified CheckBox state
		/// </summary>
		/// <param name="checkState">The current state of the Cells CheckBox</param>
		public CheckBoxRendererData(CheckBoxStates checkState)
		{
			this.checkState = checkState;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the current state of the Cells checkbox
		/// </summary>
		public CheckBoxStates CheckState
		{
			get
			{
				return this.checkState;
			}

			set
			{
				if (!Enum.IsDefined(typeof(CheckBoxStates), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(CheckBoxStates));
				}
					
				this.checkState = value;
			}
		}

		#endregion
	}
}