using System;

using KellControls.KellTable.Events;


namespace KellControls.KellTable.Editors
{
	/// <summary>
	/// Specifies that a CellEditor uses the buttons provided by its counter-part 
	/// CellRenderer during editing
	/// </summary>
	public interface IEditorUsesRendererButtons
	{
		/// <summary>
		/// Raises the EditorButtonMouseDown event
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">A CellMouseEventArgs that contains the event data</param>
		void OnEditorButtonMouseDown(object sender, CellMouseEventArgs e);
		
		
		/// <summary>
		/// Raises the EditorButtonMouseUp event
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">A CellMouseEventArgs that contains the event data</param>
		void OnEditorButtonMouseUp(object sender, CellMouseEventArgs e);
	}
}
