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

        private RibbonPanel _commandsRibbonPanel;
        private WindsorContainer _container;

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            UIControlledApplication = application;
            MyPythonEngine = new PythonExecutionEnviroment();

            RegisterTypesToDIContainer(application);

            return Result.Succeeded;
        }

        private void RegisterTypesToDIContainer(UIControlledApplication application)
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<UIControlledApplication>().Instance(application));
            _container.Register(Component.For<ICompositionRoot>().ImplementedBy<CompositionRoot>());
            //types for dynamic buttons generation
            _container.Register(Component.For<ICommandsGenerator>().ImplementedBy<CommandsGenerator>());
            _container.Register(Component.For<IClassEmitter>().
                               ImplementedBy<ClassEmitter<PythonCommandProxyBaseClass>>());
            _container.Register(
                Component.For<IEmitterSetting, IXmlPythonScriptsSettingsProvider>().
                ImplementedBy<DefaultSettingsProvider>());
            _container.Register(Component.For<ISettingsInterpreter>().ImplementedBy<XmlSettingsInterpreter>());
            _container.Register(Component.For<RibbonPanel>().Instance(_commandsRibbonPanel).
                LifestyleSingleton());
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
