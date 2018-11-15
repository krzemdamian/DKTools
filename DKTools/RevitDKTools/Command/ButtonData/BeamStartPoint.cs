using System;

namespace RevitDKTools.Command.ButtonData
{
    class BeamStartPoint : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Beam Start Point";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command checks if startpoint is in the left down position " +
                "in comparision to end point.\n\n After verifications it's possible " +
                "to fix all of them automaticly.";
        }
    }
}
