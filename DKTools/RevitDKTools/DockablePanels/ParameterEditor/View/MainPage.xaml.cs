using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RevitDKTools.DockablePanels.ParameterEditor.ViewModel;

namespace RevitDKTools.DockablePanels.ParameterEditor.View
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        protected TextRange _highlightedText;
        protected TextRange _allDocumentTextRange;

        public MainPage()
        {
            try
            {
                InitializeComponent();
                VM.LengthParsed += new EventHandler(UpdateHighlightedText);
                _allDocumentTextRange = new TextRange(
                    ParameterValueTextBox.Document.ContentStart, ParameterValueTextBox.Document.ContentEnd);
               
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception handler from MainPage.xaml.cs. Exeption message: " + exc.Message);
            }
        }

        public ParameterEditorViewModel VM
        {
            get { return (ParameterEditorViewModel)Resources["ViewModel"]; }
        }

        /// <summary>
        /// Forwards active selection to ViewModel.
        /// Uses ViewModel method to assign proper selection for unit conversion.
        /// Highlights proper selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParameterValueTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender!=null)
            {
                RichTextBox rtb = (RichTextBox)sender;
                TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                VM.ParameterValue = textRange.Text;
                VM.LengthModel.LengthRepresentation = string.Empty;
                
                if (VM.LengthTextBoxIsFocused == false)
                {
                    textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Transparent);
                }
                
                
                VM.LengthModel.SelectedText = (!rtb.Selection.IsEmpty) ? rtb.Selection.Text : string.Empty;
                VM.FindProperTextSelection();
                //VM.OnLengthParsed(new EventArgs());

                
                #region Text Highlight
                if(!string.IsNullOrEmpty(VM.LengthModel.ProperTextSelection) && !rtb.Selection.IsEmpty)
                {
                    TextPointer ss = rtb.Selection.Start;
                    TextPointer se = rtb.Selection.End;

                    
                    int counter = VM.LengthModel.SelectionStart;

                    while (counter > 0)
                    {
                        if (ss.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                        {
                            counter = counter - 1;
                        }
                        ss = ss.GetNextInsertionPosition(LogicalDirection.Forward);
                    }

                    TextPointer seTemp = se;
                    TextRange trTemp = new TextRange(ss, seTemp);

                    while (trTemp.Text.Contains(VM.LengthModel.ProperTextSelection))
                    {
                        seTemp = seTemp.GetNextInsertionPosition(LogicalDirection.Backward);
                        trTemp = new TextRange(ss, seTemp);
                        if (!trTemp.Text.Contains(VM.LengthModel.ProperTextSelection))
                            break;
                        se = seTemp;
                    }

                    _highlightedText = new TextRange(ss, se);


                    _highlightedText.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightGreen);



                    //I find set focus iritating. One cannot select whole parameter value.
                    //LengthTextBox.Focus();
                }

                if (!string.IsNullOrEmpty(VM.LengthModel.ProperTextSelection))
                {
                    LengthTextBox.Background = Brushes.LightGreen;
                }
                else
                {
                    LengthTextBox.Background = Brushes.Transparent;
                }
                #endregion
            }
        }
      
        private void UpdateHighlightedText(object sender, EventArgs e)
        {
            _highlightedText.Text = VM.LengthModel.UpdatedCodedLength;

            //if statement validation can be changed from public field to event arugment
            if (VM.LengthModel.ParsingResult)
            {
                _highlightedText.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightGreen);
                LengthTextBox.Background = Brushes.LightGreen;
            }
            else
            {
                _highlightedText.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightPink);
                LengthTextBox.Background = Brushes.LightPink;
            }
            VM.ParameterValue = _allDocumentTextRange.Text;
        }

        private void LengthTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Return))
            {
                VM.LengthTextBox_TextChanged_Command.Execute(sender);
                VM.UpdateLengthRepresentation();
            }
        }

        private void LengthTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Background = Brushes.Transparent;
        }

        public void OverwriteContentInRichTextBox(object sender, EventArgs eventArgs)
        {

            TextRange allRange = new TextRange(
                ParameterValueTextBox.Document.ContentStart, 
                ParameterValueTextBox.Document.ContentEnd);
            if (VM.RevitSelectionWatcher.Selection.Count == 0)
            {
                VM.ParameterValue = string.Empty;
            }
            allRange.Text = VM.ParameterValue;
            LengthTextBox.Background = Brushes.Transparent;
            LengthTextBox.Text = string.Empty;
        }
    }
}