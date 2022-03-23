[Serializable]
class GameSession
{
    public string? GameId { get; set; }
    public List<GameSituation>? Turns { get; set; }
}