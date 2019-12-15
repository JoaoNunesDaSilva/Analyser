using Analyser.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Analyser.Infrastructure.Interfaces
{
    public interface IContext
    {
        IShell Shell { get; }
        Dictionary<string, ObjectImpl> Views { get; }
        Dictionary<string, ObjectImpl> Modules { get; }
        Dictionary<string, ObjectImpl> Services { get; }
        Dictionary<string, IService> ServiceInstances { get; }
        Dictionary<string, IModule> ModuleInstances { get; }
        Dictionary<string, IView> ViewInstances { get; }
        void RegisterView(string name, Type component, Type interf);
        void RegisterModule(string name, Type module, Type interf);
        void RegisterService(string name, Type service, Type interf);
        void Initialize(Window mainWindow);
        IService GetService(string name);
    }
}
