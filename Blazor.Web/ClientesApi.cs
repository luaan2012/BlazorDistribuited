using Blazor.Web.Components.Pages;
using static System.Net.WebRequestMethods;

namespace Blazor.Web
{
    public class WeatherApiClient(HttpClient client)
    {
        private readonly HttpClient _client = client;
        private readonly string _urlUser = "http://localhost:5070/";
        private readonly string _urlClient = "http://localhost:5080/";

        public async Task<Home.Client[]> GetClientsAsync()
        {
            var result = await _client.GetFromJsonAsync<Home.Client[]>(_urlClient + "listall");
            return result;
        }
    }
}
