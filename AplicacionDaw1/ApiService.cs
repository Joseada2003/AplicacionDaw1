using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using AplicacionDaw1.Models;

namespace AplicacionDaw1
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://dragonball-api.com/api/");
        }

        public async Task<List<Character>> GetCharactersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("characters");
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                // La API devuelve un objeto con propiedad "items" que contiene el array
                var apiResponse = JsonSerializer.Deserialize<JsonElement>(json);

                if (apiResponse.TryGetProperty("items", out var itemsElement))
                {
                    var characters = JsonSerializer.Deserialize<List<Character>>(itemsElement.GetRawText(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return characters ?? new List<Character>();
                }

                return new List<Character>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Character>();
            }
        }
    }
}