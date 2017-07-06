using System.Threading.Tasks;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	interface IAction
	{
	Task HandleAsync();
	}
}
