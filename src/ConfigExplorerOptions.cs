namespace SodaPop.ConfigExplorer
{
    public class ConfigExplorerOptions
    {
        public string PathMatch { get; set; } = "/config";
        public bool LocalHostOnly { get; set; } = true;
        public bool TryRedactConnectionStrings { get; set; } = true;
    }
}
