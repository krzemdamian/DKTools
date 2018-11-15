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

namespace RevitDKTools.Command.Receiver
{
    public class MyPythonEngine : IExternalEventHandler, IMyPythonEngine
    {
        public MyPythonEngine EngineInstance { get; set; }
        public ScriptRuntime PythonScriptRuntime { get; set; }
        public ScriptEngine PythonEngine { get; set; }
        public Dictionary<string, CompiledCode> CompiledPythonScripts { get; set; } = new Dictionary<string, CompiledCode>();
        public Dictionary<string, ScriptScope> CommandScopes { get; set; } = new Dictionary<string, ScriptScope>();
        public string ExternalEventPythonScriptPath { get; set; }
        public ScriptScope LastUsedScope { get; set; }
        public Dictionary<string, dynamic> ScriptVariables { get; set; } = new Dictionary<string, dynamic>();

        public MyPythonEngine()
        {
            Dictionary<string, object> engineOptions = new Dictionary<string, object>
            {
                ["LightweightScopes"] = true
            };
            EngineInstance = this;

            PythonEngine = Python.CreateEngine(engineOptions);
            PythonScriptRuntime = PythonEngine.Runtime;
            PythonScriptRuntime.LoadAssembly(Assembly.Load("RevitAPI"));
            PythonScriptRuntime.LoadAssembly(Assembly.Load("RevitAPIUI"));


            ScriptScope rvt = PythonEngine.CreateModule("rvt");
            rvt.SetVariable("_app_", RevitDKTools.DKToolsApp.Application);
            rvt.SetVariable("_handler_", EngineInstance);
            ExternalEvent exEvent = ExternalEvent.Create(EngineInstance);
            rvt.SetVariable("_event_", exEvent);
            rvt.SetVariable("_variables_", ScriptVariables);
            
        }

        public void RunScript(string commandPath, ExternalCommandData commandData,
            out string errorMessage, ElementSet elementSelection)
        {
            //throw new NotImplementedException();
            
            errorMessage = "No additional information";


            if (CompiledPythonScripts.ContainsKey(commandPath))
            {
                ScriptScope defaultScope = CompiledPythonScripts[commandPath].DefaultScope;
                defaultScope.SetVariable("_command_data_", commandData);
                CompiledPythonScripts[commandPath].Execute();

                // region where engine tries to retrive _event_path_, _error_message_, _element_set_
                #region _get_information_from_python_script_
                bool getPathResult = defaultScope.TryGetVariable<string>("_event_path_", out string tempPath);
                if (getPathResult)
                {
                    ExternalEventPythonScriptPath = tempPath;
                }
                else
                {
                    ExternalEventPythonScriptPath = null;
                }

                bool getErrorMessageResult = defaultScope.TryGetVariable<string>("_error_message_", out string tempErrorMessage);
                if (getErrorMessageResult)
                {
                    errorMessage = tempErrorMessage;
                }

                bool getElementSetResult = defaultScope.TryGetVariable<ElementSet>("_element_set_", out ElementSet tempElementSet);
                if (getElementSetResult)
                {
                    elementSelection = tempElementSet;
                }
                #endregion

            }

            else if (!CompiledPythonScripts.ContainsKey(commandPath))
            {
                ScriptSource source = PythonEngine.CreateScriptSourceFromFile(commandPath);
                ScriptScope scope = PythonEngine.CreateScope();
                scope.SetVariable("_command_data_", commandData);
                CompiledCode compiled = source.Compile();
                CompiledPythonScripts.Add(commandPath, compiled);
                //compiled.Execute(scope);
                source.Execute(scope);
                CommandScopes.Add(commandPath, scope);
                LastUsedScope = scope;

                
                // region where engine tries to retrive _event_path_, _error_message_, _element_set_
                #region _get_information_from_python_script_
                bool getPathResult = scope.TryGetVariable<string>("_event_path_", out string tempPath);
                if (getPathResult)
                {
                    ExternalEventPythonScriptPath = tempPath;
                }
                else
                {
                    ExternalEventPythonScriptPath = null;
                }

                bool getErrorMessageResult = scope.TryGetVariable<string>("_error_message_", out string tempErrorMessage);
                if (getErrorMessageResult)
                {
                    errorMessage = tempErrorMessage;
                }

                bool getElementSetResult = scope.TryGetVariable<ElementSet>("_element_set_", out ElementSet tempElementSet);
                if (getElementSetResult)
                {
                    elementSelection = tempElementSet;
                }
                #endregion
                
            }
        }

        public string GetName()
        {
            return "DKTools Python Engine with External Event";
        }

        public void Execute(UIApplication app)
        {
            if (ExternalEventPythonScriptPath.Any())
            {
                ScriptSource externalScriptSource = PythonEngine.CreateScriptSourceFromFile(ExternalEventPythonScriptPath);
                //externalScriptSource.Compile();
                externalScriptSource.Execute(LastUsedScope);
            }
        }

        private string RelativeToAbsolutePath(string path)
        {
            throw new NotImplementedException();
        }
    }
}
