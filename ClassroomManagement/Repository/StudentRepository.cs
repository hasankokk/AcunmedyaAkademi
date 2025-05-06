using ClassroomManagement.Helpers;
using ClassroomManagement.Models;

namespace ClassroomManagement.Repository;

public class StudentRepository
{
    private List<Students> _students = new List<Students>();
    private static int _nextNo = 1;
    
    public void AddStudent(Students student, ClassroomRepository classroom)
    {
        student.No = _nextNo++;
        _students.Add(student);
        var rooms = classroom.GetClassroomsList();
        string selectClassName = "";
        if (rooms.Count > 0)
        {
            var className = rooms.Select(r => r.Name).ToArray();
            selectClassName = Helper.AskClass("Hangi sınıfı seçmek istersin", className);
            var studentRoom = classroom.CreateClassrooms(selectClassName);
            student.Classrooms.Add(studentRoom);
            studentRoom.Students.Add(student);
            Helper.ShowSuccessMsg($"Öğrenci {student.No} - {student.Name} {studentRoom.Name} sınıfına eklendi");
        }
        else
            Helper.ShowErrorMsg("Uygun sınıf bulunamadı.");
    }

    public void ListStudent()
    {
        if (_students.Count == 0)
            Helper.ShowErrorMsg("Listelenecek öğrenci yok.");
        foreach (var student in _students)
        {
            var classname = string.Join(", ", student.Classrooms.Select(r => r.Name));
            Helper.ShowColoredMsgManuel(
                $"{student.No} - {student.Name} {student.LastName} {classname}", 
                ConsoleColor.Cyan
            );
        }
    }

    public void RemoveStudent(Students student)
    {
       var findUser = _students.FirstOrDefault(s => s.No == student.No && s.Name == student.Name);
       if (findUser != null)
       {
           foreach (var room in findUser.Classrooms)
           {
               room.Students.Remove(student);
           }
           _students.Remove(student);
           Helper.ShowSuccessMsg($"Öğrenci {student.No} - {student.Name} başarıyla silindi.");
           return;
       }
       Helper.ShowErrorMsg("Öğrenci bulunamadı.");
       var listQuest = Helper.AskOption("Öğrencileri listelemek ister misin?", ["Evet", "Hayır"]);
       if (listQuest == 1)
       {
           ListStudent();
           var removeSelectStr = Helper.Ask("Hangi öğrenciyi silmek istersin? No giriniz...");
           if (int.TryParse(removeSelectStr, out int removeSelectStudent))
           {
               var selectedStudent = _students.FirstOrDefault(s => s.No == removeSelectStudent);
               if (selectedStudent != null)
               {
                   foreach (var room in selectedStudent.Classrooms)
                   {
                       room.Students.Remove(selectedStudent);
                   }

                   _students.Remove(selectedStudent);
                   Helper.ShowSuccessMsg($"Öğrenci {selectedStudent.No} - {selectedStudent.Name} başarıyla silindi.");
               }
               else
               {
                   Helper.ShowErrorMsg("Bu numaraya sahip öğrenci bulunamadı.");
               }
           }
           else
           {
               Helper.ShowErrorMsg("Geçerli bir numara girmelisin.");
           }
       }

    }

    public Students GetStudentNo(int no)
    {
        return _students.FirstOrDefault(s => s.No == no);
    }
    public void RemoveClassroomFromStudents(Classrooms deletedClassroom)
    {
        foreach (var student in _students)
        {
            student.Classrooms.RemoveAll(c => c.Name == deletedClassroom.Name);
        }
    }
}