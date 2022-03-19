using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;

class Room
{
    private static Room? instance;
    private HashSet<User> users = new HashSet<User>();
    private string roomName;

    public HashSet<User> Users { get { return users; } }
    public string RoomName { get { return roomName; } }

    public static Room getInstance(string roomName)
    {
        if (instance == null)
        {
            instance = new Room(roomName);
        }
        return instance;
    }

    private Room(string roomName)
    {
        this.roomName = roomName;
    }
    public bool AddUser(string userName, string userId)
    {
        bool isUnique = true;
        User newUser = new User(userName, userId);

        foreach (User user in Users)
        {
            if (user.UserName == userName)
            {
                isUnique = false;
            }
        }

        if (isUnique)
        {
            Users.Add(newUser);
        }

        return isUnique;
    }

    public int countPlayersInWaitRoom()
    {
        int count = 0;
        foreach (User user in Users)
        {
            if (user.IsInLobby == false)
            {
                count++;
            }
        }
        return count;
    }
}

class User
{
    private string userName;
    private string userId;
    private bool isInLobby = false;
    private HashSet<string> joinedLobbiesNames = new HashSet<string>();

    public void JoinLobby(string lobbyName)
    {
        joinedLobbiesNames.Add(lobbyName);
        isInLobby = true;
    }


    public string UserName { get { return userName; } }
    public string UserId { get { return userId; } }
    public bool IsInLobby { get { return isInLobby; } }


    public User(string userName, string userId)
    {
        this.userName = userName;
        this.userId = userId;
    }
}
static class RoomManager
{
    static Room waitingRoom = Room.getInstance("WaitRoom");
    static List<Lobby> lobbies = new List<Lobby>();

    public static void addLobby(Lobby lobby)
    {
        lobbies.Add(lobby);
    }

    public static List<Lobby> returnLobbyList()
    {
        return lobbies;
    }

    public static int getWaitingPlayersCount()
    {
        return waitingRoom.countPlayersInWaitRoom();
    }

    public static void deleteUserFromRooms(string userId)
    {
        foreach (User user in waitingRoom.Users)
        {
            if (user.UserId == userId)
            {
                //delete from lobbies
            }
        }
        waitingRoom.Users.RemoveWhere(user => user.UserId == userId);
    }

    public static User findUser(string userId)
    {
        User? foundUser = null;
        foreach (User user in waitingRoom.Users)
        {
            if (user.UserId == userId)
            {
                foundUser = user;
            }
        }
        if (foundUser != null)
        {
            return foundUser;
        }
        throw new Exception("No such user with userId was found");
    }

    public static Lobby findLobby(int id)
    {
        Lobby? foundLobby = lobbies.Find(lobby => lobby.LobbyId == id);
        if (foundLobby != null)
        {
            return foundLobby;
        }
        throw new Exception("No such lobby with lobbyId was found");
    }

    public static bool addUserToWaitingRoom(string userName, string userId)
    {
        return waitingRoom.AddUser(userName, userId);
    }




}

class UniquenessError
{
    private string errorDetails;

    public string ErrorDetails { get { return errorDetails; } }
    public UniquenessError(string errorDetails)
    {
        this.errorDetails = errorDetails;
    }
}

[Serializable]
class GameSituation
{
    // private string turnUserName;
    // private List<string> lst = new List<string>();
    public string TurnUserName { get; set; }
    public List<string> Lst { get; set; }
    public bool Finished { get; set; }
    public string message { get; set; }

}

