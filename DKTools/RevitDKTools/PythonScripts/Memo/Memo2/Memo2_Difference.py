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

import rvt
if not rvt._variables_.ContainsKey("memo2"):
    rvt._variables_['memo2'] = List[ElementId]()

memo = rvt._variables_['memo2']

class Program(MainForm):
	#set class console variable to 1 to show console
	console = 0
	def Start(self):
		# ids of selected elements
		sElIds = uidoc.Selection.GetElementIds()
		
		# change Lists to python sets
		sElIdsSet = {sElId for sElId in sElIds}
		memoSet = {mem for mem in memo}
		
		newSelection = memoSet.symmetric_difference(sElIdsSet)
		newSelectionList = List[ElementId]()
		map(newSelectionList.Add, newSelection)
		
		uidoc.Selection.SetElementIds(newSelectionList)
		
		# information for console
		if self.console:
			toLog = [x for x in newSelection]
			self.log(toLog)
		else:
			self.log("?")

f = Program()
