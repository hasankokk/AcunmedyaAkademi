using DiaryEditor.Data;
using DiaryEditor.Helpers;
using DiaryEditor.Models;
using Microsoft.EntityFrameworkCore;

namespace DiaryEditor.Repository;

public class UserRepository
{
    private readonly AppDbContext _context = new  AppDbContext();

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public bool UpdatePassword(string username, string newPass)
    {
        var findUser = _context.Users.FirstOrDefault(u => u.Username == username);
        if (findUser != null)
        {
            findUser.Password = newPass;
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool GetUser(string username)
    {
        if (_context.Users.AsNoTracking().Any(x => x.Username == username))
        {
            return true;
        }
        return false;
    }

    public bool GetUserPass(string username, string password, out string errorpass)
    {
        errorpass = null;
        var findUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.Username == username);
        try
        {
            bool result = HashHelper.VerifyPassword(password, findUser.Password);
            if (!result)
            {
                Helper.ShowErrorMsg("Hatalı şifre!");
                return false;
            }
            findUser.Password = HashHelper.HashPassword(password);
            return true;
        }
        catch (Exception e)
        {
            Helper.ShowErrorMsg(e.ToString());
            throw;
        }
    }

    public void IsUserActive(string username)
    {
        _context.Users.FirstOrDefault(x => x.Username == username).IsActive = true;
        _context.SaveChanges();
    }
    public void IsUserInactive(string username)
    {
        _context.Users.FirstOrDefault(x => x.Username == username).IsActive = false;
        _context.SaveChanges();
    }

    public int GetUserById(string username)
    {
        return _context.Users.FirstOrDefault(x => x.Username == username).Id;
    }

    public string GetUserName(int id)
    {
        return  _context.Users.FirstOrDefault(x => x.Id == id).Username;
    }
    public bool IsUserActiveOrInactive(string username)
    {
        int id = GetUserById(username);
        return _context.Users.FirstOrDefault(x => x.Id == id).IsActive ? true : false;
    }
    
    public static void CancelKeyPressHandler(string username)
    {
        Console.WriteLine("Çıkış yapılıyor...");
        UserHelper.Disconnected(username);
    }
}