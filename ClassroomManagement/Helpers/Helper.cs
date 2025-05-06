namespace ClassroomManagement.Helpers;

public static class Helper
{
    public static int MenuMessage(string message)
    {
        if (message == "firstMenu")
        {
            int whichSelect = AskOption("Ne yapmak istersiniz?", ["Öğrenci Yönetimi", "Öğretmen Yönetimi", "Sınıf Yönetimi", "Çıkış Yap"]);
            return whichSelect;
        }
        if (message == "StudentManagement")
        {
            int studenSelect = AskOption("Yapmak istediğiniz işlem nedir?",
                ["Öğrencileri Listele", "Öğrenci Ekleme", "Öğrenci Silme"]);
            return studenSelect;
        }

        if (message == "TeacherManagement")
        {
            int teacherSelect = AskOption("Yapmak istediğiniz işlem nedir?",
                ["Öğretmenleri Listele", "Öğretmen Ekleme", "Öğretmen Silme"]);
            return teacherSelect;
        }

        if (message == "ClassManagement")
        {
            int classSelect = AskOption("Yapmak istediğiniz işlem nedir?",
                ["Sınıfları Listele", "Sınıf Ekleme", "Sınıf Silme"]);
            return classSelect;
        }
        return 0;
    }
    public static string? Ask(string question, bool isRequired = false, string validationMsg = "Bu alanı boş bırakamazsın.")
    {
        string? response;
        do
        {
            ShowInfoMsgNoLine($"{question}: ");
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

        ShowInfoMsg(question);
        
        for (int i = 0; i < options.Length; i++)
        {
            ShowInfoMsg($"{i + 1}. {options[i]}");
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

    public static string AskClass(string question, string[] options)
    {
        if (options.Length == 0)
        {
            throw new ArgumentException($"{nameof(options)} içinde seçenekler olmalı.", nameof(options));
        }

        Console.WriteLine(question);
    
        for (int i = 0; i < options.Length; i++)
        {
            ShowInfoMsg($"{i + 1}. {options[i]}");
        }

        while (true)
        {
            var inputResponse = AskNumber($"Seçimin (1-{options.Length})");
            if (inputResponse >= 1 && inputResponse <= options.Length)
            {
                
                return options[inputResponse - 1];
            }
            else
            {
                ShowErrorMsg("Hatalı seçim yaptın.");
            }
        }
    }

    public static string AskPassword(string question, string validationMsg = "Bu alanı boş bırakamazsın.")
    {
        var password = "";
        do
        {
            Console.Write($"{question}: ");
            password = ReadSecretLine();
            
            if (string.IsNullOrWhiteSpace(password))
            {
                ShowErrorMsg(validationMsg);
            }
            
        } while (string.IsNullOrWhiteSpace(password));
        
        return password;
    }
    
    public static void ShowSuccessMsg(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.Green);
    }

    public static void ShowErrorMsg(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.Red);
    }

    public static void ShowInfoMsg(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.Yellow);
    }
    public static void ShowInfoMsgNoLine(string msg)
    {
        ShowColoredMsgNoLine(msg, ConsoleColor.Yellow);
    }

    private static string ReadSecretLine()
    {
        var line = "";
        ConsoleKeyInfo key;
        do
        {
            key =  Console.ReadKey(true);

            if (key.Key == ConsoleKey.Backspace && line.Length > 0)
            {
                line = line.Substring(0, line.Length - 1);
                Console.Write("\b \b");
                continue;
            }
            
            if (!IsSecurePassChar(key.KeyChar))
            {
                continue;
            }
            
            line += key.KeyChar;

            Console.Write(key.KeyChar);
            Thread.Sleep(75);
            Console.Write("\b*");

        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine("\n");
        return line;
    }

    private static void ShowColoredMsg(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    private static void ShowColoredMsgNoLine(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ResetColor();
    }
    public static void ShowColoredMsgManuel(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    private static bool IsSecurePassChar(char c)
    {
        return char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c) || char.IsWhiteSpace(c);
    }
}