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
using Newtonsoft.Json.Linq;

namespace ParkingApp
{
    [Activity(Label = "AtividadeMapa")]
    public class AtividadeMapa :Activity, IOnMapReadyCallback
    {
        GoogleMap GMap;
        private SearchView search;

        public ControladorMapa ControleMapa { get; private set; }
        public JArray Estacionamentos { get; private set; }

        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.CameraPosition.Zoom = 1.0f;
            GMap = googleMap;
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(-23.185106, -50.656109), 25.0f);
            GMap.MoveCamera(camera);

            this.ControleMapa = new ControladorMapa(this, googleMap);
            ControleMapa.Mapa.MarkerClick += Mapa_MarkerClick;
            ControleMapa.IniciarControle();


        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.TelaMapa);
            
            SetUpMap();
           

            // Create your application here
            search = FindViewById<SearchView>(Resource.Id.searchView1);
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
            e.Handled = true;
            search.ClearFocus();
            BuscaEstacionamento(e.Query.Trim());
        }


        private void BuscaEstacionamento(string termoBusca)
        {


            {
                try
                {
                    this.Estacionamentos = ControleMapa.ObterEstacionamentos(true, termoBusca.ToLower());
                    if (Estacionamentos != null && Estacionamentos.Count > 0)
                    {
                        double lat = Estacionamentos.First["Localizacao"].Value<double>("Latitude");
                        double lng = Estacionamentos.First["Localizacao"].Value<double>("Longitude");
                        ControleMapa.DarZoom(lat, lng, ControleMapa.Mapa.MaxZoomLevel);
                    }

                }
                catch (Exception ex)
                {

                    Toast toast = Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                    toast.Show();
                }

            }
        }

        private void Mapa_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            //Gera e desenha a rota da posição atual do smartphone até o estacionamento escolhido
            foreach (Marcador marcador in ControleMapa.MarcadoresColocados)
            {
                if (marcador.Marker.Id == e.Marker.Id)
                {
                    string origem = ControleMapa.LocalizacaoAtual.Latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + ControleMapa.LocalizacaoAtual.Longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                    string destino = marcador.Estacionamento["Localizacao"].Value<double>("Latitude").ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + marcador.Estacionamento["Localizacao"].Value<double>("Longitude").ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                    JObject direcoes = ControleMapa.ObterDirecoes(origem, destino, true);//Direções
                    break;
                }
            }

        }

    }
}