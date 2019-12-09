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
            foreach (ManifestItem item in manifest.Where(p => p.name != "Context"))
            {
                Type iType = Type.GetType(item.inter);
                Type oType = Type.GetType(string.Concat(item.impl, ", ", item.assemb));

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
                ManifestItem mitem = new ManifestItem(item.Attributes["name"].Value, item.Attributes["inter"].Value, item.Attributes["impl"].Value, item.Attributes["assemb"].Value, bool.Parse(item.Attributes["activate"].Value));
                manifest.Add(mitem);
                // load assembly into the appdomain?
                if (asmsloaded.Contains(mitem.assemb)) continue;
                AppDomain.CurrentDomain.Load(mitem.assemb);
            }
        }
    }
}
