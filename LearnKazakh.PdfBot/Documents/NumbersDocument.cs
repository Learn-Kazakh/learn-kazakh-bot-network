using LearnKazakh.PdfBot.Composers;
using LearnKazakh.PdfBot.Documents.Base;
using LearnKazakh.PdfBot.Models;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Documents;

public class NumbersDocument(List<Content> contents) : BaseDocument
{
    private readonly List<Content> _contents = contents;

    public override string DocumentTitle => "Rəqəmlər və Saylar";

    public override string DocumentDescription => "1-dən 1000-ə qədər";

    public override string FileName => "kazakh-numbers.pdf";

    public override void ComposeContent(IContainer container)
    {
        CommonComposer.ComposeContent(container, _contents, ComposeComments);
    }
}
