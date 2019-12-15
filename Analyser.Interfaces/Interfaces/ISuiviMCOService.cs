using Analyser.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Interfaces
{
    public interface ISuiviMCOService : IService
    {
        ObservableCollection<MCOData> MCOData { get; }
        ObservableCollection<LookupData> LookupData { get; }
        void LoadDataFromFiles(ISuiviMCO module);
    }
}
