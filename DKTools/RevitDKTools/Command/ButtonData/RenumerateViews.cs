using System;

namespace RevitDKTools.Command.ButtonData
{
    class RenumerateViews : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Renumerate Views";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command renumerate views. User should click subsequent " +
                "viewports assigning new numbers starting from 1.";
        }
    }
}
