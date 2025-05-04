using DiaryEditor.Classes;
using DiaryEditor.Helpers;

namespace DiaryEditor;

public class DiaryHelper
{
    public static void LoadCsvDaily()
    {
        var listUser = CsvHelper.CsvReadLoadJournal("journal.csv");
        foreach (var u in listUser)
        {
            DailyManager.AddFunctToCsv(u);
        }
    }

    public static string ListMenu()
    {
        string listMenu = "(F)Sonraki Kayıt | (D)üzenle | (S)il | (B)ul | (A)na Menü";
        return listMenu;
    }
    public static string[] MenuStrings()
    {
        string[] menuStrings = ["Yeni Kayıt Ekle", "Kayıtları Listele", "Ben Kimim?", "Çıkış Yap"];
        return menuStrings;
    }
    public static string MenuStrings(int index)
    {
        string[] menuStrings = ["Yeni Kayıt Ekle", "Kayıtları Listele", "Ben Kimim?", "Çıkış Yap"];
        return menuStrings[index];
    }
    
    public static void ListDiary(string username, string password)
    {
        var myList = DailyManager.GetUserListDaily(username, password);
        var menu = ListMenu();
        int i = 0;
        
        if (myList == null)
            return;
        while (myList.Count > 0)
        {
            Console.Clear();
            Helper.ShowInfoMsg($"{myList[i].RecordDate:yyyy-MM-dd HH:mm:ss} - ");
            Helper.ShowInfoMsgList($"{myList[i].Entry}\n");
            Helper.ShowInfoMsgLine(menu);
    
            var key = Console.ReadKey(true).Key;
    
            switch (key)
            {
                case ConsoleKey.F:
                    i = (i + 1) % myList.Count; // Liste sonuna geldiyse başa dön
                    break;
    
                case ConsoleKey.D:
                    Console.Clear();
                    var edit = DailyManager.EditNote(myList[i], username, password);
                    Helper.ShowInfoMsgLine($"Düzenlenme tamamlandı. Yeni içerik : {edit}");
                    Thread.Sleep(1000);
                    myList = DailyManager.GetUserListDaily(username, password); // Listeyi güncelle
                    if (myList == null)
                        return;
                    break;
    
                case ConsoleKey.S:
                    DailyManager.DeleteDiaryByJournal(myList[i]);
                    Thread.Sleep(1000);
                    myList = DailyManager.GetUserListDaily(username, password);
                    if (myList == null)
                        return;
                    i = 0;
                    break;
    
                case ConsoleKey.B:
                    Console.Clear();
                    var findword = Helper.Ask("Kayıtlarınız arasında filtreleyeceğiniz kelime giriniz", true);
                    var search = DailyManager.SearchTo(findword, username, password);
                    Helper.ShowInfoMsgLine($"Aradığın içerik : {search}");
                    Thread.Sleep(1000);
                    break;
    
                case ConsoleKey.A:
                    Console.Clear();
                    Helper.ShowSuccessMsg("Ana Menüye dönüyorsun...");
                    Thread.Sleep(1000);
                    return; 
            }
        }
    }

    public static bool SameNoteControl(DateTime date1, string username, string password, string note)
    {
        var info = DailyManager.GetAllDailyRecord();
        string infoNote;
        for (int i = 0; i < info.Count; i++)
        {
            if (info[i].RecordDate == date1 &&
                info[i].RecordUsername == username
                && info[i].RecordPassword == password && info[i].JournalEntry == note)
                return false;
        }
        return true;
    }
}