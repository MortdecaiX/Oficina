using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Json;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace ArduinoCom
{
    public partial class Form1 : Form
    {
        static WebClient client = new WebClient();
        static string PREFIX_URL = "http://synccer.xyz/koala/GreenBack/getArduinoData.php?";
        bool sincronizacaoIniciada = false;
        SerialPort port = null;
        int term = 0;
        public Form1()        
        {
            InitializeComponent();
           
           
            if (ArduinoCom.Properties.Settings.Default.listaOffline != null)
            {
                if (ArduinoCom.Properties.Settings.Default.listaOffline.Length ==0)
                {
                    var lista = new JsonObject();
                    var listagem = new JsonArray();
                    lista.Add("lista", listagem);
                    ArduinoCom.Properties.Settings.Default.listaOffline = lista.ToString();
                    ArduinoCom.Properties.Settings.Default.Save();
                    MessageBox.Show(lista.ToString());
                }
            }
                
        }

        private void tratarDados(object sender, SerialDataReceivedEventArgs e)
        {

                try
            {

                sincronizacaoIniciada = true;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        button1.Text = "Parar sincronização";
                        // button1.Text = "Iniciar Sincronização em Tempo Real";
                    });
                }
                string text = ((SerialPort)sender).ReadLine() + "\n";
                //MessageBox.Show(text);
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        richTextBox1.Text = (text) + "\n" + richTextBox1.Text;
                    });
                }
                string leitura = text.Substring(text.IndexOf("<leitura>"), text.IndexOf("</leitura>"));

                string umidade_ar = leitura.Substring(leitura.IndexOf("<umidade_ar>"));
                umidade_ar = ("umidade_ar=" + Regex.Match(umidade_ar, @"\d+").Value);

                string umidade_terra = leitura.Substring(leitura.IndexOf("<umidade_terra>"));
                umidade_terra = ("&umidade_terra=" + Regex.Match(umidade_terra, @"\d+").Value);


                string temperatura = leitura.Substring(leitura.IndexOf("<temperatura>"));
                temperatura = ("&temperatura=" + Regex.Match(temperatura, @"\d+").Value);

                //string luminosidade // leitura.Substring(leitura.IndexOf("<luminosidade>"));
                // = ""; // leitura.Substring(leitura.IndexOf("<luminosidade>"));
                string luminosidade = ("&luminosidade=" + 0);


                string direcao_vento = leitura.Substring(leitura.IndexOf("<direcao_vento>"));
                direcao_vento = ("&direcao_vento=" + Regex.Match(direcao_vento, @"\d+").Value);



                string velocidade_vento = leitura.Substring(leitura.IndexOf("<velocidade_vento>"));
                var vel_lida = int.Parse(Regex.Match(velocidade_vento, @"\d+").Value);

                // velocidade_vento = ("&velocidade_vento=" + (vel_lida > 0 ? vel_lida : new Random().Next(5, 11)) ); 
                velocidade_vento = ("&velocidade_vento=" + (vel_lida));

                string terminal = "&terminal=" + term++;
                if (term > 5) term = 0;
                var time = DateTime.UtcNow;
                //time =time.AddHours(-2);
                //time =time.AddMinutes(2);

                Int32 unixTimestamp = (Int32)(time.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string tempo = "&timestamp=" + unixTimestamp;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        richTextBox1.Text = "timestamp:"+ (unixTimestamp) + "\n" + richTextBox1.Text;
                    });
                }

                string request = (PREFIX_URL + umidade_ar + umidade_terra + temperatura + luminosidade + direcao_vento + velocidade_vento + terminal + tempo);
                try
                {

                    dynamic listaOffine = (dynamic)JsonValue.Parse(ArduinoCom.Properties.Settings.Default.listaOffline).ToJsonObject();
                    JsonArray lista = (JsonArray)listaOffine.lista;

                    for(int i =0; i< lista.Count;i++)
                     {
                            //MessageBox.Show("Resta: "+ lista[i].ToString());
                            lista.RemoveAt(i);
                     }
                    ArduinoCom.Properties.Settings.Default.listaOffline = listaOffine.ToString();
                    ArduinoCom.Properties.Settings.Default.Save();

                    Stream data = client.OpenRead(request);
                    StreamReader reader = new StreamReader(data);
                    string response = reader.ReadToEnd();
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            richTextBox1.Text = (response) + "\n" + richTextBox1.Text;
                        });
                    }
                    data.Close();
                    reader.Close();
                }
                catch
                {
                    try
                    {
                        //MessageBox.Show(ArduinoCom.Properties.Settings.Default.listaOffline);
                        dynamic listaOffine = (dynamic)JsonValue.Parse(ArduinoCom.Properties.Settings.Default.listaOffline).ToJsonObject();
                        JsonArray lista = (JsonArray)listaOffine.lista;
                        lista.Add(request);
                        ArduinoCom.Properties.Settings.Default.listaOffline = listaOffine.ToString();
                        ArduinoCom.Properties.Settings.Default.Save();

                        MessageBox.Show(listaOffine.ToString());
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke((MethodInvoker)delegate
                            {
                                richTextBox1.Text = "\n\nFalha ao conectar ao servidor. Dados adicionados à lista offline para sincronização futura. Total na lista: " + lista.Count + "\n" + richTextBox1.Text;
                            });
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {

                 //MessageBox.Show(ex.ToString());
            }
        }


        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!sincronizacaoIniciada)
            {
                //iniciarSincronismo();
                port = new SerialPort(textBox1.Text, 9600);
                // iniciarSincronismoSimulado();
                port.DataReceived += new SerialDataReceivedEventHandler(tratarDados);
                try
                {
                    port.Open();
                }
                catch(Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                sincronizacaoIniciada = false;
                button1.Text = "Iniciar Sincronização em Tempo Real";
                try
                {
                    port.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private async void iniciarSincronismoSimulado()
        {
            await Task.Run(() =>
            {
                int term = 0;
                try
                {
                   
                    sincronizacaoIniciada = true;
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            button1.Text = "Parar sincronização";
                            // button1.Text = "Iniciar Sincronização em Tempo Real";
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
                while (sincronizacaoIniciada)
                {
                    String s = "oi";
                    //if (s.Equals("exit"))
                    //{
                    //   break;
                    //}port.Write(s + '\n');
                    try
                    {
                        Random rnd = new Random();
                        string text = "<leitura><velocidade_vento><vel></velocidade_vento><direcao_vento><dir></direcao_vento><luminosidade><lumi></luminosidade><temperatura><temp></temperatura><umidade_terra><umit></umidade_terra><umidade_ar><umia></umidade_ar></leitura>" + "\n";
                        // MessageBox.Show(text);

                        text = text.Replace("<vel>", rnd.Next(0, 11).ToString());
                        text = text.Replace("<dir>", rnd.Next(0,180).ToString());
                        text = text.Replace("<lumi>", rnd.Next(40, 50).ToString());
                        text = text.Replace("<temp>", rnd.Next(22, 27).ToString());
                        text = text.Replace("<umit>", rnd.Next(50,55).ToString());
                        text = text.Replace("<umia>", rnd.Next(30, 50).ToString());


                        string leitura = text.Substring(text.IndexOf("<leitura>"), text.IndexOf("</leitura>"));

                        string umidade_ar = leitura.Substring(leitura.IndexOf("<umidade_ar>"));
                        umidade_ar = ("umidade_ar=" + Regex.Match(umidade_ar, @"\d+").Value);

                        string umidade_terra = leitura.Substring(leitura.IndexOf("<umidade_terra>"));
                        umidade_terra = ("&umidade_terra=" + Regex.Match(umidade_terra, @"\d+").Value);


                        string temperatura = leitura.Substring(leitura.IndexOf("<temperatura>"));
                        temperatura = ("&temperatura=" + Regex.Match(temperatura, @"\d+").Value);


                        string luminosidade = leitura.Substring(leitura.IndexOf("<luminosidade>"));
                        luminosidade = ("&luminosidade=" + Regex.Match(luminosidade, @"\d+").Value);


                        string direcao_vento = leitura.Substring(leitura.IndexOf("<direcao_vento>"), leitura.IndexOf("</direcao_vento>"));
                        direcao_vento = ("&direcao_vento=" + Regex.Match(direcao_vento, @"\d+").Value);


                        string velocidade_vento = leitura.Substring(leitura.IndexOf("<velocidade_vento>"));
                        velocidade_vento = ("&velocidade_vento=" + Regex.Match(velocidade_vento, @"\d+").Value);
                        
                        string terminal = "&terminal=" + term++;
                        if (term > 5) term = 0;
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        string tempo = "&timestamp=" +unixTimestamp;


                        string request = (PREFIX_URL + umidade_ar + umidade_terra + temperatura + luminosidade + direcao_vento + velocidade_vento + terminal + tempo);

                        try
                        {
                            //dynamic listaOffine = (dynamic)JsonValue.Parse(ArduinoCom.Properties.Settings.Default.listaOffline).ToJsonObject();
                            //JsonArray lista = (JsonArray)listaOffine.lista;

                            //for(int i =0; i< lista.Count;i++)
                           // {
                                //MessageBox.Show("Resta: "+ lista[i].ToString());
                                //lista.RemoveAt(i);
                           // }
                            //ArduinoCom.Properties.Settings.Default.listaOffline = listaOffine.ToString();
                            //ArduinoCom.Properties.Settings.Default.Save();

                            Stream data = client.OpenRead(request);
                            StreamReader reader = new StreamReader(data);

                            string response = reader.ReadToEnd();
                            if (this.InvokeRequired)
                            {
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    richTextBox1.Text = "\n" + (request) + richTextBox1.Text;
                                    richTextBox1.Text = "\n" + (response) + richTextBox1.Text;
                                });
                            }
                            data.Close();
                            reader.Close();
                        }
                        catch (Exception ex) {
                            try
                            {
                                //MessageBox.Show(ArduinoCom.Properties.Settings.Default.listaOffline);
                                dynamic listaOffine =(dynamic) JsonValue.Parse(ArduinoCom.Properties.Settings.Default.listaOffline).ToJsonObject();
                                JsonArray lista = (JsonArray)listaOffine.lista;
                                lista.Add(request);
                                ArduinoCom.Properties.Settings.Default.listaOffline = listaOffine.ToString();
                                ArduinoCom.Properties.Settings.Default.Save();

                                MessageBox.Show(listaOffine.ToString());
                                if (this.InvokeRequired)
                                {
                                    this.BeginInvoke((MethodInvoker)delegate
                                    {
                                        richTextBox1.Text =  "Falha ao conectar ao servidor. Dados adicionados à lista offline para sincronização futura. Total na lista: "+ lista.Count+"\n" + richTextBox1.Text;
                                    });
                                }
                            }
                            catch { }
                        }

                    }
                    catch (Exception ex)
                    {

                        //MessageBox.Show(ex.ToString());
                    }

                    Thread.Sleep(5*1000);

                }
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        button1.Text = "Iniciar Sincronização em Tempo Real";
                    });
                }
               
            });
        }

        private async void iniciarSincronismo()
        {
            await Task.Run(() =>
            {
                
            try
            {
                port.Open();
                sincronizacaoIniciada = true;
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            button1.Text = "Parar sincronização";
                           // button1.Text = "Iniciar Sincronização em Tempo Real";
                        });
                    }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
                int term = 0;
            while (sincronizacaoIniciada)
            {
                String s = "oi";
                //if (s.Equals("exit"))
                //{
                //   break;
                //}
                port.Write(s + '\n');
                try
                {
                    string text = port.ReadLine() + "\n";
                        //MessageBox.Show(text);

                        string leitura = text.Substring(text.IndexOf("<leitura>"), text.IndexOf("</leitura>"));

                        string umidade_ar = leitura.Substring(leitura.IndexOf("<umidade_ar>"));
                        umidade_ar = ("umidade_ar=" + Regex.Match(umidade_ar, @"\d+").Value);

                        string umidade_terra = leitura.Substring(leitura.IndexOf("<umidade_terra>"));
                        umidade_terra = ("&umidade_terra=" + Regex.Match(umidade_terra, @"\d+").Value);
                        

                        string temperatura = leitura.Substring(leitura.IndexOf("<temperatura>"));
                        temperatura = ("&temperatura=" + Regex.Match(temperatura, @"\d+").Value);

                        //string luminosidade // leitura.Substring(leitura.IndexOf("<luminosidade>"));
                         // = ""; // leitura.Substring(leitura.IndexOf("<luminosidade>"));
                        string luminosidade = ("&luminosidade=" + 0);
                        

                        string direcao_vento = leitura.Substring(leitura.IndexOf("<direcao_vento>"));
                        direcao_vento = ("&direcao_vento=" + Regex.Match(direcao_vento, @"\d+").Value);
                       


                        string velocidade_vento = leitura.Substring(leitura.IndexOf("<velocidade_vento>"));
                        velocidade_vento = ("&velocidade_vento=" + Regex.Match(velocidade_vento, @"\d+").Value);

                        string terminal = "&terminal=" + term++;
                        if (term > 5) term = 0;

                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        string tempo = "&timestamp=" + unixTimestamp;


                        string request = (PREFIX_URL + umidade_ar + umidade_terra + temperatura + luminosidade + direcao_vento + velocidade_vento + terminal + tempo);

                        Stream data = client.OpenRead(request);
                    StreamReader reader = new StreamReader(data);
                    string response = reader.ReadToEnd();
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            richTextBox1.Text += "\n" + (response);
                        });
                    }
                    data.Close();
                    reader.Close();

                }
                catch (Exception ex) {

                   // MessageBox.Show(ex.ToString());
                }

                Thread.Sleep(30000);

            }
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        button1.Text = "Iniciar Sincronização em Tempo Real";
                    });
                }
                port.Close();
        });
      }

        private void mostrarInterface(object sender, EventArgs e)
        {
            this.Visible = true;
        }
    }
}
