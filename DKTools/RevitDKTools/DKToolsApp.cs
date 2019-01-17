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
        public static IPythonExecutionEnvironment MyPythonEngine { get; set; }

        private ComponentRegistrator _registrator;

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            _registrator = new ComponentRegistrator(application);
            _registrator.Container.Resolve<ICompositionRoot>();

            // Its more complicated to get rid of PythonEngine.
            // Base class for emitter has strong reference static property of DKToolsApp.
            MyPythonEngine = _registrator.Container.Resolve<IPythonExecutionEnvironment>();

            return Result.Succeeded;
        }
   }
}
