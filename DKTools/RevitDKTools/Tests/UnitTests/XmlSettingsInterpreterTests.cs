using NUnit.Framework;
using RevitDKTools.Commands.Generate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using System.Resources;
using System.Reflection;
using System.Windows;
using System.IO;
using System.Xml.Linq;

namespace RevitDKTools.Tests.UnitTests
{
    [TestFixture]
    public class XmlSettingsInterpreterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Cleanup()
        {
        }

        [Test]
        public void XmlSettingsInterpreterTests_ReadXdoc()
        {
            // arrange
            string test = string.Empty;

            /*
            ResourceManager resourceManager = new ResourceManager(
                "RevitDKTools.Properties.Resources",
                Assembly.GetExecutingAssembly());

            Assert.Fail(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\",string.Empty) +
                      resourceManager.GetString("VISIBILITY_SWITCHER_XML_LOCATION"));
            */

            DefaultSettingsProvider defaultSettingsProvider = new DefaultSettingsProvider();
            XmlVisibilitySwitcherSettingsProvider switcherSettingsProvider =
                new XmlVisibilitySwitcherSettingsProvider();
            XmlSettingsInterpreter interpreter =
            new XmlSettingsInterpreter(defaultSettingsProvider,switcherSettingsProvider);

            // act
            /*
            XDocument xdoc = switcherSettingsProvider.XDoc;
            IEnumerable<VisibilitySwitcherCommandSetting> sth = xdoc.Root.Elements()
                .Where(e => e.Name.LocalName == "VisibilitySwitcher")
                .Where(e => e.Attribute(XName.Get("CommandName")) != null)
                .Where(e => e.Attribute(XName.Get("VisibilityNameRegex")) != null)
                .Select(e =>
                {
                    var v = new VisibilitySwitcherCommandSetting();
                    v.CommandName = e.Attribute(XName.Get("CommandName")).Value;
                    v.VisibilityNameRegex = e.Attribute(XName.Get("VisibilityNameRegex")).Value;
                    return v;
                });

            foreach (var e in sth)
            {
                test = test + "commandName = " + e.CommandName + " | " + " regex = " +
                    e.VisibilityNameRegex + "\r\n";
            }
            */

            foreach (var e in interpreter.VisibilitySwitcherCommandSettings)
            {
                test = test + "commandName = " + e.CommandName + " | " + " regex = " +
                    e.VisibilityNameRegex + "\r\n";
            }

            // assert
            Assert.Fail(test);
        }
    }
}
