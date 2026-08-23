using Yarkov.Tfs.Utilities;

namespace Yarkov.Tfs.Models;

class TfsResponse
{
	public Guid SubscriptionId {get;set;}
	public int NotificationId {get;set;}
	public Guid Id {get;set;}
	public string EventType {get;set;}
	public string PublisherId {get;set;}
	public Message Message {get;set;}
	public Message DetailedMessage {get;set;}
	public Resource Resource {get;set;}
	public string ResourceVersion {get;set;}
  public Dictionary<string, ResourceContainer> ResourceContainers {get;set;}
  public DateTime CreatedDate {get;set;}

	public override string ToString()
	{
		return Printer.Print(this);
	}
}