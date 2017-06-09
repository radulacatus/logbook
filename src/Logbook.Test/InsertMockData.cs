using System;
using System.Collections.Generic;
using System.Linq;
using Logbook.Contract;
using RawRabbit.Configuration;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;
using Xunit;

namespace Logbook.Test
{
    public class InsertMockData
    {
        private static readonly DateTime StartDate = DateTime.UtcNow.AddDays(-7);
        private static readonly DateTime EndDate = DateTime.UtcNow;
        private const int Count = 500;
        private static IEnumerable<DateTime> DateRange
        {
            get
            {
                return Enumerable.Range(0, Count)
                    .Select(
                        idx => StartDate.AddSeconds(EndDate.Subtract(StartDate).TotalSeconds / Count * (idx + 1)));
            }
        }

        [Fact]
        public void AddLogs()
        {
            var rand = new Random(DateTime.UtcNow.Millisecond);

            var client = BusClient();

            foreach (var item in DateRange.Select((d, i) => new {Index = i, Date = d}))
            {
                var logLevel = (LogLevel) (1 + rand.Next()%(int) LogLevel.Critical);
                SendLogMessage(item.Index, client, item.Date, logLevel);
            }
        }

        [Fact]
        public void AddAnalytics()
        {
            var rand = new Random(DateTime.UtcNow.Millisecond);

            var client = BusClient(); 
            
            foreach (var item in DateRange)
            {
                SendAnalyticsMessage((decimal) rand.NextDouble(), client, item.Date, "MocksPerSecond");
                SendAnalyticsMessage((decimal) rand.NextDouble() + 0.2m, client, item.Date, "StubsPerSecond");
            }
        }

        private static void SendLogMessage(int index, IBusClient client, DateTime timestamp, LogLevel logLevel)
        {
            client.PublishAsync(new LogMessage
            {
                CorrelationId = Guid.NewGuid().ToString(),
                LogLevel = logLevel,
                Message = "Some test message" + index,
                Tags = new[] { "InsertMockData" },
                Timestamp = timestamp
            });
        }

        private static void SendAnalyticsMessage(decimal value, IBusClient client, DateTime timestamp, string name)
        {
            client.PublishAsync(new AnalyticsMessage()
            {
                AnalyticsType = AnalyticsType.Duration,
                Name = name,
                Value = value,
                Tags = new[] { "InsertMockData" },
                Timestamp = timestamp
            });
        }

        private static IBusClient BusClient()
        {
            var busConfig = new RawRabbitConfiguration
            {
                Username = "rabbitmq",
                Password = "rabbitmq",
                Port = 5672,
                VirtualHost = "/",
                Hostnames = { "localhost" }
            };
            var client = BusClientFactory.CreateDefault(busConfig);
            return client;
        }
    }
}