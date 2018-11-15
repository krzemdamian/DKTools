using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Scripting.Hosting;
using System.Reflection;
using RevitDKTools.Panels;
using RevitDKTools.Command.Receiver;
using RevitDKTools.DockablePanels.ParameterEditor.View;
using Autodesk.Revit.UI.Events;

namespace RevitDKTools
{
    public class DKToolsApp : IExternalApplication
    {
        public static DKToolsApp thisApp = null;
        public static UIControlledApplication Application { get; set; } = null;

        public static IMyPythonEngine MyPythonEngine { get; set; }

        private MainRevitPage mainRevitPage;

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            #region Assign properties
            thisApp = this;
            Application = application;
            MyPythonEngine = new MyPythonEngine();
            // RevitApiVersion = application.ControlledApplication.VersionNumber;
            #endregion

            #region Create Panels
            RibbonPanel combinedCommandsPanel = application.CreateRibbonPanel("Commands");

            RibbonPanelMaker panelMaker = new RibbonPanelMaker(new CombinedCommandsPanel(), combinedCommandsPanel);
            panelMaker.BuildPanel();
            #endregion

            #region Register Dockable Panel: Parameter Editor
            DockablePaneProviderData data = new DockablePaneProviderData();
            //MainRevitPage mainDocableWindow = new MainRevitPage();
            mainRevitPage = new MainRevitPage();
            mainRevitPage.VM.RevitSelectionWatcher = new SelectionChangedWatcher(application);

            data.FrameworkElement = mainRevitPage
                as System.Windows.FrameworkElement;

            data.InitialState = new DockablePaneState();

            data.InitialState.DockPosition = DockPosition.Bottom;

            DockablePaneId dpid = new DockablePaneId(
                new Guid("{F1D5DCB2-DB78-483C-8A77-C7BD7CBC6557}"));

            application.RegisterDockablePane(
                dpid, "DKTools: Parameter Editor", mainRevitPage
                as IDockablePaneProvider);
            
            application.ViewActivated += new EventHandler
                <Autodesk.Revit.UI.Events.ViewActivatedEventArgs>(OnViewActivated);

            mainRevitPage.VM.RevitSelectionWatcher.SelectionChanged += 
                new EventHandler(mainRevitPage.VM.RevitActiveSelection_SelectionChanged);
            mainRevitPage.VM.RevitSelectionWatcher.SelectionChanged += new EventHandler(
                mainRevitPage.OverwriteContentInRichTextBox);

            #endregion

            return Result.Succeeded;
        }

        void OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            if (mainRevitPage.RevitDocument != e.Document)
            {
                mainRevitPage.RevitDocument = e.Document;
            }
        }
    }
}
