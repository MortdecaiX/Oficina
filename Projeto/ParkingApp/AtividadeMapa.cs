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
using System.Threading.Tasks;

namespace ParkingApp
{
    [Activity(Label = "AtividadeMapa")]
    public class AtividadeMapa : Activity, IOnMapReadyCallback
    {
        GoogleMap GMap;
        private SearchView search;

        public ControladorMapa ControleMapa { get; private set; }
        public JArray Estacionamentos { get; private set; }
        Marker marcadorPosicao = null;
        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.CameraPosition.Zoom = 1.0f;
            GMap = googleMap;
            //CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(-23.185106, -50.656109), 25.0f);
            //GMap.MoveCamera(camera);


            MarkerOptions options = new MarkerOptions().SetPosition(GMap.CameraPosition.Target).SetTitle("").SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.dot)).Visible(false);

            marcadorPosicao = GMap.AddMarker(options);


            this.ControleMapa = new ControladorMapa(this, googleMap);
            ControleMapa.Mapa.MarkerClick += Mapa_MarkerClick;
            ControleMapa.LocalizacaoAtualAlteradaEvent = MudancaLocalizacao;
            ControleMapa.IniciarControle();


        }

        private void MudancaLocalizacao()
        {
            if (ControleMapa.LocalizacaoAtual != null)
            {
                marcadorPosicao.Position = new LatLng(ControleMapa.LocalizacaoAtual.Latitude, ControleMapa.LocalizacaoAtual.Longitude);
                marcadorPosicao.Visible = true;
            }
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


        private async void BuscaEstacionamento(string termoBusca)
        {
           await Task.Run(() =>
            {
                try
                {

                    this.Estacionamentos = ControleMapa.ObterEstacionamentos(termoBusca.ToLower());

                    this.RunOnUiThread(() =>
                    {
                        ControleMapa.MostrarEstacionamentosNoMap(this.Estacionamentos);
                        if (Estacionamentos != null && Estacionamentos.Count > 0)
                        {
                            double lat = Estacionamentos.First["Localizacao"].Value<double>("Latitude");
                            double lng = Estacionamentos.First["Localizacao"].Value<double>("Longitude");
                            ControleMapa.DarZoom(lat, lng, ControleMapa.Mapa.MaxZoomLevel);
                        }
                    });

                }
                catch (Exception ex)
                {
                    this.RunOnUiThread(() =>
                    {
                        Toast toast = Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                        toast.Show();
                    });
                }

            });
        }

        private void Mapa_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            //Gera e desenha a rota da posição atual do smartphone até o estacionamento escolhido

           
            foreach (Marcador marcador in ControleMapa.MarcadoresColocados)
            {
                if (marcador.Marker.Id == e.Marker.Id)
                {

                    if (ControleMapa.LocalizacaoAtual == null)
                    {
                        Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog alert = dialog.Create();
                        alert.SetTitle("Serviços de Localização");
                        alert.SetMessage("Não foi possível obter a sua localização atual. Ative os serviços de localização e tente novamente.");
                        alert.SetButton("OK", (c, ev) =>
                        {
                            // Ok button click task  
                            StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings));
                        });

                        alert.Show();
                        return;
                    }

                    string origem = ControleMapa.LocalizacaoAtual.Latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + ControleMapa.LocalizacaoAtual.Longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                    string destino = marcador.Estacionamento["Localizacao"].Value<double>("Latitude").ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + marcador.Estacionamento["Localizacao"].Value<double>("Longitude").ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                    JObject direcoes = ControleMapa.ObterDirecoes(origem, destino, true);//Direções
                    break;
                }
            }

        }

    }
}