using StringCompare;

Console.Write("2 Adet kelime giriniz...\n" +
              "1.Kelime : ");
string word = Console.ReadLine();
Console.Write("2.Kelime : ");
string otherWord = Console.ReadLine();

string compareStr = Helper.CompareWords(word, otherWord);
string res = Helper.SeperateWords(compareStr, ", ");

Console.WriteLine(res);
