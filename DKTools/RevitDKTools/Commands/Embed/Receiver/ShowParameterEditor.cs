using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace RevitDKTools.Commands.Embed.Receiver
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ShowParameterEditor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DockablePaneId dpid = new DockablePaneId(
                new Guid("{F1D5DCB2-DB78-483C-8A77-C7BD7CBC6557}"));

            DockablePane dp = commandData.Application.GetDockablePane(dpid);

            dp.Show();

            return Result.Succeeded;
        }
    }
}
