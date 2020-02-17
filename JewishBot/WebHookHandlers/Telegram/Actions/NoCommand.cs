namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Threading.Tasks;

    internal class NoCommand : IAction
    {
        public Task HandleAsync()
        {
            return null;
        }
    }
}