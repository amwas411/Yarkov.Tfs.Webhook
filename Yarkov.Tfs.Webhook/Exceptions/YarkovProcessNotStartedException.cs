using System.Diagnostics;
using Yarkov.Tfs.Webhook.Exceptions;

class YarkovProcessNotStartedException(ProcessStartInfo info) : YarkovException($"{info.FileName} {info.Arguments}")
{
}