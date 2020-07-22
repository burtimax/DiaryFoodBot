using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDiaryBot.DataBase.Models;
using JustDoItBot.DataBase.Models;

namespace JustDoItBot.Source.Constants
{
    public class Answer
    {
        public static string NotCommand = "Не понимаю!\nВозможно ты забыл(а) нажать на кнопку.";
        public static string YouAreBlocked = "Служба поддержки заблокировала тебя!\nПоэтому не могу тебе отвечать!";
        public static string MarathonList = "Выбери марафон из списка";
        public static string ChoseMarathonForShowParticipants = "Выберете марафон для просмотра участников";
        public static string ChoseMarathon = "Выбери марафон";
        public static string ChoseUser = "Выбери участника";
        public static string YouHaventMarathons = "Ты не создал(а) ни одного марафона!";
        public static string YouHaventParticipants = "У тебя нет участников марафонов!\nСоздай марафон и пригласи уастников";
        public static string NoData = "Нет данных!";
        public static string IAmConsultantQuery = "ХОЧУ СТАТЬ КОНСУЛЬТАНТОМ!";
        public static string CommandsDescription = $"КОМАНДЫ АДМИНА\n" +
                                                   $"1) /approve [ChatId]\n" +
                                                   $"2) /deny [ChatId]\n" +
                                                   $"3) [ChatId] : [Message]\n";
        public static string NoConsultantQueries = "Нет запросов от пользователей";



        public static string ParticipantListOfMarathon = "Список участников";
        public static string ParticipantListOfConcreteMarathon(Marathon m)
        {
            if (Equals(m, null) == true) return ParticipantListOfMarathon;
            return $"Участники марафона [{m.Name}]";
        }

        public static string NoParticipantsInMarathon = "Нет участников в марафоне!";
        public static string NoParticipantsInConcreteMarathon(Marathon marathon)
        {
            if (Equals(marathon, null) == true)
            {
                return NoParticipantsInMarathon;
            }

            return $"Нет участников в марафоне [{marathon.Name}]";
        }

        public static string FoodDiaryOfUsername(string username, int days = -1)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            if (days > -1)
            {
                return $"Дневник питания ({days} дн)\n[{username}]\n\n";
            }
            return $"Дневник питания\n[{username}]\n\n";
        }

        public static string ConfigureMessageToUserFromConsultant(string message,string consultantName = null)
        {
            if (string.IsNullOrWhiteSpace(message)) return null;

            if (string.IsNullOrWhiteSpace(consultantName) == true)
            {
                return $"Сообщение от консультанта:\n\"{message}\"";
            }

            return $"Сообщение от консультанта [{consultantName}]\n\"{message}\"";
        }

        public static string ConfigureMessageToUser(string message, string senderName = null)
        {
            if (string.IsNullOrWhiteSpace(message)) return null;

            if (string.IsNullOrWhiteSpace(senderName) == true)
            {
                return "Сообщение:\n" + "\"" + message + "\"";
            }

            return $"Сообщение от [{senderName}]\n\"{message}\"";
        }

        public static string RegisteredNewUserMessageForSupport(string senderName)
        {
            if (string.IsNullOrWhiteSpace(senderName) == true)
            {
                return "Новый пользователь";
            }

            return $"Новый пользователь [{senderName}]\n";
        }

