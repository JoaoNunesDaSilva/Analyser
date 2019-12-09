using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Analyser.Interfaces
{
    public interface IModule
    {
        MenuItem FileNewMenuItem { get; }
        MenuItem ModuleMenu { get; }
        void DestroyView();
    }
}
