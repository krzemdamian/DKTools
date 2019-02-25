using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows;

namespace RevitDKTools.Commands.Generate
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class VisibilitySwitcherBaseClass : IExternalCommand
    {
        private string _visibilityNameRegex;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            View view = commandData.Application.ActiveUIDocument.Document.ActiveView;
            ICollection<ElementId> filters = view.GetFilters();
            string output = string.Empty;
            foreach (var id in filters)
            {
                output = output + id + "\r\n";
            }

            MessageBox.Show(output);

            return Result.Succeeded;
        }
    }
}
