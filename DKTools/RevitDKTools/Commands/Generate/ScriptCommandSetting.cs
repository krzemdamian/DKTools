using System;
using System.Text.RegularExpressions;

namespace RevitDKTools.Commands.Generate
{
    internal class ScriptCommandSetting
    {
        private string _commandName;

        public string CommandName
        {
            get { return _commandName; }
            set
            {
                _commandName = Regex.Replace(value, @"\s+", ""); // remove white spaces
            }
        }
        public string NameOnRibbon { get; set; }
        public string ScriptPath { get; set; }
        public string ParentButton { get; set; }
        public string ToolTip { get; set; }
        public string ImageUri { get; set; }

        internal bool HasRequiredItems()
        {
            if (string.IsNullOrEmpty(CommandName) ||
               string.IsNullOrEmpty(NameOnRibbon) ||
               string.IsNullOrEmpty(ScriptPath) ||
               string.IsNullOrEmpty(ParentButton))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}