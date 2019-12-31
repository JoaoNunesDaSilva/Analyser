using Analyser.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Interfaces
{
    public interface ISuiviMCOModel
    {
        string LookupFile { get; set; }
        string MCOFile { get; set; }
        string MCOFileEspaceClient { get; set; }
        ObservableCollection<MCOData> MCOData { get; set; }
        ObservableCollection<SuiviData> SuiviData { get; set; }
        ObservableCollection<MCOData> MCODataEspaceClient { get; set; }
        ObservableCollection<LookupData> LookupData { get; set; }
        string DataFile { get; set; }
        ObservableCollection<FilterModel> Filters { get; set; }
        int NroFiches { get; }
        // Lookups
        List<string> Priorite { get; set; }
        string SelectedPriorite { get; set; }
        List<string> Gravite { get; set; }
        string SelectedGravite { get; set; }
        List<string> Statut { get; set; }
        string SelectedStatut { get; set; }
        List<string> Createur { get; set; }
        string SelectedCreateur { get; set; }
        List<string> Diagnostiqueur { get; set; }
        string SelectedDiagnostiqueur { get; set; }
        List<string> Correcteur { get; set; }
        string SelectedCorrecteur { get; set; }
        List<string> Site { get; set; }
        string SelectedSite { get; set; }
        List<string> Responsable { get; set; }
        string SelectedResponsable { get; set; }
        List<string> BlocsAplicatifsACorriger { get; set; }
        string SelectedBlocsAplicatifsACorriger { get; set; }
        List<string> NatureDeLaMaintenance { get; set; }
        string SelectedNatureDeLaMaintenance { get; set; }
        List<string> NatureDeLaFiche { get; set; }
        string SelectedNatureDeLaFiche { get; set; }
        List<string> Version { get; set; }
        string SelectedVersion { get; set; }
        List<string> TypeMaintenance { get; set; }
        string SelectedTypeMaintenance { get; set; }
        List<string> DirectionResponsable { get; set; }
        string SelectedDirectionResponsable { get; set; }
        List<string> SecteurDeRecette { get; set; }
        string SelectedSecteurDeRecette { get; set; }
        List<string> DomaineDeDetection { get; set; }
        string SelectedDomaineDeDetection { get; set; }
        List<string> DomaineCorrection { get; set; }
        string SelectedDomaineCorrection { get; set; }
        List<string> SecteurCorrection { get; set; }
        string SelectedSecteurCorrection { get; set; }
        List<string> SousSecteur { get; set; }
        string SelectedSousSecteur { get; set; }
        List<string> Gamme { get; set; }
        string SelectedGamme { get; set; }
        List<string> Report { get; set; }
        string SelectedReport { get; set; }
    }
}
