using System.Collections.Generic;

namespace SodaPop.ConfigExplorer
{
    public class ConfigurationItem
    {
        public string Path { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public IEnumerable<ConfigurationItem> Children { get; set; }
    }
}
