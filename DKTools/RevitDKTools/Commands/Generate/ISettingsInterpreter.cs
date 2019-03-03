using System.Collections.Generic;

namespace RevitDKTools.Commands.Generate
{
    interface ISettingsInterpreter
    {
        ICollection<PythonCommandSetting> PythonCommandSettings { get; }
        ICollection<VisibilitySwitcherCommandSetting> VisibilitySwitcherCommandSettings { get; }
    }
}