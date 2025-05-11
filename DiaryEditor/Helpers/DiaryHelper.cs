using DiaryEditor.Models;
using DiaryEditor.Helpers;
using DiaryEditor.Repository;

namespace DiaryEditor;

public class DiaryHelper
{
    private static readonly DiaryRepository  _diaryRepository = new DiaryRepository();
    private static readonly UserRepository _userRepository = new UserRepository();
    
    public static bool PassMatch(string savePass, int userId)
    {
        if (!string.IsNullOrEmpty(savePass))
        {
            string inputPass;
            bool passwordVerified = false;

            do
            {
                inputPass = Helper.Ask("Kayıtlarınıza erişmek için şifrenizi giriniz", true);

                if (inputPass != savePass)
                {
                    Helper.ShowErrorMsg("Hatalı şifre. Lütfen tekrar deneyin.");
                    var forgotPass = Helper.AskOption("Kayıt Şifreni mi unuttun?", new[] { "Evet", "Hayır" });

                    if (forgotPass == 1)
                    {
                        Thread.Sleep(1000);
                        Console.Clear();

                        if (ResetDailyPassword(userId))
                        {
                            Helper.ShowSuccessMsg("Şifre yenileme işlemi başarılı, tekrar giriş yapınız.");
                            return false;
                        }
                        else
                        {
                            Helper.ShowErrorMsg("Şifre yenileme işlemi başarısız!");
                        }
                    }
                }
                else
                {
                    passwordVerified = true;
                }

            } while (!passwordVerified);
        }
        return true;
    }

    private static List<Daily> DecryptNotes(List<Daily> notes, string password)
    {
        if (notes == null || notes.Count == 0)
            return notes;
        foreach (var note in notes)
        {
            try
            {
                if (!CryptoHashHelper.CryptoHelper.IsBase64String(note.JournalEntry))
                    continue;
                note.JournalEntry = CryptoHashHelper.CryptoHelper.Decrypt(note.JournalEntry, password);
            }
            catch
            {
                Helper.ShowErrorMsg("Şifre çözme başarısız...");
            }
        }
        return notes;
    }

    public static bool ResetDailyPassword(int userId)
    {
        var user = _userRepository.GetUserName(userId);
        if (user == null)
            return false;

        if (UserHelper.QuestionBool(user) && UserHelper.AnswerBool(user))
        {
            string oldPassword = _diaryRepository.GetDailyPassword(userId);

            string newDailyPass = Helper.Ask("Yeni bir günlük parolası belirleyin:", true);

            _diaryRepository.UpdateUserPasswordOnRecords(userId, oldPassword, newDailyPass);

            Helper.ShowSuccessMsg("Şifre ve kayıtlar başarıyla güncellendi.");
            return true;
        }

        return false;
    }

    private static string EditNote(string note, string password)
    {
        Helper.ShowMsg($"Güncellemek istediğiniz not : {note}", ConsoleColor.DarkGreen);
        var newNote = Helper.Ask("\nLütfen yeni notunuzu giriniz");
        var hashNote = CryptoHashHelper.CryptoHelper.Encrypt(newNote, password);
        return hashNote;
    }
    public static void CreateRecord(int id, string text, string textPassword)
    {
        var passtext = CryptoHashHelper.CryptoHelper.Encrypt(text, textPassword);
        if (text == null)
            Helper.ShowErrorMsg("Girmek istediğiniz not boş!");
        var daily = new Daily
        {
            UserId = id, JournalEntry = passtext, RecordPassword = textPassword
        };
        _diaryRepository.AddRecord(daily);
    }
    
    private static void SearchTo(int userId, string keyword, string textPassword)
    {
        var myNotes = _diaryRepository.GetMyNotes(userId);

        if (myNotes == null || myNotes.Count == 0)
        {
            Helper.ShowErrorMsg("Hiç kayıt bulunamadı.");
            return;
        }

        myNotes = DecryptNotes(myNotes, textPassword); // Şifre çözümü

        var filteredNotes = myNotes
            .Where(n => n.JournalEntry != null && n.JournalEntry.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();

        if (filteredNotes.Count == 0)
        {
            Helper.ShowInfoMsgLine("Aradığınız kelimeye ait bir sonuç bulunamadı.");
            return;
        }

        Console.Clear();
        Helper.ShowSuccessMsg($"{filteredNotes.Count} sonuç bulundu:");
        foreach (var note in filteredNotes)
        {
            Helper.ShowMsg($"{note.Id} - ", ConsoleColor.Red);
            Helper.ShowMsg($"{note.Created} - ", ConsoleColor.DarkYellow);
            Helper.ShowMsg($"{note.JournalEntry}", ConsoleColor.Cyan);
            Console.WriteLine();
        }

        Console.WriteLine();
        Helper.ShowInfoMsgLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey(true);
    }

    public static void ListRecord(int id, string textPassword)
    {
        var myNotes = _diaryRepository.GetMyNotes(id);
        if (myNotes == null || myNotes.Count == 0)
        {
            Helper.ShowErrorMsg("Hiç kayıt bulunamadı.");
            return;
        }
        myNotes = DecryptNotes(myNotes, textPassword);
        int index = 0;
        string menu = DiaryNavigationHelper.ListMenu();
        while (true)
        {
            if (myNotes == null)
                return;
            Console.Clear();
            if (index < 0 || index >= myNotes.Count)
            {
                Helper.ShowErrorMsg("Geçerli bir kayıt yok.");
                Thread.Sleep(500);
                return; // Liste boş ya da index geçersizse çıkıyoruz
            }
            var currentNote = myNotes[index];
            Helper.ShowMsg($"{currentNote.Id} - ", ConsoleColor.Red);
            Helper.ShowMsg($"{currentNote.Created} - ", ConsoleColor.DarkYellow);
            Helper.ShowMsg($"{currentNote.JournalEntry}", ConsoleColor.Cyan);
            Console.WriteLine();
            Helper.ShowInfoMsgLine(menu);
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.F:
                    index = (index + 1) % myNotes.Count;
                    break;
                case ConsoleKey.D:
                    Console.Clear();
                    var edited = EditNote(myNotes[index].JournalEntry, textPassword);
                    _diaryRepository.EditRecord(edited, myNotes[index].Id);
                    Helper.ShowInfoMsgLine($"Düzenlenme tamamlandı.");
                    Thread.Sleep(1000);
                    myNotes = DecryptNotes(_diaryRepository.GetMyNotes(id), textPassword);
                    index = Math.Min(index, myNotes.Count - 1);
                    break;
                case ConsoleKey.S:
                    if (myNotes == null || myNotes.Count == 0)
                        return;
                    if(_diaryRepository.DeleteRecord(id, myNotes[index].Id))
                    {
                        Helper.ShowSuccessMsg($"Silme işlemi başarılı! Silinen not : {myNotes[index].JournalEntry}");
                        Thread.Sleep(1000);
                        myNotes = DecryptNotes(_diaryRepository.GetMyNotes(id), textPassword);
                    }
                    index = Math.Min(index, myNotes.Count - 1);
                    break;
                case ConsoleKey.B:
                    Console.Clear();
                    var keyword = Helper.Ask("Kayıtlarınız arasında filtreleyeceğiniz kelimeyi giriniz", true);
                    SearchTo(id, keyword, textPassword);
                    Thread.Sleep(1000);
                    break;
                case ConsoleKey.A:
                    Console.Clear();
                    Helper.ShowSuccessMsg("Ana menüye dönülüyor...");
                    Thread.Sleep(1000);
                    return;
            } 
        } 
    } 
}