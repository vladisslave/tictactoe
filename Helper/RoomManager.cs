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
