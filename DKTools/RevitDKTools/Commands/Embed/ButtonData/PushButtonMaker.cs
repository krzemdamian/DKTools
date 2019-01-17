using Autodesk.Revit.UI;

namespace RevitDKTools.Commands.Embed.ButtonData
{
    public class PushButtonMaker
    {
        readonly PushButtonDataBuilder builder;

        public PushButtonMaker(PushButtonDataBuilder builder)
        {
            this.builder = builder;
        }

        public void BuildPushButtonData()
        {
            builder.SetConstructorArguments();
            builder.CreatePushButtonData();
            builder.AddToolTip();
            builder.SetOptions();
            builder.AddImage();
            builder.AddLongDescription();
            builder.AddToolTipImage();
        }

        public PushButtonData GetPushButtonData()
        {
            return builder.GetPushButtonData();
        }
    }
}
