using System.Diagnostics.CodeAnalysis;

using Cronos;

using SingularisWhaa.Services.Abstractions;

namespace SingularisWhaa.Services
{
    public class CronosCronParserService : ICronParserService
    {
        private readonly Dictionary<string, CronExpression> cronExpressionsCache = new();

        public DateTimeOffset? NextOccurrence(string cronExpr, bool localTime)
        {
            if (!cronExpressionsCache.TryGetValue(cronExpr, out var expr))
            {
                expr = CronExpression.Parse(cronExpr);
                cronExpressionsCache.Add(cronExpr, expr);
            }

            TimeZoneInfo targetZone = localTime ? TimeZoneInfo.Local : TimeZoneInfo.Utc;

            return cronExpressionsCache[cronExpr].GetNextOccurrence(DateTimeOffset.UtcNow, targetZone);
        }

        public TimeSpan? NextOccurrenceDelta(string cronExpr)
        {
            DateTimeOffset? dateTime = NextOccurrence(cronExpr, true);
            if (dateTime == null)
                return null;

            return dateTime - DateTimeOffset.Now;
        }

        public bool TryNextOccurrence(string cronExpr, bool localTime, [NotNullWhen(true)] out DateTimeOffset? dateTime)
        {
            dateTime = NextOccurrence(cronExpr, localTime);
            return dateTime is not null;
        }

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
}
