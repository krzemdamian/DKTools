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
		if doc.IsWorkshared == True and doc.IsDetached == False:
			sElIds = uidoc.Selection.GetElementIds()
			sEls = map(doc.GetElement, sElIds)
			
			reservedElements = WorksharingUtils.CheckoutElements(doc, sElIds)
			if reservedElements.Count != sElIds.Count:
				selected_element_set = {x for x in sElIds}
				reserved_element_set = {x for x in reservedElements}
				not_reserved_elements_set = [x for x in selected_element_set if x not in reserved_element_set]
				
				current_owner = []
				counter = 0
				for id in not_reserved_elements_set:
					WorksharingUtils.GetCheckoutStatus(doc, id, current_owner[counter])
				
			else:
				TaskDialog.Show("Information", "All selected elements have been reserved from central model.\n\nThis tool has not been tested and is not fully implemented.\nIt is possible that not all elements have been reserved and it is not checked if elements are all up to date with central model.\nThis tool should be developed on model which has some elements locked by other user.")
				uidoc.Selection.SetElementIds(reservedElements)
			
			
			## SET OF ELEMENTS TO BE RESERVED
			## CHECK CheckoutStatus  - if reserved by others
			## RESEVE AS MUCH AS CAN  --> SET OF RESERVED ELEMENTS
			## 		CHECK ELEMENTS UPDATE STATUS, IF NOT UP TO DATE: PROMPT TO RELOAD LATEST
			##      
			
			
		else:
			TaskDialog.Show("Error", "Document is not workshared or is detached.")
		
f = Program()