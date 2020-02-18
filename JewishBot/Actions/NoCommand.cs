namespace JewishBot.Actions
{
    using System.Threading.Tasks;

    internal class NoCommand : IAction
    {
        public Task HandleAsync()
        {
            return Task.CompletedTask;
        }
    }
}