Dictionary<string, int> ModelsWordEndings = new();
char[] VowelLetters = {
        'а', 'е', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я'
};

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
    int wlen = word.Length;
    for (int i = 0; i < wlen; i++)
    {
        for (int j = 0; j < VowelLetters.Length; j++)
        {
            if (word[i] == VowelLetters[j])
            {
                flagCompliance = true;
                break;
            }
        }
        if (flagCompliance) break;
    }
    return (flagCompliance && (wlen >= 2));
}

string StemmStemka(string word)
{
    int max = 0;
    int wlen = word.Length;
    foreach (var model in ModelsWordEndings) // итерации по массиву окончаний
    {
        string wordTest = word;
        bool flagEqually = true;
        string modelword = model.Key;
        int mwlen = modelword.Length;
        if (mwlen < wlen)
        {
            flagEqually = true;
            for (int j = mwlen - 1; j >= 0; j--) // итерации по концу слова
                if (modelword[j] != word[wlen - mwlen + j])
                {
                    flagEqually = false;
                    break;
                }
            if (flagEqually && mwlen > max && ComplianceBasis(wordTest[..^(max - 2)]))
            {
                max = mwlen;
            }
        }
    }
    Console.WriteLine(max);
    Console.WriteLine(max - 2);
    return word[..^(max - 2)];
}