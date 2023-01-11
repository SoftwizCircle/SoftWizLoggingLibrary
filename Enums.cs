using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    public enum LogType
    {
        Critical,
        Debug,
        Error,
        Information,
        Trace,
        Warning
    }

    public enum TimeRange
    {
        All = 0,
        Last_15_minutes = 15,
        Last_30_minutes = 30,
        Last_1_hour = 1,
        Last_4_hours = 4,
        Last_12_hours = 12,
        Last_24_hours = 24,
        Last_7_days = 7,
        Last_30_days = 30,
        Last_60_days = 60,
        Last_90_days = 90,
        Last_6_month = 6,
        Last_1_year = 1,
        Last_2_years = 2,
        Last_5_years = 5
    }
}