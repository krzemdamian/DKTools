using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Commands.Embed.ButtonData
{
    public class PushButtonMaker
    {
        readonly PushButtonDataBuilder builder;

        public PushButtonMaker(PushButtonDataBuilder builder)
        {
            this.builder = builder;
        }

        public void BuildPushButtonData()
        {
            builder.SetConstructorArguments();
            builder.CreatePushButtonData();
            builder.AddToolTip();
            builder.SetOptions();
            builder.AddImage();
            builder.AddLongDescription();
            builder.AddToolTipImage();
        }

        public PushButtonData GetPushButtonData()
        {
            return builder.GetPushButtonData();
        }
    }
}
