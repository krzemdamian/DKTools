﻿using Autodesk.Revit.UI;
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

namespace RevitDKTools.Commands.Generate
{
    public interface IPythonExecutionEnviroment
    {
        PythonExecutionEnviroment EngineInstance { get; set; }
        ScriptEngine PythonEngine { get; set; }
        ScriptRuntime PythonScriptRuntime { get; set; }
        Dictionary<string, CompiledCode> CompiledPythonScripts { get; set; }
        Dictionary<string, ScriptScope> CommandScopes { get; set; }
        ExternalPythonScriptSetting ExternalEventPythonScriptPath { get; set; }
        ScriptScope LastUsedScope { get; set; }
        Dictionary<string, dynamic> ScriptVariables { get; set; }

        void RunScript(string commandPath, ExternalCommandData commandData,
            out string errorMessage, ElementSet elementSelection);
    }
}
