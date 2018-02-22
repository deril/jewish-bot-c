namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Threading.Tasks;

    internal interface IAction
    {
        Task HandleAsync();
    }
}
