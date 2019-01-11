using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Scripting.Hosting;
using System.Reflection;
using RevitDKTools.Commands.Panels;
using RevitDKTools.Commands.Embed.Receiver;
using RevitDKTools.DockablePanels.ParameterEditor.View;
using Autodesk.Revit.UI.Events;
using System.IO;
using RevitDKTools.Commands.Generate;

namespace RevitDKTools
{
    public class DKToolsApp : IExternalApplication
    {
        public static DKToolsApp DKToolsAppInstance { get; set; }
        public static UIControlledApplication UIControlledApplication { get; set; } = null;
        public static IPythonExecutionEnviroment MyPythonEngine { get; set; }

        private ParameterEditorWPFPage _parameterEditorWPFPage;
        private RibbonPanel _commandsRibbonPanel;

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            AssignProperties(application);
            InstantiatePythonEngine();
            CreateButtonsOnRevitRibbon(application);
            PrepareParameterEditorDockablePanel(application);
            CreateDynamicAssemblyAsProxyForPythonScripts();

            return Result.Succeeded;
        }
        
        private void CreateDynamicAssemblyAsProxyForPythonScripts()
        {
            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            xml.Load(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                "\\PythonScripts\\ScriptsSettings.xml");
            DynamicButtonGenerator generator = new DynamicButtonGenerator(xml, _commandsRibbonPanel);
            generator.GenerateDynamicButtons();
        }

        private void PrepareParameterEditorDockablePanel(UIControlledApplication application)
        {
            CreateDockablePanel(application);
            application.ViewActivated += new EventHandler
                <Autodesk.Revit.UI.Events.ViewActivatedEventArgs>(PassRevitDocumentInstance_OnViewActivated);
            RegisterDocablePanelToRevit(application);
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

        private void CreateDockablePanel(UIControlledApplication application)
        {
            _parameterEditorWPFPage = new ParameterEditorWPFPage();
            _parameterEditorWPFPage.VM.RevitSelectionWatcher = new SelectionChangedWatcher(application);
            _parameterEditorWPFPage.VM.RevitSelectionWatcher.SelectionChanged +=
                new EventHandler(_parameterEditorWPFPage.VM.RevitActiveSelection_SelectionChanged);
            _parameterEditorWPFPage.VM.RevitSelectionWatcher.SelectionChanged +=
                new EventHandler(_parameterEditorWPFPage.OverwriteContentInRichTextBox);
        }

        private void CreateButtonsOnRevitRibbon(UIControlledApplication application)
        {
            _commandsRibbonPanel = application.CreateRibbonPanel("Commands");
            RibbonPanelButtonMaker ribbonPanelButtonMaker = new RibbonPanelButtonMaker(new CombinedCommandsPanel(), _commandsRibbonPanel);
            ribbonPanelButtonMaker.BuildButtons();
        }

        private static void InstantiatePythonEngine()
        {
            MyPythonEngine = new PythonExecutionEnviroment();
        }

        private void AssignProperties(UIControlledApplication application)
        {
            DKToolsAppInstance = this;
            UIControlledApplication = application;
        }

        private void PassRevitDocumentInstance_OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            if (_parameterEditorWPFPage.RevitDocument != e.Document)
            {
                _parameterEditorWPFPage.RevitDocument = e.Document;
            }
        }
    }
}
