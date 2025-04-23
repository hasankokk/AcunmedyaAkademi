namespace Calculator;

public class CalculatorHelper
{
    public List<int> numberList = new List<int>();
    public List<int> AddListNumbers(int numb)
    {
        numberList.Add(numb);
        return numberList;
    }

    public bool KeyTControl(string key)
    {
        if (key == "t")
            return true;
        return false;
    }
    /*
     * Kullanıcıdan alınan parapmetreyi kontrol eder ve geçerli ise out ile değeri
     * dışarıya gönderebiliriz. Method sonucunda ise bir koşul kontrolü yapmış oluruz.
     * Dönüş değerimiz bool çünkü.
     */
    public bool TryToInt(string str, out int res)
    {
        if (int.TryParse(str, out res))
            return true;
        else
        {
            Console.WriteLine("Yanlış giriş lütfen 't' ya da bir sayı giriniz...");
            return false;
        }
    }

    public string PrintInformation(int totalProcess, int total)
    {
        return $"Girilen toplam sayı adedi : {totalProcess}\n" +
               $"Girilen sayıların toplamı : {total}\n";
    }

    public int AddUpNumbers(int total)
    {
        foreach (var number in numberList)
        {
            total += number;
        }
        
        return total;
    }

    public string WelcomeMessage()
    {
        return "Hoş geldiniz...\n" +
               "Sizler 't' harfine basana kadar girilen tüm sayılar\n" +
               "Bir Listede tutulup 't' harfine bastığınız da toplamı ekranınıza getirecek!";
    }
}