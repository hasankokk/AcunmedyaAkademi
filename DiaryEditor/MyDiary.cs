using DiaryEditor.Classes;
using DiaryEditor.Helpers;

namespace DiaryEditor;

public class MyDiary
{
    public static bool StartApp(string username, bool userLogin)
    {
        do
        {
            Thread.Sleep(1000);
            Console.Clear();
            int selectIndex = Helper.AskOption("Yapmak istediğin işlem nedir?", DiaryHelper.MenuStrings());
            var select = DiaryHelper.MenuStrings(selectIndex - 1);
            Console.WriteLine(selectIndex);
            if (selectIndex == 1)
            {
                Thread.Sleep(500);
                Console.Clear();
                var textPassword = Helper.Ask("Kaydınız için bir şifre belirleyin", true);
                var recordContent = Helper.Ask("Kayıt etmek istediğiniz notu yazınız");
                DailyManager.NewRecordDaily(username, textPassword, recordContent);
                Helper.ShowSuccessMsg("Kayıt başarılı bir şekilde oluşturuldu.");
            }

            if (selectIndex == 2)
            {
                Thread.Sleep(500);
                Console.Clear();
                var pass = Helper.Ask("Kayıtlarınızı listelemek için lütfen şifrenizi giriniz");
                DiaryHelper.ListDiary(username,pass);
            }

            if (selectIndex == 3)
            {
                Thread.Sleep(500);
                Console.Clear();
                UserInfo.UserInformation(username);
                Helper.ShowErrorMsg("Devam etmek için herhangi bir tuşa başın!");
                Console.ReadKey(true);
            }

            if (selectIndex == 4)
            {
                Console.Clear();
                Thread.Sleep(500);
                Helper.ShowSuccessMsg("Görüşmek üzere... Yarın yeniden bekliyorum...");
                Thread.Sleep(1000);
                userLogin = false;
                return false;
            }
                
        } while (userLogin);

        return false;
    }
}