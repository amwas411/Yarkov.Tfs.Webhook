using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Yarkov.Tfs.Webhook.Exceptions;
using Yarkov.Tfs.Webhook.Models;
using Yarkov.Tfs.Webhook.Storage;

public class AiController
{
  private static ConcurrentDictionary<int, Task> ActiveThreads = [];
  public static async Task<IResult> Invoke(IFileStorage storage, ILogger logger, IOptions<AppOptions> options, TfsResponseComment response, HttpContext ctx, [FromQuery]int? id)
  {
    if (id.HasValue)
    {
      var t = ActiveThreads.GetValueOrDefault(id.Value);
      if (t == null)
      {
        return TypedResults.BadRequest();
      }
      if (t.IsCompleted)
      {
        return TypedResults.Ok();
      }
      return TypedResults.NoContent();
    }

    try
    {
      #region Validation
      if (response.Resource.Fields == null)
      {
        throw new YarkovValidationException($"\"{nameof(response)}.{response.Resource}.{response.Resource.Fields}\" is null.");
      }
      if (!response.Resource.Fields.ContainsKey(Constants.FieldNames.HistoryFieldName) || 
        string.IsNullOrEmpty(response.Resource.Fields[Constants.FieldNames.HistoryFieldName].ToString()))
      {
        throw new YarkovValidationException($"\"{nameof(response)}.{response.Resource}.{response.Resource.Fields}[{Constants.FieldNames.HistoryFieldName}]\" is null or empty.");
      }
      if (!response.Resource.Fields[Constants.FieldNames.HistoryFieldName].ToString().ToLowerInvariant().Contains(options.Value.AiName))
      {
        throw new YarkovAiException($"Message does not contain AI name \"{options.Value.AiName}\".");
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
      var prompt = $"{runnerPath} -w \"{response.Resource.Url}\" --cd {directoryPath} --review {diagnostic}";
      var arguments = string.Format(formatArgument, prompt);
      
      var info = new ProcessStartInfo()
      {
        FileName = runner,
        Arguments = arguments,
        CreateNoWindow = true,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
      };

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
      var t = Task.Run(async () =>
      {
        logger.Log(Thread.CurrentThread.ManagedThreadId.ToString(), "INFO", typeof(AiController).Name);
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
      });
      ActiveThreads.AddOrUpdate(t.Id, t, (threadId, task) => task);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

      return TypedResults.Accepted(string.Concat([ctx.Request.PathBase.Value, ctx.Request.Path.Value, $"?id={t.Id}"]));
    }
    catch (YarkovAiException e)
    {
      return TypedResults.Ok(e.Message);
    }
    catch (YarkovClientException e)
    {
      return TypedResults.BadRequest(e.Message);
    }
    catch (YarkovServerException e)
    {
      return TypedResults.InternalServerError(e.Message);
    }
  }
}