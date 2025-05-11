using DiaryEditor.Data;
using DiaryEditor.Models;
using DiaryEditor.Helpers;
using Microsoft.EntityFrameworkCore;
namespace DiaryEditor.Repository;

public class DiaryRepository
{
    private static readonly UserRepository _userRepository = new UserRepository();
    private static readonly AppDbContext _context = new AppDbContext();
    
   public void AddRecord(Daily daily)
   {
       _context.Dailies.Add(daily);
       _context.SaveChanges();
   }

   public List<Daily> GetMyNotes(int userId)
   {
       return _context.Dailies
           .AsNoTracking()
           .Where(d => d.UserId == userId)
           .ToList();
   }

   public bool DeleteRecord(int userId, int textId)
   {
       if (_context.Dailies.Any(d => d.UserId == userId))
       {
           _context.Dailies.Remove(_context.Dailies.Where(d => d.Id == textId).FirstOrDefault());
           _context.SaveChanges();
           return true;
       }

       return false;
   }

   public void EditRecord(string newText, int textId)
   {
       var diary = _context.Dailies.FirstOrDefault(d => d.Id == textId);
       if (diary != null)
       {
           diary.JournalEntry = newText;
           diary.Updated = DateTime.Now;
           _context.SaveChanges();
       }
       else
       {
           Helper.ShowErrorMsg("Güncellenecek kayıt bulunamadı.");
       }
   }

   public string GetDailyPassword(int userId)
   {
       var findUser =  _context.Dailies.FirstOrDefault(d => d.UserId == userId);
       if (findUser != null)
       {
           return findUser.RecordPassword;
       }
       return null;
   }

   public void UpdateDailyPassword(int userId, string newPassword)
   {
       var userDailies = _context.Dailies.Where(d => d.UserId == userId).ToList();

       if (userDailies == null || userDailies.Count == 0)
       {
           Console.WriteLine("Kullanıcıya ait şifre güncellenecek kayıt bulunamadı.");
           return;
       }

       foreach (var daily in userDailies)
       {
           daily.RecordPassword = newPassword;
       }

       _context.SaveChanges();
       Console.WriteLine("Tüm şifreler başarıyla güncellendi.");
   }

   
   public void UpdateUserPasswordOnRecords(int userId, string oldPassword, string newPassword)
   {
       var notes = GetMyNotes(userId);
       if (notes == null || notes.Count == 0)
       {
           Helper.ShowErrorMsg("Kullanıcıya ait hiç kayıt bulunamadı.");
           return;
       }

       foreach (var note in notes)
       {
           try
           {
               string decrypted = CryptoHashHelper.CryptoHelper.Decrypt(note.JournalEntry, oldPassword);
               string reEncrypted = CryptoHashHelper.CryptoHelper.Encrypt(decrypted, newPassword);
               EditRecord(reEncrypted, note.Id);
           }
           catch (Exception ex)
           {
               Helper.ShowErrorMsg($"Kayıt ID {note.Id} dönüştürülürken hata oluştu: {ex.Message}");
           }
       }
       
       UpdateDailyPassword(userId, newPassword);

       Helper.ShowSuccessMsg("Tüm kayıtlar ve şifre başarıyla güncellendi.");
   }
}