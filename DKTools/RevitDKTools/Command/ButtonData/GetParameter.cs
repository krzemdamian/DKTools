using System;

namespace RevitDKTools.Command.ButtonData
{
    class GetParameter : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Get\nParam";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command adjust view to show selected elements";
        }

        public override void SetOptions()
        {
            base.Image = ButtonImage.Large;
            //base.LargeImageStream = new Uri(@"E:\DKTools_refactoring\DKTools\RevitDKTools\Command\ButtonImages\Large\clipboard.png");
        }
    }
}
