using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;

namespace ParkingApp
{
    [Activity(Label = "ParkingApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private static MainActivity ThisActivity { get; set; }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ThisActivity = this;
            SetContentView(Resource.Layout.TelaLogin);
            var btnCadastro = FindViewById<Button>(Resource.Id.buttonCadastro);
            var btnLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            btnLogin.Click += capturaClickLogin;
            btnCadastro.Click += capturaClickCadastro;


        }
        public static JObject Usuario { get; private set; }
        private void capturaClickLogin(object sender, EventArgs e)
        {
            var editLogin = FindViewById<EditText>(Resource.Id.editEmail);
            var editSenha = FindViewById<EditText>(Resource.Id.editSenha);

            JObject dadosUsuario = LogonAPI(editLogin.Text, editSenha.Text);

            if(dadosUsuario!=null)
            {
               
                AbrirSessao(dadosUsuario);
                this.Finish();
            }
            else
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Erro");
                alert.SetMessage("Usuário ou senha incorretos");
                alert.SetButton("OK", (c, ev) =>
                {
                    // Ok button click task  
                });

                alert.Show();
            }
        }

        private JObject LogonAPI(string email, string senha)
        {
            JObject usuario = null;
            JObject dadosLogon = new JObject();
            dadosLogon.Add("Email", email);
            dadosLogon.Add("Senha", senha);
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/json");

                string url = "http://parkingmanagerserver.azurewebsites.net/api/UsuarioModels/Logon";
                string vagasJsonText = wc.UploadString(url,"POST", dadosLogon.ToString());
                usuario = (JObject)JsonConvert.DeserializeObject<JObject>(vagasJsonText);
                return usuario;

            }
        }

        private void capturaClickCadastro(object sender, EventArgs e)
        {
            StartActivity(typeof(AtividadeCadastro));
        }

        public static void AbrirSessao(JObject usuarioCadastrado)
        {
            Usuario = usuarioCadastrado;
            ThisActivity.StartActivity(typeof(AtividadeMapa));
        }
    }
}

