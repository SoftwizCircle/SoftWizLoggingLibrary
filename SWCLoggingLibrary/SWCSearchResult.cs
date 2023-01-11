﻿using System;
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
        public string RequestId { get; set; }
        public string RequestPath { get; set; }
        public string SpanId { get; set; }
        public string TraceId { get; set; }
        public string ParentId { get; set; }
    }
}