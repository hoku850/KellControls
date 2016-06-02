using System;

using KellControls.KellTable.Events;


namespace KellControls.KellTable.Renderers
{
	/// <summary>
	/// Exposes common methods provided by Column header renderers
	/// </summary>
	public interface IHeaderRenderer : IRenderer
	{
		/// <summary>
		/// Raises the PaintHeader event
		/// </summary>
		/// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
		void OnPaintHeader(PaintHeaderEventArgs e);
		
		
		/// <summary>
		/// Raises the MouseEnter event
		/// </summary>
		/// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
		void OnMouseEnter(HeaderMouseEventArgs e);


		/// <summary>
		/// Raises the MouseLeave event
		/// </summary>
		/// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
		void OnMouseLeave(HeaderMouseEventArgs e);


		/// <summary>
		/// Raises the MouseUp event
		/// </summary>
		/// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
		void OnMouseUp(HeaderMouseEventArgs e);


		/// <summary>
		/// Raises the MouseDown event
		/// </summary>
		/// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
		void OnMouseDown(HeaderMouseEventArgs e);


		/// <summary>
		/// Raises the MouseMove event
		/// </summary>
		/// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
		void OnMouseMove(HeaderMouseEventArgs e);


		/// <summary>
		/// Raises the Click event
		/// </summary>
		/// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
		void OnClick(HeaderMouseEventArgs e);


		/// <summary>
		/// Raises the DoubleClick event
		/// </summary>
		/// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
		void OnDoubleClick(HeaderMouseEventArgs e);
	}
}
