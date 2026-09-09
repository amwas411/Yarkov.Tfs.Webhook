using System.Diagnostics;

class YarkovProcessNotStartedException(ProcessStartInfo info) : Exception($"{info.FileName} {info.Arguments}")
{
}