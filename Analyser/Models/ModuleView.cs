using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Models
{
    public class ModuleView
    {
        public ModuleView(IView view, IModule module)
        {
            this.View = view;
            this.Module = module;
        }
        public IView View { get; set; }
        public IModule Module { get; set; }
    }
}
