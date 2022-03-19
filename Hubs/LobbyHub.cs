using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace BlazorServerSignalRApp.Hubs
{
    public class LobbyHub : Hub
    {

        public async Task CreateLobby(string turnType)
        {
            Lobby lobby = new Lobby(new LobbySettings(turnType), Context.ConnectionId);
            RoomManager.addLobby(lobby);
            await Clients.Caller.SendAsync("JoinedLobby", lobby.LobbyId);
        }

        public async Task JoinLobby(int lobbyId)
        {
            RoomManager.findLobby(lobbyId).JoinLobby(Context.ConnectionId);
            await Clients.Caller.SendAsync("JoinedLobby", lobbyId);

            await UpdateLobbyList();
        }

        public async Task StartGame(int lobbyId)
        {
            var lobby = RoomManager.findLobby(lobbyId);
            string u1 = lobby.JoinedUserId;
            string u2 = lobby.CreatorUserId;
            GameSituation st = lobby.StartGame();
            // Logger.writeMessage(JsonConvert.SerializeObject(st));
            // GameSituation desSt = JsonConvert.DeserializeObject<GameSituation>(JsonConvert.SerializeObject(st));
            // Logger.writeMessage("serial deserial");
            // Logger.writeMessage(JsonConvert.SerializeObject(desSt));
            string gameSituationJson = JsonConvert.SerializeObject(st);
            Logger.writeMessage(u1);
            Logger.writeMessage(u2);
            await Clients.Client(u1).SendAsync("ReceiveSituation", gameSituationJson);
            await Clients.Client(u2).SendAsync("ReceiveSituation", gameSituationJson);
        }

        public async Task MakeTurn(int cellNum, int lobbyId)
        {
            var lobby = RoomManager.findLobby(lobbyId);
            string u1 = lobby.JoinedUserId;
            string u2 = lobby.CreatorUserId;
            string gameSituationJson = JsonConvert.SerializeObject(lobby.GetGame.MakeTurn(cellNum, Context.ConnectionId));
            await Clients.Client(u1).SendAsync("ReceiveSituation", gameSituationJson);
            await Clients.Client(u2).SendAsync("ReceiveSituation", gameSituationJson);
            if (u1 == u2) {
                Logger.writeMessage("SUPERPATAU");
            }
        }

        public async Task UpdateLobbyList()
        {
            List<Lobby> lobbyList = RoomManager.returnLobbyList();
            List<string> creatorNames = new List<string>();
            List<int> lobbyIds = new List<int>();
            foreach (Lobby lobby in lobbyList)
            {
                if (!lobby.IsLobbyFull)
                {
                    creatorNames.Add(lobby.CreatorUserName);
                    lobbyIds.Add(lobby.LobbyId);
                }
            }
            // Console.WriteLine(lobbyList);
            await Clients.All.SendAsync("UpdateLobbyList", lobbyIds, creatorNames);
        }
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
                await Clients.Caller.SendAsync("ShowWaitingRoom", "str");
            }
            else
            {
                await Clients.Caller.SendAsync("UniquenessErrorHandler", new UniquenessError("userName"));
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
            RoomManager.deleteUserFromRooms(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendUsersCountOnline()
        {
            await Clients.All.SendAsync("CountOnline", RoomManager.getWaitingPlayersCount());
        }
    }
}