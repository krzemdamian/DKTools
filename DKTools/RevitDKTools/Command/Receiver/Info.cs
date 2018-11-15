using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using RevitDKTools;

namespace RevitDKTools.Command.Receiver
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Info : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DKToolsApp.MyPythonEngine.RunScript(
                @"E:\Repos\DKTools_refactoring\DKTools\RevitDKTools\PythonScripts\Test\Info.py", 
                commandData, out message, elements);

            return Result.Succeeded;
        }
    }
}
