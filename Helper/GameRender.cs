class GameRender
{
    private string _userName;
    private List<Cell> _cells = new List<Cell>();


    public GameRender(string userName)
    {
        _userName = userName;
        for (int i = 0; i < 9; ++i)
        {
            _cells.Add(new Cell());
        }
    }

    public void ParseSituation(GameRenderElement gameRenderElement)
    {
        for (int i = 0; i < 9; ++i)
        {
            if (gameRenderElement.CellValueList != null)
                GetCell(i).Value = gameRenderElement.CellValueList[i];
        }

        if (gameRenderElement.TurnUserName == _userName)
        {
            DisableActiveСells();
        }
        if (gameRenderElement.TurnUserName != _userName)
        {
            DisableAllCells();
        }
    }

    public void DisableAllCells()
    {
        foreach (Cell cell in _cells)
        {
            cell.IsDisabled = true;
        }
    }

    public void DisableActiveСells()
    {
        foreach (Cell cell in _cells)
        {
            if (cell.Value != String.Empty)
            {
                cell.IsDisabled = true;
            }
            else
            {
                cell.IsDisabled = false;
            }
        }
    }

    public Cell GetCell(int i)
    {
        return _cells.ElementAt(i);
    }

    public class Cell
    {
        private bool isDisabled = true;
        public bool IsDisabled { get { return isDisabled; } set { isDisabled = value; } }
        private string value = "";
        public string Value { get { return value; } set { this.value = value; } }
    }
}