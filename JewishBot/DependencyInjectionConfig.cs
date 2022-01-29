using System;
using JewishBot.WebHookHandlers.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JewishBot;

public static class DependencyInjectionConfig
{
    public static void AddScope(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);
        services.AddScoped<IWebHookHandler, WebHookHandler>();
        services.AddHttpClient();
        services.AddHttpClient("urbandictionary",
            c => { c.BaseAddress = new Uri(configuration["ExternalHosts:urbandictionary"]); });
        services.AddHttpClient("duckduckgo",
            c => { c.BaseAddress = new Uri(configuration["ExternalHosts:duckduckgo"]); });
        services.AddHttpClient("googleapis",
            c => { c.BaseAddress = new Uri(configuration["ExternalHosts:googleapis"]); });
        services.AddHttpClient("greatadvice",
            c => { c.BaseAddress = new Uri(configuration["ExternalHosts:greatadvice"]); });
        services.AddHttpClient("mathapi",
            c => { c.BaseAddress = new Uri(configuration["ExternalHosts:mathapi"]); });
        services.AddHttpClient("poemapi",
            c => { c.BaseAddress = new Uri(configuration["ExternalHosts:poemapi"]); });
        services.AddHttpClient("weatherapi",
            c => { c.BaseAddress = new Uri(configuration["ExternalHosts:weatherapi"]); });
    }
}