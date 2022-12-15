using System.Diagnostics;

Dictionary<string, int> ModelsWordEndings = new();


string[] End = {
        "а", "я", "о", "е", "ь", "ы", "и", "ая", "яя", "ое", "ее", "ой", "ые", "ие", "ый", "йй",
        "ать", "ять", "оть", "еть", "уть", "у", "ю", "ем", "ете", "ите", "ет", "ют", "ят", "ал", "ял", "ала", "яла", "али", "яли", "ол",
        "ел", "ола", "ела", "оли", "ели", "ул", "ула", "ули", "ам", "ами", "ас", "aм", "ax", "ен", "ей", "еми", "емя", "ex", "ею", "ех", "ешь", "ий",
        "им", "ими", "ит", "их", "ишь", "ию", "м", "ми", "мя", "ов", "ого", "ойт", "ом", "ому", "ою", "cм", "ум", "умя", "ут", "ух", "ую", "шь", "ся", "сь"
};

char[] VowelLetters = {
        'а', 'е', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я'
};

string[] Suffixes = {
        "ерт", "ес", "ех", "ехоньк", "жды", "иад", "инг", "инь", "ион", "иот", "ир",
        "ирова", "исмент", "иссимус", "итет", "ича", "лан", "менн", "надцать", "нич", "ован",
        "овик", "овит", "овл", "озо", "ол", "теп", "тор", "ул", "фик", "фиц",
        "ци", "ческ", "чиц", "ья", "явк", "янн", "яр", "яшк", "а", "абельн",
        "ав", "аж", "аз", "айш", "ал", "альн", "ан", "ант", "ану", "ар",
        "арн", "арь", "аст", "яст", "ат", "атор", "аци", "ач", "ашн", "б",
        "бищ", "в", "ва", "вш", "вши", "гейт", "диил", "е", "ей", "ебн",
        "ев", "ова", "ева", "евич", "евн", "ее", "еж", "ез", "ейш", "ем",
        "ен", "ени", "енн", "енок", "ент", "енци", "ень", "еньк", "енько", "ер",
        "еск", "есс", "еств", "ец", "еч", "ечк", "и", "ив", "ива", "ивн",
        "ид", "иден", "изм", "ий", "ик", "ил", "илк", "им", "ин", "инк",
        "ист", "ит", "ительн", "их", "иц", "ич", "ическ", "ичк", "ичн", "ишк",
        "ишн", "ищ", "ка", "л", "лив", "н", "ни", "ник", "ниц", "нича",
        "нн", "ну", "няк", "о", "об", "ов", "оват", "ович", "овн", "оз",
        "озн", "оид", "ок", "ом", "он", "онк", "онн", "онок", "онько", "ор",
        "орн", "ость", "от", "оч", "очк", "очн", "очниц", "ош", "ск", "ств",
        "сть", "тель", "тельн", "ти", "ть", "у", "уг", "ум", "ун", "ур",
        "ух", "уч", "авши", "авшись", "яв", "явши", "явшись", "ивши", "ившись", "ыв",
        "ывши", "ывшись", "аем", "анн", "авш", "ающ", "яем", "явш", "яющ", "ящ",
        "ивш", "ывш", "ующ", "ала", "ана", "аете", "айте", "али", "ай", "ало",
        "ано", "ает", "ают", "аны", "ать", "аешь", "анно", "яла", "яна", "яете",
        "яйте", "яли", "яй", "ял", "ян", "яло", "яно", "яет", "яют", "яны",
        "ять", "яешь", "янно", "ила", "ыла", "ена", "ейте", "уйте", "ите", "или",
        "ыли", "уй", "ыл", "ым", "ило", "ыло", "ено", "ят", "ует", "уют",
        "ыт", "ены", "ить", "ыть", "ишь", "ую", "ю", "ие", "ье", "иями",
        "ями", "ами", "еи", "ии", "ией", "ой", "й", "иям", "ям", "ием",
        "ах", "иях", "ях", "ы", "ь", "ию", "ью", "ия", "я", "ейше",
        "ост", "ущ", "ющ", "ш", "т", "ши", "оньк", "чив", "еват", "евит",
        "ек", "чик", "щик", "ышк", "ушк"
};

