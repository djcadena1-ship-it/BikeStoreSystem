using Microsoft.AspNetCore.Mvc;
using BikeStore.Web.Services;

namespace BikeStore.Web.Controllers
{
    public class BicicletasController : Controller
    {
        private readonly BicicletaApiService _apiService;

        public BicicletasController(BicicletaApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Bicicletas?marca=...&categoria=...
        public async Task<IActionResult> Index(string? marca, string? categoria)
        {
            var bicicletas = (marca != null || categoria != null)
                ? await _apiService.BuscarAsync(marca, categoria)
                : await _apiService.ObtenerTodasAsync();

            ViewBag.MarcaFiltro = marca;
            ViewBag.CategoriaFiltro = categoria;

            return View(bicicletas);
        }
    }
}
