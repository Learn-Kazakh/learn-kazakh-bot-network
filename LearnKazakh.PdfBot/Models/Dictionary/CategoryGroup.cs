namespace LearnKazakh.PdfBot.Models.Dictionary;

public class CategoryGroup
{
    public string Name { get; set; } = string.Empty;
    public List<WordEntry> Words { get; set; } = [];
}
