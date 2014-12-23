using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace BunchyAndroid
{
	public class RidesAdapter : BaseAdapter
	{

		List<RideModel> _rideModels;
		Activity _activity;

		public RidesAdapter (Activity activity, string username)
		{
			_activity = activity;
			FillRides (username);
		}

		void FillRides(string username)
		{
			BunchyServices _BunchyServices = new BunchyServices ();
			List<RideModel> _RideModel = new List<RideModel>();
			_rideModels = _BunchyServices.GetBunches (username);
		}

		public override int Count {
			get { return _rideModels.Count; }
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId (int position) {
			return _rideModels [position].Id;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate (
				Resource.Layout.RidesListItem, parent, false);

			var contactName = view.FindViewById<TextView> (Resource.Id.RideName);
			contactName.Text = _rideModels [position].Name;

			var keenCount = view.FindViewById<TextView> (Resource.Id.KeenCount);
			keenCount.Text = "Keen (" + _rideModels [position].KeenCount + ")";

			return view;
		}
	}
}

