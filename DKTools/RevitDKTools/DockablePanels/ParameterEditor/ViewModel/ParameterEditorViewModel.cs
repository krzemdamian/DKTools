using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RevitDKTools.DockablePanels.ParameterEditor.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Drawing;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.ObjectModel;

namespace RevitDKTools.DockablePanels.ParameterEditor.ViewModel
{
    public class ParameterEditorViewModel : IParameterEditorViewModel, INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<string> _revitElementParameterNames;
        private int _indexOfSelectedRevitParameterName = 0;
        private LengthModel _lengthModel;
        private bool _autoUpdateModifiedParameterValue;
        private bool _lenghtTextBoxIsFocused = false; //field should be eliminated
        
        private string _revitElementSelectionStatus = "Selected 0 elements";
        private bool _manualParameterEdition = true; //field should be eliminated
        private bool _parameterUpdateAvailable = false;
        private string _parameterName = string.Empty;
        private string _parameterValue = string.Empty;
        private ExternalEventSetParameterValue _propertyExternalEventSetParameterValue;

        private Regex _selectionRegex = null;

        //injected fields:
        private Autodesk.Revit.DB.Document _revitDocument;
        public SelectionChangedWatcher RevitSelectionWatcher { get; set; }


        // Command fields for Binding from View
        private ICommand _getParameterValueCommand;
        private ICommand _setParameterValueCommand;
        private ICommand _setParameterNameCommand;
        private ICommand _getParameterNameCommand;
        private ICommand _parameterValueTextBox_SelectionChanged;
        private ICommand _lengthTextBox_TextChanged;
        private ICommand _lengthTextBox_GotFocus_Command;
        private ICommand _lengthTextBox_LostFocus_Command;
        #endregion

        // Constructor 
        public ParameterEditorViewModel()
        {
            PropertyExteralEventSetParameterValue = new ExternalEventSetParameterValue();
            SetParameterExternalEvent = ExternalEvent.Create(PropertyExteralEventSetParameterValue);
            _selectionRegex = new Regex(@"\d{4,5}");
            _parameterName = "Comments";

            _revitElementParameterNames = new ObservableCollection<string>
            {
                "Comments"
            };


            
            ParameterValue = string.Empty;

            _lengthModel = new LengthModel
            {
                //VisibleLength = "-222' 11 22/32\"",
                LengthRepresentation = string.Empty
                //SelectedText = string.Empty
            };

            _autoUpdateModifiedParameterValue = true;
        }

        #region ViewModel properties
        // Parameters not notifying OnPropertyChanged
        public Document RevitDocument
        {
            get { return _revitDocument; }
            set { _revitDocument = value; }
        }

        public ExternalEventSetParameterValue PropertyExteralEventSetParameterValue
        {
            get
            {
                return _propertyExternalEventSetParameterValue;
            }
            set
            {
                if(_propertyExternalEventSetParameterValue != value)
                {
                    _propertyExternalEventSetParameterValue = value;
                }
            }
        }

        public ExternalEvent SetParameterExternalEvent { get; }

        public bool LengthTextBoxIsFocused
        {
            get
            {
                return _lenghtTextBoxIsFocused;
            }
            set
            {
                if (value != _lenghtTextBoxIsFocused)
                {
                    _lenghtTextBoxIsFocused = value;
                }
            }
        }

        //Parameters notifying OnPropertyChanged
        public string ParameterName
        {
            get
            {
                return _parameterName;
            }
            set
            {
                if (value != _parameterName)
                {
                    _parameterName = value;
                    OnPropertyChanged("ParameterName");
                }
            }
        }

        public string ParameterValue
        {
            get
            {
                return _parameterValue;
            }
            set
            {
                if (value != _parameterValue)
                {
                    _parameterValue = value;
                    OnPropertyChanged("ParameterValue");
                }

            }
        }

        public bool ManualParameterEdition
        {
            get
            {
                return _manualParameterEdition;
            }
            set
            {
                if (value != _manualParameterEdition)
                {
                    _manualParameterEdition = value;
                    OnPropertyChanged("ManualParameterEdition");
                }
            }
        }

        public bool ParameterUpdateAvailable
        {
            get
            {
                return _parameterUpdateAvailable;
            }
            set
            {
                if (value != _parameterUpdateAvailable)
                {
                    _parameterUpdateAvailable = value;
                    OnPropertyChanged("ParameterUpdateAvailable");
                }
            }
        }

