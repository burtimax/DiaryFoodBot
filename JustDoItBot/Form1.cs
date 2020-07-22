using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using BotLibrary.Classes.Controller;
using BotLibrary.Classes.Controller;
using FoodDiaryBot.Source.DbInteractionMethods;
using Telegram.Bot;


namespace FoodDiaryBot
{
    public partial class BotForm : Form
    {
        private BotController botController;

        public BotForm()
        {
            InitializeComponent();
            string token = null;
            try
            {
                token = File.ReadAllText(Directory.GetCurrentDirectory() + "\\token.txt");
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new Exception("Cant read token from file token.txt");
                }
            }
            catch
            {
                return;
            }

            TelegramBotClient bot = new TelegramBotClient(token);
            ReflectionInfo info = new ReflectionInfo(typeof(BotForm).Assembly, "FoodDiaryBot.Source.ChatStates");
            PullMethods methods = new PullMethods(BotPullMethods.GetCurrentUserState,
                BotPullMethods.CheckUserBeforeMessageProcessing,
                BotPullMethods.ChangeCurrentStateAndMakeHop,
                BotPullMethods.ProcessException,
                BotPullMethods.BotPreProcessMessage,
                BotPullMethods.OnFailUserCheck);

            this.botController = new BotController(bot, info, methods);
        }

        private void StartBotButton_Click(object sender, EventArgs e)
        {
            this.botController.StartBot();
        }

        private void StopBotButton_Click(object sender, EventArgs e)
        {
            this.botController.StopBot();
        }

        private void BotForm_Load(object sender, EventArgs e)
        {
            this.botController.StartBot();
        }
    }
}
