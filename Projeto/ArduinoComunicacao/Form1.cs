using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ArduinoComunicacao
{
    public partial class Form1 : Form
    {
        SerialPort port = null;
        private bool sincronizacaoIniciada =false;

        public Form1()
        {
            InitializeComponent();
            textBox2.Text = Properties.Settings.Default.PortaCOM;
            CarregarRelacaoNumIdVaga();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!sincronizacaoIniciada)
            {
              
                
                try
                {
                    port = new SerialPort(Properties.Settings.Default.PortaCOM, 9600);

                    port.DataReceived += new SerialDataReceivedEventHandler(TratarDados);
                    port.Open();
                    sincronizacaoIniciada = true;
                    button1.Text = "Parar";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                sincronizacaoIniciada = false;
                button1.Text = "Iniciar Leitura";
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

        private void TratarDados(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string text = ((SerialPort)sender).ReadLine() + "\n";//"<vaga><numero>1</numero><estado>1</estado></vaga>"
                XmlDocument xm = new XmlDocument();
                xm.LoadXml(text);

                JArray lista = JsonConvert.DeserializeObject<JArray>(Properties.Settings.Default.RelacaoNumeroIdVaga);

                foreach (XmlNode vaga in xm.DocumentElement.ChildNodes)
                {
                    string numVaga = vaga["numero"].InnerText;
                    BeginInvoke(new Action(() =>
                    {
                        try
                        {


                            string idVaga = lista.Where(x => x.Value<string>("num") == numVaga).FirstOrDefault()["id"].Value<string>();

                            

                        //"http://parkingmanagerserver.azurewebsites.net/Help
                        using (WebClient wb = new WebClient())
                            {
                                wb.Headers.Add("Content-Type", "application/json");
                                wb.Headers.Add(HttpRequestHeader.Accept, "application/json");

                                string endereco = "http://parkingmanagerserver.azurewebsites.net/api/VagaModels/" + idVaga + "/ModificarEstado/" + vaga["estado"].InnerText;


                                string resultado = wb.DownloadString(endereco);
                                richTextBox1.Text = "[" + numVaga + "]"+ "[" + vaga["estado"].InnerText + "]" + " Ok\n"+ richTextBox1.Text;

                            }
                        }
                        catch (Exception ex)
                        {
                            richTextBox1.Text = "[" + numVaga + "]" + "[" + vaga["estado"].InnerText + "]" + " Falha\n" + richTextBox1.Text;
                        }


                    }));
                }



            }
            catch(WebException ex)
            {

            }
            catch (Exception ex)
            {

            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PortaCOM = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string num = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                JArray lista = JsonConvert.DeserializeObject<JArray>(Properties.Settings.Default.RelacaoNumeroIdVaga);


                lista.Remove(lista.Where(x => x.Value<string>("num") == num).FirstOrDefault());
                Properties.Settings.Default.RelacaoNumeroIdVaga = lista.ToString();
                Properties.Settings.Default.Save();
                CarregarRelacaoNumIdVaga();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(Properties.Settings.Default.RelacaoNumeroIdVaga))
            {
                Properties.Settings.Default.RelacaoNumeroIdVaga =  new JArray().ToString();
            }

            JArray lista = JsonConvert.DeserializeObject<JArray>(Properties.Settings.Default.RelacaoNumeroIdVaga);
            JObject item = new JObject();
            item.Add("num", tbNumVaga.Text);
            item.Add("id", tbIdVaga.Text);
            lista.Add(item);

            Properties.Settings.Default.RelacaoNumeroIdVaga = lista.ToString();
            Properties.Settings.Default.Save();
            CarregarRelacaoNumIdVaga();
        }

        private void CarregarRelacaoNumIdVaga()
        {

            
            if (string.IsNullOrEmpty(Properties.Settings.Default.RelacaoNumeroIdVaga))
            {
                Properties.Settings.Default.RelacaoNumeroIdVaga = new JArray().ToString();
            }

            JArray lista = JsonConvert.DeserializeObject<JArray>(Properties.Settings.Default.RelacaoNumeroIdVaga);

            List<object> listaObj = new List<object>();

            foreach(var item in lista)
            {
                listaObj.Add(new { Numero = item.Value<string>("num"), ID = item.Value<string>("id") });
            }
            
            dataGridView1.DataSource = listaObj;
        }
    }
}
