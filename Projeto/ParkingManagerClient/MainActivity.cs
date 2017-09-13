using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using System;
using Android.Gms.Maps.Model;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Content;
using System.Collections.Generic;

namespace ParkingManagerClient
{
    [Activity(Label = "ParkingManagerClient", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback
    {
        enum StatusGUI { Normal, DesenhandoCaminho }
        Marker _UltimoPontoInteracao = null;
        Marker UltimoPontoInteracao {
            get { return _UltimoPontoInteracao; }
            set {
                if (_UltimoPontoInteracao != null)
                    _UltimoPontoInteracao.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.dot));
                _UltimoPontoInteracao = value;
                if (_UltimoPontoInteracao != null)
                    _UltimoPontoInteracao.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.red_dot));
            }
        }
        Button btNovoCaminho = null;
        ToggleButton tButton = null;

        private StatusGUI _STATUS_GUI = StatusGUI.Normal;
        StatusGUI STATUS_GUI
        {
            get { return _STATUS_GUI; }
            set
            {
                StatusGUI _STATUS_GUI_ANT = _STATUS_GUI;
                _STATUS_GUI = value;

                MudancaDeEstado(_STATUS_GUI_ANT, _STATUS_GUI);
            }
        }

        private void MudancaDeEstado(StatusGUI _STATUS_GUI_ANT, StatusGUI _STATUS_GUI)
        {
            if (_STATUS_GUI_ANT == StatusGUI.DesenhandoCaminho && _STATUS_GUI == StatusGUI.Normal)
            {
                btNovoCaminho.Text = "Novo Caminho";
                btNovoCaminho.Visibility = ViewStates.Invisible;
                
                UltimoPontoInteracao = null;
            }
        }

        private GoogleMap GMap;

        public JObject EstacionamentoSelecionado { get; private set; }
        public List<Marcador> MarcadoresColocados = new List<Marcador>();
        private Button btAtualizar = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);
            SetUpMap();

            btNovoCaminho = FindViewById<Button>(Resource.Id.button1);
            btAtualizar = FindViewById<Button>(Resource.Id.button3);
            btAtualizar.Click += Atualizar;


            tButton = FindViewById<ToggleButton>(Resource.Id.toggleButton1);
            tButton.CheckedChange += TrocarTipoMapa;
            btNovoCaminho.Click += (o, a) =>
            {
                if (STATUS_GUI == StatusGUI.Normal)
                {
                    MostraPopUpEInserirPontos();

                }
                else
                    if (STATUS_GUI == StatusGUI.DesenhandoCaminho)
                {
                     STATUS_GUI = StatusGUI.Normal;
                }
            };

        }

        private void Atualizar(object sender, EventArgs e)
        {
            SetUpMap();
            foreach (Marcador marcador in MarcadoresColocados)
            {
                marcador.Marker.Remove();
                foreach(Polyline linha in marcador.Linhas)
                {
                    linha.Remove();
                }
                
            }
            MarcadoresColocados.Clear();
            ObterEstacionamentos();

        }

        private void TrocarTipoMapa(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                GMap.MapType = GoogleMap.MapTypeSatellite;
                
            }
            else
            {
                GMap.MapType = GoogleMap.MapTypeNormal;
            }
        }

        private void MostraPopUpEInserirPontos()
        {
            // get prompts.xml view
            LayoutInflater li = LayoutInflater.From(this);
            View promptsView = li.Inflate(Resource.Layout.CustomPopUpDialog, null);

            AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(
                    this);

            // set prompts.xml to alertdialog builder
            alertDialogBuilder.SetView(promptsView);

            TextView userInput = (TextView)promptsView.FindViewById(Resource.Id.textView1);
            userInput.Text = "Iniciar marcação de caminho?";

            // set dialog message
            alertDialogBuilder.SetCancelable(false);
            alertDialogBuilder.SetPositiveButton("Sim",
                (sender, args) =>
                {
                    btNovoCaminho.Text = "Parar";
                    STATUS_GUI = StatusGUI.DesenhandoCaminho;
                }
                );
            alertDialogBuilder.SetNegativeButton("Não", PopUpButtonNegativeOnClickListener);
            // create alert dialog
            AlertDialog alertDialog = alertDialogBuilder.Create();

            // show it
            alertDialog.Show();
        }

        private Marcador SalvarPontoInserido(Marcador marcador)
        {

            
                JArray jPontos = new JArray();
                
                
                {
                    string url = Resources.GetString(Resource.String.ParkingManagerServerURL) + string.Format("api/EstacionamentoModel/{0}/AdicionarPonto", EstacionamentoSelecionado["Id"].Value<long>());
                    JObject jPonto = new JObject();
                    jPonto.Add("VagasConectadas", null);
                    JObject jLocalizacao = new JObject();
                    jLocalizacao.Add("Latitude", marcador.Marker.Position.Latitude);
                    jLocalizacao.Add("Longitude", marcador.Marker.Position.Longitude);
                    jLocalizacao.Add("Altitude", 0);
                    jPonto.Add("Localizacao", jLocalizacao);
                    jPonto.Add("Entrada", false);
                    jPonto.Add("Saida", false);

                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                        string result = wc.UploadString(url, jPonto.ToString());
                        long id = ((JObject)JsonConvert.DeserializeObject(result))["Id"].Value<long>() ;
                        marcador.Id = id;
                        marcador.Marker.Title = id.ToString();
                        if(marcador.Conexoes!= null && marcador.Conexoes.Count>0)
                            AssociarPontos(id, marcador.Conexoes);

                        return marcador;
                    }
                }
                

            

        }

        private void AssociarPontos(long id, List<long> conexoes)
        {
            foreach (long conexao in conexoes) {
                string url = Resources.GetString(Resource.String.ParkingManagerServerURL) + string.Format("api/PontoModels/ConectarPontos/{0}/{1}", id, conexao);
                Uri uri = new Uri(url);
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadStringAsync(uri);
                }
            }
        }

        public override void OnBackPressed()
        {
            if (STATUS_GUI == StatusGUI.Normal)
                base.OnBackPressed();
            if (STATUS_GUI == StatusGUI.DesenhandoCaminho)
            {
                STATUS_GUI = StatusGUI.Normal;
            }
        }



        private void SetUpMap()
        {
            GMap = null;
            if (GMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.googlemap).GetMapAsync(this);
            }
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;
            this.GMap.MapLongClick += GMapLongClickEvent;
            this.GMap.MarkerClick += MarkerClickEvent;

            ObterEstacionamentos();

        }
        public void MarkerClickEvent(object sender, GoogleMap.MarkerClickEventArgs args)
        {
            Marcador marcador = this.MarcadoresColocados.Find(m => m.Marker.Id == args.Marker.Id);
            if(marcador!=null)
                UltimoPontoInteracao = args.Marker;
            else
            {

            }
        }
        private void GMapLongClickEvent(object sender, GoogleMap.MapLongClickEventArgs e)
        {
            if (STATUS_GUI == StatusGUI.DesenhandoCaminho)
            {
                if (UltimoPontoInteracao == null)
                {
                    //Colocar novo ponto
                    ColocarNovoPontoMapa(e.Point, (LatLng)null, EstacionamentoSelecionado);
                }
                else
                {
                    //colocar novo ponto a partir do ultimo interagido
                    ColocarNovoPontoMapa(e.Point, UltimoPontoInteracao.Position, EstacionamentoSelecionado);
                }
            }


        }

        private void ColocarNovoPontoMapa(LatLng latlngOrigem, LatLng latlngDest, JObject estacionamento)
        {
            MarkerOptions options = new MarkerOptions().SetPosition(latlngOrigem).SetTitle("").SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.dot));
            
            Marker ponto = GMap.AddMarker(options);
            
            Marcador marcador = new Marcador(estacionamento, ponto);
            if (latlngDest != null)
            {
                var _latitude = latlngDest.Latitude;
                var _longitude = latlngDest.Longitude;
                //var _altitude = (ponto["Localizacao"])["Altitude"].Value<double>();
                PolylineOptions opt = new PolylineOptions();
                opt = opt.Add(latlngOrigem, new LatLng(_latitude, _longitude));
                opt = opt.InvokeWidth(5);
                opt = opt.InvokeColor(Color.Red);

                Polyline line = GMap.AddPolyline(opt);
                marcador.Linhas.Add(line);
            }
            if (UltimoPontoInteracao != null)
            {
                marcador.Conexoes.Add(long.Parse(UltimoPontoInteracao.Title));
            }
            marcador = SalvarPontoInserido(marcador);
            UltimoPontoInteracao = marcador.Marker;
            this.MarcadoresColocados.Add(marcador);
            
        }

        private void PopUpButtonNegativeOnClickListener(object sender, DialogClickEventArgs e)
        {
            if (sender is DialogInterface)
            {
                ((DialogInterface)sender).Dispose();
            }
        }

        

        private void ObterEstacionamentos()
        {
            JArray lista = null;
            using (WebClient wc = new WebClient())
            {
                string url = Resources.GetString(Resource.String.ParkingManagerServerURL) + "api/EstacionamentoModels";
                try
                {
                    string vagasJsonText = wc.DownloadString(url);
                    lista = (JArray)JsonConvert.DeserializeObject(vagasJsonText);
                    MostrarEstacionamentosNoMap(lista);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void MostrarEstacionamentosNoMap(JArray lista)
        {
            foreach (var estacionamento in lista)
            {
                var latitude = (estacionamento["Localizacao"])["Latitude"].Value<double>();
                var longitude = (estacionamento["Localizacao"])["Longitude"].Value<double>();
                var altitude = (estacionamento["Localizacao"])["Altitude"].Value<float>();
                LatLng latlng = new LatLng(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, altitude);
                GMap.MoveCamera(camera);

                var imagemMarcador = BitmapDescriptorFactory.FromResource(Resource.Drawable.parking_sign);
                MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle("").SetIcon(imagemMarcador);

                Marker ponto = GMap.AddMarker(options);

                var pontos = (JArray)estacionamento["Pontos"];
                MostrarPontosNoMapa((JObject)estacionamento,pontos);
                if (!string.IsNullOrEmpty(estacionamento["ImagemBase64"].Value<string>()))
                {
                    try
                    {
                        var imglatitude = (estacionamento["LocalizacaoImagem"])["Latitude"].Value<double>();
                        var imglongitude = (estacionamento["LocalizacaoImagem"])["Longitude"].Value<double>();
                        var imgaltitude = (estacionamento["LocalizacaoImagem"])["Altitude"].Value<double>();
                        var altura = (estacionamento["ImagemAltura"]).Value<float>();
                        var largura = (estacionamento["ImagemLargura"]).Value<float>();
                        LatLng imgLatlng = new LatLng(Convert.ToDouble(imglatitude), Convert.ToDouble(imglongitude));


                       



                        byte[] decodedString = Base64.Decode(estacionamento["ImagemBase64"].Value<string>(), Base64Flags.Default);

                        Bitmap decodedByte = BitmapFactory.DecodeByteArray(decodedString, 0, decodedString.Length);

                        if (altura == 0 || largura == 0)
                        {
                            altura = decodedByte.Height;
                            largura = decodedByte.Width;
                        }

                        GroundOverlay overlay = this.SobreporMapaComImagem(imgLatlng, BitmapDescriptorFactory.FromBitmap(decodedByte), largura, altura);
                        overlay.Bearing = estacionamento["ImagemRotacao"].Value<float>();

                        overlay.Clickable = true;
                        GMap.GroundOverlayClick += (obj, args) =>
                        {
                            if (args.GroundOverlay.Id == overlay.Id)
                            {
                                if (STATUS_GUI == StatusGUI.Normal)
                                {
                                    this.EstacionamentoSelecionado = (JObject)estacionamento;
                                    btNovoCaminho.Visibility = ViewStates.Visible;
                                    btNovoCaminho.Text = "Novo Caminho em: " +              EstacionamentoSelecionado["Nome"];
                                }
                            }
                            else
                            {
                                btNovoCaminho.Visibility = ViewStates.Invisible;
                            }
                        };
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void MostrarPontosNoMapa(JObject estacionamento,JArray lista)
        {
            foreach (var ponto in lista)
            {

                var latitude = (ponto["Localizacao"])["Latitude"].Value<double>();
                var longitude = (ponto["Localizacao"])["Longitude"].Value<double>();
                var altitude = (ponto["Localizacao"])["Altitude"].Value<double>();
                LatLng latlng = new LatLng(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle(ponto["Id"].Value<long>().ToString()).SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.dot));
                Marker marker = GMap.AddMarker(options);
                var vagas = (JArray)ponto["VagasConectadas"];
                Marcador marcador = new Marcador(estacionamento, marker)
                {
                    Id = ponto["Id"].Value<long>()
                };
                
                foreach (var conexao in ponto["Conexoes"])
                {

                    foreach (var _ponto in lista)
                    {
                        if (_ponto["Id"].Value<long>() == conexao.Value<long>())
                        {
                            var _latitude = (_ponto["Localizacao"])["Latitude"].Value<double>();
                            var _longitude = (_ponto["Localizacao"])["Longitude"].Value<double>();
                            var _altitude = (_ponto["Localizacao"])["Altitude"].Value<double>();
                            PolylineOptions opt = new PolylineOptions();
                            opt = opt.Add(latlng, new LatLng(_latitude, _longitude));
                            opt = opt.InvokeWidth(5);
                            opt = opt.InvokeColor(Color.Red);

                            Polyline line = GMap.AddPolyline(opt);
                            marcador.Linhas.Add(line);
                        }

                    }
                }
                MarcadoresColocados.Add(marcador);


                MostrarVagasNoMapa((JObject)ponto, vagas);



            }
        }

        private void MostrarVagasNoMapa(JObject ponto, JArray lista)
        {
            if (lista != null)
            {
                foreach (var vaga in lista)
                {
                    var latitude = (vaga["Localizacao"])["Latitude"].Value<double>();
                    var longitude = (vaga["Localizacao"])["Longitude"].Value<double>();
                    var altitude = (vaga["Localizacao"])["Altitude"].Value<double>();
                    LatLng latlng = new LatLng(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                    MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle(vaga["Numero"].Value<long>().ToString());
                    GMap.AddMarker(options);


                    var _latitude = (ponto["Localizacao"])["Latitude"].Value<double>();
                    var _longitude = (ponto["Localizacao"])["Longitude"].Value<double>();
                    var _altitude = (ponto["Localizacao"])["Altitude"].Value<double>();
                    PolylineOptions opt = new PolylineOptions();
                    opt = opt.Add(new LatLng(latitude, longitude), new LatLng(_latitude, _longitude));
                    opt = opt.InvokeWidth(5);
                    opt = opt.InvokeColor(Color.Blue);

                    Polyline line = GMap.AddPolyline(opt);

                }

            }
        }

        private GroundOverlay SobreporMapaComImagem(LatLng geoPosition, BitmapDescriptor bitmapDescriptor, float width, float heigth)
        {
            //LatLng NEWARK = new LatLng(-23.312847, -51.1448709);
            //BitmapDescriptorFactory.FromResource(Resource.Drawable.Icon)
            GroundOverlayOptions newarkMap = new GroundOverlayOptions()
            .InvokeImage(bitmapDescriptor).Position(geoPosition, width, heigth);
            return GMap.AddGroundOverlay(newarkMap);
        }
    }
}

