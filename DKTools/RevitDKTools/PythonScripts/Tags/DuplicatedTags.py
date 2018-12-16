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
		els = FilteredElementCollector(doc, activeView.Id).OfCategory(BuiltInCategory.OST_WallTags).ToElements()
		[els.Add(element) for element in FilteredElementCollector(doc, activeView.Id).OfCategory(BuiltInCategory.OST_StructuralColumnTags).ToElements()]
		[els.Add(element) for element in FilteredElementCollector(doc, activeView.Id).OfCategory(BuiltInCategory.OST_StructuralFramingTags).ToElements()]

		tagHostIds = []
		tagIds = []

		for el in els:
			tagIds.append(el.Id.IntegerValue)
			tagHostIds.append(el.TaggedLocalElementId.IntegerValue)

		#check which elements have duplicated tags
		duplicatedHostIds = set([x for x in tagHostIds if tagHostIds.count(x) > 1])
		
		#start of set with elements to be selected - currently hosts only
		selectIds=set(duplicatedHostIds)
		
		#double loop to add duplicated tags to selection
		[[selectIds.add(tagIds[index]) for index in range(len(tagHostIds)) if tagHostIds[index] == hostId] for hostId in duplicatedHostIds]
		
		self.log("There are {} elements with duplicated tags.".format(len(duplicatedHostIds)))
		self.log("Affected elements have been selected.")
		
		#change set elements to Revit ElementId objects and make ICollection containting this elements
		revitEls = List[ElementId]()
		[revitEls.Add(ElementId(selectId)) for selectId in selectIds]
		
		#select listed objects
		uidoc.Selection.SetElementIds(revitEls)
		
		#[self.log(revitEl) for revitEl in revitEls]
		self.ProgresBarUpdate(100)

f = Program()