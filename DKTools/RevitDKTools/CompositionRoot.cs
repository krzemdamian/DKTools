using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using RevitDKTools.Commands.Generate;
using RevitDKTools.Commands.Panels;
using RevitDKTools.DockablePanels.ParameterEditor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools
{
    class CompositionRoot : ICompositionRoot
    {
        private UIControlledApplication _application;
        private ParameterEditorWPFPage _parameterEditorWPFPage;

        public CompositionRoot(UIControlledApplication application, ICommandsGenerator commandsGenerator)
        {
            _application = application;

            PrepareParameterEditorDockablePanel();
            commandsGenerator.GenerateDynamicCommands();
        }

        private void PrepareParameterEditorDockablePanel()
        {
            CreateDockablePanel(_application);
            _application.ViewActivated += new EventHandler
                <Autodesk.Revit.UI.Events.ViewActivatedEventArgs>(PassRevitDocumentInstance_OnViewActivated);
            RegisterDocablePanelToRevit(_application);
        }

        private void PassRevitDocumentInstance_OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            if (_parameterEditorWPFPage.RevitDocument != e.Document)
            {
                _parameterEditorWPFPage.RevitDocument = e.Document;
            }
        }

        private void CreateDockablePanel(UIControlledApplication application)
        {
            _parameterEditorWPFPage = new ParameterEditorWPFPage();
            _parameterEditorWPFPage.VM.RevitSelectionWatcher = new SelectionChangedWatcher(application);
            _parameterEditorWPFPage.VM.RevitSelectionWatcher.SelectionChanged +=
                new EventHandler(_parameterEditorWPFPage.VM.RevitActiveSelection_SelectionChanged);
            _parameterEditorWPFPage.VM.RevitSelectionWatcher.SelectionChanged +=
                new EventHandler(_parameterEditorWPFPage.OverwriteContentInRichTextBox);
        }

        private void RegisterDocablePanelToRevit(UIControlledApplication application)
        {
            DockablePaneProviderData dockablePaneProviderData = new DockablePaneProviderData();
            dockablePaneProviderData.FrameworkElement = _parameterEditorWPFPage as System.Windows.FrameworkElement;
            dockablePaneProviderData.InitialState = new DockablePaneState();
            dockablePaneProviderData.InitialState.DockPosition = DockPosition.Bottom;
            DockablePaneId dpid = new DockablePaneId(
                new Guid("{F1D5DCB2-DB78-483C-8A77-C7BD7CBC6557}"));
            application.RegisterDockablePane(
                dpid, "DKTools: Parameter Editor", _parameterEditorWPFPage
                as IDockablePaneProvider);
        }
   }
}
