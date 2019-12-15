using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Analyser.Core
{

    [Injectable("Context")]
    public class Context : IContext
    {
        ObjectFactory refl;
        public IShell Shell { get; private set; }
        public Context()
        {
            Views = new Dictionary<string, ObjectImpl>();
            Modules = new Dictionary<string, ObjectImpl>();
            Services = new Dictionary<string, ObjectImpl>();
            ServiceInstances = new Dictionary<string, IService>();
            ModuleInstances = new Dictionary<string, IModule>();
            ViewInstances = new Dictionary<string, IView>();
            refl = new ObjectFactory(this);
        }
        public Dictionary<string, ObjectImpl> Views { get; private set; }
        public Dictionary<string, ObjectImpl> Modules { get; private set; }
        public Dictionary<string, ObjectImpl> Services { get; private set; }
        public void RegisterView(string name, Type component, Type interf)
        {
            if (!Views.ContainsKey(name))
                Views.Add(name, new ObjectImpl() { oType = component, iType = interf });
        }
        public void RegisterModule(string name, Type module, Type interf)
        {
            if (!Modules.ContainsKey(name))
                Modules.Add(name, new ObjectImpl() { oType = module, iType = interf });
        }
        public void RegisterService(string name, Type service, Type interf)
        {
            if (!Services.ContainsKey(name))
                Services.Add(name, new ObjectImpl() { oType = service, iType = interf });
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
            // Boot up all the modules and register their views
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
            if (ServiceInstances.ContainsKey(name))
                return ServiceInstances[name];
            ObjectImpl impl = this.Services[name];
            IService service = (IService)refl.CreateInstance(impl);
            ServiceInstances.Add(name, service);
            return service;
        }
        internal IModule CreateModule(string name)
        {
            if (ModuleInstances.ContainsKey(name))
                return ModuleInstances[name];
            ObjectImpl impl = this.Modules[name];
            IModule module = (IModule)refl.CreateInstance(impl);
            ModuleInstances.Add(name, module);
            return module;
        }
        internal IView CreateView(string name)
        {
            if (ViewInstances.ContainsKey(name))
                return ViewInstances[name];
            ObjectImpl impl = this.Views[name];
            IView view = (IView)refl.CreateInstance(impl);
            ViewInstances.Add(name, view);
            return view;
        }
    }
}
