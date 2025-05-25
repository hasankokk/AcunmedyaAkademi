using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories;

namespace StudentManagementSystem.Helpers;

public class TeacherHelper
{
    private readonly TeacherRepository _teacherRepository;
    private readonly ClassroomRepository _classroomRepository;
    private readonly ClassroomHelper _classroomHelper;

    public TeacherHelper(AppDbContext context)
    { 
        _teacherRepository = new TeacherRepository(context);
        _classroomRepository = new ClassroomRepository(context);
        _classroomHelper = new ClassroomHelper(context);
    }

    public bool RegisterTeacher(string? inputName, string? inputSurname, long inputTckn, int? classroomId = null)
    {
        var teacher = new Teacher
        {
            Name = inputName,
            Surname = inputSurname,
            TeacherTckn = inputTckn
        };
        
        var successRegister = _teacherRepository.AddTeacher(teacher);
        if (!successRegister)
        {
            return false;
        }

        if (classroomId.HasValue)
        {
            var classroom = _classroomRepository.GetClassroomById(classroomId.Value);
            if (classroom != null)
            {
                if (!_teacherRepository.AssignTeacherToClassroom(teacher.TeacherTckn, classroomId.Value))
                {
                    ColoredHelper.ShowErrorMsg("İlişki kurulamadı!");
                    return false;
                }
            }
            else
            {
                ColoredHelper.ShowErrorMsg("Belirtilen sınıf bulunamadı!");
                ColoredHelper.ShowInfoMsg("Öğretmen eklerken yeni sınıf oluşturulabilir");
                var inputCreated = Helper.AskOption(["Evet", "Hayır"], "Oluşturulsun mu?");
                if (inputCreated == 1)
                {
                    var inputClassName = Helper.Ask("Eklenecek olan sınıf adını giriniz", true);
                    var classname = new Classroom
                    {
                        Name = inputClassName,
                    };
                    if (_classroomRepository.AddClassroom(classname))
                    {
                        ColoredHelper.ShowSuccessMsg("Sınıf başarıyla oluşturuldu. Öğretmen sınıf'a eklendi!");
                        _teacherRepository.AssignTeacherToClassroom(teacher.TeacherTckn, classroomId.Value);
                        return true;
                    }
                    ColoredHelper.ShowErrorMsg("Sınıf oluşturulamadı!");
                    return false;
                }
            }
        }
        return true;
    }
    public void RemoveTeacher()
    {
        var allTeachers = _teacherRepository.GetTeachers();
        if (!allTeachers.Any())
        {
            ColoredHelper.ShowErrorMsg("Listelenecek öğretmen bulunamadı!");
            return;
        }
        foreach (var teacher in allTeachers)
        {
            ColoredHelper.ShowListMsg($"{teacher.TeacherTckn} - {teacher.Name} {teacher.Surname}");
        }
        var removeTeacher = long.Parse(Helper.Ask("Silmek istediğiniz öğretmenin TCKN giriniz", true));
        var teacherToRemove = _teacherRepository.GetTeacher(removeTeacher);
        bool successRemove;
        if (teacherToRemove != null)
        {
            successRemove = _teacherRepository.DeleteTeacher(teacherToRemove);
            if  (successRemove)
            {
                ColoredHelper.ShowSuccessMsg("Öğretmen başarıyla silindi!");
                return;
            }
            ColoredHelper.ShowErrorMsg("Öğretmen silinemedi!");
        }
    }
    public void UpdateTeacher()
    {
        var allTeachers = _teacherRepository.GetTeachers();
        if (!allTeachers.Any())
        {
            ColoredHelper.ShowErrorMsg("Listelenecek öğretmen bulunamadı!");
            return;
        }
        foreach (var teacherList in allTeachers)
        {
            ColoredHelper.ShowListMsg($"{teacherList.TeacherTckn} - {teacherList.Name} {teacherList.Surname}");
        }
        var updateTeacher = long.Parse(Helper.Ask("Güncellemek istediğiniz öğretmenin TCKN giriniz", true));
        var teacher = _teacherRepository.GetTeacher(updateTeacher);
        if (teacher != null)
        {
            var inputName = Helper.Ask("Yeni bir isim giriniz");
            var newName = string.IsNullOrWhiteSpace(inputName) ? teacher.Name : inputName;

            var inputSurname = Helper.Ask("Yeni bir soyisim giriniz");
            var newSurname = string.IsNullOrWhiteSpace(inputSurname) ? teacher.Surname : inputSurname;
            var updateClassroom = Helper.AskOption(new[] { "Ekle", "Sil" }, "Sınıf güncellemesi yapmak istiyor musun", "Vazgeç");
            
            if (updateClassroom == 1) // Ekle
            {
                var existingClassroomIds = teacher.Classrooms?.Select(c => c.ClassroomId).ToList() ?? new List<int>();
                var selectUpdate = _classroomHelper.UpdateClassRoom("Ekle", existingClassroomIds);
                foreach (var classroom in selectUpdate)
                {
                    _teacherRepository.AssignTeacherToClassroom(teacher.TeacherTckn, classroom.ClassroomId);
                }
            }
            if (updateClassroom == 2) // Sil
            {
                var existingClassrooms = teacher.Classrooms?.ToList() ?? new List<Classroom>();
                var selectToRemove = _classroomHelper.UpdateClassRoomsForRemove(existingClassrooms);
                foreach (var classroom in selectToRemove)
                {
                    _teacherRepository.RemoveTeacherFromClassroom(teacher.TeacherTckn, classroom.ClassroomId);
                }
            }
            _teacherRepository.UpdateTeacher(teacher.TeacherTckn, newName, newSurname);
        }
    }
    public void ListTeachers()
    {
        var teachers = _teacherRepository.GetTeachersWithClassrooms();
        if (!teachers.Any())
        {
            ColoredHelper.ShowErrorMsg("Kayıtlı öğretmen bulunamadı!");
            return;
        }
        foreach (var teacher in teachers)
        {
            var classroomNames = teacher.Classrooms != null && teacher.Classrooms.Any()
                ? string.Join(", ", teacher.Classrooms.Select(c => c.Name))
                : "Sınıf yok";
            ColoredHelper.ShowListMsg($"{teacher.TeacherTckn} - {teacher.Name} {teacher.Surname} | Sınıflar: {classroomNames}");
        }
    }
}