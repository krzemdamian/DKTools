#region Copyright (c) 2009-2011 Arup, All rights reserved.
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion
#region File Information
//
// Filename: TestItemViewModel.cs
// Author: Yamin Tengono <Yamin.Tengono@arup.com.au>
//
// This file is part of CADtools Revit - rvtUnit module.
//
#endregion

// !!! This file is applicable for all versions of Revit. !!! //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using NUnit.Core;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using rvtUnit.Helpers;

namespace rvtUnit.Controls
{

   /// <summary>
   /// Provide data link for TestItem user interface.
   /// </summary>
   public class TestItemViewModel
   {

      private TestResult _testResult;

      /// <summary>
      /// Creates an instance of TestItemViewModel.
      /// </summary>
      /// <param name="result">Test result data associated with this view model.</param>
      public TestItemViewModel(TestResult result)
      {
         _testResult = result;
      }

      /// <summary>
      /// Gets the test full name.
      /// </summary>
      public string FullName
      {
         get { return _testResult.FullName; }
      }

      /// <summary>
      /// Gets the test short name.
      /// </summary>
      public string Name
      {
         get { return _testResult.Name; }
      }

      /// <summary>
      /// Gets the test result.
      /// </summary>
      public string Result
      {
         get { return _testResult.ResultState.ToString(); }
      }

      /// <summary>
      /// Gets the test result message.
      /// </summary>
      public string Message
      {
         get { return _testResult.Message; }
      }

      /// <summary>
      /// Is the test result is failure.
      /// </summary>
      public bool IsFailure
      {
         get { return (_testResult.ResultState != ResultState.Success); }
      }

      /// <summary>
      /// Gets the result icon.
      /// </summary>
      public BitmapImage ResultIcon
      {
         get
         {
            if (IsFailure)
            { return GeneralHelper.BitmapToBitmapImage(Resources.Resources.Cancel_32); }
            else
			{ return GeneralHelper.BitmapToBitmapImage(Resources.Resources.Check_32); }
         }
      }

   }  // End of class TestItemViewModel

}  // End of namespace rvtUnit.Controls
