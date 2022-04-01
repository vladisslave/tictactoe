public class Board
{
    private readonly int _squaresNumber;
    private BoardState _boardState;
    private List<BoardSquare> _boardSquareList;


    public Board()
    {
        _boardSquareList = new List<BoardSquare>();
        _squaresNumber = 9; // 3x3 Board
        _boardState = BoardState.Unfinished;

        for (int squareNumber = 0; squareNumber < _squaresNumber; ++squareNumber)
        {
            _boardSquareList.Add(new BoardSquare());
        }
    }

    public List<BoardSquare> BoardSquaresList
    {
        get
        {
            return _boardSquareList;
        }
    }

    public BoardState BoardState
    {
        get
        {
            return _boardState;
        }
    }

    public bool MakeUserXTurn(int squareNumber)
    {
        return MakeUserTurn(squareNumber, 1);
    }

    public bool MakeUserOTurn(int squareNumber)
    {
        return MakeUserTurn(squareNumber, -1);
    }

    private bool MakeUserTurn(int squareNumber, int squareValue)
    {
        if (_boardState != BoardState.Unfinished)
        {
            return false;
        }

        try
        {
            _boardSquareList[squareNumber].SquareValue = squareValue;

            CalculateState();
            return true;
        }
        catch
        {

        }

        return false;
    }

    public void CalculateState()
    {
        bool hasWinner = CheckForWinner();

        if (!hasWinner)
        {
            CheckForDraw();
        }
    }

    public bool CheckForWinner()
    {
        int[,] winnerCombination = new int[,] {
            {0, 1, 2},
            {3, 4, 5},
            {6, 7, 8},
            {0, 3, 6},
            {1, 4, 7},
            {2, 5, 8},
            {0, 4, 8},
            {2, 4, 6}
        };

        for (int x = 0; x < winnerCombination.GetLength(0); x += 1)
        {
            int el1 = _boardSquareList[winnerCombination[x, 0]].SquareValue;
            int el2 = _boardSquareList[winnerCombination[x, 1]].SquareValue;
            int el3 = _boardSquareList[winnerCombination[x, 2]].SquareValue;

            if (el1 == el2 && el2 == el3 && el3 == 1)
            {
                _boardState = BoardState.XWin;

                return true;
            }
            else if (el1 == el2 && el2 == el3 && el3 == -1)
            {
                _boardState = BoardState.OWin;

                return true;
            }
        }

        return false;
    }

    public void CheckForDraw()
    {
        bool isDraw = true;

        for (int squareNumber = 0; squareNumber < _boardSquareList.Count(); ++squareNumber)
        {
            int squareValue = _boardSquareList[squareNumber].SquareValue;

            if (squareValue == 0)
            {
                isDraw = false;
            }
        }

        if (isDraw)
        {
            _boardState = BoardState.Draw;
        }
    }

    public Board CopyBoard()
    {
        Board board = new Board();

        for (int squareNumber = 0; squareNumber < _squaresNumber; ++squareNumber)
        {
            board._boardSquareList[squareNumber] = new BoardSquare(this._boardSquareList[squareNumber].SquareValue);
            board._boardState = this._boardState;
        }

        return board;
    }
}