using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Analyser.Interfaces
{
    public interface IContext
    {
        IShell Shell { get; }
        Dictionary<string, Type> Views { get; }
        Dictionary<string, Type> Modules { get; }
        Dictionary<string, Type> Services { get; }
        Dictionary<string, IService> ServiceInstances { get; }
        Dictionary<string, IModule> ModuleInstances { get; }
        Dictionary<string, IView> ViewInstances { get; }
        void RegisterView(string name, Type component);
        void RegisterModule(string name, Type component);
        void RegisterService(string name, Type component);
        void Initialize(Window mainWindow);
        IService GetService(string name);
    }
}
