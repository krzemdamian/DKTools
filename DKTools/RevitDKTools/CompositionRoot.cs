using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools
{
    class CompositionRoot : ICompositionRoot
    {
        private UIControlledApplication _application;

        public CompositionRoot(UIControlledApplication application)
        {
            _application = application;
        }
    }
}
