namespace Yarkov.Tfs.Webhook.Storage;

public interface IFileStorage
{
  void Save(string fileName, string text, string? header = null);
}