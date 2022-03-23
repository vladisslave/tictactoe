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


