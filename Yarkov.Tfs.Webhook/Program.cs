using Yarkov.Tfs.Webhook.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFileStorage, FileStorage>();
builder.Services.AddOptions<FileStorageOptions>().Bind(builder.Configuration.GetSection("FileStorageOptions"));
builder.Services.AddTransient<ILogger, Logger>();

var app = builder.Build();

app.MapPost("/timesheet", TimesheetController.Invoke);

app.Run();