var select1 = Suffixes
.Join(End,
s => 1,
e => 1,
(s, e) => new { s, e })
.ToArray();

string[] Endings = new string[Suffixes.Length + select1.Length + End.Length];
for (int i = 0; i < Suffixes.Length; i++)
    Endings[i] = Suffixes[i];
for (int i = 0; i < select1.Length; i++)
    Endings[Suffixes.Length + i] = select1[i].s + select1[i].e;
for (int i = 0; i < End.Length; i++)
    Endings[Suffixes.Length + select1.Length + i] = End[i];


Stopwatch time = Stopwatch.StartNew();
int NumberOfWords = 0;

using (var reader = new StreamReader("../../../../RussianWords.txt", System.Text.Encoding.Default)) // конечный файл со словами и их повторениями
{
    string? word;
    while ((word = await reader.ReadLineAsync()) != null)
    {
        NumberOfWords++;
        for (int i = 0; i < Endings.Length; i++)
        {
            string wordTest = word;
            if (DeleteEnding(ref wordTest, Endings[i]) & ComplianceBasis(wordTest))
            {
                AddModelToDictionary(word[(wordTest.Length - 2)..]);

            }
        }
    }
}
Console.WriteLine($"записей в словаре: {ModelsWordEndings.Count}");
time.Stop();
Console.WriteLine($"время обработки слов: {time.ElapsedMilliseconds / 1000} seconds");

time.Restart();
ModelsWordEndings = ModelsWordEndings
    .OrderByDescending(m => m.Value)
    .ToDictionary(m => m.Key, m => m.Value);
time.Stop();
Console.WriteLine($"время сортировки словаря: {time.ElapsedMilliseconds / 1000} seconds");


foreach (var mod in ModelsWordEndings)
    if ((mod.Value / 19117) < (1 / 10000)) ModelsWordEndings.Remove(mod.Key);


time.Restart();
using (var writer = new StreamWriter("../../../../ModelsWordEnding.txt", false, System.Text.Encoding.Default))
{
    foreach (var mod in ModelsWordEndings)
    {
        writer.WriteLine($"{mod.Key} {mod.Value}");
    }
}
time.Stop();
Console.WriteLine($"время записи словаря в файл: {time.ElapsedMilliseconds} ms");
Console.WriteLine($"записей в словаре: {ModelsWordEndings.Count}");



string s = "анансирующий";
Console.WriteLine(StemmStemka(ref s));




static bool DeleteEnding(ref string word, string ending) // удаление окончания слова
{
    bool flagEqually = true;
    int l = 0;
    if (ending.Length < word.Length)
    {
        for (int j = ending.Length - 1; j >= 0; j--) // итерации по концу слова
            if (ending[j] != word[word.Length - ending.Length + j])
            {
                flagEqually = false;
                break;
            }
    }
    else flagEqually = false;

    if (flagEqually)
    {
        l = ending.Length;
        word = word[..^l];
    }

    if (l > 0) return true; return false;
}

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

void AddModelToDictionary(string model)
{
    if (ModelsWordEndings.ContainsKey(model)) // если такое слово уже есть в словаре, то увеличиваем вес
    {
        ModelsWordEndings[model]++;
    }
    else // если нет, то добавляем
    {
        ModelsWordEndings.Add(model, 1);
    }
}

bool StemmStemka(ref string word)
{
    int max = 0;
    foreach (var mod in ModelsWordEndings) // итерации по массиву окончаний
    {
        string wordTest = word;
        bool flagEqually = true;
        if (mod.Key.Length < word.Length)
        {
            flagEqually = true;
            for (int j = mod.Key.Length - 1; j >= 0; j--) // итерации по концу слова
                if (mod.Key[j] != word[word.Length - mod.Key.Length + j])
                {
                    flagEqually = false;
                    break;
                }
            if (flagEqually & mod.Key.Length > max & ComplianceBasis(wordTest[..^max]))
            {
                max = mod.Key.Length;
            }
        }
    }
    word = word[..^max];
    Console.WriteLine($"max {max} | word {word}");

    if (max > 0) return true; return false;
}