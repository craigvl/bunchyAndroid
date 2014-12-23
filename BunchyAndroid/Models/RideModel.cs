using System;
using Newtonsoft.Json;

namespace BunchyAndroid
{
	public class RideModel
	{

		[JsonProperty("name")]
		public string Name { get; set; }
			
		[JsonProperty("KeenCount")]
		public string KeenCount { get; set;}
	}
}

