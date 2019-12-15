using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Model
{
    public class SuiviData
    {
        public SuiviData(string[] data)
        {
            NoFiche = data[0];
            Priorite = data[1];
            Gravite = data[2];
            Domaine = data[3];
            Libelle = data[4];
            Statut = data[5];
            Commentaires = data[6];
            Perimetre = data[7];
            Version = data[8];
            Status = data[9];
            Responsable = data[10];
            Creation = !string.IsNullOrEmpty(data[11]) ? DateTime.Parse(data[11]) : null as DateTime?;
            Affectation = !string.IsNullOrEmpty(data[12]) ? DateTime.Parse(data[12]) : null as DateTime?;
            Cloture = !string.IsNullOrEmpty(data[13]) ? DateTime.Parse(data[13]) : null as DateTime?;
            Dev = !string.IsNullOrEmpty(data[14]) ? DateTime.Parse(data[14]) : null as DateTime?;
            Recette = !string.IsNullOrEmpty(data[15]) ? DateTime.Parse(data[15]) : null as DateTime?;
            Livraison = !string.IsNullOrEmpty(data[16]) ? DateTime.Parse(data[16]) : null as DateTime?;
            Report = data[17] == "Oui" ? true : false;
        }
        public string NoFiche { get; set; }
        public string Priorite { get; set; }
        public string Gravite { get; set; }
        public string Domaine { get; set; }
        public string Libelle { get; set; }
        public string Statut { get; set; }
        public string Commentaires { get; set; }
        public string Perimetre { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public string Responsable { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? Affectation { get; set; }
        public DateTime? Cloture { get; set; }
        public DateTime? Dev { get; set; }
        public DateTime? Recette { get; set; }
        public DateTime? Livraison { get; set; }
        public bool Report { get; set; }
    }
}
