using Telegram.Bot;
using Telegram.Bot.Types;

namespace LearnKazakh.Bot.Core;

public class TelegramHelper(long chatId, string token)
{
    private readonly TelegramBotClient _botClient = new TelegramBotClient(token);
    private readonly long _chatId = chatId;

    public async Task SendTextAsync(string text)
    {
        await _botClient.SendMessage(_chatId, text);
    }

    public async Task SendQuizPollAsync(string question, string[] answers, int correctOptionId)
    {
        List<InputPollOption> options = [.. answers.Select(a => new InputPollOption { Text = a })];

        await _botClient.SendPoll(_chatId, question, options, correctOptionId: correctOptionId, type: Telegram.Bot.Types.Enums.PollType.Quiz);
    }

    public async Task SendMediaAsync(byte[] data, string fileName)
    {
        InputFileStream fileStream = new InputFileStream(new MemoryStream(data), fileName);

        await _botClient.SendDocument(_chatId, fileStream, caption: $"[ {Path.GetFileNameWithoutExtension(fileName)} ] - {DateTime.UtcNow:dd.MM.yyyy}");
    }
}
