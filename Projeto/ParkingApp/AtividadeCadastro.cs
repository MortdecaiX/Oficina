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
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ParkingApp
{
    [Activity(Label = "AtividadeCadastro")]
    public class AtividadeCadastro : Activity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
             base.OnCreate(savedInstanceState);

            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.TelaCadastro);

            var btnCadastro = FindViewById<Button>(Resource.Id.button1);
            btnCadastro.Click += capturaClickLogin;

        }

        private void capturaClickLogin(object sender, EventArgs e)
        {
            var editNome = FindViewById<EditText>(Resource.Id.editText1);
            var editSobrenome = FindViewById<EditText>(Resource.Id.editText2);
            var editEmail = FindViewById<EditText>(Resource.Id.editText3);
            var editSenha = FindViewById<EditText>(Resource.Id.editText4);
            var checkVagaEspecial = FindViewById<CheckBox>(Resource.Id.checkBoxVagaEspecial);

            Regex regexNome = new Regex(@"^([A-Za-z]+\s?)+\S$");
            Regex regexSobrenome = new Regex(@"^([A-Za-z]+\s?)+\S$");
            Regex regexEmail = new Regex(@"^([a-zA-Z0-9][-.a-zA-Z0-9_]{2}[-.a-zA-Z0-9_]*@gmail.com)$");
            Regex regexSenha = new Regex(@"^.{6}.*$");
            Match Nome = regexNome.Match(editNome.Text);
            Match Sobrenome = regexSobrenome.Match(editSobrenome.Text);
            Match Email = regexEmail.Match(editEmail.Text);
            Match Senha = regexSenha.Match(editSenha.Text);

            if (Nome.Success == true && Sobrenome.Success == true && Email.Success == true && Senha.Success == true)
            {
                try
                {
                    JObject usuarioCadastrado = null;
                    using (WebClient wb = new WebClient())
                    {
                        wb.Headers.Add("Content-Type", "application/json");
                        string endereco = "http://parkingmanagerserver.azurewebsites.net/api/UsuarioModels";
                        JObject cadastro = new JObject();

                        JObject usuario = new JObject();
                        usuario.Add("Id", 0);
                        usuario.Add("Nome", editNome.Text);
                        usuario.Add("Sobrenome", editSobrenome.Text);
                        usuario.Add("CPF", null);
                        usuario.Add("Email", editEmail.Text);
                        if(checkVagaEspecial.Checked == true)
                            usuario.Add("VagaEspecial",true);
                        else
                            usuario.Add("VagaEspecial",false);
                        
                        cadastro.Add("Usuario", usuario);
                        cadastro.Add("Senha", editSenha.Text);
                        

                        string conteudo = cadastro.ToString();
                        string resultado = wb.UploadString(endereco, "POST", conteudo);
                        usuarioCadastrado = (JObject)JsonConvert.DeserializeObject<JObject>(resultado);
                       
                    }


                    Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Sucesso");
                    alert.SetMessage("Cadastro realizado");
                    alert.SetButton("OK", (c, ev) =>
                    {
                        this.Finish();
                        MainActivity.AbrirSessao(usuarioCadastrado);
                       
                    });

                    alert.Show();
                }catch(Exception ex)
                {
                    Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Erro");
                    alert.SetMessage(ex.Message);
                    alert.SetButton("OK", (c, ev) =>
                    {

                    });

                    alert.Show();
                }
            }

            else if(Nome.Success == false)
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Erro");
                alert.SetMessage("Nome inválido");
                alert.SetButton("OK", (c, ev) =>
                {
                    editNome.IsFocused.Equals(true);
                });

                alert.Show();
            }

            else if (Sobrenome.Success == false)
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Erro");
                alert.SetMessage("Sobrenome inválido");
                alert.SetButton("OK", (c, ev) =>
                {
                    editSobrenome.IsFocused.Equals(true);
                });

                alert.Show();
            }

            else if (Email.Success == false)
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Erro");
                alert.SetMessage("Email inválido");
                alert.SetButton("OK", (c, ev) =>
                {
                    editEmail.IsFocused.Equals(true);
                });

                alert.Show();
            }

            else
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Erro");
                alert.SetMessage("Senha inválida");
                alert.SetButton("OK", (c, ev) =>
                {
                    editSenha.IsFocused.Equals(true);
                });

                alert.Show();
            }
        }
    }
}