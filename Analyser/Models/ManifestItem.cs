using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Models
{
    public enum TypeEnum
    {
        assembly = 0,
        singleton = 1,
        injectable = 2
    }
    public class ManifestItem
    {
        public TypeEnum type { get; set; }
        public string name { get; set; }
        public string inter { get; set; }
        public string impl { get; set; }
        public string assemb { get; set; }
        public bool activate { get; set; }
        public string _ref { get; set; }

        public ManifestItem()
        {
            this._ref = "";
            this.activate = false;
            this.assemb = "";
            this.impl = "";
            this.inter = "";
            this.name = "";
        }
        public ManifestItem(string name, string inter, string impl, string assemb, bool activate, string _ref)
        {
            this._ref = _ref;
            this.activate = activate;
            this.assemb = assemb;
            this.impl = impl;
            this.inter = inter;
            this.name = name;
        }
    }
}
