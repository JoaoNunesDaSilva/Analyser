using Analyser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Core.Services
{
    public class SuiviMCOService : ISuiviMCOService
    {
        IContext context;
        public SuiviMCOService(IContext context)
        {
            this.context = context;
        }
    }
}
