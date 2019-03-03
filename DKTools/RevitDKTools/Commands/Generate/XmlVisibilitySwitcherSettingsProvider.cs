using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RevitDKTools.Commands.Generate
{
    class XmlVisibilitySwitcherSettingsProvider : IXmlVisibilitySwitcherSettingsProvider
    {
        XDocument _xdoc;
        public XDocument XDoc { get { return _xdoc; } }

        public XmlVisibilitySwitcherSettingsProvider()
        {
            ResourceManager resourceManager = new ResourceManager(
                "RevitDKTools.Properties.Resources",
                Assembly.GetExecutingAssembly());

            _xdoc = XDocument.Load(
                Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\",string.Empty) +
                    resourceManager.GetString("VISIBILITY_SWITCHER_XML_LOCATION"));
        }
    }
}
