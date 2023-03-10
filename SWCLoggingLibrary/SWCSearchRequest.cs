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
        private int? pageNumber = null;

        public string LoggingType { get; set; }
        public string Fields { get; set; }
        public string TextToSearch { get; set; }
        public string TimeRange { get; set; }
        public int PageNumber {
            get
            {
                return pageNumber.HasValue ? pageNumber.Value : 1;
            }
            set
            {
                pageNumber = value;
            }
        }
        public int NoOfRcordsToFetch
        {
            get
            {
                return noOfRcordsToFetch.HasValue ? noOfRcordsToFetch.Value : 50;
            }
            set
            {
                noOfRcordsToFetch = value;
            }
        }
    }
}
