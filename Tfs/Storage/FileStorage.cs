using System.Text;
using Microsoft.Extensions.Options;

namespace Yarkov.Tfs.Storage;

public class FileStorage
{
  private StringBuilder _stringStorage;
  private DirectoryInfo _directory;
  private FileInfo _file;
  private object _lock = new object();

  public FileStorage(IOptions<FileStorageOptions> options)
  {
    ArgumentException.ThrowIfNullOrEmpty(options.Value.DirectoryName);
    ArgumentException.ThrowIfNullOrEmpty(options.Value.FileName);
    _directory = Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, options.Value.DirectoryName));
    _file = new FileInfo(Path.Combine(_directory.FullName, options.Value.FileName));
    _stringStorage = new StringBuilder();
    if (_file.Exists)
    {
      using var fs = _file.OpenRead();
      var buffer = new byte[_file.Length];
      fs.ReadAsync(buffer, 0, buffer.Length).GetAwaiter().GetResult();
      _stringStorage.Append(Encoding.UTF8.GetString(buffer));
    }
  }

  public void SaveToFile(string text)
  {
    lock (_lock)
    {
      _directory.Refresh();
      if (!_directory.Exists)
      {
        _directory.Create();
      }

      _file.Refresh();
      if (!_file.Exists)
      {
        _stringStorage.Clear();
      }
      
      _stringStorage.AppendLine(text);

      using var fs = File.OpenWrite(_file.FullName);
      var buffer = Encoding.UTF8.GetBytes(_stringStorage.ToString());
      fs.WriteAsync(buffer, 0, buffer.Length).GetAwaiter().GetResult();
    }
  }
}