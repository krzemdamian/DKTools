using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Command.Receiver
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class DynamicCommandBase : IExternalCommand
    {
        readonly string _scriptPath;

        public DynamicCommandBase(string scriptPath)
        {
            _scriptPath = scriptPath;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DKToolsApp.MyPythonEngine.RunScript(
                _scriptPath,
                commandData, out message, elements);

            return Result.Succeeded;
        }
    }
}
