using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class FieldChange
{
	public string OldValue {get;set;}
	public string NewValue {get;set;}

	public override string ToString()
	{
		return Printer.Print(this);
	}
}