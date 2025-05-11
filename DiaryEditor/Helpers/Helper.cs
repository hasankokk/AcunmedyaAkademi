using DiaryEditor.Validation;

namespace DiaryEditor.Helpers;

public static class Helper
{
    private static readonly IsValid _isValid = new IsValid(); 
    public static string? Ask(string question, bool isRequired = false, string validationMsg = "Bu alanı boş bırakamazsın.")
    {
        string? response;
        bool isValid;
        do
        {
            ShowInfoMsg($"{question}: ");
            response = Console.ReadLine();
            isValid = true;
            if (isRequired && string.IsNullOrWhiteSpace(response))
            {
                ShowErrorMsg(validationMsg);
                isValid = false;
            }
            
            if (question == "Kullanıcı Adı Giriniz" && !_isValid.IsValiduserName(response, out string errorusername))
            {
                ShowErrorMsg(errorusername);
                isValid = false;
            }

            if ((question == "Adınızı Giriniz" || question == "Soyadınızı Giriniz") && !IsValid.IsValidName(response, out string errorname))
            {
                ShowErrorMsg(errorname);
                isValid = false;
            }

            if (question == "E-Mail Adresinizi Giriniz" && !_isValid.IsValidMail(response, out string errormail))
            {
                ShowErrorMsg(errormail);
                isValid = false;
            }

            if (question == "Kullanıcı Adı" &&
                !_isValid.IsValidLogin(response, out string errorLogin, out bool isRegister))
            {
                if (!isRegister)
                    ShowErrorMsg(errorLogin);
                if (isRegister)
                {
                    ShowErrorMsg(errorLogin);
                    UserHelper.RegisterForm();
                }
                isValid = false;
                    
            }
        } while (!isValid);
        
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

        ShowInfoMsgLine(question);
        
        for (int i = 0; i < options.Length; i++)
        {
            ShowInfoMsgLine($"{i + 1}. {options[i]}");
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

    public static string AskPassword(string question, string validationMsg = "Bu alanı boş bırakamazsın.")
    {
        var password = string.Empty;
        bool isValidPass;
        do
        {
            ShowInfoMsg($"{question}: ");
            password = ReadSecretLine();
            isValidPass = true;
            
            if (string.IsNullOrWhiteSpace(password))
            {
                ShowErrorMsg(validationMsg);
                isValidPass = false;
            }

            if (!IsValid.IsValidPassword(password, out string errorpass))
            {
                ShowErrorMsg(errorpass);
                isValidPass = false;
            }
            
        } while (!isValidPass);
        
        return HashHelper.HashPassword(password);
    }
    public static bool AskLoginPassword(string username, string question, string validationMsg = "Bu alanı boş bırakamazsın.")
    {
        var password = string.Empty;
        bool isValidPass;
        do
        {
            ShowInfoMsg($"{question}: ");
            password = ReadSecretLine();
            isValidPass = true;
            
            if (string.IsNullOrWhiteSpace(password))
            {
                ShowErrorMsg(validationMsg);
                isValidPass = false;
            }
            
            if (!_isValid.IsValidLoginPassword(username, password, out string errorlogin))
            {
                ShowErrorMsg(errorlogin);
                isValidPass = false;
            }

            if (!isValidPass)
            {
                var forgotPass = AskOption("Şifreni mi unuttun? Sıfırlamak ister misin?", ["Evet", "Hayır"]);
                if (forgotPass == 1)
                {
                    Thread.Sleep(1000);
                    Console.Clear();
                    if (UserHelper.ForgotPassword(username))
                        ShowSuccessMsg("Şifre yenileme işlemi başarılı tekrardan giriş yapınız.");
                }
            }
            
        } while (!isValidPass);

        if (isValidPass)
        {
            ShowSuccessMsg("Giriş Başarılı!");
            Thread.Sleep(1000);
            return isValidPass;
        }

        return false;
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
        ShowWriteColor(msg, ConsoleColor.Yellow);
    }
    public static void ShowInfoMsgLine(string msg)
    {
        ShowColoredMsg(msg, ConsoleColor.Yellow);
    }
    public static void ShowMsg(string msg, ConsoleColor color)
    {
        ShowWriteColor(msg, color);
    }
    private static string ReadSecretLine()
    {
        var line = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;
            if (key.Key == ConsoleKey.Backspace && line.Length > 0)
            {
                line = line.Substring(0, line.Length - 1);
                Console.Write("\b \b");
                continue;
            }
            
            line += key.KeyChar;

            Console.Write(key.KeyChar);
            Thread.Sleep(75);
            Console.Write("\b*");

        } while (true);
        Console.WriteLine();
        return line;
    }

    private static void ShowColoredMsg(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    private static void ShowWriteColor(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ResetColor();
    }
    
}