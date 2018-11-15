using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace rvtUnit.Models
{
	public class Test : INotifyPropertyChanged
	{
		private bool _IsChecked;
		public bool IsChecked
		{
			get
			{
				return _IsChecked;
			}
			set
			{
				_IsChecked = value;
				NotifyPropertyChanged("IsChecked");
			}
		}

		private string _TestName;
		public string TestName
		{
			get
			{
				return _TestName;
			}
			set
			{
				_TestName = value;
				NotifyPropertyChanged("TestName");
			}
		}

		private Brush _brush;
		public Brush Brush
		{
			get { return _brush; }
			set
			{
				_brush = value;
				NotifyPropertyChanged("Brush");
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
