using NUnit.Framework;
using RevitDKTools.Commands.Generate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Tests.UnitTests
{
    [TestFixture]
    public class VisibilitySwitcherTests
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
        public void Switcher_Test()
        {
            string test = string.Empty;
            VisibilitySwitcherBaseClass switcher = new VisibilitySwitcherBaseClass();
            switcher.Execute(Helpers.GeneralHelper.ExternalCommandData,
                ref test, new Autodesk.Revit.DB.ElementSet());
           
        }
    }
}
