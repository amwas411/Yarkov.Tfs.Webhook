namespace Yarkov.Tfs.Exceptions;

public class YarkovException : Exception
{
	public YarkovException(string paramName): base(paramName){}
}

public class UnsupportedDictionaryException: YarkovException
{
	public UnsupportedDictionaryException(string dictionaryName): base(dictionaryName){}
}