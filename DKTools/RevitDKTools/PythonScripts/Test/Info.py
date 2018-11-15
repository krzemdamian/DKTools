#Created by Damian Krzemiński, contact: krzemodam2@gmail.com

import clr
clr.AddReference("System.Collections")
import System.Collections.Generic
from System.Collections.Generic import List, Dictionary

from Autodesk.Revit.DB import *
from Autodesk.Revit.UI import *


# Creates a Revit task dialog to communicate information to the user.
mainDialog = TaskDialog("Information about DKTools Add-In");
mainDialog.MainInstruction = "Information about DKTools Add-In";
mainDialog.MainContent = """
Created by Damian Krzemiński {}

CONTACT:
damian.krzeminski@op.pl
krzemodam2@gmail.com
\n
Icons designed by Smashicons from Flaticon: www.flaticon.com""".format(chr(169))

tResult = mainDialog.Show()