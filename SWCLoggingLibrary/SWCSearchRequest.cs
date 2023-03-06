using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    public class SWCSearchRequest
    {
        private int? noOfRcordsToFetch = null;

        public string LoggingType { get; set; }
        public string Fields { get; set; }
        public string TextToSearch { get; set; }
        public string TimeRange { get; set; }
        public int NoOfRcordsToFetch
        {
            get
            {
                return noOfRcordsToFetch.HasValue ? noOfRcordsToFetch.Value : Int32.MaxValue;
            }
            set
            {
                noOfRcordsToFetch = value;
            }
        }
    }
}
