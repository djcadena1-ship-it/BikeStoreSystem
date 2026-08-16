using Microsoft.AspNetCore.Mvc;
using BikeStore.Web.Services;
using BikeStore.Web.Models;

namespace BikeStore.Web.Controllers
{
    public class VentasController : Controller
    {
        private readonly VentaApiService _ventaService;
        private readonly ClienteApiService _clienteService;
        private readonly BicicletaApiService _bicicletaService;

        public VentasController(
            VentaApiService ventaService,
            ClienteApiService clienteService,
            BicicletaApiService bicicletaService)
        {
            _ventaService = ventaService;
            _clienteService = clienteService;
            _bicicletaService = bicicletaService;
        }

        // GET: /Ventas  -> historial de ventas
        public async Task<IActionResult> Index()
        {
            var ventas = await _ventaService.ObtenerTodasAsync();
            return View(ventas);
        }

        // GET: /Ventas/Crear -> muestra el formulario de facturación
        public async Task<IActionResult> Crear()
        {
            ViewBag.Clientes = await _clienteService.ObtenerTodosAsync();
            ViewBag.Bicicletas = await _bicicletaService.ObtenerTodasAsync();
            return View();
        }

        // POST: /Ventas/Crear -> procesa la venta
        [HttpPost]
        public async Task<IActionResult> Crear(RegistrarVentaRequest request)
        {
            var (exito, error, resultado) = await _ventaService.RegistrarAsync(request);

            if (!exito)
            {
                ViewBag.Clientes = await _clienteService.ObtenerTodosAsync();
                ViewBag.Bicicletas = await _bicicletaService.ObtenerTodasAsync();
                ModelState.AddModelError("", error ?? "No se pudo registrar la venta.");
                return View();
            }

            TempData["Mensaje"] = "Venta registrada con éxito.";
            return RedirectToAction(nameof(Index));
        }
    }
}
