#if DEBUG
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
using System.Windows;
using RevitDKTools.Command.ButtonData;

namespace RevitDKTools.Tests.UnitTests
{
    [TestFixture]
    public class DynamicButtonGeneratorTests
    {
        private RibbonPanel _panel;

        [SetUp]
        public void Setup()
        {
            List<RibbonPanel> lrp = Helpers.GeneralHelper.ActiveUIDocument.Application.GetRibbonPanels();
            foreach(RibbonPanel rp in lrp)
            {
                if (rp.Name == "Test")
                {
                    _panel = rp;
                }
            }

            if (_panel == null)
            {
                _panel = Helpers.GeneralHelper.ActiveUIDocument.Application.CreateRibbonPanel("Test");
            }
        }

        [TearDown]
        public void Cleanup()
        {
            // cleanup
        }

       
        [Test]
        public void DynamicButtonGeneratorTests_GetNode()
        {
            // arrange
            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            xml.Load("ScriptsSettings.xml");
            DynamicButtonGenerator generator = new DynamicButtonGenerator(xml,_panel);

            // act
            generator.ReadXML();
            generator.CreateDynamicAssembly();
            generator.CreateButtons();

            //TODO: change to check assembly
            // assert
            Assert.Fail();
        }

        [Test]
        public void BitmapTest()
        {
            string output = Path.GetFullPath("Path/Images\\bug.png");
            //string imagePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
            //            "\\PythonScripts\\" + cmd["ImageUri"];
            //string output = string.Empty;
            //foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    output = output + ass.FullName + Environment.NewLine;
            //}
            Assert.Fail(output);
        }
        /*
        [Test]
        public void AssemblyTest()
        {
            string output = string.Empty;
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                output = output + ass.FullName + Environment.NewLine;
            }
            Assert.Fail(output);
        }
        
        [Test]
        public void AssemblyTest2()
        {
            string location;
            if (!string.IsNullOrEmpty(Assembly.GetExecutingAssembly().Location))
            {
                location = Assembly.GetExecutingAssembly().Location;
                location = Path.GetDirectoryName(location);
                location = location + "\\temp";
            }
            else
            {
                location = Path.GetTempPath();
            }
            Assert.Fail(location);
        }*/
    }

}
#endif