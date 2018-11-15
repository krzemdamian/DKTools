using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.DockablePanels.ParameterEditor.Model
{
    public interface ISelectedElementParameterModel
    {
        string ParameterName { get; set; }
        string ParameterValue { get; set; }
    }
}
