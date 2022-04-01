[Serializable]
public class GameRenderElement
{
    public string? TurnUserName { get; set; }
    public List<string>? CellValueList { get; set; }
    public bool IsFinished { get; set; }
    public string? Message { get; set; }

    public GameRenderElement()
    {

    }
    public GameRenderElement(Game game)
    {
        TurnUserName = game.CurrentUser.UserName;
        CellValueList = ConvertSquareListToStringList(game.Board.BoardSquaresList);

        if (game.Board.BoardState != BoardState.Unfinished)
        {
            IsFinished = true;
        }
        else
        {
            IsFinished = false;
        }

        Message = CreateMessage(game);
    }

    private string CreateMessage(Game game)
    {
        if (game.Board.BoardState == BoardState.OWin)
        {
            return game.UserO.UserName + " has won!";
        }
        if (game.Board.BoardState == BoardState.XWin)
        {
            return game.UserX.UserName + " has won!";
        }
        if (game.Board.BoardState == BoardState.Unfinished)
        {
            return game.CurrentUser.UserName + "`s turn!";
        }

        return "Draw";
    }

    public List<string> ConvertSquareListToStringList(List<BoardSquare> boardSquareList)
    {
        List<string> cellValueList = new List<string>();

        foreach (BoardSquare square in boardSquareList)
        {
            if (square.SquareValue == 1)
            {
                cellValueList.Add("x");
            }
            else if (square.SquareValue == -1)
            {
                cellValueList.Add("o");
            }
            else
            {
                cellValueList.Add(String.Empty);
            }
        }

        return cellValueList;
    }
}