        public string RevitElementSelectionStatus
        {
            get { return _revitElementSelectionStatus; }
            set
            {
                if (value != _revitElementSelectionStatus)
                {
                    _revitElementSelectionStatus = value;
                    OnPropertyChanged("RevitElementSelectionStatus");
                }
            }
        }

        public ObservableCollection<string> RevitElementParameterNames
        {
            get { return _revitElementParameterNames; }
            set
            {
                if (value != _revitElementParameterNames)
                {
                    _revitElementParameterNames = value;
                    OnPropertyChanged("RevitElementParameterNames");
                }
            }
        }

        public bool AutoUpdateModifiedParameterValue
        {
            get { return _autoUpdateModifiedParameterValue; }
            set
            {
                if (value != _autoUpdateModifiedParameterValue)
                {
                    _autoUpdateModifiedParameterValue = value;
                    OnPropertyChanged("AutoUpdateModifiedParameterValue");
                }
            }
        }

        public int IndexOfSelectedRevitParameter
        {
            get { return _indexOfSelectedRevitParameterName; }
            set
            {
                if (value != _indexOfSelectedRevitParameterName)
                {
                    _indexOfSelectedRevitParameterName = value;
                    OnPropertyChanged("IndexOfSelectedRevitParameter");
                }
            }
        }

        public LengthModel LengthModel
        {
            get { return _lengthModel; }
            set
            {
                if (value != _lengthModel)
                {
                    _lengthModel = value;
                    OnPropertyChanged("LengthModel");
                }
            }
        }
        #endregion

        #region Commands for XAML view

        public ICommand LengthTextBox_GotFocus_Command
        {
            get
            {
                if (_lengthTextBox_GotFocus_Command == null)
                {
                    _lengthTextBox_GotFocus_Command = new RelayCommand(
                        param => LengthTextBox_GotFocus()
                    );
                }
                return _lengthTextBox_GotFocus_Command;
            }
        }

        public ICommand LengthTextBox_LostFocus_Command
        {
            get
            {
                if (_lengthTextBox_LostFocus_Command == null)
                {
                    _lengthTextBox_LostFocus_Command = new RelayCommand(
                        param => LengthTextBox_LostFocus()
                    );
                }
                return _lengthTextBox_LostFocus_Command;
            }
        }

        public ICommand LengthTextBox_TextChanged_Command
        {
            get
            {
                if (_lengthTextBox_TextChanged == null)
                {
                    _lengthTextBox_TextChanged = new RelayCommand(
                        param => LengthTextBox_TextChanged()
                    );
                }
                return _lengthTextBox_TextChanged;
            }
        }

        public ICommand ParameterValueTextBox_SelectionChanged_Command
        {
            get
            {
                if (_parameterValueTextBox_SelectionChanged == null)
                {
                    _parameterValueTextBox_SelectionChanged = new RelayCommand(
                        param => ParameterValueTextBox_SelectionChanged()
                    );
                }
                return _parameterValueTextBox_SelectionChanged;
            }
        }

        // TO DO: implement method
        public ICommand GetParameterValueCommand
        {
            get
            {
                if (_getParameterValueCommand == null)
                {
                    _getParameterValueCommand = new RelayCommand(
                        param => GetParameterValue()
                    );
                }
                return _getParameterValueCommand;
            }
        }

        // TO DO: implement method
        public ICommand SetParameterValueCommand
        {
            get
            {
                if (_setParameterValueCommand == null)
                {
                    _setParameterValueCommand = new RelayCommand(
                        param => SetParameterValue()
                    );
                }
                return _setParameterValueCommand;
            }
        }

        // TO DO: implement method
        public ICommand GetParameterNameCommand
        {
            get
            {
                if (_getParameterNameCommand == null)
                {
                    _getParameterNameCommand = new RelayCommand(
                        param => this.GetParameterName()
                        
                    );
                }
                return _getParameterNameCommand;
            }
        }

        // TO DO: implement method
        public ICommand SetParameterNameCommand
        {
            get
            {
                if (_setParameterNameCommand == null)
                {
                    _setParameterNameCommand = new RelayCommand(
                        param => SetParameterName()
                        
                    );
                }
                return _setParameterNameCommand;
            }
        }
        #endregion

        #region Command implementations

