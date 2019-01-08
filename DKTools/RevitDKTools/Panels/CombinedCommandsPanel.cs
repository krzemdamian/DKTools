using Autodesk.Revit.UI;
using RevitDKTools.Command.ButtonData;
using RevitDKTools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Panels
{
    class CombinedCommandsPanel : RibbonPanelButtonBuilder
    {
        public override void BuildPanelButtons()
        {
            PushButtonMaker buttonMaker = new PushButtonMaker(new Info());
            buttonMaker.BuildPushButtonData();
            PushButtonData buttonData = buttonMaker.GetPushButtonData();
            PushButton button = Panel.AddItem(buttonData) as PushButton;

            #region Visual
            PulldownButtonData visualPulldownButtonData = new PulldownButtonData("Visual","Visual");
            visualPulldownButtonData.LargeImage =
                AssemblyResourceUtils.GetImageFromResource("Command/Images/Large/blue/visibility.png");
            PulldownButton visualPulldownButton = Panel.AddItem(visualPulldownButtonData) as PulldownButton;


            #endregion

            PulldownButtonData selectionPulldownButtonData = new PulldownButtonData("Selection", "Selection");
            selectionPulldownButtonData.LargeImage =
                 AssemblyResourceUtils.GetImageFromResource("Command/Images/Large/blue/hands-and-gestures.png");
            PulldownButton selectionPulldownButton = Panel.AddItem(selectionPulldownButtonData) as PulldownButton;

            #region Parameters
            PulldownButtonData parametersPulldownButtonData = new PulldownButtonData("Parameters", "Parameters");
            parametersPulldownButtonData.LargeImage =
                 AssemblyResourceUtils.GetImageFromResource("Command/Images/Large/blue/controls.png");
            PulldownButton parametersPulldownButton = Panel.AddItem(parametersPulldownButtonData) as PulldownButton;
            
            buttonMaker = new PushButtonMaker(new ShowParameterEditor());
            buttonMaker.BuildPushButtonData();
            buttonData = buttonMaker.GetPushButtonData();
            button = parametersPulldownButton.AddPushButton(buttonData) as PushButton;

            #endregion

            PulldownButtonData viewsPulldownButtonData = new PulldownButtonData("Views", "Views");
            viewsPulldownButtonData.LargeImage =
                 AssemblyResourceUtils.GetImageFromResource("Command/Images/Large/blue/crop.png");
            PulldownButton viewsPulldownButton = Panel.AddItem(viewsPulldownButtonData) as PulldownButton;


            PulldownButtonData worksharingPulldownButtonData = new PulldownButtonData("Worksharing", "Worksharing");
            worksharingPulldownButtonData.LargeImage =
                 AssemblyResourceUtils.GetImageFromResource("Command/Images/Large/blue/network.png");
            PulldownButton worksharingPulldownButton = Panel.AddItem(worksharingPulldownButtonData) as PulldownButton;


            PulldownButtonData analyzePulldownButtonData = new PulldownButtonData("Analyze", "Analyze");
            analyzePulldownButtonData.LargeImage =
                 AssemblyResourceUtils.GetImageFromResource("Command/Images/Large/blue/analytics.png");
            PulldownButton analyzePulldownButton = Panel.AddItem(analyzePulldownButtonData) as PulldownButton;

        }
    }
}
