using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Helpers;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Repositories;

public class StudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Student> GetStudents()
    {
        return _context.Students.ToList();
    }
    public List<Student> GetStudentsWithClassrooms()
    {
        return _context.Students
            .Include(s => s.Classrooms)
            .ToList();
    }
    public Student? GetStudent(long id)
    {
        return _context.Students.Include(c=>c.Classrooms).FirstOrDefault(s => s.StudentTckn == id);
    }
    public bool AddStudent(Student student)
    {
        var findStudent = _context.Students.FirstOrDefault(s => s.StudentTckn == student.StudentTckn);
        var findTeacher = _context.Teachers.FirstOrDefault(t => t.TeacherTckn == student.StudentTckn);        
        if (findStudent != null || findTeacher != null)
        {
            ColoredHelper.ShowErrorMsg("Tckn sistemimizde kayıtlı!");
            return false;
        }
        _context.Students.Add(student);
        _context.SaveChanges();
        return true;
    }
    public bool DeleteStudent(Student? student)
    {
        var deletedStudent = _context.Students.FirstOrDefault(s => s.StudentTckn == student.StudentTckn);
        if (deletedStudent == null)
        {
            return false;
        }
        _context.Students.Remove(deletedStudent);
        _context.SaveChanges();
        return true;
    }

    public void UpdateStudent(long tckn, string? firstName, string? lastName)
    {
        var findStudent = _context.Students.FirstOrDefault(s => s.StudentTckn == tckn);
        if (findStudent == null)
        {
            return ;
        }
        findStudent.Name = firstName;
        findStudent.Surname = lastName;
        _context.SaveChanges();
    }
    public void AssignStudentToClassroom(long studentTckn, int classroomId)
    {
        var student = _context.Students
            .Include(s => s.Classrooms)
            .FirstOrDefault(s => s.StudentTckn == studentTckn);
        var classroom = _context.Classrooms.FirstOrDefault(c => c.ClassroomId == classroomId);

        if (student == null || classroom == null)
        {
            ColoredHelper.ShowErrorMsg("Öğrenci veya sınıf bulunamadı!");
            return ;
        }

        if (student.Classrooms.Any(c => c.ClassroomId == classroomId))
        {
            ColoredHelper.ShowInfoMsg($"{classroom.Name} Öğrenci sınıfa kayıtlı.");
            return ;
        }

        student.Classrooms.Add(classroom);
        _context.SaveChanges();
        return ;
    }

    public void DeleteStudentToClassroom(long studentTckn, int classroomId)
    {
        var student = _context.Students
            .Include(s => s.Classrooms)
            .FirstOrDefault(s => s.StudentTckn == studentTckn);
        var classroom = _context.Classrooms.FirstOrDefault(c => c.ClassroomId == classroomId);
        if (student == null || classroom == null)
        {
            ColoredHelper.ShowErrorMsg("Öğrenci veya sınıf bulunamadı!");
            return ;
        }
        if (!student.Classrooms.Contains(classroom))
        {
            ColoredHelper.ShowInfoMsg("Öğrenci bu sınıfa kayıtlı değil.");
            return ;
        }
        student.Classrooms.Remove(classroom);
        _context.SaveChanges();
        return ;
    }

}