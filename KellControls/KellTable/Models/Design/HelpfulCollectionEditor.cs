using System;
using System.ComponentModel.Design;
using System.Windows.Forms;


namespace KellControls.KellTable.Models.Design
{
	/// <summary>
	/// A CollectionEditor that displays the help and command areas of its PropertyGrid
	/// </summary>
	public class HelpfulCollectionEditor : CollectionEditor
	{
		/// <summary>
		/// Initializes a new instance of the HelpfulCollectionEditor class using 
		/// the specified collection type
		/// </summary>
		/// <param name="type">The type of the collection for this editor to edit</param>
		public HelpfulCollectionEditor(Type type) : base(type)
		{

		}


		/// <summary>
		/// Creates a new form to display and edit the current collection
		/// </summary>
		/// <returns>An instance of CollectionEditor.CollectionForm to provide as the 
		/// user interface for editing the collection</returns>
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm editor = base.CreateCollectionForm();

			foreach (Control control in editor.Controls)
			{
				//
				if (control is PropertyGrid)
				{
					PropertyGrid grid = (PropertyGrid) control;
					
					grid.HelpVisible = true;
					grid.CommandsVisibleIfAvailable = true;
				}
			}

			return editor;
		}
	}
}
