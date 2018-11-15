using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using SampleTool.Helper;

namespace SampleTool
{
   [Transaction(TransactionMode.Manual)]
	public class SampleToolCommand : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
		{

         IParameterHelper ph = new ParameterHelper(commandData.Application.ActiveUIDocument.Document);

		   var paramName = "SomeParameterThatNormallyDoesnotExist";
		   var exists = ph.HasParameter(paramName);

		   MessageBox.Show(paramName + " exists: " + exists);


         paramName = "Project Name";
         exists = ph.HasParameter(paramName);

         MessageBox.Show(paramName + " exists: " + exists);

			return Result.Succeeded;
		}
	}
}
