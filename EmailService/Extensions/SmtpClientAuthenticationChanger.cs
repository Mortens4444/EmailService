using System;
using System.Net.Mail;
using System.Reflection;
using EmailService.Enum;
using EmailService.Model;

namespace EmailService
{
	/// <summary>
	/// NTLM (NT LAN Manager) Authentication /SMTP Extension/ throws System.FormatException - Invalid length for a Base-64 char array.
	/// </summary>
	public static class SmtpClientAuthenticationChanger
	{
		public static void ForceChangeIfNeeded(this SmtpClient smtpClient, SmtpServer smtpServer)
		{
			if (smtpServer.ForceAuthenticationMethod)
			{
				smtpClient.ForceChange(smtpServer.SmtpAuthentication.Value);
			}
		}

		public static void ForceChange(this SmtpClient smtpClient, SmtpAuthentication smtpAuthentication)
		{
			var transport = smtpClient.GetType().GetField("transport", BindingFlags.NonPublic | BindingFlags.Instance);
			if (transport != null)
			{
				var authenticationModules = transport.GetValue(smtpClient)
					.GetType()
					.GetField("authenticationModules", BindingFlags.NonPublic | BindingFlags.Instance);
				if (authenticationModules != null)
				{
					if (authenticationModules.GetValue(transport.GetValue(smtpClient)) is Array modulesArray)
					{
						var smtpAuthenticationModule = modulesArray.GetValue((byte)smtpAuthentication);
						modulesArray.SetValue(smtpAuthenticationModule, 0);
						modulesArray.SetValue(smtpAuthenticationModule, 1);
						modulesArray.SetValue(smtpAuthenticationModule, 2);
						modulesArray.SetValue(smtpAuthenticationModule, 3);
					}
				}
			}
		}
	}
}
