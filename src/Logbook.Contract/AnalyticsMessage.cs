using System;

namespace Logbook.Contract
{
    public class AnalyticsMessage : ICommand
    {
        public AnalyticsType AnalyticsType { get; set; }
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public string[] Tags { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
