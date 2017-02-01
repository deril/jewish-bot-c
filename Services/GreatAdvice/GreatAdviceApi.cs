namespace JewishBot.Services.GreatAdvice
{
    public class GreatAdviceApi : ApiService
    {
        private const string BaseUrl = "http://fucking-great-advice.ru/api/random";

        public override string BuildEndpointRoute(string term)
        {
            return BaseUrl;
        }
    }
}