namespace JamilNativeAPI.TimeHelper
{
    public static class GetEastFricanTime
    {
        public static DateTime RetrieveEastFricanTime()
        {
            var eatZone = TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time");

            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, eatZone);
        }

        public static DateTime RetrieveRealTime(DateTime realTime)
        {
            var eatZone = TimeZoneInfo.FindSystemTimeZoneById("Africa/Nairobi");

            return TimeZoneInfo.ConvertTimeFromUtc(realTime, eatZone);
        }

    }
}
