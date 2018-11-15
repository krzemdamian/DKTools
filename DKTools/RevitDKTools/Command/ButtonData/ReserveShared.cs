using System;

namespace RevitDKTools.Command.ButtonData
{
    class ReserveShared : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Reserve Shared";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Reserve selected elements to edit in worksharing mode";
        }

        public override void SetOptions()
        {
            base.Image = ButtonImage.Small;
            string path = FormatResourceName("Command/Images/Large/push-pin.png");
            base.LargeImageStream = ThisAssembly.GetManifestResourceStream(path);
        }
    }
}
