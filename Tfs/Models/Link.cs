using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class Link
{
	public string Href {get;set;}

	public override string ToString()
	{
		return Printer.Print(this);
	}
}