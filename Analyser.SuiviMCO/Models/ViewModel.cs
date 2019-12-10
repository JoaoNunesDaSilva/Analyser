using Analyser.Infrastructure.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.SuiviMCO.Models
{
    public class ViewModel : ISuiviMCOModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        string _LookupFile;
        public string LookupFile
        {
            get
            {
                return _LookupFile;
            }
            set
            {
                _LookupFile = value;
                OnPropertyChanged("LookupFile");
            }
        }
        string _MCOFile;
        public string MCOFile
        {
            get
            {
                return _MCOFile;
            }
            set
            {
                _MCOFile = value;
                OnPropertyChanged("MCOFile");
            }
        }
        string _DataFile;
        public string DataFile
        {
            get
            {
                return _DataFile;
            }
            set
            {
                _DataFile = value;
                OnPropertyChanged("DataFile");
            }
        }

    }
}
