using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    public static class SWCExtensions
    {
        public static long AsYMDHMS(this DateTime datetime)
        {
            return
                (datetime.Year * 10000000000) +
                (datetime.Month * 100000000) +
                (datetime.Day * 1000000) +
                (datetime.Hour * 10000) +
                (datetime.Minute * 100) +
                datetime.Second + Convert.ToInt32(datetime.ToString("fff"));
        }
    }
}
