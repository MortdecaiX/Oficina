using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace ParkingApp
{
    [Activity(Label = "ParkingApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TelaLogin);
            var btnCadastro = FindViewById<Button>(Resource.Id.button2);
            var btnLogin = FindViewById<Button>(Resource.Id.button1);
            btnLogin.Click += capturaClickLogin;
            btnCadastro.Click += capturaClickCadastro;


        }

        private void capturaClickLogin(object sender, EventArgs e)
        {
            var viewLogin = FindViewById<EditText>(Resource.Id.editText2);
            var viewSenha = FindViewById<EditText>(Resource.Id.editText1);

            if(viewLogin.Text == "Luccas" && viewSenha.Text == "200996")
            {
                StartActivity(typeof(AtividadeMapa));
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

        private void capturaClickCadastro(object sender, EventArgs e)
        {
            StartActivity(typeof(AtividadeCadastro));
        }
    }
}

