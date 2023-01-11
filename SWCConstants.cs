using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    public static class SWCConstants
    {
        public const string LogLevel = "LogLevel";
        public const string EventId = "EventId";
        public const string Message = "Message";
        public const string Exception = "Exception";
        public const string StackTrace = "StackTrace";
        public const string LogTime = "LogTime";
        public const string CreationDate = "CreationDate";
        public const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
    }
}
