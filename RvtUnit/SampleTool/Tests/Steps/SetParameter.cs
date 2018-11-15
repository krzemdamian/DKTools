using System;
using TechTalk.SpecFlow;
using SampleTool.Helper;
using Moq;
using NUnit.Framework;

namespace SampleTool.Tests.Steps
{
    [Binding]
    public class SetParameterSteps
    {
        [Given(@"I have a parameter called ""(.*)""")]
        public void GivenIHaveAParameterCalled(string paramName)
        {
			Mock<IParameterHelper> paramHelperMoq = new Mock<IParameterHelper>();
			paramHelperMoq.Setup(paramHelper => paramHelper.HasParameter(paramName)).Returns(true);
			ScenarioContext.Current.Add("paramHelperMoq", paramHelperMoq);
			ScenarioContext.Current.Add("paramName", paramName);
        }
        
        [When(@"I change its value to (.*)")]
        public void WhenIChangeItsValueTo(string value)
        {
			Mock<IParameterHelper> paramHelperMoq = (Mock<IParameterHelper>)ScenarioContext.Current["paramHelperMoq"];
			string paramName = (string)ScenarioContext.Current["paramName"];
			paramHelperMoq.Setup(paramHelper => paramHelper.GetParameterValueAsString(paramName)).Returns(value);
        }
        
        [Then(@"the ""(.*)"" value should be (.*)")]
        public void ThenTheValueShouldBe(string paramName, string value)
        {
			Mock<IParameterHelper> paramHelperMoq = (Mock<IParameterHelper>)ScenarioContext.Current["paramHelperMoq"];
			Assert.That(paramHelperMoq.Object.GetParameterValueAsString(paramName), Iz.EqualTo(value));
        }

		[AfterScenario]
		public void Cleanup()
		{
			// Note: you should always dispose mock object after using
			// to make it possible for GC to collect it
			Mock<IParameterHelper> paramHelperMoq = (Mock<IParameterHelper>)ScenarioContext.Current["paramHelperMoq"];
			paramHelperMoq.Dispose();
		}
    }
}
