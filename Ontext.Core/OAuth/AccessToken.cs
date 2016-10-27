using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace Ontext.Core.OAuth
{
	/// <summary>
	/// Represents a Token.
	/// </summary>
	[JsonObject]
	public class AccessToken
	{
		public AccessToken ()
		{
			UserId = Guid.Empty;
		}

		[JsonProperty ("access_token")]
		public string Token { get; set; }

		[JsonProperty ("token_type")]
		public string Type { get; set; }

		[JsonProperty ("expires_in")]
		public string ExpiresIn { get; set; }

		[JsonProperty ("userName")]
		public string UserName { get; set; }

		[JsonProperty ("refresh_token")]
		public string RefreshToken { get; set; }

		[JsonProperty (".expires")]
		public DateTime Expires { get; set; }

		[JsonProperty ("userId", NullValueHandling = NullValueHandling.Ignore)]
		public Guid UserId { get; set; }

		[JsonIgnore]
		public bool IsExpired {
			get { 
				return Expires.AddSeconds (30) < DateTime.UtcNow;
			}
		}
	}
}