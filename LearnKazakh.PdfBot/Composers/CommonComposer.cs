using LearnKazakh.PdfBot.Configuration;
using QuestPDF.Fluent;
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
}
