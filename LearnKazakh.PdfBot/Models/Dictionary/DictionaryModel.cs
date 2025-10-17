namespace LearnKazakh.PdfBot.Models.Dictionary;

public class DictionaryModel
{
    public List<CategoryGroup> Categories { get; set; } = [];
    public DateTime ExportedDate { get; set; } = DateTime.UtcNow;
    public int TotalWords { get; set; }
}
