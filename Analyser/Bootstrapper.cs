using Analyser.Interfaces;
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
        /// Creates the instance of the context singleton 
        /// </summary>
        /// <returns></returns>
        private void LoadContext()
        {
            ManifestItem contextItem = manifest.First(p => p.name == "Context");
            // instantiate
            context = (IContext)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(contextItem.assemb, contextItem.impl);
            // load from manifest
            foreach (ManifestItem item in manifest.Where(p => p.name != "Context" && p._ref != "Context"))
            {

                Type iType = null;
                Type oType = null;

                if (!string.IsNullOrEmpty(item._ref))
                {
                    iType = Type.GetType(item.inter);
                    oType = Type.GetType(string.Concat(item.impl, ", ", item.assemb));
                }
                else
                {
                    iType = Type.GetType(item.inter);
                    oType = Type.GetType(string.Concat(item.impl, ", ", item.assemb));
                }

                if (iType.GetInterfaces().Contains(typeof(IView)))
                    context.RegisterView(item.name, oType);

                if (iType.GetInterfaces().Contains(typeof(IModule)))
                    context.RegisterModule(item.name, oType);

                if (iType.GetInterfaces().Contains(typeof(IService)))
                    context.RegisterService(item.name, oType);
            }
        }
        /// <summary>
        /// Loads the manifest and ites assemblies into the appdomain
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

                if (item.Name == "assemblies")
                {
                    ManifestItem mitem = new ManifestItem()
                    {
                        type = TypeEnum.assembly,
                        name = item["name"].Value,
                        assemb = item["assemb"].Value
                    };

                    manifest.Add(mitem);
                    if (asmsloaded.Contains(mitem.assemb)) continue;
                    AppDomain.CurrentDomain.Load(mitem.assemb);
                    continue;
                }

                if (item.Name == "singletons")
                {
                    ManifestItem mitem = new ManifestItem()
                    {
                        type = TypeEnum.singleton,
                        name = item["name"].Value,
                        inter = item["inter"].Value,
                        impl = item["impl"].Value,
                        activate = bool.Parse(item["activate"].Value)
                    };

                    manifest.Add(mitem);
                    continue;
                }

                if (item.Name == "injectables")
                {
                    ManifestItem mitem = new ManifestItem()
                    {
                        type = TypeEnum.injectable,
                        name = item["name"].Value ?? null,
                        inter = item["inter"].Value ?? null,
                        impl = item["impl"].Value ?? null,
                        _ref = item["ref"].Value ?? null,
                        activate = !string.IsNullOrEmpty(item["activate"].Value) ? bool.Parse(item["activate"].Value) : false
                    };
                    manifest.Add(mitem);
                    continue;
                }

            }
        }
    }
}
