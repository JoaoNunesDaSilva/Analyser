using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Model
{
    public class InjectableAttribute : Attribute
    {
        public string Name { get; set; }
        public InjectableAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
