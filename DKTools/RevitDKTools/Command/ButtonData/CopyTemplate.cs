using System;

namespace RevitDKTools.Command.ButtonData
{
    class CopyTemplate : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Copy Template";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command copies template assigned to active view into other " +
                "documents opened in current Revit session";
        }

        public override void SetOptions()
        {
            base.Image = ButtonImage.Small;
            string path = FormatResourceName("Command/Images/Small/files.png");
            base.LargeImageStream = ThisAssembly.GetManifestResourceStream(path);
        }
    }
}
