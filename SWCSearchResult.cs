using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    public class SWCSearchResult
    {
        public string LogLevel { get; set; }
        public string EventId { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public string LogTime { get; set; }
        public string CreationDate { get; set; }
    }
}