using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Interfaces
{
    public interface ISuiviMCOModel
    {
        string LookupFile { get; set; }
        string MCOFile { get; set; }
        string DataFile { get; set; }
    }
}
