public class LobbySettings
{
    private TurnType _firstTurn;

    public LobbySettings(string turnType)
    {
        switch (turnType)
        {
            case "0":
                _firstTurn = TurnType.Random;
                break;

            case "1":
                _firstTurn = TurnType.Creator;
                break;

            case "2":
                _firstTurn = TurnType.Guest;
                break;

            default:
                _firstTurn = TurnType.Random;
                break;
        }
    }

    public TurnType FirstTurn
    {
        get
        {
            return _firstTurn;
        }
    }
}