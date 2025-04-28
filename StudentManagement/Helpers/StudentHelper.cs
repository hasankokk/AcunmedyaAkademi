namespace StudentManagement.Helpers;

public static class StudentHelper
{
    private static int _generateStudentId = 0;

    public static int GenerateStudentId()
    {
        return _generateStudentId++;
    }
}