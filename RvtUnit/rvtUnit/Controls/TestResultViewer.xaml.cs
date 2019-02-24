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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace rvtUnit.Controls
{
   /// <summary>
   /// Interaction logic for TestResultViewer.xaml
   /// </summary>
   public partial class TestResultViewer : Window
   {
      public TestResultViewer(TestResultViewerViewModel vm)
      {
         InitializeComponent();

         this.DataContext = vm;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="e"></param>
      protected override void OnSourceInitialized(EventArgs e)
      {
         base.OnSourceInitialized(e);
      }
   }
}
