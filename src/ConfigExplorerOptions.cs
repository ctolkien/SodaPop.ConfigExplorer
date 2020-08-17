namespace SodaPop.ConfigExplorer
{
    public class ConfigExplorerOptions
    {
        /// <summary>
        /// The path to expose this information on. Defaults to "/config"
        /// </summary>
        public string PathMatch { get; set; } = "/config";

        /// <summary>
        /// Will only run when on localhost. Defaults to true
        /// </summary>
        public bool LocalHostOnly { get; set; } = true;

        /// <summary>
        /// Will attempt to redact configuration that looks like connection strings. Defaults to true
        /// </summary>
        public bool TryRedactConnectionStrings { get; set; } = true;
    }
}
