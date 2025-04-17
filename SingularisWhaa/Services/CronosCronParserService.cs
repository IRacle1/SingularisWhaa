using System.Diagnostics.CodeAnalysis;

using Cronos;

using SingularisWhaa.Services.Abstractions;

namespace SingularisWhaa.Services;

/// <inheritdoc/>
public class CronosCronParserService : ICronParserService
{
    private readonly Dictionary<string, CronExpression> cronExpressionsCache = new();

    /// <inheritdoc/>
    public DateTimeOffset? NextOccurrence(string cronExpr, bool localTime)
    {
        if (!cronExpressionsCache.TryGetValue(cronExpr, out CronExpression? expr))
        {
            expr = CronExpression.Parse(cronExpr);
            cronExpressionsCache.Add(cronExpr, expr);
        }

        TimeZoneInfo targetZone = localTime ? TimeZoneInfo.Local : TimeZoneInfo.Utc;

        return cronExpressionsCache[cronExpr].GetNextOccurrence(DateTimeOffset.UtcNow, targetZone);
    }

    /// <inheritdoc/>
    public TimeSpan? NextOccurrenceDelta(string cronExpr)
    {
        DateTimeOffset? dateTime = NextOccurrence(cronExpr, true);
        if (dateTime == null)
            return null;

        return dateTime - DateTimeOffset.Now;
    }

    /// <inheritdoc/>
    public bool TryNextOccurrence(string cronExpr, bool localTime, [NotNullWhen(true)] out DateTimeOffset? dateTime)
    {
        dateTime = NextOccurrence(cronExpr, localTime);
        return dateTime is not null;
    }

    /// <inheritdoc/>
    public bool TryNextOccurrenceDelta(string cronExpr, [NotNullWhen(true)] out TimeSpan? timeSpan)
    {
        if (!TryNextOccurrence(cronExpr, true, out DateTimeOffset? dateTime))
        {
            timeSpan = null;
            return false;
        }

        timeSpan = dateTime - DateTimeOffset.Now;
        return true;
    }
}
