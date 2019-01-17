using System.Xml;

namespace RevitDKTools.Commands.Generate
{
    public interface IXmlPythonScriptsSettingsProvider
    {
        XmlDocument Xml { get; }
    }
}