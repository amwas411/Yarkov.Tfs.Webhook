using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Yarkov.Tfs.Webhook.Models;
using Yarkov.Tfs.Webhook.Storage;

namespace Yarkov.Tfs.Webhook.Test;

public static class TestConstants
{
  public static string LogFileName = "log.txt";
  public static string TerminalRunner = "E:/Programs SSD/Git/bin/bash.exe";
  public static string WorkDirectoryName = "data";
  public static string AiWorkDirectoryName = "aidata";
  public static string TimesheetFileName = "timesheet.csv";
  public static bool AiDiagnosticMode = true;
  public static string AiName = "$ревью";
}

[TestClass]
public sealed class Test1
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
  private static ServiceProvider ServiceProvider;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

  [AssemblyInitialize]
  public static void Initialize(TestContext ctx)
  {
    var c = new ServiceCollection();
    c.AddSingleton<IFileStorage, FileStorage>();
    c.AddTransient<ILogger, Logger>();
    c.AddOptions<AppOptions>().Configure((appOptions) =>
    {
      appOptions.LogFileName = TestConstants.LogFileName;
      appOptions.TerminalRunner = TestConstants.TerminalRunner;
      appOptions.WorkDirectoryName = TestConstants.WorkDirectoryName;
      appOptions.AiWorkDirectoryName = TestConstants.AiWorkDirectoryName;
      appOptions.TimesheetFileName = TestConstants.TimesheetFileName;
      appOptions.AiDiagnosticMode = TestConstants.AiDiagnosticMode;
      appOptions.AiName = TestConstants.AiName;
    });
    ServiceProvider = c.BuildServiceProvider();
  }

  [TestInitialize]
  public void TestInitialize()
  {
    FileStorage.Storage.Clear();
  }

  [TestMethod]
  [DataRow("sample1.json")]
  [DataRow("sample2.json")]
  [DataRow("sample3.json")]
  [DataRow("sample4.json")]
  [DataRow("sample5.json")]
  public void TimesheetControllerTest(string sample)
  {
    var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples", sample);
    
    var file = new FileInfo(fileName);
    if (file == null)
    {
      throw new FileNotFoundException("Test file not found", fileName);
    }

    string content = string.Empty;
    if (file.Exists)
    {
      using var rfs = file.OpenRead();
      var readBuffer = new byte[file.Length];
      rfs.ReadAsync(readBuffer, 0, readBuffer.Length).GetAwaiter().GetResult();
      content = Encoding.UTF8.GetString(readBuffer);
    }
    Assert.IsNotEmpty(content);
    var response = JsonSerializer.Deserialize<TfsResponse>(content, new JsonSerializerOptions()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      
    });
    Assert.IsNotNull(response);
    TimesheetController.Invoke(ServiceProvider.GetService<IFileStorage>(), ServiceProvider.GetService<ILogger>(), ServiceProvider.GetService<IOptions<AppOptions>>(), response);
    Assert.Contains(TestConstants.TimesheetFileName, FileStorage.Storage.Keys);
    Assert.AreEqual(Constants.TimesheetController.CsvHeader, FileStorage.Storage[TestConstants.TimesheetFileName][0]);
    switch (sample)
    {
      case "sample1.json":
        Assert.AreEqual("2026.09.09 19:08:59,\"Реализовать сервис получения погоды\",0.75,https://tfs.company.ru/web/wi.aspx?pcguid=70c2d8cc-553f-4819-8ba2-e56afc3bf040&id=358448,company\\User,MyProject", FileStorage.Storage[TestConstants.TimesheetFileName][1]);
        break;
      case "sample2.json":
        Assert.AreEqual("2026.09.09 19:08:59,\"Реализовать сервис получения погоды\",-0.7,https://tfs.company.ru/web/wi.aspx?pcguid=70c2d8cc-553f-4819-8ba2-e56afc3bf040&id=358448,company\\User,MyProject", FileStorage.Storage[TestConstants.TimesheetFileName][1]);
        break;
      case "sample3.json":
        Assert.AreEqual("2026.09.09 19:08:59,\"Реализовать сервис получения погоды\",3,https://tfs.company.ru/web/wi.aspx?pcguid=70c2d8cc-553f-4819-8ba2-e56afc3bf040&id=358448,company\\User,MyProject", FileStorage.Storage[TestConstants.TimesheetFileName][1]);
        break;
      case "sample4.json":
        Assert.AreEqual("2026.09.09 19:08:59,\"Реализовать сервис получения погоды\",-3,https://tfs.company.ru/web/wi.aspx?pcguid=70c2d8cc-553f-4819-8ba2-e56afc3bf040&id=358448,company\\User,MyProject", FileStorage.Storage[TestConstants.TimesheetFileName][1]);
        break;
      case "sample5.json":
        Assert.AreEqual("2026.09.09 19:08:59,\"Реализовать сервис получения погоды написать тесты\",-3,https://tfs.company.ru/web/wi.aspx?pcguid=70c2d8cc-553f-4819-8ba2-e56afc3bf040&id=358448,company\\User,MyProject", FileStorage.Storage[TestConstants.TimesheetFileName][1]);
        break;
      default:
        break;
    }
    Assert.IsFalse(FileStorage.Storage.ContainsKey(TestConstants.LogFileName));
  }

  [TestMethod]
  [DataRow("comment_sample1.json")]
  [DataRow("comment_sample2.json")]
  public void AiControllerTest(string sample)
  {
    var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples", sample);
    
    var file = new FileInfo(fileName);
    if (file == null)
    {
      throw new FileNotFoundException("Test file not found", fileName);
    }

    string content = string.Empty;
    if (file.Exists)
    {
      using var rfs = file.OpenRead();
      var readBuffer = new byte[file.Length];
      rfs.ReadAsync(readBuffer, 0, readBuffer.Length).GetAwaiter().GetResult();
      content = Encoding.UTF8.GetString(readBuffer);
    }
    Assert.IsNotEmpty(content);
    var response = JsonSerializer.Deserialize<TfsResponseComment>(content, new JsonSerializerOptions()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });

    Assert.IsNotNull(response);
    AiController.Invoke(ServiceProvider.GetService<IFileStorage>(), ServiceProvider.GetService<ILogger>(), ServiceProvider.GetService<IOptions<AppOptions>>(), response);
    var result = FileStorage.Storage[TestConstants.LogFileName][0];
    switch (sample)
    {
      case "comment_sample1.json":
        Assert.AreEqual("2", result);
        break;
      default:
        break;
    }
  }
}

class FileStorage : IFileStorage
{
  public static Dictionary<string, List<string>> Storage = [];
  public object Lock = new object();
  public void Save(string fileName, string text, string? header = null)
  {
    lock (Lock)
    {
      if (!Storage.ContainsKey(fileName))
      {
        Storage.Add(fileName, []);
      }

      if (!string.IsNullOrEmpty(header))
      {
        Storage[fileName].Add(header);
      }
      Storage[fileName].Add(text);
    }
  }
}

class Logger : ILogger
{
  private IFileStorage _storage;
  public Logger(IFileStorage storage)
  {
    ArgumentNullException.ThrowIfNull(storage);
    _storage = storage;
  }

  public void Log(string text, string level, string method)
  {
    _storage.Save(TestConstants.LogFileName, $"[{level}] [{method}] {text}");
  }
}