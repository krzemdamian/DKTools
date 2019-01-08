using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitDKTools.Command.ButtonData;

namespace RevitDKTools.Panels
{
    public class GenericPanel : RibbonPanelButtonBuilder
    {
        public override void BuildPanelButtons()
        {
            PushButtonMaker buttonMaker = new PushButtonMaker(new Info());
            buttonMaker.BuildPushButtonData();
            PushButtonData InfoButtonData = buttonMaker.GetPushButtonData();

            buttonMaker = new PushButtonMaker(new ShowParameterEditor());
            buttonMaker.BuildPushButtonData();
            PushButtonData ShowParameterEditorButtonData = buttonMaker.GetPushButtonData();

            PushButton newButton = Panel.AddItem(InfoButtonData) as PushButton;
            newButton = Panel.AddItem(ShowParameterEditorButtonData) as PushButton;
        }
    }
}
