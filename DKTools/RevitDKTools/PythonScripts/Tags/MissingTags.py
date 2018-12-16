#Created by Damian Krzemiński, contact: krzemodam2@gmail.com
_debug_ = False

import sys
sys.path.append(_app_path_ + "\\PythonScripts")
from MainForm import MainForm

import clr
clr.AddReference("System.Collections")
import System.Collections.Generic
from System.Collections.Generic import List, Dictionary

from Autodesk.Revit.DB import *
from Autodesk.Revit.UI import *


uiapp = _command_data_.Application
uidoc = uiapp.ActiveUIDocument
doc = uidoc.Document
selection = uidoc.Selection
selectedIds = uidoc.Selection.GetElementIds
activeView = uidoc.ActiveView
activeViewId = activeView.Id


class Program(MainForm):
	#set class console variable to 1 to show console
	console = 0
	def Start(self):
		sElIds = uidoc.Selection.GetElementIds()
		#sEls = FilteredElementCollector(doc,sElIds).WhereElementIsNotElementType().ToElements()
			
		sTags = [doc.GetElement(sElId) for sElId in sElIds if doc.GetElement(sElId).Category.IsTagCategory]
		sEls = [doc.GetElement(sElId) for sElId in sElIds if not doc.GetElement(sElId).Category.IsTagCategory]
		
		self.log("Number of tags: {}".format(len(sTags)))
		self.log("Number of elements: {}".format(len(sEls)))
		
		sTagHostIdsSet = set([sTag.TaggedLocalElementId.IntegerValue for sTag in sTags])
		sElIdsSet = set([sEl.Id.IntegerValue for sEl in sEls])
		
		elIdsWithMissingTagsSet = sElIdsSet.difference(sTagHostIdsSet)
		
		#change set elements to Revit ElementId objects and make ICollection containting this elements
		revitEls = List[ElementId]()
		[revitEls.Add(ElementId(x)) for x in elIdsWithMissingTagsSet]
		
		#select listed objects
		uidoc.Selection.SetElementIds(revitEls)
		
		#information in console
		self.log("There are {} elements with missing tags from selection.".format(len(elIdsWithMissingTagsSet)))
		self.log("Elements with missing tags [COPIED TO CLIPBOARD]:")
		
		try:
			copyToClipboard = [str(a) for a in elIdsWithMissingTagsSet]
			clipboardString = "\r\n".join(copyToClipboard)
			System.Windows.Forms.Clipboard.SetText(clipboardString)
			
		except:
			self.log("Something went wrong")
		
		
		barCount = 0
		
		[self.log(x) for x in elIdsWithMissingTagsSet]
		
		self.ProgresBarUpdate(100)


f = Program()