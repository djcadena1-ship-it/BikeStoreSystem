using System.Text;
using System.Text.Json;
using BikeStore.Web.Models;

namespace BikeStore.Web.Services
{
    public class BicicletaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public BicicletaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Bicicleta>> ObtenerTodasAsync()
        {
            var response = await _httpClient.GetAsync("api/bicicletas");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Bicicleta>>(json, _jsonOptions) ?? new List<Bicicleta>();
        }

        public async Task<Bicicleta?> ObtenerPorIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/bicicletas/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Bicicleta>(json, _jsonOptions);
        }

        public async Task<List<Bicicleta>> BuscarAsync(string? marca, string? categoria)
        {
            var todas = await ObtenerTodasAsync();

            var filtradas = todas.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(marca))
                filtradas = filtradas.Where(b => b.Marca.Contains(marca, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(categoria))
                filtradas = filtradas.Where(b => b.IdCategoria.ToString() == categoria);

            return filtradas.ToList();
        }

        public async Task<bool> RegistrarAsync(Bicicleta bicicleta)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(bicicleta), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/bicicletas", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(int id, Bicicleta bicicleta)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(bicicleta), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/bicicletas/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/bicicletas/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}