/*
// Tests became obsolete after refactoring
// It require rebuilding from MyPythonEngine to PythonExecutionEnvironment
#if DEBUG
using NUnit.Framework;
using System;
using Helpers;
using RevitDKTools.Commands.Embed.Receiver;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Moq;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using RevitDKTools.Commands.Generate;

namespace RevitDKTools.Tests.UnitTests
{
    [TestFixture]
    public class PythonExecutionEnvironmentTests
    {
        [SetUp]
        public void Setup()
        {
            // setup pre-test environment
        }

        [TearDown]
        public void Cleanup()
        {
            // cleanup
        }

       
        [Test]
        public void PythonExecutionEnvironment_instanceCreation()
        {
            PythonExecutionEnvironment instance = new PythonExecutionEnvironment();
            Assert.IsInstanceOf(typeof(MyPythonEngine), instance);
        }

        [Test]
        public void MyPythonEngine_publicPropertySelf()
        {
            MyPythonEngine instance = new MyPythonEngine();
            MyPythonEngine propertyInstance = instance.EngineInstance;
            Assert.IsInstanceOf(typeof(MyPythonEngine), propertyInstance);
        }

        [Test]
        public void MyPythonEngine_RuntimeCreation()
        {
            MyPythonEngine instance = new MyPythonEngine();
            Assert.IsInstanceOf(typeof(ScriptRuntime), instance.PythonScriptRuntime);
        }

        [Test]
        public void MyPythonEngine_PythonEnigneCreation()
        {
            MyPythonEngine instance = new MyPythonEngine();
            Assert.IsInstanceOf(typeof(ScriptEngine), instance.PythonEngine);
        }

        [Test]
        public void MyPythonEngine_RuntimeSetupLightScopes()
        {
            MyPythonEngine instance = new MyPythonEngine();
            IList<LanguageSetup> options = instance.PythonScriptRuntime.Setup.LanguageSetups;
            
            bool lightweightScopesCheck = false;
            
            foreach (LanguageSetup option in options)
            {
                IDictionary<string,object> pyOptions = option.Options;
                foreach (KeyValuePair<string,object> opt in pyOptions)
                {
                    if (opt.Key == "LightweightScopes" && (bool)opt.Value == true) { lightweightScopesCheck = true; };
                }
            }
            Assert.True(lightweightScopesCheck);
        }


        [Test]
        public void MyPythonEngine_PythonBuiltIn_RunScript_LastUsedScope()
        {
            MyPythonEngine instance = new MyPythonEngine();

            string[] lines =
            {
                "#it is commentary line",
                "_return_ = 'testing'",
            };
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllLines(fileName, lines);
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            //string result = instance.TestRun(fileName);
            File.Delete(fileName);

            IEnumerable<KeyValuePair<string, dynamic>> items = instance.LastUsedScope.GetItems();
            System.Collections.Generic.List<string> itemNames = new List<string>();
            foreach (KeyValuePair<string, dynamic> item in items)
            {
                itemNames.Add(item.Key);
            }

            Assert.Contains("_return_",itemNames);
        }

        [Test]
        public void MyPythonEngine_RunScript_GetFromScript_event_path()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string[] lines =
            {
                "#it is commentary line",
                "_return_ = 'testing'",
                "_event_path_ = 'some_path'",
            };
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllLines(fileName, lines);

            //Action
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            File.Delete(fileName);


            //Assert
            Assert.IsNotNullOrEmpty(instance.ExternalEventPythonScriptPath[0]);            
        }

        [Test]
        public void MyPythonEngine_RunScript_GetFromScript_error_message()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string[] lines =
            {
                "#it is commentary line",
                "_return_ = 'testing'",
                "_error_message_ = 'test'",
            };
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllLines(fileName, lines);

            //Action
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            File.Delete(fileName);


            //Assert
            Assert.AreEqual("test",message);
        }

        [Test]
        public void MyPythonEngine_SetVariableToScript_command_data()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string[] lines =
            {
                "#it is commentary line",
                "_return_ = _command_data_",
            };
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllLines(fileName, lines);

            //Action
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            File.Delete(fileName);
            ExternalCommandData retrivedCmdData = instance.LastUsedScope.GetVariable<ExternalCommandData>("_return_");


            //Assert
            Assert.AreSame(retrivedCmdData, Helpers.GeneralHelper.ExternalCommandData);
        }

        [Test]
        public void MyPythonEngine_Load_RevitAPI()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string[] lines =
            {
                "#it is commentary line",
                "import clr",
                "import Autodesk",
                "from Autodesk.Revit.DB import ElementId",
                "from Autodesk.Revit.UI import UIApplication"
            };
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllLines(fileName, lines);

            //Action
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            File.Delete(fileName);

            //Assert
            IEnumerable<KeyValuePair<string, dynamic>> items = instance.LastUsedScope.GetItems();
            System.Collections.Generic.List<string> itemNames = new List<string>();
            foreach (KeyValuePair<string, dynamic> item in items)
            {
                itemNames.Add(item.Key);
            }
            Assert.Contains("Autodesk", itemNames);
            Assert.Contains("UIApplication", itemNames);
        }

        [Test]
        public void MyPythonEngine_RunScript_SetVariable_handler()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string scriptText = @"
 
import rvt

all_items = dir(rvt)
_result_ = False
if '_handler_' in all_items:
    _result_ = True

str_items = ''
for item in all_items:
    str_items = str_items + item + '\r\n'

handler = rvt._handler_

";
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllText(fileName, scriptText);

            //Action
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            File.Delete(fileName);

            //Assert
            IEnumerable<KeyValuePair<string, dynamic>> items = instance.LastUsedScope.GetItems();
            System.Collections.Generic.List<string> itemNames = new List<string>();
            foreach (KeyValuePair<string, dynamic> item in items)
            {
                itemNames.Add(item.Key);
            }
            Assert.AreSame(instance, instance.LastUsedScope.GetVariable<MyPythonEngine>("handler"));
            //Assert.Fail(instance.LastUsedScope.GetVariable<string>("str_items"));
            //Assert.IsTrue(instance.LastUsedScope.GetVariable<bool>("_result_"));
        }

        [Test]
        public void MyPythonEngine_RunScript_SetVariable_event()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string scriptText = @"
 
import rvt

all_items = dir(rvt)
_result_ = False
if '_event_' in all_items:
    _result_ = True

str_items = ''
for item in all_items:
    str_items = str_items + item + '\r\n'

event = rvt._event_

";
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllText(fileName, scriptText);

            //Action
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            File.Delete(fileName);

            //Assert
            IEnumerable<KeyValuePair<string, dynamic>> items = instance.LastUsedScope.GetItems();
            System.Collections.Generic.List<string> itemNames = new List<string>();
            foreach (KeyValuePair<string, dynamic> item in items)
            {
                itemNames.Add(item.Key);
            }
            Assert.IsInstanceOf(typeof(ExternalEvent), instance.LastUsedScope.GetVariable<ExternalEvent>("event"));
            //Assert.AreSame(instance, instance.LastUsedScope.GetVariable<MyPythonEngine>("event"));
            //Assert.Fail(instance.LastUsedScope.GetVariable<string>("str_items"));
            //Assert.IsTrue(instance.LastUsedScope.GetVariable<bool>("_result_"));
        }

        [Test]
        public void MyPythonEngine_CompiledPythonScripts()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string scriptText = @"
 
import rvt
";
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllText(fileName, scriptText);

            scriptText = @"
 
import rvt
";
            string fileName2 = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllText(fileName2, scriptText);

            //Action
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            instance.RunScript(fileName2, Helpers.GeneralHelper.ExternalCommandData, out string message2, new ElementSet());
            File.Delete(fileName);
            File.Delete(fileName2);

            //Assert
            Assert.Greater(instance.CompiledPythonScripts.Count,1);
        }


        [Test]
        public void MyPythonEngine_CommandScopes()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string scriptText = @"
 
import rvt
";
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllText(fileName, scriptText);

            scriptText = @"
 
import rvt
";
            string fileName2 = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllText(fileName2, scriptText);

            //Action
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            instance.RunScript(fileName2, Helpers.GeneralHelper.ExternalCommandData, out string message2, new ElementSet());
            File.Delete(fileName);
            File.Delete(fileName2);

            //Assert
            Assert.Greater(instance.CommandScopes.Count,1);
        }


        [Test]
        public void MyPythonEngine_RunScript_Twice_and_Get_variables()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string scriptText = @"
 
import rvt
import Autodesk.Revit.DB as db

all_items = dir(rvt)
_result_ = False
if '_event_' in all_items:
    _result_ = True

str_items = ''
for item in all_items:
    str_items = str_items + item + '\r\n'

event = rvt._event_
_event_path_ = 'testing'
_error_message_ = 'testing2'

_element_set_ = db.ElementSet()

";
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            File.WriteAllText(fileName, scriptText);

            //Action
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            instance.RunScript(fileName, Helpers.GeneralHelper.ExternalCommandData, out message, new ElementSet());
            File.Delete(fileName);

            //Assert
            IEnumerable<KeyValuePair<string, dynamic>> items = instance.LastUsedScope.GetItems();
            System.Collections.Generic.List<string> itemNames = new List<string>();
            foreach (KeyValuePair<string, dynamic> item in items)
            {
                itemNames.Add(item.Key);
            }
            Assert.IsInstanceOf(typeof(ExternalEvent), instance.LastUsedScope.GetVariable<ExternalEvent>("event"));
            Assert.AreEqual("testing", instance.LastUsedScope.GetVariable<string>("_event_path_"));
            Assert.AreEqual("testing2", message);
            Assert.IsInstanceOf(typeof(ElementSet), instance.LastUsedScope.GetVariable<ElementSet>("_element_set_"));
        }

        [Test]
        public void MyPythonEngine_RunExternalEvent()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string mainScriptPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";
            string externalEventScriptPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            string scriptText = $@"
 
import rvt
import Autodesk.Revit.DB as db

_event_path_ = '{externalEventScriptPath}'
_event_ = rvt._event_
_event_.Raise()


";

            string externalEventScript = @"
 
import rvt
import Autodesk.Revit.DB as db

_result_ = 'external_script'
";

            File.WriteAllText(mainScriptPath, scriptText);
            File.WriteAllText(externalEventScriptPath, externalEventScript);

            //Action
            instance.RunScript(mainScriptPath, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            File.Delete(mainScriptPath);
            File.Delete(externalEventScriptPath);

            //Assert
            IEnumerable<KeyValuePair<string, dynamic>> items = instance.LastUsedScope.GetItems();
            System.Collections.Generic.List<string> itemNames = new List<string>();
            foreach (KeyValuePair<string, dynamic> item in items)
            {
                itemNames.Add(item.Key);
            }
            Assert.IsTrue(instance.LastUsedScope.GetVariable<ExternalEvent>("_event_").IsPending);
        }

        [Test]
        public void MyPythonEngine_SetAndGetSharedVariabe()
        {
            //Arrange
            MyPythonEngine instance = new MyPythonEngine();

            string mainScriptPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";
            string secondScriptPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";

            string scriptText = $@"
 
import rvt
import Autodesk.Revit.DB as db

rvt._variables_['memo'] = 'test'
rvt._variables_['number'] = 2

";

            string secondScript = @"
import rvt
import Autodesk.Revit.DB as db

_result_ = rvt._variables_['memo']
";

            File.WriteAllText(mainScriptPath, scriptText);
            File.WriteAllText(secondScriptPath, secondScript);

            //Action
            instance.RunScript(mainScriptPath, Helpers.GeneralHelper.ExternalCommandData, out string message, new ElementSet());
            instance.RunScript(secondScriptPath, Helpers.GeneralHelper.ExternalCommandData, out message, new ElementSet());
            File.Delete(mainScriptPath);
            File.Delete(secondScriptPath);

            //Assert
            IEnumerable<KeyValuePair<string, dynamic>> items = instance.LastUsedScope.GetItems();
            System.Collections.Generic.List<string> itemNames = new List<string>();
            foreach (KeyValuePair<string, dynamic> item in items)
            {
                itemNames.Add(item.Key);
            }
            Assert.AreEqual(instance.LastUsedScope.GetVariable<string>("_result_"), "test");
            
        }
    }

}
#endif
*/