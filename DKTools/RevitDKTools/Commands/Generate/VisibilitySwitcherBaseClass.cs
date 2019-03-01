using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows;
using System;

namespace RevitDKTools.Commands.Generate
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class VisibilitySwitcherBaseClass : IExternalCommand
    {
        private string _visibilityNameRegex;
        private View _view;
        Document _doc;
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

        private void GetFiltersAppliedToView(ExternalCommandData commandData)
        {
            _view = commandData.Application.ActiveUIDocument.Document.ActiveView;
            ICollection<ElementId> filterIds = _view.GetFilters();
            _doc = commandData.Application.ActiveUIDocument.Document;
            _filterElementsAppliedToView = new List<ParameterFilterElement>();
            foreach (ElementId id in filterIds)
            {
                _filterElementsAppliedToView.Add(_doc.GetElement(id) as ParameterFilterElement);
            }
        }

        public void SwitchVisibility(ParameterFilterElement filter)
        {
            try
            {
                Transaction t = new Transaction(_doc, "Switch Visibility");
                t.Start();
                _view.SetFilterVisibility(filter.Id, !_view.GetFilterVisibility(filter.Id));
                t.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