        private void LengthTextBox_GotFocus()
        {
            _lenghtTextBoxIsFocused = true;
            AutoUpdateModifiedParameterValue = _lenghtTextBoxIsFocused;
        }

        private void LengthTextBox_LostFocus()
        {
            _lenghtTextBoxIsFocused = false;
            LengthModel.LengthRepresentation = string.Empty;
            AutoUpdateModifiedParameterValue = _lenghtTextBoxIsFocused;
        }

        private void LengthTextBox_TextChanged()
        {
            if (_lenghtTextBoxIsFocused && !string.IsNullOrEmpty(LengthModel.SelectedText))
            {
                // run only if revit context injected
                if (_revitDocument != null && LengthModel.LengthRepresentation != null)
                {
                    Units docUnits = _revitDocument.GetUnits();
                    ValueParsingOptions vpo = new ValueParsingOptions();
                    vpo.AllowedValues = AllowedValues.NonNegative;
                    LengthModel.ParsingResult = UnitFormatUtils.TryParse(docUnits, UnitType.UT_Length,
                       LengthModel.LengthRepresentation, vpo, out double output);

                    if (LengthModel.ParsingResult == true)
                    {
                        LengthModel.LengthInDouble = output;
                        output = output * 12 * 32;
                        output = Math.Round(output, MidpointRounding.AwayFromZero);
                        int outputInt = Convert.ToInt32(output);
                        int count = LengthModel.ProperTextSelection.Length;
                        if (outputInt.ToString().Length > count)
                        {
                            LengthModel.ParsingResult = false;
                            LengthModel.UpdatedCodedLength = new string('9', count);
                        }
                        else
                        {
                            LengthModel.UpdatedCodedLength = PadNumberWithLeadingZeros(outputInt, count);
                        }
                    }
                    else if (LengthModel.UpdatedCodedLength == null)
                    {
                        int count = LengthModel.ProperTextSelection.Length;
                        LengthModel.UpdatedCodedLength = new string('9', count);
                    }

                    //TO DO: Change event to PropertyChanged event. This one is redundant.
                    OnLengthParsed(EventArgs.Empty);
                }
            }
        }

        public virtual void ParameterValueTextBox_SelectionChanged()
        {
            if (TryFormatCodedLengthToDouble())
            {
                TryFormatLengthToRepresentation();
            }
        }



        public void SetParameterName()
        {
            throw new NotImplementedException();
        }

