namespace StudentManagement.Helpers;

public static class Helpers
{
    public static void MenuMessage()
    {
        PrintMenuMessage("\n1 - Öğrenci Ekle\n" +
                       "2 - Öğrencileri Listele\n" +
                       "3 - Öğrenci Sil\n" +
                       "4 - Öğrenci Bilgisi Güncelleme\n" +
                       "5 - Çıkış Yap.");
    }
    public static string? Ask(string question, bool isRequired = false, string validationMsg = "Bu alanı boş bırakamazsın.")
    {
        string? response;
        do
        {
            Console.Write($"{question}: ");
            response = Console.ReadLine();

            if (isRequired && string.IsNullOrWhiteSpace(response))
            {
                ShowErrorMsg(validationMsg);
            }
            
        } while (isRequired && string.IsNullOrWhiteSpace(response));
        
        return response?.Trim();
    }

    public static int AskNumber(string question, string validationMsg = "Bir sayı girmelisin.")
    {
        while (true)
        {
            var response = Ask(question, true);
            if (int.TryParse(response, out var result))
            {
                return result;
            }
            ShowErrorMsg(validationMsg);
        }
    }

    public static int AskOption(string question, string[] options)
    {
        if (options.Length == 0)
        {
            throw new ArgumentException($"{nameof(options)} içinde seçenekler olmalı.", nameof(options));
        }

        Console.WriteLine(question);
        
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }

        while (true)
        {
            var inputResponse = AskNumber($"Seçimin (1-{options.Length})");
            if (inputResponse >= 1 && inputResponse <= options.Length)
            {
                return inputResponse;
            }
    
            ShowErrorMsg("Hatalı seçim yaptın.");
        }
    }

    private static void ShowColoredMsg(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ResetColor();
    }
    
    public static void ShowSuccessMsg(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.Green);
    }

    public static void ShowErrorMsg(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.Red);
    }
    public static void PrintMenuMessage(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.DarkRed);
    }
    
    public static void PrintInputMessage(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.Cyan);
    }
    public static void PrintInputMessage(string msg, ConsoleColor color)
    {
        ShowColoredMsg(msg, color);
    }
}