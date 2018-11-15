using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core;

namespace rvtUnit.Helpers
{
	public class SimpleTestFilter : TestFilter
	{
		IEnumerable<Models.Test> _tests;
		public SimpleTestFilter(IEnumerable<Models.Test> tests)
		{
			_tests = tests;
		}

		public override bool Match(ITest test)
		{
			if (_tests == null) { return true; }
			return _tests.Any(t => t.TestName == test.TestName.Name && t.IsChecked);
		}
	}
}
