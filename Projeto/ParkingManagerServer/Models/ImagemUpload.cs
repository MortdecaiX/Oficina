using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkingManagerServer.Models
{
    public class UploadData
    {
        public string Data { get; set; }
        public string Filename{ get; set; }
        public byte[] DataToByteArray()
        {
            string dataFormated = Data.Split(',')[1];
            return Convert.FromBase64String(dataFormated);
        }

        public double centerLat { get; set; }
        public double centerLng { get; set; }

        public double swBoundLat { get; set; }
        public double swBoundLng { get; set; }
        public double neBoundLat { get; set; }
        public double neBoundLng { get; set; }
        public string url { get; set; }
    }
}