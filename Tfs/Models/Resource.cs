using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class Resource
{
	public int Id  {get;set;}
	public int WorkItemId  {get;set;}
	public int Rev  {get;set;}
	public RevisedBy RevisedBy  {get;set;}
	public DateTime RevisedDate  {get;set;}
	public Dictionary<string, FieldChange> Fields {get;set;}
	public Dictionary<string, Link> _links {get;set;}
  public Revision Revision {get;set;}
	public string Url {get;set;}
	public override string ToString()
	{
		return Printer.Print(this);
	}
}