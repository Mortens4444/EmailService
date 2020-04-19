using System;
using System.Net.Mail;

namespace EmailService.EventArgs
{
	public class SentChangedEventArgs : System.EventArgs
	{
		public bool Sent { get; }

		public Exception Exception { get; }

		public MailAddress From { get; }

		public MailAddressCollection To { get; }

		public MailAddressCollection CC { get; }

		public MailAddressCollection Bcc { get; }

		public SentChangedEventArgs(Exception exception,
			MailAddress from,
			MailAddressCollection to,
			MailAddressCollection cc,
			MailAddressCollection bcc)
		{
			Sent = exception == null;
			Exception = exception;
			From = from;
			To = to;
			CC = cc;
			Bcc = bcc;
		}
	}
}
