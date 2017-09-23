namespace JewishBot.WebHookHandlers.Telegram.Services.GreatAdvice
{
    public class GreatAdviceApi : ApiService
    {
        const string BaseUrl = "http://fucking-great-advice.ru/api/random";

        public override string BuildEndpointRoute(string argument)
        {
            return BaseUrl;
        }
    }
}