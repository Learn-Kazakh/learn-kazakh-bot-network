using LearnKazakh.PdfBot.Composers;
using LearnKazakh.PdfBot.Configuration;
using LearnKazakh.PdfBot.Documents.Base;
using LearnKazakh.PdfBot.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Documents;

public class AlphabetDocument(List<Content> contents) : BaseDocument
{
    private readonly List<Content> _contents = contents;

    public override string DocumentTitle => "Qazax Əlifbası";

    public override string DocumentDescription => $"{_contents.Sum(c => c.Contents.Count)} hərf";

    public override string FileName => "kazakh-alphabet.pdf";

    public override void ComposeContent(IContainer container)
    {
        CommonComposer.ComposeContent(container, _contents, ComposeComments);
    }
}
