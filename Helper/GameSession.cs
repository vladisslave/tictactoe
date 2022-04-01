[Serializable]
class GameSession
{
    public string? GameId { get; set; }
    public List<GameRenderElement>? Turns { get; set; }
}
