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
        private ICollection<ScriptCommandSetting> _scriptCommandSettings;

        public ICollection<ScriptCommandSetting> ScriptCommandSettings { get { return _scriptCommandSettings; } }

        public SettingsInterpreter(XmlDocument xml)
        {
            CreateSettingsFromXml(xml);
        }

        private void CreateSettingsFromXml(XmlDocument xml)
        {
            foreach (XmlNode node in xml.DocumentElement)
            {
                ScriptCommandSetting scriptCommandSetting = GetSettingFromXmlNode(node);
                if (scriptCommandSetting.HasRequiredItems())
                {
                    _scriptCommandSettings.Add(scriptCommandSetting);
                }
            }
        }

        private ScriptCommandSetting GetSettingFromXmlNode(XmlNode node)
        {
            ScriptCommandSetting scriptCommandSetting = new ScriptCommandSetting();

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
