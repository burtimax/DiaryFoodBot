using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using BotLibrary.Classes.Controller;
using DbInteractionMethods;
using JustDoItBot.Source.ChatStates;
using JustDoItBot.Source.Constants;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FoodDiaryBot.Source.ChatStates
{
    public class InputFirstName : ParentState
    {
        public InputFirstName(State state) : base(state)
        {

        }

        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes )
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    //Пользоатель прислал имя. Сохранить имя в базу данных
                    DataBase.Models.User user = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
                    user.FirstName = mes.Data as string;
                    Db.SaveChanges();
                    //Переход OnSuccess
                    return this.State.HopOnSuccess;
                    break;
                default:
                    bot.SendTextMessageAsync(mes.ChatId, Answer.FailInputFirstName);
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }

       
    }
}
