
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace BikeStore.Datos
{
    public class CategoriaRepository
    {
        private readonly string _cadenaConexion;

        public CategoriaRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        // GET: Listar todas las categorías
        public List<Categoria> ObtenerCategorias()
        {
            var lista = new List<Categoria>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                string query = @"SELECT IdCategoria, Nombre, Descripcion, Activo
                                 FROM Categoria";

                var cmd = new SqlCommand(query, conexion);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Categoria
                        {
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"] != DBNull.Value
                                ? dr["Descripcion"].ToString()
                                : string.Empty,
                            Activo = Convert.ToBoolean(dr["Activo"])
                        });
                    }
                }
            }

            return lista;
        }

        // GET: Obtener una categoría por ID
        public Categoria ObtenerPorId(int idCategoria)
        {
            Categoria categoria = null;

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                string query = @"SELECT IdCategoria, Nombre, Descripcion, Activo
                                 FROM Categoria
                                 WHERE IdCategoria = @IdCategoria";

                var cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        categoria = new Categoria
                        {
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"] != DBNull.Value
                                ? dr["Descripcion"].ToString()
                                : string.Empty,
                            Activo = Convert.ToBoolean(dr["Activo"])
                        };
                    }
                }
            }

            return categoria;
        }

        // POST: Registrar categoría
        public bool Registrar(Categoria oCategoria)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                string query = @"INSERT INTO Categoria
                                 (Nombre, Descripcion, Activo)
                                 VALUES
                                 (@Nombre, @Descripcion, @Activo)";

                var cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@Nombre", oCategoria.Nombre);
                cmd.Parameters.AddWithValue(
                    "@Descripcion",
                    oCategoria.Descripcion ?? (object)DBNull.Value
                );
                cmd.Parameters.AddWithValue("@Activo", oCategoria.Activo);

                int filasAfectadas = cmd.ExecuteNonQuery();

                return filasAfectadas > 0;
            }
        }

        // PUT: Actualizar categoría
        public bool Actualizar(Categoria oCategoria)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                string query = @"UPDATE Categoria
                                 SET Nombre = @Nombre,
                                     Descripcion = @Descripcion,
                                     Activo = @Activo
                                 WHERE IdCategoria = @IdCategoria";

                var cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue(
                    "@IdCategoria",
                    oCategoria.IdCategoria
                );

                cmd.Parameters.AddWithValue(
                    "@Nombre",
                    oCategoria.Nombre
                );

                cmd.Parameters.AddWithValue(
                    "@Descripcion",
                    oCategoria.Descripcion ?? (object)DBNull.Value
                );

                cmd.Parameters.AddWithValue(
                    "@Activo",
                    oCategoria.Activo
                );

                int filasAfectadas = cmd.ExecuteNonQuery();

                return filasAfectadas > 0;
            }
        }

        // DELETE: Eliminar categoría
        public bool Eliminar(int idCategoria)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                string query = @"DELETE FROM Categoria
                                 WHERE IdCategoria = @IdCategoria";

                var cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue(
                    "@IdCategoria",
                    idCategoria
                );

                int filasAfectadas = cmd.ExecuteNonQuery();

                return filasAfectadas > 0;
            }
        }
    }
}

