#created by Damian Krzemiński, contact: krzemodam2@gmail.com

import clr

clr.AddReference("System.Drawing")
clr.AddReference("System.Windows.Forms")

import System.Drawing
import System.Windows.Forms

from System.Drawing import *
from System.Windows.Forms import *


#by default console won't start
#console = 1

class MainForm(Form):
	#by default console won't start
	console = 0
	def __init__(self):
		if self.console == 1:
			self.InitializeComponent()
			self.ShowDialog()

		elif self.console == 2:
			self.InitializeComponent()
		else:
			self.Start()
		
	def InitializeComponent(self):
		self._button1 = System.Windows.Forms.Button()
		self._progressBar1 = System.Windows.Forms.ProgressBar()
		self._label1 = System.Windows.Forms.Label()
		self._textBox1 = System.Windows.Forms.TextBox()
		self._button2 = System.Windows.Forms.Button()
		self._label2 = System.Windows.Forms.Label()
		self._label3 = System.Windows.Forms.Label()
		self.SuspendLayout()
		# 
		# button1
		# 
		self._button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
		self._button1.ForeColor = System.Drawing.SystemColors.WindowText
		self._button1.Location = System.Drawing.Point(374, 12)
		self._button1.Name = "button1"
		self._button1.Size = System.Drawing.Size(75, 23)
		self._button1.TabIndex = 4
		self._button1.Text = "Confirm"
		self._button1.UseVisualStyleBackColor = True
		self._button1.Click += self.Button1Click
		# 
		# progressBar1
		# 
		self._progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
		self._progressBar1.Location = System.Drawing.Point(12, 41)
		self._progressBar1.Name = "progressBar1"
		self._progressBar1.Size = System.Drawing.Size(432, 23)
		self._progressBar1.TabIndex = 6
		# 
		# label1
		# 
		self._label1.Location = System.Drawing.Point(12, 20)
		self._label1.Name = "label1"
		self._label1.Size = System.Drawing.Size(59, 18)
		self._label1.TabIndex = 5
		self._label1.Text = "Progres:"
		# 
		# textBox1
		# 
		self._textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
		self._textBox1.BackColor = System.Drawing.SystemColors.Info
		self._textBox1.Cursor = System.Windows.Forms.Cursors.Default
		self._textBox1.Location = System.Drawing.Point(12, 86)
		self._textBox1.MaxLength = 90000
		self._textBox1.Multiline = True
		self._textBox1.Name = "textBox1"
		self._textBox1.ReadOnly = True
		self._textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		self._textBox1.Size = System.Drawing.Size(432, 188)
		self._textBox1.TabIndex = 9
		# 
		# button2
		# 
		self._button2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
		self._button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
		self._button2.ForeColor = System.Drawing.SystemColors.WindowText
		self._button2.Location = System.Drawing.Point(293, 12)
		self._button2.Name = "button2"
		self._button2.Size = System.Drawing.Size(75, 23)
		self._button2.TabIndex = 3
		self._button2.Text = "Close"
		self._button2.UseVisualStyleBackColor = True
		self._button2.Click += self.Button2Click
		# 
		# label2
		# 
		self._label2.Location = System.Drawing.Point(12, 67)
		self._label2.Name = "label2"
		self._label2.Size = System.Drawing.Size(437, 16)
		self._label2.TabIndex = 8
		self._label2.Text = "Log:"
		# 
		# label3
		# 
		self._label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
		self._label3.Location = System.Drawing.Point(369, 68)
		self._label3.Name = "label3"
		self._label3.Size = System.Drawing.Size(74, 15)
		self._label3.TabIndex = 7
		self._label3.Text = "0%"
		self._label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		# 
		# MainForm
		# 
		self.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange
		self.CancelButton = self._button2
		self.ClientSize = System.Drawing.Size(455, 286)
		self.Controls.Add(self._label3)
		self.Controls.Add(self._label2)
		self.Controls.Add(self._button2)
		self.Controls.Add(self._textBox1)
		self.Controls.Add(self._label1)
		self.Controls.Add(self._progressBar1)
		self.Controls.Add(self._button1)
		self.DoubleBuffered = True
		self.MinimumSize = System.Drawing.Size(366, 255)
		self.Name = "MainForm"
		self.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		self.DesktopLocation = Point(50,50)
		self.Text = "DKTools Console"
		self.ResumeLayout(False)
		self.PerformLayout()

	def log(self,new_line):
		if self.console:
			new_line=str(new_line)
			if self._textBox1.Text == "":
				self._textBox1.Text = new_line
			else:
				self._textBox1.Text = self._textBox1.Text + "\r\n" + new_line		
				self._textBox1.Select(self._textBox1.TextLength, 0)
				self._textBox1.ScrollToCaret()
				self._textBox1.Refresh()
		
	def Button1Click(self, sender, e):
		self.Start()

	def ProgresBarUpdate(self, new_value):
		if self.console:
			if new_value <= 100:
				new_value2 = int(round(new_value,0))
				self._progressBar1.Value = new_value2
				self.Refresh()
				self._label3.Text = "{0}%".format(str(new_value2))


	def Button2Click(self, sender, e):
		self.Close()
		self.Dispose()
		#commandData.Application.Application.PurgeReleasedAPIObjects()
		#commandData.Dispose()
			
#	def GUIClosing(self, sender, e):
#		commandData.Application.Application.PurgeReleasedAPIObjects()
#		commandData.Dispose()