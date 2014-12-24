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
using ByteSmith.WindowsAzure.Messaging;
using Gcm.Client;
using BunchyAndroid.Constant;

namespace BunchyAndroid
{

	[Activity (Label = "NewUser", Icon = "@drawable/bunchy")]
	public class NewUserActivity : Activity
	{

		EditText edittext;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.NewUser);
			Button buttonSubmit = FindViewById<Button> (Resource.Id.buttonNewUserSubmit);

			IEnumerable<Account> accounts = AccountStore.Create (this).FindAccountsForService ("google");
			Account useraccount = accounts.FirstOrDefault();

			edittext = FindViewById<EditText> (Resource.Id.editTextUserName);

			buttonSubmit.Click += buttonSubmit_Click;

		}

		void buttonSubmit_Click(object sender, EventArgs e)
		{

			Android.Widget.Toast.MakeText(this,edittext.Text, Android.Widget.ToastLength.Short).Show();
		}

	}

	[Activity (Label = "RideDetails", Icon = "@drawable/bunchy")]
	public class RideDetailsActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			string id = Intent.GetStringExtra ("Id") ?? "Id not available";
			SetContentView (Resource.Layout.RideDetail);
			RideDetailModel _RideDetailModel = new RideDetailModel ();
			_RideDetailModel = GetRideDetail (id);

			TextView DetailName = FindViewById<TextView> (Resource.Id.rideDetailName);
			DetailName.Text = _RideDetailModel.name;
		}

		RideDetailModel GetRideDetail(string id)
		{
			IEnumerable<Account> accounts = AccountStore.Create (this).FindAccountsForService ("google");
			Account useraccount = accounts.FirstOrDefault();
			BunchyServices _BunchyServices = new BunchyServices ();
			RideDetailModel _RideModel = new RideDetailModel();
			_RideModel = _BunchyServices.GetRideDetails (id,useraccount.Username );
			return _RideModel;
		}
	}



	[Activity (Label = "Rides", Icon = "@drawable/bunchy")]
	public class RidesActivity : Activity
	{
		//private IList<RideModel> rides;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			IEnumerable<Account> accounts = AccountStore.Create (this).FindAccountsForService ("google");
			Account useraccount = accounts.FirstOrDefault();

			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			SetContentView (Resource.Layout.Rides);

			ActionBar.Tab tab = ActionBar.NewTab();
			tab.SetText("Rides");
			tab.SetIcon(Resource.Drawable.bunchy);
			tab.TabSelected += (sender, args) => {
				// Do something when tab is selected
			};

			ActionBar.AddTab(tab);

			tab = ActionBar.NewTab();
			tab.SetText("Following");
			tab.SetIcon(Resource.Drawable.bunchy);
			tab.TabSelected += (sender, args) => {
				// Do something when tab is selected
			};
			ActionBar.AddTab(tab);

			var ridesAdapter = new RidesAdapter (this, useraccount.Username);
			var ridesListView = FindViewById<ListView> (Resource.Id.rideListView);

			ridesListView.Adapter = ridesAdapter;
			ridesListView.ItemClick += OnRideItemClick;
		}

		void OnRideItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var listView = sender as ListView;		
			Android.Widget.Toast.MakeText(this,"Loading....", Android.Widget.ToastLength.Short).Show();
			var rideDetailIntent = new Intent(this, typeof(RideDetailsActivity));
			rideDetailIntent.PutExtra ("Id", e.Id);
			StartActivity(rideDetailIntent);
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
				RegisterWithGCM ();
				var rideintent = new Intent (this, typeof(RidesActivity));
				StartActivity (rideintent);
			} else {
				Button buttonGoogle = FindViewById<Button> (Resource.Id.googleLogin);
				Button buttonFaceBook = FindViewById<Button> (Resource.Id.facebookLogin);

				buttonGoogle.Click += buttonGoogle_Click;
				buttonFaceBook.Click += buttonFaceBook_Click;
			}
		}

		public void RegisterWithGCM()
		{
			// Check to ensure everything's setup right
			GcmClient.CheckDevice(this);
			GcmClient.CheckManifest(this);

			// Register for push notifications
			System.Diagnostics.Debug.WriteLine("Registering...");
			GcmClient.Register(this, BunchyAndroid.Constant.Constant.SenderID);
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
			
					e.Account.Properties.TryGetValue ( "access_token" , out access_token );
					access_token_return = access_token;
					Console.WriteLine ( access_token );
					_TokenResponse = BunchyAuth(access_token_return,"google");
					e.Account.Username = _TokenResponse.Username;
					if (e.Account.Username == "New User") {
					AccountStore.Create (this).Save (e.Account, "google");
					var newuserintent = new Intent(this, typeof(NewUserActivity));
					StartActivity(newuserintent);
					}
					else
					{
					AccountStore.Create (this).Save (e.Account, "google");
					var rideintent = new Intent(this, typeof(RidesActivity));
					StartActivity(rideintent);
					}
				


				//textView.Text = access_token;
			} ; 

			//_TokenResponse = await loginservice.LoginExternal(access_token,"google");

			var intent = auth.GetUI (this);
			StartActivity (intent);

		}
	}
}