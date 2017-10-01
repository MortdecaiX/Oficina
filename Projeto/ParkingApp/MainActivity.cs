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

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.TelaLogin);
            var btnLogin = FindViewById<Button>(Resource.Id.button2);
            btnLogin.Click += capturaClick;


        }

        private void capturaClick(object sender, EventArgs e)
        {
            StartActivity(typeof(AtividadeCadastro));
        }
    }
}

