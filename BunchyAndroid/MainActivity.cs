using System;
using BunchyAndroid.Services;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Auth;
using BunchyAndroid.Models;
using System.Collections.Generic;
using System.Linq;

namespace BunchyAndroid
{
	[Activity (Label = "BunchyRides", Icon = "@drawable/bunchy")]
	public class RidesActivity : ListActivity
	{
		private IList<RideModel> rides;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			IEnumerable<Account> accounts = AccountStore.Create (this).FindAccountsForService ("google");
			Account useraccount = accounts.FirstOrDefault();

			//SetContentView (Resource.Layout.Rides);

			rides = PopulateRides(useraccount.Username);

			var names = rides.Select (r => r.Name).ToList();

			ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, names);

			//EditText rideListTitle = FindViewById<EditText> (Resource.Id.rideListTitle);
			//rideListTitle.Text = useraccount.Username;

		}

		List<RideModel> PopulateRides(string username)
		{
			BunchyServices _BunchyServices = new BunchyServices ();
			List<RideModel> _RideModel = new List<RideModel>();
			_RideModel = _BunchyServices.GetBunches (username);
			return _RideModel;
		}
	}

	[Activity (Label = "Bunchy", MainLauncher = true, Icon = "@drawable/bunchy")]
	public class MainActivity : Activity
	{
		TextView textView;
		string access_token_return;
		private TokenResponseModel _TokenResponse = new TokenResponseModel();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it

			IEnumerable<Account> accounts = AccountStore.Create (this).FindAccountsForService ("google");

//			var enumerable = accounts as IList<Account> ?? accounts.ToList();
//			foreach (Account a in accounts) {
//				AccountStore.Create (this).Delete (a,a.Username);
//			}
				
			if (accounts.Any()) {
				var rideintent = new Intent (this, typeof(RidesActivity));
				StartActivity (rideintent);
			} else {
				Button buttonGoogle = FindViewById<Button> (Resource.Id.googleLogin);
				Button buttonFaceBook = FindViewById<Button> (Resource.Id.facebookLogin);
				textView = FindViewById<TextView> (Resource.Id.textViewb);

				buttonGoogle.Click += buttonGoogle_Click;
				buttonFaceBook.Click += buttonFaceBook_Click;
			}
		}

		void buttonGoogle_Click(object sender, EventArgs e)
		{
			LoginToGoogle ();
			//BunchyAuth(access_token_return,"google");
		}

		void buttonFaceBook_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException ();
		}

		TokenResponseModel BunchyAuth(string access_token_return, string provider)
		{
			LoginServices loginservices = new LoginServices ();
			_TokenResponse = loginservices.LoginExternal(access_token_return,provider);
			return _TokenResponse;
		}



		void LoginToGoogle ()
		{
			string access_token;

			LoginServices loginservice = new LoginServices ();
			var auth = new OAuth2Authenticator (
				clientId: "549020993769-tn974vfkrovsr1k4g65135k6m02vec6j.apps.googleusercontent.com", 
				scope: "https://www.googleapis.com/auth/userinfo.email", 
				authorizeUrl: new Uri ("https://accounts.google.com/o/oauth2/auth"),
				redirectUrl: new Uri ("http://bunchy.com/oauth2callback"), 
				getUsernameAsync: null);  

			auth.Completed += (sender , e ) =>
			{  
				Console.WriteLine ( e.IsAuthenticated );
				e.Account.Properties.TryGetValue ( "access_token" , out access_token );
				access_token_return = access_token;
				Console.WriteLine ( access_token );

				_TokenResponse = BunchyAuth(access_token_return,"google");

				e.Account.Username = _TokenResponse.Username;

				AccountStore.Create (this).Save (e.Account, "google");

				var rideintent = new Intent(this, typeof(RidesActivity));
				StartActivity(rideintent);
				//textView.Text = access_token;
			} ; 

			//_TokenResponse = await loginservice.LoginExternal(access_token,"google");

			var intent = auth.GetUI (this);
			StartActivity (intent);

		}
	}
}