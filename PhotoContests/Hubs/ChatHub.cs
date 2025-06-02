using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using PhotoContests.Entities;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Hubs
{
    public class ChatHub: Hub
    {
        private readonly UserManager<User> userManager;
        private readonly IChatManager manager;

        public ChatHub(UserManager<User> userManager, IChatManager manager)
        {
            this.userManager = userManager;
            this.manager = manager;
        }

        private static List<string> groups = new List<string>();

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Chat");
            await Clients.Caller.SendAsync("ConnectedUser");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Chat");
            var user = userManager.Users.Where(u => u.connectionCode== Context.ConnectionId).FirstOrDefault();

            manager.UpdateConnectionCode(user.Id, null);
            manager.UpdateAvailability(user.Id, false);

            await OnlineUsers();
            await OfflineUsers();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task ConnectionIdForUser(string email) //connection is set and availability = true
        {
            var user = userManager.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
            manager.UpdateConnectionCode(user.Id, Context.ConnectionId);
            manager.UpdateAvailability(user.Id, true);

            await OnlineUsers();
            await OfflineUsers();
        }

        public async Task CreatePrivateChat(ChatConvModel message)
        {
            var idUserFrom = userManager.Users.Where(u => u.Email.Equals(message.idUser))
                .Select(u => u.Id).FirstOrDefault();
            var idUserTo = userManager.Users.Where(u => u.Email.Equals(message.idUser2))
                .Select(u => u.Id).FirstOrDefault();

            var availabilityFrom = manager.GetAvailability(idUserFrom);
            var availabilityTo = manager.GetAvailability(idUserTo);

            if(availabilityFrom == true && availabilityTo == false)
            {
                manager.UpdateReadingMessageToOffline(idUserFrom, idUserTo, true);
                string privateGroupName = GetPrivateGroupName(message.idUser, "");
                await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);

                var newMessage = new ChatCreateModel
                {
                    idUser = idUserFrom,
                    idUser2 = idUserTo,
                    messageText = message.messageText,
                    visibility = false
                };

                manager.CreateMessage(newMessage);
            }

            if(availabilityFrom == true && availabilityTo == true) //add group name
            {
                manager.UpdateAvailability(idUserFrom, false);
                manager.UpdateAvailability(idUserTo, false);

                manager.UpdateReadingMessage(idUserFrom,idUserTo, true);
                await OnlineUsers();

                string privateGroupName = GetPrivateGroupName(message.idUser, message.idUser2);

                groups.Add(privateGroupName);

                await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);

                var connectionIdUser2 = userManager.Users.Where(u => u.Email.Equals(message.idUser2))
                    .Select(u => u.connectionCode).FirstOrDefault();
                await Groups.AddToGroupAsync(connectionIdUser2, privateGroupName); // for the user we send the message to open the chat window

                var newMessage = new ChatCreateModel
                {
                    idUser = idUserFrom,
                    idUser2 = idUserTo,
                    messageText = message.messageText,
                    visibility = true
                };

                manager.CreateMessage(newMessage);
                await Clients.Client(connectionIdUser2).SendAsync("OpenPrivateChat", message); //to send user 2 the message
            }
        }

        public async Task ReceivePrivateMessage(ChatConvModel message)
        {
            var idUserFrom = userManager.Users.Where(u => u.Email.Equals(message.idUser))
                .Select(u => u.Id).FirstOrDefault();
            var idUserTo = userManager.Users.Where(u => u.Email.Equals(message.idUser2))
                .Select(u => u.Id).FirstOrDefault();

            var availabilityFrom = manager.GetAvailability(idUserFrom);
            var availabilityTo = manager.GetAvailability(idUserTo);

            if(availabilityFrom == true && availabilityTo == false)
            {
                string privateGroupName = GetPrivateGroupName(message.idUser, "");
                var newMessage = new ChatCreateModel
                {
                    idUser = idUserFrom,
                    idUser2 = idUserTo,
                    messageText = message.messageText,
                    visibility = false
                };
                manager.CreateMessage(newMessage);
                await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
            }

            if((availabilityFrom == false && availabilityTo == false) || (availabilityFrom == true && availabilityTo == true))
            {
                string privateGroupName = GetPrivateGroupName(message.idUser, message.idUser2);
                var existingGroup = groups.Contains(privateGroupName);
                if(existingGroup == false)
                {
                    await Clients.Group(GetPrivateGroupName(message.idUser, ""))
                        .SendAsync("NewPrivateMessage", message);
                    await CreatePrivateChat(message);
                }
                else
                {
                    var newMessage = new ChatCreateModel
                    {
                        idUser = idUserFrom,
                        idUser2 = idUserTo,
                        messageText = message.messageText,
                        visibility = true
                    };

                    manager.CreateMessage(newMessage);
                    await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
                }
            }
        }

        public async Task ClosePrivateChat(string emailUser, string emailUser2)
        {
            var idUserFrom = userManager.Users.Where(u => u.Email.Equals(emailUser))
                .Select(u => u.Id).FirstOrDefault();
            var idUserTo = userManager.Users.Where(u => u.Email.Equals(emailUser2))
                .Select(u => u.Id).FirstOrDefault();

            var availabilityFrom = manager.GetAvailability(idUserFrom);
            var availabilityTo = manager.GetAvailability(idUserTo);

            if(availabilityFrom == false && availabilityTo == false)
            {
                manager.UpdateAvailability(idUserFrom, true);
                manager.UpdateAvailability(idUserTo, true);

                await OnlineUsers();
                await UsersAndUnreadMessages(emailUser2);

                string privateGroupName = GetPrivateGroupName(emailUser,emailUser2);
                groups.Remove(privateGroupName);

                await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
                var connectionIdUser2 = userManager.Users.Where(u => u.Email.Equals(emailUser2))
                    .Select(u => u.connectionCode).FirstOrDefault();
                await Groups.RemoveFromGroupAsync(connectionIdUser2, privateGroupName);
            }

            if(availabilityFrom == true && availabilityTo == false)
            {
                await OnlineUsers();
                await UsersAndUnreadMessages(emailUser);
                string privateGroupName = GetPrivateGroupName(emailUser, "");
                await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            }
        }


        private async Task OnlineUsers()
        {
            var users = manager.UsersOnline();
            await Clients.Groups("Chat").SendAsync("OnlineUsers", users);
        }

        private async Task OfflineUsers()
        {
            var users = manager.UsersOffline();
            await Clients.Groups("Chat").SendAsync("OfflineUsers", users);
        }

        private string GetPrivateGroupName(string emailUser, string emailUser2)
        {
            var stringCompare = string.CompareOrdinal(emailUser, emailUser2) < 0;
            return stringCompare ? $"{emailUser}-{emailUser2}" : $"{emailUser2}-{emailUser}";
        }

        public async Task UsersAndUnreadMessages(string emailUserConnected)
        {
            var userAndMessages = manager.GetUnreadMessagesByLogged(emailUserConnected);
            var connectionCodeUser = userManager.Users.Where(u => u.Email.Equals(emailUserConnected))
                .Select(u => u.connectionCode).FirstOrDefault();
            await Clients.Client(connectionCodeUser).SendAsync("UserMessagesNr", userAndMessages);
        }
    }
}
