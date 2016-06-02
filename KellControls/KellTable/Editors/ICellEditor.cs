using System;
using System.Drawing;

using KellControls.KellTable.Models;


namespace KellControls.KellTable.Editors
{
	/// <summary>
	/// Exposes common methods provided by Cell editors
	/// </summary>
	public interface ICellEditor
	{
		/// <summary>
		/// Prepares the ICellEditor to edit the specified Cell
		/// </summary>
		/// <param name="cell">The Cell to be edited</param>
		/// <param name="table">The Table that contains the Cell</param>
		/// <param name="cellPos">A CellPos representing the position of the Cell</param>
		/// <param name="cellRect">The Rectangle that represents the Cells location and size</param>
		/// <param name="userSetEditorValues">Specifies whether the ICellEditors 
		/// starting value has already been set by the user</param>
		/// <returns>true if the ICellEditor can continue editing the Cell, false otherwise</returns>
		bool PrepareForEditing(Cell cell, Table table, CellPos cellPos, Rectangle cellRect, bool userSetEditorValues);


		/// <summary>
		/// Starts editing the Cell
		/// </summary>
		void StartEditing();


		/// <summary>
		/// Stops editing the Cell and commits any changes
		/// </summary>
		void StopEditing();


		/// <summary>
		/// Stops editing the Cell and ignores any changes
		/// </summary>
		void CancelEditing();
	}
}
