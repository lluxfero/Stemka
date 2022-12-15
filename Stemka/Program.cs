Dictionary<string, int> ModelsWordEndings = new();

using (var reader = new StreamReader("../../../../ModelsWordEnding.txt", System.Text.Encoding.Default))
{
    string? line;
    string[] w;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        w = line.ToLower().Replace("ё", "е").Split(" ");
        if (Convert.ToInt32(w[1]) == 300) break;
        try
        {
            ModelsWordEndings.Add(w[0], Convert.ToInt32(w[1]));
        }
        catch(ArgumentException)
        {
            ModelsWordEndings[w[0]]++;
        }
    }
}

string s = "протоколировать";
StemmStemka(s);
Console.WriteLine(StemmStemka(s));

bool ComplianceBasis(string word) // основа >= 2 и есть хотя бы одна гласная
{
    bool flagCompliance = false;
    for (int i = 0; i < word.Length; i++)
    {
        for (int j = 0; j < word.Length; j++)
        {
            if (word[i] == word[j])
            {
                flagCompliance = true;
                break;
            }
        }
        if (flagCompliance) break;
    }
    if (flagCompliance & word.Length >= 2) return true; return false;
}

string StemmStemka(string word)
{
    int max = 0;
    foreach (var model in ModelsWordEndings) // итерации по массиву окончаний
    {
        string wordTest = word;
        bool flagEqually = true;
        if (model.Key.Length < word.Length)
        {
            flagEqually = true;
            for (int j = model.Key.Length - 1; j >= 0; j--) // итерации по концу слова
                if (model.Key[j] != word[word.Length - model.Key.Length + j])
                {
                    flagEqually = false;
                    break;
                }
            if (flagEqually & model.Key.Length > max & ComplianceBasis(wordTest[..^max]))
            {
                max = model.Key.Length;
            }
        }
    }
    return word[..^max];
}