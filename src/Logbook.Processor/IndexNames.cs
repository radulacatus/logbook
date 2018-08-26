using Logbook.Contract;
using Nest;

namespace Logbook.Processor
{
    internal partial class Program
    {
        public static class IndexNames
        {
            public static readonly IndexName LogMessage = "logs";
            public static readonly IndexName AnalyticsMessage = "analytics";
        }
    }
}