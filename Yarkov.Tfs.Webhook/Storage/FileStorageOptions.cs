namespace Yarkov.Tfs.Webhook.Storage
{
  public class FileStorageOptions
  {
    public required string WorkDirectoryName { get; set; }
    public required string LogFileName { get; set; }
  }
}