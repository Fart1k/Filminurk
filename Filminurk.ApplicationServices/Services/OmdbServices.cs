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
        public async Task<OmdbRootDTO> OmdbRootSearchResult(string title)
        {
            string apiKey = Filminurk.Data.Environment.omdbkey;
            var baseUrl = "http://www.omdbapi.com/";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{baseUrl}?apikey={apiKey}&t={title}");
                var jsonResponse = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<OmdbRootDTO>(jsonResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

    }
}
