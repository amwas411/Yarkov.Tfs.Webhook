namespace Yarkov.Tfs.Webhook.Storage
{
  public class AppOptions
  {
    public required string WorkDirectoryName { get; set; }
    public required string LogFileName { get; set; }
    public required string TerminalRunner { get; set; }
    public required string TimesheetFileName { get; set; }
    public required string AiWorkDirectoryName { get; set; }
    public required string AiName { get; set; }
    public required bool AiDiagnosticMode { get; set; }
  }
}