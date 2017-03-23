using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace AspNetOptionsExplorer
{
    internal sealed class Dashboard
    {
        public Dashboard(HttpContext context, IEnumerable<Option> options )
        {

        }
    }
}
