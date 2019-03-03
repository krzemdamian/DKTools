using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Text.RegularExpressions;
using RevitDKTools.Common;

namespace RevitDKTools.Commands.Generate
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class VisibilitySwitcherBaseClass : IExternalCommand
    {
        public string _visibilityNameRegex;
        private View _view;
        Document _doc;
        private IList<ParameterFilterElement> _filterElementsAppliedToView;

        public IList<ParameterFilterElement> FiltersAppliedToView
        {
            get { return _filterElementsAppliedToView; }
        }

        public VisibilitySwitcherBaseClass() { }

        public Result Execute(ExternalCommandData commandData, ref string message,
            ElementSet elements)
        {
            GetFiltersAppliedToView(commandData);
            EliminateFiltersNotMatchingRegex();
            SwitchFilters();
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

        public void SwitchFilters(ParameterFilterElement filter = null)
        {
            if (IsViewTemplateApplied())
            {
                if (UserDecideToKeepTemplate()) return;
            }

            try
            {
                Transaction t = new Transaction(_doc, "Switch Visibility");
                t.Start();
                if (filter != null)
                {
                    _view.SetFilterVisibility(filter.Id, !_view.GetFilterVisibility(filter.Id));
                }
                else
                {
                    foreach (ParameterFilterElement f in _filterElementsAppliedToView)
                    {
                        _view.SetFilterVisibility(f.Id, !_view.GetFilterVisibility(f.Id));
                    }
                }
                t.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool UserDecideToKeepTemplate()
        {
            string templateName = _doc.GetElement(_view.ViewTemplateId).Name;
            TaskDialog taskDialog = new TaskDialog("Template Applied");
            taskDialog.MainContent = string.Format("Do you want to discard\r\n{0}\r\n" +
                "template and switch requested visibility?", templateName);
            taskDialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
            taskDialog.DefaultButton = TaskDialogResult.Yes;
            TaskDialogResult result = taskDialog.Show();
            if (result == TaskDialogResult.Yes)
            {
                Transaction t = new Transaction(_doc, "Discard Template");
                t.Start();
                _view.ViewTemplateId = ElementId.InvalidElementId;
                t.Commit();
                return false;
            }
            return true;
        }

        private bool IsViewTemplateApplied()
        {
            return !(_view.ViewTemplateId == ElementId.InvalidElementId);
        }

        private void EliminateFiltersNotMatchingRegex()
        {
            if (RegexUtils.IsValidRegex(_visibilityNameRegex))
            {
                IList<ParameterFilterElement> filtersMatchingRegex =
                    new List<ParameterFilterElement>();
                foreach (ParameterFilterElement filterElement in _filterElementsAppliedToView)
                {
                    if (Regex.IsMatch(filterElement.Name, _visibilityNameRegex))
                    {
                        filtersMatchingRegex.Add(filterElement);
                    }
                }
                _filterElementsAppliedToView = filtersMatchingRegex;
            }
            else
            {
                MessageBox.Show("Invalid regex!");
            }
        }
    }
}
