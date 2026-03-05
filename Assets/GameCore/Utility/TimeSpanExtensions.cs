using System;

namespace GameCore.Utility
{
    public static class TimeSpanExtensions
    {
        private const string hm = "h'h 'm'm'";
        private const string mss = "m'm 's's'";
        private const string hhmm = "hh\\:mm";
        private const string mmss = "mm\\:ss";
        private const string hhmmss = "hh\\:mm\\:ss";

        public static string TimeFormatSuffix(this TimeSpan timeSpan)
        {
            var formattedTimeSpan = timeSpan.TotalHours > 1
                ? timeSpan.ToString(hm)
                : timeSpan.ToString(mss);
            return formattedTimeSpan;
        }
        
        public static string TimeFormatSimple(this TimeSpan timeSpan)
        {
            var formattedTimeSpan = timeSpan.TotalHours > 1
                ? timeSpan.ToString(hm)
                : timeSpan.ToString(mmss);
            return formattedTimeSpan;
        }
        
        public static string TimeFormatDigital(this TimeSpan timeSpan)
        {
            var formattedTimeSpan = timeSpan.ToString(hhmmss);
            return formattedTimeSpan;
        }
    }
}