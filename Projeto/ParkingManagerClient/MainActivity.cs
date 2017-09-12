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
        Marker UltimoPontoInteracao = null;
        Button btNovoCaminho = null;


        private StatusGUI _STATUS_GUI = StatusGUI.Normal;
        StatusGUI STATUS_GUI  {
            get { return _STATUS_GUI; }
            set {
                StatusGUI _STATUS_GUI_ANT = _STATUS_GUI;
                _STATUS_GUI = value;

                MudancaDeEstado(_STATUS_GUI_ANT, _STATUS_GUI);
            }
        }

        private void MudancaDeEstado(StatusGUI _STATUS_GUI_ANT, StatusGUI _STATUS_GUI)
        {
            if (_STATUS_GUI_ANT == StatusGUI.DesenhandoCaminho && _STATUS_GUI == StatusGUI.Normal)
            {
                foreach(Marcador marcador in MarcadoresColocados)
                {
                   if (marcador.Linha != null)
                        marcador.Linha.Remove();
                    marcador.Marker.Remove();
                }
                btNovoCaminho.Text = "Novo Caminho";
                btNovoCaminho.Visibility = ViewStates.Invisible;
                MarcadoresColocados.Clear();
                UltimoPontoInteracao = null;
            }
        }

        private GoogleMap GMap;

        public JObject EstacionamentoSelecionado { get; private set; }
        public List<Marcador> MarcadoresColocados = new List<Marcador>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            this.RequestWindowFeature( WindowFeatures.NoTitle);
            SetContentView (Resource.Layout.Main);
            SetUpMap();

            btNovoCaminho = FindViewById<Button>(Resource.Id.button1);

            btNovoCaminho.Click += (o, a) => {
                if (STATUS_GUI == StatusGUI.Normal)
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
                            btNovoCaminho.Text = "Salvar";
                            PopUpButtonPositiveOnClickListener(sender, args);

                        }
                        );
                    alertDialogBuilder.SetNegativeButton("Não", PopUpButtonNegativeOnClickListener);
                    // create alert dialog
                    AlertDialog alertDialog = alertDialogBuilder.Create();

                    // show it
                    alertDialog.Show();

                }
                else
                    if(STATUS_GUI == StatusGUI.DesenhandoCaminho)
                {
                    
                    SalvarPontosMostrados();
                    STATUS_GUI = StatusGUI.Normal;
                    
                    
                }
            };

        }

        private void SalvarPontosMostrados()
        {
            
                
                try
                {
                    JArray jPontos = new JArray();

                    foreach(var marcador in MarcadoresColocados)
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
                        string result = wc.UploadString(url, jPonto.ToString());
                        int.Parse(result);
                    }
                }
                    
                    
                }
                catch(FormatException ex)
                {

                }
                catch (Exception ex)
                {

                }
            
        }

        public override void OnBackPressed()
        {
            if(STATUS_GUI == StatusGUI.Normal)
                base.OnBackPressed();
            if(STATUS_GUI == StatusGUI.DesenhandoCaminho)
            {
                if (this.MarcadoresColocados.Count > 0)
                {
                    Marcador marcador = MarcadoresColocados[this.MarcadoresColocados.Count-1];
                    if(marcador.Linha!=null)
                        marcador.Linha.Remove();
                    marcador.Marker.Remove();
                    MarcadoresColocados.Remove(marcador);
                    

                }
                if(this.MarcadoresColocados.Count ==0)
                {
                    
                    STATUS_GUI = StatusGUI.Normal;
                }else
                {
                    UltimoPontoInteracao = MarcadoresColocados[this.MarcadoresColocados.Count - 1].Marker;
                }
            }
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
            this.GMap.MapLongClick += GMapLongClickEvent;

            ObterEstacionamentos();

        }

        private void GMapLongClickEvent(object sender, GoogleMap.MapLongClickEventArgs e)
        {
            if(STATUS_GUI == StatusGUI.DesenhandoCaminho)
            {
                if(UltimoPontoInteracao == null)
                {
                    //Colocar novo ponto
                    ColocarNovoPontoMapa(e.Point, (LatLng)null, EstacionamentoSelecionado);
                }else
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
            UltimoPontoInteracao = ponto;
            Marcador marcador = new Marcador(estacionamento, ponto);
            if (latlngDest != null)
            {
                var _latitude = latlngDest.Latitude;
                var _longitude = latlngDest.Longitude;
                //var _altitude = (ponto["Localizacao"])["Altitude"].Value<double>();
                PolylineOptions opt = new PolylineOptions();
                opt = opt.Add(latlngOrigem, new LatLng(_latitude, _longitude));
                opt = opt.InvokeWidth(5);
                opt = opt.InvokeColor(Color.Blue);

                Polyline line = GMap.AddPolyline(opt);
                marcador.Linha = line;
            }

            
            this.MarcadoresColocados.Add(marcador);

        }

        private void PopUpButtonNegativeOnClickListener(object sender, DialogClickEventArgs e)
        {
            if (sender is DialogInterface)
            {
                ((DialogInterface)sender).Dispose();
            }
        }

        private void PopUpButtonPositiveOnClickListener(object sender, DialogClickEventArgs e)
        {
            if (sender is DialogInterface)
            {
                ((DialogInterface)sender).Dispose();
            }
            STATUS_GUI = StatusGUI.DesenhandoCaminho;
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
           foreach(var estacionamento in lista)
            {
                var latitude = (estacionamento["Localizacao"])["Latitude"].Value<double>();
                var longitude = (estacionamento["Localizacao"])["Longitude"].Value<double>();
                var altitude = (estacionamento["Localizacao"])["Altitude"].Value<float>();
                LatLng latlng = new LatLng(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, altitude);
                GMap.MoveCamera(camera);

                var pontos = (JArray)estacionamento["Pontos"];
                MostrarPontosNoMapa(pontos);
                if (estacionamento["ImagemBase64"] != null)
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

                        if(altura == 0 || largura == 0)
                        {
                            altura = decodedByte.Height;
                            largura = decodedByte.Width;
                        }

                        GroundOverlay overlay = this.SobreporMapaComImagem(imgLatlng, BitmapDescriptorFactory.FromBitmap(decodedByte), largura, altura);
                        overlay.Clickable = true;
                        GMap.GroundOverlayClick += (obj, args) => {
                            if(args.GroundOverlay.Id == overlay.Id)
                            {
                                if (STATUS_GUI == StatusGUI.Normal)
                                {
                                    this.EstacionamentoSelecionado = (JObject)estacionamento;
                                    btNovoCaminho.Visibility = ViewStates.Visible;
                                    btNovoCaminho.Text = "Novo Caminho em: " + EstacionamentoSelecionado["Nome"];
                                }
                            }
                            else
                            {
                                btNovoCaminho.Visibility = ViewStates.Invisible;
                            }
                        };
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void MostrarPontosNoMapa(JArray lista)
        {
            foreach (var ponto in lista)
            {

                var latitude = (ponto["Localizacao"])["Latitude"].Value<double>();
                var longitude = (ponto["Localizacao"])["Longitude"].Value<double>();
                var altitude = (ponto["Localizacao"])["Altitude"].Value<double>();
                LatLng latlng = new LatLng(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle(ponto["Id"].Value<long>().ToString()).SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.dot));
                GMap.AddMarker(options);
                var vagas = (JArray)ponto["VagasConectadas"];

                foreach(var conexao in ponto["Conexoes"])
                {
                   
                    foreach(var _ponto in lista)
                    {
                        if(_ponto["Id"].Value<long>()== conexao.Value<long>())
                        {
                            var _latitude = (_ponto["Localizacao"])["Latitude"].Value<double>();
                            var _longitude = (_ponto["Localizacao"])["Longitude"].Value<double>();
                            var _altitude = (_ponto["Localizacao"])["Altitude"].Value<double>();
                            PolylineOptions opt = new PolylineOptions();
                            opt = opt.Add(new LatLng(latitude, longitude), new LatLng(_latitude, _longitude));
                            opt = opt.InvokeWidth(5);
                            opt = opt.InvokeColor(Color.Red);
                            
                            Polyline line = GMap.AddPolyline(opt);
                        }

                    }
                }


                MostrarVagasNoMapa((JObject)ponto,vagas);



            }
        }

        private void MostrarVagasNoMapa(JObject ponto,JArray lista)
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

