using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Model
{
    public class SuiviData
    {
        public SuiviData()
        {
        }

        public SuiviData(string[] data)
        {
            NoFiche = data[0];
            Commentaires = data[1];
            Status = data[2];
            Responsable = data[3];
            Analyse = !string.IsNullOrEmpty(data[4]) ? DateTime.Parse(data[4]) : null as DateTime?;
            AnalyseFin = !string.IsNullOrEmpty(data[5]) ? DateTime.Parse(data[5]) : null as DateTime?;
            Dev = !string.IsNullOrEmpty(data[6]) ? DateTime.Parse(data[6]) : null as DateTime?;
            DevFin = !string.IsNullOrEmpty(data[7]) ? DateTime.Parse(data[7]) : null as DateTime?;
            Recette = !string.IsNullOrEmpty(data[8]) ? DateTime.Parse(data[8]) : null as DateTime?;
            RecetteFin = !string.IsNullOrEmpty(data[9]) ? DateTime.Parse(data[9]) : null as DateTime?;
            Prod = !string.IsNullOrEmpty(data[10]) ? DateTime.Parse(data[10]) : null as DateTime?;
            Report = data[11] == "Oui" ? true : false;
        }
        public string NoFiche { get; set; }
        public string Commentaires { get; set; }
        public string Status { get; set; }
        public string Responsable { get; set; }
        public DateTime? Analyse { get; set; }
        public DateTime? AnalyseFin { get; set; }
        public DateTime? Dev { get; set; }
        public DateTime? DevFin { get; set; }
        public DateTime? Recette { get; set; }
        public DateTime? RecetteFin { get; set; }
        public DateTime? Prod { get; set; }
        public bool Report { get; set; }
    }
}
