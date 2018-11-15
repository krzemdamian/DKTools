using System;

namespace RevitDKTools.Command.ButtonData
{
    class OpeningVerifyAlignment : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Opening Verify Alignment";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command verifies alignment on selected openings.\n " +
                "If openings are not aligned command will ask if it should be corrected, " +
                "if yes it will ask for main opening to which other openings should be aligned.";
        }
    }
}
