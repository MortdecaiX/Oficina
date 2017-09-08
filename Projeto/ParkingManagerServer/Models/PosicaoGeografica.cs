namespace ParkingManagerServer.Models
{
    public class PosicaoGeografica
    {

        public PosicaoGeografica() { }

        public PosicaoGeografica(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}