using System;

namespace RevitDKTools.Command.ButtonData
{
    class AssignDetailReference : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Assign Detail Reference";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command will copy view infomrmation to custom parameter " +
                "'Detail Reference'.\nCommand should be performed on Revit file with details " +
                "for future use of import Detail.\nThis way Import Referenced Detail command " +
                "will grant information which view is referencing detail in model.";
        }
    }
}
