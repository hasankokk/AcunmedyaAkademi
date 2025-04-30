using WordGame.Helpers;

string word = await WordGameHelper.LoadWord();
List<string> forecasts = new List<string>();
int tryGuess = 5;
while (tryGuess != 0)
{   
    WordGameHelper.ForecastPrint(word, forecasts);
    var inputWord = Helper.Ask("Kelime tahmini gir", tryGuess, true, "Lütfen kelime girişi yapınız!\n");
    if (inputWord.Length != word.Length)
    {
        Helper.ShowErrorMsg("Yanlış kelime uzunluğu girdiniz.");
        Thread.Sleep(1200);
        Console.Clear();
        continue;
    }
    if (forecasts.Contains(inputWord))
    {
        Helper.ShowErrorMsg("Daha önce girdiğiniz bir tahminde bulundunuz.\n");
        continue;
    }
    
    forecasts.Add(inputWord);
    if (inputWord == word)
    {
        Console.Clear();
        WordGameHelper.ForecastPrint(word, forecasts);
        Helper.ShowSuccessMsg("\aTebrikler! Kelimeyi doğru tahmin ettin!\n" +
                              "Çıkış yapmak için herhangi bir tuşa bas...");
        Console.ReadKey();
        break;
    }
    tryGuess--;
    if (tryGuess == 0)
    {
        Helper.ShowErrorMsg("Ne yazık ki deneme hakkın kalmadı...");
        Helper.ShowInfoMsg($"\nDoğru kelime : {word}", ConsoleColor.DarkMagenta);
        Thread.Sleep(1200);
        break;
    }
    
}