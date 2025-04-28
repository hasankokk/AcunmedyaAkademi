using StudentManagement;
using StudentManagement.Helpers;

Console.Clear();
Helpers.PrintInputMessage("Hoş geldiniz...\n" +
                     "Yapmak istediğiniz işlemi seçiniz.");
Student student = new Student(0,"Hasan", "Kök", new DateOnly(1998, 2, 25) , "Erkek", "hsnnkok@gmail.com");
while (true)
{
    Helpers.MenuMessage();
    Console.Write("\nSeçiminiz : ");
    int choise = int.Parse(Console.ReadLine());
    if (choise == 1)
    {
        Console.Clear();
        var inputStudentName = Helpers.Ask("\nÖğrenci Adı", true);
        var inputStudentSurname = Helpers.Ask("\nÖğrenci Soyadı", true);
        Console.Write("\nÖğrencinin Doğum Yılı : ");
        int inputStudentYear = int.Parse(Console.ReadLine());
        Console.Write("\nÖğrencinin Doğum Ayı : ");
        int inputStudentMonth = int.Parse(Console.ReadLine());
        Console.Write("\nÖğrencinin Doğum Günü : ");
        int inputStudentDay = int.Parse(Console.ReadLine());
        DateOnly date = new DateOnly(inputStudentYear, inputStudentMonth, inputStudentDay);
        var input =  Helpers.AskOption("\nÖğrencinin Cinsiyeti", ["Erkek", "Kadın"]);
        string inputStudentGender = input == 1 ? "Erkek" : "Kadın";
        string inputStudentEmail =  Helpers.Ask("\nÖğrenci Maili", true);
        student.AddStudent(new Student(0, inputStudentName, inputStudentSurname, date, inputStudentGender, inputStudentEmail));
    }
    else if (choise == 2)
    {
        Console.Clear();
        student.GetAllStudents();
        Helpers.PrintInputMessage("\nDevam etmek için herhangi bir tuşa basın...", ConsoleColor.DarkRed);
        Console.ReadKey(true);
    }
    else if (choise == 3)
    {
        Console.Clear();
        Helpers.PrintInputMessage("Silmek istediğiniz öğrenciyi seçiniz : \n", ConsoleColor.DarkRed);
        student.GetAllStudents();
        Helpers.PrintInputMessage("Öğrenci ID giriniz : ", ConsoleColor.DarkRed);
        student.StudentRemove(int.Parse(Console.ReadLine()));
        Helpers.PrintInputMessage("\nDevam etmek için herhangi bir tuşa basın...", ConsoleColor.DarkRed);
        Console.ReadKey(true);
    }
    else if (choise == 4)
    {
        Console.Clear();
        student.GetAllStudents();
        var updateStudent = int.Parse(Helpers.Ask("Güncellemek istediğiniz Öğrenci ID'sini seçiniz...", true));
        var choiseUpdate = Helpers.AskOption("Güncellemek istediğiniz bilgiyi seçin : ", ["İsim", "Soyisim", "Email"]);
        string updateParam;
        if (choiseUpdate == 1)
        {
            updateParam = Helpers.Ask("Lütfen bir isim giriniz", true);
            student.UpdateName(updateStudent, updateParam);
        }
        else if (choiseUpdate == 2)
        {
            updateParam = Helpers.Ask("Lütfen bir soyisim giriniz", true);
            student.UpdateSurname(updateStudent, updateParam);
        }
        else if (choiseUpdate == 3)
        {
            updateParam = Helpers.Ask("Lütfen bir mail giriniz", true);
            student.UpdateEmail(updateStudent, updateParam);
        }

    }
    else if (choise == 5)
    {
        Console.Clear();
        Helpers.PrintInputMessage("Görüşmek üzere...", ConsoleColor.Blue);
        Thread.Sleep(2000);
        break;
    }
    
}