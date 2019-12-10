using Analyser.Infrastructure;
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
            Views = new Dictionary<string, Implementation>();
            Modules = new Dictionary<string, Implementation>();
            Services = new Dictionary<string, Implementation>();
            ServiceInstances = new Dictionary<string, IService>();
            ModuleInstances = new Dictionary<string, IModule>();
        }
        public Dictionary<string, Implementation> Views { get; private set; }
        public Dictionary<string, Implementation> Modules { get; private set; }
        public Dictionary<string, Implementation> Services { get; private set; }
        public void RegisterView(string name, Type component, Type interf)
        {
            if (!Views.ContainsKey(name))
                Views.Add(name, new Implementation() { oType = component, iType = interf });
        }
        public void RegisterModule(string name, Type module, Type interf)
        {
            if (!Modules.ContainsKey(name))
                Modules.Add(name, new Implementation() { oType = module, iType = interf });
        }
        public void RegisterService(string name, Type service, Type interf)
        {
            if (!Services.ContainsKey(name))
                Services.Add(name, new Implementation() { oType = service, iType = interf });
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
            Implementation impl = this.Services[name];
            Type[] types = ResolveCtorTypes(impl);
            object[] parms = ResolveCtorParams(types);
            IService service = (IService)impl.oType.GetConstructor(types).Invoke(parms);
            ServiceInstances.Add(name, service);
            return service;
        }
        internal void CreateModule(string name)
        {
            Implementation impl = this.Modules[name];
            Type[] types = ResolveCtorTypes(impl);
            object[] parms = ResolveCtorParams(types);
            IModule module = (IModule)impl.oType.GetConstructor(types).Invoke(parms);
            ModuleInstances.Add(name, module);
        }
        internal void CreateView(string name)
        {
            Implementation impl = this.Views[name];
            Type[] types = ResolveCtorTypes(impl);
            object[] parms = ResolveCtorParams(types);
            IView module = (IView)impl.oType.GetConstructor(types).Invoke(parms);
            ViewInstances.Add(name, module);
        }
        internal Type[] ResolveCtorTypes(Implementation impl)
        {
            List<Type> types = new List<Type>();
            List<ConstructorInfo> ctors = new List<ConstructorInfo>();
            ConstructorInfo selectedCtor = null;
            foreach (ConstructorInfo ctor in impl.oType.GetConstructors())
                ctors.Insert(ctor.GetParameters().Count(), ctor);

            bool resolved = false;
            for (int i = ctors.Count(), j = 0; i >= j; i--)
            {
                bool soFar = true;
                ConstructorInfo ctor = ctors[i];
                ParameterInfo[] parms = ctor.GetParameters();
                foreach (ParameterInfo parm in parms)
                {
                    // try to resolve param
                    // if no !soFar, break;
                }
                if (resolved) break;
            }
            return types.Count() > 0 ? types.ToArray() : null;
        }
        private object[] ResolveCtorParams(object[] types)
        {
            List<object> objs = new List<object>();


            return objs.Count() > 0 ? objs.ToArray() : null;
        }
    }
}
