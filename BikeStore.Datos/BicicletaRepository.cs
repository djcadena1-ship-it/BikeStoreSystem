
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace BikeStore.Datos
{
    public class BicicletaRepository
    {
        private readonly string _cadenaConexion;

        public BicicletaRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        // GET: Listar todas las bicicletas
        public List<Bicicleta> ObtenerBicicletas()
        {
            var lista = new List<Bicicleta>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                string query = @"SELECT IdBicicleta,
                                        IdCategoria,
                                        Marca,
                                        Modelo,
                                        Precio,
                                        Stock,
                                        Estado
                                 FROM Bicicleta";

                var cmd = new SqlCommand(query, conexion);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Bicicleta
                        {
                            IdBicicleta = Convert.ToInt32(dr["IdBicicleta"]),
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                            Marca = dr["Marca"].ToString(),
                            Modelo = dr["Modelo"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"]),
                            Stock = Convert.ToInt32(dr["Stock"]),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        // GET: Obtener una bicicleta por ID
        public Bicicleta ObtenerPorId(int idBicicleta)
        {
            Bicicleta bicicleta = null;

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                string query = @"SELECT IdBicicleta,
                                        IdCategoria,
                                        Marca,
                                        Modelo,
                                        Precio,
                                        Stock,
                                        Estado
                                 FROM Bicicleta
                                 WHERE IdBicicleta = @IdBicicleta";

                var cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue(
                    "@IdBicicleta",
                    idBicicleta
                );

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        bicicleta = new Bicicleta
                        {
                            IdBicicleta = Convert.ToInt32(dr["IdBicicleta"]),
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                            Marca = dr["Marca"].ToString(),
                            Modelo = dr["Modelo"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"]),
                            Stock = Convert.ToInt32(dr["Stock"]),
                            Estado = dr["Estado"].ToString()
                        };
                    }
                }
            }

            return bicicleta;
        }

        
        // POST: Registrar una bicicleta
        public bool Registrar(Bicicleta oBicicleta)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                // 1. Verificar que la categoría exista
                string queryCategoria =
                    "SELECT COUNT(1) FROM Categoria WHERE IdCategoria = @IdCategoria";

                using (var cmdCategoria = new SqlCommand(queryCategoria, conexion))
                {
                    cmdCategoria.Parameters.AddWithValue(
                        "@IdCategoria",
                        oBicicleta.IdCategoria
                    );

                    int existeCategoria = Convert.ToInt32(
                        cmdCategoria.ExecuteScalar()
                    );

                    // Si la categoría no existe, NO hacemos el INSERT
                    if (existeCategoria == 0)
                    {
                        return false;
                    }
                }

                // 2. Registrar la bicicleta
                string query =
                    @"INSERT INTO Bicicleta
              (IdCategoria, Marca, Modelo, Precio, Stock, Estado)
              VALUES
              (@IdCategoria, @Marca, @Modelo, @Precio, @Stock, @Estado)";

                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue(
                        "@IdCategoria",
                        oBicicleta.IdCategoria
                    );

                    cmd.Parameters.AddWithValue(
                        "@Marca",
                        oBicicleta.Marca
                    );

                    cmd.Parameters.AddWithValue(
                        "@Modelo",
                        oBicicleta.Modelo
                    );

                    cmd.Parameters.AddWithValue(
                        "@Precio",
                        oBicicleta.Precio
                    );

                    cmd.Parameters.AddWithValue(
                        "@Stock",
                        oBicicleta.Stock
                    );

                    cmd.Parameters.AddWithValue(
                        "@Estado",
                        oBicicleta.Estado
                    );

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    return filasAfectadas > 0;
                }
            }
        }



        
            // PUT: Actualizar una bicicleta
        public bool Actualizar(Bicicleta oBicicleta)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                // 1. Verificar que la categoría exista
                string queryCategoria =
                    "SELECT COUNT(*) FROM Categoria WHERE IdCategoria = @IdCategoria";

                var cmdCategoria = new SqlCommand(queryCategoria, conexion);

                cmdCategoria.Parameters.AddWithValue(
                    "@IdCategoria",
                    oBicicleta.IdCategoria
                );

                int categoriaExiste = Convert.ToInt32(
                    cmdCategoria.ExecuteScalar()
                );

                // Si la categoría no existe, no actualizamos la bicicleta
                if (categoriaExiste == 0)
                    return false;

                // 2. Actualizar la bicicleta
                string query =
                    @"UPDATE Bicicleta
              SET IdCategoria = @IdCategoria,
                  Marca = @Marca,
                  Modelo = @Modelo,
                  Precio = @Precio,
                  Stock = @Stock,
                  Estado = @Estado
              WHERE IdBicicleta = @IdBicicleta";

                var cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue(
                    "@IdBicicleta",
                    oBicicleta.IdBicicleta
                );

                cmd.Parameters.AddWithValue(
                    "@IdCategoria",
                    oBicicleta.IdCategoria
                );

                cmd.Parameters.AddWithValue(
                    "@Marca",
                    oBicicleta.Marca
                );

                cmd.Parameters.AddWithValue(
                    "@Modelo",
                    oBicicleta.Modelo
                );

                cmd.Parameters.AddWithValue(
                    "@Precio",
                    oBicicleta.Precio
                );

                cmd.Parameters.AddWithValue(
                    "@Stock",
                    oBicicleta.Stock
                );

                cmd.Parameters.AddWithValue(
                    "@Estado",
                    oBicicleta.Estado
                );

                // 3. Comprobar si realmente se actualizó
                int filasAfectadas = cmd.ExecuteNonQuery();

                return filasAfectadas > 0;
            }
        }



        // DELETE: Eliminar una bicicleta
        public bool Eliminar(int idBicicleta)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                string query = @"DELETE FROM Bicicleta
                                 WHERE IdBicicleta = @IdBicicleta";

                var cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue(
                    "@IdBicicleta",
                    idBicicleta
                );

                int filasAfectadas = cmd.ExecuteNonQuery();

                return filasAfectadas > 0;
            }
        }

        // GET: Obtener bicicletas con stock crítico
        public List<Bicicleta> ObtenerStockCritico()
        {
            var lista = new List<Bicicleta>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                string query = @"SELECT IdBicicleta,
                                        IdCategoria,
                                        Marca,
                                        Modelo,
                                        Precio,
                                        Stock,
                                        Estado
                                 FROM Bicicleta
                                 WHERE Stock <= 5";

                var cmd = new SqlCommand(query, conexion);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Bicicleta
                        {
                            IdBicicleta = Convert.ToInt32(dr["IdBicicleta"]),
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                            Marca = dr["Marca"].ToString(),
                            Modelo = dr["Modelo"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"]),
                            Stock = Convert.ToInt32(dr["Stock"]),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
