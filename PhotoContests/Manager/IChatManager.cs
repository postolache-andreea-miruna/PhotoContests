using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IChatManager
    {
        void UpdateConnectionCode(string idUser, string connection);
        FirstLastNameUserChat GetFullName(string idUser);
        bool GetAvailability(string idUser);
        void UpdateAvailability(string idUser, bool availability);
        void CreateMessage(ChatCreateModel model);
        void UpdateReadingMessage(string idUser, string idUser2, bool readingMessage);
        void UpdateReadingMessageToOffline(string idUser, string idUser2, bool readingMessage);
        List<GetMessageModel> GetAllMessagesConv(string idUser, string idUser2);
        List<string> OnlineUser();
        List<OnlineUserModel> UsersOnline();
        int NumberUnreadMessages(string emailUser);
        List<OnlineUsersMessagesModel> GetUnreadMessagesByLogged(string emailUser);
        List<OnlineUserModel> UsersOffline();
    }
}
