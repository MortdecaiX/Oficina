using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;

namespace ParkingApp
{
    [Activity(Label = "Mapa")]
    public class Mapa : Activity, IOnMapReadyCallback
    {
        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.CameraPosition.Zoom = 1.0f;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TelaMapa);

            // Create your application here
            var search = FindViewById<SearchView>(Resource.Id.searchView1);
            search.QueryTextSubmit += capturaTexto;
        }


        private void capturaTexto(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            //e.Query
        }
    }
}