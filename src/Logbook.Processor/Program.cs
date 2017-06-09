using System;
using Logbook.Contract;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;

namespace Logbook.Processor
{
    internal partial class Program
    {
        private static IServiceProvider _serviceProvider;

        public static void Main(string[] args)
        {
            Register();

            CreateIndexIfNotExists(IndexNames.LogMessage);
            CreateIndexIfNotExists(IndexNames.AnalyticsMessage);

            Subscribe<LogMessage>();
            Subscribe<AnalyticsMessage>();
        }

        private static void Subscribe<T>() where T : ICommand
        {
            var client = _serviceProvider.GetService<IBusClient>();
            client.SubscribeAsync<T>(async (m, c) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetService<ICommandHandler<T>>();
                    await handler.HandleAsync(m);
                }
            });
        }

        private static RawRabbitConfiguration RawRabbitConfiguration()
        {
            var hostname = Environment.GetEnvironmentVariable("LOG_RABBIT_HOST");
            var busConfig = new RawRabbitConfiguration
            {
                Username = "rabbitmq",
                Password = "rabbitmq",
                Port = 5672,
                VirtualHost = "/",
                Hostnames = {hostname}
            };
            return busConfig;
        }

        private static void Register()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddScoped<ICommandHandler<LogMessage>, LogMessageHandler>()
                .AddScoped<ICommandHandler<AnalyticsMessage>, AnalyticsMessageHandler>()
                .AddScoped<IBusClient>(p => BusClientFactory.CreateDefault(RawRabbitConfiguration()))
                .AddScoped<IElasticClient>(p => ElasticClient())
                .BuildServiceProvider();
        }

        private static void CreateIndexIfNotExists(IndexName indexName)
        {
            var client = _serviceProvider.GetService<IElasticClient>();
            var result = client.IndexExists(indexName);
            if (!result.Exists)
                client.CreateIndex(indexName);
        }

        private static ElasticClient ElasticClient()
        {
            var node = new Uri("http://elasticsearch:9200");
            var settings = new ConnectionSettings(node);
            settings.BasicAuthentication("elastic", "changeme");
            var client = new ElasticClient(settings);
            return client;
        }
    }
}