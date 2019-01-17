using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RevitDKTools.Common
{
    public static class AssemblyResourceUtils
    {
        public static string FormatResourceName(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetName().
                            Name + "." + 
                            resourceName.Replace(" ", "_")
                                        .Replace("\\", ".")
                                        .Replace("/", ".");
        }

        public static BitmapImage GetImageFromResource(string resourcePath)
        {
            string path = FormatResourceName(resourcePath);
            try
            {
                Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = imageStream;
                bitmap.EndInit();
                return bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Faild creating BitmapImage from resouce. Error:\r\n" + ex.Message);
                return null;
            }
        }
    }
}
