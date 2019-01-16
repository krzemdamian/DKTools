#Created by Damian Krzemiński, contact: krzemodam2@gmail.com
_debug_ = False

import sys
sys.path.append(_app_path_ + "\\PythonScripts")
sys.path.append(_app_path_ + "\\PythonScripts\\Lib")
from MainForm import MainForm
import threading
import time
import rvt

import clr
clr.AddReference("System.Collections")
import System.Collections.Generic
from System.Collections.Generic import List, Dictionary

from Autodesk.Revit.DB import *
from Autodesk.Revit.DB.Structure import *

from Autodesk.Revit.UI import *
import Autodesk.Revit.Creation
from Autodesk.Revit.Creation import *


uiapp = _command_data_.Application
app = uiapp.Application
uidoc = uiapp.ActiveUIDocument
doc = uidoc.Document
cdoc = doc.Create

rvt._event_path_.Location = "{}\\PythonScripts\\Connections\\ConnectionHostVerification_sub.py".format(_app_path_)

class Program(MainForm):
    #set "console" class parameter to 1 for debug dialog, set 2 fore modeless dialog
    console = 2
    
    def Start(self):
        #self.log([opt for opt in rvt._event_path_])
        rvt._event_.Raise()
        #exEvent.Dispose()
    
f = Program()

def start_form():
    f.ShowDialog()


thread_operation = threading.Thread(target = start_form)
thread_operation.daemon = True
thread_operation.start()
