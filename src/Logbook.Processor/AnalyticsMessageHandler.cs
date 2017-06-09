using System.Threading.Tasks;
using Logbook.Contract;
using Nest;

namespace Logbook.Processor
{
    internal class AnalyticsMessageHandler : ICommandHandler<AnalyticsMessage>
    {
        private readonly IElasticClient _elasticClient;

        public AnalyticsMessageHandler(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task HandleAsync(AnalyticsMessage command)
        {
            await _elasticClient.IndexAsync(command, x => x.Index(Program.IndexNames.AnalyticsMessage));
        }
    }
}