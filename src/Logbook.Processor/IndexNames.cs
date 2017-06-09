using Logbook.Contract;
using Nest;

namespace Logbook.Processor
{
    internal partial class Program
    {
        public static class IndexNames
        {
            public static readonly IndexName LogMessage = new IndexName {Name = "logs", Type = typeof(LogMessage)};
            public static readonly IndexName AnalyticsMessage = new IndexName {Name = "analytics", Type = typeof(AnalyticsMessage) };
        }
    }
}