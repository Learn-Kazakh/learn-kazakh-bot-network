using LearnKazakh.PdfBot.Configuration;
using LearnKazakh.PdfBot.Documents.Base;
using LearnKazakh.PdfBot.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot.Documents;

public class DictionaryDocument(DictionaryModel model) : BaseDocument
{
    private readonly DictionaryModel _model = model;

    public override string DocumentTitle => "Qazax Dili Lüğəti";

    public override string DocumentDescription => $"Cəmi: {_model.TotalWords:N0} söz";

    public override string FileName => "kazakh-dictionary.pdf";

    public override void ComposeContent(IContainer container)
    {
        container.PaddingTop(10).Column(column =>
        {
            foreach (var category in _model.Categories)
            {
                column.Item().Element(c => ComposeCategory(c, category));
            }

            column.Item().PaddingTop(50).Element(ComposeComments);
        });
    }

    private void ComposeCategory(IContainer container, CategoryGroup category)
    {
        container.Column(column =>
        {
            column.Item()
                  .PaddingTop(15)
                  .PaddingBottom(15)
                  .Text(category.Name)
                  .FontSize(14)
                  .Bold()
                  .FontColor(AppColors.Primary);

            column.Item().Element(c => ComposeCategoryTable(c, category));
        });
    }

    private void ComposeCategoryTable(IContainer container, CategoryGroup category)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(30);
                columns.RelativeColumn(2);
                columns.RelativeColumn(2);
                columns.RelativeColumn(2);
                columns.RelativeColumn(3);
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Qazax");
                header.Cell().Element(CellStyle).Text("Azərbaycan");
                header.Cell().Element(CellStyle).Text("Tələffüz");
                header.Cell().Element(CellStyle).Text("Nümunə Cümlə");

                static IContainer CellStyle(IContainer container)
                {
                    return container.Background(Colors.Grey.Lighten4)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .BorderBottom(1)
                                    .Padding(5);
                }
            });

            int index = 1;
            foreach (var item in category.Words)
            {
                table.Cell().Element(CellStyle).Text($"{index++}");
                table.Cell().Element(CellStyle).Text(item.Kazakh);
                table.Cell().Element(CellStyle).Text(item.Azerbaijani);
                table.Cell().Element(CellStyle).Text(item.Pronunciation);
                table.Cell().Element(CellStyle).Text(text =>
                {
                    if (item.Example != null)
                    {
                        text.Line(item.Example.Kazakh);
                        text.Line(item.Example.Azerbaijani);
                    }
                });

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(0.5f)
                                    .BorderColor(Colors.Grey.Lighten4)
                                    .Padding(5);
                }
            }
        });
    }
}
