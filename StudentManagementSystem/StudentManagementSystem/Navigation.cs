using StudentManagementSystem.Data;
using StudentManagementSystem.Helpers;

namespace StudentManagementSystem;

public class Navigation
{
    private readonly AppDbContext _context;
    private readonly Auth _auth;
    private readonly ClassroomHelper _classroomHelper;
    private readonly StudentHelper _studentHelper;
    private readonly TeacherHelper _teacherHelper;

    public Navigation(AppDbContext context)
    {
        _context = context;
        _auth = new Auth(_context);
        _classroomHelper = new ClassroomHelper(_context);
        _studentHelper = new StudentHelper(_context);
        _teacherHelper = new TeacherHelper(_context);
    }

    public void StartApp()
    {
        var mainMenu = new ConsoleMenu("Öğrenci Yönetim Sistemi");
        mainMenu
            .AddOption("Öğrenci Yönetimi", StudentManagement)
            .AddOption("Öğretmen Yönetimi", TeacherManagement)
            .AddOption("Sınıf Yönetimi", ClassroomManagement);

        mainMenu.Show(isRoot: true);
    }

    public void StudentManagement()
    {
        var studentMenu = new ConsoleMenu("Öğrenci Yönetimi");
        studentMenu
            .AddOption("Öğrenci Ekle", () => _auth.RegisterUser("Student"))
            .AddOption("Öğrenci Düzenle", () => _studentHelper.UpdateStudent())
            .AddOption("Öğrenci Sil", () => _studentHelper.RemoveStudent())
            .AddOption("Öğrencileri Listele", () => _studentHelper.ListStudent());
        studentMenu.Show();
    }
    
    public void TeacherManagement()
    {
        var teacherMenu = new ConsoleMenu("Öğretmen Yönetimi");
        teacherMenu
            .AddOption("Öğretmen Ekle", () => _auth.RegisterUser("Teacher"))
            .AddOption("Öğretmen Düzenle", () => _teacherHelper.UpdateTeacher())
            .AddOption("Öğretmen Sil", () => _teacherHelper.RemoveTeacher())
            .AddOption("Öğretmenleri Listele", () => _teacherHelper.ListTeachers());
        teacherMenu.Show();
    }

    public void ClassroomManagement()
    {
        var classroomMenu = new ConsoleMenu("Sınıf Yönetimi");
        classroomMenu
            .AddOption("Sınıf Ekle", () => _classroomHelper.AddClassroom())
            .AddOption("Sınıf Düzenle", () => _classroomHelper.UpdateClassroomName())
            .AddOption("Sınıf Sil", () => _classroomHelper.DeleteClassroom())
            .AddOption("Sınıfları Listele", () => _classroomHelper.ListClassrooms());
        classroomMenu.Show();
    }
}