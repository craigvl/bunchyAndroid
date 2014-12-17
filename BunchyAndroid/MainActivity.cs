using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Auth;

namespace BunchyAndroid
{
	[Activity (Label = "Bunchy", MainLauncher = true, Icon = "@drawable/bunchy")]
	public class MainActivity : Activity
	{

		TextView textView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button buttonGoogle = FindViewById<Button> (Resource.Id.googleLogin);
			Button buttonFaceBook = FindViewById<Button> (Resource.Id.facebookLogin);
			textView = FindViewById<TextView> (Resource.Id.textViewb);
			
			buttonGoogle.Click += buttonGoogle_Click;
			buttonFaceBook.Click += buttonFaceBook_Click;

		}

		void buttonGoogle_Click(object sender, EventArgs e)
		{
			LoginToGoogle ();
		}

		void buttonFaceBook_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException ();
		}

		void LoginToGoogle ()
		{
			string access_token;
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
				Console.WriteLine ( access_token );
				textView.Text = access_token;
			} ; 
			var intent = auth.GetUI (this);
			StartActivity (intent);
		}
	}
}