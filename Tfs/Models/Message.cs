using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class Message
{
	public string Text {get;set;}
	public string Html {get;set;}
	public string Markdown {get;set;}
	public override string ToString()
	{
		return Printer.Print(this);
	}
}