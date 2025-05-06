using ClassroomManagement.Helpers;
using ClassroomManagement.Models;
using ClassroomManagement.Repository;

var classroomRepository = new ClassroomRepository();
var studentRepository = new StudentRepository();
var teacherRepository = new TeacherRepository();

while(true)
{
    var selectMenu = Helper.MenuMessage("firstMenu");
    int selectSecondMenu;
    if (selectMenu == 1)
    {
        Thread.Sleep(1000);
        Console.Clear();
        selectSecondMenu = Helper.MenuMessage("StudentManagement");
        if (selectSecondMenu == 1)
        {
            studentRepository.ListStudent();
            Helper.ShowInfoMsg("Devam etmek için herhangi bir tuşa basın...");
            Console.ReadKey(true);
            Thread.Sleep(1000);
            Console.Clear();
        }
        if (selectSecondMenu == 2)
        {
            Thread.Sleep(1000); 
            Console.Clear();
            var studentName = Helper.Ask("Eklemek istediğiniz öğrencinin adı", true);
            var studentSurName = Helper.Ask("Eklemek istediğiniz öğrencinin soyadı", true);
            Students student = new Students
            {
                Name = studentName,
                LastName = studentSurName,
            };
            //Sınıf listesi verilecek
            studentRepository.AddStudent(student, classroomRepository);
            Thread.Sleep(1000);
            Console.Clear();
        }

        if (selectSecondMenu == 3)
        {
            studentRepository.ListStudent();
            var studentNoStr = Helper.Ask("Silmek istediğiniz öğrencinin nosu", true);
            if (int.TryParse(studentNoStr, out int studentNo))
            {
                var removedStudent = studentRepository.GetStudentNo(studentNo);
                if (removedStudent != null)
                    studentRepository.RemoveStudent(removedStudent);
            }
            else
                Helper.ShowErrorMsg("Hatalı öğrenci numarası girişi, tekrar deneyin...");
            Thread.Sleep(1000);
            Console.Clear();
        }
    }

    else if (selectMenu == 2)
    {
        Thread.Sleep(1000);
        Console.Clear();
        selectSecondMenu = Helper.MenuMessage("TeacherManagement");
        if (selectSecondMenu == 1)
        {
            teacherRepository.ListTeachers();
            Helper.ShowInfoMsg("Devam etmek için herhangi bir tuşa basın...");
            Console.ReadKey(true);
            Thread.Sleep(1000);
            Console.Clear();
        }
        if (selectSecondMenu == 2)
        {
            var teacherName = Helper.Ask("Eklemek istediğiniz öğretmenin adı", true);
            var teacherSurName = Helper.Ask("Eklemek istediğiniz öğretmenin soyadı", true);
            Teachers teacher = new Teachers
            {
                Name = teacherName,
                LastName = teacherSurName,
            };
            teacherRepository.AddTeacher(teacher, classroomRepository);
            Thread.Sleep(1000);
            Console.Clear();
        }

        if (selectSecondMenu == 3)
        {
            teacherRepository.ListTeachers();
            var teacheerName = Helper.Ask("Silmek istediğiniz öğretmenin Adı", true);
            var teacheerSurName = Helper.Ask("Silmek istediğiniz öğretmenin Soyadı", true);
            var removedTeacher = teacherRepository.GetTeacherNameSurname(teacheerName, teacheerSurName);
            if (removedTeacher != null)
            {
                teacherRepository.DeleteTeacher(removedTeacher);
            }
            else
                Helper.ShowErrorMsg("Hatalı öğretmen girişi, tekrar deneyin...");
            Thread.Sleep(1000);
            Console.Clear();
            
        }
    }
    else if (selectMenu == 3)
    {
        Thread.Sleep(1000);
        Console.Clear();
        selectSecondMenu = Helper.MenuMessage("ClassManagement");
        if (selectSecondMenu == 1)
        {
            classroomRepository.ListClassrooms();
            Helper.ShowInfoMsg("Devam etmek için herhangi bir tuşa basın...");
            Console.ReadKey(true);
            Thread.Sleep(1000);
            Console.Clear();
        }

        if (selectSecondMenu == 2)
        {
            var roomName = Helper.Ask("Oluşturmak istediğiniz sınıf adını giriniz", true);
            classroomRepository.CreateClassrooms(roomName);
            Thread.Sleep(1000);
            Console.Clear();
        }

        if (selectSecondMenu == 3)
        {
            classroomRepository.ListClassrooms();
            var selectRoom = Helper.Ask("Silmek istediğiniz sınıf adı", true);
            classroomRepository.DeleteClassroom(selectRoom, studentRepository, teacherRepository);
            Thread.Sleep(1000);
            Console.Clear();
        }
    }

    if (selectMenu == 4)
    {
        Helper.ShowColoredMsgManuel("Görüşmek üzere...", ConsoleColor.Black);
        break;
    }
}
