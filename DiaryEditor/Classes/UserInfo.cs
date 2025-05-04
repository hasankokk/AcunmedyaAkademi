using DiaryEditor.Helpers;
namespace DiaryEditor.Classes;

public class UserInfo
{
    private static List<UserInfo> _userList = new List<UserInfo>();
    public DateTime RegisterTime { get; private set; }
    public string Id {get; private set;}
    public string Username {get; private set;}
    public string? UserFirstName { get; private set;}
    public string? UserLastName {get; private set;}
    public string? UserPassword {get; private set;}
    public DateOnly UserBirthDate {get; private set;}
    public string? UserGender {get; private set;}
    public string? SecurityQuestion {get; private set;}
    public string? SecurityAnswer {get; private set;}
    public bool UserLogin {get; private set;} = false;
    
    public string? UserEmail {get; private set;}

    public UserInfo(DateTime registertime ,string userId,string username ,string userFirstName, string userLastName, string userPassword, DateOnly birtdate, 
        string securityquestion, string securityanswer, string gender, string email)
    {
        RegisterTime = registertime;
        Id = userId;
        Username = username;
        UserFirstName = userFirstName;
        UserLastName = userLastName;
        UserPassword = userPassword;
        UserBirthDate = birtdate;
        SecurityQuestion = securityquestion;
        SecurityAnswer = securityanswer;
        UserGender = gender;
        UserEmail = email;
    }
    public UserInfo(string username,string userFirstName, string userLastName, string userPassword, DateOnly birtdate, 
        string securityquestion, string securityanswer, string gender, string email)
    {
        RegisterTime = DateTime.Now;
        Id = Guid.NewGuid().ToString();
        Username = username;
        UserFirstName = userFirstName;
        UserLastName = userLastName;
        UserPassword = userPassword;
        UserBirthDate = birtdate;
        SecurityQuestion = securityquestion;
        SecurityAnswer = securityanswer;
        UserGender = gender;
        UserEmail =  email;
    }

    public static List<UserInfo> GetAllUser()
    {
        return _userList;
    }
    public static void UserInformation(string username)
    {
        var u = _userList.FirstOrDefault(a => a.Username == username);
            Helper.ShowSuccessMsg($"{u.UserFirstName} {u.UserLastName}");
    }
    public static void AddUser(string username,string userFirstName, string userLastName, string userPassword, DateOnly birtdate, string securityquestion, string securityanswer, string gender, string email)
    {
        if (_userList.Any(u => u.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)))
        {
            Helper.ShowErrorMsg("Kullanıcı zaten mevcut!");
            return;
        }
        var user = new UserInfo(username, userFirstName, userLastName, userPassword, birtdate, securityquestion, securityanswer, gender, email);
        _userList.Add(user);
        CsvHelper.CsvCreator("users.csv");
    }

    public static void AddUserForCsv(UserInfo user)
    {
        _userList.Add(user);
    }
    public int GetAge()
    {
        return CalculateAge();
    }
    private int CalculateAge()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - UserBirthDate.Year;
        if (today < UserBirthDate.AddYears(age))
        {
            age--;
        }

        return age;
    }

    public static bool GetUserName(string user)
    {
        foreach (var u in _userList)
        {
            if (u.Username.Equals(user, StringComparison.InvariantCultureIgnoreCase))
                return true;
        }
        return false;
    }

    public static string GetUserAnswer(string user)
    {
        var findUser = _userList.FirstOrDefault(u => u.Username == user);
        if (findUser == null)
            return "Kullanıcı bulunamadı.";
        return findUser.SecurityAnswer;
    }
    public static string GetUserQuestion(string user)
    {
        var findUser = _userList.FirstOrDefault(u => u.Username == user);
        if (findUser == null)
            return "Kullanıcı bulunamadı.";
        return findUser.SecurityQuestion;
    }

    public static bool GetUserPass(string user, string password, out string errorpass)
    {
        errorpass = "";
        var findUser = _userList.FirstOrDefault(u => u.Username == user);
        try
        {
            bool result = HashHelper.VerifyPassword(password, findUser.UserPassword);
            if (!result)
                throw new Exception("Şifre hatalı!");
            findUser.UserLogin = true;
            return findUser.UserLogin;
        }
        catch (Exception e)
        {
            Helper.ShowErrorMsg(e.ToString());
            return findUser.UserLogin;
        }
    }

    public static bool ForgotPassword(string username)
    {
        try
        {
            if (!UserHelper.QuestionBool(username))
                throw new Exception("Güvenlik sorusu hatalı!");

            if (!UserHelper.AnswerBool(username))
                throw new Exception("Güvenlik sorusu cevabı hatalı!");
            var findUser = _userList.FirstOrDefault(u => u.Username == username);
            string newPassword = Helper.AskPassword("Yeni parola belirleyin");
            findUser.UserPassword = newPassword;
            CsvHelper.UploadToCsv(username,"users.csv", findUser.UserPassword);
            return true;
        }
        catch (Exception e)
        {
            Helper.ShowErrorMsg(e.ToString());
            throw;
        }
    }
}