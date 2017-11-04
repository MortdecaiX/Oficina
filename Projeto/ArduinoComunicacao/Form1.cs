using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            textBox1.Text = Properties.Settings.Default.IdVaga;
            textBox2.Text = Properties.Settings.Default.PortaCOM;
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

                BeginInvoke(new Action(() =>
                {

                    richTextBox1.Text += text;

                    //"http://parkingmanagerserver.azurewebsites.net/Help
                    using (WebClient wb = new WebClient())
                    {
                        wb.Headers.Add("Content-Type", "application/json");
                        wb.Headers.Add(HttpRequestHeader.Accept, "application/json");

                        string endereco = "http://parkingmanagerserver.azurewebsites.net/api/VagaModels/" + textBox1.Text + "/ModificarEstado/" + xm.DocumentElement["estado"].InnerText;


                        string resultado = wb.DownloadString(endereco);
                        //faz alguma coisa om o resultado a atualização aqui

                    }


                }));



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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IdVaga = textBox1.Text;
            Properties.Settings.Default.Save();
        }
    }
}
