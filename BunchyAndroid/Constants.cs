using System;

namespace BunchyAndroid.Constant
{
	public static class Constant
	{
			// Azure app specific URL and key
			public const string ApplicationURL = @"https://bunchy-ns.servicebus.windows.net/";
			public const string ApplicationKey = @"5Yd4V8KeJbMC6mLC/L0Mr71zFISf8xsYyDLKwY4g+o4=";
			public const string SenderID = "549020993769"; // Google API Project Number

			// Azure app specific connection string and hub path
			public const string ConnectionString = "Endpoint=sb://bunchy-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=5Yd4V8KeJbMC6mLC/L0Mr71zFISf8xsYyDLKwY4g+o4=";
			public const string NotificationHubPath = "bunchy";

		}
}