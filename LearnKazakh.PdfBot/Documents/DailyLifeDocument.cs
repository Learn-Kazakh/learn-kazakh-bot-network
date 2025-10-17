using LearnKazakh.PdfBot.Composers;
using LearnKazakh.PdfBot.Documents.Base;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Documents;

public class DailyLifeDocument : BaseDocument
{
    public override string DocumentTitle => "Gündəlik İfadələr";

    public override string DocumentDescription => "Hər gün istifadə olunan ifadələr";

    public override string FileName => "kazakh-daily-life.pdf";

    public override void ComposeContent(IContainer container)
    {
        CommonComposer.ComposeNotSupportedYet(container, "Gündəlik İfadələr");
    }
}
