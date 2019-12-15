using Analyser.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Infrastructure.Model
{
    public class ObjectFactory
    {
        IContext context;
        public ObjectFactory(IContext context)
        {
            this.context = context;
        }
        public object CreateInstance(ObjectImpl impl)
        {
            List<ObjectImpl> types = ResolveCtorTypes(impl);
            object[] parms = ResolveCtorParams(types);
            return impl.oType.GetConstructor(types.Select(p => p.iType).ToArray()).Invoke(parms);
        }
        public List<ObjectImpl> ResolveCtorTypes(ObjectImpl impl)
        {
            List<ObjectImpl> types = new List<ObjectImpl>();
            List<ConstructorInfo> ctors = new List<ConstructorInfo>();
            // fetch and sort constructors ascendingly by number of parameters.
            foreach (ConstructorInfo ctor in impl.oType.GetConstructors())
                ctors.Add(ctor);
            ctors.Sort((a, b) => { return a.GetParameters().Count().CompareTo(b.GetParameters().Count()); });

            // try to resolve the ctor with the maximum number of parameters.
            // the first one wins
            bool ctorResolved = false;
            for (int i = ctors.Count() - 1, j = 0; i >= j; i--)
            {
                bool paramResolved = true;
                ConstructorInfo ctor = ctors[i];
                ParameterInfo[] parms = ctor.GetParameters();
                foreach (ParameterInfo parm in parms)
                {
                    Type parmType = parm.ParameterType;
                    if (parmType == typeof(IContext))
                    {
                        types.Add(new ObjectImpl() { iType = typeof(IContext), oType = context.GetType() });
                        continue;
                    }
                    // try to resolve param
                    // scan services
                    KeyValuePair<string, ObjectImpl> implSvc = context.Services.FirstOrDefault(p => p.Value.iType == parmType);
                    paramResolved = !string.IsNullOrEmpty(implSvc.Key);
                    if (paramResolved)
                    {
                        types.Add(implSvc.Value);
                        continue;
                    }
                    // scan modules
                    KeyValuePair<string, ObjectImpl> implMod = context.Modules.FirstOrDefault(p => p.Value.iType == parmType);
                    paramResolved = !string.IsNullOrEmpty(implMod.Key);
                    if (paramResolved)
                    {
                        types.Add(implMod.Value);
                        continue;
                    }
                    // scan views
                    KeyValuePair<string, ObjectImpl> implView = context.Views.FirstOrDefault(p => p.Value.iType == parmType);
                    paramResolved = !string.IsNullOrEmpty(implView.Key);
                    if (paramResolved)
                    {
                        types.Add(implView.Value);
                        continue;
                    }
                    // cant resolve this parameter? break and go to next ctor
                    if (!paramResolved) break;
                }
                // piss of as soon as it resolves one of the ctors
                if (paramResolved)
                {
                    ctorResolved = true;
                    break;
                }
                // if not resolved go to next ctor
            }

            if (ctorResolved)
                return types;
            else
                return new List<ObjectImpl>();
        }
        public object[] ResolveCtorParams(List<ObjectImpl> types)
        {
            List<object> instances = new List<object>();
            foreach (ObjectImpl impl in types)
            {
                object instance = null;
                if (impl.iType == typeof(IContext))
                    instance = this.context;
                else if (context.ServiceInstances.Any(p => impl.iType.IsAssignableFrom(p.Value.GetType())))
                    instance = context.ServiceInstances.First(p => impl.iType.IsAssignableFrom(p.Value.GetType())).Value;
                else if (context.ModuleInstances.Any(p => impl.iType.IsAssignableFrom(p.Value.GetType())))
                    instance = context.ModuleInstances.First(p => impl.iType.IsAssignableFrom(p.Value.GetType())).Value;
                else if (context.ViewInstances.Any(p => impl.iType.IsAssignableFrom(p.Value.GetType())))
                    instance = context.ViewInstances.First(p => impl.iType.IsAssignableFrom(p.Value.GetType())).Value;
                else
                {
                    List<ObjectImpl> ctorTypes = ResolveCtorTypes(impl);
                    object[] parms = ResolveCtorParams(ctorTypes);
                    instance = impl.oType.GetConstructor(ctorTypes.Select(p => p.iType).ToArray()).Invoke(parms);
                    InjectableAttribute nameAttr = impl.oType.GetCustomAttribute<InjectableAttribute>();
                    if (instance is IService && !context.ServiceInstances.ContainsKey(nameAttr.Name))
                        context.ServiceInstances.Add(nameAttr.Name, (IService)instance);
                    else if (instance is IModule && !context.ModuleInstances.ContainsKey(nameAttr.Name))
                        context.ModuleInstances.Add(nameAttr.Name, (IModule)instance);
                    else if (instance is IView && !context.ViewInstances.ContainsKey(nameAttr.Name))
                        context.ViewInstances.Add(nameAttr.Name, (IView)instance);
                }
                instances.Add(instance);
            }
            return instances.Count() > 0 ? instances.ToArray() : new object[] { };
        }
    }
}
