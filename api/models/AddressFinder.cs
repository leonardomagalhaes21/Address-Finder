using Newtonsoft.Json;
public class AddressFinder
{
    private static readonly HttpClient client = new HttpClient();
    
    async public static Task<Address?> FindAddress(string postcode)
    {
        var response = await client.GetAsync($"https://api.postcodes.io/postcodes/{postcode}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        dynamic ?data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
        
        if (data?.status == 200)
        {
            double latitude = (double?) data.result.latitude ?? 0;
            double longitude = (double?) data.result.longitude ?? 0;
            string fullAddress = (string) data.result.admin_district + ", " + data.result.region + ", " + data.result.postcode + ", " + data.result.country;
            
            return new Address(postcode, fullAddress, latitude, longitude);
        }
        
        return null;
    }
}