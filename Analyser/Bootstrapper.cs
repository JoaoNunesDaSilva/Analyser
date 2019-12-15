using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;
using Analyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Analyser
{
    public class Bootstrapper
    {
        IContext context = null;
        List<ManifestItem> manifest = new List<ManifestItem>();
        internal IContext Load()
        {
            LoadManifest();
            LoadContext();
            return context;
        }
        /// <summary>
        /// Build the manifest registry and loads it's assemblies into the appdomain
        /// </summary>
        private void LoadManifest()
        {
            List<string> asmsloaded = new List<string>();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load("Manifest.xml");
            XmlElement root = doc.DocumentElement;
            AppDomainSetup setup = new AppDomainSetup();
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item.NodeType != XmlNodeType.Element) continue;
                else if (item.Name == "assemblies")
                {
                    foreach (XmlNode asm in item.ChildNodes)
                    {
                        if (asm.NodeType != XmlNodeType.Element) continue;
                        ManifestItem mitem = new ManifestItem()
                        {
                            type = TypeEnum.assembly,
                            name = asm.Attributes["name"].Value,
                            assemb = asm.Attributes["assemb"].Value
                        };
                        manifest.Add(mitem);
                        if (asmsloaded.Contains(mitem.assemb)) continue;
                        AppDomain.CurrentDomain.Load(mitem.assemb);
                    }
                }
                else if (item.Name == "singletons")
                {
                    foreach (XmlNode singleton in item.ChildNodes)
                    {
                        if (singleton.NodeType != XmlNodeType.Element) continue;
                        ManifestItem mitem = new ManifestItem()
                        {
                            type = TypeEnum.singleton,
                            name = singleton.Attributes["name"].Value,
                            inter = singleton.Attributes["inter"].Value,
                            impl = singleton.Attributes["impl"].Value,
                            assemb = singleton.Attributes["assemb"].Value,
                            activate = singleton.Attributes.GetNamedItem("activate") != null ? bool.Parse(singleton.Attributes["activate"].Value) : false,

                        };
                        manifest.Add(mitem);
                    }
                }
                else if (item.Name == "injectables")
                {
                    foreach (XmlNode intectable in item.ChildNodes)
                    {
                        if (intectable.NodeType != XmlNodeType.Element) continue;
                        ManifestItem mitem = new ManifestItem()
                        {
                            type = TypeEnum.injectable,
                            name = intectable.Attributes.GetNamedItem("name") != null ? intectable.Attributes["name"].Value : null,
                            _ref = intectable.Attributes.GetNamedItem("ref") != null ? intectable.Attributes["ref"].Value : null,
                            inter = intectable.Attributes.GetNamedItem("inter") != null ? intectable.Attributes["inter"].Value : null,
                            impl = intectable.Attributes.GetNamedItem("impl") != null ? intectable.Attributes["impl"].Value : null,
                            assemb = intectable.Attributes.GetNamedItem("assemb") != null ? intectable.Attributes["assemb"].Value : null,
                            activate = intectable.Attributes.GetNamedItem("activate") != null ? bool.Parse(intectable.Attributes["activate"].Value) : false,
                        };
                        manifest.Add(mitem);
                    }
                }
            }
        }
        /// <summary>
        /// Creates the context singleton and registers all manifest items
        /// </summary>
        /// <returns></returns>
        private void LoadContext()
        {
            ManifestItem contextItem = manifest.First(p => p.name == "Context");
            // instantiate Context Singleton
            context = (IContext)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(contextItem.assemb, contextItem.impl);
            // load from manifest
            foreach (ManifestItem item in manifest.Where(p => p.name != "Context" && p._ref != "Context" && p.type != TypeEnum.assembly))
            {
                Type iType = null;
                Type oType = null;
                if (!string.IsNullOrEmpty(item._ref))
                {
                    if (!context.Services.ContainsKey(item._ref))
                        throw new Exception(string.Concat("Could not resolve reference to ", item._ref));
                    ObjectImpl impl = context.Services[item._ref];
                    iType = impl.iType;
                    oType = impl.oType;
                }
                else
                {
                    iType = Type.GetType(item.inter);
                    oType = Type.GetType(string.Concat(item.impl, ", ", item.assemb));
                }
                if (iType.GetInterfaces().Contains(typeof(IView)))
                    context.RegisterView(item.name, oType, iType);
                else if (iType.GetInterfaces().Contains(typeof(IModule)))
                    context.RegisterModule(item.name ?? item._ref, oType, iType);
                else if (iType.GetInterfaces().Contains(typeof(IService)))
                    context.RegisterService(item.name ?? item._ref, oType, iType);
                else
                    throw new Exception(string.Concat("Could not resolve manifest item ", item.inter));
            }
        }
    }
}
