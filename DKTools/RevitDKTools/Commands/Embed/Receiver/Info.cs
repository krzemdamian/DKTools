﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Reflection;
using System.IO;

namespace RevitDKTools.Commands.Embed.Receiver
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Info : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DKToolsApp.MyPythonEngine.RunScript(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\",string.Empty) + @"\PythonScripts\Test\Info.py", 
                commandData, out message, elements);

            return Result.Succeeded;
        }
    }
}
