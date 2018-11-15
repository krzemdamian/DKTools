using System;

namespace RevitDKTools.Command.ButtonData
{
    class MissingTags : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Missing\nTags";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Command works only on elements and tags selection. " +
                "It searches for elements without corresponding tags in the active selection";
        }

        public override void SetOptions()
        {
            base.Image = ButtonImage.Large;
            string path = FormatResourceName("Command/Images/Large/tag-missing.png");
            base.LargeImageStream = ThisAssembly.GetManifestResourceStream(path);
            base.LongDescription = "1. Select elements.\n" +
                "2. Select corresponding tags using Select Tags command\n" +
                "3. Run command";
        }
    }
}
