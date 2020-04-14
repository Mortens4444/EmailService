using EmailService.Enum;

namespace EmailService.Model
{
	public class SmtpServer
	{
		public string Host { get; }

		public int Port { get; }

		public bool Ssl { get; }

		public bool RequiresAuthentication { get; }

		public bool SupportAsync { get; }

		public SmtpAuthentication? SmtpAuthentication { get; }

		public bool ForceAuthenticationMethod => SmtpAuthentication.HasValue;

		public string Username { get; }

		public string Password { get; }

		public SmtpServer(string host,
			int port = 25,
			bool ssl = false,
			string username = null,
			string password = null,
			SmtpAuthentication? smtpAuthentication = null,
			bool supportAsync = true)
		{
			Host = host;
			Port = port;
			Ssl = ssl;
			SupportAsync = supportAsync;
			SmtpAuthentication = smtpAuthentication;
			Username = username;
			Password = password;
			RequiresAuthentication = username != null || password != null;
		}
	}
}
