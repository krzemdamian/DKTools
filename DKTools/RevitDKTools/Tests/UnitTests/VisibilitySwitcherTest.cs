using NUnit.Framework;
using RevitDKTools.Commands.Generate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

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
        public void VisibilitySwitcher_GetFiltersFromView()
        {
            // arrange
            string test = string.Empty;
            VisibilitySwitcherBaseClass switcher = new VisibilitySwitcherBaseClass();

            // act
            switcher.Execute(Helpers.GeneralHelper.ExternalCommandData,
                ref test, new ElementSet());
            IList<ParameterFilterElement> filters = switcher.FiltersAppliedToView;
            string output = string.Empty;
            foreach (ParameterFilterElement element in filters)
            {
                output = output + element.Name + "\r\n";
            }

            // assert
            Assert.IsInstanceOf(typeof(IList<ParameterFilterElement>),filters);
        }

        [Test]
        public void VisibilitySwitcher_SwitchVisibility()
        {
            // arrange
            string test = string.Empty;
            VisibilitySwitcherBaseClass switcher = new VisibilitySwitcherBaseClass();
            switcher.Execute(Helpers.GeneralHelper.ExternalCommandData,
                ref test, new ElementSet());

            // act
            switcher.SwitchVisibility(switcher.FiltersAppliedToView[0]);

            // assert
        } 
    }
}
