using Microsoft.AspNetCore.Identity;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;

namespace PhotoContests.Manager
{
    public class ChatManager: IChatManager
    {
        private readonly IChatRepo repo;
        private readonly UserManager<User> userManager;
        private readonly PhotoContestsContext db;

        public ChatManager(IChatRepo repo, UserManager<User> userManager, PhotoContestsContext db)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.db = db;
        }

        /* public void UpdateCodConexiune(string idUtilizator, string conexiune)
         {

             var utilizator = userManager.Users
                 .FirstOrDefault(u => u.Id.Equals(idUtilizator));
             if (utilizator == null)
                 return;
             utilizator.codConexiune = conexiune;
             repo.Update(utilizator);
         }*/
        public void UpdateConnectionCode(string idUser, string connection)
        {
            var user = userManager.Users
                .FirstOrDefault(u => u.Id.Equals(idUser));
            if (user == null)
                return;
            user.connectionCode = connection;
            repo.Update(user);
        }

        public FirstLastNameUserChat GetFullName(string idUser)
        {
            var user = userManager.Users
                .FirstOrDefault(u => u.Id.Equals(idUser));

            if (user == null)
            {
                return new FirstLastNameUserChat();
            }
            var userName = userManager.Users
                    .Where(u => u.Id.Equals(idUser))
                    .Select(u => new FirstLastNameUserChat{
                        lastName = user.lastName,
                        firstName = user.firstName
                     })
                    .FirstOrDefault();
            return userName;
        }

        public bool GetAvailability(string idUser)
        {
            var user = userManager.Users.FirstOrDefault(u => u.Id.Equals(idUser));
            if (user == null) return false; 
            var availability = user.availability;
            return availability;
        }

        public void UpdateAvailability(string idUser, bool availability)
        {
            var user = userManager.Users.FirstOrDefault(u => u.Id.Equals(idUser));
            if (user == null) return;
            user.availability = availability;
            repo.Update(user);
        }

        public void CreateMessage(ChatCreateModel model)
        {
            var newMessage = new Message
            {
                idUser = model.idUser,
                idUser2 = model.idUser2,
                messageText = model.messageText,
                sendDate = DateTime.Now,
                visibility = model.visibility
            };
            repo.CreateMessage(newMessage);
        }

        public void UpdateReadingMessage(string idUser, string idUser2, bool readingMessage)
        {
            var messages = repo.GetMessages()
                .Where(m => (m.idUser == idUser && m.idUser2 == idUser2 && m.visibility == false)
                || (m.idUser2 == idUser && m.idUser == idUser2 && m.visibility == false))
                .ToList();
            messages.ForEach(m => m.visibility = readingMessage);
            db.SaveChanges();
        }
        
        public void UpdateReadingMessageToOffline(string idUser, string idUser2, bool readingMessage)
        {
            var messages = repo.GetMessages()
                .Where(m => m.idUser2 == idUser && m.idUser == idUser2 && m.visibility == false)
                .ToList();
            messages.ForEach(m => m.visibility = readingMessage);
            db.SaveChanges();
        }

        public List<GetMessageModel> GetAllMessagesConv(string idUser, string idUser2)
        {
            var messagesFromTo = repo.GetMessages().Where(m => m.idUser == idUser && m.idUser2 == idUser2);
            var messagesToFrom = repo.GetMessages().Where(m => m.idUser == idUser2 && m.idUser2 == idUser);
            var messages = messagesFromTo.Union(messagesToFrom);

            if (messages == null)
                return new List<GetMessageModel>();

            var allMessages = messages.Select(m => new GetMessageModel
            {
                emailUser = userManager.Users.Where(u => u.Id == m.idUser).Select(u => u.Email).FirstOrDefault(),
                emailUser2 = userManager.Users.Where(u => u.Id == m.idUser2).Select(u => u.Email).FirstOrDefault(),
                messageText = m.messageText,
                sendDate= m.sendDate
            })
                .OrderBy(m => m.sendDate)
                .ToList();
            return allMessages;
        }

        public List<string> OnlineUser()
        {
            var onlineUsers = userManager.Users.Where(u => u.connectionCode != null).Select(u => u.Email).ToList();
            return onlineUsers;
        }

        public List<OnlineUserModel> UsersOnline()
        {
            var onlineUsers = userManager.Users.Where(u => u.connectionCode != null)
                .Select(u => new OnlineUserModel
                {
                    firstName = u.firstName,
                    lastName = u.lastName,  
                    availability= u.availability,
                    Email  = u.Email
                })
                .OrderBy(u => u.lastName) .ToList();
            return onlineUsers;
        }

        public int NumberUnreadMessages(string emailUser)
        {
            var totalNoUnreadMessages = 0;
            var loggedUser = userManager.Users
               .Where(u => u.Email.Equals(emailUser))
               .FirstOrDefault();

            var users = userManager.Users
                .Where(u => u.Email != emailUser)
                .Select(u => new
                {
                    id = u.Id,
                })
                .ToList();

            for (int i = 0; i < users.Count(); i++)
            {
                var numberUnreadMessages = repo.GetMessages()
                    .Where(m => m.idUser == users[i].id && m.idUser2 == loggedUser.Id && m.visibility == false)
                    .Count();
                totalNoUnreadMessages+= numberUnreadMessages;
            }
            return totalNoUnreadMessages;
        }

        public List<OnlineUsersMessagesModel> GetUnreadMessagesByLogged(string emailUser)
        {
            var loggedUser = userManager.Users
               .Where(u => u.Email.Equals(emailUser))
               .FirstOrDefault();

            var users = userManager.Users
                .Where(u => u.Email != emailUser)
                .Select(u => new OnlineUsersMessagesModel
                {
                    lastName = u.lastName,
                    firstName = u.firstName,
                    availability = u.availability,
                    Email = u.Email,
                    idUser = u.Id,
                    messageNo = 0
                })
                .ToList();

            for (int i = 0; i < users.Count(); i++)
            {
                var numberUnreadMessages = repo.GetMessages()
                    .Where(m => m.idUser == users[i].idUser && m.idUser2 == loggedUser.Id && m.visibility == false)
                    .Count();
                users[i].messageNo = numberUnreadMessages;
            }

            var loggedUserMess = new OnlineUsersMessagesModel();
            loggedUserMess.lastName = loggedUser.lastName;
            loggedUserMess.firstName = loggedUser.firstName;
            loggedUserMess.Email= emailUser;
            loggedUserMess.idUser = loggedUser.Id;
            loggedUserMess.availability = loggedUser.availability;
            loggedUserMess.messageNo = 0;

            users.Add(loggedUserMess);

            var usersUnreadMessages = users.Where( u => u.messageNo > 0)
                .OrderBy(u => u.lastName)
                .ToList();
            return usersUnreadMessages;
        }


        public List<OnlineUserModel> UsersOffline()
        {
            var oflineUsers = userManager.Users
                .Where(u => u.connectionCode == null && !u.UsersRoles.Any(r => r.Role.Name == "AdminUser"))
                .Select(u => new OnlineUserModel
                {
                    firstName = u.firstName,
                    lastName = u.lastName,
                    availability = u.availability,
                    Email = u.Email
                })
                .OrderBy(u => u.lastName).ToList();
            return oflineUsers;
        }
    }
}
