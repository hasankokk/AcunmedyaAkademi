namespace WordGame.Helpers;

public static class WordGameHelper
{
    public static async Task<string> LoadWord()
    {
        var wordList = new List<string>();
        try
        {
            using var reader = new StreamReader("words.txt");
            var lines = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(lines))
                throw new Exception("Geçerli bir satır okunamadı.");
            foreach (var line in lines.Split(','))
                wordList.Add(line);
            Random random = new Random();
            int randomIndex = random.Next(0, wordList.Count);
            string randomWord = wordList[randomIndex];
            return randomWord;
        }
        catch (Exception e)
        {
            Helper.ShowErrorMsg(e.ToString());
            throw;
        }
    }
    
    public static void ForecastPrint(string word, List<string> inputWord)
    {
        if (inputWord == null)
            return;
        foreach (var guess in inputWord)
        {
            Console.Write(" \t \t \t");
            for (int i = 0; i < word.Length; i++)
            {
                Console.Write("|");
                char c = guess[i];

                if (word[i] == c)
                    Helper.ShowSuccessMsgChar(c);
                else if (word.Contains(c))
                    Helper.ShowErrorMsgChar(c);
                else
                    Helper.ShowInfoMsg(c);
            }
            Console.Write("|");
            Console.WriteLine();
        }
    }
}