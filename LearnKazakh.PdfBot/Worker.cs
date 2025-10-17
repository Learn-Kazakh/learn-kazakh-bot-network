using LearnKazakh.Bot.Core;

namespace LearnKazakh.PdfBot;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly TelegramHelper _telegramHelper;
    private readonly PdfGenerator _generator = new();
    private readonly Dictionary<ScheduleModel, string[]> _scheduleToArgument = new()
    {
        // first one is intended just for test purposes.
        //[new ScheduleModel { DayOfWeek = DayOfWeek.Friday, Hour = 23, Minute = 17 }] = ["-d", "-g", "-n", "-a", "-dl"],

        [new ScheduleModel { DayOfWeek = DayOfWeek.Monday, Hour = 9, Minute = 0 }] = ["--dictionary"],
        [new ScheduleModel { DayOfWeek = DayOfWeek.Wednesday, Hour = 9, Minute = 0 }] = ["--alphabet"],
        [new ScheduleModel { DayOfWeek = DayOfWeek.Friday, Hour = 9, Minute = 0 }] = ["--grammar"],
        [new ScheduleModel { DayOfWeek = DayOfWeek.Sunday, Hour = 10, Minute = 30 }] = ["--daily-life", "--numbers"]
    };

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;

        string token = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN") ?? throw new Exception("Missing Telegram Token");

        _ = long.TryParse(Environment.GetEnvironmentVariable("TELEGRAM_CHAT_ID"), out long chatId);

        _telegramHelper = new TelegramHelper(chatId, token);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                foreach (var schedule in _scheduleToArgument)
                {
                    ScheduleModel scheduleModel = schedule.Key;

                    if (ScheduleHelper.IsNow(scheduleModel))
                    {
                        if (scheduleModel.LastExecuted == null || Math.Abs(DateTime.UtcNow.Minute - scheduleModel.Minute) > 1)
                        {
                            _logger.LogInformation("Generating PDF for {day} {time}", scheduleModel.DayOfWeek, DateTime.Now.ToString("HH:mm"));

                            var result = await _generator.GenerateAsync(schedule.Value);

                            foreach (var pdf in result)
                            {
                                await _telegramHelper.SendMediaAsync(pdf.Data, pdf.Title);
                            }

                            scheduleModel.LastExecuted = DateTime.Now;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating scheduled PDFs");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
