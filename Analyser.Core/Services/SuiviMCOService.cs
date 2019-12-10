using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Core.Services
{
    [Injectable("SuiviMCOService")]
    public class SuiviMCOService : ISuiviMCOService
    {
        IContext context;
        public SuiviMCOService(IContext context)
        {
            this.context = context;
        }

        public void LoadData(ISuiviMCO module)
        {
            
        }
    }
}
