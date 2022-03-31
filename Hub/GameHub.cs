using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace BlazorServerSignalRApp.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinWaitingRoom(string userName)
        {
            User newUser = new User(userName, Context.ConnectionId);
            bool wasAdded = App.GetInstance().AddUser(newUser);

            if (!wasAdded)
            {
                await SendErrorMessage(ErrorMessage.UserNameIsNotUnique, Context.ConnectionId);
                return;
            }

            await Clients.Caller.SendAsync("ShowWaitingRoom", "str");
        }

        public async Task UpdateLobbyList()
        {
            List<Lobby> lobbyList = App.GetInstance().LobbyList;
            List<string> creatorNames = new List<string>();
            List<string> lobbyIds = new List<string>();

            foreach (Lobby lobby in lobbyList)
            {
                if (!lobby.IsFull)
                {
                    creatorNames.Add(lobby.CreatorUser.UserName);
                    lobbyIds.Add(lobby.LobbyId);
                }
            }
            await Clients.All.SendAsync("UpdateLobbyList", lobbyIds, creatorNames);
        }

        public async Task CreateLobby(string turnType)
        {
            User? user = App.GetInstance().FindUserByUserId(Context.ConnectionId);

            if (user != null)
            {
                LobbySettings lobbySettings = new LobbySettings(turnType);

                user.CreateLobby(lobbySettings);
                await Clients.Caller.SendAsync("JoinedLobby", String.Empty);
            }
        }

        public async Task JoinLobby(string lobbyId)
        {
            User? user = App.GetInstance().FindUserByUserId(Context.ConnectionId);

            if (user != null)
            {
                user.JoinLobby(lobbyId);
            }

            await Clients.Caller.SendAsync("JoinedLobby");
        }

        public async Task StartGame()
        {
            User? user = App.GetInstance().FindUserByUserId(Context.ConnectionId);

            if (user != null)
            {
                user?.CurrentLobby?.StartGame();

                var gameRenderElement = user?.CurrentLobby?.CurrentGame?.GameRenderElement;

                if (gameRenderElement != null)
                {
                    string gameRenderElementJson = JsonConvert.SerializeObject(gameRenderElement);

                    string userOId = user!.CurrentLobby!.CurrentGame!.UserO.UserId;
                    string userXId = user!.CurrentLobby!.CurrentGame!.UserX.UserId;

                    await Clients.Client(userOId).SendAsync("ReceiveGameRenderElement", gameRenderElementJson);
                    await Clients.Client(userXId).SendAsync("ReceiveGameRenderElement", gameRenderElementJson);
                }
            }

        }

        public async Task MakeTurn(int squareNumber)
        {
            User? user = App.GetInstance().FindUserByUserId(Context.ConnectionId);

            if (user != null)
            {
                user?.CurrentLobby?.CurrentGame?.MakeTurn(user, squareNumber);

                var gameRenderElement = user?.CurrentLobby?.CurrentGame?.GameRenderElement;

                if (gameRenderElement != null)
                {
                    string gameRenderElementJson = JsonConvert.SerializeObject(gameRenderElement);

                    string userOId = user!.CurrentLobby!.CurrentGame!.UserO.UserId;
                    string userXId = user!.CurrentLobby!.CurrentGame!.UserX.UserId;

                    await Clients.Client(userOId).SendAsync("ReceiveGameRenderElement", gameRenderElementJson);
                    await Clients.Client(userXId).SendAsync("ReceiveGameRenderElement", gameRenderElementJson);
                }
            }

        }



        public async Task RestoreLastTurn()
        {
            User? user = App.GetInstance().FindUserByUserId(Context.ConnectionId);

            if (user != null)
            {
                Game? game = user?.CurrentLobby?.CurrentGame;

                if (game != null)
                {
                    game.RestoreGameMemento(game.GameMemory.GameMemento);
                    var gameRenderElement = game.GameRenderElement;

                    string gameRenderElementJson = JsonConvert.SerializeObject(gameRenderElement);

                    string userOId = game.UserO.UserId;
                    string userXId = game.UserX.UserId;

                    await Clients.Client(userOId).SendAsync("ReceiveGameRenderElement", gameRenderElementJson);
                    await Clients.Client(userXId).SendAsync("ReceiveGameRenderElement", gameRenderElementJson);
                }
            }

        }

        public async Task SendErrorMessage(ErrorMessage errorMessage, string userId)
        {
            await Clients.Client(userId).SendAsync("ReceiveErrorMessage", errorMessage);
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            App app = App.GetInstance();
            app.RemoveUser(Context.ConnectionId);
            app.RemoveLobby(Context.ConnectionId);
            await UpdateLobbyList();
        }


    }
}