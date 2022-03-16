using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;

class Room
{
    private static Room? instance;
    private HashSet<User> Users = new HashSet<User>();
    private string roomName;

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
            newUser.joinRoom(roomName);
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
    private HashSet<string> joinedRoomsNames = new HashSet<string>();

    public void joinRoom(string roomName)
    {
        joinedRoomsNames.Add(roomName);
    }

    public void JoinLobby(string lobbyName)
    {
        joinRoom(lobbyName);
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

    public static int getWaitingPlayersCount()
    {
        return waitingRoom.countPlayersInWaitRoom();
    }

    public static bool addUserToWaitingRoom(string userName, string userId)
    {
        return waitingRoom.AddUser(userName, userId);
    }



    // public static void writeMessage(string message)
    // {
    //     // File.AppendAllText(@"c:\path\file.txt", "text content" + Environment.NewLine);
    //     string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    //     dirPath = Path.Combine(dirPath, "БС подготов очка", "csharp", "tictactoe", "tictactoe");
    //     string fileName = "messages.txt";
    //     Task asyncTask = WriteFileAsync(dirPath, fileName, message);
    //     Console.WriteLine(dirPath);
    //     Console.WriteLine(fileName);

    // }

    //  static async Task WriteFileAsync(string dir, string file, string content)
    //     {
    //         Console.WriteLine("Async Write File has started.");
    //         using(StreamWriter outputFile = new StreamWriter(Path.Combine(dir, file), append: true) )
    //         {
    //             await outputFile.WriteAsync(content + Environment.NewLine);
    //         }
    //         Console.WriteLine("Async Write File has completed.");
    //     }
}

