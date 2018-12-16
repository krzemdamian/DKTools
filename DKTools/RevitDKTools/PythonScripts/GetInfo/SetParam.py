#Created by Damian Krzemiński, contact: krzemodam2@gmail.com
_debug_ = False

import sys
sys.path.append(_app_path_ + "\\PythonScripts")
sys.path.append(_app_path_ + "\\PythonScripts\\Lib")
from MainForm import MainForm
sys.path.append(_app_path_ + "\\PythonScripts\\GetInfo")
from SetParamForm import SetParamForm

import clr
clr.AddReference("System.Collections")
import System.Collections.Generic
from System.Collections.Generic import List, Dictionary

from Autodesk.Revit.DB import *
from Autodesk.Revit.UI import *

import rvt
if not rvt._variables_.ContainsKey('param'):
	rvt._variables_['param'] = ""



uiapp = _command_data_.Application
uidoc = uiapp.ActiveUIDocument
doc = uidoc.Document

class Program(MainForm):
	#set class console variable to 1 to show console
	console = 0
	def Start(self):
		f = SetParamForm()
		
		try:
			#f._textBox1.Text = param[0]
			f._textBox1.Text = rvt._variables_['param']
			#f.Refresh()
		except Exception as exception:
			pass
		
		f.ShowDialog()
		
		
		#if param.Count == 0:
		#	param.Add(f.result)
		#else:		
		#	rvt._variables_['param'] = f.result

		rvt._variables_['param'] = f.result

f = Program()
