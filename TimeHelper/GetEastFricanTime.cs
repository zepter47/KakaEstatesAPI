namespace JamilNativeAPI.TimeHelper
{
    public static class GetEastFricanTime
    {
        public static DateTime RetrieveEastFricanTime()
        {
            var eatZone = TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time");

            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, eatZone);
        }
    }
}
