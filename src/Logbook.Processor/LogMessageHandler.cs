using System.Threading.Tasks;
using Logbook.Contract;
using Nest;

namespace Logbook.Processor
{
    internal class LogMessageHandler : ICommandHandler<LogMessage>
    {
        private readonly IElasticClient _elasticClient;

        public LogMessageHandler(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task HandleAsync(LogMessage command)
        {
            var indexResponse = await _elasticClient.IndexAsync(command, x => x.Index(Program.IndexNames.LogMessage));
        }
    }
}