using LearnKazakh.PdfBot;

internal class Program
{
    private static async Task Main(string[] args)
    {
        args = ["-d", "-g", "-n", "-a", "-dl"];
        //args = ["-a"];

        var generator = new PdfGenerator();

        var result = await generator.GenerateAsync(args);

        string outputDir = Path.Combine(Environment.CurrentDirectory, "output");

        Directory.CreateDirectory(outputDir);

        foreach (var item in result)
        {
            string filePath = Path.Combine(outputDir, item.Title);

            await File.WriteAllBytesAsync(filePath, item.Data);
        }
    }
}