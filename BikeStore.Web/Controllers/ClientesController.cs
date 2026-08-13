using Microsoft.AspNetCore.Mvc;
using BikeStore.Web.Services;
using BikeStore.Web.Models;

namespace BikeStore.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteApiService _apiService;

        public ClientesController(ClienteApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _apiService.ObtenerTodosAsync();
            return View(clientes);
        }

        // GET: /Clientes/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Clientes/Crear
        [HttpPost]
        public async Task<IActionResult> Crear(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            var (exito, error) = await _apiService.RegistrarAsync(cliente);

            if (!exito)
            {
                ModelState.AddModelError("", error ?? "No se pudo registrar el cliente.");
                return View(cliente);
            }

            TempData["Mensaje"] = "Cliente registrado con éxito.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Clientes/Eliminar/5
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _apiService.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}