using System.Threading.Tasks;

namespace JewishBot.Actions;

internal interface IAction
{
    Task HandleAsync();
}