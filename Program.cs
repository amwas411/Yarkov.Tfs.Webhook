using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Yarkov.Tfs.Models;
using Yarkov.Tfs.Storage;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FileStorage>();
builder.Services.AddOptions<FileStorageOptions>().Bind(builder.Configuration.GetSection("FileStorageOptions"));
var app = builder.Build();


app.MapPost("/probe", (FileStorage s, HttpContext ctx) =>
{
  var buffer = new byte[(int)ctx.Request.ContentLength];
  ctx.Request.Body.ReadAsync(buffer).GetAwaiter().GetResult();
  var payload = Encoding.UTF8.GetString(buffer);

  var sb = new StringBuilder();

var request = 
$"""
[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] {ctx.Request.GetDisplayUrl()}
{payload}

""";

s.SaveToFile(request);
return "";

});


app.Run();