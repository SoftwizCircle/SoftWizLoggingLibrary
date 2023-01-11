using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    public class SWCSearchResponse
    {
        public SWCSearchResponse()
        {
            SWCSearchResults = new List<SWCSearchResult>();
        }

        public List<SWCSearchResult> SWCSearchResults { get; set; }

        public int TotalScoreDocs { get; set; }
    }
}