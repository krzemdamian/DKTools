using System.Xml;
using System.Xml.Linq;

namespace RevitDKTools.Commands.Generate
{
    public interface IXmlVisibilitySwitcherSettingsProvider
    {
        XDocument XDoc { get; }
    }
}