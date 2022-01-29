using JewishBot;
using JewishBot.WebHookHandlers.Telegram;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
DependencyInjectionConfig.AddScope(builder.Services, builder.Configuration);
builder.Services.AddSingleton<IBotService, BotService>().AddScoped<WebHookLogger>();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.Configure<BotConfiguration>(builder.Configuration.GetSection("BotConfiguration"));

var app = builder.Build();
app.MapControllers();

app.Run();