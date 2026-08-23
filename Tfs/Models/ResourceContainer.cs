using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class ResourceContainer
{
  public Guid Id {get;set;}
  public string? BaseUrl {get;set;}
  public override string ToString()
	{
		return Printer.Print(this);
	}
}