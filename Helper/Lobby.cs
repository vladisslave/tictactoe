public class Lobby
{
    private string _lobbyId;
    private LobbySettings _lobbySettings;
    private bool _isFull;
    private User _creatorUser;
    private User? _joinedUser;
    private Game? _currentGame;


    public Lobby(LobbySettings lobbySettings, User creatorUser)
    {
        _lobbySettings = lobbySettings;
        _lobbyId = creatorUser.UserId;
        _creatorUser = creatorUser;
        _isFull = false;
    }

    public string LobbyId
    {
        get
        {
            return _lobbyId;
        }
    }

    public bool IsFull
    {
        get
        {
            return _isFull;
        }
    }

    public User CreatorUser
    {
        get
        {
            return _creatorUser;
        }
    }

    public Game? CurrentGame
    {
        get
        {
            return _currentGame;
        }
    }

    public bool JoinLobby(User joinedUser)
    {
        if (!_isFull)
        {
            _joinedUser = joinedUser;
            _isFull = true;

            return true;
        }

        return false;
    }

    public void StartGame()
    {
        if (_joinedUser != null)
        {
            if (_lobbySettings.FirstTurn == TurnType.Random)
            {
                int rnd = (new Random()).Next(2);
                if (rnd % 2 == 0)
                {
                    _currentGame = new Game(_creatorUser, _joinedUser);
                }
                else
                {
                    _currentGame = new Game(_joinedUser, _creatorUser);
                }

            }
            else if (_lobbySettings.FirstTurn == TurnType.Creator)
            {
                _currentGame = new Game(_creatorUser, _joinedUser);
            }
            else if (_lobbySettings.FirstTurn == TurnType.Guest)
            {
                _currentGame = new Game(_joinedUser, _creatorUser);
            }
        }
    }
}
