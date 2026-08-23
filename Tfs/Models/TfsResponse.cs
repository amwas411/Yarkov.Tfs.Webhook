using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class TfsResponse
{
	public Guid SubscriptionId {get;set;}
	public int NotificationId {get;set;}
	public Guid Id {get;set;}
	public required string EventType {get;set;}
	public required string PublisherId {get;set;}
	public Message? Message {get;set;}
	public Message? DetailedMessage {get;set;}
	public required Resource Resource {get;set;}
	public required string ResourceVersion {get;set;}
	public required Dictionary<string, ResourceContainer> ResourceContainers {get;set;}
	public DateTime CreatedDate {get;set;}

	public override string ToString()
	{
		return Printer.Print(this);
	}
}