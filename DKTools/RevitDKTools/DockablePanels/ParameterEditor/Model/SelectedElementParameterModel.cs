using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.DockablePanels.ParameterEditor.Model
{
    class SelectedElementParameterModel : ISelectedElementParameterModel, INotifyPropertyChanged
    {
        private string _parameterName = null;
        private string _parameterValue = null;


        public string ParameterName
        {
            get
            {
                return _parameterName;
            }
            set
            {
                if(value != _parameterName)
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
                if(value != _parameterValue)
                {
                    _parameterValue = value;
                    OnPropertyChanged("ParameterValue");
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
