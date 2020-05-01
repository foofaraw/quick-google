using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGoogle
{
    public class SearchModel
    {
        public string Search { get; set; }

        public void RunSearch(string uri)
        {
            System.Diagnostics.Process.Start(uri);
        }
    }
}
