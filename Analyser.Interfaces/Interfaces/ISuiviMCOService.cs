using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Interfaces
{
    public interface ISuiviMCOService : IService
    {
        void LoadData(ISuiviMCO module);
    }
}
