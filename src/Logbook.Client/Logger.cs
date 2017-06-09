using System;
using System.Linq;
using RawRabbit;

namespace Logbook.Contract
{
    public class Logger<T> : Logger, ILogger<T>
    {
        private readonly string _type;

        public Logger(IBusClient busClient) : base(busClient)
        {
            _type = typeof(T).FullName;
        }

        public override void Log(LogLevel logLevel, string message, string correlationId, string[] tags)
        {
            var typeTagArray = new[] {_type};
            tags = tags == null ? typeTagArray : tags.Union(typeTagArray).ToArray();
            base.Log(logLevel, message, correlationId, tags);
        }
    }

    public class Logger : ILogger
    {
        protected readonly IBusClient BusClient;

        public Logger(IBusClient busClient)
        {
            BusClient = busClient;
        }

        public virtual void Log(LogLevel logLevel, string message, string correlationId, string[] tags)
        {
            BusClient.PublishAsync(new LogMessage
            {
                CorrelationId = correlationId,
                LogLevel = logLevel,
                Message = message,
                Tags = tags,
                Timestamp = DateTime.UtcNow
            });
        }

        public void Debug(string message, string correlationId, string[] tags)
        {
            Log(LogLevel.Debug, message, correlationId, tags);
        }

        public void Info(string message, string correlationId, string[] tags)
        {
            Log(LogLevel.Information, message, correlationId, tags);
        }

        public void Warn(string message, string correlationId, string[] tags)
        {
            Log(LogLevel.Warning, message, correlationId, tags);
        }

        public void Error(string message, string correlationId, string[] tags)
        {
            Log(LogLevel.Error, message, correlationId, tags);
        }

        public void Critical(string message, string correlationId, string[] tags)
        {
            Log(LogLevel.Critical, message, correlationId, tags);
        }
    }

    internal interface ILogger<T> : ILogger
    {
    }

    internal interface ILogger
    {
        void Log(LogLevel logLevel, string message, string correlationId, string[] tags);
        void Debug(string message, string correlationId, string[] tags);
        void Info(string message, string correlationId, string[] tags);
        void Warn(string message, string correlationId, string[] tags);
        void Error(string message, string correlationId, string[] tags);
        void Critical(string message, string correlationId, string[] tags);
    }
}