using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

using KellControls.KellTable.Events;
using KellControls.KellTable.Win32;


namespace KellControls.KellTable.Editors
{
	/// <summary>
	/// A message filter that filters mouse messages
	/// </summary>
	internal class MouseMessageFilter : IMessageFilter
	{
		/// <summary>
		/// An IMouseMessageFilterClient that wishes to receive mouse events
		/// </summary>
		private IMouseMessageFilterClient client;
		
		
		/// <summary>
		/// Initializes a new instance of the CellEditor class with the 
		/// specified IMouseMessageFilterClient client
		/// </summary>
		public MouseMessageFilter(IMouseMessageFilterClient client)
		{
			this.client = client;
		}


		/// <summary>
		/// Gets or sets the IMouseMessageFilterClient that wishes to 
		/// receive mouse events
		/// </summary>
		public IMouseMessageFilterClient Client
		{
			get
			{
				return this.client;
			}

			set
			{
				this.client = value;
			}
		}


		/// <summary>
		/// Filters out a message before it is dispatched
		/// </summary>
		/// <param name="m">The message to be dispatched. You cannot modify 
		/// this message</param>
		/// <returns>true to filter the message and prevent it from being 
		/// dispatched; false to allow the message to continue to the next 
		/// filter or control</returns>
		public bool PreFilterMessage(ref Message m)
		{
			// make sure we have a client
			if (this.Client == null)
			{
				return false;
			}

			// make sure the message is a mouse message
			if ((m.Msg >= (int) WindowMessage.WM_MOUSEMOVE && m.Msg <= (int) WindowMessage.WM_XBUTTONDBLCLK) || 
				(m.Msg >= (int) WindowMessage.WM_NCMOUSEMOVE && m.Msg <= (int) WindowMessage.WM_NCXBUTTONUP))
			{
				// try to get the target control
				UIPermission uiPermission = new UIPermission(UIPermissionWindow.AllWindows);
				uiPermission.Demand();
				Control target = Control.FromChildHandle(m.HWnd);

				return this.Client.ProcessMouseMessage(target, (WindowMessage) m.Msg, m.WParam.ToInt32(), m.LParam.ToInt32());
			}
				
			return false;
		}
	}
}
