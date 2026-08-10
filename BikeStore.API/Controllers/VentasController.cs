using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeStore.Datos;
using BikeStore.Datos.Models;
using System.ComponentModel.DataAnnotations;

namespace BikeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            try
            {
                return await _context.Ventas
                    .Include(v => v.Cliente)
                    .Include(v => v.Detalles)
                    .ThenInclude(d => d.Bicicleta)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/ventas/cliente/5
        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentasPorCliente(int idCliente)
        {
            try
            {
                var ventas = await _context.Ventas
                    .Include(v => v.Detalles)
                    .ThenInclude(d => d.Bicicleta)
                    .Where(v => v.IdCliente == idCliente)
                    .ToListAsync();

                if (ventas == null || !ventas.Any())
                    return NotFound($"No se encontraron ventas para el cliente con ID {idCliente}.");

                return ventas;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/ventas
        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] RegistrarVentaRequest request)
        {
            if (request.Detalles == null || !request.Detalles.Any())
                return BadRequest("La venta debe incluir al menos un detalle de artículo.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cliente = await _context.Clientes.FindAsync(request.IdCliente);
                if (cliente == null)
                    return NotFound($"Cliente con ID {request.IdCliente} no encontrado.");

                decimal subtotalGeneral = 0;
                var detallesVenta = new List<DetalleVenta>();

                foreach (var item in request.Detalles)
                {
                    var bici = await _context.Bicicletas.FindAsync(item.IdBicicleta);
                    
                    if (bici == null)
                        return NotFound($"Bicicleta con ID {item.IdBicicleta} no encontrada.");

                    if (bici.Stock < item.Cantidad)
                        return BadRequest($"Stock insuficiente para la bicicleta '{bici.Marca} {bici.Modelo}'. Stock disponible: {bici.Stock}, Cantidad solicitada: {item.Cantidad}.");

                    var subtotal = item.Cantidad * item.Precio;
                    
                    detallesVenta.Add(new DetalleVenta
                    {
                        IdBicicleta = bici.IdBicicleta,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                        SubTotal = subtotal
                    });

                    subtotalGeneral += subtotal;

                    bici.Stock -= item.Cantidad;
                    if (bici.Stock == 0)
                    {
                        bici.Estado = "Agotado";
                    }
                    else if (bici.Stock < 3) 
                    {
                        bici.Estado = "Stock Bajo";
                    }
                }

                decimal iva = subtotalGeneral * 0.15m;
                decimal totalFinal = subtotalGeneral + iva;

                var nuevaVenta = new Venta
                {
                    IdCliente = request.IdCliente,
                    Fecha = DateTime.Now,
                    Total = totalFinal,
                    Detalles = detallesVenta
                };

                _context.Ventas.Add(nuevaVenta);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    Mensaje = "Venta registrada con éxito.",
                    VentaId = nuevaVenta.IdVenta,
                    SubTotalBase = subtotalGeneral,
                    Iva = iva,
                    TotalCancelado = totalFinal
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error interno al procesar la transacción de venta: {ex.Message}");
            }
        }
    }

    public class RegistrarVentaRequest
    {
        [Required]
        public int IdCliente { get; set; }

        [Required]
        public List<DetalleVentaRequest> Detalles { get; set; } = new List<DetalleVentaRequest>();
    }

    public class DetalleVentaRequest
    {
        [Required]
        public int IdBicicleta { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }
    }
}