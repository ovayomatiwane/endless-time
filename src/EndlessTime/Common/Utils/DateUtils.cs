namespace Common.Utils
{
    public static class DateUtils
    {
        public static (DateTime Start, DateTime End) GetUtcDayRange(DateTime utcDateTime)
        {
            var start = utcDateTime.Date;
            var end = start.AddDays(1).AddTicks(-1);

            return (start, end);
        }

        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return dateTime.StartOfDay().AddDays(1).AddTicks(-1);
        }
    }
}
