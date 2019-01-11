using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using Autodesk.Revit.DB;
using RevitDKTools;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace RevitDKTools.Commands.Generate.ButtonData
{
    public interface IPythonExecutionEnviroment
    {
        ScriptRuntime PythonScriptRuntime { get; set; }
        ScriptEngine PythonEngine { get; set; }
        Dictionary<string, CompiledCode> CompiledPythonScripts { get; set; }
        List<string> ExternalEventPythonScriptPath { get; set; }
        ScriptScope LastUsedScope { get; set; }


        void RunScript(string commandPath, ExternalCommandData commandData,
            out string errorMessage, ElementSet elementSelection);
    }
}
