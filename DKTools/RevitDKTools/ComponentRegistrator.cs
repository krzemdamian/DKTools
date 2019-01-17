using Autodesk.Revit.UI;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using RevitDKTools.Commands.Generate;
using RevitDKTools.Commands.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools
{
    class ComponentRegistrator
    {
        private UIControlledApplication _application;
        private WindsorContainer _container;
        private RibbonPanel _commandsRibbonPanel;

        public WindsorContainer Container { get { return _container; } }

        public ComponentRegistrator(UIControlledApplication application)
        {
            _application = application;
            CreateButtonsOnRevitRibbon();
            RegisterTypesToDIContainer();
        }

        private void RegisterTypesToDIContainer()
        {

            _container = new WindsorContainer();
            _container.Register(Component.For<UIControlledApplication>().Instance(_application));
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
            _container.Register(Component.For<IPythonExecutionEnvironment>().
                ImplementedBy<PythonExecutionEnvironment>());
        }

        private void CreateButtonsOnRevitRibbon()
        {
            _commandsRibbonPanel = _application.CreateRibbonPanel("Commands");
            RibbonPanelButtonMaker ribbonPanelButtonMaker =
                new RibbonPanelButtonMaker (new CombinedCommandsPanel(), _commandsRibbonPanel);
            ribbonPanelButtonMaker.BuildButtons();
        }
    }
}
