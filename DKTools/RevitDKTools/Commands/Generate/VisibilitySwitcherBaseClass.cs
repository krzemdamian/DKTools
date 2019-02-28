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
        private IList<ParameterFilterElement> _filterElementsAppliedToView;

        public IList<ParameterFilterElement> FiltersAppliedToView
        {
            get { return _filterElementsAppliedToView; }
        }

        public Result Execute(ExternalCommandData commandData, ref string message,
            ElementSet elements)
        {
            GetFiltersAppliedToView(commandData);
            return Result.Succeeded;
        }

        public void SwitchVisibilitySetting()
        {
        }

        private void GetFiltersAppliedToView(ExternalCommandData commandData)
        {
            View view = commandData.Application.ActiveUIDocument.Document.ActiveView;
            ICollection<ElementId> filterIds = view.GetFilters();
            Document doc = commandData.Application.ActiveUIDocument.Document;
            _filterElementsAppliedToView = new List<ParameterFilterElement>();
            foreach (ElementId id in filterIds)
            {
                _filterElementsAppliedToView.Add(doc.GetElement(id) as ParameterFilterElement);
            }
        }
    }
}
