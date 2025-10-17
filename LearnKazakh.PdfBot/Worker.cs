using LearnKazakh.Bot.Core;

namespace LearnKazakh.PdfBot;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly PdfGenerator _generator;
    private readonly TelegramHelper _telegramHelper;
    private readonly Dictionary<ScheduleModel, string[]> _scheduleToArgument = [];

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;

        string token = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN") ?? throw new Exception("Missing Telegram Token");
        _ = long.TryParse(Environment.GetEnvironmentVariable("TELEGRAM_CHAT_ID"), out long chatId);

        _generator = new PdfGenerator();
        _telegramHelper = new TelegramHelper(chatId, token);

        _scheduleToArgument.Add(new ScheduleModel
        {
            DayOfWeek = DayOfWeek.Monday,
            Hour = 9,
            Minute = 0,
        }, ["--dictionary=true"]);

        _scheduleToArgument.Add(new ScheduleModel
        {
            DayOfWeek = DayOfWeek.Wednesday,
            Hour = 9,
            Minute = 0,

        }, ["--alphabet=true"]);

        _scheduleToArgument.Add(new ScheduleModel
        {
            DayOfWeek = DayOfWeek.Friday,
            Hour = 9,
            Minute = 0,

        }, ["--grammar=true"]);

        _scheduleToArgument.Add(new ScheduleModel
        {
            DayOfWeek = DayOfWeek.Sunday,
            Hour = 10,
            Minute = 30,
        }, ["--daily-life=true", "--numbers=true"]);
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
