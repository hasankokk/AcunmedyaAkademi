using Calculator;

var numbers = new CalculatorHelper();

Console.WriteLine(numbers.WelcomeMessage());
int totalProcess = 0;
int total = 0;
while (true)
{
    Console.Write("Sayı giriniz (Toplama yapmak için 't'): ");
    string input = Console.ReadLine();
    int inputNumber;
    if (!numbers.KeyTControl(input) && !string.IsNullOrEmpty(input) 
                                    && numbers.TryToInt(input, out inputNumber))
    {
        var listArg = numbers.AddListNumbers(inputNumber);
        totalProcess++;
        foreach (var arg in listArg)
        {
            Console.WriteLine(arg);
        }
    } 
    
    else if (numbers.KeyTControl(input) && !string.IsNullOrEmpty(input))
    {
        total = numbers.AddUpNumbers(total);
        Console.WriteLine(numbers.PrintInformation(totalProcess, total));
        break;
    }
    else
        Console.WriteLine("Hatalı bir giriş yaptınız...");
}

