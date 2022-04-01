public class Game
{
    private User _userX;
    private User _userO;
    private User _currentUser;
    private Board _board;
    private GameMemory _gameMemory;
    private GameSession _gameSession;

    
    public Game(User userX, User userO)
    {
        _userX = userX;
        _userO = userO;
        _currentUser = _userX;
        _board = new Board();
        _gameMemory = new GameMemory(SaveGameMomento());
        _gameSession = InitializeGameSession();
    }

    public User UserX
    {
        get
        {
            return _userX;
        }
    }

    public GameMemory GameMemory
    {
        get
        {
            return _gameMemory;
        }
    }

    public User UserO
    {
        get
        {
            return _userO;
        }
    }

    public User CurrentUser
    {
        get
        {
            return _currentUser;
        }
    }

    public Board Board
    {
        get
        {
            return _board;
        }
    }

    public GameRenderElement GameRenderElement
    {
        get
        {
            GameRenderElement gameRenderElement = new GameRenderElement(this);
            _gameSession.Turns?.Add(gameRenderElement);
            GameSessionWriter.WriteSession(_gameSession);

            return gameRenderElement;
        }
    }

    private GameSession InitializeGameSession()
    {
        return new GameSession()
        {
            GameId = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") +
        Environment.NewLine + UserX.UserName + " VS " + UserO.UserName,
            Turns = new List<GameRenderElement>()
        };
    }

    public GameMemento SaveGameMomento()
    {
        return new GameMemento(_currentUser, _board);
    }

    public void RestoreGameMemento(GameMemento gameMemento)
    {
        _currentUser = gameMemento.CurrentUser;
        _board = gameMemento.Board;
    }

    public void MakeTurn(User userCaller, int squareNumber)
    {
        bool isTurnSuccessfull = false;
        GameMemento gameMemento = SaveGameMomento();

        if (userCaller == _userX && _currentUser == _userX)
        {
            isTurnSuccessfull = _board.MakeUserXTurn(squareNumber);
        }
        else if (userCaller == _userO && _currentUser == _userO)
        {
            isTurnSuccessfull = _board.MakeUserOTurn(squareNumber);
        }

        if (isTurnSuccessfull)
        {
            SwapCurrentPlayer();
            _gameMemory.GameMemento = gameMemento;
        }
    }

    public void SwapCurrentPlayer()
    {
        if (_currentUser == _userX)
        {
            _currentUser = _userO;
        }
        else
        {
            _currentUser = _userX;
        }
    }
}