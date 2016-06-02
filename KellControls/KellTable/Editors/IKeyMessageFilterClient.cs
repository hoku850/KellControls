using System;
using System.Windows.Forms;

using KellControls.KellTable.Events;
using KellControls.KellTable.Win32;


namespace KellControls.KellTable.Editors
{
	/// <summary>
	/// Indicates that an object is interested in receiving key messages 
	/// before they are sent to their destination
	/// </summary>
	public interface IKeyMessageFilterClient
	{
		/// <summary>
		/// Filters out a key message before it is dispatched
		/// </summary>
		/// <param name="target">The Control that will receive the message</param>
		/// <param name="msg">A WindowMessage that represents the message to process</param>
		/// <param name="wParam">Specifies the WParam field of the message</param>
		/// <param name="lParam">Specifies the LParam field of the message</param>
		/// <returns>true to filter the message and prevent it from being dispatched; 
		/// false to allow the message to continue to the next filter or control</returns>
		bool ProcessKeyMessage(Control target, WindowMessage msg, int wParam, int lParam);
	}
}
