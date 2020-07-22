using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using BotLibrary.Classes.Controller;
using JustDoItBot.Source.ChatStates;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FoodDiaryBot.Source.ChatStates
{
    public class StartDialog : ParentState
    {
        public StartDialog(State state) : base(state)
        {

        }

        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes )
        {
            return this.State?.HopOnSuccess ?? null;
        }
    }
}
