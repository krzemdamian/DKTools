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
		# ids of selected elements
		sElIds = uidoc.Selection.GetElementIds()
		
		# selected elements
		sEls = [doc.GetElement(sElId) for sElId in sElIds]
		
		# retrive element parameter
		if rvt._variables_['param']:
			sElParPos = [sEl.LookupParameter(rvt._variables_['param']) for sEl in sEls]
			sElParPositions = [sEl.LookupParameter(rvt._variables_['param']).AsString() for sEl in sEls if sEl.LookupParameter(rvt._variables_['param'])]
			
			
			# check if elements have this parameters and verify if this parameters have values assigned
			if None in sElParPos:	
				if "" in sElParPositions:
					TaskDialog.Show("Warning", "Some elements have no {} parameter.\nSome elements have no value in {} parameter.\nOther elemets' parameters have been copied to clipboard.".format(rvt._variables_['param']))
					
					clipboardList = [p + "\r\n" for p in sElParPositions if p]
		
					clipboard = "".join(clipboardList)
				
					self.log(clipboard)
					
					try:
						System.Windows.Forms.Clipboard.Clear()
						System.Windows.Forms.Clipboard.SetText(clipboard)
						
					except:
						System.Windows.Forms.Clipboard.Clear()
						self.log("Something went wrong")
	
				else:
					TaskDialog.Show("Warning", "Some elements have no {} parameter.\nOther elemets' parameters have been copied to clipboard.".format(rvt._variables_['param']))			
					
					clipboardList = [p + "\r\n" for p in sElParPositions if p]
				
					clipboard = "".join(clipboardList)
				
					self.log(clipboard)
					
					try:
						System.Windows.Forms.Clipboard.Clear()
						System.Windows.Forms.Clipboard.SetText(clipboard)
						
					except:
						System.Windows.Forms.Clipboard.Clear()
						self.log("Something went wrong")
	
					
			else:
				if "" in sElParPositions:
					TaskDialog.Show("Warning", "Some elements have no value in {} parameter.\nOther elemets' parameters have been copied to clipboard.".format(rvt._variables_['param']))
					
					clipboardList = [p + "\r\n" for p in sElParPositions if p]
		
					clipboard = "".join(clipboardList)
				
					self.log(clipboard)
					
					try:
						System.Windows.Forms.Clipboard.Clear()
						System.Windows.Forms.Clipboard.SetText(clipboard)
						
					except:
						System.Windows.Forms.Clipboard.Clear()
						self.log("Something went wrong")
	
				else:				
					clipboardList = [p + "\r\n" for p in sElParPositions if p]
				
					clipboard = "".join(clipboardList)
				
					self.log(clipboard)
					
					try:
						System.Windows.Forms.Clipboard.Clear()
						System.Windows.Forms.Clipboard.SetText(clipboard)
						
					except:
						System.Windows.Forms.Clipboard.Clear()
						self.log("Something went wrong")
		
		else:
			TaskDialog.Show("Error", "Set parameter first")

f = Program()
