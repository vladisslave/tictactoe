public class App
{
    private static readonly App AppInstance = new App();
    private List<User> _userList;
    private List<Lobby> _lobbyList;


    public App()
    {
        _userList = new List<User>();
        _lobbyList = new List<Lobby>();
    }

    public List<Lobby> LobbyList
    {
        get
        {
            return _lobbyList;
        }
    }

    public static App GetInstance()
    {
        return AppInstance;
    }

    public bool AddUser(User user)
    {
        User? userWithDuplicateName = FindUserByUserName(user.UserName);

        if (userWithDuplicateName == null)
        {
            _userList.Add(user);

            return true;
        }

        return false;
    }

    public void AddLobby(Lobby lobby)
    {
        _lobbyList.Add(lobby);
    }

    public User? FindUserByUserId(string userId)
    {
        User? foundUser = _userList.Find(user => user.UserId == userId);

        return foundUser;
    }

    private User? FindUserByUserName(string userName)
    {
        User? foundUser = _userList.Find(user => user.UserName == userName);

        return foundUser;
    }

    public Lobby? FindLobby(string lobbyId)
    {
        Lobby? lobby = _lobbyList.Find(lobby => lobby.LobbyId == lobbyId);

        return lobby;
    }

    public void RemoveUser(string userId)
    {
        _userList.RemoveAll(user => user.UserId == userId);
    }

    public void RemoveLobby(string LobbyId)
    {
        _lobbyList.RemoveAll(lobby => lobby.LobbyId == LobbyId);
    }
}