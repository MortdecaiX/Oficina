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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!sincronizacaoIniciada)
            {
                //iniciarSincronismo();
                port = new SerialPort(textBox2.Text, 9600);
                // iniciarSincronismoSimulado();
                port.DataReceived += new SerialDataReceivedEventHandler(TratarDados);
                try
                {
                    port.Open();
                }
                catch (Exception ex)
                {
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

        private void TratarDados(object sender, SerialDataReceivedEventArgs e)
        {
            string text = ((SerialPort)sender).ReadLine() + "\n";//"<vaga><id>1</id><estado>1</estado></vaga>"
            XmlDocument xm = new XmlDocument();
            xm.LoadXml(text);

            BeginInvoke(new Action(() => {
                
                richTextBox1.Text = text;

                //"http://parkingmanagerserver.azurewebsites.net/Help



            }));

            
            
                using (WebClient wb = new WebClient())
                {
                    wb.Headers.Add("Content-Type", "application/json");
                    string endereco = "http://parkingmanagerserver.azurewebsites.net/api/VagaModels/" + xm.DocumentElement["id"].Value + "/ModificarEstado/"+ xm.DocumentElement["estado"].Value;
                    
                    string resultado = wb.DownloadString(endereco);
                    //faz alguma coisa om o resultado a atualização aqui

                }
            

        }
    }
}
