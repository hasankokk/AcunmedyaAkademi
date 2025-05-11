using DiaryEditor;
using DiaryEditor.Data;
using DiaryEditor.Models;
using DiaryEditor.Helpers;
using DiaryEditor.Repository;

while(true)
{
    var firstQuestion = Helper.AskOption("Yapmak istediğiniz işlem?", ["Giriş Yap", "Kayıt Ol", "Çıkış Yap"]);
    Thread.Sleep(1000);
    Console.Clear();
    switch (firstQuestion)
    {
        case 1:
            var userLogUserName = UserHelper.SelectLogin();
            var userLogin = UserHelper.LoginSuccesPass(userLogUserName);
            if (userLogin)
            {
                var active = MyDiary.StartApp(userLogUserName, userLogin);
                if (!active)
                    continue;
                break;
            }
            break;
        
        case 2:
            UserHelper.RegisterForm();
            break;
        case 3:
            Helper.ShowSuccessMsg("Uygulamadan çıkış yapılıyor...");
            return;
        default:
            Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
            break;
    }
}