using ClassroomManagement.Helpers;
using ClassroomManagement.Models;

namespace ClassroomManagement.Repository;

public class ClassroomRepository
{
    private List<Classrooms> _classrooms = new List<Classrooms>();
    

    public List<Classrooms> GetClassroomsList()
    {
        return _classrooms;
    }

    public Classrooms? CreateClassrooms(string name)
    {
        var classroom = _classrooms.FirstOrDefault(c => c.Name == name);
        if (classroom == null)
        {
            classroom = new Classrooms { Name = name };
            _classrooms.Add(classroom);
            Helper.ShowSuccessMsg($"Sınıf oluşturuldu. {classroom.Name}");
        }
        return classroom;
    }

    public void ListClassrooms()
    {
        foreach (var classroom in _classrooms)
        {
            Helper.ShowColoredMsgManuel($"{classroom.Name}", ConsoleColor.DarkBlue);
            if (classroom.Students != null)
            {
                Helper.ShowInfoMsg("Öğretmenler");
                foreach (var teacher in classroom.Teachers)
                {
                    Helper.ShowColoredMsgManuel($"{teacher.Name} {teacher.LastName}", ConsoleColor.DarkGreen);
                }
                
                Helper.ShowInfoMsg("Öğrenciler :");
                foreach (var student in classroom.Students)
                {
                    Helper.ShowColoredMsgManuel($"{student.No} | {student.Name} {student.LastName}", ConsoleColor.DarkMagenta);
                }
            }
        }
    }

    public void DeleteClassroom(string name, StudentRepository student, TeacherRepository teacher)
    {
        var findClass =  _classrooms.FirstOrDefault(c => c.Name == name);
        if (findClass != null)
        {
            _classrooms.Remove(findClass);
            student.RemoveClassroomFromStudents(findClass);
            teacher.RemoveClassFromTeacher(findClass);
            
            Helper.ShowSuccessMsg("Sınıf başarıyla silindi.");
            return;
        }
        Helper.ShowErrorMsg("Sınıf bulunamadı.");
    }
}