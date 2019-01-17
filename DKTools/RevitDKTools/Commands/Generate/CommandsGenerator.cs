using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Resources;

namespace RevitDKTools.Commands.Generate
{
    class CommandsGenerator : ICommandsGenerator
    {
        private readonly IClassEmitter _emitter;
        private readonly ISettingsInterpreter _settingsInterpreter;
        private readonly RibbonPanel _ribbonPanel;
        private readonly ICollection<PushButtonData> _generatedButtons;

        public CommandsGenerator(
            IClassEmitter emitter,
            ISettingsInterpreter settingsInterpreter,
            RibbonPanel ribbonPanel)
        {
            _emitter = emitter;
            _settingsInterpreter = settingsInterpreter;
            _ribbonPanel = ribbonPanel;
        }

        public void GenerateDynamicCommands()    
        {
            EmitProxyClassesToDynamicAssembly();

            Dictionary<string, PulldownButton> pulldownButtons = GetDictionaryWithPulldownButtons();

            foreach (var commandSetting  in _settingsInterpreter.ScriptCommandSettings)
            {
                PushButtonData dynamicCommandPushButton = 
                    CreatePushButtonDataFromSetting(commandSetting);

                PulldownButton parentPulldownButton = pulldownButtons[commandSetting.ParentButton];
                parentPulldownButton.AddPushButton(dynamicCommandPushButton);
            }
        }

        private void EmitProxyClassesToDynamicAssembly()
        {
            ResourceManager resourceManager = new ResourceManager(
                "RevitDKTools.Properties.Resources",
                Assembly.GetExecutingAssembly());
            string scriptFolderLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                resourceManager.GetString("SCRIPTS_FOLDER_LOCATION");

            foreach (CommandSetting setting in _settingsInterpreter.ScriptCommandSettings)
            {
                _emitter.BuildCommandType(
                    setting.CommandName, scriptFolderLocation + setting.ScriptRelativePath);
            }

            _emitter.SaveAssembly();
        }

        private PushButtonData CreatePushButtonDataFromSetting(CommandSetting commandSetting)
        {
            PushButtonData pushButton = new PushButtonData(
                commandSetting.CommandName, commandSetting.NameOnRibbon,
                _emitter.AssemblyLocation, commandSetting.CommandName);
            if (!string.IsNullOrEmpty(commandSetting.ImageUri)) { AddPngImage(commandSetting, pushButton); } 
            AddToolTip(commandSetting, pushButton);
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

        private void AddToolTip(CommandSetting commandSetting, PushButtonData pushButtonData)
        {
            if (!string.IsNullOrEmpty(commandSetting.ToolTip))
            {
                pushButtonData.ToolTip = commandSetting.ToolTip;
            }
        }

        private void AddPngImage(CommandSetting commandSetting, PushButtonData pbd)
        {
            if (Path.GetExtension(commandSetting.ImageUri) == ".png")
            {
                ResourceManager resourceManager = new ResourceManager(
                    "RevitDKTools.Properties.Resources",
                    Assembly.GetExecutingAssembly());
                string imagePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                     resourceManager.GetString("SCRIPTS_FOLDER_LOCATION") + commandSetting.ImageUri;
                imagePath = Path.GetFullPath(imagePath);
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
                pbd.LargeImage = bitmapImage;

                if (bitmapImage.PixelHeight == 16 && bitmapImage.PixelWidth == 16)
                {
                    pbd.Image = bitmapImage;
                }
            }
        }
    }
}
