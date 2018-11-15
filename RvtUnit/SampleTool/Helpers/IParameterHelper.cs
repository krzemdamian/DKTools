using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;

namespace SampleTool.Helper
{
	public interface IParameterHelper
	{
		string GetParameterValueAsString(string parameterName);

		bool HasParameter(string parameterName);

		bool SetParameterValue(string parameterName, string value);
	}
}
