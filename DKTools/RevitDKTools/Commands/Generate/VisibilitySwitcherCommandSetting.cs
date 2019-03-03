using System;
using System.Text.RegularExpressions;

namespace RevitDKTools.Commands.Generate
{
    internal class VisibilitySwitcherCommandSetting
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

        public string VisibilityNameRegex { get; set; }

        internal bool HasRequiredItems()
        {
            if (string.IsNullOrEmpty(CommandName) ||
                string.IsNullOrEmpty(VisibilityNameRegex))
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