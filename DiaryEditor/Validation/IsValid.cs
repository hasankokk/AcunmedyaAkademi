using DiaryEditor.Data;
using DiaryEditor.Models;
using DiaryEditor.Data;
using DiaryEditor.Helpers;
using DiaryEditor.Repository;

namespace DiaryEditor.Validation;

public class IsValid
{
    private readonly AppDbContext _context = new AppDbContext();
    private readonly UserRepository _userRepository = new UserRepository();
    public bool IsValiduserName(string name, out string error)
    {
        error = "";
        if (name.Length < 5 || name.Length > 20)
        {
            error = "Kullanıcı adı en az 5 en çok 20 karakter olmalıdır!";
            return false;
        }

        if (_userRepository.GetUser(name))
        {
            error = "Kullanıcı adı zaten kullanılıyor!";
            return false;
        }

        foreach (var c in name)
        {
            if (!char.IsLetterOrDigit(c) && c != '-' && c != '_')
            {
                error = "Kullanıcı adınız sadece harf, rakam ve \"_\", \"-\" oluşmalıdır.";
                return false;
            }
        }
        return true;
    }

    public static bool IsValidName(string name, out string error)
    {
        error = "";
        if (name.Length < 2 || name.Length > 30)
        {
            error = "Adınız ve soyadınız en az 2 en çok 30 karakter olmalıdır!";
            return false;
        }
        
        foreach (var c in name)
        {
            if (!char.IsLetter(c) && c != ' ')
            {
                error = "Adınız ve soyadınız sadece harflerden ve Boşluklardan oluşabilir.";
                return false;
            }
        }
        return true;
    }

    public static bool IsValidPassword(string password, out string errorpass)
    {
        errorpass = "";
        if (password.Length < 8 || password.Length > 32)
        {
            errorpass = "Şifreniz 8 ile 32 karakter arasında olmalıdır!";
            return false;
        }

        bool passUpper = false, passLower = false, passDigit = false, passSpecial = false, passWhiteSpace = false;
        string specialChar = "!@_-+?=&*#$'\"";
        foreach (var c in password)
        {
            if (char.IsUpper(c)) passUpper = true;
            else if (char.IsLower(c)) passLower = true;
            else if (char.IsDigit(c)) passDigit = true;
            else if (specialChar.Contains(c.ToString())) passSpecial = true;
            if (char.IsWhiteSpace(c)) passWhiteSpace = true;

        }
        if (!passUpper)
        {
            errorpass = "Şifreniz en az 1 büyük harf (A-Z) içermelidir.";
            return false;
        }

        if (!passLower)
        {
            errorpass = "Şifreniz en az 1 küçük harf (a-z) içermelidir.";
            return false;
        }

        if (!passDigit)
        {
            errorpass = "Şifreniz en az 1 rakam (0-9)içermelidir.";
            return false;
        }

        if (!passSpecial)
        {
            errorpass = "Şifreniz en az 1 özel karakter (!@_-+?=&*#$'\") içermelidir.";
            return false;
        }

        if (passWhiteSpace)
        {
            errorpass = "Şifreniz boşluk içeremez!";
            return false;
        }
        return true;
    }

    public bool IsValidMail(string mail, out string errormail)
    {
        errormail = "";
        var domain = mail.IndexOf('@');
        if (domain <= 0 || domain == mail.Length - 1)
        {
            errormail = "Adresin ve alan adı arasında kesinlikle '@' olmalıdır";
            return false;
        }

        var mailPart = mail.Substring(0, domain);
        var domainPart = mail.Substring(domain + 1);

        if (_context.Users.Any(u => u.Email == mailPart))
        {
            errormail = "Mail adresi zaten sistemimizde kayıtlı!";
            return false;
        }
        if (domainPart.StartsWith(".") || domainPart.EndsWith("."))
        {
            errormail = "Geçersiz bir domain alanı girdin. Alan adı kısmı . ile başlayıp . ile bitemez.";
            return false;
        }

        if (!domainPart.Contains("."))
        {
            errormail = "Alan adı kısmı en az 1 nokta(.) içermelidir.";
            return false;
        }
        return true;
    }
    public bool IsValidLogin(string login, out string errorLogin, out bool register)
    {
        errorLogin = "";
        register = false;
        if (login.Length < 5 || login.Length > 20)
        {
            errorLogin = "Kullanıcı adı en az 5 en çok 20 karakter olmalıdır!";
            return false;
        }

        if (!_context.Users.Any(u => u.Username == login))
        {
            errorLogin = "Kullanıcı adı hatalı.";
            Helper.ShowErrorMsg(errorLogin);
            Thread.Sleep(1000);
            var registerOp = Helper.AskOption("Kayıt olmak ister misin?", ["Evet", "Hayır"]);
            switch (registerOp)
            {
                case 1:
                    errorLogin = "Kayıt sayfasına yönlendiriliyorsunuz.";
                    register = true;
                    break;
                case 2:
                    register = false;
                    break;
            }
            return false;
        }
        
        return true;
    }

    public bool IsValidLoginPassword(string username,string password, out string errorpass)
    {
        errorpass = "";
        if (!IsValidPassword(password,  out errorpass))
        {
            errorpass = errorpass;
            return false;
        }

        if (!_userRepository.GetUserPass(username, password, out errorpass))
            return false;
        return true;
    }
    
    public bool IsValidQuestion(string username,string question, out string errorQuestion)
    {
        errorQuestion = "";
        if (!_context.Users.Any(u => u.Username == username && u.SecurityQuestion == question))
        {
            errorQuestion = "Güvenlik sorusu yanlış!";
            return false;
        }
        Helper.ShowInfoMsgLine("Güvenlik sorusu doğru!");
        return true;
    }
    public bool IsValidAnswer(string username,string answer, out string erroranswer)
    {
        erroranswer = "";
        if (!_context.Users.Any(u => u.Username == username && u.SecurityAnswer == answer))
        {
            erroranswer = "Güvenlik sorusu cevabı yanlış!";
            return false;
        }
        Helper.ShowInfoMsgLine("Güvenlik sorusu cevabı doğru!");
        return true;
    }
}