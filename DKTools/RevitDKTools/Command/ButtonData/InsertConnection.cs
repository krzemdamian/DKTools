using System;

namespace RevitDKTools.Command.ButtonData
{
    class InsertConnection : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Insert Connection";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command places Structural Connection based on elements propertie";
        }
    }
}
