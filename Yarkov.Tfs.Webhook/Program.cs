using Yarkov.Tfs.Webhook.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFileStorage, FileStorage>();
builder.Services.AddOptions<AppOptions>().Bind(builder.Configuration.GetSection("AppOptions"));
builder.Services.AddTransient<ILogger, Logger>();

var app = builder.Build();

app.MapPost("/timesheet", TimesheetController.Invoke);
app.MapPost("/ai", AiController.Invoke);

app.Run();