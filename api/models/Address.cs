public class Address
{
    public string Postcode { get; set; }
    public string FullAddress { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double DistanceToHeathrowAirportKm { get; set; }
    public double DistanceToHeathrowAirportMiles { get; set; }
    
    private double HeathrowAirportLatitude = 51.470022;
    private double HeathrowAirportLongitude = -0.454295;
    
    private static double HaversineDistance(double lat1, double lat2, double lon1, double lon2)
    {
        const double r = 6378100;
            
        var sdlat = Math.Sin((lat2 - lat1) / 2);
        var sdlon = Math.Sin((lon2 - lon1) / 2);
        var q = sdlat * sdlat + Math.Cos(lat1) * Math.Cos(lat2) * sdlon * sdlon;
        var d = 2 * r * Math.Asin(Math.Sqrt(q));

        return d;
    }
    private void CalculateDistanceToHeathrowAirport()
    {
        DistanceToHeathrowAirportKm = HaversineDistance(Latitude, HeathrowAirportLatitude, Longitude, HeathrowAirportLongitude) / 1000;
        DistanceToHeathrowAirportMiles = DistanceToHeathrowAirportKm * 0.621371;
    }
    public Address(string postcode, string fullAddress,  double latitude, double longitude)
    {
        Postcode = postcode;
        FullAddress = fullAddress;
        Latitude = latitude;
        Longitude = longitude;
        CalculateDistanceToHeathrowAirport();
    }
}