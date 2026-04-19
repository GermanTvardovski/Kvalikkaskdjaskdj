namespace Matie.Data;

/// <summary>
/// Npgsql не записывает <see cref="DateTimeKind.Utc"/> в колонки PostgreSQL <c>timestamp without time zone</c>.
/// </summary>
public static class PostgresDateTime
{
    public static DateTime ForNaiveTimestamp(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => DateTime.SpecifyKind(value, DateTimeKind.Unspecified),
            DateTimeKind.Local => DateTime.SpecifyKind(value.ToUniversalTime(), DateTimeKind.Unspecified),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Unspecified)
        };
    }
}
