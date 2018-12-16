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
		selection_count = uidoc.Selection.GetElementIds().Count
		view = doc.ActiveView
		view_type = view.ViewType
		
		if view_type == ViewType.ThreeD and selection_count:	
			cmd = RevitCommandId.LookupPostableCommandId(PostableCommand.SelectionBox)
			uiapp.PostCommand(cmd)
		
		elif view_type == ViewType.ThreeD and not selection_count:
			view_section_box_active = view.IsSectionBoxActive
			view_parameter = view.LookupParameter("Section Box")
			
			if view_section_box_active:
				t = Transaction(doc, "Turn off Section Box")
				t.Start()
				self.log(view_parameter.Set(0))
				t.Commit()
			
		
f = Program()