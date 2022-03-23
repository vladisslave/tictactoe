using Newtonsoft.Json;
static class GameSessionWriter
{
    public static List<GameSession>? gSList;
    public static void writeSession(GameSession gameSession)
    {
        string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        dirPath = Path.Combine(dirPath, "БС подготов очка", "csharp", "tictactoe", "tictactoe");
        string fileName = "sessions.json";

        string filePath = Path.Combine(dirPath, fileName);

        string text = File.ReadAllText(filePath);
        gSList = JsonConvert.DeserializeObject<List<GameSession>>(text);
        if (gSList == null)
        {
            gSList = new List<GameSession>();
        }
        GameSession? foundGS = gSList?.Find(gS => gS.GameId == gameSession.GameId);
        if (foundGS == null)
        {
            gSList?.Add(gameSession);
        }
        else
        {
            foundGS.Turns = gameSession.Turns;
        }

        text = JsonConvert.SerializeObject(gSList);
        Console.WriteLine(text);

        Task asyncTask = WriteFileAsync(dirPath, fileName, text);
        Console.WriteLine(dirPath);
        Console.WriteLine(fileName);

    }

    static async Task WriteFileAsync(string dir, string file, string content)
    {
        Console.WriteLine("Async Write File has started.");
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(dir, file), append: false))
        {
            await outputFile.WriteAsync(content);
        }
        Console.WriteLine("Async Write File has completed.");
    }
}