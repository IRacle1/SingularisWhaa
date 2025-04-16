using System.Diagnostics.CodeAnalysis;

namespace SingularisWhaa.Services.Abstractions
{
    public interface ICronParserService
    {
        public DateTimeOffset? NextOccurrence(string cronExpr, bool localTime);
        public bool TryNextOccurrence(string cronExpr, bool localTime, [NotNullWhen(true)] out DateTimeOffset? dateTime);

        public TimeSpan? NextOccurrenceDelta(string cronExpr);
        public bool TryNextOccurrenceDelta(string cronExpr, [NotNullWhen(true)] out TimeSpan? dateTime);
    }
}
