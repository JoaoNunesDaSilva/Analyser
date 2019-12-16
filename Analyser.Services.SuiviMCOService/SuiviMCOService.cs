using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Analyser.Services
{
    [Injectable("SuiviMCOService")]
    public class SuiviMCOService : ISuiviMCOService
    {
        ObservableCollection<FilterModel> activeFilters = new ObservableCollection<FilterModel>();
        bool filesLoaded = false;
        IContext context;
        ISuiviMCOModel model;
        public ObservableCollection<MCOData> AllMCOData { get; private set; }
        public ObservableCollection<MCOData> MCOData { get; private set; }
        public ObservableCollection<LookupData> LookupData { get; private set; }

        public SuiviMCOService(IContext context)
        {
            this.context = context;
            MCOData = new ObservableCollection<MCOData>();
            this.LookupData = new ObservableCollection<LookupData>();
        }
        public void LoadDataFromFiles(ISuiviMCO module)
        {
            if (filesLoaded) return;

            model = module.Model;

            FileStream streamLookup = null;
            FileStream streamMCO = null;
            FileStream streamData = null;

            StreamReader readerLookup = null;
            StreamReader readerMCO = null;
            StreamWriter writerData = null;

            try
            {

                streamLookup = new FileStream(model.LookupFile, FileMode.Open, FileAccess.Read);
                streamMCO = new FileStream(model.MCOFile, FileMode.Open, FileAccess.Read);
                streamData = new FileStream(model.DataFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                // Lookup file with all the tickets and max detail
                readerLookup = new StreamReader(streamLookup, Encoding.UTF8, true);
                bool headerline = true;
                while (!readerLookup.EndOfStream)
                {
                    string line = readerLookup.ReadLine();
                    if (headerline)
                    {
                        headerline = false;
                        continue;
                    }
                    ProcessLookupLine(line);
                }
                //module.Model.LookupData = this.LookupData;

                // MCO file filtered in the source for all tickets that matter open or closed
                readerMCO = new StreamReader(streamMCO, Encoding.UTF8, true);
                headerline = true;
                while (!readerMCO.EndOfStream)
                {
                    string line = readerMCO.ReadLine();
                    if (headerline)
                    {
                        headerline = false;
                        continue;
                    }
                    ProcessMCOLine(line);
                }

                model.MCOData = AllMCOData = MCOData;

                // user file with the Analysis data
                writerData = new StreamWriter(streamData, Encoding.UTF8);

                filesLoaded = true;

            }
            catch (Exception) { }
            finally
            {
                readerLookup.Close();
                readerMCO.Close();
                writerData.Close();
            }

            streamLookup.Close();
            streamMCO.Close();
            streamData.Close();
        }
        static Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
        private void ProcessMCOLine(string line)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (Match match in csvSplit.Matches(line))
                    list.Add(match.Value.TrimStart(',').Replace("\"", ""));
                MCOData mco = new MCOData(list.ToArray());
                mco.Ticket = LookupData.First(p => p.NoFiche == mco.NoFiche);
                MCOData.Add(mco);
            }
            catch (Exception) { }
        }
        private void ProcessLookupLine(string line)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (Match match in csvSplit.Matches(line))
                    list.Add(match.Value.TrimStart(',').Replace("\"", ""));
                LookupData.Add(new LookupData(list.ToArray()));
            }
            catch (Exception) { }
        }
        public ObservableCollection<FilterModel> CreateColumnFilters(int colIndex)
        {
            activeFilters = new ObservableCollection<FilterModel>();
            List<string> items = new List<string>();
            switch (colIndex)
            {
                case 0:
                    items.AddRange(MCOData.Select(p => p.NoFiche).Distinct());
                    break;
                case 1:
                    items.AddRange(MCOData.Select(p => p.Priorite).Distinct());
                    break;
                case 2:
                    items.AddRange(MCOData.Select(p => p.Ticket.Gravite).Distinct());
                    break;
                case 3:
                    items.AddRange(MCOData.Select(p => p.Statut).Distinct());
                    break;
                case 4:
                    items.AddRange(MCOData.Select(p => p.Libelle).Distinct());
                    break;
                case 5:
                    items.AddRange(MCOData.Select(p => p.Ticket.DateCreation.HasValue ? p.Ticket.DateCreation.Value.ToString("yyyy-MM-dd") : "").Distinct());
                    break;
                case 6:
                    items.AddRange(MCOData.Select(p => p.Ticket.CreateurDeLaFiche).Distinct());
                    break;
                case 7:
                    items.AddRange(MCOData.Select(p => p.Ticket.Diagnostiqueur).Distinct());
                    break;
                case 8:
                    items.AddRange(MCOData.Select(p => p.Ticket.Correcteur).Distinct());
                    break;
                case 9:
                    items.AddRange(MCOData.Select(p => p.Ticket.Site).Distinct());
                    break;
                case 10:
                    items.AddRange(MCOData.Select(p => p.BlocsAplicatifsACorriger).Distinct());
                    break;
                case 11:
                    items.AddRange(MCOData.Select(p => p.NatureDeLaMaintenance).Distinct());
                    break;
                case 12:
                    items.AddRange(MCOData.Select(p => p.NatureDeLaFiche).Distinct());
                    break;
                case 13:
                    items.AddRange(MCOData.Select(p => p.Version).Distinct());
                    break;
                case 14:
                    items.AddRange(MCOData.Select(p => p.Niveu0).Distinct());
                    break;
                case 15:
                    items.AddRange(MCOData.Select(p => p.Niveu1).Distinct());
                    break;
                case 16:
                    items.AddRange(MCOData.Select(p => p.TypeMaintenance).Distinct());
                    break;
                case 17:
                    items.AddRange(MCOData.Select(p => p.DirectionResponsable).Distinct());
                    break;
                case 18:
                    items.AddRange(MCOData.Select(p => p.SecteurDeRecette).Distinct());
                    break;
                case 19:
                    items.AddRange(MCOData.Select(p => p.DomaineDeDetection).Distinct());
                    break;
                case 20:
                    items.AddRange(MCOData.Select(p => p.Ticket.DomaineCorrection).Distinct());
                    break;
                case 21:
                    items.AddRange(MCOData.Select(p => p.Ticket.SecteurCorrection).Distinct());
                    break;
                case 22:
                    items.AddRange(MCOData.Select(p => p.SousSecteur).Distinct());
                    break;
                case 23:
                    items.AddRange(MCOData.Select(p => p.Gamme).Distinct());
                    break;
                case 24:
                    items.AddRange(MCOData.Select(p => p.Ticket.ModifieLe.HasValue ? p.Ticket.ModifieLe.Value.ToString("yyyy-MM-dd") : "").Distinct());
                    break;
                case 25:
                    items.AddRange(MCOData.Select(p => p.Ticket.FinDeCloture.HasValue ? p.Ticket.FinDeCloture.Value.ToString("yyyy-MM-dd") : "").Distinct());
                    break;
                case 26:
                    items.AddRange(MCOData.Select(p => p.Ticket.DateDeDerniereAffectation.HasValue ? p.Ticket.DateDeDerniereAffectation.Value.ToString("yyyy-MM-dd") : "").Distinct());
                    break;
            }
            items.Sort();
            foreach (string s in items)
                activeFilters.Add(new FilterModel(s));
            return activeFilters;
        }
        public ObservableCollection<MCOData> ApplyMCOFilter(int colIndex, ObservableCollection<FilterModel> activeFilters)
        {
            switch (colIndex)
            {
                case 0:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.NoFiche && x.Checked)));
                    break;
                case 1:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Priorite && x.Checked)));
                    break;
                case 2:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Ticket.Gravite && x.Checked)));
                    break;
                case 3:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Statut && x.Checked)));
                    break;
                case 4:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Libelle && x.Checked)));
                    break;
                case 5:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == (p.Ticket.DateCreation.HasValue ? p.Ticket.DateCreation.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                    break;
                case 6:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Ticket.CreateurDeLaFiche && x.Checked)));
                    break;
                case 7:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Ticket.Diagnostiqueur && x.Checked)));
                    break;
                case 8:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Ticket.Correcteur && x.Checked)));
                    break;
                case 9:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Ticket.Site && x.Checked)));
                    break;
                case 10:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.BlocsAplicatifsACorriger && x.Checked)));
                    break;
                case 11:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.NatureDeLaMaintenance && x.Checked)));
                    break;
                case 12:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.NatureDeLaFiche && x.Checked)));
                    break;
                case 13:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Version && x.Checked)));
                    break;
                case 14:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Niveu0 && x.Checked)));
                    break;
                case 15:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Niveu1 && x.Checked)));
                    break;
                case 16:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.TypeMaintenance && x.Checked)));
                    break;
                case 17:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.DirectionResponsable && x.Checked)));
                    break;
                case 18:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.SecteurDeRecette && x.Checked)));
                    break;
                case 19:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.DomaineDeDetection && x.Checked)));
                    break;
                case 20:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Ticket.DomaineCorrection && x.Checked)));
                    break;
                case 21:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Ticket.SecteurCorrection && x.Checked)));
                    break;
                case 22:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.SousSecteur && x.Checked)));
                    break;
                case 23:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == p.Gamme && x.Checked)));
                    break;
                case 24:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == (p.Ticket.ModifieLe.HasValue ? p.Ticket.ModifieLe.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                    break;
                case 25:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == (p.Ticket.FinDeCloture.HasValue ? p.Ticket.FinDeCloture.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                    break;
                case 26:
                    MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => activeFilters.Any(x => x.Text == (p.Ticket.DateDeDerniereAffectation.HasValue ? p.Ticket.DateDeDerniereAffectation.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                    break;
            }
            return new ObservableCollection<MCOData>(MCOData);
        }
    }
}
