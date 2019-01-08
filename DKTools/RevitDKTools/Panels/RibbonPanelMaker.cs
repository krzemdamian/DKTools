using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Panels
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
