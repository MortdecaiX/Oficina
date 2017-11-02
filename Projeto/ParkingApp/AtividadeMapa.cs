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
using Android.Graphics;

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

            foreach(Vaga vaga in this.ControleMapa.VagasColocadas)
            {
                if(e.Marker.Id == vaga.Marker.Id)
                {
                    MostrarRotaParaVaga(vaga);
                    break;
                }
            }

           
            foreach (Marcador marcador in ControleMapa.MarcadoresColocados)
            {
                if (marcador.Marker.Id == e.Marker.Id)
                {
                    DirecoesParaMarcador(marcador);
                    break;
                }
            }

        }
        bool alertadoGpsDesligado = false;
        private void DirecoesParaMarcador(Marcador marcador)
        {
            if (ControleMapa.LocalizacaoAtual == null)
            {
                if (!alertadoGpsDesligado)
                {
                    AlertaServicoLocalizacao();
                    alertadoGpsDesligado = true;
                }

            }
            else
            {

                string origem = ControleMapa.LocalizacaoAtual.Latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + ControleMapa.LocalizacaoAtual.Longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                string destino = marcador.Estacionamento["Localizacao"].Value<double>("Latitude").ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + marcador.Estacionamento["Localizacao"].Value<double>("Longitude").ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                JObject direcoes = ControleMapa.ObterDirecoes(origem, destino, true);//Direções
            }
        }

        private void AlertaServicoLocalizacao()
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
        }

        private void MostrarRotaParaVaga(Vaga vaga)
        {
            this.ControleMapa.PolylinesCaminhoInterno.ForEach(x => x.Remove());

               JObject estacionamento = null;
           if(ControleMapa.EstacionamentoSelecionado!=null && ControleMapa.EstacionamentoSelecionado.Value<long>("Id")== vaga.IdEstacionamento)
            {
                estacionamento = ControleMapa.EstacionamentoSelecionado;
            }
           else
            {
                estacionamento = ControleMapa.ObterEstacionamento(vaga.IdEstacionamento);
            }
            if (estacionamento==null) return;

            JArray pontos = (JArray)estacionamento["Pontos"];

            Graph g = new Graph();
            /*
            g.add_vertex('A', new Dictionary<char, int>() { { 'B', 7 }, { 'C', 8 } });
            g.add_vertex('B', new Dictionary<char, int>() { { 'A', 7 }, { 'F', 2 } });
            g.add_vertex('C', new Dictionary<char, int>() { { 'A', 8 }, { 'F', 6 }, { 'G', 4 } });
            g.add_vertex('D', new Dictionary<char, int>() { { 'F', 8 } });
            g.add_vertex('E', new Dictionary<char, int>() { { 'H', 1 } });
            g.add_vertex('F', new Dictionary<char, int>() { { 'B', 2 }, { 'C', 6 }, { 'D', 8 }, { 'G', 9 }, { 'H', 3 } });
            g.add_vertex('G', new Dictionary<char, int>() { { 'C', 4 }, { 'F', 9 } });
            g.add_vertex('H', new Dictionary<char, int>() { { 'E', 1 }, { 'F', 3 } });
            */
            JObject entrada = (JObject)pontos.Where(x => x.Value<bool>("Entrada") == true).FirstOrDefault();
            foreach (JObject ponto in pontos)
            {
                // g.add_vertex('A', new Dictionary<long, int>() { { 'B', 7 }, { 'C', 8 } });
                long id = ponto.Value<long>("Id");
                Dictionary<string, int> distancias = new Dictionary<string, int>();
                LatLng origem = new LatLng(ponto["Localizacao"].Value<double>("Latitude"), ponto["Localizacao"].Value<double>("Longitude"));
                foreach(var conexao in ponto["ConexoesComplexas"])
                {
                    var alvo = pontos.Where(x => x.Value<long>("Id") == conexao.ToObject<long>()).FirstOrDefault();
                    LatLng destino = new LatLng(alvo["Localizacao"].Value<double>("Latitude"), alvo["Localizacao"].Value<double>("Longitude"));

                    var distancia = (int)( CalculationByDistance(origem, destino)*1000);
                    distancias.Add(alvo.Value<long>("Id").ToString(), distancia);

                }

                g.add_vertex(ponto.Value<long>("Id").ToString(), distancias);

            }
            string c_entrada = entrada.Value<long>("Id").ToString();
            string c_alvo = vaga.Ponto.Value<long>("Id").ToString() ;

            
            var shortPath = g.shortest_path(c_entrada, c_alvo);

            JArray caminho = new JArray();
            List<string> demo = new List<string>();
            foreach (var no in shortPath)
            {
                var ponto = pontos.Where(x => no == x.Value<long>("Id").ToString()).FirstOrDefault();
                demo.Add(no);
                caminho.Add(ponto);
            }

            PolylineOptions opt = new PolylineOptions();
            double _lat = entrada["Localizacao"].Value<double>("Latitude");
            double _lng = entrada["Localizacao"].Value<double>("Longitude");
            
            opt = opt.InvokeWidth(20);
            opt = opt.InvokeColor( Color.Blue);


            foreach (var no in caminho)
            {
                double lat = no["Localizacao"].Value<double>("Latitude");
                double lng = no["Localizacao"].Value<double>("Longitude");
                opt = opt.Add(new LatLng(lat, lng));
            }
            opt.Add(new LatLng(_lat, _lng));


            ControleMapa.PolylinesCaminhoInterno.Add( GMap.AddPolyline(opt));

            if (ControleMapa.LocalizacaoAtual == null)
            {
                if (!alertadoGpsDesligado)
                {
                    AlertaServicoLocalizacao();
                    alertadoGpsDesligado = true;
                }

            }
            else
            {

                string origem_s = ControleMapa.LocalizacaoAtual.Latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + ControleMapa.LocalizacaoAtual.Longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                string destino_s = estacionamento["Localizacao"].Value<double>("Latitude").ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + estacionamento["Localizacao"].Value<double>("Longitude").ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                JObject direcoes = ControleMapa.ObterDirecoes(origem_s, destino_s, true);
            }

        }

        //Fonte https://stackoverflow.com/questions/14394366/find-distance-between-two-points-on-map-using-google-map-api-v2
        public double CalculationByDistance(LatLng StartP, LatLng EndP)
        {
            int Radius = 6371;// radius of earth in Km
            double lat1 = StartP.Latitude;
            double lat2 = EndP.Latitude;
            double lon1 = StartP.Longitude;
            double lon2 = EndP.Longitude;
            double dLat = (lat2 - lat1).ToRadians();
            double dLon = (lon2 - lon1).ToRadians();
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                    + Math.Cos((lat1).ToRadians())
                    * Math.Cos((lat2).ToRadians()) * Math.Sin(dLon / 2)
                    * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            double valueResult = Radius * c;
            double km = valueResult / 1;
            
            int kmInDec =(int) Math.Truncate(km) ;
            double meter = valueResult % 1000;
            int meterInDec = (int)Math.Truncate(meter); 

            return Radius * c;
        }

    }
}