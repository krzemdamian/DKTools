using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Commands.Generate
{
    /// <summary>
    /// This is a base class used for dynamic class emitter.
    /// Inherited necessary Revit API interface to bind commands for script execution.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DynamicCommandBase : IExternalCommand
    {
        public string _scriptPath;

        public DynamicCommandBase() { }
        
        // dynamic class have to have ctor assigning _scriptpath

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DKToolsApp.MyPythonEngine.RunScript(
                _scriptPath,
                commandData, out message, elements);

            return Result.Succeeded;
        }
    }
}
