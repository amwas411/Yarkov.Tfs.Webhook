namespace Yarkov.Tfs.Webhook.Exceptions;

class YarkovValidationException(string message) : YarkovClientException(message)
{
}