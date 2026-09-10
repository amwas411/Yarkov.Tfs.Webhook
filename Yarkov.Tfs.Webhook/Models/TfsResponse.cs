namespace Yarkov.Tfs.Webhook.Models;

public class TfsResponse
{
	// public Guid SubscriptionId {get;set;}
	// public int NotificationId {get;set;}
	// public Guid Id {get;set;}
	// public required string EventType {get;set;}
	// public required string PublisherId {get;set;}
	// public Message? Message {get;set;}
	// public Message? DetailedMessage {get;set;}
	public required Resource Resource {get;set;}
	// public required string ResourceVersion {get;set;}
	// public required Dictionary<string, ResourceContainer> ResourceContainers {get;set;}
	public DateTime CreatedDate {get;set;}
}

public class TfsResponseComment
{
	// public Guid SubscriptionId {get;set;}
	// public int NotificationId {get;set;}
	// public Guid Id {get;set;}
	// public required string EventType {get;set;}
	// public required string PublisherId {get;set;}
	// public Message? Message {get;set;}
	// public Message? DetailedMessage {get;set;}
	public required ResourceComment Resource {get;set;}
	// public required string ResourceVersion {get;set;}
	// public required Dictionary<string, ResourceContainer> ResourceContainers {get;set;}
	public DateTime CreatedDate {get;set;}
}