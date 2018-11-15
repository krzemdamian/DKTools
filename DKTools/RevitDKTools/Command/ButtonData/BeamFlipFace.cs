using System;

namespace RevitDKTools.Command.ButtonData
{
    class BeamFlipFace : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Beam Flip Face";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Flip beam's face";
        }
    }
}
