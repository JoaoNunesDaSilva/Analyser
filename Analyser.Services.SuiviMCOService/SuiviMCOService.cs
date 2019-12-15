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
        IContext context;
        ISuiviMCOModel model;
        public ObservableCollection<MCOData> MCOData { get; private set; }
        public ObservableCollection<LookupData> LookupData { get; private set; }
        public SuiviMCOService(IContext context)
        {
            this.context = context;
            this.MCOData = new ObservableCollection<MCOData>();
            this.LookupData = new ObservableCollection<LookupData>();
        }
        public void LoadDataFromFiles(ISuiviMCO module)
        {
            model = module.Model;
            FileStream streamLookup = new FileStream(model.LookupFile, FileMode.Open, FileAccess.Read);
            FileStream streamMCO = new FileStream(model.MCOFile, FileMode.Open, FileAccess.Read);
            FileStream streamData = new FileStream(model.DataFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader readerLookup = new StreamReader(streamLookup, true);
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
            StreamReader readerMCO = new StreamReader(streamMCO, true);
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
            module.Model.MCOData = this.MCOData;
            StreamWriter readerData = new StreamWriter(streamData, Encoding.UTF8);
        }
        static Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
        private void ProcessMCOLine(string line)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (Match match in csvSplit.Matches(line))
                    list.Add(match.Value.TrimStart(',').Replace("\"", ""));
                MCOData.Add(new MCOData(list.ToArray()));
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
    }
}
