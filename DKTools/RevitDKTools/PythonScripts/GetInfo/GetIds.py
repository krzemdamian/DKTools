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


class Program(MainForm):
	#set class console variable to 1 to show console
	console = 0
	def Start(self):		
		# ids of selected elements
		sElIds = uidoc.Selection.GetElementIds()
		
		# ids to string
		sElIdsInteger = [id.IntegerValue for id in sElIds]
		
		clipboardList = [str(id) + "\r\n" for id in sElIdsInteger]
		#clipboardList = [p + "\r\n" for t, p in sElZipped]

		clippobard = "".join(clipboardList)
		
		self.log(clippobard)
		
		try:
			System.Windows.Forms.Clipboard.SetText(clippobard)
			
		except:
			self.log("Something went wrong")


f = Program()
