using DiaryEditor.Helpers;
namespace DiaryEditor;

public class DiaryNavigationHelper
{
    public static string ListMenu()
    {
        string listMenu = "(F)Sonraki Kayıt | (D)üzenle | (S)il | (B)ul | (A)na Menü";
        return listMenu;
    }
    public static string[] MenuStrings()
    {
        string[] menuStrings = ["Yeni Kayıt Ekle", "Kayıtları Listele", "Çıkış Yap"];
        return menuStrings;
    }
    public static void CreateNewRecord(int userId, string textPassword)
    {
        Thread.Sleep(500);
        Console.Clear();
        var recordContent = Helper.Ask("Kayıt etmek istediğiniz notu yazınız");
        DiaryHelper.CreateRecord(userId, recordContent, textPassword);
        Helper.ShowSuccessMsg("Kayıt başarılı bir şekilde oluşturuldu.");
    }

    public static void ListRecords(int userId, string textPassword)
    {
        Thread.Sleep(500);
        Console.Clear();
        DiaryHelper.ListRecord(userId, textPassword);
    }

    public static void HandleExit(string username)
    {
        Console.Clear();
        Thread.Sleep(500);
        Helper.ShowSuccessMsg("Görüşmek üzere... Yarın yeniden bekliyorum...");
        Thread.Sleep(1000);
        UserHelper.Disconnected(username);
    }
}