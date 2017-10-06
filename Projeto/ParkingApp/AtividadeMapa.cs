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
    [Activity(Label = "AtividadeMapa")]
    public class AtividadeMapa :Activity, IOnMapReadyCallback
    {
        GoogleMap GMap;
        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.CameraPosition.Zoom = 1.0f;
            GMap = googleMap;
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(-23.185106, -50.656109), 25.0f);
            GMap.MoveCamera(camera);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TelaMapa);
            SetUpMap();
           

            // Create your application here
            var search = FindViewById<SearchView>(Resource.Id.searchView1);
            search.QueryTextSubmit += capturaTexto;
        }
        private void SetUpMap()
        {
            GMap = null;
            if (GMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.gmap).GetMapAsync(this);
            }
        }

        private void capturaTexto(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            //e.Query
        }
    }
}