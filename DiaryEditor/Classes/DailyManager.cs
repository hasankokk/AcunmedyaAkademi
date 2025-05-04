using System.Runtime.InteropServices.JavaScript;
using DiaryEditor.Helpers;

namespace DiaryEditor.Classes;

public class DailyManager
{
    private static List<DailyManager> _journaling = new List<DailyManager>();
    public Guid Id { get; set; }
    public DateTime RecordDate { get; private set; }

    public string? RecordUsername { get; private set; }
    public string? RecordPassword { get; private set; }
    public string? JournalEntry { get; private set; }

    public class JournalList
    {
        public Guid Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string Entry { get; set; }
    }
    public DailyManager(DateTime recordDate, string? recordUsername, string recordPassword,string? journalEntry)
    {
        RecordDate = recordDate;
        RecordUsername = recordUsername;
        RecordPassword = recordPassword;
        JournalEntry = journalEntry;
    }
    public DailyManager(Guid id,DateTime recordDate, string? recordUsername, string recordPassword,string? journalEntry)
    {
        Id = id;
        RecordDate = recordDate;
        RecordUsername = recordUsername;
        RecordPassword = recordPassword;
        JournalEntry = journalEntry;
    }
    
    private static void AddDailyRecord(string username, string recordPassword, string journalentry)
    {
        var filename = "journal.csv";
        var record = new DailyManager(Guid.NewGuid() , DateTime.Now, username, recordPassword, journalentry);
        _journaling.Add(record);
        CsvHelper.AppendSingleJournal(filename, record);
        
    }

    public static List<DailyManager> GetAllDailyRecord()
    {
        return _journaling;
    }
    
    public static void NewRecordDaily(string username, string userPassword ,string? recordContent)
    {
        var encryptContent = CryptoHashHelper.CryptoHelper.Encrypt(recordContent, userPassword);
        AddDailyRecord(username, userPassword, encryptContent);
    }

    private static List<JournalList> ListRecordDaily(string username, string password)
    {
        var matchedEntries = _journaling
            .Where(a => a.RecordUsername == username && a.RecordPassword == password)
            .OrderByDescending(a => a.RecordDate)
            .Select((a) => new JournalList
            {
                Id = a.Id,
                RecordDate = a.RecordDate,
                Entry = a.JournalEntry
            })
            .ToList();
        if (matchedEntries.Count == 0)
            return null;
        
        return matchedEntries;
    }

    public static List<JournalList> GetUserListDaily(string username, string password)
    {
        try
        {
            var list = ListRecordDaily(username, password);
            var decryptedList = new List<JournalList>();
            if (list == null)
                throw new Exception("Liste boş.");
            foreach (var item in list)
            {
                decryptedList.Add(new JournalList
                {
                    Id = item.Id,
                    RecordDate = item.RecordDate,
                    Entry = CryptoHashHelper.CryptoHelper.Decrypt(item.Entry, password)
                });
            }
            return decryptedList;
        }
        catch (Exception e)
        {
            Helper.ShowErrorMsg(e.ToString());
            return null;
        }
    }

    public static void AddFunctToCsv(DailyManager dailyManager)
    {
        _journaling.Add(dailyManager);
    }
    
    public static bool DeleteDiaryByJournal(JournalList journal)
    {
        try
        {
            var whoIs = _journaling.FirstOrDefault(u => u.Id == journal.Id);
            if (whoIs != null)
            {
                int select = Helper.AskOption("Kaydı gerçekten silmek istediğine emin misin?", ["Evet", "Hayır"]);
                if (select == 1)
                {
                    Console.Clear();
                    _journaling.Remove(whoIs);
                    CsvHelper.DeleteData(whoIs.Id);
                    Helper.ShowSuccessMsg("Silme işlemi başarılı");
                    return true;
                }
            }

            if (whoIs == null)
            {
                throw new Exception($"{whoIs.Id} {journal.Id}");
            }
        }
        catch (Exception e)
        {
            Helper.ShowErrorMsg(e.ToString());
            throw ;
        }
        return false;
    }

    public static string SearchTo(string content, string username, string? password)
    {
        var list = GetUserListDaily(username, password);
        string[] findcontent = new string[list.Count];
        int answer;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Entry.Contains(content))
            {
                Helper.ShowInfoMsgLine(list[i].Entry);
                answer = Helper.AskOption("Aradığınız içerik aşağıdaki mi?", ["Evet", "Hayır"]);
                if (answer == 1)
                {
                    findcontent[i] = list[i].Entry;
                    return findcontent[i];
                }
                else
                    continue;
            }
        }
        return null;
    }

    public static string EditNote(JournalList list, string username, string userPassword)
    {
        try
        {

            var findUser = _journaling.FirstOrDefault(u => u.Id == list.Id);
            var content = CryptoHashHelper.CryptoHelper.Decrypt(findUser.JournalEntry, userPassword);
            if (findUser != null)
            {
                Helper.ShowSuccessMsg(content);
                var ask = Helper.AskOption("İçeriği değiştirmek istiyor musun?", ["Evet", "Hayır"]);
                if (ask == 1)
                {
                    var newContent = Helper.Ask("Girmek istediğin içeriği yazar mısın", true);
                    var newCont = CryptoHashHelper.CryptoHelper.Encrypt(newContent, userPassword);
                    findUser.JournalEntry = newCont;
                    CsvHelper.UploadToRecord(list.Id, "journal.csv", newCont);
                    return newContent;
                }
                if (ask == 2)
                {
                    Helper.ShowErrorMsg($"İçerik değiştirilmedi.");
                    return string.Empty;
                }
            }
        }
        catch (Exception e)
        {
            Helper.ShowErrorMsg(e.ToString());
            throw ;
        }
        return null;
    }
}