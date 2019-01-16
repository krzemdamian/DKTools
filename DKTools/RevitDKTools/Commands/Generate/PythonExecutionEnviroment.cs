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
        private PythonExecutionEnviroment _engineInstance;
        private ScriptEngine _pythonEngine;
        private ScriptRuntime _pythonScriptRuntime;
        private Dictionary<string, CompiledCode> _compiledPythonScripts;
        private Dictionary<string, ScriptScope> _commandScopes;
        private ExternalPythonScriptSetting _externalEventPythonScriptPath;
        private ScriptScope _lastUsedScope;
        private Dictionary<string, dynamic> _scriptVariables;

        public PythonExecutionEnviroment()
        {
            _engineInstance = this;
            Dictionary<string, object> engineOptions = new Dictionary<string, object>
            {
                ["LightweightScopes"] = true
            };
            _pythonEngine = Python.CreateEngine(engineOptions);
            _pythonScriptRuntime = _pythonEngine.Runtime;
            _compiledPythonScripts = new Dictionary<string, CompiledCode>();
            _commandScopes = new Dictionary<string, ScriptScope>();
            _scriptVariables = new Dictionary<string, dynamic>();

            CreateRvtModuleInEnviroment();
        }

        public void RunScript(string commandPath, ExternalCommandData commandData,
                              out string errorMessage, ElementSet elementSelection)
        {
            errorMessage = "No additional information";

            if (_compiledPythonScripts.ContainsKey(commandPath))
            {
                ScriptScope defaultScope = _compiledPythonScripts[commandPath].DefaultScope;
                defaultScope.SetVariable("_command_data_", commandData);
                defaultScope.SetVariable("_my_path_", commandPath);
                defaultScope.SetVariable("_app_path_",
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                _lastUsedScope = defaultScope;

                //TODO: add scope from dictionary
                _compiledPythonScripts[commandPath].Execute();

                errorMessage = RetriveErrorMessageFromPythonScript(errorMessage, defaultScope);
            }

            else
            {
                ScriptSource source = _pythonEngine.CreateScriptSourceFromFile(commandPath);
                ScriptScope scope = _pythonEngine.CreateScope();
                scope.SetVariable("_command_data_", commandData);
                scope.SetVariable("_my_path_", commandPath);
                scope.SetVariable("_app_path_",
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                CompiledCode compiled = source.Compile();
                _lastUsedScope = scope;
                source.Execute(scope);

                errorMessage = RetriveErrorMessageFromPythonScript(errorMessage, scope);

                if (_commandScopes.ContainsKey(commandPath) == false)
                {
                    _commandScopes.Add(commandPath, scope);
                }
                AddToCompiledScriptsList(commandPath, scope, compiled);
            }
        }

        private void CreateRvtModuleInEnviroment()
        {
            _pythonScriptRuntime.LoadAssembly(Assembly.Load("RevitAPI"));
            _pythonScriptRuntime.LoadAssembly(Assembly.Load("RevitAPIUI"));

            _externalEventPythonScriptPath = new ExternalPythonScriptSetting();

            ScriptScope rvt = _pythonEngine.CreateModule("rvt");
            rvt.SetVariable("_app_", RevitDKTools.DKToolsApp.UIControlledApplication);
            rvt.SetVariable("_event_path_", _externalEventPythonScriptPath);
            rvt.SetVariable("_handler_", _engineInstance);
            ExternalEvent exEvent = ExternalEvent.Create(_engineInstance);
            rvt.SetVariable("_event_", exEvent);
            rvt.SetVariable("_variables_", _scriptVariables);
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
                _compiledPythonScripts.Add(commandPath, compiled);
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
            if (string.IsNullOrEmpty(_externalEventPythonScriptPath.Location) == false)
            {
                ScriptSource externalScriptSource =
                    _pythonEngine.CreateScriptSourceFromFile(_externalEventPythonScriptPath.Location);
                externalScriptSource.Execute(_lastUsedScope);
            }
        }
    }
}