        public static string Introduction = "Привет, Я - твой онлайн дневник питания!\n" +
                                            "Я помогу тебе следить за приемами пищи и считать количество воды ежедневно.\n";
        public static string IntroductionMainMenu = "Главное меню\n";
        public static string IntroductionSettings= "Настройки/помощь\n";
        public static string IntroductionChangeFirstName= "Изменение имени!\nВведите свое имя?";
        public static string IntroductionChangeLastName= "Изменение фамилии!\nВведите свою фамилию!";
        public static string IntroductionChangeMarathon= "Введи ID номер марафона, в котором хочешь участвовать?\n" +
                                                         "Если не знаешь ID, запроси его у своего консультанта.";
        public static string IntroductionToWriteToSupport= "Напиши в службу поддержки";
        public static string IntroductionToWriteToConsultantDefault = "Введите сообщение консультанту!";
        public static string IntroductionToUserStatistics = "Дневник питания";
        public static string IntroductionToModeratorMain = "Панель консультанта";
        public static string IntroductionToAdminMain = "Панель администратора";
        public static string IntroductionToModeratorMarathons = "Управление марафонами";
        public static string IntroductionToModeratorAddMarathons = "Введи название нового марафона?";
        public static string IntroductionToModeratorEditMarathonName = "Переименуй марафон";
        public static string IntroductionToModeratorDeleteMarathon = "Ты действительно хочешь удалить марафон?";
        public static string IntroductionToModeratorWritePanel = "Выбери тип сообщения?";
        public static string IntroductionToModeratorWriteAllUsers = "Введи сообщение для всех участников";
        public static string IntroductionToModeratorQuery = "Консультант может:\n" +
                                                            "* Регистрировать участников в онлайн дневнике питания\n" +
                                                            "* Создавать группы марафонов\n" +
                                                            "* Отслеживать дневники питания каждого участника\n" +
                                                            "* Рассылать оповещения и новости участникам марафона\n\n" +
                                                            "Повысить уровень до [Консультанта]?";
        public static string IntroductionToWriteToConsultant(User consultant)
        {
            if (consultant == null) return null;

            return $"Введи сообщение консультанту [{consultant.FirstName} {consultant.LastName}]?";
        }

        public static string IntroductionToModeratorEditConcreteMarathonName(string oldMarathonName)
        {
            if (string.IsNullOrWhiteSpace(oldMarathonName)) return IntroductionToModeratorEditMarathonName;
            return $"Введи новое название марафона [{oldMarathonName}]?";
        }

        public static string IntroductionToModeratorDeleteConcreteMarathon(string oldMarathonName)
        {
            if (string.IsNullOrWhiteSpace(oldMarathonName)) return IntroductionToModeratorDeleteMarathon;
            return $"Ты действительно хочешь удалить марафон [{oldMarathonName}]?";
        }

        public static string IntroductionToModeratorWriteMarathonUsers = "Введи сообщение для участников марафона";
        public static string IntroductionToModeratorWriteConcreteMarathonUsers(string marathonName)
        {
            if (string.IsNullOrWhiteSpace(marathonName))
            {
                return IntroductionToModeratorWriteMarathonUsers;
            }

            return IntroductionToModeratorWriteMarathonUsers + $" [{marathonName}]";
        }

