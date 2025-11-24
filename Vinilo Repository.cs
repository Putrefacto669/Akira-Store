using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akira_Store
{
    internal class ViniloRepository
    {
        private readonly SqlConnection conexion;

        public ViniloRepository(SqlConnection conn)
        {
            conexion = conn;
        }

        // Crear un nuevo vinilo
        public void AgregarVinilo(Vinilo vinilo)
        {
            string query = "INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) " +
                           "VALUES (@Titulo, @Artista, @Genero, @Precio, @Stock, @Imagen)";
            SqlCommand cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@Titulo", vinilo.Titulo);
            cmd.Parameters.AddWithValue("@Artista", vinilo.Artista);
            cmd.Parameters.AddWithValue("@Genero", vinilo.Genero);
            cmd.Parameters.AddWithValue("@Precio", vinilo.Precio);
            cmd.Parameters.AddWithValue("@Stock", vinilo.Stock);
            cmd.Parameters.AddWithValue("@Imagen", vinilo.Imagen);

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();
        }

        // Leer todos los vinilos
        public List<Vinilo> ObtenerVinilos()
        {
            List<Vinilo> lista = new List<Vinilo>();
            string query = "SELECT * FROM Vinilos";
            SqlCommand cmd = new SqlCommand(query, conexion);

            conexion.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Vinilo v = new Vinilo
                {
                    IdVinilo = Convert.ToInt32(reader["IdVinilo"]),
                    Titulo = reader["Titulo"].ToString(),
                    Artista = reader["Artista"].ToString(),
                    Genero = reader["Genero"].ToString(),
                    Precio = Convert.ToDecimal(reader["Precio"]),
                    Stock = Convert.ToInt32(reader["Stock"]),
                    Imagen = reader["Imagen"].ToString()
                };
                lista.Add(v);
            }
            conexion.Close();
            return lista;
        }

        // Actualizar vinilo
        public void ActualizarVinilo(Vinilo vinilo)
        {
            string query = "UPDATE Vinilos SET Titulo=@Titulo, Artista=@Artista, Genero=@Genero, " +
                           "Precio=@Precio, Stock=@Stock, Imagen=@Imagen WHERE IdVinilo=@IdVinilo";
            SqlCommand cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@Titulo", vinilo.Titulo);
            cmd.Parameters.AddWithValue("@Artista", vinilo.Artista);
            cmd.Parameters.AddWithValue("@Genero", vinilo.Genero);
            cmd.Parameters.AddWithValue("@Precio", vinilo.Precio);
            cmd.Parameters.AddWithValue("@Stock", vinilo.Stock);
            cmd.Parameters.AddWithValue("@Imagen", vinilo.Imagen);
            cmd.Parameters.AddWithValue("@IdVinilo", vinilo.IdVinilo);

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();
        }

        // Eliminar vinilo
        public void EliminarVinilo(int idVinilo)
        {
            string query = "DELETE FROM Vinilos WHERE IdVinilo=@IdVinilo";
            SqlCommand cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@IdVinilo", idVinilo);

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();
        }
    }
}

