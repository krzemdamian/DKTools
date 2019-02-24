#region Copyright (c) 2009-2011 Arup, All rights reserved.
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion
#region File Information
//
// Filename: TestResultViewerViewModel.cs
// Author: Yamin Tengono <Yamin.Tengono@arup.com.au>
//
// This file is part of CADtools Revit - rvtUnit module.
//
#endregion

// !!! This file is applicable for all versions of Revit. !!! //

using System;
using System.Collections.ObjectModel;
using NUnit.Core;

namespace rvtUnit.Controls
{

   /// <summary>
   /// Provide data link for TestResultViewer user interface.
   /// </summary>
   public class TestResultViewerViewModel
   {

      /// <summary>
      /// Creates an instance of TestResultViewerViewModel.
      /// </summary>
      /// <param name="rootResult">The root of test result object.</param>
      public TestResultViewerViewModel(TestResult rootResult, string resultFile)
      {
         TestResults = new ObservableCollection<TestItemViewModel>();

         ResultFileName = resultFile;

         WalkTheTestResult(rootResult);
      }

      /// <summary>
      /// List of available test results.
      /// </summary>
      public ObservableCollection<TestItemViewModel> TestResults { get; set; }

      /// <summary>
      /// Gets the result file name.
      /// </summary>
      public string ResultFileName { get; set; }

      private void WalkTheTestResult(TestResult result)
      {
         if (result.Test.TestType.Equals("TestMethod", StringComparison.InvariantCultureIgnoreCase))
         {
            TestItemViewModel itemVM = new TestItemViewModel(result);
            TestResults.Add(itemVM);
            return;
         }
         if (result.Results == null) { return; }
         if (result.Results.Count <= 0) { return; }
         foreach (TestResult child in result.Results)
         {
            WalkTheTestResult(child);
         }
      }

   }  // End of class TestResultViewerViewModel

}  // End of namespace rvtUnit.Controls
