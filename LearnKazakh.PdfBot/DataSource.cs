using LearnKazakh.PdfBot.Models.Dictionary;
using LearnKazakh.Shared.DTOs;
using System.Net.Http.Json;

namespace LearnKazakh.PdfBot;

public class DataSource
{
    readonly HttpClient _httpClient;

    public DataSource()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("https://learnkz.com/api/") };
    }

    public async Task<DictionaryModel> LoadDictionaryAsync()
    {
        var vocabularyDtos = await FetchAllVocabularyAsync();

        return new DictionaryModel
        {
            ExportedDate = DateTime.UtcNow,
            TotalWords = vocabularyDtos.Count,
            Categories = [.. vocabularyDtos.GroupBy(dto => dto.Type).Select(CreateCategoryGroup)]
        };
    }

    async Task<List<VocabularyDto>> FetchAllVocabularyAsync()
    {
        var vocabularyDtos = new List<VocabularyDto>();
        var offset = 0;

        ApiPagedResponse<VocabularyDto>? response;

        do
        {
            response = await _httpClient.GetFromJsonAsync<ApiPagedResponse<VocabularyDto>>($"vocabulary?offset={offset}");

            if (response?.Data is { HasMore: true } pagedData)
            {
                vocabularyDtos.AddRange(pagedData.Items);
                offset = pagedData.NextOffset;
            }
            else
            {
                break;
            }
        }
        while (response?.Data?.HasMore == true);

        return vocabularyDtos;
    }

    static CategoryGroup CreateCategoryGroup(IGrouping<string, VocabularyDto> group)
    {
        return new CategoryGroup
        {
            Name = group.Key,
            Words = [.. group.Select(CreateWordEntry)]
        };
    }

    static WordEntry CreateWordEntry(VocabularyDto dto)
    {
        return new WordEntry
        {
            Kazakh = dto.WordKazakh,
            Azerbaijani = dto.WordAzerbaijani,
            Pronunciation = dto.Pronounciation ?? "Tələffüz Təyin Edilməyib.",
            Example = CreateExampleSentence(dto)
        };
    }

    static ExampleSentence? CreateExampleSentence(VocabularyDto dto)
    {
        if (dto.Examples == null || dto.Examples.Count == 0)
        {
            return null;
        }

        var firstExample = dto.Examples[0];
        return new ExampleSentence
        {
            Kazakh = firstExample.SentenceKazakh,
            Azerbaijani = firstExample.SentenceTranslation
        };
    }
}
