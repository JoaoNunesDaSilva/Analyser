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

        string _LookupFile = @"D:\Daily Reports\MCO\Suivi_MCO_LOOKUP";
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
        string _MCOFile = @"D:\Daily Reports\MCO\Suivi_MCO";
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
        string _MCOFileEspaceClient = @"D:\Daily Reports\MCO\Suivi_MCO_EspClient";
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
        string _DataFile = @"D:\Daily Reports\MCO\Suivi.data";
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

        // Lookups
        private List<string> _Priorite;
        public List<string> Priorite
        {
            get
            {
                return _Priorite;
            }
            set
            {
                _Priorite = value;
                OnPropertyChanged("Priorite");
            }
        }
        private List<string> _Gravite;
        public List<string> Gravite
        {
            get
            {
                return _Gravite;
            }
            set
            {
                _Gravite = value;
                OnPropertyChanged("Gravite");
            }
        }
        private List<string> _Statut;
        public List<string> Statut
        {
            get
            {
                return _Statut;
            }
            set
            {
                _Statut = value;
                OnPropertyChanged("Statut");
            }
        }
        private List<string> _Createur;
        public List<string> Createur
        {
            get
            {
                return _Createur;
            }
            set
            {
                _Createur = value;
                OnPropertyChanged("Createur");
            }
        }
        private List<string> _Diagnostiqueur;
        public List<string> Diagnostiqueur
        {
            get
            {
                return _Diagnostiqueur;
            }
            set
            {
                _Diagnostiqueur = value;
                OnPropertyChanged("Diagnostiqueur");
            }
        }
        private List<string> _Correcteur;
        public List<string> Correcteur
        {
            get
            {
                return _Correcteur;
            }
            set
            {
                _Correcteur = value;
                OnPropertyChanged("Correcteur");
            }
        }
        private List<string> _Site;
        public List<string> Site
        {
            get
            {
                return _Site;
            }
            set
            {
                _Site = value;
                OnPropertyChanged("Site");
            }
        }
        private List<string> _Responsable;
        public List<string> Responsable
        {
            get
            {
                return _Responsable;
            }
            set
            {
                _Responsable = value;
                OnPropertyChanged("Responsable");
            }
        }
        private List<string> _BlocsAplicatifsACorriger;
        public List<string> BlocsAplicatifsACorriger
        {
            get
            {
                return _BlocsAplicatifsACorriger;
            }
            set
            {
                _BlocsAplicatifsACorriger = value;
                OnPropertyChanged("BlocsAplicatifsACorriger");
            }
        }
        private List<string> _NatureDeLaMaintenance;
        public List<string> NatureDeLaMaintenance
        {
            get
            {
                return _NatureDeLaMaintenance;
            }
            set
            {
                _NatureDeLaMaintenance = value;
                OnPropertyChanged("NatureDeLaMaintenance");
            }
        }

        private List<string> _NatureDeLaFiche;
        public List<string> NatureDeLaFiche
        {
            get
            {
                return _NatureDeLaFiche;
            }
            set
            {
                _NatureDeLaFiche = value;
                OnPropertyChanged("NatureDeLaFiche");
            }
        }

        private List<string> _Version;
        public List<string> Version
        {
            get
            {
                return _Version;
            }
            set
            {
                _Version = value;
                OnPropertyChanged("Version");
            }
        }

        private List<string> _TypeMaintenance;
        public List<string> TypeMaintenance
        {
            get
            {
                return _TypeMaintenance;
            }
            set
            {
                _TypeMaintenance = value;
                OnPropertyChanged("TypeMaintenance");
            }
        }

        private List<string> _DirectionResponsable;
        public List<string> DirectionResponsable
        {
            get
            {
                return _DirectionResponsable;
            }
            set
            {
                _DirectionResponsable = value;
                OnPropertyChanged("DirectionResponsable");
            }
        }

        private List<string> _SecteurDeRecette;
        public List<string> SecteurDeRecette
        {
            get
            {
                return _SecteurDeRecette;
            }
            set
            {
                _SecteurDeRecette = value;
                OnPropertyChanged("SecteurDeRecette");
            }
        }

        private List<string> _DomaineDeDetection;
        public List<string> DomaineDeDetection
        {
            get
            {
                return _DomaineDeDetection;
            }
            set
            {
                _DomaineDeDetection = value;
                OnPropertyChanged("DomaineDeDetection");
            }
        }

        private List<string> _DomaineCorrection;
        public List<string> DomaineCorrection
        {
            get
            {
                return _DomaineCorrection;
            }
            set
            {
                _DomaineCorrection = value;
                OnPropertyChanged("DomaineCorrection");
            }
        }

        private List<string> _SecteurCorrection;
        public List<string> SecteurCorrection
        {
            get
            {
                return _SecteurCorrection;
            }
            set
            {
                _SecteurCorrection = value;
                OnPropertyChanged("SecteurCorrection");
            }
        }

        private List<string> _SousSecteur;
        public List<string> SousSecteur
        {
            get
            {
                return _SousSecteur;
            }
            set
            {
                _SousSecteur = value;
                OnPropertyChanged("SousSecteur");
            }
        }

        private List<string> _Gamme;
        public List<string> Gamme
        {
            get
            {
                return _Gamme;
            }
            set
            {
                _Gamme = value;
                OnPropertyChanged("Gamme");
            }
        }

        private List<string> _Report;
        public List<string> Report
        {
            get
            {
                return _Report;
            }
            set
            {
                _Report = value;
                OnPropertyChanged("Report");
            }
        }
    }
}
