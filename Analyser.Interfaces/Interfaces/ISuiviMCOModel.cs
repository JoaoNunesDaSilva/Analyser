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
        List<string> Gravite { get; set; }
        List<string> Statut { get; set; }
        List<string> Createur { get; set; }
        List<string> Diagnostiqueur { get; set; }
        List<string> Correcteur { get; set; }
        List<string> Site { get; set; }
        List<string> Responsable { get; set; }
        List<string> BlocsAplicatifsACorriger { get; set; }
        List<string> NatureDeLaMaintenance { get; set; }
        List<string> NatureDeLaFiche { get; set; }
        List<string> Version { get; set; }
        List<string> TypeMaintenance { get; set; }
        List<string> DirectionResponsable { get; set; }
        List<string> SecteurDeRecette { get; set; }
        List<string> DomaineDeDetection { get; set; }
        List<string> DomaineCorrection { get; set; }
        List<string> SecteurCorrection { get; set; }
        List<string> SousSecteur { get; set; }
        List<string> Gamme { get; set; }
        List<string> Report { get; set; }
    }
}