        public static string IntroductionToModeratorWriteUser = "Введи сообщение для участника";
        public static string IntroductionToModeratorWriteConcreteUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return IntroductionToModeratorWriteUser;
            }

            return "Введи сообщение для" + $" [{username}]";
        }

        public static string IncludedNewParticipant = "Новый участник марафона!";
        public static string IncludedNewParticipantToMarathon(string marathonName, string participantName)
        {
            if (string.IsNullOrWhiteSpace(marathonName) || string.IsNullOrWhiteSpace(participantName))
            {
                return IncludedNewParticipant;
            }

            return $"К марафону [{marathonName}] присоединился новый участник [{participantName}]!";
        }

        public static string AskInputMessage = "Введи сообщение";
        public static string AskInputMessageToUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return AskInputMessage;

            return $"Введи сообщение для [{username}]";
        }


        public static string AskFirstName = "Введи своё имя!";
        public static string AskLastName = "Введи свою фамилию!";
        public static string AskMarathonId = "Введи ID марафона в котором ты участвуешь?\n" +
                                             "Или введи \"Пропустить\"\n" +
                                             "Ты все равно сможешь записаться на марафон в любое другое время!";
        public static string AskFood = "Опиши прием пищи!";
        public static string AskWeight = "Введи свой вес (кг)!";
        public static string AskWeighAgain = "Введи вес снова!\nНапример (75.3)";
        public static string AskWeighAgainNotParsed = "Не могу распознать число (вес). Попробуй ввести вес снова!";
        public static string AskWater = "Сколько ты сегодня выпил(а) воды (мл)!\nНапример (2700)!";
        public static string AskWaterAgain = "Введите объем воды снова (мл)!\nНапример (2700)";
        public static string AskWaterAgainNotParsed = "Не могу распознать число. Попробуй ввести объем воды снова!";
        public static string AskWriteAboutProblem = "Опиши проблему!";
        public static string AskWriteFeedBack = "Дай обратную связь)";

        public static string SuccessIncludeToMarathon(Marathon marathon)
        {
            return $"Ты успешно подключен(а) к марафону \"{marathon.Name}\"\n";
        }

        public static string SuccessSetWater(int volume)
        {
            return $"Сегодня ты выпил(а) {volume.ToString()} мл.";
        }
        
        public static string FailInputFirstName = "Введи имя текстом!";
        public static string FailInputLastName = "Введи фамилию текстом!";
        public static string FailInputMarathonId = "Попробуй снова ввести ID марафона!";
        public static string FailInputMarathonName = "Введи название марафона снова!";
        public static string FailWriteToConsultantNotParticipant = "Ты не можешь написать консультанту, потому что не участвуешь ни в одном марафоне";
        public static string FailFindInputedMarathon(string id)
        {
            return $"Нет марафона с ID [{id}]. Попробуй ввести ID марафона снова!\n" +
                   $"Если не знаешь ID, то спроси у своего консультанта";
        }
        public static string FailWriteToConsultantMarathonFinished(Marathon marathon)
        {
            if(marathon == null)
            {
                return $"Нельзя написать консультанту!\nМарафон, в котором ты участвуешь уже завершился!\n" +
                       $"Ты всегда можешь записаться в новый марафон!";
            }
            return $"Нельзя написать консультанту!\nМарафон [{marathon?.Name}], в котором ты участвуешь уже завершился!"+ 
                   $"Ты всегда можешь записаться в новый марафон!";
        }

        public static string AlreadySended = "Отправлено";
        public static string AlreadyFirstNameChanged = "Имя изменено!";
        public static string AlreadyLastNameChanged = "Фамилия изменена!";
        public static string AlreadyCancelled = "Действие отменено";
        public static string AlreadyMessageToSupportSended = "Сообщение отправлено в службу поддержки";
        public static string AlreadyMessageToConsultantSended = "Сообщение отправлено";
        public static string AlreadyModeratorQuerySended = "Запрос на получение прав (КОНСУЛЬТАНТА) отправлен!";
        public static string AlreadyMarathonCreated = "Марафон успешно создан!";
        public static string AlreadyMarathonNameChanged = "Название марафона изменено";
        public static string AlreadyAdminApprovedConsultantQuery = "Запрос подтверждён";
        public static string AlreadyAdminDeniedConsultantQuery = "Запрос отклонён";
        public static string NotFoundUser = "Не найден пользователь";
        public static string AlreadyMarathonDeleted = "Марафон удалён";
        public static string AlreadyConsultantQueryApproved = "Твой уровень прав повышен до (КОНСУЛЬТАНТА)!\n\n" +
                                                              "Теперь тебе доступна панель консультанта.\n" +
                                                              "* Ты можешь создавать и проводить марафоны.\n" +
                                                              "* Приглашать участников в марафоны.\n" +
                                                              "* Просматривать и анализировать дневники питания участников марафонов.\n" +
                                                              "* Делать массовые и одиночные рассылки новостей и оповещений участникам марафонов\n\n" +
                                                              "По всем вопросам пиши в службу поддержки!";

        public static string AlreadyConsultantQueryDenied = "Запрос на уровень прав пользователя (КОНСУЛЬТАНТ) ОТКЛОНЁН!\n\n" +
                                                            "Для получения или возобновления доступа подай запрос (Я - КОНСУЛЬТАНТ) снова\nИли напиши в службу поддержки!";


        public static string AlreadyIncludedToMarathon(string marathonName)
        {
            return $"Ты подключен(а) к марафону \"{marathonName}\"! ";
        }

        

        public const string BtnSkip = "Пропустить";

        public const string BtnWaterAdd300 = "+300 мл";
        public const string BtnWaterRemove300 = "-300 мл";
        public const string BtnSetWater = "Вода";
        public const string BtnSetWeight = "Вес";
        public const string BtnSetFood = "Прием пищи";
        public const string BtnStatistics = "Мой дневник";
        public const string BtnSettings = "Настройки/Помощь";
        public const string BtnCancel = "Отменить";
        
        public const string BtnFirstName = "Имя";
        public const string BtnChangeFirstName = "Изменить имя";
        public const string BtnLastName = "Фамилия";
        public const string BtnChangeLastName = "Изменить фамилию";
        public const string BtnMarathon = "марафон";
        public const string BtnOrderToMarathon = "Записаться в марафон";
        public const string BtnWriteToSupport = "Служба поддержки";
        public const string BtnWriteToConsultant = "Написать консультанту";
        public const string BtnGoToMain = "На главную";

        public const string Btn1Day = "1 день";
        public const string Btn3Day = "3 дней";
        public const string Btn7Day = "7 дней";

        public const string Btn1DayShort = "1 дн";
        public const string Btn3DayShort = "3 дн";
        public const string Btn7DayShort = "7 дн";

        public const string BtnModeratorQuery = "Я - КОНСУЛЬТАНТ";
        public const string BtnWriteAboutProblem = "Сообщить о проблеме";
        public const string BtnFeedBack = "Обратная связь";
        public const string BtnGoBack = "Назад";
        
        public const string BtnYes = "Да";
        public const string BtnNo = "Нет";
        
        public const string BtnModeratorWritePanel = "Написать участникам";
        
        public const string BtnConsultantPanel = "Панель консультанта";
        
        public const string BtnAdminPanel = "Панель админа";
        
        public const string BtnMarathons = "Марафоны";
        public const string BtnParticipantsData = "Дневники участников";
        public const string BtnUserPanel = "Панель пользователя";

        public const string BtnMyMarathons = "Мои марафоны";
        public const string BtnAddMarathon = "Добавить марафон";
        public const string BtnEditDeleteMarathon = "Редактировать/Удалить";
        
        public const string BtnWrap = "Свернуть";
        public const string BtnWrapList = "Свернуть список";
        
        public const string BtnAnswerMessage = "Ответить";
        
        public const string BtnModeratorWriteAllUsers = "Всем участникам";
        public const string BtnModeratorWriteMarathonUsers = "Участникам марафона";
        public const string BtnModeratorWriteConcreteUser = "Одному участнику";
        
        public const string BtnApproveConsultant = "+";
        public const string BtnDenyConsultant = "-";

        public const string BtnBotInfo = "Инфо";
        public const string BtnBotMarathons = "Все марафоны";
        public const string BtnBotModerators = "Модеры";
        public const string BtnListQueries = "Запросы";
        public const string BtnAdminCommands = "Команды";
        
        public const string EmojiWasteBasket = "🗑️";
        public const string EmojiPen = "🖊️";




        public static string CallbackShowMarathon = "show_marathon";
        public static string CallbackShowUser = "user";
        public static string CallbackShowUserData = "show_user_data";
        public static string CallbackEditMarathon = "edit_marathon";
        public static string CallbackDeleteMarathon = "delete_marathon";
        public static string CallbackWrapThisMessage = "wrap_message";
        public static string CallbackAnswerToMessage = "answer_message_to_chat_id";
        public static string CallbackGoBack = "go_back";
        public static string CallbackConsultantQueryAnswer = "answer_consultant_query_";
        public static string CallbackConsultantQueryAnswerYes = "answer_consultant_query_yes";
        public static string CallbackConsultantQueryAnswerNo = "answer_consultant_query_no";
        

    }
}
