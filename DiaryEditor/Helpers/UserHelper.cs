using DiaryEditor.Models;
using DiaryEditor.Repository;
using DiaryEditor.Validation;

namespace DiaryEditor.Helpers;

public static class UserHelper
{
    private readonly static UserRepository _userRepository = new UserRepository();
    private static readonly IsValid _isValid = new IsValid(); 
    public static string[] QuestionString()
    {
        string[] questionStrings = [
            "En sevdiğiniz okul öğretmeninin adı nedir?",
            "Çocukken sahip olduğunuz ilk evcil hayvanın adı nedir?",
            "İlk arabanızın markası nedir?",
            "En sevdiğiniz tatil yeri neresi?",
            "Doğum günü tarihinizi unuttuğunuzda hangi tarihi hatırlıyorsunuz?",
            "En sevdiğiniz müzik grubunun adı nedir?",
            "İlk okuduğunuz kitap neydi?",
            "En yakın arkadaşınızın doğum tarihi nedir?",
            "Büyüdüğünüz mahalledeki sokak adı nedir?",
            "İlk telefonunuzun markası nedir?"
            ];
        return questionStrings;
    }
    public static string QuestionString(int questionIndex)
    {
        string[] questionStrings = [
            "En sevdiğiniz okul öğretmeninin adı nedir?",
            "Çocukken sahip olduğunuz ilk evcil hayvanın adı nedir?",
            "İlk arabanızın markası nedir?",
            "En sevdiğiniz tatil yeri neresi?",
            "Doğum günü tarihinizi unuttuğunuzda hangi tarihi hatırlıyorsunuz?",
            "En sevdiğiniz müzik grubunun adı nedir?",
            "İlk okuduğunuz kitap neydi?",
            "En yakın arkadaşınızın doğum tarihi nedir?",
            "Büyüdüğünüz mahalledeki sokak adı nedir?",
            "İlk telefonunuzun markası nedir?"
        ];
        return questionStrings[questionIndex];
    }

    public static bool QuestionBool(string username)
    {
        bool questionBool;
        do
        {
            var question = Helper.AskOption("Güvenlik sorunuzu seçiniz", QuestionString());
            var questionString = QuestionString(question - 1);
            
            if (!_isValid.IsValidQuestion(username, questionString, out string error))
            {
                Helper.ShowErrorMsg(error);
                return false;
            }
            questionBool = true;
            
        } while (!questionBool);
        return questionBool;
    }
    public static bool AnswerBool(string username)
    {
        bool answerBool;
        do
        {
            var inputAnswer = Helper.Ask("Güvenlik sorusu cevabı nedir?", true);
            
            if (!_isValid.IsValidAnswer(username, inputAnswer, out string error))
            {
                Helper.ShowErrorMsg(error);
                return false;
            }
            answerBool = true;
            
        } while (!answerBool);
        return answerBool;
    }

    public static void CreateUser(string username, string name, string surname, string password, DateOnly birthdate,
        string securityquestion, string securityanswer, string gender, string email)
    {
        var user = new User
        {
            Username = username,
            FirstName = name,
            LastName = surname,
            Password = password,
            BirthDate = birthdate,
            SecurityQuestion = securityquestion,
            SecurityAnswer = securityanswer,
            Gender = gender,
            Email = email
        };
        Helper.ShowSuccessMsg("Başarı ile kayıt oldunuz!");
        _userRepository.AddUser(user);
    }

    public static bool ForgotPassword(string username)
    {
        if (QuestionBool(username))
        {
            if (AnswerBool(username))
            {
                var newPass = Helper.AskPassword("Yeni parola belirleyin.");
                if (_userRepository.UpdatePassword(username, newPass))
                    return true;
            }
        }
        return false;
    }
    public static void RegisterForm()
    {
        Thread.Sleep(800);
        Console.Clear();
        var username = Helper.Ask("Kullanıcı Adı Giriniz", true);
        var name =  Helper.Ask("Adınızı Giriniz", true);
        var surname = Helper.Ask("Soyadınızı Giriniz", true);
        var userPass = Helper.AskPassword("Parola Belirleyin");
        string? birthDateInput = Helper.Ask("Doğum Tarihi Giriniz (yyyy.mm.dd)", true);
        DateOnly birthDate = DateOnly.Parse(birthDateInput);
        var inputQuestion = Helper.AskOption("Güvenlik Sorusu?", QuestionString());
        var securityQuestion = QuestionString(inputQuestion - 1);
        var securityAnswer = Helper.Ask(securityQuestion, true);
        var genderInt = Helper.AskOption("Cinsiyetiniz Nedir", ["Erkek", "Kadın", "Diğer"]);
        var gender = "";
        switch (genderInt)
        {
            case 1:
                gender = "Erkek";
                break;
            case 2:
                gender = "Kadın";
                break;
            case 3:
                gender = "Diğer";
                break;
        }
        var email = Helper.Ask("E-Mail Adresinizi Giriniz", true);
        CreateUser(username, name, surname, userPass, birthDate, securityQuestion, securityAnswer, gender, email);
        Thread.Sleep(1000);
        Console.Clear();
    }
    public static string SelectLogin()
    {
        var login = Helper.Ask("Kullanıcı Adı", true);
        return login;
    }

    public static bool LoginSuccesPass(string login)
    {
        var loginSucces = Helper.AskLoginPassword(login,"Parola");
        Console.Clear();
        if (loginSucces)
        {
            _userRepository.IsUserActive(login);
            return true;
        }

        return false;
    }

    public static void Disconnected(string username)
    {
        _userRepository.IsUserInactive(username);
    }
}