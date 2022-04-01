public class GameMemory
{
    private GameMemento _gameMemento;


    public GameMemory(GameMemento gameMemento)
    {
        _gameMemento = gameMemento;
    }

    public GameMemento GameMemento
    {
        get
        {
            return _gameMemento;
        }
        set
        {
            _gameMemento = value;
        }
    }
}
