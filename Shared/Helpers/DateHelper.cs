namespace ims.Shared.Helpers
{
    public class DateHelper
    {
        public static DateTime UtcNow() => DateTime.UtcNow;

        public static DateTime AddDaysUtc(int days) => DateTime.UtcNow.AddDays(days);
    }
}
