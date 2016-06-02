using System;
using System.Runtime.InteropServices;


namespace KellControls.KellTable.Win32
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct MSG
	{
		public IntPtr hwnd;
		public uint message;
		public IntPtr wParam;
		public IntPtr lParam;
		public uint time;
		public POINT pt;
	}
}
