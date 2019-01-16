using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RevitDKTools.Commands.Generate
{
    class DefaultSettingsProvider : IEmitterSetting, IXmlPythonScriptsSettingsProvider
    {
        private readonly string _dynamicAssemblName;
        private readonly XmlDocument _xml;

        public string DynamicAssemblName
        {
            get
            {
                return _dynamicAssemblName;
            }
        }

        public XmlDocument Xml
        {
            get
            {
                return _xml;
            }
        }

        public DefaultSettingsProvider()
        {
            ResourceManager resourceManager = new ResourceManager(
                "RevitDKTools.Properties.Resources",
                Assembly.GetExecutingAssembly());

            _dynamicAssemblName = resourceManager.GetString("DYNAMIC_ASSEMBLY_NAME");

            _xml = new XmlDocument();
            _xml.Load(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                      resourceManager.GetString("SCRIPTS_SETTINGS_XML_LOCATION"));
        }
    }
}
