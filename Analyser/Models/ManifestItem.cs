using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Models
{
    public class ManifestItem
    {
        public string name { get; set; }
        public string inter { get; set; }
        public string impl { get; set; }
        public string assemb { get; set; }
        public bool activate{ get; set; }

        public ManifestItem(string name, string inter, string impl, string assemb, bool activate)
        {
            this.activate= this.activate;
            this.assemb = assemb;
            this.impl = impl;
            this.inter = inter;
            this.name = name;
        }
    }
}
