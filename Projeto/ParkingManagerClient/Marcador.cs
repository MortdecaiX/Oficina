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
using Android.Gms.Maps.Model;
using Newtonsoft.Json.Linq;

namespace ParkingManagerClient
{
   public class Marcador
    {
        public Marcador(JObject estacionamento, Marker marker)
        {
            Estacionamento = estacionamento;
            Marker = marker;
        }
        public long Id { get; set; }
        public List<long> Conexoes = new List<long>();
        public JObject Estacionamento { get; set; }
        public Marker Marker { get; set; }
        private List<Polyline> _Linhas = new List<Polyline>();
        public List<Polyline> Linhas { get { return _Linhas; } set { _Linhas = value; } }
    }
}