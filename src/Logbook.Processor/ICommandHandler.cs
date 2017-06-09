using System.Threading.Tasks;
using Logbook.Contract;

namespace Logbook.Processor
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}