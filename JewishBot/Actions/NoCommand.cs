using System.Threading.Tasks;

namespace JewishBot.Actions;

internal class NoCommand : IAction
{
    public Task HandleAsync()
    {
        return Task.CompletedTask;
    }
}