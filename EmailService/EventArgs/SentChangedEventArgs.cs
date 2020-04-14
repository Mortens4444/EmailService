using System;

namespace EmailService.EventArgs
{
	public class SentChangedEventArgs : System.EventArgs
	{
		public bool Sent { get; }

		public Exception Exception { get; }

		public SentChangedEventArgs(bool sent, Exception exception)
		{
			Sent = sent;
			Exception = exception;
		}
	}
}
