using System;

namespace RevitDKTools.Command.ButtonData
{
    class GetId : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Get\nId";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Copy selected elements' ids to clipboard.";
        }

        public override void SetOptions()
        {
            base.Image = ButtonImage.Large;
            string path = FormatResourceName("Command/Images/Large/credit-card.png");
            base.LargeImageStream = ThisAssembly.GetManifestResourceStream(path);
        }
    }
}
