using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlazorServerSignalRApp.Hubs
{
    public class LobbyHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinWaitingRoom(string roomName, string userName)
        {
            if (RoomManager.addUserToWaitingRoom(userName, Context.ConnectionId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Clients.Group(roomName).SendAsync("ReceiveMessage", userName, $" has joined the group {roomName}.");
            } else {
                await Clients.Caller.SendAsync("Alert", "Name is not unique");
            }
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", "grpMsg", $"{Context.ConnectionId} has disconnected from the group {roomName}.");

        }


        public override Task OnConnectedAsync()
        {
            // RoomManager.ConnectedIds.Add(Context.ConnectionId);
            // var task = Task.Run(async () => await SendUsersCountOnline());
            // task.Wait();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // RoomManager.ConnectedIds.Remove(Context.ConnectionId);
            // var task = Task.Run(async () => await SendUsersCountOnline());
            // task.Wait();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendUsersCountOnline()
        {
            await Clients.All.SendAsync("CountOnline", RoomManager.getWaitingPlayersCount());
        }
    }
}