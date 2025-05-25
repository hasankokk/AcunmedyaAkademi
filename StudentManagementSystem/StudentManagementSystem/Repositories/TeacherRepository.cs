using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Helpers;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Repositories
{
    public class TeacherRepository
    {
        private readonly AppDbContext _context;

        public TeacherRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Teacher> GetTeachers()
        {
            return _context.Teachers.ToList();
        }

        public List<Teacher> GetTeachersWithClassrooms()
        {
            return _context.Teachers
                .Include(t => t.Classrooms)
                .ToList();
        }

        public Teacher? GetTeacher(long tckn)
        {
            return _context.Teachers
                .Include(t => t.Classrooms)
                .FirstOrDefault(t => t.TeacherTckn == tckn);
        }

        public bool AddTeacher(Teacher teacher)
        {
            var existsStudent = _context.Students.Any(s => s.StudentTckn == teacher.TeacherTckn);
            var existsTeacher = _context.Teachers.Any(t => t.TeacherTckn == teacher.TeacherTckn);
            if (existsStudent || existsTeacher)
            {
                ColoredHelper.ShowErrorMsg("TCKN sistemde kayıtlı!");
                return false;
            }

            _context.Teachers.Add(teacher);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteTeacher(Teacher teacher)
        {
            var removeTeacher = _context.Teachers.FirstOrDefault(t => t.TeacherTckn == teacher.TeacherTckn);
            if (removeTeacher == null)
            {
                return false;
            }
            _context.Teachers.Remove(removeTeacher);
            _context.SaveChanges();
            return true;
        }

        public void UpdateTeacher(long tckn, string? firstName, string? lastName)
        {
            var updateTeacher = _context.Teachers.FirstOrDefault(t => t.TeacherTckn == tckn);
            if (updateTeacher == null)
                return ;

            updateTeacher.Name = firstName;
            updateTeacher.Surname = lastName;
            _context.SaveChanges();
        }

        public bool AssignTeacherToClassroom(long teacherTckn, int classroomId)
        {
            var teacher = _context.Teachers.Include(t => t.Classrooms).FirstOrDefault(t => t.TeacherTckn == teacherTckn);
            var classroom = _context.Classrooms.FirstOrDefault(c => c.ClassroomId == classroomId);
            if (teacher == null || classroom == null)
            {
                ColoredHelper.ShowErrorMsg("Öğretmen veya sınıf bulunamadı!");
                return false;
            }
            if (teacher.Classrooms.Any(c => c.ClassroomId == classroomId))
            {
                ColoredHelper.ShowInfoMsg($"{classroom.Name} öğretmen zaten sınıfa kayıtlı.");
                return false;
            }
            teacher.Classrooms.Add(classroom);
            _context.SaveChanges();
            return true;
        }

        public void RemoveTeacherFromClassroom(long teacherTckn, int classroomId)
        {
            var teacher = _context.Teachers.Include(t => t.Classrooms).FirstOrDefault(t => t.TeacherTckn == teacherTckn);
            var classroom = _context.Classrooms.FirstOrDefault(c => c.ClassroomId == classroomId);
            if (teacher == null || classroom == null)
            {
                ColoredHelper.ShowErrorMsg("Öğretmen veya sınıf bulunamadı!");
                return ;
            }
            var classroomToRemove = teacher.Classrooms.FirstOrDefault(c => c.ClassroomId == classroomId);
            if (classroomToRemove == null)
            {
                ColoredHelper.ShowInfoMsg($"{classroom.Name} öğretmene bağlı değil.");
                return ;
            }
            teacher.Classrooms.Remove(classroomToRemove);
            _context.SaveChanges();
        }
    }
}
