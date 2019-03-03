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
            CreatePythonCommandButtonsOnRibbon();
            CreateVisibilitySwitcherCommandsOnRibbonAsSlideOut();
        }

        private void CreatePythonCommandButtonsOnRibbon()
        {
            Dictionary<string, PulldownButton> pulldownButtons = GetDictionaryWithPulldownButtons();

            foreach (var commandSetting in _settingsInterpreter.PythonCommandSettings)
            {
                PushButtonData dynamicCommandPushButton =
                    CreatePythonPushButtonDataFromSetting(commandSetting);

                PulldownButton parentPulldownButton = pulldownButtons[commandSetting.ParentButton];
                parentPulldownButton.AddPushButton(dynamicCommandPushButton);
            }
        }

        private void CreateVisibilitySwitcherCommandsOnRibbonAsSlideOut()
        {
            _ribbonPanel.AddSlideOut();
            PulldownButtonData pdbd = 
                new PulldownButtonData("VisibiitySwitcher", "VisibiitySwitcher");
            PulldownButton pulldownButton = _ribbonPanel.AddItem(pdbd) as PulldownButton;
            foreach (var commandSetting in _settingsInterpreter.VisibilitySwitcherCommandSettings)
            {
                PushButtonData dynamicCommandPushButton =
                    CreateVisibilitySwitcherPushButtonDataFromSetting(commandSetting);
                dynamicCommandPushButton.ToolTip = commandSetting.VisibilityNameRegex;

                pulldownButton.AddPushButton(dynamicCommandPushButton);
            }
        }

        private void EmitProxyClassesToDynamicAssembly()
        {
            ResourceManager resourceManager = new ResourceManager(
                "RevitDKTools.Properties.Resources",
                Assembly.GetExecutingAssembly());
            string scriptFolderLocation = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\",string.Empty) +
                resourceManager.GetString("SCRIPTS_FOLDER_LOCATION");

            foreach (PythonCommandSetting setting in _settingsInterpreter.PythonCommandSettings)
            {
                _emitter.BuildPythonCommandType<PythonCommandProxyBaseClass>(
                    setting.CommandName, scriptFolderLocation + setting.ScriptRelativePath);
            }

            foreach (VisibilitySwitcherCommandSetting setting
                in _settingsInterpreter.VisibilitySwitcherCommandSettings)
            {
                _emitter.BuildVisibilityShortcutCommand<VisibilitySwitcherBaseClass>(
                    setting.CommandName,setting.VisibilityNameRegex);
            }

            _emitter.SaveAssembly();
        }

        private PushButtonData CreatePythonPushButtonDataFromSetting
            (PythonCommandSetting commandSetting)
        {
            PushButtonData pushButton = new PushButtonData(
                commandSetting.CommandName, commandSetting.NameOnRibbon,
                _emitter.AssemblyLocation, commandSetting.CommandName);
            if (!string.IsNullOrEmpty(commandSetting.ImageUri)) { AddPngImage(commandSetting, pushButton); }
            AddToolTip(commandSetting, pushButton);
            return pushButton;
        }

        private PushButtonData CreateVisibilitySwitcherPushButtonDataFromSetting(
            VisibilitySwitcherCommandSetting commandSetting)
        {
            PushButtonData pushButton = new PushButtonData(
                commandSetting.CommandName, commandSetting.CommandName,
                _emitter.AssemblyLocation, commandSetting.CommandName);
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

        private void AddToolTip(PythonCommandSetting commandSetting, PushButtonData pushButtonData)
        {
            if (!string.IsNullOrEmpty(commandSetting.ToolTip))
            {
                pushButtonData.ToolTip = commandSetting.ToolTip;
            }
        }

        private void AddPngImage(PythonCommandSetting commandSetting, PushButtonData pbd)
        {
            if (Path.GetExtension(commandSetting.ImageUri) == ".png")
            {
                ResourceManager resourceManager = new ResourceManager(
                    "RevitDKTools.Properties.Resources",
                    Assembly.GetExecutingAssembly());
                string imagePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\",string.Empty) +
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
