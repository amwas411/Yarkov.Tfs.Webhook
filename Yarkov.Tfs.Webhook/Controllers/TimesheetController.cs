using System.Globalization;
using Yarkov.Tfs.Webhook.Exceptions;
using Yarkov.Tfs.Webhook.Models;
using Yarkov.Tfs.Webhook.Storage;

public class TimesheetController
{
  public static void Invoke(IFileStorage s, ILogger logger, TfsResponse response)
  {
    var culture = CultureInfo.InvariantCulture;
    try
    {
      #region Validation
      if (response.Resource.Fields == null)
      {
        throw new YarkovException($"\"Response.Resource.Fields\" is null.");
      }
      if (response.Resource.Revision == null)
      {
        throw new YarkovException($"\"Response.Resource.Revision\" is null.");
      }
      if (response.Resource._links == null)
      {
        throw new YarkovException($"\"Response.Resource._links\" is null.");
      }
      if (response.Resource.RevisedBy == null)
      {
        throw new YarkovException($"\"Response.Resource.RevisedBy\" is null.");
      }
      if (!response.Resource.Fields.ContainsKey(Constants.FieldNames.CompletedWorkFieldName))
      {
        throw new YarkovException($"Could not get \"Completed Work\" from a request: \"Response.Resource.Fields.{Constants.FieldNames.CompletedWorkFieldName}\" is empty or nonexistent.");
      }
      if (!response.Resource.Revision.Fields.ContainsKey(Constants.FieldNames.TitleFieldName))
      {
        throw new YarkovException($"Could not get \"Title\" from a request: \"response.Resource.Revision.Fields.{Constants.FieldNames.TitleFieldName}\" is empty or nonexistent.");
      }
      if (!response.Resource.Revision.Fields.ContainsKey(Constants.FieldNames.ProjectFieldName))
      {
        throw new YarkovException($"Could not get \"Title\" from a request: \"response.Resource.Revision.Fields.{Constants.FieldNames.ProjectFieldName}\" is empty or nonexistent.");
      }
      #endregion

      var changedDate = response.CreatedDate;
      if (DateTime.TryParse(response.Resource.Fields[Constants.FieldNames.ChangedDateFieldName]?.NewValue?.ToString(), out var dateTime))
      {
        changedDate = dateTime;
      }
      if (!double.TryParse(response.Resource.Fields[Constants.FieldNames.CompletedWorkFieldName].OldValue?.ToString(), culture, out var oldCompletedWork))
      {
        oldCompletedWork = 0.0;
      }
      if (!double.TryParse(response.Resource.Fields[Constants.FieldNames.CompletedWorkFieldName].NewValue?.ToString(), culture, out var actualCompletedWork))
      {
        actualCompletedWork = 0.0;
      }
      var title = response.Resource.Revision.Fields[Constants.FieldNames.TitleFieldName]?.ToString()?.Replace("\"", string.Empty).Replace(",", string.Empty);
      s.Save(
        Constants.TimesheetControllerConstants.CsvFileName,
        $"{changedDate:yyyy.MM.dd HH:mm:ss}," +
        $"\"{title}\"," +
        $"{Math.Round(actualCompletedWork - oldCompletedWork, 3).ToString(culture)}," +
        $"{response.Resource._links["html"].Href}," +
        $"{response.Resource.RevisedBy.UniqueName}," +
        $"{response.Resource.Revision.Fields[Constants.FieldNames.ProjectFieldName]}",
        Constants.TimesheetControllerConstants.CsvHeader
      );
    }
    catch (YarkovException e)
    {
      logger.Log(e.Message, "INFO", typeof(TimesheetController).Name);
    }
    catch (Exception e)
    {
      logger.Log(e.ToString(), "ERROR", typeof(TimesheetController).Name);
      throw;
    }
  }
}