namespace WordGame.Helpers;

public static class Helper
{
    public static string? Ask(string question,int guess, bool isRequired = false, string validationMsg = "Bu alanı boş bırakamazsın.")
    {
        string? response;
        do
        {
            ShowInfoMsg($"Kalan hakkın : {guess} ", ConsoleColor.Yellow);
            Console.WriteLine();
            ShowInfoMsg($"{question}: ", ConsoleColor.Cyan);
            response = Console.ReadLine();

            if (isRequired && string.IsNullOrWhiteSpace(response))
            {
                ShowErrorMsg(validationMsg);
            }
            
        } while (isRequired && string.IsNullOrWhiteSpace(response));
        
        return response?.Trim().ToUpper();
    }
    public static void ShowSuccessMsg(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.Green);
    }

    public static void ShowErrorMsg(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.Red);
    }
    public static void ShowErrorMsgChar(char c)
    {
        ShowColoredChar(c, ConsoleColor.Red);
    }
    public static void ShowSuccessMsgChar(char c)
    {
        ShowColoredChar(c, ConsoleColor.Green);
    }
    public static void ShowInfoMsg(char msg)
    {
        ShowColoredChar(msg, ConsoleColor.Gray);
    }
    public static void ShowInfoMsg(string msg, ConsoleColor color)
    {
        ShowColoredMsg(msg, color);
    }
    private static void ShowColoredMsg(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ResetColor();
    }
    private static void ShowColoredChar(Char msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write($" {msg} ");
        Console.ResetColor();
    }
}