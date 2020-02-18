namespace JewishBot.Actions
{
    using System.Threading.Tasks;

    internal interface IAction
    {
        Task HandleAsync();
    }
}
