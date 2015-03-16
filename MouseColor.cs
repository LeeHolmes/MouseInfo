using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseInfo
{
	class MouseInfo
	{
		[STAThread]
		static void Main(string[] args)
		{
			System.Threading.Thread.Sleep(1000);

			Point mouseCursorPoint = Cursor.Position;
			IntPtr displayDC = GdiWrapper.CreateDC("DISPLAY", null, null, (IntPtr)null);
			
			Graphics displayGraphics = Graphics.FromHdc(displayDC);

			Bitmap savedBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, 
				Screen.PrimaryScreen.Bounds.Height, displayGraphics);

			Graphics savedGraphics = Graphics.FromImage(savedBitmap);
			IntPtr screenHandle = displayGraphics.GetHdc();
			IntPtr savedHandle = savedGraphics.GetHdc();

			GdiWrapper.BitBlt(savedHandle, 0, 0, savedBitmap.Width, savedBitmap.Height,
				screenHandle, 0, 0, 0x00CC0020);
			
			savedGraphics.ReleaseHdc(savedHandle);
			displayGraphics.ReleaseHdc(screenHandle);

			Console.WriteLine("Mouse at: {0}, {1}", mouseCursorPoint.X, mouseCursorPoint.Y);
			Color pixelColor = savedBitmap.GetPixel(mouseCursorPoint.X, mouseCursorPoint.Y);
			Console.WriteLine("Cursor color: {0}", pixelColor);

			savedGraphics.Dispose();
			displayGraphics.Dispose();
		}
	}

	class GdiWrapper
	{
		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hdcDst, int xDst, int yDst, int cx, int cy,
			IntPtr hdcSrc, int xSrc, int ySrc, uint ulRop);	

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateDC(string name, string name2, string name3, IntPtr data);
	}
}
