#Created by Damian Krzemiński, contact: krzemodam2@gmail.com
_debug_ = False

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
<a href="https://github.com/krzemdamian/DKTools">Check source code GitHub Repository</a>

CONTACT:
damian.krzeminski@op.pl
krzemodam2@gmail.com
\n
Icons designed by Smashicons from Flaticon: www.flaticon.com""".format(chr(169))
mainDialog.FooterText = '<a href="https://github.com/krzemdamian/DKTools/wiki.html">https://github.com/krzemdamian/DKTools</a>'
tResult = mainDialog.Show()