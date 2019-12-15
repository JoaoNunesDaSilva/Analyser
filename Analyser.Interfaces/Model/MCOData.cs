using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Model
{

    public class MCOData
    {
        public MCOData(string[] data)
        {
            NoFiche = data[0];
            Priorite = data[1];
            Libelle = data[2];
            BlocsAplicatifsACorriger = data[3];
            NatureDeLaMaintenance = data[4];
            NatureDeLaFiche = data[5];
            Charge = !string.IsNullOrEmpty(data[6]) ? int.Parse(data[6]) : null as int?;
            ChargeValide = data[7] == "Oui" ? true : false;
            ImpactMensuel = data[8] == "Oui" ? true : false;
            ImpactTFA = data[9] == "Oui" ? true : false;
            Version = data[10];
            NoCRST = data[11];
            NoASAEnCours = data[12];
            Niveu0 = data[13];
            Niveu1 = data[14];
            Niveu2 = data[15];
            Statut = data[16];
            DateLivraisonCible = !string.IsNullOrEmpty(data[17]) ? DateTime.Parse(data[17]) : null as DateTime?;
            TypeMaintenance = data[18];
            DirectionResponsable = data[19];
            SecteurDeRecette = data[20];
            DomaineDeDetection = data[21];
            SousSecteur = data[22];
            Gamme = data[23];
        }
        public string NoFiche { get; set; }
        public string Priorite { get; set; }
        public string Libelle { get; set; }
        public string BlocsAplicatifsACorriger { get; set; }
        public string NatureDeLaMaintenance { get; set; }
        public string NatureDeLaFiche { get; set; }
        public int? Charge { get; set; }
        public bool? ChargeValide { get; set; }
        public bool? ImpactMensuel { get; set; }
        public bool? ImpactTFA { get; set; }
        public string Version { get; set; }
        public string NoCRST { get; set; }
        public string NoASAEnCours { get; set; }
        public string Niveu0 { get; set; }
        public string Niveu1 { get; set; }
        public string Niveu2 { get; set; }
        public string Statut { get; set; }
        public DateTime? DateLivraisonCible { get; set; }
        public string TypeMaintenance { get; set; }
        public string DirectionResponsable { get; set; }
        public string SecteurDeRecette { get; set; }
        public string DomaineDeDetection { get; set; }
        public string SousSecteur { get; set; }
        public string Gamme { get; set; }
    }
}
