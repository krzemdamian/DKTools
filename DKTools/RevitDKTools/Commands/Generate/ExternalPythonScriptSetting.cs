namespace RevitDKTools.Commands.Generate
{
    public class ExternalPythonScriptSetting : IExternalPythonScriptSetting
    {
        public string Location { get; set; }

        public ExternalPythonScriptSetting()
        {
            Location = string.Empty;
        }
    }
}