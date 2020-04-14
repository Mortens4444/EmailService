using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using EmailService.EventArgs;
using EmailService.Model;

namespace EmailService
{
	public class SendMail
	{
		public delegate void SentChangedEventHandler(object sender, SentChangedEventArgs e);
		public event SentChangedEventHandler SentChanged;

		private readonly SmtpClient smtpClient;
		private readonly MailMessageManager mailMessageManager = new MailMessageManager();

		private const int NotUsed = 0, Used = 1;
		private int resource = NotUsed;

		public bool SupportAsync { get; private set; }

		public SendMail(SmtpServer smtpServer)
		{
			smtpClient = new SmtpClient(smtpServer.Host, smtpServer.Port)
			{
				Timeout = 180000,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				EnableSsl = smtpServer.Ssl,
				UseDefaultCredentials = !smtpServer.RequiresAuthentication,
				Credentials = smtpServer.RequiresAuthentication ?
					new NetworkCredential(smtpServer.Username, smtpServer.Password) :
					CredentialCache.DefaultNetworkCredentials
			};
			smtpClient.SendCompleted += (_, e) => { EndSending((MailMessage)e.UserState, e.Error); };
			smtpClient.ForceChangeIfNeeded(smtpServer);

			SupportAsync = smtpServer.SupportAsync;
		}

		public void Send(string sender, string recipient, string subject, string body, string carbonCopy = null, string blindCarbonCopy = null)
		{
			var mailMessage = mailMessageManager.Build(sender, recipient, subject, body, carbonCopy, blindCarbonCopy);

			try
			{
				while (Interlocked.Exchange(ref resource, Used) != NotUsed)
				{
					Thread.Sleep(TimeSpan.FromMilliseconds(100));
				}

				if (SupportAsync)
				{
					smtpClient.SendAsync(mailMessage, mailMessage);
				}
				else
				{
					smtpClient.Send(mailMessage);
					EndSending(mailMessage, null);
				}
			}
			catch (Exception ex)
			{
				EndSending(mailMessage, ex);
			}
		}

		private void EndSending(MailMessage mailMessage, Exception exception)
		{
			SentChanged?.Invoke(this, new SentChangedEventArgs(exception == null, exception));
			mailMessage.Dispose();
			Interlocked.Exchange(ref resource, NotUsed);
		}
	}
}