class Game
{
    public User? firstTurnUser;
    public User? secondTurnUser;
    private List<int> lst = new List<int>();
    public List<int> Lst { get { return lst; } }
    public Game()
    {
        for (int i = 0; i < 9; ++i)
        {
            lst.Add(0);
        }
    }
    public GameSituation MakeTurn(int cellNum, string userId)
    {
        lst[cellNum] = userId == firstTurnUser?.UserId ? 1 : -1;
        return new GameSituation
        {
            Lst = Game.convertSituationList(lst),
            TurnUserName = userId == firstTurnUser?.UserId ? secondTurnUser!.UserName : firstTurnUser!.UserName,
            Finished = IsWin(),
            message = GetMessage(userId)
        };
    }
    public string GetMessage(string userId)
    {
        string message = IsWin() ? (userId == firstTurnUser!.UserId ? firstTurnUser!.UserName : secondTurnUser!.UserName) : null;
        if (message == null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (lst[i] == 0)
                {
                    string TurnUserName = userId == firstTurnUser?.UserId ? secondTurnUser!.UserName : firstTurnUser!.UserName;
                    TurnUserName += "`s turn!";
                    return TurnUserName;
                }
            }
            return "Draw";
        }
        return message;
    }
    public void setUsers(User first, User second)
    {
        firstTurnUser = first;
        secondTurnUser = second;
    }
    private bool IsWin()
    {
        int[,] wC = new int[,] {
            {0, 1, 2},
            {3, 4, 5},
            {6, 7, 8},
            {0, 3, 6},
            {1, 4, 7},
            {2, 5, 8},
            {0, 4, 8},
            {2, 4, 6}
        };

        for (int x = 0; x < 8; x += 1)
        {
            int y = 0;

            if (
                (
                    (lst.ElementAt(wC[x, y]) == lst.ElementAt(wC[x, y + 1]))
            &&
             lst.ElementAt((wC[x, y + 1])) == lst.ElementAt(wC[x, y + 2])
            &&
             (lst.ElementAt(wC[x, y]) == 1)
             )
            ||
             (
                 (lst.ElementAt(wC[x, y]) == lst.ElementAt(wC[x, y + 1]))
            &&
             (lst.ElementAt(wC[x, y + 1]) == lst.ElementAt(wC[x, y + 2]))
            &&
             (lst.ElementAt(wC[x, y]) == -1)
             )
             )
            {
                return true;
            }

        }
        return false;
    }
    public static List<string> convertSituationList(List<int> intLst)
    {
        List<string> lst = new List<string>();
        for (int i = 0; i < 9; ++i)
        {
            if (intLst.ElementAt(i) == 0)
            {
                lst.Add("");
            }
            else if (intLst.ElementAt(i) == 1)
            {
                lst.Add("x");
            }
            else
            {
                lst.Add("o");
            }
        }
        return lst;
    }
}

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

class LobbySettings
{
    private TurnType firstTurn;
    public TurnType FirstTurn { get { return firstTurn; } }
    public LobbySettings(string turnType)
    {
        switch (turnType)
        {
            case "0":
                firstTurn = TurnType.Random;
                break;

            case "1":
                firstTurn = TurnType.Creator;
                break;

            case "2":
                firstTurn = TurnType.Guest;
                break;

            default:
                break;
        }
    }
}

enum TurnType
{
    Random,
    Creator,
    Guest
}


class LobbyIdAndName
{
    public string? name;
    public int id;

    public LobbyIdAndName(int _id, string _name)
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

    public void ParseSituation(GameSituation situation)
    {
        for (int i = 0; i < 9; ++i)
        {
            getCell(i).Value = situation.Lst.ElementAt(i);
        }

        if (situation.TurnUserName == userName)
        {
            DisableActiveCells();
        }
        if (situation.TurnUserName != userName)
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
            if (cell.Value != "")
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

class Logger
{
    public static void writeMessage(string message)
    {
        // File.AppendAllText(@"c:\path\file.txt", "text content" + Environment.NewLine);
        string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        dirPath = Path.Combine(dirPath, "БС подготов очка", "csharp", "tictactoe", "tictactoe");
        string fileName = "messages.txt";
        Task asyncTask = WriteFileAsync(dirPath, fileName, message + " :" + DateTime.Now.ToString("HH:mm"));
        Console.WriteLine(dirPath);
        Console.WriteLine(fileName);

    }

    static async Task WriteFileAsync(string dir, string file, string content)
    {
        Console.WriteLine("Async Write File has started.");
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(dir, file), append: true))
        {
            await outputFile.WriteAsync(content + Environment.NewLine);
        }
        Console.WriteLine("Async Write File has completed.");
    }
}
