using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Helpers;

public class Auth
{
    private readonly StudentHelper _studentHelper;
    private readonly ClassroomHelper _classroomHelper;
    private readonly TeacherHelper _teacherHelper;

    public Auth(AppDbContext context)
    {
        _studentHelper = new StudentHelper(context);
        _classroomHelper = new ClassroomHelper(context);
        _teacherHelper = new TeacherHelper(context);
    }
    public enum RegisterStatus
    {
        Success,
        InvalidPassword,
        InvalidTckn
    }
    
    public static RegisterStatus Register(long tckn)
    {
        if (!Validation.IsValidTckn(tckn))
            return RegisterStatus.InvalidTckn;
        return RegisterStatus.Success;
    }
    
    public  void RegisterUser(string registerType)
    {
        var inputTckn = long.Parse(Helper.Ask("Tckn", true));
        var isValidRegister = Register(inputTckn);
    
        switch (isValidRegister)
        {
            case RegisterStatus.Success:
                var inputName = Helper.Ask("Ad", true);
                var inputSurname = Helper.Ask("Soyad", true);
            
                if (registerType == "Student")
                {
                    var selectedClassroom = _classroomHelper.AskClassroom(registerType);
                    int? classroomId = selectedClassroom?.ClassroomId;

                    if(_studentHelper.RegisterStudent(inputName, inputSurname, inputTckn, classroomId))
                    {
                        ColoredHelper.ShowSuccessMsg("Öğrenci kaydı başarılı!");
                    }
                    else
                    {
                        ColoredHelper.ShowErrorMsg("Öğrenci kaydı başarısız!");
                    }
                }

                if (registerType == "Teacher")
                {
                    var selectedClassroom = _classroomHelper.AskClassroom(registerType);
                    int? classroomId = selectedClassroom?.ClassroomId;                   
                    if (_teacherHelper.RegisterTeacher(inputName, inputSurname, inputTckn, classroomId))
                    {
                        ColoredHelper.ShowSuccessMsg("Öğretmen kaydı başarılı!");
                    }
                    else
                    {
                        ColoredHelper.ShowErrorMsg("Öğretmen kaydı başarısız!");
                    }
                }
                break;
            case RegisterStatus.InvalidPassword:
                break;
            case RegisterStatus.InvalidTckn:
                ColoredHelper.ShowErrorMsg("Geçersiz Tckn!");
                break;
        }
    }

}