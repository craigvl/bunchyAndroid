using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace BunchyAndroid
{
	public class BunchyServices
	{
		public BunchyServices ()
		{
		}

		public RideDetailModel GetRideDetails(string id,string username)
		{
			var request = HttpWebRequest.Create(string.Format(@"http://private-d1ca0-bunchyapi.apiary-mock.com/bunch/ridedetail/"));
			//var request = HttpWebRequest.Create(string.Format(@"http://192.168.56.1:1524/api/bunch/get/{0}", "townsville"));
			request.ContentType = "application/json";
			request.Method = "GET";

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				if (response.StatusCode != HttpStatusCode.OK)
					Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					var content = reader.ReadToEnd();
					if(string.IsNullOrWhiteSpace(content)) {
						Console.Out.WriteLine("Response contained empty body...");
						return null;
					}
					else {
						Console.Out.WriteLine("Response Body: \r\n {0}", content);
						RideDetailModel _BunchDetailModel = JsonConvert.DeserializeObject<RideDetailModel>(content);
						return _BunchDetailModel;
					}
				}
			}
		}


		public List<RideModel> GetBunches(string username)
		{
			//var request = HttpWebRequest.Create(string.Format(@"http://private-d1ca0-bunchyapi.apiary-mock.com/bunch/get/{0}",username));
			var request = HttpWebRequest.Create(string.Format(@"http://private-d1ca0-bunchyapi.apiary-mock.com/bunch/get/{0}","foo"));
			//var request = HttpWebRequest.Create(string.Format(@"http://192.168.56.1:1524/api/bunch/get/{0}", "townsville"));
			request.ContentType = "application/json";
			request.Method = "GET";

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				if (response.StatusCode != HttpStatusCode.OK)
					Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					var content = reader.ReadToEnd();
					if(string.IsNullOrWhiteSpace(content)) {
						Console.Out.WriteLine("Response contained empty body...");
						return null;
					}
					else {
						Console.Out.WriteLine("Response Body: \r\n {0}", content);
						List<RideModel> _BunchListModel = JsonConvert.DeserializeObject<List<RideModel>>(content);
						return _BunchListModel;
					}
				}
			}
		}
	}
}

