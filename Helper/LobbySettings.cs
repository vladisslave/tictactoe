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