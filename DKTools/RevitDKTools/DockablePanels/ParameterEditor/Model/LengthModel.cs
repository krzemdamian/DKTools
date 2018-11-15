using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RevitDKTools.DockablePanels.ParameterEditor.ViewModel;

namespace RevitDKTools.DockablePanels.ParameterEditor.Model
{
    public class LengthModel : INotifyPropertyChanged
    {
        private int _selectionStart;  //redundant?
        private int _selectionEnd; //redundant?
        private string _selectedText = string.Empty;
        private string _properTextSelection;
        private double _lengthInDouble = double.NaN;
        private string _lengthRepresentation;
        private bool _parsingResult;
        private string _updatedCodedLength;

        public string UpdatedCodedLength
        {
            get { return _updatedCodedLength; }
            set
            {
                if (value != _updatedCodedLength)
                {
                    _updatedCodedLength = value;
                    OnPropertyChanged("UpdatedCodedLength");
                }
            }
        }  //redundant?


        public bool ParsingResult
        {
            get { return _parsingResult; }
            set
            {
                if (value != _parsingResult)
                {
                    _parsingResult = value;
                    OnPropertyChanged("ParsingResult");
                }
            }
        }

        public string ProperTextSelection
        {
            get { return _properTextSelection; }
            set
            {
                if (value != _properTextSelection)
                {
                    _properTextSelection = value;
                    OnPropertyChanged("ProperTextSelection");
                }
            }
        }

        public string SelectedText
        {
            get { return _selectedText; }
            set
            {
                if (value != _selectedText)
                {
                    _selectedText = value;
                    OnPropertyChanged("SelectedText");
                }
            }
        }

        public int SelectionStart
        {
            get { return _selectionStart; }
            set
            {
                if (value != _selectionStart)
                {
                    _selectionStart = value;
                    OnPropertyChanged("SelectionStart");
                }
            }
        }

        public int SelectionEnd
        {
            get { return _selectionEnd; }
            set
            {
                if (value != _selectionEnd)
                {
                    _selectionEnd = value;
                    OnPropertyChanged("SelectionEnd");
                }
            }
        }

        public double LengthInDouble
        {
            get { return _lengthInDouble; }
            set
            {
                if (value != _lengthInDouble)
                {
                    _lengthInDouble = value;
                    OnPropertyChanged("LengthInDouble");
                }
            }
        }

        public string LengthRepresentation
        {
            get
            {
                return _lengthRepresentation;
            }
            set
            {
                if (value != _lengthRepresentation)
                {
                    _lengthRepresentation = value;
                    OnPropertyChanged("LengthRepresentation");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
