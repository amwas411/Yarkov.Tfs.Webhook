namespace Yarkov.Tfs.Webhook.Models;

public class Revision
{
	public int Id {get;set;}
	public int Rev {get;set;}
	public required Dictionary<string, object> Fields {get;set;}
	public Dictionary<string, Link>? _links {get;set;}
	public required string Url {get;set;}
}