#Created by Damian Krzemiński, contact: krzemodam2@gmail.com
_debug_ = False

import sys
sys.path.append(_app_path_ + "\\PythonScripts")
sys.path.append(_app_path_ + "\\PythonScripts\\Lib")
from MainForm import MainForm
import os

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
dkrzeminski@prescientco.com
damian.krzeminski@op.pl
krzemodam2@gmail.com
\n
Icons designed by Smashicons from Flaticon: www.flaticon.com""".format(chr(169))

# Add commmandLink options to task dialog
mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "View information about icons licence (PDF File)");

# Set common buttons and default button. If no CommonButton or CommandLink is added,
# task dialog will show a Close button by default
mainDialog.CommonButtons = TaskDialogCommonButtons.Ok;
mainDialog.DefaultButton = TaskDialogResult.Ok;

# Set footer text. Footer text is usually used to link to the help document.
#mainDialog.FooterText = "Damian Krzemiński";

tResult = mainDialog.Show();

# If the user clicks the licence link:
if TaskDialogResult.CommandLink1 == tResult:
	os.system("start E:\DKTools\icons\license.pdf")