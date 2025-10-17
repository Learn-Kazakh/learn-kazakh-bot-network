using LearnKazakh.PdfBot.Models;
using LearnKazakh.Shared.DTOs;
using System.Net.Http.Json;

namespace LearnKazakh.PdfBot;

public class DataSource
{
    readonly HttpClient _httpClient;

    private readonly string _alphabetCategoryId = "941c0ef8-e6aa-4bb0-ac6b-f9941d6151f2";

    private readonly string _grammarCategoryId = "33941de7-7b48-479d-8990-53f3c15f9847";

    private readonly string _numbersCategoryId = "d8d87b51-781f-4b80-b549-b3719c770436";

    private readonly string _dailyLifeCategoryId = "4f5bf5e2-c4f7-40bf-b6a4-1f20cd0909ea";

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

    CategoryGroup CreateCategoryGroup(IGrouping<string, VocabularyDto> group)
    {
        return new CategoryGroup
        {
            Name = group.Key,
            Words = [.. group.Select(CreateWordEntry)]
        };
    }

    WordEntry CreateWordEntry(VocabularyDto dto)
    {
        return new WordEntry
        {
            Kazakh = dto.WordKazakh,
            Azerbaijani = dto.WordAzerbaijani,
            Pronunciation = dto.Pronounciation ?? "Tələffüz Təyin Edilməyib.",
            Example = CreateExampleSentence(dto)
        };
    }

    ExampleSentence? CreateExampleSentence(VocabularyDto dto)
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

    public async Task<List<Content>> LoadAlphabetAsync() => await LoadContentByCategoryAsync(_alphabetCategoryId);

    public async Task<List<Content>> LoadNumbersAsync() => await LoadContentByCategoryAsync(_numbersCategoryId);

    public async Task<List<Content>> LoadGrammarAsync() => await LoadContentByCategoryAsync(_grammarCategoryId);

    public async Task<List<Content>> LoadDailyLifeAsync() => await LoadContentByCategoryAsync(_dailyLifeCategoryId);

    async Task<List<Content>> LoadContentByCategoryAsync(string categoryId)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CategoryContentDto>>>($"content/by-category?categoryId={categoryId}");

        return response?.Data != null ? CreateCategory(response.Data) : [];
    }

    List<Content> CreateCategory(List<CategoryContentDto> data)
    {
        return [.. data.Select(cc => new Content
        {
            SectionTitle = cc.SectionTitle,
            Contents = cc.ContentTexts
        })];
    }
}
