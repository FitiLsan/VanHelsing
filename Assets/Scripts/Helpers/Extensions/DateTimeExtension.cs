using System;


namespace BeastHunter
{
    public static class DateTimeExtension
    {
        private static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTimestamp(this DateTime dateTime) => (long)dateTime.Subtract(EpochTime).TotalSeconds;
        
        public static DateTime FromUnixTimestamp(long unixTimestamp) => EpochTime.AddSeconds(unixTimestamp);
    }
}
