using LearnKazakh.PdfBot.Configuration;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Composers;

public static class FooterComposer
{
    public static void Compose(IContainer container, string url, DateTime? exportDate = null)
    {
        container.Column(column =>
        {
            column.Item()
                  .PaddingVertical(5)
                  .LineHorizontal(1)
                  .LineColor(AppColors.Border);

            column.Item().PaddingTop(5).Row(row =>
            {
                row.RelativeItem()
                   .Text(url)
                   .FontColor(AppColors.Primary);

                row.RelativeItem()
                   .AlignCenter()
                   .Text(text =>
                   {
                       text.Span("Səhifə ").FontColor(AppColors.Primary).Bold();
                       text.CurrentPageNumber().FontColor(AppColors.Primary).Bold();
                   });

                row.RelativeItem()
                   .AlignRight()
                   .Text((exportDate ?? DateTime.Now).ToString("dd.MM.yyyy"))
                   .FontColor(AppColors.Secondary);
            });
        });
    }
}
