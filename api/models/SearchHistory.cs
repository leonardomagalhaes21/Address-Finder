
public static class SearchHistory
{
    private static readonly string filename = "search_history.txt";

    public static List<string> GetSearchHistory()
    {
        if (!File.Exists(filename))
        {
            return new List<string>();
        }

        return File.ReadAllLines(filename).ToList();
    }

    public static void AddSearchHistory(string postcode)
    {
        var history = GetSearchHistory();
        history.Insert(0, postcode);

        if (history.Count > 3)
        {
            history = history.Take(3).ToList();
        }

        File.WriteAllLines(filename, history);
    }
}
