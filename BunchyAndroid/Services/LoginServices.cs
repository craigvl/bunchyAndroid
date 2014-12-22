using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using BunchyAndroid.Models;

namespace BunchyAndroid.Services
{
	public class LoginServices
	{
		public LoginServices ()
		{
		
		}

		public async Task<TokenResponseModel> Login(string username, string password)
		{
			//HttpWebRequest request = new HttpWebRequest(new Uri("http://192.168.56.1:1524/token"));
			HttpWebRequest request = new HttpWebRequest(new Uri("http://bunchyapi.azurewebsites.net/token"));
			request.Method = "POST";

			string postString = String.Format("username={0}&password={1}&grant_type=password", "craig", "pword");
			//Log.Info("bunchy",postString);
			byte[] bytes = Encoding.UTF8.GetBytes(postString);

			using (Stream requestStream = await request.GetRequestStreamAsync())
			{
				requestStream.Write(bytes, 0, bytes.Length);
			}

			try
			{
				HttpWebResponse httpResponse =  (HttpWebResponse)(await request.GetResponseAsync());
				string json;
				using (Stream responseStream = httpResponse.GetResponseStream())
				{
					json = new StreamReader(responseStream).ReadToEnd();
				}
				TokenResponseModel tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(json);
				//App.UserPreferences.SetString("Token",tokenResponse.AccessToken);
				//App.UserPreferences.SetString("UserName",username);
				//App.RegisterUser.RegisterWithGCMAndriod();
				return tokenResponse;
			}
			catch (Exception ex)
			{
				//Log.Info("bunchy",ex.Message);
				TokenResponseModel tokenResponse = new TokenResponseModel();
				tokenResponse.AccessToken = null;
				return tokenResponse;
			}	
		}

		public TokenResponseModel LoginExternal(string token, string provider)
		{
			string url = "http://bunchyapidev.azurewebsites.net/api/account/ObtainLocalAccessTokenIOS?provider=" + provider + "&externalAccessToken=" + token;

			var request = HttpWebRequest.Create(url);
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
						TokenResponseModel _TokenResponseModel = JsonConvert.DeserializeObject<TokenResponseModel>(content);
						return _TokenResponseModel;
					}
				}
			}	
		}
	}
}