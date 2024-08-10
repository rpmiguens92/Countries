using Countries.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Countries.Services
{
    public class APIService
    {

        public async Task<Response> GetCountries(string urlBase, string controller)//metodo assincrono e devolve objecto "response"
        {
        

            try
            { 
                var client = new HttpClient(); //liga se à api
                client.BaseAddress = new Uri(urlBase);//liga se ao controlador

                var response = await client.GetAsync(controller); //fica à espera de uma resposta

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response // se nao se consegue ligar devolve a resposta e sai
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }
                var countries = JsonConvert.DeserializeObject<List<Country>>(result);
                //se tudo corre bem converte a lista
                return new Response
                {
                    IsSuccess = true,
                    Result = countries,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}