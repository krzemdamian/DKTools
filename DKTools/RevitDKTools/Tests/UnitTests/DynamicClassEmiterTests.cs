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

            // assert
            //Assert.Fail("assembly: " + dynamicType.Assembly.ToString());
            //Assert.Fail("module: " + dynamicType.Module.ToString());
            //Assert.Fail("is class : " + dynamicType.IsClass.ToString());
        }
    }

}
