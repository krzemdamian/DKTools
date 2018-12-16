#Created by Damian Krzemiński, contact: krzemodam2@gmail.com

import clr

clr.AddReference("System.Drawing")
clr.AddReference("System.Windows.Forms")

import System.Drawing
import System.Windows.Forms

from System.Drawing import *
from System.Windows.Forms import *

class SetParamForm(Form):
	result = ""
	
	def __init__(self):
		self.InitializeComponent()
		#self.ShowDialog()
	
	def InitializeComponent(self):
		self._button1 = System.Windows.Forms.Button()
		self._textBox1 = System.Windows.Forms.TextBox()
		self.SuspendLayout()
		# 
		# button1
		# 
		self._button1.Location = System.Drawing.Point(179, 10)
		self._button1.Name = "button1"
		self._button1.Size = System.Drawing.Size(75, 23)
		self._button1.TabIndex = 0
		self._button1.Text = "Confirm"
		self._button1.UseVisualStyleBackColor = True
		self._button1.Click += self.Button1Click
		# 
		# textBox1
		# 
		self._textBox1.Location = System.Drawing.Point(12, 12)
		self._textBox1.MaxLength = 128
		self._textBox1.Name = "textBox1"
		self._textBox1.Size = System.Drawing.Size(161, 20)
		self._textBox1.TabIndex = 1
		# 
		# SetParamForm
		# 
		self.AcceptButton = self._button1
		self.ClientSize = System.Drawing.Size(266, 42)
		self.Controls.Add(self._textBox1)
		self.Controls.Add(self._button1)
		self.KeyPreview = True
		self.MaximizeBox = False
		self.MaximumSize = System.Drawing.Size(282, 80)
		self.MinimizeBox = False
		self.MinimumSize = System.Drawing.Size(282, 80)
		self.Name = "SetParamForm"
		self.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		self.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		self.Text = "SetParamForm"
		self.TopMost = True
		self._textBox1.Focus()
		self.ActiveControl = self._textBox1
		self.ResumeLayout(False)
		self.PerformLayout()
		

	def Button1Click(self, sender, e):
		if self._textBox1.Text:
			self.result = self._textBox1.Text
			self.DialogResult = DialogResult.OK
		self.Close()
		
#f = SetParamForm()
#f.ShowDialog()
#print(f.result)