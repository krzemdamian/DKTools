using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using RevitDKTools.Command.Receiver;

[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
public class ImportReferencedDetail : IExternalCommand
{
    public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
        ref string errorMessage, ElementSet errorElementSelection)
    {
        //RevitPythonEngine.Instance.RunScript("some_python_script.py", commandData,
        //    out errorMessage, errorElementSelection);

        return Autodesk.Revit.UI.Result.Succeeded;
    }
}