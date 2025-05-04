using DiaryEditor.Classes;

namespace DiaryEditor.Helpers;

public static class UserHelper
{
    public static void LoadCsv()
    {
        var listUser = CsvHelper.CsvReadLoad("users.csv");
        foreach (var u in listUser)
        {
            UserInfo.AddUserForCsv(u);
            //Helper.ShowSuccessMsg($"{u.Id} {u.RegisterTime} {u.Username} {u.UserFirstName} {u.UserLastName} {u.UserPassword} {u.UserBirthDate} {u.UserGender} {u.UserEmail}");
        }
    }

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
            
            if (!IsValid.IsValidQuestion(username, questionString, out string error))
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
            
            if (!IsValid.IsValidAnswer(username, inputAnswer, out string error))
            {
                Helper.ShowErrorMsg(error);
                return false;
            }
            answerBool = true;
            
        } while (!answerBool);
        return answerBool;
    }
    public static void RegisterUser()
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
        UserInfo.AddUser(username, name, surname, userPass, birthDate, securityQuestion, securityAnswer ,gender, email);
        Helper.ShowSuccessMsg("Başarı ile kayıt oldunuz!");
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
            CsvHelper.CsvCreator("journal.csv");
            return true;
        }
        return false;
    }
}