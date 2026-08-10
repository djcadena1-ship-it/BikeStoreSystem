
using BikeStore.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.API.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly string _cadenaConexion;

        public CategoriasController(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("CadenaSQL");
        }

        // GET: api/categorias
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var db = new CategoriaRepository(_cadenaConexion);
                var lista = db.ObtenerCategorias();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al obtener las categorías: " + ex.Message }
                );
            }
        }

        // GET: api/categorias/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var db = new CategoriaRepository(_cadenaConexion);

                var categoria = db.ObtenerPorId(id);

                if (categoria == null)
                    return NotFound(new { mensaje = "Categoría no encontrada" });

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al obtener la categoría: " + ex.Message }
                );
            }
        }

        // POST: api/categorias
        [HttpPost]
        public IActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                var db = new CategoriaRepository(_cadenaConexion);
                bool respuesta = db.Registrar(categoria);

                if (!respuesta)
                    return BadRequest(new
                    {
                        mensaje = "No se pudo registrar la categoría."
                    });

                return StatusCode(
                    StatusCodes.Status201Created,
                    new { mensaje = "Categoría registrada con éxito" }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al registrar la categoría: " + ex.Message }
                );
            }
        }

        // PUT: api/categorias/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                categoria.IdCategoria = id;

                var db = new CategoriaRepository(_cadenaConexion);
                bool respuesta = db.Actualizar(categoria);

                if (!respuesta)
                    return NotFound(new
                    {
                        mensaje = "Error: La categoría no existe"
                    });

                return Ok(new
                {
                    mensaje = "Categoría actualizada con éxito"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al actualizar la categoría: " + ex.Message }
                );
            }
        }

        // DELETE: api/categorias/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var db = new CategoriaRepository(_cadenaConexion);
                bool respuesta = db.Eliminar(id);

                if (!respuesta)
                    return NotFound(new
                    {
                        mensaje = "Error: La categoría no existe"
                    });

                return Ok(new
                {
                    mensaje = "Categoría eliminada con éxito"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al eliminar la categoría: " + ex.Message }
                );
            }
        }
    }
}


