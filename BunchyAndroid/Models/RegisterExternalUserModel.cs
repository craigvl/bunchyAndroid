using System;
using Newtonsoft.Json;

namespace BunchyAndroid
{
	public class RegisterExternalUserModel
	{
		public RegisterExternalUserModel ()
		{
		}

		[JsonProperty("username")]
		public string username { get; set; }

		[JsonProperty("email")]
		public string email { get; set; }
	}
}

