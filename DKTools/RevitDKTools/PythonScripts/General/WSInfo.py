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
		sElIds = uidoc.Selection.GetElementIds()
		sEls = map(doc.GetElement, sElIds)
		if sElIds.Count == 1:
			tooltipInfo = WorksharingUtils.GetWorksharingTooltipInfo(doc, sElIds[0])
			td = TaskDialog("Worksharing information about element")
			td.MainInstruction = "Worksharing Information"
			td.MainContent = "Creator: {}\nLast changed by: {}\nCurrent owner: {}".format(tooltipInfo.Creator,tooltipInfo.LastChangedBy,tooltipInfo.Owner)
			td.Show()
			
		else:
			TaskDialog.Show("Error", "Please select one element.")

f = Program()