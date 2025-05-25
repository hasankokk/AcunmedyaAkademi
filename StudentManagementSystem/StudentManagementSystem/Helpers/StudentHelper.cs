using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories;

namespace StudentManagementSystem.Helpers
{
    public class StudentHelper
    {
        private readonly StudentRepository _studentRepository;
        private readonly ClassroomRepository _classroomRepository;
        private readonly ClassroomHelper _classroomHelper;

        // Constructor ile bağımlılıkları alıyoruz
        public StudentHelper(AppDbContext context)
        {
            _studentRepository = new StudentRepository(context);
            _classroomRepository = new ClassroomRepository(context);
            _classroomHelper = new ClassroomHelper(context);
        }

        public bool RegisterStudent(string? inputName, string? inputSurname, long inputTckn, int? classroomId = null)
        {
            var student = new Student
            {
                Name = inputName,
                Surname = inputSurname,
                StudentTckn = inputTckn
            };
            var alreadyExists = _studentRepository.AddStudent(student);
            if (!alreadyExists)
            {
                return false;
            }
            
            if (classroomId.HasValue)
            {
                var classroom = _classroomRepository.GetClassroomById(classroomId.Value);
                if (classroom != null)
                {
                    _studentRepository.AssignStudentToClassroom(student.StudentTckn, classroomId.Value);
                }
                else
                {
                    ColoredHelper.ShowErrorMsg("Belirtilen sınıf bulunamadı!");
                    return false;
                }
            }

            return true;
        }

        public void RemoveStudent()
        {
            var allStudent = _studentRepository.GetStudents();
            if (allStudent.Any())
            {
                ColoredHelper.ShowErrorMsg("Listelenecek öğrenci bulunamadı!");
                return;
            }
            foreach (var studentList in allStudent)
            {
                ColoredHelper.ShowListMsg($"{studentList.StudentTckn} - {studentList.Name} {studentList.Surname}");
            }
            var removeStudent = long.Parse(Helper.Ask("Silmek istediğiniz öğrencinin TCKN giriniz", true));
            var student = _studentRepository.GetStudent(removeStudent);
            bool successRemove;
            if (student != null)
            {
                successRemove = _studentRepository.DeleteStudent(student);
                if  (successRemove)
                {
                    ColoredHelper.ShowSuccessMsg("Öğrenci başarıyla silindi!");
                    return;
                }
                ColoredHelper.ShowErrorMsg("Öğrenci silinemedi!");
            }
        }

        public void UpdateStudent()
        {
            var allStudent = _studentRepository.GetStudents();
            if (!allStudent.Any())
            {
                ColoredHelper.ShowErrorMsg("Listelenecek öğrenci bulunamadı!");
                return;
            }
            foreach (var studentList in allStudent)
            {
                ColoredHelper.ShowListMsg($"{studentList.StudentTckn} - {studentList.Name} {studentList.Surname}");
            }
            var updateStudent = long.Parse(Helper.Ask("Güncellemek istediğiniz öğrencinin TCKN giriniz", true));
            var student = _studentRepository.GetStudent(updateStudent);
            if (student != null)
            {
                var inputName = Helper.Ask("Yeni bir isim giriniz");
                var newName = string.IsNullOrWhiteSpace(inputName) ? student.Name : inputName;

                var inputSurname = Helper.Ask("Yeni bir soyisim giriniz");
                var newSurname = string.IsNullOrWhiteSpace(inputSurname) ? student.Surname : inputSurname;

                var updateClassroom = Helper.AskOption(["Ekle", "Sil"], "Sınıf güncellemesi yapmak istiyor musun", "Vazgeç");
                if (updateClassroom == 1) // Ekle
                {
                    var existingClassroomIds = student.Classrooms?.Select(c => c.ClassroomId).ToList() ?? new List<int>();

                    var selectUpdate = _classroomHelper.UpdateClassRoom("Ekle", existingClassroomIds);

                    foreach (var classroom in selectUpdate)
                    {
                        _studentRepository.AssignStudentToClassroom(student.StudentTckn, classroom.ClassroomId);
                    }
                }

                if (updateClassroom == 2) // Sil
                {
                    var existingClassrooms = student.Classrooms?.ToList() ?? new List<Classroom>();
                    var selectToRemove = _classroomHelper.UpdateClassRoomsForRemove(existingClassrooms);

                    foreach (var classroom in selectToRemove)
                    {
                        _studentRepository.DeleteStudentToClassroom(student.StudentTckn, classroom.ClassroomId);
                    }
                }
                _studentRepository.UpdateStudent(student.StudentTckn, newName, newSurname);
            }
        }

        public void ListStudent()
        {
            var students = _studentRepository.GetStudentsWithClassrooms();

            if (!students.Any())
            {
                ColoredHelper.ShowErrorMsg("Kayıtlı öğrenci bulunamadı!");
                return;
            }

            foreach (var student in students)
            {
                var classroomNames = student.Classrooms != null && student.Classrooms.Any()
                    ? string.Join(", ", student.Classrooms.Select(c => c.Name))
                    : "Sınıf yok";

                ColoredHelper.ShowListMsg($"{student.StudentTckn} - {student.Name} {student.Surname} | Sınıflar: {classroomNames}");
            }
        }
    }
}