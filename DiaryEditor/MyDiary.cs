using DiaryEditor.Models;
using DiaryEditor.Helpers;
using DiaryEditor.Repository;

namespace DiaryEditor;

public class MyDiary
{
    private static readonly UserRepository _user = new UserRepository();
    private static readonly DiaryRepository _daily = new DiaryRepository();

     public static bool StartApp(string username, bool userLogin)
    {
        // Çıkış işlemi için CancelKeyPress handler'ı
        Console.CancelKeyPress += (sender, e) =>
        {
            UserRepository.CancelKeyPressHandler(username);
            e.Cancel = true;
            Console.Clear();
            Console.WriteLine();
            Helper.ShowSuccessMsg("Uygulamadan çıkış yapılıyor...");
            Thread.Sleep(1000);
            Environment.Exit(0);
        };

        if (_user.IsUserActiveOrInactive(username))
        {
            int userId = _user.GetUserById(username);
            string existingPass = _daily.GetDailyPassword(userId);
            string diaryPassword;

            if (!string.IsNullOrEmpty(existingPass))
            {
                bool isVerified = DiaryHelper.PassMatch(existingPass, userId);

                if (!isVerified)
                {
                    // yeni şifre veritabanında zaten güncellendi, yeniden alma
                    diaryPassword = _daily.GetDailyPassword(userId);
                }
                else
                {
                    diaryPassword = existingPass;
                }
            }
            else
            {
                diaryPassword = Helper.Ask("Kayıtlarınız için bir şifre belirleyiniz", true);
                _daily.UpdateDailyPassword(userId, diaryPassword);
                Helper.ShowSuccessMsg("Şifreniz başarıyla kaydedildi.");
            }


            while (userLogin)
            {
                int selectIndex = Helper.AskOption("Yapmak istediğin işlem nedir?", DiaryNavigationHelper.MenuStrings());

                switch (selectIndex)
                {
                    case 1:
                        DiaryNavigationHelper.CreateNewRecord(userId, diaryPassword);
                        Thread.Sleep(500);
                        Console.Clear();
                        break;

                    case 2:
                        DiaryNavigationHelper.ListRecords(userId, diaryPassword);
                        Thread.Sleep(500);
                        Console.Clear();
                        break;

                    case 3:
                        DiaryNavigationHelper.HandleExit(username);
                        Thread.Sleep(500);
                        Console.Clear();
                        return false;

                    default:
                        Helper.ShowErrorMsg("Geçersiz işlem, lütfen tekrar deneyin.");
                        break;
                }
            }
        }
        return false;
    }
}
