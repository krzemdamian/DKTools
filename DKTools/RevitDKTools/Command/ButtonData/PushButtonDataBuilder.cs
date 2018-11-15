using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RevitDKTools.Command.ButtonData
{
    public abstract class PushButtonDataBuilder
    {
        protected PushButtonData _pushButtonData;
        readonly string _thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
        protected Assembly ThisAssembly { get; } = Assembly.GetExecutingAssembly();
        public string Name { get; protected set; }
        public string TextOnRibbon { get; protected set; }
        public string ClassName { get; protected set; }
        public string ToolTip { get; protected set; }
        public ButtonImage Image { get; protected set; }
        public Stream SmallImageStream { get; protected set; }
        public Stream LargeImageStream { get; protected set; }
        public string LongDescription { get; protected set; }
        public Stream ToolTipImageStream { get; set; }
       // public ImageSource ToolTipImage { get; protected set; }


        public abstract void SetConstructorArguments();
        public abstract void SetToolTip();


        public void CreatePushButtonData()
        {
            if (Name.Any() && TextOnRibbon.Any() && _thisAssemblyPath.Any() && ClassName.Any())
            {
                _pushButtonData = new PushButtonData(Name, TextOnRibbon, _thisAssemblyPath, ClassName);
            }
        }

        public PushButtonData GetPushButtonData()
        {
            return _pushButtonData;
        }

        public virtual void SetOptions()
        {
            Image = ButtonImage.None;
            SmallImageStream = null;
            LargeImageStream = null;
            ToolTipImageStream = null;
            LongDescription = null;
        }

        public void AddToolTip()
        {
            _pushButtonData.ToolTip = ToolTip;
        }

        public void AddLongDescription()
        {
            _pushButtonData.LongDescription = LongDescription;
        }

        public void AddToolTipImage()
        {
            if (ToolTipImageStream != null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ToolTipImageStream;
                bitmap.EndInit();
                _pushButtonData.ToolTipImage = bitmap;
            }
        }

        protected string FormatResourceName(string resourceName)
        {
            return ThisAssembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                               .Replace("\\", ".")
                                                               .Replace("/", ".");
        }

        public void AddImage()
        {
            if (Image == ButtonImage.Small && SmallImageStream != null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = SmallImageStream;
                bitmap.EndInit();
                //_pushButtonData.Image = bitmap;
                if (bitmap.PixelHeight == 16 && bitmap.PixelWidth == 16)
                {
                    _pushButtonData.Image = bitmap;
                }

            }
            else if (Image == ButtonImage.Large && LargeImageStream != null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = LargeImageStream;
                bitmap.EndInit();
                //_pushButtonData.LargeImage = bitmap;
                if (bitmap.PixelHeight == 32 && bitmap.PixelWidth == 32)
                {
                    _pushButtonData.LargeImage = bitmap;
                }
            }
        }
    }

}

