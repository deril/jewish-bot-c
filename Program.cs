using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace JewishBot
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static TelegramBotClient Bot { get; set; }

        public static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                Configuration = builder.Build();
            }
            catch (FileNotFoundException e)
            {
                System.Console.WriteLine(e.Message);
                return;
            }

            Bot = new TelegramBotClient(Configuration["telegramBotApi"]);

            Bot.OnMessage += BotOnMessageReceived;

            Bot.StartReceiving();
            while (Bot.IsReceiving)
            {
                Thread.Sleep(500);
            }
            Bot.StopReceiving();
        }

        private static void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;

            var command = new CommandParser(message.Text).Parse();

            new CommandsHandler(Bot, message).Execute(command);
        }
    }
}