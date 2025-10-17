using LearnKazakh.PdfBot.Composers;
using LearnKazakh.PdfBot.Documents.Base;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Documents;

public class NumbersDocument : BaseDocument
{
    public override string DocumentTitle => "Rəqəmlər və Saylar";

    public override string DocumentDescription => "1-dən 1000-ə qədər";

    public override string FileName => "kazakh-numbers.pdf";

    public override void ComposeContent(IContainer container)
    {
        CommonComposer.ComposeNotSupportedYet(container, "Rəqəmlər");
    }
}
