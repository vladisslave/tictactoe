class GameRender
{
    private string userName;
    private List<Cell> cells = new List<Cell>();

    public class Cell
    {
        private bool isDisabled = true;
        public bool IsDisabled { get { return isDisabled; } set { isDisabled = value; } }
        private string value = "";
        public string Value { get { return value; } set { this.value = value; } }
    }

    public void ParseSituation(GameSituation situation)
    {
        for (int i = 0; i < 9; ++i)
        {
            getCell(i).Value = situation!.Lst!.ElementAt(i);
        }

        if (situation.TurnUserName == userName)
        {
            DisableActiveCells();
        }
        if (situation.TurnUserName != userName)
        {
            DisableAllCells();
        }
    }

    public void DisableAllCells()
    {
        foreach (Cell cell in cells)
        {
            cell.IsDisabled = true;
        }
    }

    public void DisableActiveCells()
    {
        foreach (Cell cell in cells)
        {
            if (cell.Value != "")
            {
                cell.IsDisabled = true;
            }
            else
            {
                cell.IsDisabled = false;
            }
        }
    }

    public Cell getCell(int i)
    {
        return cells.ElementAt(i);
    }
    public GameRender(string UserName)
    {
        this.userName = UserName;
        for (int i = 0; i < 9; ++i)
        {
            cells.Add(new Cell());
        }
    }
}
