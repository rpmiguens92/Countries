using Countries.Classes;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Countries.Services
{
    public class LoadAsync
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiUrl = "https://restcountries.com/v3.1/all";

        //public async Task<List<Country>> LoadingAsync()
        //{
        //    //var response = await client.GetAsync(apiUrl);
        //    //var countries = JsonConvert.DeserializeObject<List<Country>>(response);
        //    //return countries;
        //}
    }
}
