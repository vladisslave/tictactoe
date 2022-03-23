[Serializable]
class GameSituation
{
    public string? TurnUserName { get; set; }
    public List<string>? Lst { get; set; }
    public bool Finished { get; set; }
    public string? message { get; set; }

}