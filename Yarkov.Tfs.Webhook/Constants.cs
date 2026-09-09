public static class Constants
{
  public static class FieldNames
  {
    public static string CompletedWorkFieldName = "Microsoft.VSTS.Scheduling.CompletedWork";
    public static string ChangedDateFieldName = "System.ChangedDate";
    public static string TitleFieldName = "System.Title";
    public static string ProjectFieldName = "System.TeamProject";
  }

  public static class TimesheetControllerConstants
  {
    public static string CsvHeader = "changed date,title,completed work,url,author,project";
    public static string CsvFileName = "timesheet.csv";
  }
}