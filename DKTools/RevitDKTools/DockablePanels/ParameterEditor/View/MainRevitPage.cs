using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitDKTools.DockablePanels.ParameterEditor.View
{
    public class ParameterEditorWPFPage : MainPage, IDockablePaneProvider
    {
        public Autodesk.Revit.DB.Document RevitDocument
        {
            get { return VM.RevitDocument; }
            set
            {
                VM.RevitDocument = value;
            }
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            try
            {
                data.FrameworkElement = this as FrameworkElement;
                data.InitialState = new Autodesk.Revit.UI.DockablePaneState();
                data.InitialState.DockPosition = DockPosition.Bottom;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
