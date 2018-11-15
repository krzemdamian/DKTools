using System;

namespace RevitDKTools.Command.ButtonData
{
    class GetOpenings : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Get Openings";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command selects openings inserted on element.\n " +
                "If used on opening it selects host element";
        }
    }
}
