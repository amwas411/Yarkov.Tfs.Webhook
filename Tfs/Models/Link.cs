using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class Link
{
	public required string Href {get;set;}

	public override string ToString()
	{
		return Printer.Print(this);
	}
}