using System.Globalization;
using System.Text;
using DiaryEditor.Classes;

namespace DiaryEditor.Helpers;

public class CsvHelper
{
    /// <summary>
    /// Bu metod, eğer yoksa istenilen parametrelere ve headere uygun bir csv dosyası oluşturur.
    /// </summary>
    /// <param name="fileName">-> oluşturacak ya da aranacak dosyanın adı></param>
    public static async Task CsvCreator(string fileName)
    {
        if (fileName == "users.csv")
        {
            if (File.Exists(fileName))
            {
                await CsvAppend(fileName);
                return;
            }
            string seperator = "|";
            string csvFileName = $"{fileName}";
            string[] csvHeaders =
            {
                "REGISTER-DATE", "ID", "USERNAME", "NAME", "SURNAME", "PASSWORD", "AGE", "SECURITY_QUESTION",
                "SECURITY_ANSWER", "GENDER", "E-MAIL"
            };
            string
                csvHeaderText =
                    string.Join(seperator, csvHeaders); //Verdiğim string[] seperatore göre ayırıp tek bir dizi döner.
            try
            {
                using (StreamWriter csvWriter = new StreamWriter(csvFileName, true))
                    csvWriter.WriteLine(csvHeaderText);
                await CsvAppend(fileName);
            }
            catch (Exception e)
            {
                Helper.ShowErrorMsg(e.ToString());
                throw;
            }
        }
        if (fileName == "journal.csv")
        {
            if (File.Exists(fileName))
            {
                //await CsvAppend(fileName);
                return;
            }
            string seperator = "|";
            string csvFileName = $"{fileName}";
            string[] csvHeaders =
            {
                "ID", "JOURNAL-DATE", "USERNAME", "DAILY_PASSWORD","DAILY_RECORD"
            };
            string
                csvHeaderText =
                    string.Join(seperator, csvHeaders); //Verdiğim string[] seperatore göre ayırıp tek bir dizi döner.
            try
            {
                using (StreamWriter csvWriter = new StreamWriter(csvFileName, true))
                    csvWriter.WriteLine(csvHeaderText);
            }
            catch (Exception e)
            {
                Helper.ShowErrorMsg(e.ToString());
                throw;
            }
        }
    }

