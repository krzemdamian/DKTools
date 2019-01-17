namespace RevitDKTools.Commands.Embed.ButtonData
{
    class Info : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Info";
            base.ClassName = "RevitDKTools.Commands.Embed.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Basic info about DKTools Revit Add-In.";
        }

        public override void SetOptions()
        {
            base.Image = ButtonImage.Large;
            string path = FormatResourceName("Commands/Images/Large/blue/laptop.png");
            base.LargeImageStream = ThisAssembly.GetManifestResourceStream(path);
        }
    }
}