        public void GetParameterName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set current state of ParameterValue to selected elements.
        /// </summary>
        public void SetParameterValue()
        {
            try
            {
                PropertyExteralEventSetParameterValue.RevitDocument = RevitDocument;
                PropertyExteralEventSetParameterValue.ElementList = RevitSelectionWatcher.Selection;
                PropertyExteralEventSetParameterValue.ParameterName = this.RevitElementParameterNames[IndexOfSelectedRevitParameter];
                PropertyExteralEventSetParameterValue.ParameterValue = this.ParameterValue;
                SetParameterExternalEvent.Raise();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetParameterValue()
        {
            MessageBox.Show("This is function invoced by ICommand interface through RelayCommand.");
        }

        #endregion

        #region Partial Methods for command implementations
        public void FindProperTextSelection()
        {
            LengthModel.ProperTextSelection = string.Empty;
            LengthModel.SelectionStart = int.MinValue;

            if (!string.IsNullOrEmpty(LengthModel.SelectedText))
            {
                Match match = _selectionRegex.Match(LengthModel.SelectedText);
                LengthModel.ProperTextSelection = match.Value;
                LengthModel.SelectionStart = match.Index;
            }
        }

        public bool TryFormatCodedLengthToDouble()
        {
            LengthModel.LengthInDouble = double.NaN;
            bool result = double.TryParse(LengthModel.ProperTextSelection, out double res);
            if (result)
            {
                LengthModel.LengthInDouble = res/12/32;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryFormatLengthToRepresentation()
        {
            try
            {
                if (_revitDocument != null && !double.IsNaN(LengthModel.LengthInDouble))
                {
                    Units docUnits = _revitDocument.GetUnits();
                    LengthModel.LengthRepresentation = UnitFormatUtils.Format(
                        docUnits, UnitType.UT_Length, LengthModel.LengthInDouble, true, true);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption hander from ParameterEditorViewModel.cs. \r\n" +
                    "TryFormatLengthToRepresentation method,\r\n" +
                    " Exeption message: \r\n\r\n" + ex.Message);
                return false;
            }
        }

        public void UpdateLengthRepresentation()
        {
            //Check if in Revit context.
            if (_revitDocument != null)
            {
                Units docUnits = _revitDocument.GetUnits();
                ValueParsingOptions vpo = new ValueParsingOptions();
                vpo.AllowedValues = AllowedValues.NonNegative;
                bool parsRes = UnitFormatUtils.TryParse(docUnits, UnitType.UT_Length,
                   LengthModel.LengthRepresentation, vpo, out double output);
                //If parsing succeded
                if (parsRes == true)
                {
                    output = output * 12 * 32;
                    output = Math.Round(output, 0, MidpointRounding.AwayFromZero);
                    output = output / 12 / 32;
                    LengthModel.LengthRepresentation = UnitFormatUtils.Format(
                        docUnits, UnitType.UT_Length, output, true, true);
                }
            }
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Converts integer to string padded with zeros to obtain given string length.
        /// </summary>
        /// <param name="value">Value cannot have more digits than stringLength.</param>
        /// <param name="stringLength">Count of final string length padded with zeros.</param>
        /// <returns></returns>
        private string PadNumberWithLeadingZeros(int value, int stringLength)
        {
            if (value >= 0)
            {
                string result = value.ToString();
                if (result.Length <= stringLength)
                {
                    int total = stringLength - result.Length;
                    for (int i = 0; i < total; i++)
                    {
                        result = "0" + result;
                    }
                }

                return result;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateParameterValueOnRevitSelectionChange()
        {
            FilteredElementCollector collector = new FilteredElementCollector(
                RevitDocument, RevitSelectionWatcher.Selection).WhereElementIsNotElementType();

            //Parameter selectedParameter = collector.FirstOrDefault().
            //    LookupParameter(RevitElementParameterNames[IndexOfSelectedRevitParameter]);
            Parameter selectedParameter = collector.FirstOrDefault().
                LookupParameter(ParameterName);
            
            if (selectedParameter != null && selectedParameter.HasValue)
            {
                string parameterValue = selectedParameter.AsString();
                bool allValuesMatch = true;
                foreach (Element el in collector.ToElements())
                {
                    Parameter parameter = el.LookupParameter(ParameterName);
                    //Parameter parameter = el.LookupParameter(RevitElementParameterNames[IndexOfSelectedRevitParameter]);
                    if (parameter == null)
                    {
                        ManualParameterEdition = false;
                        ParameterUpdateAvailable = false;
                        ParameterValue = "Element without provided parameter in the selection.";
                        allValuesMatch = false;
                        break;
                    }
                    else if (parameter.AsString() != parameterValue)
                    {
                        ManualParameterEdition = false;
                        ParameterUpdateAvailable = false;
                        ParameterValue = "Parameter value varies throughout selected elements";
                        allValuesMatch = false;
                        break;
                    }

                }
                if (allValuesMatch == true)
                {
                    ManualParameterEdition = true;
                    ParameterUpdateAvailable = true;
                    ParameterValue = parameterValue;
                }

            }
            else
            {
                ManualParameterEdition = false;
                ParameterUpdateAvailable = false;
                ParameterValue = "Element without provided parameter or value in the selection.";
            }
    }
        #endregion

        #region Event Handlers
        public void RevitActiveSelection_SelectionChanged(object sender, EventArgs eventArgs)
        {
            if (RevitSelectionWatcher.Selection.Count == 0)
            {
                ManualParameterEdition = true;
                ParameterValue = string.Empty; //binding lost already
                RevitElementSelectionStatus = string.Format("No selection. Sandbox mode.");
                ParameterUpdateAvailable = false;
            }
            else if (RevitSelectionWatcher.Selection.Count == 1)
            {
                RevitElementSelectionStatus = string.Format("Selected element Id: {0}", RevitSelectionWatcher.Selection[0].IntegerValue);
                UpdateParameterValueOnRevitSelectionChange();
            }
            else
            {
                RevitElementSelectionStatus = string.Format("Selected {0} elements", RevitSelectionWatcher.Selection.Count.ToString());
                UpdateParameterValueOnRevitSelectionChange();
            }

        }
        #endregion

        #region Event declarations
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        
        public event EventHandler LengthParsed;

        public virtual void OnLengthParsed(EventArgs e)
        {
            EventHandler handler = LengthParsed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
