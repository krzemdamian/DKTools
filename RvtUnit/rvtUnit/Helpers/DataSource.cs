#region Copyright (c) 2009-2011 Arup, All rights reserved.
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion
#region File Information
//
// Filename: DataSource.cs
// Author: Yamin Tengono <Yamin.Tengono@arup.com.au>
//
// This file is part of CADtools Revit - IntegrationTesting module.
//
#endregion

// !!! This file is applicable for all versions of Revit. !!! //

#if RVT2010
using Autodesk.Revit;
#else
using Autodesk.Revit.DB;
#endif

using System;
using System.IO;
using System.Reflection;

namespace IntegrationTesting.Helpers
{

   /// <summary>
   /// Provides the data for Revit integration testing.
   /// </summary>
   public static class DataSource
   {

      private const string _TestProjectFileName = "TestProject.rvt";
      private const string _TestProjectSrc2010 = "TestProject.2010.rvt";
      private const string _TestProjectSrc2011 = "TestProject.2011.rvt";
      private const string _TestProjectSrc2012 = "TestProject.2012.rvt";
      private const string _TestProjectSrc2013 = "TestProject.2013.rvt";

      private static string _localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + AppHelper.LocalRelativePath;

      /// <summary>
      /// Array that holds the test data.
      /// Only one item in the array needed.
      /// </summary>
      public static Document[] TestData = new Document[1];

      /// <summary>
      /// Gets or sets the Revit document object.
      /// </summary>
      public static Document RevitDoc
      {
         get { return TestData[0]; }
         set { TestData[0] = value; }
      }

      /// <summary>
      /// Gets the test project file name.
      /// </summary>
      public static string TestProjectFile { get; private set; }

      /// <summary>
      /// Extracts the required resources for integration testing.
      /// </summary>
      public static void ExtractRequiredResources()
      {
         Assembly thisAssembly = Assembly.GetExecutingAssembly();
         string sourceName = "IntegrationTesting.EmbeddedResources.";

         // Checks if the target folder exists:
         if (!Directory.Exists(_localPath))
         { Directory.CreateDirectory(_localPath); }

         // Extract the test project file:
         TestProjectFile = _localPath + _TestProjectFileName;
#if RVT2010
         sourceName += _TestProjectSrc2010;
#elif RVT2011
         sourceName += _TestProjectSrc2011;
#elif RVT2012
         sourceName += _TestProjectSrc2012;
#elif RVT2013
         sourceName += _TestProjectSrc2013;
#endif
         if (!File.Exists(TestProjectFile))
         {
            File.Create(TestProjectFile).Close();
            ACLHelper.ExtractResourceFileFromAssembly(thisAssembly, sourceName, TestProjectFile);
         }

      }

      /// <summary>
      /// Removes resources extracted for integration testing.
      /// </summary>
      public static void RemoveExtractedResources()
      {
         if (File.Exists(TestProjectFile)) { File.Delete(TestProjectFile); }
      }

   }  // End of class DataSource

}  // End of namespace IntegrationTesting.Tests
