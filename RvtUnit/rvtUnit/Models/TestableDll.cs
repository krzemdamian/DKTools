using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace rvtUnit.Models
{
    public class TestableDll : INotifyPropertyChanged
    {
        public TestableDll(string fullFileName)
        {
            Name = Path.GetFileName(fullFileName);
            Folder = Path.GetDirectoryName(fullFileName);
			IsAllChecked = true;
        }

		public IEnumerable<Test> Tests { get; set; }

        public string Name { get; private set; }

        public string Folder { get; private set; }

        public string FullPath
        {
            get
            {
                return (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Folder)) ? Path.Combine(Folder, Name) : null;
            }
        }

		private bool _IsAllChecked;
		public bool IsAllChecked
		{
			get
			{
				return _IsAllChecked;
			}
			set
			{
				_IsAllChecked = value;
				NotifyPropertyChanged("IsAllChecked");
				if (Tests != null)
				{
					foreach (Test test in Tests)
					{
						test.IsChecked = value;
					}
				}
			}
		}

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		#endregion
    }
}
