using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Xml;

namespace RevitDKTools.Commands.Generate
{
    class DynamicButtonGenerator
    {
        private readonly XmlDocument _xml;
        private readonly string _assemblyName;
        private readonly DynamicCommandClassEmiter<DynamicCommandBase> _emiter;
        private List<Dictionary<string,string>> _commandDescriptionsList;
        private readonly RibbonPanel _ribbonPanel;

        public DynamicButtonGenerator(XmlDocument xml, RibbonPanel panel, string assemblyName = "DynamicProxy")
        {
            _xml = xml;
            _ribbonPanel = panel;
            _assemblyName = assemblyName;
            _emiter = new DynamicCommandClassEmiter<DynamicCommandBase>(assemblyName);
            _commandDescriptionsList = new List<Dictionary<string,string>>();
        }

        public void GenerateDynamicButtons()
        {
            xmlToCommandDescriptionList();
            EmitProxyTypesToDynamicAssembly();

            Dictionary<string, PulldownButton> pulldownButtons = GetDictionaryWithPulldownButtons();

            foreach (Dictionary<string, string> commandDescription in _commandDescriptionsList)
            {
                PushButtonData dynamicCommandPushButton = CreatePushButtonDataFromDescription(commandDescription);

                PulldownButton parentPulldownButton = pulldownButtons[commandDescription["ParentButton"]];
                parentPulldownButton.AddPushButton(dynamicCommandPushButton);
            }
        }

        private void xmlToCommandDescriptionList()
        {
            string check = string.Empty;
            foreach(XmlNode node in _xml.DocumentElement)
            {
                string commandName = string.Empty;
                string nameOnRibbon = string.Empty;
                string scriptPath = string.Empty;
                string parentButton = string.Empty;
                string toolTip = string.Empty;
                string imageUri = string.Empty;

                foreach (XmlNode child in node)
                {
                    switch (child.Name)
                    {
                        case "CommandName":
                            commandName = child.InnerText;
                            break;
                        case "NameOnRibbon":
                            nameOnRibbon = child.InnerText;
                            break;
                        case "ScriptPath":
                            scriptPath = child.InnerText;
                            break;
                        case "ParentButton":
                            parentButton = child.InnerText;
                            break;
                        case "ToolTip":
                            toolTip = child.InnerText;
                            break;
                        case "ImageUri":
                            imageUri = child.InnerText;
                            break;
                    }
                }
                
                if(!string.IsNullOrEmpty(commandName) &&
                    !string.IsNullOrEmpty(nameOnRibbon) &&
                    !string.IsNullOrEmpty(scriptPath) &&
                    !string.IsNullOrEmpty(parentButton))
                {
                    commandName = Regex.Replace(commandName, @"\s+", "");  // remove white spaces

                    Dictionary<string, string> command = new Dictionary<string, string>()
                    {
                        {"CommandName", commandName },
                        {"NameOnRibbon", nameOnRibbon },
                        {"ScriptPath", scriptPath},
                        {"ParentButton", parentButton }
                    };

                    if (!string.IsNullOrEmpty(toolTip)) { command.Add("ToolTip", toolTip); }
                    if (!string.IsNullOrEmpty(imageUri)) { command.Add("ImageUri", imageUri); }

                    _commandDescriptionsList.Add(command);
                }

                commandName = string.Empty;
                nameOnRibbon = string.Empty;
                scriptPath = string.Empty;
                parentButton = string.Empty;
                toolTip = string.Empty;
                imageUri = string.Empty;
            }
        }

        private void EmitProxyTypesToDynamicAssembly()
        {
            string pathPrefix = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) + "\\PythonScripts\\";

            foreach (Dictionary<string, string> command in _commandDescriptionsList)
            {
                _emiter.BuildCommandType(command["CommandName"], pathPrefix + command["ScriptPath"]);
            }

            _emiter.SaveAssembly();
        }

        private PushButtonData CreatePushButtonDataFromDescription(Dictionary<string, string> commandDescription)
        {
            PushButtonData pushButton = new PushButtonData(
                                commandDescription["CommandName"], commandDescription["NameOnRibbon"],
                                _emiter.AssemblyLocation, commandDescription["CommandName"]);
            AddImage(commandDescription, pushButton);
            AddToolTip(commandDescription, pushButton);
            return pushButton;
        }

        private Dictionary<string, PulldownButton> GetDictionaryWithPulldownButtons()
        {
            IList<RibbonItem> itemsOnRibbon = _ribbonPanel.GetItems();
            Dictionary<string, PulldownButton> pulldownButtons = new Dictionary<string, PulldownButton>();
            foreach (var ribbonItem in itemsOnRibbon)
            {
                if (ribbonItem.ItemType == RibbonItemType.PulldownButton)
                {
                    pulldownButtons[ribbonItem.Name] = (PulldownButton)ribbonItem;
                }
            }

            return pulldownButtons;
        }

        private void AddToolTip(Dictionary<string, string> cmd, PushButtonData pbd)
        {
            if (cmd.ContainsKey("ToolTip"))
            {
                pbd.ToolTip = cmd["ToolTip"];
            }
        }

        private void AddImage(Dictionary<string, string> cmd, PushButtonData pbd)
        {
            if (cmd.ContainsKey("ImageUri"))
            {
                if(Path.GetExtension(cmd["ImageUri"]) == ".png")
                {
                    string imagePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                        "\\PythonScripts\\" + cmd["ImageUri"];
                    imagePath = Path.GetFullPath(imagePath);
                    BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
                    if (bitmapImage.PixelHeight == 16 && bitmapImage.PixelWidth == 16 )
                    {
                        pbd.Image = bitmapImage;
                    }
                    pbd.LargeImage = bitmapImage;
                    
                }
            }
        }
    }
}
