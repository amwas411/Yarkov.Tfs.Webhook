namespace Yarkov.Tfs.Webhook.Models;

public class RevisedBy: Contact
{
	public required string Url {get;set;}
	public required Dictionary<string, Link> _links {get;set;}
	public required string ImageUrl {get;set;}
	public required string Descriptor {get;set;}
}

public class Contact
{
	public Guid Id {get;set;}
	public required string DisplayName {get;set;}
	public required string UniqueName {get;set;}	
}