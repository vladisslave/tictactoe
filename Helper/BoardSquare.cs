public class BoardSquare
{
    private int _squareValue;

    
    public BoardSquare()
    {
        _squareValue = 0;
    }

    public int SquareValue
    {
        get
        {
            return _squareValue;
        }
        set
        {
            if (SquareValue == 0)
            {
                _squareValue = value;
                return;
            }

            throw new Exception("wrong square to change");
        }
    }

    public BoardSquare(int BoardSquareValue)
    {
        _squareValue = BoardSquareValue;
    }
}