using System;

namespace RevitDKTools.Commands.Generate
{
    interface IClassEmitter
    {
        string AssemblyLocation { get; }

        Type BuildCommandType(string commandTypeName, string scriptPath);
        void SaveAssembly();
    }
}