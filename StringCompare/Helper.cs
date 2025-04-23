namespace StringCompare;

public static class Helper
{
    public static string CompareWords(string word, string otherWord)
    {
        string result = "";
        int minLength = Math.Min(word.Length, otherWord.Length);
        for (int i = 0; i < minLength; i++) 
        { 
            if(word[i] != otherWord[i]) 
                result += word[i];
        }
        if (word.Length > otherWord.Length)
            result += AddLongString(word, minLength);
        if (word.Length < otherWord.Length)
            result += AddLongString(otherWord, minLength);
        return result;
    }

    public static string AddLongString(string word, int index)
    {
        string addResult = "";
        for (int i = index; i < word.Length; i++)
            addResult += word[i];
        return addResult;
    }

    public static string SeperateWords(string word, string seperator)
    {
        string res = "";
        for (int i = 0; i < word.Length; i++)
        {
            res += word[i];
            if (i != word.Length - 1)
                res += seperator;
        }
        return res;
    }
}