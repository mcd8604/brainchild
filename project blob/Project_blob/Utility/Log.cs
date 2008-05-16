using System;

public class Log : IDisposable
{
#if FINAL
	private const string outfilename = "error.log";
	private static System.IO.TextWriter writer;
#else
	private static readonly System.IO.TextWriter writer = System.Console.Out;
#endif
	public static System.IO.TextWriter Out
	{
		get
		{
			return writer;
		}
	}

	public Log()
	{
#if FINAL
		writer = new System.IO.StreamWriter(outfilename);
#endif
	}

	void IDisposable.Dispose()
	{
#if FINAL
		writer.Dispose();
#endif
	}
}