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
		# all tag elements visibile in active view
		els = FilteredElementCollector(doc, activeView.Id).OfClass(IndependentTag).ToElements()

		# list with tag elements visible in active view
		tags = [el for el in els if el.Category.IsTagCategory]
		
		# list with tag ids visible in active view
		tagIds = [tag.Id.IntegerValue for tag in tags]
		
		# list with tag host ids (unique and duplicated)
		tagHostIds = [tag.TaggedLocalElementId.IntegerValue for tag in tags]
		
		# ids of selected elements
		sElIds = uidoc.Selection.GetElementIds()
		
		# get host ids of selected tags
		sTags = [doc.GetElement(sElId).TaggedLocalElementId.IntegerValue for sElId in sElIds if doc.GetElement(sElId).Category.IsTagCategory]

		#start list with elements to be selected - current selection
		selectIds=[sElId.IntegerValue for sElId in sElIds]
		
		# add host ids of selected tags to selection
		[selectIds.append(sTag) for sTag in sTags]
		
		#double loop to add duplicated tags to selection
		[[selectIds.append(tagIds[index]) for index in range(len(tagHostIds)) if tagHostIds[index] == sElId.IntegerValue] for sElId in sElIds]
		
		#change set elements to Revit ElementId objects and make ICollection containting this elements
		revitEls = List[ElementId]()
		[revitEls.Add(ElementId(selectId)) for selectId in selectIds]
		
		#select listed objects
		uidoc.Selection.SetElementIds(revitEls)		
		
		self.ProgresBarUpdate(100)




f = Program()
