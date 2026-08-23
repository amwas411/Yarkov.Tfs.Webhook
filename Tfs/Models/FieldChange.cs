using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class FieldChange
{
	public required object OldValue {get;set;}
	public required object NewValue {get;set;}

	public override string ToString()
	{
		return Printer.Print(this);
	}
}