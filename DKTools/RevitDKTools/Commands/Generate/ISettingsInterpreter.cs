using System.Collections.Generic;

namespace RevitDKTools.Commands.Generate
{
    interface ISettingsInterpreter
    {
        ICollection<CommandSetting> ScriptCommandSettings { get; }
    }
}