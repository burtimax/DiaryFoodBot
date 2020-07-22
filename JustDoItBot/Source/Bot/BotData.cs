using System;
using System.Collections.Generic;
using System.Text;
using BotLibrary.Classes.Bot;
using Telegram.Bot;

namespace BotLibrary.Classes.Controller
{
    /// <summary>
    /// Хранит все данные по боту, список активных состояний, ссылка на клавиатуры, ответы и прочее.
    /// Вся информация, которой пользуется бот.
    /// </summary>
    public class BotData
    {
        public TelegramBotClient TelegramClient { get; private set; }

        /// <summary>
        /// Хранит все состояния бота
        /// </summary>
        public List<State> States;

        /// <summary>
        /// Конструктор
        /// </summary>
        private BotData()
        {
            this.States = new List<State>();
        }

        public BotData(TelegramBotClient bot) : this()
        {
            this.TelegramClient = bot;

        }
    }
}
