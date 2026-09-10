public static class Constants
{
  public static class FieldNames
  {
    public static string CompletedWorkFieldName = "Microsoft.VSTS.Scheduling.CompletedWork";
    public static string ChangedDateFieldName = "System.ChangedDate";
    public static string TitleFieldName = "System.Title";
    public static string ProjectFieldName = "System.TeamProject";
    public static string HistoryFieldName = "System.History";
  }

  public static class TimesheetController
  {
    public static string CsvHeader = "changed date,title,completed work,url,author,project";
  }

  public static class AiController
  {
    public static string TfsBaseUrl = "TFS_BASE_URL";
    public static string TfsPat = "TFS_PAT";
  }
}