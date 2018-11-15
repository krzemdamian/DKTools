using System;

namespace RevitDKTools.Command.ButtonData
{
    class ImportReferencedDetail : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Import Referenced Detail";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command will import views from model  with details updating " +
                "selected views.\nDetail model with performed Assign Detail Reference should be " +
                "opened in the same Revit instance.\nCommand compares custom view parameter " +
                "'Detail Reference' with the same parameter of drafting view in detail model. " +
                "'Detail Reference' custom parameters should be filled equally to create link for " +
                "Import Referenced Detail command.";
        }
    }
}
