using LearnKazakh.PdfBot.Configuration;
using LearnKazakh.PdfBot.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Composers;

public static class CommonComposer
{
    public static void ComposeNotSupportedYet(IContainer container, string featureName)
    {
        container
            .AlignCenter()
            .AlignMiddle()
            .Column(column =>
            {
                column.Spacing(10);

                column.Item()
                    .Text($"{featureName} hələ dəstəklənmir")
                    .FontSize(16)
                    .SemiBold()
                    .FontColor(AppColors.TextDark);

                column.Item()
                    .Text("Yaxın vaxtlarda əlavə ediləcək")
                    .FontColor(AppColors.Secondary);
            });
    }

    public static void ComposeContent(IContainer container, List<Content> contents, Action<IContainer> composeComments)
    {
        container.PaddingTop(10).Column(column =>
        {
            foreach (var content in contents)
            {
                column.Item().Element(c => ComposeInnerContent(c, content));
            }

            column.Item().PaddingTop(50).Element(composeComments);
        });
    }

    static void ComposeInnerContent(IContainer container, Content content)
    {
        container.Column(column =>
        {
            column.Item()
                  .Text(content.SectionTitle)
                  .FontSize(14)
                  .Bold()
                  .FontColor(AppColors.Primary);

            foreach (var content in content.Contents)
            {
                column.Item().PaddingVertical(10).Element(c => ComposeTexts(c, content));
            }
        });
    }

    static void ComposeTexts(IContainer container, string content)
    {
        container
           .Border(1)
           .BorderColor(Colors.Grey.Lighten2)
           .Background(Colors.Grey.Lighten4)
           .Padding(15)
           .Column(column =>
           {
               column.Item().Text(content);
           });
    }
}
