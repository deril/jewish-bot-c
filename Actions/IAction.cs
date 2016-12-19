namespace JewishBot.Actions {
    interface IAction {
        void HandleAsync(long chatId, string[] args = null);
    }
}