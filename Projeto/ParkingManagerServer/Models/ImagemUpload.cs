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

        public float centerLat { get; set; }
        public float centerLng { get; set; }

        public float swBoundLat { get; set; }
        public float swBoundLng { get; set; }
        public float neBoundLat { get; set; }
        public float neBoundLng { get; set; }
        public string url { get; set; }
    }
}