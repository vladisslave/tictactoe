class Game
{
    public User? firstTurnUser;
    public GameSession gameSession = new GameSession();
    public User? secondTurnUser;
    private List<int> lst = new List<int>();
    public List<int> Lst { get { return lst; } }
    public Game()
    {
        for (int i = 0; i < 9; ++i)
        {
            lst.Add(0);
        }
    }
    public GameSituation MakeTurn(int cellNum, string userId)
    {
        lst[cellNum] = userId == firstTurnUser?.UserId ? 1 : -1;
        bool finished = IsWin();
        GameSituation gameSituation = new GameSituation
        {
            Lst = Game.convertSituationList(lst),
            TurnUserName = userId == firstTurnUser?.UserId ? secondTurnUser!.UserName : firstTurnUser!.UserName,
            Finished = finished,
            message = GetMessage(userId, finished)
        };

        gameSession.Turns?.Add(gameSituation);
        GameSessionWriter.writeSession(gameSession);

        return gameSituation;
    }
    public string GetMessage(string userId, bool finished)
    {
        string? message = finished ? (userId == firstTurnUser!.UserId ? firstTurnUser!.UserName : secondTurnUser!.UserName) : null;
        if (message == null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (lst[i] == 0)
                {
                    string TurnUserName = userId == firstTurnUser?.UserId ? secondTurnUser!.UserName : firstTurnUser!.UserName;
                    TurnUserName += "`s turn!";
                    return TurnUserName;
                }
            }
            return "Draw";
        }
        return message;
    }
    public void setUsers(User first, User second)
    {
        firstTurnUser = first;
        secondTurnUser = second;
        gameSession.GameId = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") +
        Environment.NewLine + first.UserName + " VS " + second.UserName;
        gameSession.Turns = new List<GameSituation>();
    }
    private bool IsWin()
    {
        int[,] wC = new int[,] {
            {0, 1, 2},
            {3, 4, 5},
            {6, 7, 8},
            {0, 3, 6},
            {1, 4, 7},
            {2, 5, 8},
            {0, 4, 8},
            {2, 4, 6}
        };

        for (int x = 0; x < 8; x += 1)
        {
            int y = 0;

            if (
                (
                    (lst.ElementAt(wC[x, y]) == lst.ElementAt(wC[x, y + 1]))
            &&
             lst.ElementAt((wC[x, y + 1])) == lst.ElementAt(wC[x, y + 2])
            &&
             (lst.ElementAt(wC[x, y]) == 1)
             )
            ||
             (
                 (lst.ElementAt(wC[x, y]) == lst.ElementAt(wC[x, y + 1]))
            &&
             (lst.ElementAt(wC[x, y + 1]) == lst.ElementAt(wC[x, y + 2]))
            &&
             (lst.ElementAt(wC[x, y]) == -1)
             )
             )
            {
                return true;
            }

        }
        return false;
    }
    public static List<string> convertSituationList(List<int> intLst)
    {
        List<string> lst = new List<string>();
        for (int i = 0; i < 9; ++i)
        {
            if (intLst.ElementAt(i) == 0)
            {
                lst.Add("");
            }
            else if (intLst.ElementAt(i) == 1)
            {
                lst.Add("x");
            }
            else
            {
                lst.Add("o");
            }
        }
        return lst;
    }
}