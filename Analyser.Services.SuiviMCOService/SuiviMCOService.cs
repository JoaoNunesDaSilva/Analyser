using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;
using Analyser.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Services
{
    [Injectable("SuiviMCOService")]
    public class SuiviMCOService : ISuiviMCOService
    {
        IContext context;
        ISuiviMCOModel model;

        public List<MCOData> MCOData { get; private set; }
        public List<LookupData> LookupData { get; private set; }

        public SuiviMCOService(IContext context)
        {
            this.context = context;
            this.MCOData = new List<MCOData>();
            this.LookupData = new List<LookupData>();
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

            StreamReader readerMCO = new StreamReader(streamLookup, true);
            headerline = true;
            while (!readerMCO.EndOfStream)
            {
                string line = readerLookup.ReadLine();
                if (headerline)
                {
                    headerline = false; 
                    continue;
                }
                ProcessMCOLine(line);
            }

            StreamWriter readerData = new StreamWriter(streamData, Encoding.UTF8);

        }

        private void ProcessMCOLine(string line)
        {
            string[] fields = line.Replace("\"", "").Split(',');
            MCOData.Add(new MCOData(fields));
        }

        private void ProcessLookupLine(string line)
        {
            string[] fields = line.Replace("\"", "").Split(',');
            LookupData.Add(new LookupData(fields));
        }
    }
}
