using LearnKazakh.PdfBot.Composers;
using LearnKazakh.PdfBot.Documents.Base;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Documents;

public class AlphabetDocument : BaseDocument
{
    public override string DocumentTitle => "Qazax Əlifbası";

    public override string DocumentDescription => "33 hərf";

    public override string FileName => "kazakh-alphabet.pdf";

    public override void ComposeContent(IContainer container)
    {
        CommonComposer.ComposeNotSupportedYet(container, "Əlifba");
    }
}
