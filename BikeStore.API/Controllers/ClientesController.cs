using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeStore.Datos;
using BikeStore.Datos.Models;

namespace BikeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            try
            {
                return await _context.Clientes.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetClientePorId(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                    return NotFound($"Cliente con ID {id} no encontrado.");

                return cliente;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/clientes/cedula
        [HttpGet("cedula/{cedula}")]
        public async Task<ActionResult<Cliente>> GetClientePorCedula(string cedula)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula);

                if (cliente == null)
                    return NotFound($"Cliente con cédula {cedula} no encontrado.");

                return cliente;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/clientes/apellido/Perez
        [HttpGet("apellido/{apellido}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesPorApellido(string apellido)
        {
            try
            {
                var clientes = await _context.Clientes
                    .Where(c => c.Apellidos.Contains(apellido))
                    .ToListAsync();

                if (clientes == null || !clientes.Any())
                    return NotFound($"No se encontraron clientes con el apellido {apellido}.");

                return clientes;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> RegistrarCliente(Cliente cliente)
        {
            try
            {
                // Verificamos si la cedula ya existe para informar al usuario de forma clara
                if (await _context.Clientes.AnyAsync(c => c.Cedula == cliente.Cedula))
                    return BadRequest($"Ya existe un cliente con la cédula {cliente.Cedula}.");

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetClientePorId), new { id = cliente.IdCliente }, cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor al registrar el cliente: {ex.Message}");
            }
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
                return BadRequest("El ID del cliente en la URL no coincide con el del cuerpo de la petición.");

            try
            {
                var clienteExistente = await _context.Clientes.FindAsync(id);
                if (clienteExistente == null)
                    return NotFound($"Cliente con ID {id} no encontrado.");

                // Check cedula to avoid Unique constraint exception properly
                if (clienteExistente.Cedula != cliente.Cedula && 
                    await _context.Clientes.AnyAsync(c => c.Cedula == cliente.Cedula))
                {
                    return BadRequest($"Ya existe un cliente con la cédula {cliente.Cedula}.");
                }

                _context.Entry(clienteExistente).CurrentValues.SetValues(cliente);
                await _context.SaveChangesAsync();

                return Ok("Cliente actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                    return NotFound($"Cliente con ID {id} no encontrado.");

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                return Ok("Cliente eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor al eliminar el cliente: {ex.Message}");
            }
        }
    }
}
