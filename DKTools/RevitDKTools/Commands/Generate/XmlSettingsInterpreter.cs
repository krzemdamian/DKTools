using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using RevitDKTools.Common;


namespace RevitDKTools.Commands.Generate
{
    class XmlSettingsInterpreter : ISettingsInterpreter
    {
        private ICollection<PythonCommandSetting> _pythonCommandSettings;
        private ICollection<VisibilitySwitcherCommandSetting> _visibilitySwitcherCommandSettings;

        public ICollection<PythonCommandSetting> PythonCommandSettings
        {
            get { return _pythonCommandSettings; }
        }

        public ICollection<VisibilitySwitcherCommandSetting> VisibilitySwitcherCommandSettings
        {
            get { return _visibilitySwitcherCommandSettings; }
        }

        public XmlSettingsInterpreter(IXmlPythonScriptsSettingsProvider pythonSettingsProvider
            ,IXmlVisibilitySwitcherSettingsProvider switcherSettingsProvider)
        {
            _pythonCommandSettings = new List<PythonCommandSetting>();
            XmlDocument pythonXml = pythonSettingsProvider.Xml;
            AssignPythonCommandSettingsToCollection(pythonXml);

            _visibilitySwitcherCommandSettings = new List<VisibilitySwitcherCommandSetting>();
            XDocument switcherXml = switcherSettingsProvider.XDoc;
            AssignSwitcherCommandSettingsToCollection(switcherXml);
        }

        private void AssignPythonCommandSettingsToCollection(XmlDocument xml)
        {
            foreach (XmlNode node in xml.DocumentElement)
            {
                PythonCommandSetting scriptCommandSetting = GetSettingFromXmlNode(node);
                if (scriptCommandSetting.HasRequiredItems())
                {
                    _pythonCommandSettings.Add(scriptCommandSetting);
                }
            }
        }

        private void AssignSwitcherCommandSettingsToCollection(XDocument xdoc)
        {
            _visibilitySwitcherCommandSettings = xdoc.Root.Elements()
                .Where(e => e.Name.LocalName == "VisibilitySwitcher")
                .Where(e => e.Attribute(XName.Get("CommandName")) != null)
                .Where(e => e.Attribute(XName.Get("VisibilityNameRegex")) != null)
                .Select(e =>
                {
                    VisibilitySwitcherCommandSetting v = new VisibilitySwitcherCommandSetting();
                    v.CommandName = e.Attribute(XName.Get("CommandName")).Value;
                    v.VisibilityNameRegex = e.Attribute(XName.Get("VisibilityNameRegex")).Value;
                    return v;
                })
                .DistinctBy(n => n.CommandName)
                .ToList();
        }

        private PythonCommandSetting GetSettingFromXmlNode(XmlNode node)
        {
            PythonCommandSetting scriptCommandSetting = new PythonCommandSetting();

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
                        scriptCommandSetting.ScriptRelativePath = child.InnerText;
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
