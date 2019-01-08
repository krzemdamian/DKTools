#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Helpers;
using RevitDKTools.DockablePanels.ParameterEditor;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Moq;
using System.IO;
using System.Reflection;
using System.Threading;
using RevitDKTools.DockablePanels.ParameterEditor.ViewModel;
using RevitDKTools.DockablePanels.ParameterEditor.View;

namespace RevitDKTools.Tests.UnitTests
{
    [TestFixture]
    public class ParameterEditiorTests
    {
        [SetUp]
        public void Setup()
        {
            // setup pre-test environment
        }

        [TearDown]
        public void Cleanup()
        {
            // cleanup
        }


        [Test]
        public void PE_ShouldFormatText()
        {
            //Arrange
            double d = 5.726548;
            Document doc = GeneralHelper.ExternalCommandData.Application.ActiveUIDocument.Document;
            Units docUnits = doc.GetUnits();

            //Act
            string output = UnitFormatUtils.Format(docUnits, UnitType.UT_Length, d, false, true);

            //Assert
            Assert.Fail(output);

        }

        [Test]
        public void PE_ShouldSetDocumentVariable()
        {
            //Arrange
            DockablePanels.ParameterEditor.View.ParameterEditorWPFPage mrp = new DockablePanels.ParameterEditor.View.ParameterEditorWPFPage();
            mrp.RevitDocument = GeneralHelper.ExternalCommandData.Application.ActiveUIDocument.Document;
            mrp.VM.LengthModel.LengthInDouble = 5.726;

            //Act
            mrp.VM.TryFormatLengthToRepresentation();

            //Assert
            Assert.Fail(mrp.VM.LengthModel.LengthRepresentation);
        }


        [Test]
        public void PE_Window()
        {
            //Arrange
            RevitDKTools.DockablePanels.ParameterEditor.View.ParameterEditorWPFPage mrp = new RevitDKTools.DockablePanels.ParameterEditor.View.ParameterEditorWPFPage();
            mrp.RevitDocument = GeneralHelper.ExternalCommandData.Application.ActiveUIDocument.Document;

            TestWindow testWindow = new TestWindow();

            //testWindow.Run(mrp);
            //testWindow.ShowDialog();


            //Act
            mrp.VM.TryFormatLengthToRepresentation();

            //Assert
            //Assert.Fail(mrp.VM.UnitEditor.VisibleLength);
        }
    }
}
#endif