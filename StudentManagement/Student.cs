using StudentManagement.Helpers;

namespace StudentManagement;

public class Student
{
    private List<Student> _students = new List<Student>();
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public string? Surname { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string? Gender { get; private set; }
    public string? Email { get; private set; }
    
    public Student(int id,string? name, string? surname, DateOnly age, string? gender, string? email)
    {
        Id = StudentHelper.GenerateStudentId();
        Name = name;
        Surname = surname;
        BirthDate = age;
        Gender = gender;
        Email = email;
    }
    
    public void AddStudent(Student student)
    {
        _students.Add(student);
    }
    public void GetAllStudents()
    {
        Helpers.Helpers.PrintInputMessage("ID | İsim | Soyisim | Yaş | Cinsiyet | Email\n");
        for (int i = 0; i < _students.Count; i++)
        {
            var student = _students[i];
                Helpers.Helpers.PrintInputMessage($"{student.Id}. | {student.Name} | {student.Surname} | {student.GetAge()} | {student.Gender} | {student.Email}", ConsoleColor.Yellow);
                Console.WriteLine();
        }
    }
    
    public void StudentRemove(int studentId)
    {
        for(int i = 0; i < _students.Count; i++)
        {
            if (_students[i].Id == studentId)
                _students.Remove(_students[i]);
        }
        Helpers.Helpers.PrintInputMessage("Öğrenci başarıyla silindi.");
    }

    public int GetAge()
    {
        return CalculateAge();
    }
    private int CalculateAge()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - BirthDate.Year;
        if (today < BirthDate.AddYears(age))
        {
            age--;
        }

        return age;
    }
    
    public void UpdateName(int studentId, string newName)
    {
        for (int i = 0; i < _students.Count; i++)
        {
            if (_students[i].Id == studentId)
                _students[i].Name = newName;
        }
    }

    public void UpdateSurname(int studentId, string newSurname)
    {
        for (int i = 0; i < _students.Count; i++)
        { 
            if (_students[i].Id == studentId)
                _students[i].Surname = newSurname;
        }
    }

    public void UpdateEmail(int studentId, string newEmail)
    {
        for  (int i = 0; i < _students.Count; i++)
        {
            if (_students[i].Id == studentId)
                _students[i].Email = newEmail;
        }
    }
}