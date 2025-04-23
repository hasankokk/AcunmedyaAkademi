using MethodPractice;

while (true)
{
    Thread.Sleep(700);
    Console.WriteLine(Helper.MenuMessage());
    Console.Write("Ne yapmak istersiniz : ");
    string choice = Console.ReadLine();
    switch(choice)
    {
        case "0":
            Console.WriteLine("Yeniden görüşmek üzere...");
            Thread.Sleep(700);
            return;
        case "1":
            Console.Write("İşlem yapmak istediğiniz tutar: ");
            var input = Console.ReadLine();
            double number = double.Parse(input);
            Console.Write("Kdv Oranı: ");
            input = Console.ReadLine();
            double rate = double.Parse(input);
            double res = Helper.KdvCalculate(number, rate);
            Console.WriteLine($"{res}\n" +
                              $"Devam etmek için enter'e basın...");
            Console.ReadLine();
            Console.Clear();
            break;
        case "2":
            Console.Write("Adın : ");
            var personName = Console.ReadLine();
            Console.Write("Doğum Yılı : ");
            var personAge = int.Parse(Console.ReadLine());
            Console.Write("Cinsiyet : ");
            var personGender = Console.ReadLine();
            var person = new Identy
            {
                UserName = personName,
                UserAge = personAge,
                UserGender = personGender,
                UserMilitary = false
            };
            string militaryAgeCalculator = Helper.GetIdentyInformation(person);
            Console.WriteLine($"{person.UserName} {militaryAgeCalculator}\n" +
                              $"Devam etmek için enter'e basın...");
            Console.ReadLine();
            Console.Clear();
            break;
        case "3":
            Console.Write("Ürün Fiyatınızı Giriniz : ");
            double productPrice = double.Parse(Console.ReadLine());
            Console.Write("İndirim Oranı Giriniz : ");
            double productCampaign = double.Parse(Console.ReadLine());
            double afterCampaignPrice = Helper.CampaignCalculate(productPrice, productCampaign);
            Console.WriteLine($"{afterCampaignPrice} TL ürünün yeni fiyatı.\n" + 
                              $"Devam etmek için enter'e basın...");
            Console.ReadLine();
            Console.Clear();
            break;
        case "4":
            Console.Write("Diziye dönüştürmek istediğiniz aralığı belirtiniz.\n" +
                              "Başlangıç : ");
            int startIndex = int.Parse(Console.ReadLine());
            Console.Write("Bitiş : ");
            int endIndex = int.Parse(Console.ReadLine());
            
            List<int[]> arr = Helper.NumberToArray(startIndex, endIndex);
            foreach (var arrArg in arr)
            {
                foreach (var arg in arrArg)
                    Console.Write($"[ {arg} ]");
                Console.WriteLine();
            }
            Console.WriteLine("Devam etmek için enter'e basın...");
            Console.ReadLine();
            Console.Clear();
            break;
        case "5":
            Console.Write("Tersten yazdırmak istediğiniz cümleyi yazınız : ");
            var reverseString = Helper.ReverseIt(Console.ReadLine());
            Console.WriteLine($"{reverseString}\n" +
                              $"Devam etmek için enter'e basın...");
            Console.ReadLine();
            Console.Clear();
            break;
        case "6":
            Console.Write("Tersten yazdığında aynı mı? Değil mi?\n" +
                          "Kontrol etmek için kelime girin : ");
            var inputWord = Console.ReadLine();
            var reversWord = Helper.ReverseIt(inputWord);
            if (!Helper.CheckSameWord( inputWord, reversWord))
            {
                Console.WriteLine($"Girilen kelime : {inputWord} Tersi : {reversWord}\nAynı değil!\n" +
                                  $"Devam etmek için enter'e basın...");
                Console.ReadLine();
                Console.Clear();
                break;
            }
            Console.WriteLine($"Girilen kelime : {inputWord} Tersi : {reversWord}\nAynı!\n" +
                              $"Devam etmek için enter'e basın...");
            
            Console.ReadLine();
            Console.Clear();
            break;
        case "7":
            Console.Write("Cümle kaç kelimeden oluşuyor öğrenmek ister misin?\n" +
                          "Bir cümle gir : ");
            var howManyWord = Console.ReadLine();
            int wordCount = Helper.FindWordCount(howManyWord);
            Console.WriteLine($"Girilen cümle : {howManyWord} Kelime sayısı : {wordCount}\n" +
                              $"Devam etmek için enter'e basın...");
            
            Console.ReadLine();
            Console.Clear();
            break;
        case "8":
            Console.Write("Adını soyadını istediğin gibi yaz ben düzenleyeceğim.\n" +
                          "Ad : ");
            var inputName = Console.ReadLine();
            var fixedName = Helper.FixToUpperLower(inputName);
            Console.Write("Soyad : ");
            var inputLastName = Console.ReadLine();
            var fixedLastName = Helper.FixToUpperLower(inputLastName);
            
            Console.WriteLine($"Ad : {fixedName} Soyad : {fixedLastName}\n" +
                              $"Devam etmek için enter'e basın...");
            
            Console.ReadLine();
            Console.Clear();
            break;
        case "9":
            Console.Write("Girdiğin sayı çift mi? Tek mi? Öğrenmek ister misin?\n" +
                          "Hadi bir sayı gir : ");
            int evenOrOdd = int.Parse(Console.ReadLine());
            int resEvenOrOdd = Helper.EvenOrOdd(evenOrOdd);
            string resultEvenOdd = resEvenOrOdd > 0 ? "Sayı Tek!" : "Sayı Çift!";
            Console.WriteLine($"{resultEvenOdd}\n" +
                              $"Devam etmek için enter'e basın...");
            Console.ReadLine();
            Console.Clear();
            break;
        case "10":
            var evenNumber = new List<int>();
            var oddNumber = new List<int>();
            int evenCount = 0;
            int oddCount = 0;
            while(true)
            {
                Console.Write("Girdiğin sayı çift mi? Tek mi? Öğrenmek ister misin?\n" +
                              "Hadi bir sayı gir : ");
                
                int evenOdd = int.Parse(Console.ReadLine());
                int resEvenOdd = Helper.AddListEvenOdd(evenOdd, oddNumber, evenNumber, ref oddCount, ref evenCount);
                string resListEvenOdd = resEvenOdd > 0 ? "\nSayı Tek!\n" : "\nSayı Çift!\n";

                Console.Write("Tek Sayılar : ");
                foreach (var even in evenNumber)
                    Console.Write($" {even} ");
                Console.Write("Çift Sayılar : ");
                foreach (var odd in oddNumber)
                    Console.Write($" {odd} ");
                
                Console.WriteLine($"{resListEvenOdd}\n" +
                                  $"Toplam Tek Sayı : {evenCount}\n" +
                                  $"Toplam Çift Sayı :  {oddCount}\n" +
                                  $"Devam etmek için enter'e basın...\n" +
                                  $"Çıkış yapmak için : 'exit' yaz");
                var exitMessage = Console.ReadLine();
                Console.Clear();
                if (exitMessage == "exit")
                    break;
            }

            break;
        default:
            Console.WriteLine("Geçersiz bir giriş yaptınız...\n" + 
                              "Lütfen tekrar deneyiniz...");
            break; 
    }
}