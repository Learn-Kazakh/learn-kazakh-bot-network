using LearnKazakh.PdfBot.Configuration;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Composers;

public class HeaderComposer
{
    public static void Compose(IContainer container, string documentTitle, string documentDescription)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem()
                   .Text(documentTitle)
                   .FontSize(18)
                   .Bold()
                   .FontColor(AppColors.Primary);

                row.RelativeItem()
                   .AlignBottom()
                   .AlignRight()
                   .Text(documentDescription)
                   .FontColor(AppColors.Secondary);
            });

            column.Item()
                  .PaddingVertical(5)
                  .LineHorizontal(1)
                  .LineColor(AppColors.Border);
        });
    }
}
