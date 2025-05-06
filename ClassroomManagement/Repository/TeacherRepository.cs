using ClassroomManagement.Models;
using ClassroomManagement.Helpers;
namespace ClassroomManagement.Repository;

public class TeacherRepository
{
   private List<Teachers> _teachers = new List<Teachers>();
   
   public void AddTeacher(Teachers teacher, ClassroomRepository classroom)
   {
      
      _teachers.Add(teacher);
      var rooms = classroom.GetClassroomsList();
      string selectClassName = "";
      if (rooms.Count > 0)
      {
         var className = rooms.Select(r => r.Name).ToArray();
         selectClassName = Helper.AskClass("Hangi sınıfı seçmek istersin", className);
      }
      else
         selectClassName = Helper.Ask("Herhangi bir sınıf yok. Yeni bir sınıf adı gir", true);
        
      var teacherRoom = classroom.CreateClassrooms(selectClassName);
      teacher.Classrooms.Add(teacherRoom);
      teacherRoom.Teachers.Add(teacher);
        
      Helper.ShowSuccessMsg($"Öğretmen {teacher.Name} {teacherRoom.Name} sınıfına eklendi");
   }

   public void ListTeachers()
   {
      if (_teachers.Count == 0)
         Helper.ShowErrorMsg("Listelenecek öğrenci yok.");
      foreach (var teacher in _teachers)
      {
         var classname = string.Join(", ", teacher.Classrooms.Select(r => r.Name));
         Helper.ShowColoredMsgManuel(
            $"{teacher.Name} {teacher.LastName} {classname}", 
            ConsoleColor.Cyan
         );
      }
   }

   public void DeleteTeacher(Teachers teacher)
   {
      var findUser = _teachers.FirstOrDefault(t => t.Name == teacher.Name && teacher.LastName == teacher.LastName);
      if (findUser != null)
      {
         foreach (var room in findUser.Classrooms)
         {
            room.Teachers.Remove(teacher);
         }
         _teachers.Remove(teacher);
         Helper.ShowSuccessMsg($"Öğretmen {teacher.Name} {teacher.LastName} başarıyla silindi.");
         return;
      }
      Helper.ShowErrorMsg("Öğretmen bulunamadı.");
      var listQuest = Helper.AskOption("Öğretmenleri listelemek ister misin?", ["Evet", "Hayır"]);
      if (listQuest == 1)
      {
         ListTeachers();
         var removeSelectName = Helper.Ask("Hangi öğretmeni silmek istersin? İsim giriniz...");
         var removeSelectLastName = Helper.Ask("Hangi öğretmeni silmek istersin? Soyisim giriniz...");
         if (_teachers.Any(t => t.Name == removeSelectName  && t.LastName == removeSelectLastName))
         {
            var selectedTeacher = _teachers.FirstOrDefault(t => t.Name == removeSelectName && t.LastName == removeSelectLastName);
            if (selectedTeacher != null)
            {
               foreach (var room in selectedTeacher.Classrooms)
               {
                  room.Teachers.Remove(selectedTeacher);
               }

               _teachers.Remove(selectedTeacher);
               Helper.ShowSuccessMsg($"Öğretmen {selectedTeacher.Name} {selectedTeacher.LastName} başarıyla silindi.");
            }
            else
            {
               Helper.ShowErrorMsg("Bu isme ve soyisme sahip öğretmen bulunamadı.");
            }
         }
         else
         {
            Helper.ShowErrorMsg("Geçerli bir isim ve soyisim girmelisin.");
         }
      }
   }

   public void RemoveClassFromTeacher(Classrooms deletedClassroom)
   {
      foreach (var teacher in _teachers)
      {
         teacher.Classrooms.RemoveAll(c => c.Name == deletedClassroom.Name);
      }
   }

   public Teachers GetTeacherNameSurname(string name, string surname)
   {
      return _teachers.FirstOrDefault(t => t.Name == name && t.LastName == surname);
   }
}