using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using KellControls.KellTable.Events;
using KellControls.KellTable.Models;
using KellControls.KellTable.Renderers;
using KellControls.KellTable.Win32;


namespace KellControls.KellTable.Editors
{
	/// <summary>
	/// Summary description for DropDownContainer.
	/// </summary>
	[ToolboxItem(false)]
	public class DropDownContainer : Form
	{
		#region Class Data

		/// <summary>
		/// The DropDownCellEditor that owns the DropDownContainer
		/// </summary>
		private DropDownCellEditor editor;

		/// <summary>
		/// The Control displayed in the DropDownContainer
		/// </summary>
		private Control dropdownControl;

		/// <summary>
		/// A Panel that provides the black border around the DropDownContainer
		/// </summary>
		private Panel panel;

		#endregion

		
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the DropDownContainer class with the 
		/// specified DropDownCellEditor owner
		/// </summary>
		public DropDownContainer(DropDownCellEditor editor) : base()
		{
			if (editor == null)
			{
				throw new ArgumentNullException("editor", "DropDownCellEditor cannot be null");
			}
			
			this.editor = editor;
			
			this.ControlBox = false;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.FormBorderStyle = FormBorderStyle.None;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.Manual;
			this.TabStop = false;
			this.TopMost = true;

			this.dropdownControl = null;

			this.panel = new Panel();
			this.panel.AutoScroll = false;
			this.panel.BorderStyle = BorderStyle.FixedSingle;
			this.panel.Size = this.Size;
			this.Controls.Add(this.panel);
			this.SizeChanged += new EventHandler(DropDownContainer_SizeChanged);
		}

		#endregion


		#region Methods

		/// <summary>
		/// Displays the DropDownContainer to the user
		/// </summary>
		public void ShowDropDown()
		{
			this.FlushPaintMessages();

			this.Show();
		}


		/// <summary>
		/// Hides the DropDownContainer from the user
		/// </summary>
		public void HideDropDown()
		{
			this.FlushPaintMessages();

			this.Hide();
		}


		/// <summary>
		/// Processes any Paint messages in the message queue
		/// </summary>
		private void FlushPaintMessages()
		{
			MSG msg = new MSG();
			
			while (NativeMethods.PeekMessage(ref msg, IntPtr.Zero, (int) WindowMessage.WM_PAINT, (int) WindowMessage.WM_PAINT, 1 /*PM_REMOVE*/))
			{
				NativeMethods.TranslateMessage(ref msg);
				NativeMethods.DispatchMessage(ref msg);
			}
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the Control displayed in the DropDownContainer
		/// </summary>
		public Control Control
		{
			get
			{
				return this.dropdownControl;
			}

			set
			{
				if (value != this.dropdownControl)
				{
					this.panel.Controls.Clear();

					this.dropdownControl = value;

					if (value != null)
					{
						this.panel.Controls.Add(value);
					}
				}
			}
		}


		/// <summary>
		/// Gets the required creation parameters when the control handle is created
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cparams = base.CreateParams;

				cparams.ExStyle |= (int) WindowExtendedStyles.WS_EX_TOOLWINDOW;

				if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major > 5)
				{
					cparams.ExStyle |= (int) WindowExtendedStyles.WS_EX_NOACTIVATE;
				}

				cparams.ClassStyle |= 0x800 /*CS_SAVEBITS*/;
				
				return cparams;
			}
		}


		/// <summary>
		/// Handler for the DropDownContainer's SizeChanged event
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">An EventArgs that contains the event data</param>
		private void DropDownContainer_SizeChanged(object sender, EventArgs e)
		{
			this.panel.Size = this.Size;
		}

		#endregion
	}
}
