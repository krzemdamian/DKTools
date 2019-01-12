using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace RevitDKTools.Commands.Generate
{
    class SettingsInterpreter
    {
        private ICollection<CommandSetting> _scriptCommandSettings;

        public ICollection<CommandSetting> ScriptCommandSettings { get { return _scriptCommandSettings; } }

        public SettingsInterpreter(XmlDocument xml)
        {
            NewMethod(xml);
        }

        private void NewMethod(XmlDocument xml)
        {
            foreach (XmlNode node in xml.DocumentElement)
            {
                CommandSetting scriptCommandSetting = GetSettingFromXmlNode(node);
                if (scriptCommandSetting.HasRequiredItems())
                {
                    _scriptCommandSettings.Add(scriptCommandSetting);
                }
            }
        }

        private CommandSetting GetSettingFromXmlNode(XmlNode node)
        {
            CommandSetting scriptCommandSetting = new CommandSetting();

            foreach (XmlNode child in node)
            {
                switch (child.Name)
                {
                    case "CommandName":
                        scriptCommandSetting.CommandName = child.InnerText;
                        break;
                    case "NameOnRibbon":
                        scriptCommandSetting.NameOnRibbon = child.InnerText;
                        break;
                    case "ScriptPath":
                        scriptCommandSetting.ScriptPath = child.InnerText;
                        break;
                    case "ParentButton":
                        scriptCommandSetting.ParentButton = child.InnerText;
                        break;
                    case "ToolTip":
                        scriptCommandSetting.ToolTip = child.InnerText;
                        break;
                    case "ImageUri":
                        scriptCommandSetting.ImageUri = child.InnerText;
                        break;
                }
            }

            return scriptCommandSetting;
        }
    }
}
