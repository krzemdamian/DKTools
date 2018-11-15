using System;

namespace RevitDKTools.Command.ButtonData
{
    class OpeningVerifyHost : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Opening Verify Host";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command looks for incorrect openings' hosts. \n " +
                "If it finds incorrect hosts of opening elements it will ask if it shuld be fixed.";
        }
    }
}
