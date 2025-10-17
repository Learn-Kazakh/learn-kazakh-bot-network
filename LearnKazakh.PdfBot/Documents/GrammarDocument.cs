using LearnKazakh.PdfBot.Composers;
using LearnKazakh.PdfBot.Documents.Base;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Documents;

public class GrammarDocument : BaseDocument
{
    public override string DocumentTitle => "Qrammatika Qaydaları";

    public override string DocumentDescription => "Əsas qrammatika mövzuları";

    public override string FileName => "kazakh-grammar.pdf"; 
    
    public override void ComposeContent(IContainer container)
    {
        CommonComposer.ComposeNotSupportedYet(container, "Qrammatika");
    }
}
