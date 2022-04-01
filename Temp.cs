using Newtonsoft.Json;
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

public class User
{
    private string _userName;
    private string _userId;
    private Lobby? _currentLobby;

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


    public User(string userName, string userId)
    {
        _userName = userName;
        _userId = userId;
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

public class Lobby
{
    private string _lobbyId;
    private LobbySettings _lobbySettings;
    private bool _isFull;
    private User _creatorUser;
    private User? _joinedUser;
    private Game? _currentGame;

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

    public Lobby(LobbySettings lobbySettings, User creatorUser)
    {
        _lobbySettings = lobbySettings;
        _lobbyId = creatorUser.UserId;
        _creatorUser = creatorUser;
        _isFull = false;
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

public class Game
{
    private User _userX;
    private User _userO;
    private User _currentUser;
    private Board _board;
    private GameMemory _gameMemory;
    private GameSession _gameSession;

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


    public Game(User userX, User userO)
    {
        _userX = userX;
        _userO = userO;
        _currentUser = _userX;
        _board = new Board();
        _gameMemory = new GameMemory(SaveGameMomento());
        _gameSession = InitializeGameSession();
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

[Serializable]
public class GameRenderElement
{
    public string? TurnUserName { get; set; }
    public List<string>? CellValueList { get; set; }
    public bool IsFinished { get; set; }
    public string? Message { get; set; }

    public GameRenderElement()
    {

    }
    public GameRenderElement(Game game)
    {
        TurnUserName = game.CurrentUser.UserName;
        CellValueList = ConvertSquareListToStringList(game.Board.BoardSquaresList);

        if (game.Board.BoardState != BoardState.Unfinished)
        {
            IsFinished = true;
        }
        else
        {
            IsFinished = false;
        }

        Message = CreateMessage(game);
    }

    private string CreateMessage(Game game)
    {
        if (game.Board.BoardState == BoardState.OWin)
        {
            return game.UserO.UserName + " has won!";
        }
        if (game.Board.BoardState == BoardState.XWin)
        {
            return game.UserX.UserName + " has won!";
        }
        if (game.Board.BoardState == BoardState.Unfinished)
        {
            return game.CurrentUser.UserName + "`s turn!";
        }

        return "Draw";
    }

    public List<string> ConvertSquareListToStringList(List<BoardSquare> boardSquareList)
    {
        List<string> cellValueList = new List<string>();

        foreach (BoardSquare square in boardSquareList)
        {
            if (square.SquareValue == 1)
            {
                cellValueList.Add("x");
            }
            else if (square.SquareValue == -1)
            {
                cellValueList.Add("o");
            }
            else
            {
                cellValueList.Add(String.Empty);
            }
        }

        return cellValueList;
    }
}

public enum BoardState
{
    Unfinished,
    Draw,
    XWin,
    OWin
}

public class Board
{
    private readonly int _squaresNumber;
    private BoardState _boardState;
    private List<BoardSquare> _boardSquareList;

    public List<BoardSquare> BoardSquaresList
    {
        get
        {
            return _boardSquareList;
        }
    }

    public BoardState BoardState
    {
        get
        {
            return _boardState;
        }
    }


    public Board()
    {
        _boardSquareList = new List<BoardSquare>();
        _squaresNumber = 9; // 3x3 Board
        _boardState = BoardState.Unfinished;

        for (int squareNumber = 0; squareNumber < _squaresNumber; ++squareNumber)
        {
            _boardSquareList.Add(new BoardSquare());
        }
    }

    public bool MakeUserXTurn(int squareNumber)
    {
        return MakeUserTurn(squareNumber, 1);
    }

    public bool MakeUserOTurn(int squareNumber)
    {
        return MakeUserTurn(squareNumber, -1);
    }

    private bool MakeUserTurn(int squareNumber, int squareValue)
    {
        if (_boardState != BoardState.Unfinished)
        {
            return false;
        }

        try
        {
            _boardSquareList[squareNumber].SquareValue = squareValue;

            CalculateState();
            return true;
        }
        catch
        {

        }

        return false;
    }

    public void CalculateState()
    {
        bool hasWinner = CheckForWinner();

        if (!hasWinner)
        {
            CheckForDraw();
        }
    }

    public bool CheckForWinner()
    {
        int[,] winnerCombination = new int[,] {
            {0, 1, 2},
            {3, 4, 5},
            {6, 7, 8},
            {0, 3, 6},
            {1, 4, 7},
            {2, 5, 8},
            {0, 4, 8},
            {2, 4, 6}
        };

        for (int x = 0; x < winnerCombination.GetLength(0); x += 1)
        {
            int el1 = _boardSquareList[winnerCombination[x, 0]].SquareValue;
            int el2 = _boardSquareList[winnerCombination[x, 1]].SquareValue;
            int el3 = _boardSquareList[winnerCombination[x, 2]].SquareValue;

            if (el1 == el2 && el2 == el3 && el3 == 1)
            {
                _boardState = BoardState.XWin;

                return true;
            }
            else if (el1 == el2 && el2 == el3 && el3 == -1)
            {
                _boardState = BoardState.OWin;

                return true;
            }
        }

        return false;
    }

    public void CheckForDraw()
    {
        bool isDraw = true;

        for (int squareNumber = 0; squareNumber < _boardSquareList.Count(); ++squareNumber)
        {
            int squareValue = _boardSquareList[squareNumber].SquareValue;

            if (squareValue == 0)
            {
                isDraw = false;
            }
        }

        if (isDraw)
        {
            _boardState = BoardState.Draw;
        }
    }

    public Board CopyBoard()
    {
        Board board = new Board();

        for (int squareNumber = 0; squareNumber < _squaresNumber; ++squareNumber)
        {
            board._boardSquareList[squareNumber] = new BoardSquare(this._boardSquareList[squareNumber].SquareValue);
            board._boardState = this._boardState;
        }

        return board;
    }
}

public class BoardSquare
{
    private int _squareValue;

    public int SquareValue
    {
        get
        {
            return _squareValue;
        }
        set
        {
            if (SquareValue == 0)
            {
                _squareValue = value;
                return;
            }

            throw new Exception("wrong square to change");
        }
    }
    public BoardSquare()
    {
        _squareValue = 0;
    }

    public BoardSquare(int BoardSquareValue)
    {
        _squareValue = BoardSquareValue;
    }
}

public class LobbySettings
{
    private TurnType _firstTurn;

    public TurnType FirstTurn
    {
        get
        {
            return _firstTurn;
        }
    }


    public LobbySettings(string turnType)
    {
        switch (turnType)
        {
            case "0":
                _firstTurn = TurnType.Random;
                break;

            case "1":
                _firstTurn = TurnType.Creator;
                break;

            case "2":
                _firstTurn = TurnType.Guest;
                break;

            default:
                _firstTurn = TurnType.Random;
                break;
        }
    }
}

public enum TurnType
{
    Random,
    Creator,
    Guest
}

public class GameMemory
{
    private GameMemento _gameMemento;

    public GameMemento GameMemento
    {
        get
        {
            return _gameMemento;
        }
        set
        {
            _gameMemento = value;
        }
    }

    public GameMemory(GameMemento gameMemento)
    {
        _gameMemento = gameMemento;
    }
}

public class GameMemento
{
    private User _currentUser;
    private Board _board;

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


    public GameMemento(User currentUser, Board board)
    {
        _currentUser = currentUser;
        _board = board.CopyBoard();
    }
}

public enum ErrorMessage
{
    UserNameIsNotUnique
}

[Serializable]
class GameSession
{
    public string? GameId { get; set; }
    public List<GameRenderElement>? Turns { get; set; }
}


static class GameSessionWriter
{
    public static List<GameSession>? _gameSessionList;
    private static string _filePath;

    static GameSessionWriter()
    {
        _filePath = InitializeFilePath();
    }

    public static void WriteSession(GameSession gameSession)
    {
        string sessionsJson = File.ReadAllText(_filePath);
        _gameSessionList = JsonConvert.DeserializeObject<List<GameSession>>(sessionsJson);

        if (_gameSessionList == null)
        {
            _gameSessionList = new List<GameSession>();
        }

        GameSession? foundGS = _gameSessionList?.Find(gS => gS.GameId == gameSession.GameId);

        if (foundGS == null)
        {
            _gameSessionList?.Add(gameSession);
        }
        else
        {
            foundGS.Turns = gameSession.Turns;
        }

        sessionsJson = JsonConvert.SerializeObject(_gameSessionList);
        Task asyncTask = WriteFileAsync(_filePath, sessionsJson);
    }

    private static async Task WriteFileAsync(string filePath, string content)
    {
        using (StreamWriter outputFile = new StreamWriter(filePath, append: false))
        {
            await outputFile.WriteAsync(content);
        }
    }
    
    private static string InitializeFilePath()
    {
        string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        dirPath = Path.Combine(dirPath, "БС подготов очка", "csharp", "TicTocShue");
        string fileName = "sessions.json";

        return Path.Combine(dirPath, fileName);
    }
}



/////////////////////////////////////////////////////////
//FRONTEND CLASS
///////////////////////////////////////////////////////////

class LobbyIdAndName
{
    public string? name;
    public string id;

    public LobbyIdAndName(string _id, string _name)
    {
        name = _name;
        id = _id;
    }
}

class GameRender
{
    private string userName;
    private List<Cell> cells = new List<Cell>();

    public class Cell
    {
        private bool isDisabled = true;
        public bool IsDisabled { get { return isDisabled; } set { isDisabled = value; } }
        private string value = "";
        public string Value { get { return value; } set { this.value = value; } }
    }

    public void ParseSituation(GameRenderElement gameRenderElement)
    {
        for (int i = 0; i < 9; ++i)
        {
            if (gameRenderElement.CellValueList != null)
                getCell(i).Value = gameRenderElement.CellValueList[i];
        }

        if (gameRenderElement.TurnUserName == userName)
        {
            DisableActiveCells();
        }
        if (gameRenderElement.TurnUserName != userName)
        {
            DisableAllCells();
        }
    }

    public void DisableAllCells()
    {
        foreach (Cell cell in cells)
        {
            cell.IsDisabled = true;
        }
    }

    public void DisableActiveCells()
    {
        foreach (Cell cell in cells)
        {
            if (cell.Value != String.Empty)
            {
                cell.IsDisabled = true;
            }
            else
            {
                cell.IsDisabled = false;
            }
        }
    }

    public Cell getCell(int i)
    {
        return cells.ElementAt(i);
    }
    public GameRender(string UserName)
    {
        this.userName = UserName;
        for (int i = 0; i < 9; ++i)
        {
            cells.Add(new Cell());
        }
    }
}