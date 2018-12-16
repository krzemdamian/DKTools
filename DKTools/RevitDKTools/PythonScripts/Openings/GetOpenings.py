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
app = uiapp.Application
uidoc = uiapp.ActiveUIDocument
doc = uidoc.Document

class Program(MainForm):
	#set class console variable to 1 to show console
	console = 0
	def Start(self):		
		# ids of selected elements
		sElIds = uidoc.Selection.GetElementIds()
		
		# selected elements
		sEls = [doc.GetElement(sElId) for sElId in sElIds]
		
	
		element_categories = [sEl.Category.Name for sEl in sEls]
		self.log(element_categories)
		
		elements = [sEl.FindInserts(1,0,0,0) for sEl in sEls if sEl.Category.Name == "Walls"]
		self.log("test1")
		
		hostElementsIds = [sEl.Host.Id for sEl in sEls if sEl.Category.Name == "Windows" or  sEl.Category.Name == "Doors"]
		
		newSelection = [sElId for sElId in sElIds]
		[newSelection.append(id) for id in hostElementsIds]
		for element in elements:
			enum = element.GetEnumerator()
			for e in enum:
				newSelection.append(e)

		newSelectionList = List[ElementId]()
		[newSelectionList.Add(id) for id in newSelection]

		uidoc.Selection.SetElementIds(newSelectionList)

f = Program()
