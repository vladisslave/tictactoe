class Lobby
{
    private LobbySettings lobbySettings;
    private Game game = new Game();
    public Game GetGame { get { return game; } }
    private string userNameTurn;
    private string firstTurnUser;
    // private string secondTurnUser;
    private int lobbyId;
    private bool isLobbyFull = false;
    public bool IsLobbyFull { get { return isLobbyFull; } }
    private static int lobbyCounter = 0;
    public int LobbyId { get { return lobbyId; } }
    public GameSituation StartGame()
    {

        game = new Game();
        if (lobbySettings.FirstTurn == TurnType.Random)
        {
            int rnd = (new Random()).Next(2);
            if (rnd % 2 == 0)
            {
                game.setUsers(creatorUser, joinedUser);
            }
            else
            {
                game.setUsers(joinedUser, creatorUser);
            }
        }
        else if (lobbySettings.FirstTurn == TurnType.Guest)
        {
            game.setUsers(joinedUser, creatorUser);
        }
        else
        {
            game.setUsers(creatorUser, joinedUser);
        }

        return new GameSituation
        {
            Lst = Game.convertSituationList(game.Lst),
            TurnUserName = game!.firstTurnUser!.UserName,
            Finished = false
        };
    }


    private User creatorUser;
    public string CreatorUserName { get { return creatorUser.UserName; } }
    public string CreatorUserId { get { return creatorUser.UserId; } }
    private User joinedUser;
    public string JoinedUserId { get { return joinedUser.UserId; } }

    public Lobby(LobbySettings lobbySettings, string lobbyCreatorId)
    {
        this.lobbySettings = lobbySettings;
        this.creatorUser = RoomManager.findUser(lobbyCreatorId);
        this.lobbyId = lobbyCounter;
        this.firstTurnUser = creatorUser.UserName;
        this.userNameTurn = creatorUser.UserName;
        this.joinedUser = creatorUser;
        lobbyCounter++;
    }

    public void JoinLobby(string joinedUserId)
    {
        this.joinedUser = RoomManager.findUser(joinedUserId);
        this.isLobbyFull = true;
    }

}
