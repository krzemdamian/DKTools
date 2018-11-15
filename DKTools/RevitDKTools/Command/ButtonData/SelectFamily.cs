using System;

namespace RevitDKTools.Command.ButtonData
{
    class SelectFamily : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Select Family";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Select all instances of current element's family";
        }

        public override void SetOptions()
        {
            base.Image = ButtonImage.Small;
            string path = FormatResourceName("Command/Images/Large/packages.png");
            base.LargeImageStream = ThisAssembly.GetManifestResourceStream(path);
        }
    }
}
