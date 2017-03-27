using System.Collections.Generic;

namespace AspNetOptionsExplorer
{
    public class Option
    {
        public string Path { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public IEnumerable<Option> Children { get; set; }



    }
}
