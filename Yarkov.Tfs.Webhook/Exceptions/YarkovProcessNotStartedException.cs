using System.Diagnostics;
namespace Yarkov.Tfs.Webhook.Exceptions;

class YarkovProcessNotStartedException(ProcessStartInfo info) : YarkovServerException($"{info.FileName} {info.Arguments}")
{
}