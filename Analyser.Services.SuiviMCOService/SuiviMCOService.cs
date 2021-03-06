﻿using Analyser.Infrastructure.Interfaces;
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
        #region fields
        static Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
        Dictionary<int, ObservableCollection<FilterModel>> activeFilters = new Dictionary<int, ObservableCollection<FilterModel>>();
        ObservableCollection<FilterModel> currentFilterValues = new ObservableCollection<FilterModel>();
        IContext context;
        ISuiviMCO module;
        #endregion

        #region properties  
        public ObservableCollection<MCOData> AllMCOData { get; private set; }
        public ObservableCollection<MCOData> MCOData { get; private set; }
        public ObservableCollection<LookupData> LookupData { get; private set; }
        public ObservableCollection<SuiviData> SuiviData { get; private set; }
        #endregion

        #region  ctor
        public SuiviMCOService(IContext context)
        {
            this.context = context;
           
        }
        #endregion

        #region data loading
        public void LoadDataFromFiles(ISuiviMCO module)
        {
            // initialize all collections 
            AllMCOData = new ObservableCollection<MCOData>();
            MCOData = new ObservableCollection<MCOData>();
            LookupData = new ObservableCollection<LookupData>();
            SuiviData = new ObservableCollection<SuiviData>();

            this.module =  module;

            FileStream streamLookup = null;
            FileStream streamMCO = null;
            FileStream streamMCOEspaceClient = null;
            FileStream streamData = null;

            StreamReader readerLookup = null;
            StreamReader readerMCO = null;
            StreamReader readerMCOEspaceClient = null;
            StreamReader readerData = null;

            try
            {

                streamLookup = new FileStream(this.module.Model.LookupFile, FileMode.Open, FileAccess.Read);
                streamMCO = new FileStream(this.module.Model.MCOFile, FileMode.Open, FileAccess.Read);
                streamMCOEspaceClient = new FileStream(this.module.Model.MCOFileEspaceClient, FileMode.Open, FileAccess.Read);
                streamData = new FileStream(this.module.Model.DataFile, FileMode.OpenOrCreate, FileAccess.Read);

                // Lookup file with all the tickets and max detail
                readerLookup = new StreamReader(streamLookup, Encoding.Default, true);
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
                //this.module.Model.LookupData = this.LookupData;

                // Suivi file with all user customized data
                readerData = new StreamReader(streamData, Encoding.Default, true);
                headerline = true;
                while (!readerData.EndOfStream)
                {
                    string line = readerData.ReadLine();
                    if (headerline)
                    {
                        headerline = false;
                        continue;
                    }
                    ProcessSuiviLine(line);
                }

                this.module.Model.SuiviData = SuiviData;

                // MCO file filtered in the source for all tickets that matter open or closed
                readerMCO = new StreamReader(streamMCO, Encoding.Default, true);
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


                // MCO EspaceClient file filtered in the source for all tickets that matter open or closed
                readerMCOEspaceClient = new StreamReader(streamMCOEspaceClient, Encoding.Default, true);
                headerline = true;
                while (!readerMCOEspaceClient.EndOfStream)
                {
                    string line = readerMCOEspaceClient.ReadLine();
                    if (headerline)
                    {
                        headerline = false;
                        continue;
                    }
                    ProcessMCOLine(line);
                }

                // sort collection
                MCOData.OrderByDescending(p => p.NoFiche);

                this.module.Model.MCOData = AllMCOData = MCOData;

                // Populate grid column templates
                LoadLookups();
            }
            catch (Exception) { }
            finally
            {
                readerLookup.Close();
                readerMCO.Close();
                readerData.Close();
            }

            streamLookup.Close();
            streamMCO.Close();
            streamData.Close();
        }
        private void ProcessMCOLine(string line)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (Match match in csvSplit.Matches(line))
                    list.Add(match.Value.TrimStart(',').Replace("\"", ""));
                MCOData mco = new MCOData(list.ToArray());
                mco.Ticket = LookupData.First(p => p.NoFiche == mco.NoFiche);
                mco.Traitement = SuiviData.FirstOrDefault(p => p.NoFiche == mco.NoFiche) ?? new SuiviData();
                MCOData.Add(mco);
            }
            catch (Exception) { }
        }
        private void ProcessSuiviLine(string line)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (Match match in csvSplit.Matches(line))
                    list.Add(match.Value.TrimStart(',').Replace("\"", ""));
                SuiviData.Add(new SuiviData(list.ToArray()));
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
        #endregion

        #region filtering mechanism
        public ObservableCollection<FilterModel> CreateColumnFilters(int colIndex)
        {
            currentFilterValues = new ObservableCollection<FilterModel>();
            List<string> items = new List<string>();
            switch (colIndex)
            {
                case 0:
                    items.AddRange(AllMCOData.Select(p => p.NoFiche).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 1:
                    items.AddRange(AllMCOData.Select(p => p.Priorite).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 2:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.Gravite).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 3:
                    items.AddRange(AllMCOData.Select(p => p.Statut).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 4:
                    items.AddRange(AllMCOData.Select(p => p.Libelle).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 5:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.DateCreation.HasValue ? p.Ticket.DateCreation.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 6:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.CreateurDeLaFiche).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 7:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.Diagnostiqueur).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 8:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.Correcteur).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 9:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.Site).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 10:
                    items.AddRange(AllMCOData.Select(p => p.Traitement.Responsable).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 11:
                    items.AddRange(AllMCOData.Select(p => p.Traitement.Analyse.HasValue ? p.Traitement.Analyse.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 12:
                    items.AddRange(AllMCOData.Select(p => p.Traitement.AnalyseFin.HasValue ? p.Traitement.AnalyseFin.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 13:
                    items.AddRange(AllMCOData.Select(p => p.Traitement.Dev.HasValue ? p.Traitement.Dev.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 14:
                    items.AddRange(AllMCOData.Select(p => p.Traitement.DevFin.HasValue ? p.Traitement.DevFin.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 15:
                    items.AddRange(AllMCOData.Select(p => p.Traitement.Recette.HasValue ? p.Traitement.Recette.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 16:
                    items.AddRange(AllMCOData.Select(p => p.Traitement.RecetteFin.HasValue ? p.Traitement.RecetteFin.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 17:
                    items.AddRange(AllMCOData.Select(p => p.Traitement.Prod.HasValue ? p.Traitement.Prod.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 18:
                    items.AddRange(AllMCOData.Select(p => p.BlocsAplicatifsACorriger).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 19:
                    items.AddRange(AllMCOData.Select(p => p.NatureDeLaMaintenance).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 20:
                    items.AddRange(AllMCOData.Select(p => p.NatureDeLaFiche).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 21:
                    items.AddRange(AllMCOData.Select(p => p.Version).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 22:
                    items.AddRange(AllMCOData.Select(p => p.Niveu0).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 23:
                    items.AddRange(AllMCOData.Select(p => p.Niveu1).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 24:
                    items.AddRange(AllMCOData.Select(p => p.TypeMaintenance).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 25:
                    items.AddRange(AllMCOData.Select(p => p.DirectionResponsable).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 26:
                    items.AddRange(AllMCOData.Select(p => p.SecteurDeRecette).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 27:
                    items.AddRange(AllMCOData.Select(p => p.DomaineDeDetection).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 28:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.DomaineCorrection).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 29:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.SecteurCorrection).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 30:
                    items.AddRange(AllMCOData.Select(p => p.SousSecteur).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 31:
                    items.AddRange(AllMCOData.Select(p => p.Gamme).Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 32:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.ModifieLe.HasValue ? p.Ticket.ModifieLe.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 33:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.FinDeCloture.HasValue ? p.Ticket.FinDeCloture.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 34:
                    items.AddRange(AllMCOData.Select(p => p.Ticket.DateDeDerniereAffectation.HasValue ? p.Ticket.DateDeDerniereAffectation.Value.ToString("yyyy-MM-dd") : "").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
                case 35:
                    items.AddRange(AllMCOData.Select(p => p.Traitement.Report ? "Oui" : "Non").Distinct().SkipWhile(p => string.IsNullOrEmpty(p)));
                    break;
            }

            ObservableCollection<FilterModel> filters = new ObservableCollection<FilterModel>();
            bool colIsFiltered = activeFilters.ContainsKey(colIndex);
            if (colIsFiltered)
                filters = activeFilters[colIndex];

            items.Sort();
            foreach (string s in items)
            {
                var model = new FilterModel(s, !colIsFiltered);
                if (colIsFiltered) model.Checked = filters.Any(p => p.Text == s && p.Checked);
                currentFilterValues.Add(model);
            }

            // Ajout option "Vides" par defaut 
            currentFilterValues.Add(new FilterModel("Vides", !colIsFiltered));

            return currentFilterValues;
        }
        public ObservableCollection<MCOData> ApplyMCOFilter(int colIndex, ObservableCollection<FilterModel> filterValues)
        {
            // reset list 
            MCOData = AllMCOData;
            // update or create cache with user selection for next time col is filtered
            if (activeFilters.ContainsKey(colIndex))
                activeFilters[colIndex] = filterValues;
            else
                activeFilters.Add(colIndex, filterValues);

            foreach (int i in activeFilters.Keys)
            {
                ObservableCollection<FilterModel> columnFilter = activeFilters[i];
                switch (i)
                {
                    case 0:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.NoFiche && x.Checked)));
                        break;
                    case 1:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Priorite && x.Checked)));
                        break;
                    case 2:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Ticket.Gravite && x.Checked)));
                        break;
                    case 3:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Statut && x.Checked)));
                        break;
                    case 4:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Libelle && x.Checked)));
                        break;
                    case 5:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Ticket.DateCreation.HasValue ? p.Ticket.DateCreation.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 6:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Ticket.CreateurDeLaFiche && x.Checked)));
                        break;
                    case 7:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Ticket.Diagnostiqueur && x.Checked)));
                        break;
                    case 8:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Ticket.Correcteur && x.Checked)));
                        break;
                    case 9:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Ticket.Site && x.Checked)));
                        break;
                    case 10:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Traitement.Responsable && x.Checked)));
                        break;
                    case 11:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Traitement.Analyse.HasValue ? p.Traitement.Analyse.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 12:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Traitement.AnalyseFin.HasValue ? p.Traitement.AnalyseFin.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 13:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Traitement.Dev.HasValue ? p.Traitement.Dev.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 14:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Traitement.DevFin.HasValue ? p.Traitement.DevFin.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 15:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Traitement.Recette.HasValue ? p.Traitement.Recette.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 16:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Traitement.RecetteFin.HasValue ? p.Traitement.RecetteFin.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 17:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Traitement.Prod.HasValue ? p.Traitement.Prod.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 18:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.BlocsAplicatifsACorriger && x.Checked)));
                        break;
                    case 19:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.NatureDeLaMaintenance && x.Checked)));
                        break;
                    case 20:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.NatureDeLaFiche && x.Checked)));
                        break;
                    case 21:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Version && x.Checked)));
                        break;
                    case 22:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Niveu0 && x.Checked)));
                        break;
                    case 23:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Niveu1 && x.Checked)));
                        break;
                    case 24:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.TypeMaintenance && x.Checked)));
                        break;
                    case 25:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.DirectionResponsable && x.Checked)));
                        break;
                    case 26:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.SecteurDeRecette && x.Checked)));
                        break;
                    case 27:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.DomaineDeDetection && x.Checked)));
                        break;
                    case 28:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Ticket.DomaineCorrection && x.Checked)));
                        break;
                    case 29:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Ticket.SecteurCorrection && x.Checked)));
                        break;
                    case 30:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.SousSecteur && x.Checked)));
                        break;
                    case 31:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == p.Gamme && x.Checked)));
                        break;
                    case 32:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Ticket.ModifieLe.HasValue ? p.Ticket.ModifieLe.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 33:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Ticket.FinDeCloture.HasValue ? p.Ticket.FinDeCloture.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 34:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Ticket.DateDeDerniereAffectation.HasValue ? p.Ticket.DateDeDerniereAffectation.Value.ToString("yyyy-MM-dd") : "") && x.Checked)));
                        break;
                    case 35:
                        MCOData = new ObservableCollection<MCOData>(MCOData.Where(p => columnFilter.Any(x => (x.Text != "Vides" ? x.Text : "") == (p.Traitement.Report ? "Oui" : "Non") && x.Checked)));
                        break;
                }
            }
            return new ObservableCollection<MCOData>(MCOData);
        }
        public ObservableCollection<MCOData> ShowAll()
        {
            if (AllMCOData == null) new ObservableCollection<MCOData>();
            MCOData = AllMCOData;
            activeFilters = new Dictionary<int, ObservableCollection<FilterModel>>();
            return new ObservableCollection<MCOData>(MCOData);
        }
        public void RefreshList()
        {
            LoadDataFromFiles(this.module);
        }
        #endregion

        #region lookups
        private void LoadLookups()
        {
            this.module.Model.Priorite = LookupData.OrderBy(p=>p.Priorite).Select(p => p.Priorite).Distinct().ToList();
            this.module.Model.Gravite = LookupData.OrderBy(p => p.Gravite).Select(p => p.Gravite).Distinct().ToList();
            this.module.Model.Statut = LookupData.OrderBy(p => p.Statut).Select(p => p.Statut).Distinct().ToList();
            this.module.Model.Createur = LookupData.OrderBy(p => p.CreateurDeLaFiche).Select(p => p.CreateurDeLaFiche).Distinct().ToList();
            this.module.Model.Diagnostiqueur = LookupData.OrderBy(p => p.Diagnostiqueur).Select(p => p.Diagnostiqueur).Distinct().ToList();
            this.module.Model.Correcteur = LookupData.OrderBy(p => p.Correcteur).Select(p => p.Correcteur).Distinct().ToList();
            this.module.Model.Site = LookupData.OrderBy(p => p.Site).Select(p => p.Site).Distinct().ToList();
            this.module.Model.Responsable = new List<string>() {
                "",
                "André MACHADO",
                "Catherine BARRAULT",
                "Christophe BODA",
                "Hugo OLIVEIRA",
                "João SILVA",
                "Tomaz SILVA",
                "Wallace DAMIÃO"
            };
            this.module.Model.BlocsAplicatifsACorriger = AllMCOData.OrderBy(p => p.BlocsAplicatifsACorriger).Select(p => p.BlocsAplicatifsACorriger).Distinct().ToList();
            this.module.Model.NatureDeLaMaintenance = AllMCOData.OrderBy(p => p.NatureDeLaMaintenance).Select(p => p.NatureDeLaMaintenance).Distinct().ToList();
            this.module.Model.NatureDeLaFiche = AllMCOData.OrderBy(p => p.NatureDeLaFiche).Select(p => p.NatureDeLaFiche).Distinct().ToList();
            this.module.Model.Version = AllMCOData.OrderBy(p => p.Version).Select(p => p.Version).Distinct().ToList();
            this.module.Model.TypeMaintenance = LookupData.OrderBy(p => p.TypeMaintenance).Select(p => p.TypeMaintenance).Distinct().ToList();
            this.module.Model.DirectionResponsable = AllMCOData.OrderBy(p => p.DirectionResponsable).Select(p => p.DirectionResponsable).Distinct().ToList();
            this.module.Model.SecteurDeRecette = AllMCOData.OrderBy(p => p.SecteurDeRecette).Select(p => p.SecteurDeRecette).Distinct().ToList();
            this.module.Model.DomaineDeDetection = AllMCOData.OrderBy(p => p.DomaineDeDetection).Select(p => p.DomaineDeDetection).Distinct().ToList();
            this.module.Model.DomaineCorrection = LookupData.OrderBy(p => p.DomaineCorrection).Select(p => p.DomaineCorrection).Distinct().ToList();
            this.module.Model.SecteurCorrection = LookupData.OrderBy(p => p.SecteurCorrection).Select(p => p.SecteurCorrection).Distinct().ToList();
            this.module.Model.SousSecteur = AllMCOData.OrderBy(p => p.SousSecteur).Select(p => p.SousSecteur).Distinct().ToList();
            this.module.Model.Gamme = AllMCOData.OrderBy(p => p.Gamme).Select(p => p.Gamme).Distinct().ToList();
            this.module.Model.Report = new List<string>() {
                "",
                "M19B",
                "M20A",
                "M20B",
                "M21A",
                "M21B",
                "M22A",
                "M22B"
            };

            this.module.Model.SelectedResponsable = "João SILVA";

        }
        #endregion
    }
}
