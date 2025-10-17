using DotNetEnv;
using LearnKazakh.PdfBot;

var values = Env.Load();

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Configuration.AddEnvironmentVariables();

var host = builder.Build();
host.Run();
