using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitDKTools.Panels
{
    public abstract class RibbonPanelBuilder
    {
        public RibbonPanel Panel { get; set; }

        public abstract void BuildPanel();

        public RibbonPanel GetPanel()
        {
            return Panel;
        }
    }
}