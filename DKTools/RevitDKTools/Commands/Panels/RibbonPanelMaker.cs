using Autodesk.Revit.UI;

namespace RevitDKTools.Commands.Panels
{
    public class RibbonPanelButtonMaker
    {
        readonly RibbonPanelButtonBuilder _builder;

        public RibbonPanelButtonMaker(RibbonPanelButtonBuilder builder, RibbonPanel panel)
        {
            _builder = builder;
            _builder.Panel = panel;
        }

        public void BuildButtons()
        {
            _builder.BuildPanelButtons();
        }

        public void GetPanel()
        {
            _builder.GetPanel();
        }

    }
}