    public static string CsvHeader()
    {
        string seperator = "|";
        string[] csvHeaders =
        {
            "ID", "JOURNAL-DATE", "USERNAME", "DAILY_PASSWORD","DAILY_RECORD"
        };
        string csvHeaderText = string.Join(seperator, csvHeaders);
        return csvHeaderText;
    }
    public static async Task CsvAppend(string fileName)
    {
        if (fileName == "users.csv")
        {
            var csvBuilder = new StringBuilder();
            var users = UserInfo.GetAllUser();
            string seperator = "|";
            try
            {
                foreach (var user in users)
                {
                    if (IsUserAvailable(fileName, user.Username))
                        continue;
                    csvBuilder.AppendLine(
                        $"{user.RegisterTime}{seperator}{user.Id}{seperator}{user.Username}{seperator}{user.UserFirstName}{seperator}{user.UserLastName}{seperator}{user.UserPassword}{seperator}{user.UserBirthDate}{seperator}" +
                        $"{user.SecurityQuestion}{seperator}{user.SecurityAnswer}{seperator}{user.UserGender}{seperator}{user.UserEmail}");
                }
                if (csvBuilder.Length > 0)
                    await File.AppendAllTextAsync(fileName, csvBuilder.ToString()); //Dosyasının sonuna ekleme yapabilmek için.
                    
            }
            catch (Exception e)
            {
                Helper.ShowErrorMsg(e.ToString());
                throw;
            }
        }
        if (fileName == "journal.csv")
        {
            var dailyList = DailyManager.GetAllDailyRecord(); // Yeni günlük verilerini alıyoruz
            string separator = "|";
            var csvBuilder = new StringBuilder();
            var csvheader = CsvHeader();
        
            try
            {
                var existingJournals = CsvReadLoadJournal(fileName);
        
                bool fileExists = File.Exists(fileName) && new FileInfo(fileName).Length > 0;
                
                if (!fileExists)
                {
                    csvBuilder.AppendLine(csvheader);
                }
                
                foreach (var journal in dailyList)
                {
                    bool alreadyExists = existingJournals.Any(existingJournal =>
                        existingJournal.RecordDate == journal.RecordDate &&
                        existingJournal.RecordUsername == journal.RecordUsername &&
                        existingJournal.RecordPassword == journal.RecordPassword &&
                        existingJournal.JournalEntry == journal.JournalEntry);
        
                    if (!alreadyExists)
                    {
                        csvBuilder.AppendLine($"{journal.RecordDate}{separator}{journal.RecordUsername}{separator}{journal.RecordPassword}{separator}{journal.JournalEntry}");
                    }
                }
        
                if (fileExists)
                {
                    // Dosya varsa, veriyi üzerine ekliyoruz
                    await File.AppendAllTextAsync(fileName, csvBuilder.ToString());
                }
                else
                {
                    await File.WriteAllTextAsync(fileName, csvBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }
    }
    public static List<UserInfo> CsvReadLoad(string fileName)
    {
        var users = new List<UserInfo>();
        try
        {
            if (!File.Exists(fileName))
            {
                //Helper.ShowErrorMsg("Dosya bulunamadı.");
                return users;
            }
            using var streamReader = new StreamReader(fileName);
            string? line;
            streamReader.ReadLine();//CSV Dosyası içerisindeki ilk satırı yani Header kısmını geçmek için kullanılıyor.
            while ((line = streamReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    throw new Exception("Geçerli bir veri yok.");
                var seperatorPart = line.Split("|");
                if (seperatorPart.Length != 11)
                    throw new Exception("Yeterli satır bulunamadı.");
                try
                {
                    var inputDateTime = seperatorPart[0];
                    if (DateTime.TryParseExact(inputDateTime, "d.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime userRegisterTimeParse)) { }
                    else
                        Helper.ShowErrorMsg($"Kayıt tarihi alınamadı.");
                    
                    var userRegisterTime = userRegisterTimeParse;
                    var userId = seperatorPart[1];
                    var username = seperatorPart[2];
                    var name = seperatorPart[3];
                    var surname = seperatorPart[4];
                    var userPassword = seperatorPart[5];
                    var birthDate = DateOnly.Parse(seperatorPart[6]);
                    var securityquestion = seperatorPart[7];
                    var securityanswer = seperatorPart[8];
                    var gender = seperatorPart[9];
                    var email = seperatorPart[10];
                    
                    var user = new UserInfo(userRegisterTime,userId, username, name, surname, userPassword, birthDate, securityquestion, securityanswer, gender, email);
                    users.Add(user);
                }
                catch (Exception e)
                {
                    Helper.ShowErrorMsg(e.ToString());
                    throw;
                }
            }
            return users;
        }
        catch (Exception e)
        {
            Helper.ShowErrorMsg(e.ToString());
            throw;
        }
    }

    public static List<DailyManager> CsvReadLoadJournal(string fileName)
    {
        var daily = new List<DailyManager>();
        try
        {
            if (!File.Exists(fileName))
            {
                //Helper.ShowErrorMsg("Dosya bulunamadı.");
                return daily;
            }
            using var streamReader = new StreamReader(fileName);
            string? line;
            streamReader.ReadLine();//CSV Dosyası içerisindeki ilk satırı yani Header kısmını geçmek için kullanılıyor.
            while ((line = streamReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    throw new Exception("Geçerli bir veri yok.");
                var seperatorPart = line.Split("|");
                if (seperatorPart.Length != 5)
                    throw new Exception("Yeterli satır bulunamadı.");
                try
                {
                    var inputDateTime = seperatorPart[1];
                    if (DateTime.TryParseExact(inputDateTime, "d.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime userRegisterTimeParse)) { }
                    else
                        Helper.ShowErrorMsg($"Kayıt tarihi alınamadı.");
                    
                    var userRegisterTime = userRegisterTimeParse;
                    var id = Guid.Parse(seperatorPart[0]);
                    var username = seperatorPart[2];
                    var userpassword = seperatorPart[3];
                    var journalentry = seperatorPart[4];
                    if (DiaryHelper.SameNoteControl(userRegisterTimeParse, seperatorPart[1], seperatorPart[2],
                        seperatorPart[3]))
                    {
                        var journalInput = new DailyManager( id ,userRegisterTime, username, userpassword, journalentry);
                        daily.Add(journalInput);
                    }
                }
                catch (Exception e)
                {
                    Helper.ShowErrorMsg(e.ToString());
                    throw;
                }
            }
            return daily;
        }
        catch (Exception e)
        {
            Helper.ShowErrorMsg(e.ToString());
            throw;
        }
    }
    public static bool IsUserAvailable(string filename, string username)
    {
        if (!File.Exists(filename))
        {
            Helper.ShowErrorMsg("Dosya yok.");
            return false;
        }
        var lines = File.ReadLines(filename).Skip(1);
        foreach (var line in lines)
        {
            var seperatorPart = line.Split("|");
            if  (seperatorPart.Length != 11)
                Helper.ShowErrorMsg("Yeterli bir veri yok.");
            string availableUser = seperatorPart[2];
            if (availableUser == username)
            {
                //Helper.ShowErrorMsg("");
                return true;
            }
        }
        return false;
    }

    public static void UploadToCsv(string username, string fileName, string content)
    {
        if (!File.Exists(fileName))
        {
            Helper.ShowErrorMsg("Dosya yok.");
        }
        var lines = File.ReadLines(fileName).ToList();
        var header = lines[0];
        var line =lines.Skip(1).ToList();
        bool updateCsvFile = false;
        for(int i = 0; i < line.Count; i++)
        {
            var seperatorPart = line[i].Split("|");
            if  (seperatorPart.Length != 11)
            {
                Helper.ShowErrorMsg("Yeterli bir veri yok.");
                return;
            }

            if (seperatorPart[2] == username)
            {
                seperatorPart[5] = content;
                line[i] = string.Join("|", seperatorPart); //değiştirilen verinin tekrardan string olması saglanıyor.
                updateCsvFile = true;
                break;
            }
        }
        if (!updateCsvFile)
        {
            Helper.ShowErrorMsg("Kullanıcı bulunamadı.");
            return;
        }
        lines = new List<string> {header};
        lines.AddRange(line);
        File.WriteAllLines(fileName, lines);
    }
    public static void UploadToRecord(Guid id , string fileName, string content)
    {
        if (!File.Exists(fileName))
        {
            Helper.ShowErrorMsg("Dosya yok.");
        }
        var lines = File.ReadLines(fileName).ToList();
        var header = lines[0];
        var line =lines.Skip(1).ToList();
        bool updateCsvFile = false;
        for(int i = 0; i < line.Count; i++)
        {
            var seperatorPart = line[i].Split("|");
            if  (seperatorPart.Length != 5)
            {
                Helper.ShowErrorMsg("Yeterli bir veri yok.");
                return;
            }

            if (Guid.Parse(seperatorPart[0]) == id)
            {
                seperatorPart[4] = content;
                line[i] = string.Join("|", seperatorPart); //değiştirilen verinin tekrardan string olması saglanıyor.
                updateCsvFile = true;
                break;
            }
        }
        if (!updateCsvFile)
        {
            Helper.ShowErrorMsg("Kullanıcı bulunamadı.");
            return;
        }
        lines = new List<string> {header};
        lines.AddRange(line);
        File.WriteAllLines(fileName, lines);
    }
    public static void AppendSingleJournal(string fileName, DailyManager record)
    {
        string separator = "|";
        var sb = new StringBuilder();

        if (!File.Exists(fileName) || new FileInfo(fileName).Length == 0)
        {
            sb.AppendLine("ID|JOURNAL-DATE|USERNAME|DAILY_PASSWORD|DAILY_RECORD");
        }

        sb.AppendLine($"{record.Id}{separator}{record.RecordDate}{separator}{record.RecordUsername}{separator}{record.RecordPassword}{separator}{record.JournalEntry}");
        File.AppendAllText(fileName, sb.ToString()); 
    }

    public static void DeleteData(Guid idToDelete)
    {
        string fileName = "journal.csv";
        if (!File.Exists(fileName))
        {
            Helper.ShowErrorMsg("Dosya bulunamadı.");
            return;
        }

        var allLines = File.ReadAllLines(fileName).ToList();

        if (allLines.Count <= 1)
        {
            Helper.ShowErrorMsg("Dosyada silinecek veri bulunamadı.");
            return;
        }

        string header = allLines[0];
        var newLines = new List<string> { header };

        foreach (var line in allLines.Skip(1))
        {
            var parts = line.Split('|');
            if (parts.Length < 1)
                continue;
            // ID'yi Guid olarak parse et
            if (!Guid.TryParse(parts[0], out Guid currentId))
                continue;
            // Eşleşmeyenleri ekle
            if (currentId != idToDelete)
            {
                newLines.Add(line);
            }
        }
        File.WriteAllLines(fileName, newLines);
        Helper.ShowSuccessMsg("Kayıt dosyadan silindi.");
    }

}