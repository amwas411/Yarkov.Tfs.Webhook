using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class RevisedBy
{
	public Guid Id {get;set;}
	public string DisplayName {get;set;}
	public string Url {get;set;}
	public Dictionary<string, Link> _links {get;set;}
	public string UniqueName {get;set;}
	public string ImageUrl {get;set;}
	public string Descriptor {get;set;}

	public override string ToString()
	{
		return Printer.Print(this);
	}
}