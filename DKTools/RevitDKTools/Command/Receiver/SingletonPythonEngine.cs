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

namespace RevitDKTools.Command.Receiver
{
    public class SingletonPythonEngine : IExternalEventHandler, IMyPythonEngine
    {
        public SingletonPythonEngine EngineInstance { get; set; }
        //private Dictionary<string, object> _pythonScriptRuntimeOptions = new Dictionary<string, object>();
        public ScriptRuntime PythonScriptRuntime { get; set; }
        public ScriptEngine PythonEngine { get; set; }
        public Dictionary<string, CompiledCode> CompiledPythonScripts { get; set; }
        public string ExternalEventPythonScriptPath { get; set; }
        public ScriptScope LastUsedScope { get; set; }

        public SingletonPythonEngine()
        {
            Dictionary<string, object> engineOptions = new Dictionary<string, object>
            {
                ["LightweightScopes"] = true
            };
            EngineInstance = this;
            PythonScriptRuntime = Python.CreateRuntime(engineOptions);
            /*
            PythonScriptRuntime.LoadAssembly(Assembly.Load("Autodesk.Revit.UI"));
            PythonScriptRuntime.LoadAssembly(Assembly.Load("Autodesk.Revit.DB"));
            PythonScriptRuntime.LoadAssembly(Assembly.Load("System.Windows.Forms"));
            PythonScriptRuntime.LoadAssembly(Assembly.Load("System.Drawing"));
            */
            PythonEngine = Python.CreateEngine();
            PythonEngine.Execute("testVariable = 'this is string'");
            /*
            PythonEngine.GetBuiltinModule().SetVariable("__application__", RevitDKTools.DKToolsApp.Application);
            PythonEngine.GetBuiltinModule().SetVariable("__event_handler__", EngineInstance);
            
            ExternalEvent exEvent = ExternalEvent.Create(EngineInstance);
            PythonEngine.GetBuiltinModule().SetVariable("__external_event__", exEvent);
            */
        }

        public string TestRun(string scriptPath)
        {
            ScriptSource source = PythonEngine.CreateScriptSourceFromFile(scriptPath);
            ScriptScope scope = PythonEngine.CreateScope();
            scope.SetVariable("__script_path__", scriptPath);
            source.Execute(scope);
            bool getReturn = scope.TryGetVariable<string>("__return__", out string tempReturn);
            if (getReturn)
            {
                return tempReturn;
            }
            else
            {
                return scriptPath;
            }
        }

        public void RunScript(string commandPath, ExternalCommandData commandData,
            out string errorMessage, ElementSet elementSelection)
        {
            errorMessage = "No additional information";

            if (!CompiledPythonScripts.ContainsKey(commandPath))
            {
                ScriptSource source = PythonEngine.CreateScriptSourceFromFile(commandPath);
                ScriptScope scope = PythonEngine.CreateScope();
                scope.SetVariable("__command_data__", commandData);
                CompiledCode compiled = source.Compile();
                CompiledPythonScripts.Add(commandPath, compiled);
                compiled.Execute(scope);


                // region where engine tries to retrive __event_path__, __error_message__, __element_set__
                #region _get_information_from_python_script_
                bool getPathResult = scope.TryGetVariable<string>("__event_path__", out string tempPath);
                if (getPathResult)
                {
                    ExternalEventPythonScriptPath = tempPath;
                }
                else
                {
                    ExternalEventPythonScriptPath = null;
                }

                bool getErrorMessageResult = scope.TryGetVariable<string>("__error_message__", out string tempErrorMessage);
                if (getErrorMessageResult)
                {
                    errorMessage = tempErrorMessage;
                }

                bool getElementSetResult = scope.TryGetVariable<ElementSet>("__element_set__", out ElementSet tempElementSet);
                if (getElementSetResult)
                {
                    elementSelection = tempElementSet;
                }
                #endregion


            }

            if (CompiledPythonScripts.ContainsKey(commandPath))
            {
                ScriptScope defaultScope = CompiledPythonScripts[commandPath].DefaultScope;
                defaultScope.SetVariable("__command_data__", commandData);
                CompiledPythonScripts[commandPath].Execute();

                // region where engine tries to retrive __event_path__, __error_message__, __element_set__
                #region _get_information_from_python_script_
                bool getPathResult = defaultScope.TryGetVariable<string>("__event_path__", out string tempPath);
                if (getPathResult)
                {
                    ExternalEventPythonScriptPath = tempPath;
                }
                else
                {
                    ExternalEventPythonScriptPath = null;
                }

                bool getErrorMessageResult = defaultScope.TryGetVariable<string>("__error_message__", out string tempErrorMessage);
                if (getErrorMessageResult)
                {
                    errorMessage = tempErrorMessage;
                }

                bool getElementSetResult = defaultScope.TryGetVariable<ElementSet>("__element_set__", out ElementSet tempElementSet);
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
                externalScriptSource.Compile();
                externalScriptSource.Execute(LastUsedScope);
            }
        }

        private string RelativeToAbsolutePath(string path)
        {
            throw new NotImplementedException();
        }
    }
}
