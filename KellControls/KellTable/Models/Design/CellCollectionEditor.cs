using System;
using System.ComponentModel;
using System.ComponentModel.Design;

using KellControls.KellTable.Events;
using KellControls.KellTable.Models;


namespace KellControls.KellTable.Models.Design
{
	/// <summary>
	/// Provides a user interface that can edit collections of Cells 
	/// at design time
	/// </summary>
	public class CellCollectionEditor : HelpfulCollectionEditor
	{
		/// <summary>
		///	The CellCollection being edited
		/// </summary>
		private CellCollection cells;

		
		/// <summary>
		/// Initializes a new instance of the CellCollectionEditor class 
		/// using the specified collection type
		/// </summary>
		/// <param name="type">The type of the collection for this editor to edit</param>
		public CellCollectionEditor(Type type) : base(type)
		{
			this.cells = null;
		}


		/// <summary>
		/// Edits the value of the specified object using the specified 
		/// service provider and context
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that can be 
		/// used to gain additional context information</param>
		/// <param name="isp">A service provider object through which 
		/// editing services can be obtained</param>
		/// <param name="value">The object to edit the value of</param>
		/// <returns>The new value of the object. If the value of the 
		/// object has not changed, this should return the same object 
		/// it was passed</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider isp, object value)
		{
			this.cells = (CellCollection) value;

			object returnObject = base.EditValue(context, isp, value);

			Row row = (Row) context.Instance;

			if (row.TableModel != null && row.TableModel.Table != null)
			{
				row.TableModel.Table.PerformLayout();
				row.TableModel.Table.Refresh();
			}
			
			return returnObject;
		}


		/// <summary>
		/// Creates a new instance of the specified collection item type
		/// </summary>
		/// <param name="itemType">The type of item to create</param>
		/// <returns>A new instance of the specified object</returns>
		protected override object CreateInstance(Type itemType)
		{
			Cell cell = (Cell) base.CreateInstance(itemType);

			// newly created items aren't added to the collection 
			// until editing has finished.  we'd like the newly 
			// created cell to show up in the table immediately
			// so we'll add it to the CellCollection now
			this.cells.Add(cell);

			return cell;
		}


		/// <summary>
		/// Destroys the specified instance of the object
		/// </summary>
		/// <param name="instance">The object to destroy</param>
		protected override void DestroyInstance(object instance)
		{
			if (instance != null && instance is Cell)
			{
				Cell cell = (Cell) instance;

				// the specified cell is about to be destroyed so 
				// we need to remove it from the CellCollection first
				this.cells.Remove(cell);
			}
			
			base.DestroyInstance(instance);
		}
	}
}
