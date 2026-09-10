using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Options;
using Yarkov.Tfs.Webhook.Exceptions;
using Yarkov.Tfs.Webhook.Models;
using Yarkov.Tfs.Webhook.Storage;

public class AiController
{
  public static async void Invoke(IFileStorage storage, ILogger logger, IOptions<AppOptions> options, TfsResponseComment response)
  {
    try
    {
      #region Validation
      if (response.Resource.Fields == null)
      {
        throw new YarkovException($"\"Response.Resource.Fields\" is null.");
      }
      if (!response.Resource.Fields.ContainsKey(Constants.FieldNames.HistoryFieldName) || 
        string.IsNullOrEmpty(response.Resource.Fields[Constants.FieldNames.HistoryFieldName].ToString()))
      {
        throw new YarkovException($"Could not get \"History\" from a request: \"Response.Resource.Fields.{Constants.FieldNames.HistoryFieldName}\" is empty or nonexistent.");
      }
      if (response.Resource._links == null)
      {
        throw new YarkovException($"\"Response.Resource._links\" is null.");
      }
      if (!response.Resource.Fields[Constants.FieldNames.HistoryFieldName].ToString().ToLowerInvariant().Contains(options.Value.AiName))
      {
        throw new YarkovException($"Message does not contain AI name \"{options.Value.AiName}\".");
      }

      #endregion

      var pat = Environment.GetEnvironmentVariable(Constants.AiController.TfsPat);
      if (string.IsNullOrEmpty(pat))
      {
        throw new YarkovEmptyEnvironmentVariableException(Constants.AiController.TfsPat);
      }
      var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, options.Value.AiWorkDirectoryName).Replace("\\", "/");
      var directory = new DirectoryInfo(directoryPath);
      if (!directory.Exists)
      {
        directory.Create();
      }

      var runner = options.Value.TerminalRunner;
      var runnerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "codex.sh").Replace("\\", "/");
      var formatArgument = "-c \"{0}\"";
      var diagnostic = options.Value.AiDiagnosticMode ? "--diag" : "";
      var prompt = $"{runnerPath} -w \"{response.Resource._links["html"].Href}\" --cd {directoryPath} --review {diagnostic}";
      var arguments = string.Format(formatArgument, prompt);
      
      var info = new ProcessStartInfo()
      {
        FileName = runner,
        Arguments = arguments,
        CreateNoWindow = true,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
      };

      var process = Process.Start(info) ?? throw new YarkovProcessNotStartedException(info);

      var outputReader = process.StandardOutput;
      var errorReader = process.StandardError;

      var error = await errorReader.ReadToEndAsync() ?? "";
      var output = await outputReader.ReadToEndAsync() ?? "";
      var stringBuilder = new StringBuilder();
      stringBuilder.AppendLine($"{DateTime.Now:yyyy.MM.dd HH:mm:ss}");
      stringBuilder.AppendLine(error);
      stringBuilder.AppendLine(output);
      var result = stringBuilder.ToString();
      if (!string.IsNullOrEmpty(error) || !string.IsNullOrEmpty(output))
      {
        storage.Save(options.Value.LogFileName, result);
      }
    }
    catch (YarkovException e)
    {
      logger.Log(e.ToString(), "WARN", typeof(AiController).Name);
    }
    catch (Exception e)
    {
      logger.Log(e.ToString(), "ERROR", typeof(AiController).Name);
      throw;
    }
  }
}