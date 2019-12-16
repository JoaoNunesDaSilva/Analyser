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
        ObservableCollection<MCOData> MCOData { get; set; }
        ObservableCollection<LookupData> LookupData { get; set; }
        string DataFile { get; set; }
        ObservableCollection<FilterModel> Filters { get; set; }
    }
}
