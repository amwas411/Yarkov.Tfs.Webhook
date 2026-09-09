using Microsoft.Extensions.Options;
using Yarkov.Tfs.Webhook.Storage;

class Logger: ILogger
{
  private IFileStorage _storage;
  private string _logFileName;

  public Logger(IFileStorage fileStorage, IOptions<FileStorageOptions> options)
  {
    ArgumentNullException.ThrowIfNull(fileStorage);
    ArgumentNullException.ThrowIfNull(options);
    _storage = fileStorage;
    _logFileName = options.Value.LogFileName;
  }

  public void Log(string text, string level, string method)
	{
    var message = 
$"""
[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] [{level}] [{method}] {text}
""";
		_storage.Save(_logFileName, message);
	}
}

public interface ILogger
{
  void Log(string text, string level, string method);
}