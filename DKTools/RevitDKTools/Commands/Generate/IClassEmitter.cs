using Autodesk.Revit.UI;
using System;

namespace RevitDKTools.Commands.Generate
{
    interface IClassEmitter
    {
        string AssemblyLocation { get; }

        Type BuildPythonCommandType<T>(string commandTypeName, string scriptPath)
            where T : IExternalCommand;
        Type BuildVisibilityShortcutCommand<T>(string commandName, string visibilityRegex)
            where T : IExternalCommand;
        void SaveAssembly();
    }
}