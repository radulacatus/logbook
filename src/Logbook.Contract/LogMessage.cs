using System;

namespace Logbook.Contract
{
    public class LogMessage : ICommand
    {
        public LogLevel LogLevel { get; set; }
        public string CorrelationId { get; set; }
        public string[] Tags { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
