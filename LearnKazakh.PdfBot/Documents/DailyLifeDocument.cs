using LearnKazakh.PdfBot.Composers;
using LearnKazakh.PdfBot.Documents.Base;
using LearnKazakh.PdfBot.Models;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Documents;

public class DailyLifeDocument(List<Content> contents) : BaseDocument
{
    private readonly List<Content> _contents = contents;

    public override string DocumentTitle => "Gündəlik İfadələr";

    public override string DocumentDescription => "Hər gün istifadə olunan ifadələr";

    public override string FileName => "kazakh-daily-life.pdf";

    public override void ComposeContent(IContainer container)
    {
        CommonComposer.ComposeContent(container, _contents, ComposeComments);
    }
}
