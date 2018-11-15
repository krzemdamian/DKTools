using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Panels
{
    public class RibbonPanelMaker
    {
        readonly RibbonPanelBuilder _builder;

        public RibbonPanelMaker(RibbonPanelBuilder builder, RibbonPanel panel)
        {
            _builder = builder;
            _builder.Panel = panel;
        }

        public void BuildPanel()
        {
            _builder.BuildPanel();
        }

        public void GetPanel()
        {
            _builder.GetPanel();
        }

    }
}
