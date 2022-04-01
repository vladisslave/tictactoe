public class User: IUser
{
    private string _userName;
    private string _userId;
    private Lobby? _currentLobby;


    public User(string userName, string userId)
    {
        _userName = userName;
        _userId = userId;
    }

    public string UserName
    {
        get
        {
            return _userName;
        }
    }

    public string UserId
    {
        get
        {
            return _userId;
        }
    }

    public Lobby? CurrentLobby
    {
        get
        {
            return _currentLobby;
        }
    }

    public void CreateLobby(LobbySettings lobbySettings)
    {
        Lobby lobby = new Lobby(lobbySettings, this);
        App.GetInstance().AddLobby(lobby);
        _currentLobby = lobby;
    }

    public bool JoinLobby(string lobbyId)
    {
        App app = App.GetInstance();
        Lobby? lobby = app.FindLobby(lobbyId);

        if (lobby != null)
        {
            bool hasJoined = lobby.JoinLobby(this);
            _currentLobby = lobby;

            return hasJoined;
        }

        return false;
    }
}
