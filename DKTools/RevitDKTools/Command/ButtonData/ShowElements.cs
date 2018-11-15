using System;

namespace RevitDKTools.Command.ButtonData
{
    class ShowElements : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Show\nElements";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command adjust view to show selected elements";
        }

        public override void SetOptions()
        {
            base.Image = ButtonImage.Large;
            string path = FormatResourceName("Command/Images/Large/eye-scan.png");
            base.LargeImageStream = ThisAssembly.GetManifestResourceStream(path);
        }
    }
}
