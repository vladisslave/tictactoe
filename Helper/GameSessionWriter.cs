using Newtonsoft.Json;
static class GameSessionWriter
{
    public static List<GameSession>? _gameSessionList;
    private static string _filePath;

    static GameSessionWriter()
    {
        _filePath = InitializeFilePath();
    }

    public static void WriteSession(GameSession gameSession)
    {
        string sessionsJson = File.ReadAllText(_filePath);
        _gameSessionList = JsonConvert.DeserializeObject<List<GameSession>>(sessionsJson);

        if (_gameSessionList == null)
        {
            _gameSessionList = new List<GameSession>();
        }

        GameSession? foundGS = _gameSessionList?.Find(gS => gS.GameId == gameSession.GameId);

        if (foundGS == null)
        {
            _gameSessionList?.Add(gameSession);
        }
        else
        {
            foundGS.Turns = gameSession.Turns;
        }

        sessionsJson = JsonConvert.SerializeObject(_gameSessionList);
        Task asyncTask = WriteFileAsync(_filePath, sessionsJson);
    }

    private static async Task WriteFileAsync(string filePath, string content)
    {
        using (StreamWriter outputFile = new StreamWriter(filePath, append: false))
        {
            await outputFile.WriteAsync(content);
        }
    }
    
    private static string InitializeFilePath()
    {
        string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        dirPath = Path.Combine(dirPath, "БС подготов очка", "csharp", "TicTocShue");
        string fileName = "sessions.json";

        return Path.Combine(dirPath, fileName);
    }
}
