using System;
using System.Collections.Generic;
using System.Text;
using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BotLibrary.Classes.Controller
{
    public abstract class BaseState
    {
        protected State State;

        private BaseState()
        {

        }

        public BaseState(State state) : this()
        {
            this.State = state;
        }

        /// <summary>
        /// Этот метод обрабатывает входящее сообщение
        /// </summary>
        /// <param name="mes"></param>
        /// <returns>Возвращает переход на следующее состояние</returns>
        public abstract Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes);

        public virtual Hop ProcessCallback(TelegramBotClient bot, long chatId, CallbackQuery callback, string data)
        {
            return null;
        }
    }
}
