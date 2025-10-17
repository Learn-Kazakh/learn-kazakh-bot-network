namespace LearnKazakh.PdfBot.Models;

public class CategoryGroup
{
    public string Name { get; set; } = string.Empty;
    public List<WordEntry> Words { get; set; } = [];
}
