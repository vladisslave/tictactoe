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