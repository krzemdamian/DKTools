using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace Helpers
{
	public static class GeneralHelper
	{
		[DllImport("kernel32.dll")]
		public static extern uint GetCurrentThreadId();

		/// <summary>
		/// Converts the GDI bitmap image to DirectX bitmap image.
		/// </summary>
		/// <param name="srcBitmap">The source GDI bitmap.</param>
		/// <returns>The DirectX bitmap image or null on error.</returns>
		public static BitmapImage BitmapToBitmapImage(Bitmap srcBitmap)
		{
			BitmapImage resultImage = null;
			using (MemoryStream mem = new MemoryStream())
			{
				srcBitmap.Save(mem, ImageFormat.Png);
				resultImage = new BitmapImage();
				resultImage.BeginInit();
				resultImage.StreamSource = new MemoryStream(mem.ToArray());
				resultImage.EndInit();
			}
			return resultImage;
		}

		/// <summary>
		/// Sets the given window's owner to Revit window.
		/// </summary>
		/// <param name="dialog">Target window.</param>
		public static void SetRevitAsWindowOwner(Window dialog)
		{
			if (null == dialog) { return; }

			WindowInteropHelper helper = new WindowInteropHelper(dialog);
			helper.Owner = FindRevitWindowHandle();
		}

		public static UIDocument ActiveUIDocument { get; set; }
        public static ExternalCommandData ExternalCommandData { get; set; }

		/// <summary>
		/// Finds the Revit window handle.
		/// </summary>
		/// <returns>Revit window handle.</returns>
		private static IntPtr FindRevitWindowHandle()
		{
			try
			{
				IntPtr foundRevitHandle = IntPtr.Zero;
				uint currentThreadID = GetCurrentThreadId();

				// Search for the Revit process with current thread ID.
				Process[] revitProcesses = Process.GetProcessesByName("Revit");
				Process foundRevitProcess = null;
				foreach (Process aRevitProcess in revitProcesses)
				{
					foreach (ProcessThread aThread in aRevitProcess.Threads)
					{
						if (aThread.Id == currentThreadID)
						{
							foundRevitProcess = aRevitProcess;
							break;
						}
					}  // For each thread in the process.

					// When we have found our Revit process, then stop searching.
					if (null != foundRevitProcess) { break; }
				}  // For each revit process found

				// Get the window handle of the Revit process found.
				if (null != foundRevitProcess)
				{
					foundRevitHandle = foundRevitProcess.MainWindowHandle;
				}

				return foundRevitHandle;
			}
			catch (Exception ex)
			{
				return IntPtr.Zero;
			}
		}

	}
}
