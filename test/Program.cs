using System.Diagnostics;
using System.Reflection;

int NumberOfWords = 0;
using (var writer = new StreamWriter("../../../../NewWords.txt", true, System.Text.Encoding.Default))
{
    using (var reader = new StreamReader("../../../../RussianWords.txt", System.Text.Encoding.Default)) // конечный файл со словами и их повторениями
    {
        string? word;
        while ((word = await reader.ReadLineAsync()) != null)
        {
            Console.WriteLine(word);
            NumberOfWords++;
            if (NumberOfWords % 6 == 0) await writer.WriteLineAsync(word);
        }
    }
}
Console.WriteLine(NumberOfWords);
