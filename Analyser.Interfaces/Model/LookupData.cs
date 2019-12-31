using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Model
{
    public class LookupData
    {
        public LookupData(string[] data)
        {
            NoFiche = data[0];
            Libelle = data[1];
            Priorite = data[2];
            Gravite = data[3];
            Statut = data[4];
            CreateurDeLaFiche = data[5];
            Site = data[6];
            ModifieLe = !string.IsNullOrEmpty(data[7]) ? DateTime.Parse(data[7]) : null as DateTime?;
            TypeMaintenance = data[8];
            DomaineDetection = data[9];
            DomaineCorrection = data[10];
            SecteurCorrection = data[11];
            DateCreation = !string.IsNullOrEmpty(data[12]) ? DateTime.Parse(data[12]) : null as DateTime?;
            FinDeCloture = !string.IsNullOrEmpty(data[13]) ? DateTime.Parse(data[13]) : null as DateTime?;
            DateDeDerniereAffectation = !string.IsNullOrEmpty(data[14]) ? DateTime.Parse(data[14]) : null as DateTime?;
            Diagnostiqueur = data[15];
            Correcteur = data[16];
            SousSecteur = data[17];
            DocumentUniqueID = data[18];

        }
        public string NoFiche { get; set; }
        public string Libelle { get; set; }
        public string Priorite { get; set; }
        public string Gravite { get; set; }
        public string Statut { get; set; }
        public string CreateurDeLaFiche { get; set; }
        public string Site { get; set; }
        public DateTime? ModifieLe { get; set; }
        public string TypeMaintenance { get; set; }
        public string DomaineDetection { get; set; }
        public string DomaineCorrection { get; set; }
        public string SecteurCorrection { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? FinDeCloture { get; set; }
        public DateTime? DateDeDerniereAffectation { get; set; }
        public string Diagnostiqueur { get; set; }
        public string Correcteur { get; set; }
        public string SousSecteur { get; set; }
        public string DocumentUniqueID { get; set; }
    }
}
