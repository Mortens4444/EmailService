using System;
using System.Net.Mail;
using System.Text;

namespace EmailService
{
	public class MailMessageManager
	{
		public MailMessage Build(string sender, string recipient, string subject, string body, string carbonCopy = null, string blindCarbonCopy = null)
		{
			var mailMessage = new MailMessage(sender, recipient)
			{
				SubjectEncoding = Encoding.UTF8,
				BodyEncoding = Encoding.UTF8,
				Subject = subject,
				Body = body
			};
			ExtendRecipents(mailMessage.CC, carbonCopy);
			ExtendRecipents(mailMessage.Bcc, blindCarbonCopy);
			return mailMessage;
		}

		private static void ExtendRecipents(MailAddressCollection mailAddresses, string value)
		{
			if (!String.IsNullOrEmpty(value))
			{
				mailAddresses.Add(value);
			}
		}
	}
}
