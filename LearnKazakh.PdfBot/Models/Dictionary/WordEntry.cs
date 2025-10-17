namespace LearnKazakh.PdfBot.Models.Dictionary;

public class WordEntry
{
    public string Kazakh { get; set; } = string.Empty;
    public string Azerbaijani { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public ExampleSentence? Example { get; set; }
}
