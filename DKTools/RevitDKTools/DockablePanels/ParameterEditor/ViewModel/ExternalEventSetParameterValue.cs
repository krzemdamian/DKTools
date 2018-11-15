using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitDKTools.DockablePanels.ParameterEditor.ViewModel
{
    public class ExternalEventSetParameterValue : IExternalEventHandler
    {
        public Document RevitDocument { get; set; }
        public List<ElementId> ElementList { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }

        public void Execute(UIApplication app)
        {
            FilteredElementCollector collector = new FilteredElementCollector(RevitDocument, ElementList).WhereElementIsNotElementType();
            try
            {
                using (Transaction t = new Transaction(RevitDocument))
                {
                    t.SetName("DKTools: parameter edition");
                    t.Start();
                    foreach (Element el in collector.ToElements())
                    {
                        el.LookupParameter(ParameterName).Set(ParameterValue);
                    }
                    t.Commit();
                }
                RevitDocument = null;
                ElementList.Clear();
                ParameterName = string.Empty;
                ParameterValue = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption from ExternalEventSetParameterValue \r\nExeption message:\r\n" + ex.Message);
            }
        }

        public string GetName()
        {
            return "Parameter Updater";
        }
    }
            
}
