using System;
using Newtonsoft.Json;

namespace BunchyAndroid
{
	public class RideDetailModel
	{
		public RideDetailModel ()
		{

		}

		[JsonProperty("name")]
		public string name { get; set; }

		[JsonProperty("status")]
		public string status { get; set; }

		[JsonProperty("keencount")]
		public string keencount { get; set;}

	}
}