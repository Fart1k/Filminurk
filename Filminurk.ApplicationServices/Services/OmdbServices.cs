using Filminurk.Core.Dto.OmdbDTOs;
using Filminurk.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Filminurk.ApplicationServices.Services
{
    public class OmdbServices : IOmdbServices
    {
        public async Task<OmdbRootDTO> OmdbRootSearchResult(OmdbRootDTO dto)
        {
            string apiKey = Filminurk.Data.Environment.omdbkey;
            var baseUrl = "http://www.omdbapi.com/";

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
                var response = httpClient.GetAsync($"?apikey={apiKey}&t={dto.Title}").GetAwaiter().GetResult();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                try
                {
                    List<OmdbRootDTO> omdbData = JsonSerializer.Deserialize<List<OmdbRootDTO>>(jsonResponse);
                    dto.Title = omdbData[0].Title;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return JsonSerializer.Deserialize<OmdbRootDTO>(jsonResponse);
            }
        }
    }
}
