public interface IUser
{
    public string UserName { get; }
    public string UserId { get; }

    public bool JoinLobby(string lobbyId);
    public void CreateLobby(LobbySettings lobbySettings);
}