using LearnKazakh.PdfBot.Composers;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace LearnKazakh.PdfBot.Documents.Base;

public abstract class BaseDocument : IDocument
{
    public abstract string DocumentTitle { get; }
    public abstract string DocumentDescription { get; }
    public abstract string FileName { get; }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(1, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(10));

            page.Header().Element(c => ComposeHeader(c));
            page.Content().Element(ComposeContent);
            page.Footer().Element(c => ComposeFooter(c));
        });
    }

    private void ComposeHeader(IContainer container)
    {
        HeaderComposer.Compose(container, DocumentTitle, DocumentDescription);
    }

    public abstract void ComposeContent(IContainer container);

    private void ComposeFooter(IContainer container)
    {
        FooterComposer.Compose(container, GetFooterUrl());
    }

    public virtual string GetFooterUrl()
    {
        return "https://learnkz.com";
    }

    public virtual void ComposeComments(IContainer container)
    {
        container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Background(Colors.Grey.Lighten4)
            .Padding(15)
            .Column(column =>
            {
                column.Spacing(6);

                column.Item()
                    .Text("Qeydlər və Məlumat")
                    .FontSize(13)
                    .SemiBold()
                    .FontColor(Colors.Grey.Darken3);

                column.Item()
                    .Text(text =>
                    {
                        text.Span("Bütün məlumatlar ")
                            .FontColor(Colors.Grey.Darken3);

                        text.Span("Learn Kazakh ")
                            .SemiBold()
                            .FontColor(Colors.Blue.Medium);

                        text.Span("platformasındakı dərslərdən avtomatik olaraq yığılmışdır. PDF formatı oflayn öyrənmə və çap üçün nəzərdə tutulub.")
                            .FontColor(Colors.Grey.Darken3);
                    });

                column.Item()
                      .PaddingVertical(5)
                      .LineHorizontal(1)
                      .LineColor(Colors.Grey.Lighten2);

                column.Item()
                       .Text(text =>
                       {
                           text.Span("Tərtib edən: ").SemiBold().FontColor(Colors.Grey.Darken3);
                           text.Span("Mahammad Ahmadov").FontColor(Colors.Grey.Darken3);
                       });

                column.Item()
                    .Text(text =>
                    {
                        text.Span("Veb sayt: ").SemiBold().FontColor(Colors.Grey.Darken3);
                        text.Hyperlink("https://learnkz.com", "https://learnkz.com")
                            .FontColor(Colors.Blue.Medium);
                    });

                column.Item()
                    .Text(text =>
                    {
                        text.Span("Əlaqə: ").SemiBold().FontColor(Colors.Grey.Darken3);
                        text.Hyperlink("mailto:dev.ahmadov.mahammad@gmail.com", "dev.ahmadov.mahammad@gmail.com")
                            .FontColor(Colors.Blue.Medium);
                    });

                column.Item()
                      .Text(text =>
                      {
                          text.Span("Export tarixi: ").SemiBold().FontColor(Colors.Grey.Darken3);
                          text.Span($"{DateTime.Now:dd.MM.yyyy}").FontColor(Colors.Grey.Darken3);
                      });
            });
    }
}
