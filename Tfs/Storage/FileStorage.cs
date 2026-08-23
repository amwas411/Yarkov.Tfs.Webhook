using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;

namespace Yarkov.Tfs.Storage;

public class FileStorage
{
  private DirectoryInfo _directory;
  private string _logFileName;
  private object _lock = new object();

  public FileStorage(IOptions<FileStorageOptions> options)
  {
    ArgumentException.ThrowIfNullOrEmpty(options.Value.DirectoryName);
    ArgumentException.ThrowIfNullOrEmpty(options.Value.LogFileName);
    
    _directory = Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, options.Value.DirectoryName));
    _logFileName = options.Value.LogFileName;
  }

  public void SaveToFile(string fileName, string text, string? header = null)
  {
    lock (_lock)
    {
      _directory.Refresh();
      if (!_directory.Exists)
      {
        _directory.Create();
      }

      var file = new FileInfo(Path.Combine(_directory.FullName, fileName));
      var stringStorage = new StringBuilder();
      if (file.Exists)
      {
				using var rfs = file.OpenRead();
				var readBuffer = new byte[file.Length];
				rfs.ReadAsync(readBuffer, 0, readBuffer.Length).GetAwaiter().GetResult();
				stringStorage.Append(Encoding.UTF8.GetString(readBuffer));
			}
      
      if (!string.IsNullOrEmpty(header) && (!file.Exists || file.Length == 0))
			{
        stringStorage.AppendLine(header);
			}
      
      stringStorage.AppendLine(text);

      using var wfs = File.OpenWrite(file.FullName);
      var writeBuffer = Encoding.UTF8.GetBytes(stringStorage.ToString());
      wfs.WriteAsync(writeBuffer, 0, writeBuffer.Length).GetAwaiter().GetResult();
    }
  }

  public async Task Log(string text, string level, string method)
	{
    var message = 
$"""
[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] [{level}] [{method}] {text}
""";
		SaveToFile(_logFileName, message);
	}
}