using Autodesk.Revit.UI;
using RevitDKTools.Command.Receiver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace RevitDKTools.Command.ButtonData
{
    class DynamicButtonGenerator
    {
        private readonly XmlDocument _xml;
        private readonly string _assemblyName;
        private readonly DynamicCommandClassEmiter _emiter;
        private List<Dictionary<string,string>> _commands;
        private readonly RibbonPanel _panel;

        public DynamicButtonGenerator(XmlDocument xml, RibbonPanel panel, string assemblyName = "DynamicProxy")
        {
            _xml = xml;
            _panel = panel;
            _assemblyName = assemblyName;
            _emiter = new DynamicCommandClassEmiter(assemblyName);
            _commands = new List<Dictionary<string,string>>();
        }
        /// <summary>
        /// Method reads about Python Scripts from XML file.
        /// </summary>
        /// <returns></returns>
        public void ReadXML()
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
                    commandName = Regex.Replace(commandName, @"\s+", "");
                    Dictionary<string, string> command = new Dictionary<string, string>()
                    {
                        {"CommandName", commandName },
                        {"NameOnRibbon", nameOnRibbon },
                        {"ScriptPath", scriptPath},
                        {"ParentButton", parentButton }
                    };
                    if (!string.IsNullOrEmpty(toolTip)) { command.Add("ToolTip", toolTip); }
                    if (!string.IsNullOrEmpty(imageUri)) { command.Add("ImageUri", imageUri); }
                    _commands.Add(command);
                }
                commandName = string.Empty;
                nameOnRibbon = string.Empty;
                scriptPath = string.Empty;
                parentButton = string.Empty;
                toolTip = string.Empty;
                imageUri = string.Empty;

            }
        }

        public void CreateDynamicAssembly()
        {
            foreach (Dictionary<string, string> command in _commands)
            {
                _emiter.BuildCommandType(command["CommandName"], command["ScriptPath"]);
            }

            _emiter.SaveAssembly();
        }

        public void CreateButtons()
        {
            var items = _panel.GetItems();
            Dictionary<string, PulldownButton> pulldowns = new Dictionary<string, PulldownButton>();
            foreach (var item in items)
            {
                if (item.ItemType == RibbonItemType.PulldownButton)
                {
                    pulldowns[item.Name] = (PulldownButton)item;
                }
            }
            foreach(Dictionary<string, string> cmd in _commands)
            {
                PushButtonData pbd = new PushButtonData(
                    cmd["CommandName"], cmd["NameOnRibbon"], _emiter.AssemblyLocation, cmd["CommandName"]);
                AddImage(cmd, pbd);
                AddToolTip(cmd, pbd);
                pulldowns[cmd["ParentButton"]].AddPushButton(pbd);
            }
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
                    if (bitmapImage.PixelHeight == 32 && bitmapImage.PixelWidth == 32 )
                    {
                        pbd.LargeImage = bitmapImage;
                    }
                    else if (bitmapImage.PixelHeight == 16 && bitmapImage.PixelWidth == 16)
                    {
                        pbd.Image = bitmapImage;
                    }
                }
            }
        }
    }
}
