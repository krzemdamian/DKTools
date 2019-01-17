using Autodesk.Revit.UI;

namespace RevitDKTools.Commands.Panels
{
    public abstract class RibbonPanelButtonBuilder
    {
        public RibbonPanel Panel { get; set; }

        public abstract void BuildPanelButtons();

        public RibbonPanel GetPanel()
        {
            return Panel;
        }
    }
}