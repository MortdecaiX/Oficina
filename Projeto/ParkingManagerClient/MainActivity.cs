using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using System;
using Android.Gms.Maps.Model;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ParkingManagerClient
{
    [Activity(Label = "ParkingManagerClient", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity,IOnMapReadyCallback
    {
        private GoogleMap GMap;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
             SetContentView (Resource.Layout.Main);
            SetUpMap();
        }

        private void SetUpMap()
        {
            if (GMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.googlemap).GetMapAsync(this);
            }
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;
            
            JArray lista = null;
            using (WebClient wc = new WebClient())
            {
                string url = Resources.GetString(Resource.String.ParkingManagerServerURL) + Resources.GetString(Resource.String.ObterVagasURL);
                try
                {
                    string vagasJsonText = wc.DownloadString(url);
                    lista = (JArray)JsonConvert.DeserializeObject(vagasJsonText);
                }
                catch (Exception ex)
                {

                }
            }

            if (lista != null)
            {
                foreach (var vaga in lista)
                {
                    var latitude = (vaga["Localizacao"])["Latitude"].Value<double>();
                    var longitude = (vaga["Localizacao"])["Longitude"].Value<double>();
                    var altitude = (vaga["Localizacao"])["Altitude"].Value<double>();
                    LatLng latlng = new LatLng(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                    CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);
                    GMap.MoveCamera(camera);
                    MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle(vaga["Numero"].Value<long>().ToString());
                    GMap.AddMarker(options);
                }

            }

        }

        private void SobreporMapaComImagem(LatLng NEWARK, BitmapDescriptor bitmapDescriptor, float width, float heigth)
        {
            //LatLng NEWARK = new LatLng(-23.312847, -51.1448709);
            //BitmapDescriptorFactory.FromResource(Resource.Drawable.Icon)
            GroundOverlayOptions newarkMap = new GroundOverlayOptions()
            .InvokeImage(bitmapDescriptor).Position(NEWARK, width, heigth);
            GMap.AddGroundOverlay(newarkMap);
        }
    }
}

