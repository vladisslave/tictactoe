public class GameMemento
{
    private User _currentUser;
    private Board _board;


    public GameMemento(User currentUser, Board board)
    {
        _currentUser = currentUser;
        _board = board.CopyBoard();
    }

    public User CurrentUser
    {
        get
        {
            return _currentUser;
        }
    }

    public Board Board
    {
        get
        {
            return _board;
        }
    }
}