using System.Reflection.Metadata;

namespace MethodPractice;

public class Helper
{
    public static string MenuMessage ()
    {
        Console.Clear();
        return "Yapabileceğiniz işlemler :\n" +
               "0 - Çıkış Yapmak İçin\n" +
               "1 - KDV Hesaplama\n" +
               "2 - Kimlik Bilgileri\n" +
               "3 - İndirim Sonrası Hesabı\n" +
               "4 - Tüm Sayıları Dizi Yap\n" +
               "5 - Cümleyi Tersine Çevirme\n" +
               "6 - Kelime'yi Tersine Çevirince Aynı Mı?\n" +
               "7 - Cümle Kaç Kelimeden Oluşuyor?\n" +
               "8 - İlk Harfleri Büyüt Gerisini Küçült\n" +
               "9 - Sayı Tek mi? Çift mi?\n" +
               "10 - Sayıları Tek ve Çift Olarak Gruplandırma\n";
        
    }

    public static double KdvCalculate(double number, double rate)
    {
        Console.Clear();
        Console.Write("Girdiğiniz tutara KDV dahil mi? y/n");
        string input = Console.ReadLine();
        double netResult;
        if (input == "y" || input == "yes" || input == "Y")
        {
            netResult = number / (1 + (rate / 100));
            Console.WriteLine("Kdv'siz tutar : ");
            return netResult;
        }
        else
        {
            netResult = number * (1 + (rate / 100));
            Console.WriteLine("Kdv'li tutar : ");
            return netResult;
        }
    }

    public static string GetIdentyInformation(Identy person)
    {
        Console.Clear();
        int nowTime = DateTime.Now.Year;
        int personAge;
        if (person.UserGender == "E" || person.UserGender == "e")
        {
            personAge = nowTime - person.UserAge;
            if (personAge >= 18)
            {
                Console.WriteLine($"{person.UserName} Yaşın : {personAge}" +
                                  $" Askerlik yaptın mı? E/H");
                var input = Console.ReadLine();
                Thread.Sleep(700);
                if (input == "E" ||  input == "e")
                {
                    person.UserMilitary = true;
                    return "Askerlik görevini tamamlamış.";
                }
                else if (input == "H" || input == "h")
                {
                    person.UserMilitary = false;
                    return "Askerlik görevini tamamlamamış...";
                }
            }
        }
        else if (person.UserGender == "k" || person.UserGender == "K")
            return "Türkiye'de kadınlara zorunlu askerlik görevi yok.";
        return "Askerlik yaşı gelmemiş.";
    }

    public static double CampaignCalculate(double number, double rate)
    {
        Console.Clear();
        double campaign = rate / 100;
        double netPrice = number * ((100 - rate) / 100);
        return netPrice;
    }

    public static List<int[]> NumberToArray(int start, int end)
    {
        List<int[]> intArr = new List<int[]>();
        for (int i = start; i < end; i++)
        {
            int[] arr = new int[] {i, i};
            intArr.Add(arr);
        }
        return intArr;
    }
    
    public static string ReverseIt(string str)
    {
        var tempStr = "";
        for (int i = str.Length - 1; i >= 0; i--)
            tempStr += str[i];
        return tempStr;
    }

    public static bool CheckSameWord(string word, string reverseWord)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] != reverseWord[i])
                return false;
        }
        return true;
    }

    public static int FindWordCount(string word)
    {
        int wordCount = 0;
        string[] words = word.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            Console.WriteLine($"Index {i}: <{words[i]}>");
            wordCount++;
        }
        return wordCount;
    }

    public static string FixToUpperLower(string name)
    {
        string fixedName = "";
        for (int i = 0; i < name.Length; i++)
        { 
            if (i == 0 && name[i] >= 'a' && name[i] <= 'z')
                fixedName += (char)(name[i] - 32);
            if (i > 0 && name[i] >= 'A' && name[i] <= 'Z')
                fixedName += (char)(name[i] + 32);
            else if (i == 0 && name[i] >= 'A' && name[i] <= 'Z')
                fixedName += name[i];
            else if (i > 0)
                fixedName += name[i];
        }
        return fixedName;
    }

    public static int EvenOrOdd(int number)
    {
        if (number % 2 == 0)
            return number % 2;
        return number % 2;
    }
    /*
     * ref kullanımı : değişkeni ref olarak gönderdiğimde değişkenin bellekteki adresini methoda gönderir
     * method içerisinde yapılan değişiklikler direkt olarak değişkene yansır.
     */
    public static int AddListEvenOdd(int number, List<int> oddList, List<int> evenList, ref int oddCount, ref int evenCount)
    {
        if (number % 2 == 0)
        {
            oddCount++;
            oddList.Add(number);
            return number % 2;
        }
        else
        {
            evenCount++;
            evenList.Add(number);
            return number % 2;
        }
    }
}