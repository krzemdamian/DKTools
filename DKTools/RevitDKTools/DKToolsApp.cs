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
using System.Windows;
using System.Resources;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace RevitDKTools
{
    public class DKToolsApp : IExternalApplication
    {
        public static UIControlledApplication UIControlledApplication { get; set; }
        public static IPythonExecutionEnviroment MyPythonEngine { get; set; }

        private DKToolsApp _DKToolsAppInstance;
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
            //CreateDynamicAssemblyAsProxyForPythonScripts();

            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<UIControlledApplication>().Instance(application));
            container.Register(Component.For<ICompositionRoot>().ImplementedBy<CompositionRoot>());
            //types for dynamic buttons generation
            container.Register(Component.For<ICommandsGenerator>().ImplementedBy<CommandsGenerator>());
            container.Register(Component.For<IClassEmitter>().
                               ImplementedBy<ClassEmitter<PythonCommandProxyBaseClass>>());
            container.Register(
                Component.For<IEmitterSetting,IXmlPythonScriptsSettingsProvider>().
                ImplementedBy<DefaultSettingsProvider>());
            container.Register(Component.For<ISettingsInterpreter>().ImplementedBy<XmlSettingsInterpreter>());
            container.Register(Component.For<RibbonPanel>().Instance(_commandsRibbonPanel).
                LifestyleSingleton());

            RibbonPanel panel = container.Resolve<RibbonPanel>();
            ICommandsGenerator generator = container.Resolve<ICommandsGenerator>();
            generator.GenerateDynamicCommands();

            return Result.Succeeded;
        }

        private void AssignProperties(UIControlledApplication application)
        {
            _DKToolsAppInstance = this;
            UIControlledApplication = application;
        }

        private static void InstantiatePythonEngine()
        {
            MyPythonEngine = new PythonExecutionEnviroment();
        }

        private void CreateDynamicAssemblyAsProxyForPythonScripts()
        {
            CommandsGenerator generator =
                new CommandsGenerator (
                    new ClassEmitter<PythonCommandProxyBaseClass>(new DefaultSettingsProvider()),
                    new XmlSettingsInterpreter(new DefaultSettingsProvider()),
                    _commandsRibbonPanel);
            generator.GenerateDynamicCommands();
        }

        private void PrepareParameterEditorDockablePanel(UIControlledApplication application)
        {
            CreateDockablePanel(application);
            application.ViewActivated += new EventHandler
                <Autodesk.Revit.UI.Events.ViewActivatedEventArgs>(PassRevitDocumentInstance_OnViewActivated);
            RegisterDocablePanelToRevit(application);
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

        private RibbonPanel CreateButtonsOnRevitRibbon(UIControlledApplication application)
        {
            _commandsRibbonPanel = application.CreateRibbonPanel("Commands");
            RibbonPanelButtonMaker ribbonPanelButtonMaker =
                new RibbonPanelButtonMaker (new CombinedCommandsPanel(), _commandsRibbonPanel);
            ribbonPanelButtonMaker.BuildButtons();
            return _commandsRibbonPanel;
        }
    }
}
