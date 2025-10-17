using LearnKazakh.PdfBot.Documents;
using LearnKazakh.PdfBot.Documents.Base;
using LearnKazakh.PdfBot.Models;
using LearnKazakh.PdfBot.Models.Dictionary;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace LearnKazakh.PdfBot;

public class PdfGenerator
{
    private readonly DataSource _dataSource = new DataSource();
    private readonly AppParameter _appParameter = new AppParameter();

    public async Task<List<PdfResponse>> GenerateAsync(string[] args)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        ParseArguments(args);

        List<PdfResponse> response = new List<PdfResponse>();

        foreach (var document in await GetDocumentsAsync())
        {
            byte[] bytes = Document.Create(document.Compose).GeneratePdf();

            response.Add(new PdfResponse
            {
                Title = document.FileName,
                Data = bytes,
            });
        }

        return response;
    }

    async Task<List<BaseDocument>> GetDocumentsAsync()
    {
        List<BaseDocument> documents = [];

        if (_appParameter.GenerateDictionary)
        {
            DictionaryModel model = await _dataSource.LoadDictionaryAsync();
            documents.Add(new DictionaryDocument(model));
        }

        if (_appParameter.GenerateAlphabet)
        {
            documents.Add(new AlphabetDocument());
        }

        if (_appParameter.GenerateDailyLife)
        {
            documents.Add(new DailyLifeDocument());
        }

        if (_appParameter.GenerateGrammar)
        {
            documents.Add(new GrammarDocument());
        }

        if (_appParameter.GenerateNumbers)
        {
            documents.Add(new NumbersDocument());
        }

        return documents;
    }

    void ParseArguments(string[] args)
    {
        Dictionary<List<string>, Action<bool>> flagActions = new()
        {
            { [ "--dictionary", "-d" ], (value) => _appParameter.GenerateDictionary = value },
            { [ "--grammar", "-g" ], (value) => _appParameter.GenerateGrammar = value },
            { [ "--numbers", "-n" ], (value) => _appParameter.GenerateNumbers = value },
            { [ "--alphabet", "-a" ], (value) => _appParameter.GenerateAlphabet = value },
            { [ "--daily-life", "-dl" ], (value) => _appParameter.GenerateDailyLife = value },
        };

        foreach (string arg in args)
        {
            if (!arg.StartsWith('-'))
            {
                continue;
            }

            string[] parts = arg.Split('=', 2);

            string key = parts[0].ToLowerInvariant();

            foreach (var flag in flagActions)
            {
                if (flag.Key.Contains(key))
                {
                    bool value = true; // default value is true if no ( = value ) part is provided.

                    if (parts.Length == 2)
                    {
                        _ = bool.TryParse(parts[1], out value);
                    }

                    flag.Value.Invoke(value);
                    break;
                }
            }
        }
    }
}
