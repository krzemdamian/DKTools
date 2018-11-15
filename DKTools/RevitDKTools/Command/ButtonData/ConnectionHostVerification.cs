using System;

namespace RevitDKTools.Command.ButtonData
{
    class ConnectionHostVerification : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Connection Host Verification";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command verifies and fixes structural connections hosts.\n\n Opens GUI with details";
        }
    }
}
