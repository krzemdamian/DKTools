#Created by Damian Krzemiński, contact: krzemodam2@gmail.com
_debug_ = False

import sys
sys.path.append(_app_path_ + "\\PythonScripts")
sys.path.append(_app_path_ + "\\PythonScripts\\Lib")
from MainForm import MainForm

import clr
clr.AddReference("System.Collections")
import System.Collections.Generic
from System.Collections.Generic import List, Dictionary

from Autodesk.Revit.DB import *
from Autodesk.Revit.UI import *

uiapp = _command_data_.Application
app = uiapp.Application
uidoc = uiapp.ActiveUIDocument
doc = uidoc.Document

class Program(MainForm):
	#set class console variable to 1 to show console
	console = 0
	def Start(self):
		#get the id of the view template assigned to the active view
		templateId = doc.ActiveView.ViewTemplateId
		if templateId == ElementId.InvalidElementId:
			TaskDialog.Show("Error", "Active view must have a view template assigned.")
			
		else:
			otherDocSet = app.Documents
			other_docs = []
			for d in otherDocSet:
				if d.IsLinked == False and not d.Equals(doc):
					other_docs.append(d)
					
			other_docs_len = len(other_docs)
			
			if other_docs_len > 4:
				TaskDialog.Show("Error", "There are too many documents opened in this Revit session.\nMaximum 4 documents opened.")
				
			elif other_docs_len == 0:
				TaskDialog.Show("Error", "There is no other document to copy active template!\nPlease open document to which template should be copied.")
				
			else:
				td = TaskDialog("Copy Template")
				td.MainInstruction = "Copy Template"
				td.MainContent = "Please select document to which you wish copy active template:"
				
				td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "{}".format(other_docs[0].Title))
				
				if other_docs_len > 1:
					td.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "{}".format(other_docs[1].Title))
				if other_docs_len > 2:
					td.AddCommandLink(TaskDialogCommandLinkId.CommandLink3, "{}".format(other_docs[2].Title))
				if other_docs_len > 3:
					td.AddCommandLink(TaskDialogCommandLinkId.CommandLink4, "{}".format(other_docs[3].Title))
					
				td_result = td.Show()
				
				if td_result == TaskDialogResult.CommandLink1:
					copyIds = List[ElementId]()
					copyIds.Add(templateId)
					cpOpts = CopyPasteOptions()
					
					t = Transaction(other_docs[0], "Copy View Template")
					t.Start()
					ElementTransformUtils.CopyElements(doc, copyIds, other_docs[0], Transform.Identity, cpOpts);
					t.Commit();
					TaskDialog.Show("Information", "Template successfuly copied to {}.".format(other_docs[0].Title), TaskDialogCommonButtons.Ok)				
	
				if td_result == TaskDialogResult.CommandLink2:
					copyIds = List[ElementId]()
					copyIds.Add(templateId)
					cpOpts = CopyPasteOptions()
					
					t = Transaction(other_docs[1], "Copy View Template")
					t.Start()
					ElementTransformUtils.CopyElements(doc, copyIds, other_docs[1], Transform.Identity, cpOpts);
					t.Commit();
					TaskDialog.Show("Information", "Template successfuly copied to {}.".format(other_docs[1].Title), TaskDialogCommonButtons.Ok)
	
				if td_result == TaskDialogResult.CommandLink3:
					copyIds = List[ElementId]()
					copyIds.Add(templateId)
					cpOpts = CopyPasteOptions()

					t = Transaction(other_docs[2], "Copy View Template")
					t.Start()
					ElementTransformUtils.CopyElements(doc, copyIds, other_docs[2], Transform.Identity, cpOpts);
					t.Commit();
					TaskDialog.Show("Information", "Template successfuly copied to {}.".format(other_docs[2].Title), TaskDialogCommonButtons.Ok)
					
				if td_result == TaskDialogResult.CommandLink4:
					copyIds = List[ElementId]()
					copyIds.Add(templateId)
					cpOpts = CopyPasteOptions()

					t = Transaction(other_docs[3], "Copy View Template")
					t.Start()
					ElementTransformUtils.CopyElements(doc, copyIds, other_docs[3], Transform.Identity, cpOpts);
					t.Commit();
					TaskDialog.Show("Information", "Template successfuly copied to {}".format(other_docs[3].Title), TaskDialogCommonButtons.Ok)


f = Program()
