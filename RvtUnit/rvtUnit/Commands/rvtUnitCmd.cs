#region Copyright (c) 2009-2011 Arup, All rights reserved.
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion
#region File Information
//
// Filename: rvtUnitCmd.cs
// Author: Yamin Tengono <Yamin.Tengono@arup.com.au>
//
// This file is part of CADtools Revit - rvtUnit module.
//
#endregion

// !!! This file is applicable for all versions of Revit. !!! //

#if RVT2010
using Autodesk.Revit;
#else
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
#endif

using System;
using System.Reflection;
using Helpers;
using NUnit.Core;
using System.IO;
using NUnit.Util;
using rvtUnit.Controls;
using System.Windows.Interop;
using rvtUnit.Helpers;

namespace rvtUnit.Commands
{

   /// <summary>
   /// Revit external command to do integration.
   /// </summary>
#if RVT2011
   [Regeneration(RegenerationOption.Manual)]
#endif
#if !RVT2010
   [Transaction(TransactionMode.Manual)]
#endif
   public class rvtUnitCmd : IExternalCommand
   {
      // ======================================================== FIELDS === //

      private Document _activeDoc;
      public ExternalCommandData ExternalCommandData { get; set; }

      // ==================================================== PROPERTIES === //

#if RVT2010

      /// <summary>
      /// Revit external command entry point.
      /// </summary>
      /// <param name="commandData">A ExternalCommandData object which contains reference to Application and View needed by external command.</param>
      /// <param name="message">Error message can be returned by external command.</param>
      /// <param name="elements">Element set could be used for transferring elements between external command and Autodesk Revit.</param>
      /// <returns>
      /// Result tells whether the excution fail, succeed or was canceled by user. If not succeed, Autodesk Revit should undo any changes made by the external command. 
      /// </returns>
      public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         _activeDoc = commandData.Application.ActiveDocument;

         DoAction();

         // Return Cancelled so that this command is not recorded as modifying Document:
         return IExternalCommand.Result.Cancelled;
      }

#else

      /// <summary>
      /// Revit external command entry point.
      /// </summary>
      /// <param name="commandData">A ExternalCommandData object which contains reference to Application and View needed by external command.</param>
      /// <param name="message">Error message can be returned by external command.</param>
      /// <param name="elements">Element set could be used for transferring elements between external command and Autodesk Revit.</param>
      /// <returns>
      /// Result tells whether the excution fail, succeed or was canceled by user. If not succeed, Autodesk Revit should undo any changes made by the external command. 
      /// </returns>
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         _activeDoc = commandData.Application.ActiveUIDocument.Document;


		 GeneralHelper.ActiveUIDocument = commandData.Application.ActiveUIDocument;
            GeneralHelper.ExternalCommandData = commandData;

         DoAction();

         // Return Cancelled so that this command is not recorded as modifying Document:
         return Result.Cancelled;
      }

#endif

      private void DoAction()
      {
          MainWindowViewModel vm = new MainWindowViewModel();
          MainWindow view = new MainWindow(vm);
		  GeneralHelper.SetRevitAsWindowOwner(view);
          view.ShowDialog();
      }

   }  // End of class rvtUnitCmd

}  // End of namespace rvtUnit.Commands
