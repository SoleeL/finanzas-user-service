namespace finanzas_user_service.Utilities;

public static class DateConvertionUtility
{
    public static long GetUnixTimestampMilliseconds(DateTime dateTimeUtc)
    {
        var dateTimeOffset = new DateTimeOffset(dateTimeUtc.ToUniversalTime());
        return dateTimeOffset.ToUnixTimeMilliseconds();
    }
}