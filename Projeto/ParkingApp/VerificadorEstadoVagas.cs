using Android.App;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingApp
{
    internal class VerificadorEstadoVagas
    {
        public  Vaga VagaEscolhida { get; internal set; }
        public bool VerificacaoVagasIniciada = false;
        public bool ContinuarVerificacaoVagas = true;

        public  bool ContinuarVerificacaoVagaEscolhida = true;
        public  bool VerificacaoVagaEscolhidaIniciada = false;
        private AtividadeMapa AtividadePai = null;

        public VerificadorEstadoVagas(AtividadeMapa atividadePai)
        {
            AtividadePai = atividadePai;
        }

        public  void VerificacaoVagaEscolhida()
        {
            if (VerificacaoVagaEscolhidaIniciada) return;
            VerificacaoVagaEscolhidaIniciada = true;
            Task  rotinaVerificacao = new Task(() =>
            {
                while(1==1)
                {
                    while (!ContinuarVerificacaoVagaEscolhida)
                    {
                        Thread.Sleep(500);
                    }
                    try
                    {
                        JObject vaga = null;
                        using (WebClient wc = new WebClient())
                        {
                            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                            string url = ControladorMapa.ParkingManagerServerURL + "api/VagaModels/" + VagaEscolhida.Dados.Value<long>("Id");

                            string vagasJsonText = wc.DownloadString(url);
                            vaga = (JObject)JsonConvert.DeserializeObject(vagasJsonText);

                            var ocupacao = vaga["Ocupacao"];
                            var dadosAnteriores = VagaEscolhida.Dados;
                            VagaEscolhida.Dados = vaga;
                            if (ocupacao.Type != JTokenType.Null)
                            {
                                CustomEventHandlerMudancaEstadoVaga handler = VagaEscolhidaMudouEstadoEvent;
                                if (handler != null)
                                {
                                    AtividadePai.RunOnUiThread(() =>
                                    {
                                        handler(null, new EventArgsMudancaEstadoVaga(VagaEscolhida,vaga, (JObject)dadosAnteriores));
                                    });
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }


                    Thread.Sleep(3000);
                }
            }
            );

            rotinaVerificacao.Start();
        }


        public void VerificacaoVagas()
        {
            if (VerificacaoVagasIniciada) return;
            VerificacaoVagasIniciada = true;
            Task rotinaVerificacao = new Task(() =>
            {
                while (1 == 1)
                {
                    while (!ContinuarVerificacaoVagas)
                    {
                        Thread.Sleep(500);
                    }
                    try { 
                    if (AtividadePai.ControleMapa.VagasColocadas != null && AtividadePai.ControleMapa.VagasColocadas.Count > 0)
                    {
                        Parallel.ForEach(AtividadePai.ControleMapa.VagasColocadas, new ParallelOptions() { MaxDegreeOfParallelism = -1 }, (VagaAnalisar) =>
                    {
                        try
                        {
                            JObject vaga = null;
                            using (WebClient wc = new WebClient())
                            {
                                wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                                string url = ControladorMapa.ParkingManagerServerURL + "api/VagaModels/" + VagaAnalisar.Dados.Value<long>("Id");

                                string vagasJsonText = wc.DownloadString(url);
                                vaga = (JObject)JsonConvert.DeserializeObject(vagasJsonText);

                                var ocupacao = vaga["Ocupacao"];
                                var dadosAnteriores = VagaAnalisar.Dados;
                                VagaAnalisar.Dados = vaga;

                                if ((ocupacao.Type != dadosAnteriores["Ocupacao"].Type) || (ocupacao.Type != dadosAnteriores["Reserva"].Type))
                                {

                                    CustomEventHandlerMudancaEstadoVaga handler = VagaMudouEstadoEvent;
                                    if (handler != null)
                                    {
                                        AtividadePai.RunOnUiThread(() =>
                                        {
                                            handler(null, new EventArgsMudancaEstadoVaga(VagaAnalisar, vaga, (JObject)dadosAnteriores));
                                        });
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }


                        Thread.Sleep(3000);
                    }
                    );
                        }else
                        {
                            Thread.Sleep(500);
                        }
                }catch(Exception ex)
                    {

                    }
                }
            }
            );

            rotinaVerificacao.Start();
        }


        public static event CustomEventHandlerMudancaEstadoVaga VagaEscolhidaMudouEstadoEvent;

        public static event CustomEventHandlerMudancaEstadoVaga VagaMudouEstadoEvent;


    }
    public delegate void CustomEventHandlerMudancaEstadoVaga(object sender, EventArgsMudancaEstadoVaga e);
    public class EventArgsMudancaEstadoVaga: EventArgs
    {
        public Vaga Vaga { get; private set; }
        public JObject Dados { get; private set; }
        public JObject DadosAnteriores { get; private set; }
        public EventArgsMudancaEstadoVaga(Vaga vaga,JObject dadosNovos, JObject dadosAnteriores)
        {
            this.Dados = dadosNovos;
            this.DadosAnteriores = dadosAnteriores;
            this.Vaga = vaga;
        }

    }


}