using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Yarkov.Tfs.Exceptions;
using Yarkov.Tfs.Models;
using Yarkov.Tfs.Storage;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FileStorage>();
builder.Services.AddOptions<FileStorageOptions>().Bind(builder.Configuration.GetSection("FileStorageOptions"));
var app = builder.Build();

var csvHeader = "changed date,title,completed work,url,author,project";
var tfsCompletedWorkFieldName = "Microsoft.VSTS.Scheduling.CompletedWork";
var tfsChangedDateFieldName = "System.ChangedDate";
var tfsTitleFieldName = "System.Title";
var tfsProjectFieldName = "System.TeamProject";

app.MapPost("/timesheet", async (FileStorage s, HttpContext ctx, TfsResponse response) =>
{
  try
  {
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

    if (!response.Resource.Fields.ContainsKey(tfsCompletedWorkFieldName))
    {
      throw new YarkovException($"Could not get \"Completed Work\" from a request: \"Response.Resource.Fields.{tfsCompletedWorkFieldName}\" is empty or nonexistent.");
    }

    if (!DateTime.TryParse(response.Resource.Fields[tfsChangedDateFieldName].NewValue.ToString(), out var dateTime))
    {
      throw new YarkovException($"Could not get task's \"Changed Date\" from a request: \"Response.Resource.Fields.{tfsChangedDateFieldName}\" is empty or nonexistent.");
    }
  
    s.SaveToFile(
      "timesheet.csv",
      $"{dateTime:yyyy/MM/dd HH:mm:ss}," + 
      $"\"{response.Resource.Revision.Fields[tfsTitleFieldName]}\"," +
      $"{response.Resource.Fields[tfsCompletedWorkFieldName].NewValue}," +
      $"{response.Resource._links["html"].Href}," + 
      $"{response.Resource.RevisedBy.UniqueName}," +
      $"{response.Resource.Revision.Fields[tfsProjectFieldName]}",
      csvHeader
    );
  }
  catch (YarkovException e)
	{
		await s.Log(e.Message, "ERROR", ctx.Request.GetDisplayUrl());
    ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    return "";
	}
  catch (Exception e)
  {
    await s.Log(e.ToString(), "ERROR", ctx.Request.GetDisplayUrl());
    throw;
  }

  return string.Empty;
});

app.MapPost("/commit", async () =>
{
  return "";
});


app.Run();