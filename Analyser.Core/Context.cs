using Analyser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Analyser.Core
{
    public class Context : IContext
    {
        public IShell Shell { get; private set; }
        public Context()
        {
            Views = new Dictionary<string, Type>();
            Modules = new Dictionary<string, Type>();
            Services = new Dictionary<string, Type>();
            ServiceInstances = new Dictionary<string, IService>();
            ModuleInstances = new Dictionary<string, IModule>();
        }
        public Dictionary<string, Type> Views { get; private set; }
        public Dictionary<string, Type> Modules { get; private set; }
        public Dictionary<string, Type> Services { get; private set; }
        public void RegisterView(string name, Type component)
        {
            if (!Views.ContainsKey(name))
                Views.Add(name, component);
        }
        public void RegisterModule(string name, Type module)
        {
            if (!Modules.ContainsKey(name))
                Modules.Add(name, module);
        }
        public void RegisterService(string name, Type service)
        {
            if (!Services.ContainsKey(name))
                Services.Add(name, service);
        }
        public Dictionary<string, IService> ServiceInstances { get; private set; }
        public Dictionary<string, IModule> ModuleInstances { get; private set; }
        public Dictionary<string, IView> ViewInstances { get; private set; }
        public void Initialize(Window mainWindow)
        {
            this.Shell = (IShell)mainWindow;

            // Instantiate register all services
            foreach (string k in this.Services.Keys)
                CreateService(k);

            // Boot up all the modules and register their components
            foreach (string k in this.Modules.Keys)
                CreateModule(k);
        }
        public IService GetService(string name)
        {
            if (this.ServiceInstances.ContainsKey(name))
                return this.ServiceInstances[name];
            return CreateService(name);
        }
        internal IService CreateService(string name)
        {
            Type serviceType = this.Services[name];
            Type[] types = new Type[] { typeof(IContext) };
            object[] parms = new object[] { this };
            IService service = (IService)serviceType.GetConstructor(types).Invoke(parms);
            ServiceInstances.Add(name, service);
            return service;
        }
        internal void CreateModule(string k)
        {
            Type moduleType = this.Modules[k];
            Type[] types = new Type[] { typeof(IContext) };
            object[] parms = new object[] { this };
            IModule module = (IModule)moduleType.GetConstructor(types).Invoke(parms);
            ModuleInstances.Add(k, module);
        }
        internal void CreateView(string k)
        {
            Type viewType = this.Modules[k];
            Type[] types = new Type[] {  };
            IView module = (IView)viewType.GetConstructor(types).Invoke(null);
            ViewInstances.Add(k, module);
        }
    }
}
