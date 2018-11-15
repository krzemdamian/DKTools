﻿using System;

namespace RevitDKTools.Command.ButtonData
{
    class ShowParameterEditor : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Parameter\r\nEditor";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Shows Parameter Editor dockable panel. " +
                "This tool enables easier length manipulation in picked parameter of selected element.";
        }

        public override void SetOptions()
        {
            base.Image = ButtonImage.Large;
            string path = FormatResourceName("Command/Images/Large/blue/measure.png");
            base.LargeImageStream = ThisAssembly.GetManifestResourceStream(path);
        }
    }
}
