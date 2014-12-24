using System;
using Newtonsoft.Json;

namespace BunchyAndroid.Models
{
	public class TokenResponseModel
	{
			[JsonProperty("access_token")]
			public string AccessToken { get; set; }

			[JsonProperty("token_type")]
			public string TokenType { get; set; }

			[JsonProperty("expires_in")]
			public int ExpiresIn { get; set; }

			[JsonProperty("userName")]
			public string Username { get; set; }

			[JsonProperty("email")]
			public string Email { get; set; }

			[JsonProperty(".issued")]
			public string IssuedAt { get; set; }

			[JsonProperty(".expires")]
			public string ExpiresAt { get; set; }

			[JsonProperty("error")]
			public string Error { get; set; }
		}
}

