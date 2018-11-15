using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Autodesk.Revit.DB;
using SampleTool.Helper;

namespace SampleTool.Tests.UnitTests
{
	[TestFixture]
	public class HasParameter_Tests
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
		public void ShouldNotHaveParameter_Test()
		{
			Document document = Helpers.GeneralHelper.ActiveUIDocument.Document;
			IParameterHelper parameterHelper = new ParameterHelper(document);
			Assert.That(parameterHelper.HasParameter("SomeParameterThatNormallyDoesnotExist"), Iz.False);
		}

		[Test]
		public void ShouldHaveParameter_Test()
		{
			Document document = Helpers.GeneralHelper.ActiveUIDocument.Document;
			IParameterHelper parameterHelper = new ParameterHelper(document);
			Assert.That(parameterHelper.HasParameter("Project Name"), Iz.True);
		}

        [Test]
        public void MySuperNewTest()
        {
            Assert.Fail("This is actualy my second test in the word!\n" +
                "This time it's performed in REVIT!!!");
        }
	}
}
