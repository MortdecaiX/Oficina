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

        public JObject Estacionamento { get; set; }
        public Marker Marker { get; set; }
        public Polyline Linha { get; set; }
    }
}