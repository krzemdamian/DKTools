using Autodesk.Revit.UI;
using RevitDKTools.Commands.Generate;

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
