using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class Revision
{
	public int Id {get;set;}
	public int Rev {get;set;}
	public string Url {get;set;}
  public override string ToString()
	{
		return Printer.Print(this);
	}
}