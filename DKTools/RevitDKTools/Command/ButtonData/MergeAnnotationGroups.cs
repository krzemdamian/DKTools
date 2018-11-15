using System;

namespace RevitDKTools.Command.ButtonData
{
    class MergeAnnotationGroups : PushButtonDataBuilder
    {
        public override void SetConstructorArguments()
        {
            base.Name = this.GetType().Name;
            base.TextOnRibbon = "Merge Annotation Groups";
            base.ClassName = "RevitDKTools.Command.Receiver." + this.GetType().Name;
        }

        public override void SetToolTip()
        {
            base.ToolTip = "Merge annotation groups. Command will look for the same " +
                "annotation groups and will try to merge them into one group substituting " +
                "them if they are already used in documentation.";
        }
    }
}
