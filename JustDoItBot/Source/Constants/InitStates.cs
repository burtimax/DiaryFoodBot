using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes.Bot;
using JustDoItBot.Source.Constants;

namespace FoodDiaryBot.Source.Constants
{
    public class InitStates
    {
        /// <summary>
        /// Возвращаем состояние по имени из BotStates
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static State GetStateByName(string name)
        {
            if (BotStates?.Count == 0) return null;

            foreach (var state in BotStates)
            {
                if (string.Equals(state.Name, name, StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    return state;
                }
            }

            return null;
        }

        public static List<State> BotStates = new List<State>()
        {

            #region USER_STATES
            //new State("StartDialog")
            //{
            //    Hops = new List<Hop>()
            //    {
            //        new Hop(){NextStateName = "InputFirstName"}
            //    },

            //    IntroductionString = "Привет!",
            //    HopOnSuccess = new Hop(){NextStateName = "InputFirstName"},
            //},

            new State("InputFirstName")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "InputLastName"}
                },

                IntroductionString = Answer.Introduction + Answer.AskFirstName,
                HopOnSuccess = new Hop(){NextStateName = "InputLastName"},
            },

            new State("InputLastName")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "InputMarathonId"}
                },

                IntroductionString = Answer.AskLastName,
                HopOnSuccess = new Hop(){NextStateName = "InputMarathonId"},
            },

            new State("InputMarathonId")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "StartDialog"},
                },

                IntroductionString = Answer.AskMarathonId,
                HopOnSuccess = new Hop(){NextStateName = "MainUser"},
                HopOnFailure = new Hop(){NextStateName = "InputMarathonId"},
                ReplyKeyboard = Keyboards.ButtonSkipKeyboard,
            },

            new State("MainUser")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "MainUser"},
                },

                IntroductionString = Answer.IntroductionMainMenu,
                HopOnSuccess = new Hop(){NextStateName = "MainUser"},
                ReplyKeyboard = Keyboards.MainUserKeyboard,
            },

            new State("SetFood")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "MainUser"},
                },

                IntroductionString = Answer.AskFood,
                HopOnSuccess = new Hop(){NextStateName = "MainUser"},
                HopOnFailure = new Hop(){NextStateName = "MainUser"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("SetWeight")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "MainUser"},
                },

                IntroductionString = Answer.AskWeight,
                HopOnSuccess = new Hop(){NextStateName = "MainUser"},
                HopOnFailure = new Hop(){NextStateName = "MainUser"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("SetWater")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "MainUser"},
                },

                IntroductionString = Answer.AskWater,
                HopOnSuccess = new Hop(){NextStateName = "MainUser"},
                HopOnFailure = new Hop(){NextStateName = "MainUser"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("SetSettings")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "MainUser"},
                },

                IntroductionString = Answer.IntroductionSettings,
                HopOnSuccess = new Hop(){NextStateName = "MainUser"},
                HopOnFailure = new Hop(){NextStateName = "MainUser"},
                ReplyKeyboard = Keyboards.SettingsUserKeyboard,
            },

            new State("ChangeFirstName")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "SetSettings"},
                },

                IntroductionString = Answer.IntroductionChangeFirstName,
                HopOnSuccess = new Hop(){NextStateName = "SetSettings"},
                HopOnFailure = new Hop(){NextStateName = "SetSettings"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("ChangeLastName")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "SetSettings"},
                },

                IntroductionString = Answer.IntroductionChangeLastName,
                HopOnSuccess = new Hop(){NextStateName = "SetSettings"},
                HopOnFailure = new Hop(){NextStateName = "SetSettings"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("ChangeMarathon")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "SetSettings"},
                },

                IntroductionString = Answer.IntroductionChangeMarathon,
                HopOnSuccess = new Hop(){NextStateName = "SetSettings"},
                HopOnFailure = new Hop(){NextStateName = "SetSettings"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("SupportService")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "SetSettings"},
                },

                IntroductionString = Answer.IntroductionToWriteToSupport,
                HopOnSuccess = new Hop(){NextStateName = "SetSettings"},
                HopOnFailure = new Hop(){NextStateName = "SetSettings"},
                ReplyKeyboard = Keyboards.SupportUserKeyboard,
            },

            new State("WriteToSupport")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "SupportService"},
                },

                IntroductionString = Answer.IntroductionToWriteToSupport,
                HopOnSuccess = new Hop(){NextStateName = "SupportService"},
                HopOnFailure = new Hop(){NextStateName = "SupportService"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("ModeratorQuery")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "SupportService"},
                },

                IntroductionString = Answer.IntroductionToModeratorQuery,
                HopOnSuccess = new Hop(){NextStateName = "SupportService"},
                HopOnFailure = new Hop(){NextStateName = "SupportService"},
                ReplyKeyboard = Keyboards.KeyboardYesNo,
            },

            new State("WriteToConsultant")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "SetSettings"},
                },

                IntroductionString = Answer.IntroductionToWriteToConsultantDefault,
                HopOnSuccess = new Hop(){NextStateName = "SetSettings"},
                HopOnFailure = new Hop(){NextStateName = "SetSettings"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("UserStatistics")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "MainUser"},
                },

                IntroductionString = Answer.IntroductionToUserStatistics,
                HopOnSuccess = new Hop(){NextStateName = "MainUser"},
                HopOnFailure = new Hop(){NextStateName = "MainUser"},
                ReplyKeyboard = Keyboards.StatisticsUserKeyboard,
            },
            #endregion


            #region MODERATOR_STATES
            new State("ModeratorMain")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "ModeratorMain"},
                },

                IntroductionString = Answer.IntroductionToModeratorMain,
                HopOnSuccess = new Hop(){NextStateName = "ModeratorMain"},
                HopOnFailure = new Hop(){NextStateName = "ModeratorMain"},
                ReplyKeyboard = Keyboards.ModeratorMainKeyboard,
            },

            new State("ModeratorMarathons")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "ModeratorMarathons"},
                },

                IntroductionString = Answer.IntroductionToModeratorMarathons,
                HopOnSuccess = new Hop(){NextStateName = "ModeratorMarathons"},
                HopOnFailure = new Hop(){NextStateName = "ModeratorMarathons"},
                ReplyKeyboard = Keyboards.ModeratorMarathonKeyboard,
            },

            new State("ModeratorAddMarathon")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "ModeratorMarathons"},
                },

                IntroductionString = Answer.IntroductionToModeratorAddMarathons,
                HopOnSuccess = new Hop(){NextStateName = "ModeratorMarathons"},
                HopOnFailure = new Hop(){NextStateName = "ModeratorMarathons"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("ModeratorEditMarathonName")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "ModeratorMarathons"},
                },

                IntroductionString = Answer.IntroductionToModeratorEditMarathonName,
                HopOnSuccess = new Hop(){NextStateName = "ModeratorMarathons"},
                HopOnFailure = new Hop(){NextStateName = "ModeratorMarathons"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("ModeratorDeleteMarathon")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "ModeratorMarathons"},
                },

                IntroductionString = Answer.IntroductionToModeratorDeleteMarathon,
                HopOnSuccess = new Hop(){NextStateName = "ModeratorMarathons"},
                HopOnFailure = new Hop(){NextStateName = "ModeratorMarathons"},
                ReplyKeyboard = Keyboards.KeyboardYesNo,
            },

            new State("ModeratorWritePanel")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "ModeratorMain"},
                },

                IntroductionString = Answer.IntroductionToModeratorWritePanel,
                HopOnSuccess = new Hop(){NextStateName = "ModeratorMain"},
                HopOnFailure = new Hop(){NextStateName = "ModeratorMain"},
                ReplyKeyboard = Keyboards.ModeratorWritePanelKeyboard,
            },

            new State("ModeratorWriteAllUsers")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "ModeratorWritePanel"},
                },

                IntroductionString = Answer.IntroductionToModeratorWriteAllUsers,
                HopOnSuccess = new Hop(){NextStateName = "ModeratorWritePanel"},
                HopOnFailure = new Hop(){NextStateName = "ModeratorWritePanel"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("ModeratorWriteMarathonUsers")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "ModeratorWritePanel"},
                },

                IntroductionString = Answer.IntroductionToModeratorWriteMarathonUsers,
                HopOnSuccess = new Hop(){NextStateName = "ModeratorWritePanel"},
                HopOnFailure = new Hop(){NextStateName = "ModeratorWritePanel"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },

            new State("ModeratorWriteUser")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "ModeratorWritePanel"},
                },

                IntroductionString = Answer.IntroductionToModeratorWriteUser,
                HopOnSuccess = new Hop(){NextStateName = "ModeratorWritePanel"},
                HopOnFailure = new Hop(){NextStateName = "ModeratorWritePanel"},
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },


            #endregion






            #region ADMIN_STATES
            new State("AdminMain")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "MainUser"},
                },

                IntroductionString = Answer.IntroductionToAdminMain,
                HopOnSuccess = new Hop(){NextStateName = "MainUser"},
                HopOnFailure = new Hop(){NextStateName = "MainUser"},
                ReplyKeyboard = Keyboards.AdminMainKeyboard,
            },
            #endregion
            

            new State("AnswerToUserMessage")
            {
                Hops = new List<Hop>()
                {
                    new Hop(){NextStateName = "MainUser"},
                },

                IntroductionString = Answer.AskInputMessage,
                HopOnSuccess = new Hop()
                {
                    NextStateName = "MainUser",
                    Type = HopType.BackToPreviosLevelHop,
                },
                HopOnFailure = new Hop()
                {
                    NextStateName = "MainUser",
                    Type = HopType.BackToPreviosLevelHop,
                },
                ReplyKeyboard = Keyboards.CancelOperationKeyboard,
            },
        };
    }
}
