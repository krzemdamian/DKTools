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
using System.IO;

namespace RevitDKTools.Commands.Generate
{
    public class PythonExecutionEnviroment : IExternalEventHandler, IPythonExecutionEnviroment
    {
        public PythonExecutionEnviroment EngineInstance { get; set; }
        public ScriptRuntime PythonScriptRuntime { get; set; }
        public ScriptEngine PythonEngine { get; set; }
        public Dictionary<string, CompiledCode> CompiledPythonScripts { get; set; } = new Dictionary<string, CompiledCode>();
        public Dictionary<string, ScriptScope> CommandScopes { get; set; } = new Dictionary<string, ScriptScope>();
        public List<string> ExternalEventPythonScriptPath { get; set; }
        public ScriptScope LastUsedScope { get; set; }
        public Dictionary<string, dynamic> ScriptVariables { get; set; } = new Dictionary<string, dynamic>();

        public PythonExecutionEnviroment()
        {
            EngineInstance = this;

            Dictionary<string, object> engineOptions = new Dictionary<string, object>
            {
                ["LightweightScopes"] = true
            };
            PythonEngine = Python.CreateEngine(engineOptions);
            PythonScriptRuntime = PythonEngine.Runtime;

            CreateRvtModuleInEnviroment();
        }

        private void CreateRvtModuleInEnviroment()
        {
            PythonScriptRuntime.LoadAssembly(Assembly.Load("RevitAPI"));
            PythonScriptRuntime.LoadAssembly(Assembly.Load("RevitAPIUI"));

            //TODO: Change from list to object of new class with Set(), Get() methods
            ExternalEventPythonScriptPath = new List<string>();
            ExternalEventPythonScriptPath.Add(string.Empty);

            ScriptScope rvt = PythonEngine.CreateModule("rvt");
            rvt.SetVariable("_app_", RevitDKTools.DKToolsApp.UIControlledApplication);
            rvt.SetVariable("_event_path_", ExternalEventPythonScriptPath);
            rvt.SetVariable("_handler_", EngineInstance);
            ExternalEvent exEvent = ExternalEvent.Create(EngineInstance);
            rvt.SetVariable("_event_", exEvent);
            rvt.SetVariable("_variables_", ScriptVariables);
        }

        public void RunScript(string commandPath, ExternalCommandData commandData,
                              out string errorMessage, ElementSet elementSelection)
        {
            errorMessage = "No additional information";

            if (CompiledPythonScripts.ContainsKey(commandPath))
            {
                ScriptScope defaultScope = CompiledPythonScripts[commandPath].DefaultScope;
                defaultScope.SetVariable("_command_data_", commandData);
                defaultScope.SetVariable("_my_path_", commandPath);
                defaultScope.SetVariable("_app_path_",
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                LastUsedScope = defaultScope;

                //TODO: add scope from dictionary
                CompiledPythonScripts[commandPath].Execute();

                errorMessage = RetriveErrorMessageFromPythonScript(errorMessage, defaultScope);
           }

            else //if (!CompiledPythonScripts.ContainsKey(commandPath))
            {
                ScriptSource source = PythonEngine.CreateScriptSourceFromFile(commandPath);
                ScriptScope scope = PythonEngine.CreateScope();
                scope.SetVariable("_command_data_", commandData);
                scope.SetVariable("_my_path_", commandPath);
                scope.SetVariable("_app_path_", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                CompiledCode compiled = source.Compile();
                LastUsedScope = scope;
                source.Execute(scope);

                errorMessage = RetriveErrorMessageFromPythonScript(errorMessage, scope);

                if (CommandScopes.ContainsKey(commandPath) == false)
                {
                    CommandScopes.Add(commandPath, scope);
                }
                AddToCompiledScriptsList(commandPath, scope, compiled);
            }
        }

        private void AddToCompiledScriptsList(string commandPath, ScriptScope scope, CompiledCode compiled)
        {
            bool debugMode = false;
            bool getDebugResult = scope.TryGetVariable<bool>("_debug_", out bool tempDebug);
            if (getDebugResult)
            {
                debugMode = tempDebug;
            }

            if (debugMode == false)
            {
                CompiledPythonScripts.Add(commandPath, compiled);
            }
        }

        private static string RetriveErrorMessageFromPythonScript(string errorMessage, ScriptScope defaultScope)
        {
            bool getErrorMessageResult = defaultScope.TryGetVariable<string>
                ("_error_message_", out string tempErrorMessage);
            if (getErrorMessageResult)
            {
                errorMessage = tempErrorMessage;
            }

            return errorMessage;
        }

        public string GetName()
        {
            return "DKTools Python Engine with External Event";
        }

        public void Execute(UIApplication app)
        {
            if (ExternalEventPythonScriptPath.Any())
            {
                ScriptSource externalScriptSource = PythonEngine.CreateScriptSourceFromFile(ExternalEventPythonScriptPath[0]);
                externalScriptSource.Execute(LastUsedScope);
            }
        }
    }
}
