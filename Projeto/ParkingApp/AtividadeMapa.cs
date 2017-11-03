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

        VerificadorEstadoVagas VerificadorEstadoVagas = null;

        public ControladorMapa ControleMapa { get; private set; }
        public JArray Estacionamentos { get; private set; }
        public Vaga VagaEscolhida {
            get { return _vagaEscolhida; }
            set {
                _vagaEscolhida = value;
                VerificadorEstadoVagas.VagaEscolhida = value;
                VerificadorEstadoVagas.ContinuarVerificacaoVagaEscolhida = true;
                VerificadorEstadoVagas.VagaEscolhidaMudouEstadoEvent += VerificadorEstadoVagas_VagaEscolhidaMudouEstadoEvent;
                VerificadorEstadoVagas.VerificacaoVagaEscolhida();

                  
            }
        }

        private void VerificadorEstadoVagas_VagaMudouEstadoEvent(object sender, EventArgsMudancaEstadoVaga e)
        {
           ControleMapa.ChecarVisibilidadeVaga(e.Vaga);

        }

        

        Marker marcadorPosicao = null;
        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.CameraPosition.Zoom = 1.0f;
            GMap = googleMap;
            //CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(-23.185106, -50.656109), 25.0f);
            //GMap.MoveCamera(camera);


            


            this.ControleMapa = new ControladorMapa(this, googleMap);
            ControleMapa.Mapa.MarkerClick += Mapa_MarkerClick;
            ControleMapa.LocalizacaoAtualAlteradaEvent = MudancaLocalizacao;
            ControleMapa.IniciarControle();

            btnModoDirecao= FindViewById<Button>(Resource.Id.btModoDirecao);
            
            txtAngle = FindViewById<TextView>(Resource.Id.txtAngle);

            btnModoDirecao.Click += btnModoDirecao_Click;

        }
        float Bearing = 0;
        bool _modoDirecao = true;
        bool modoDirecao {
            get { return _modoDirecao; }
            set
            {
                _modoDirecao = value;
                btnModoDirecao.Text = modoDirecao ? "Modo Normal" : "Modo de Direção";
                if (value)
                {
                    Orientacao();
                }
            }
        }
        private void btnModoDirecao_Click(object sender, EventArgs e)
        {
            modoDirecao = !modoDirecao;
        }

        private void VerificadorEstadoVagas_VagaEscolhidaMudouEstadoEvent(object sender, EventArgsMudancaEstadoVaga e)
        {

            VerificadorEstadoVagas.VagaEscolhidaMudouEstadoEvent -= VerificadorEstadoVagas_VagaEscolhidaMudouEstadoEvent;

            Vaga vaga = e.Vaga;
            vaga.Marker.Visible = false;

            this.ControleMapa.PolylinesCaminhoParaVaga.ForEach(x => x.Remove());
            VerificadorEstadoVagas.ContinuarVerificacaoVagaEscolhida = false;

            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("Vaga Ocupada");
            alert.SetMessage("A vaga que você escolheu foi ocupada. Se foi você que ocupou, clique em 'OK'. Se não, clique em 'Recomendar Vaga' para alternar automaticamente para outra vaga.");

            
            alert.SetButton("OK", (c, ev) =>
            {
                
            });
            alert.SetButton2("Recomendar Vaga", (c, ev) =>
            {
                
            });

            alert.Show();

        }

        Android.Locations.Location ultimaLocalizacao = null;
        private void MudancaLocalizacao()
        {
            if (ControleMapa.LocalizacaoAtual != null)
            {

                if (marcadorPosicao != null)
                {
                    marcadorPosicao.Remove();
                }
                MarkerOptions options = new MarkerOptions().SetPosition(GMap.CameraPosition.Target).SetTitle("").SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.dot)).Visible(false);

                marcadorPosicao = GMap.AddMarker(options);

                marcadorPosicao.Position = new LatLng(ControleMapa.LocalizacaoAtual.Latitude, ControleMapa.LocalizacaoAtual.Longitude);
                marcadorPosicao.Visible = true;

                if (ultimaLocalizacao != null)
                {
                    Bearing = ultimaLocalizacao.BearingTo(ControleMapa.LocalizacaoAtual);
                    txtAngle.Text = Bearing.ToString()+"º";

                    if (modoDirecao)
                    {
                        Orientacao();
                    }
                }

                ultimaLocalizacao = ControleMapa.LocalizacaoAtual;

            }
        }

        private void Orientacao()
        {
            try
            {
                CameraPosition cameraPosition = new CameraPosition.Builder()
                                        .Target(marcadorPosicao.Position)      // Sets the center of the map to Mountain View
                                        .Zoom(18)                   // Sets the zoom
                                        .Bearing(Bearing)                // Sets the orientation of the camera to east
                                        .Tilt(45)                   // Sets the tilt of the camera to 30 degrees
                                        .Build();                   // Creates a CameraPosition from the builder
                this.GMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
            }catch { }
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

            VerificadorEstadoVagas =  new VerificadorEstadoVagas(this);

            VerificadorEstadoVagas.VagaMudouEstadoEvent += VerificadorEstadoVagas_VagaMudouEstadoEvent;


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
            ControleMapa.Limpar();
            modoDirecao = false;
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
                        

                        VerificadorEstadoVagas.ContinuarVerificacaoVagas = false;
                        ControleMapa.MostrarEstacionamentosNoMap(this.Estacionamentos);
                        if (Estacionamentos != null && Estacionamentos.Count > 0)
                        {
                            double lat = Estacionamentos.First["Localizacao"].Value<double>("Latitude");
                            double lng = Estacionamentos.First["Localizacao"].Value<double>("Longitude");


                            ControleMapa.DarZoom(lat, lng, ControleMapa.Mapa.MaxZoomLevel);
                            VerificadorEstadoVagas.VerificacaoVagas();
                            VerificadorEstadoVagas.ContinuarVerificacaoVagas = true;


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
                    if (!MainActivity.Usuario.Value<bool>("VagaEspecial") && vaga.Dados.Value<long>("Tipo") == 0)
                    {
                        this.VagaEscolhida = vaga;
                        MostrarRotaParaVaga(vaga);
                    }else
                    {
                        Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog alert = dialog.Create();
                        alert.SetTitle("Vaga Incompatível");
                        alert.SetMessage("Você não pode utilizar esta vaga pois ela é destinada à " + (vaga.Dados.Value<long>("Tipo") == 1 ? "idosos" : "cadeirantes")+"! Escolha outra vaga.");
                        alert.SetButton("OK", (c, ev) =>
                        {
                            //faz alguma coisa se precisar
                        });

                        alert.Show();
                    }
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
        private Vaga _vagaEscolhida;
        private TextView txtAngle;
        private Button btnModoDirecao;

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
            this.ControleMapa.PolylinesCaminhoParaVaga.ForEach(x => x.Remove());

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
            opt = opt.InvokeColor( Color.DarkSlateBlue);

            opt.Add(vaga.Marker.Position);
            foreach (var no in caminho)
            {
                double lat = no["Localizacao"].Value<double>("Latitude");
                double lng = no["Localizacao"].Value<double>("Longitude");
                opt = opt.Add(new LatLng(lat, lng));
            }
            opt.Add(new LatLng(_lat, _lng));


            

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
                JObject direcoes = ControleMapa.ObterDirecoes(origem_s, destino_s, false);


                string polylineString = ((JObject)(direcoes["routes"]).First["overview_polyline"])["points"].ToString();

                var polyline = GooglePoints.Decode(polylineString);
                for (int i = polyline.Count()-1; i>=0; i--)
                {
                    opt = opt.Add(polyline.ElementAt(i));
                }
                
            }
            ControleMapa.PolylinesCaminhoParaVaga.Add(GMap.AddPolyline(opt));

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