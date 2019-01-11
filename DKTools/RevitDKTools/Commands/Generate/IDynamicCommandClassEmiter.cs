using System;

namespace RevitDKTools.Commands.Generate
{
    interface IDynamicCommandClassEmiter
    {
        string AssemblyLocation { get; }

        Type BuildCommandType(string commandTypeName, string scriptPath);
        void SaveAssembly();
    }
}