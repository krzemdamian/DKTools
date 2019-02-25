using Autodesk.Revit.UI;
using System;

namespace RevitDKTools.Commands.Generate
{
    interface IClassEmitter
    {
        string AssemblyLocation { get; }

        Type BuildCommandType<T>(string commandTypeName, string scriptPath)
            where T : IExternalCommand;
        void SaveAssembly();
    }
}