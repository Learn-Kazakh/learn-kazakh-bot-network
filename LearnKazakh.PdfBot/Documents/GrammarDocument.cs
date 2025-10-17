using LearnKazakh.PdfBot.Composers;
using LearnKazakh.PdfBot.Documents.Base;
using LearnKazakh.PdfBot.Models;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Documents;

public class GrammarDocument(List<Content> contents) : BaseDocument
{
    private readonly List<Content> _contents = contents;

    public override string DocumentTitle => "Qrammatika Qaydaları";

    public override string DocumentDescription => "Əsas qrammatika mövzuları";

    public override string FileName => "kazakh-grammar.pdf"; 
    
    public override void ComposeContent(IContainer container)
    {
        CommonComposer.ComposeContent(container, _contents, ComposeComments);
    }
}
