namespace Yarkov.Tfs.Webhook.Models;

public class Resource
{
	// public int Id  {get;set;}
	// public int WorkItemId  {get;set;}
	// public int Rev  {get;set;}
	public RevisedBy? RevisedBy  {get;set;}
	// public DateTime RevisedDate  {get;set;}
	public Dictionary<string, FieldChange>? Fields {get;set;}
	public Dictionary<string, Link>? _links {get;set;}
  public Revision? Revision {get;set;}
	// public required string Url {get;set;}
	// public int ChangesetId {get;set;}
	// public Contact? Author {get;set;}
	// public Contact? CheckedInBy {get;set;}
	// public string? Comment {get;set;}
	public DateTime CreatedDate {get;set;}
}

public class ResourceComment
{
	// public int Id  {get;set;}
	// public int WorkItemId  {get;set;}
	// public int Rev  {get;set;}
	// public RevisedBy? RevisedBy  {get;set;}
	// public DateTime RevisedDate  {get;set;}
	public Dictionary<string, object>? Fields {get;set;}
	public Dictionary<string, Link>? _links {get;set;}
  // public Revision? Revision {get;set;}
	// public required string Url {get;set;}
	// public int ChangesetId {get;set;}
	// public Contact? Author {get;set;}
	// public Contact? CheckedInBy {get;set;}
	// public string? Comment {get;set;}
	public DateTime CreatedDate {get;set;}
}