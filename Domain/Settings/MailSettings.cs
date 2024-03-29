﻿namespace Domain.Settings
{
	public class MailSettings
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string EmailFrom { get; set; }
		public bool UsingLocalServer { get; set; }

		public bool IsUsingLocalMailServer()
		{
			return UsingLocalServer;
		}
	}
}
