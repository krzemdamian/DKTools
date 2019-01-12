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
    class PythonCommandsGenerator
    {
        private readonly XmlDocument _xml;
        private readonly string _assemblyName;
        private readonly ClassEmitter<IExternalCommand> _emitter;
        private List<Dictionary<string,string>> _commandDescriptionsList;
        private readonly RibbonPanel _ribbonPanel;

        public PythonCommandsGenerator(ClassEmitter<IExternalCommand> emitter, XmlDocument xml, RibbonPanel panel, string assemblyName = "DynamicProxy")
        {
            _xml = xml;
            _ribbonPanel = panel;
            _assemblyName = assemblyName;
            _emitter = emitter;
            _commandDescriptionsList = new List<Dictionary<string,string>>();
        }

        public void GenerateDynamicButtons()
        {
            XmlToCommandDescriptionList();
            EmitProxyTypesToDynamicAssembly();

            Dictionary<string, PulldownButton> pulldownButtons = GetDictionaryWithPulldownButtons();

            foreach (Dictionary<string, string> commandDescription in _commandDescriptionsList)
            {
                PushButtonData dynamicCommandPushButton = CreatePushButtonDataFromDescription(commandDescription);

                PulldownButton parentPulldownButton = pulldownButtons[commandDescription["ParentButton"]];
                parentPulldownButton.AddPushButton(dynamicCommandPushButton);
            }
        }

        private void XmlToCommandDescriptionList()
        {
            
        }

        private void EmitProxyTypesToDynamicAssembly()
        {
            string pathPrefix = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) + "\\PythonScripts\\";

            foreach (Dictionary<string, string> command in _commandDescriptionsList)
            {
                _emitter.BuildCommandType(command["CommandName"], pathPrefix + command["ScriptPath"]);
            }

            _emitter.SaveAssembly();
        }

        private PushButtonData CreatePushButtonDataFromDescription(Dictionary<string, string> commandDescription)
        {
            PushButtonData pushButton = new PushButtonData(
                                commandDescription["CommandName"], commandDescription["NameOnRibbon"],
                                _emitter.AssemblyLocation, commandDescription["CommandName"]);
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
