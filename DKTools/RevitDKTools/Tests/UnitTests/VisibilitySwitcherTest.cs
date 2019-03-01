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

        // TODO: Create test. Arrange should create filter, assign it to view 
        //       and chek if switcher method changes it.
        //[Test]
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
            Assert.Fail("Check if transaction has been added. In other words:\r\ncheck " +
                "undo button. If there is transaction \"Switch Visibility\" it is correct.");
        } 
    }
}
