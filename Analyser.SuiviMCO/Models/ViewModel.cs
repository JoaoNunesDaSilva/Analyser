using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Analyser.SuiviMCO.Models
{
    public class ViewModel : ISuiviMCOModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ViewModel() {
            LookupData = new ObservableCollection<LookupData>();
            MCOData = new ObservableCollection<MCOData>();
            MCODataEspaceClient = new ObservableCollection<MCOData>();
            SuiviData = new ObservableCollection<SuiviData>();
        }

        string _LookupFile = @"C:\La Medicale\Analyser\Extracts\Suivi_MCO_LOOKUP";
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
        ObservableCollection<LookupData> _LookupData;
        public ObservableCollection<LookupData> LookupData
        {
            get
            {
                return _LookupData;
            }
            set
            {
                _LookupData = value;
                OnPropertyChanged("LookupData");
            }
        }
        string _MCOFile = @"C:\La Medicale\Analyser\Extracts\Suivi_MCO";
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
        ObservableCollection<MCOData> _MCOData;
        public ObservableCollection<MCOData> MCOData
        {
            get
            {
                return _MCOData;
            }
            set
            {
                _MCOData = value;
                OnPropertyChanged("MCOData");
                OnPropertyChanged("NroFiches");
            }
        }
        string _MCOFileEspaceClient = @"C:\La Medicale\Analyser\Extracts\Suivi_MCO_EspClient";
        public string MCOFileEspaceClient
        {
            get
            {
                return _MCOFileEspaceClient;
            }
            set
            {
                _MCOFileEspaceClient = value;
                OnPropertyChanged("MCOFileEspaceClient");
            }
        }
        ObservableCollection<MCOData> _MCODataEspaceClient;
        public ObservableCollection<MCOData> MCODataEspaceClient
        {
            get
            {
                return _MCODataEspaceClient;
            }
            set
            {
                _MCODataEspaceClient = value;
                OnPropertyChanged("MCODataEspaceClient");
            }
        }
        string _DataFile = @"C:\La Medicale\Analyser\Extracts\Suivi.data";
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
        ObservableCollection<SuiviData> _SuiviData;
        public ObservableCollection<SuiviData> SuiviData
        {
            get
            {
                return _SuiviData;
            }
            set
            {
                _SuiviData = value;
                OnPropertyChanged("SuiviData");
            }
        }

        ObservableCollection<FilterModel> _Filters;
        public ObservableCollection<FilterModel> Filters
        {
            get
            {
                return _Filters;
            }
            set
            {
                _Filters = value;
                OnPropertyChanged("Filters");
            }
        }

        public int NroFiches
        {
            get
            {
                return _MCOData.Count;
            }
        }

    }
}
