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
uidoc = uiapp.ActiveUIDocument
doc = uidoc.Document


class Program(MainForm):
	#set class console variable to 1 to show console
	console = 0
	def Start(self):
		# ids of selected elements
		sElIds = uidoc.Selection.GetElementIds()
		
		sEls = map(doc.GetElement, sElIds)
		
		# check from what category selected elements  - no duplicates
		# this category is used for FilteredElementCollector to retrive less elements
		seen_category = {sEl.Category.Id for sEl in sEls}
		
		# save selected elements families - no duplicates
		seen_family = {doc.GetElement(sEl.LookupParameter("Family").AsElementId()).Family.Id for sEl in sEls}
		
		# double nested loop to be able to select more than 1 element from different family
		# and search for all elements in their families
		fEls = set([])
		for category in seen_category:
			# each collector for each different category
			category_collector = FilteredElementCollector(doc).OfCategoryId(category).WhereElementIsNotElementType()
			
			cat_elems = category_collector.ToElements()

			for e in cat_elems:
				# TO BE OPTIMIZED!
				# search of elements from one family might be performed on separate sets of elements from the same category
				# all categories are stored in one set - can be seperate
				fEls.add(e)

		# new selection in python set
		new_selection = {el.Id for el in fEls if doc.GetElement(el.LookupParameter("Family").AsElementId()).Family.Id in seen_family}
		
		# show count of elements in new selection in console
		self.log(len(new_selection))
		
		# change python set of new selection to .NET List
		newSelectionList = List[ElementId]()
		map(newSelectionList.Add, new_selection)
		
		# make new selection
		uidoc.Selection.SetElementIds(newSelectionList)
		

f = Program()
