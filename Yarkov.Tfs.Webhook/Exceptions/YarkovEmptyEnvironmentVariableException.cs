using Yarkov.Tfs.Webhook.Exceptions;

class YarkovEmptyEnvironmentVariableException(string variable) : YarkovException(variable)
{
}