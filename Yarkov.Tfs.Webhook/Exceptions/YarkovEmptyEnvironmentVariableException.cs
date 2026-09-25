namespace Yarkov.Tfs.Webhook.Exceptions;

class YarkovEmptyEnvironmentVariableException(string variable) : YarkovServerException(variable)
{
}