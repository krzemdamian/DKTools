using NUnit.Framework;
using System;
using Helpers;
using RevitDKTools.Command.Receiver;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Moq;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace RevitDKTools.Tests.UnitTests
{
    [TestFixture]
    public class DynamicCommandClassEmiterTests
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
        public void DynamicCommandClassEmiter_CreateDynamicType()
        {
            // arrange
            DynamicCommandClassEmiter emiter = new DynamicCommandClassEmiter("my_assembly");
            
            // act
            Type dynamicType = emiter.BuildCommandType("my_command_class", "test");

            //TODO: change to check assembly
            // assert
            Assert.Fail("assembly: " + dynamicType.Assembly.ToString() + Environment.NewLine
                + "module: " + dynamicType.Module.ToString() + Environment.NewLine
                + "is class: " + dynamicType.IsClass.ToString() + Environment.NewLine
                + "base class: " + dynamicType.BaseType.Name.ToString() + Environment.NewLine);
        }

        [Test]
        public void DynamicCommandClassEmiter_InvokeInfo()
        {
            // arrange
            DynamicCommandClassEmiter emiter = new DynamicCommandClassEmiter("my_assembly");

            // act
            //MessageBox.Show(Helpers.GeneralHelper.ExternalCommandData.ToString());
            Type dynamicType = emiter.BuildCommandType(
                "InfoInvokerDynamic", @"E:\Repos\DKTools_refactoring\DKTools\RevitDKTools\PythonScripts\Test\Info.py");
            DynamicCommandBase infoInvoker = (DynamicCommandBase)Activator.CreateInstance(dynamicType);
            string message = string.Empty;
            
            MessageBox.Show(infoInvoker._scriptPath);
            infoInvoker.Execute(Helpers.GeneralHelper.ExternalCommandData, ref message, new ElementSet());
            


            //TODO: change to check assembly
            // assert
            Assert.Fail("assembly: " + dynamicType.Assembly.ToString() + Environment.NewLine
                + "module: " + dynamicType.Module.ToString() + Environment.NewLine
                + "is class: " + dynamicType.IsClass.ToString() + Environment.NewLine
                + "base class: " + dynamicType.BaseType.Name.ToString() + Environment.NewLine);

            
        }
    }

}
