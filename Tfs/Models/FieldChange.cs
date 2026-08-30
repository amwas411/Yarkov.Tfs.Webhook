using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class FieldChange
{
	public object? OldValue {get;set;}
	public object? NewValue {get;set;}

	public override string ToString()
	{
		return Printer.Print(this);
	}
}