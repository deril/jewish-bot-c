namespace JewishBot.WebHookHandlers.Telegram.Services.GreatAdvice
{
    public class GreatAdviceApi : ApiServiceJson
    {
        private const string BaseUrl = "http://fucking-great-advice.ru/api/random";

        public override string BuildEndpointRoute(string[] arguments)
        {
            return BaseUrl;
        }
    }
}