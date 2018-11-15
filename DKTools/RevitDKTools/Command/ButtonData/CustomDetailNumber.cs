using System;

namespace RevitDKTools.Command.ButtonData
{
    class CustomDetailNumber : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Custom Detail Number";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command will copy view detail number to 'Detail_Number' cutom view parameter.";
        }
    }
}
