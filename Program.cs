using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace JewishBot {
    public class Program {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("183899146:AAELl2uvAuSTCyRakQ0kI5zAKeEkI0KNgxQ");

        public static void Main(string[] args) {
            Bot.OnMessage += BotOnMessageReceived;

            Bot.StartReceiving();
            while (Bot.IsReceiving) {}
            Bot.StopReceiving();
        }

        private static void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs) {
            Message message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;

            Command command = new CommandParser(message.Text).Parse();

            new CommandsHandler(Bot).Execute(command, message.Chat.Id);
        }
    }
}