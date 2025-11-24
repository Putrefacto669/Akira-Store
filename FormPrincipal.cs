using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Akira_Store
{
    public partial class FormPrincipal : Form
    {
        private string connectionString = "Server=DESKTOP-9U57LTA;Database=TiendaVinilos;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            CargarVinilos();
        }
        private void CargarVinilos()
        {
            string query = "SELECT * FROM Vinilos";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvVinilos.DataSource = dt;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Vinilos (Titulo, Artista, Genero, Precio, Stock, Imagen) " +
                   "VALUES (@Titulo, @Artista, @Genero, @Precio, @Stock, @Imagen)";

            // Diccionario que mapea títulos a nombres de archivo de imagen
            Dictionary<string, string> mapaImagenes = new Dictionary<string, string>
    {
        { "Dirt", "album1.jpg" },
        { "White Pony", "album2.jpg" },
        { "Bocanada", "album3.jpg" },
        { "Cancion Animal", "album5.jpg" },
        { "Roots", "album6.jpg" },
        { "The Dark Side Of The Moon", "album15.jpg" },
        { "Demon Dayz", "album17.jpg" },
        { "OK COMPUTER", "album18.jpg" },
        { "Hybrid Theory", "album16.jpg" },
        { "Ten", "album7.jpg" },
        { "Life Is Peachy", "album11.jpg" },
        { "Fallen", "album12.jpg" },
        { "Thriller", "album13.jpg" },
        { "Vulgar Display Of Power", "album10.jpg" },
        { "Shed", "album21.jpg" } ,
        { "Purple", "album23.jpg" },
        { "jar", "album22.jpg" },
        { "Electric Heart", "album20.jpg" },
        { "Let Go", "album19.jpg" },
        { "Random Access Memories", "album14.jpg" },
        { "Ride The Lightning", "album8.jpg" },
        { "In Utero", "album4.jfif" },
        { "Meteora", "album24.jpg" },
        { "Korn", "album25.jpg" },
        { "Paranoid", "album26.jpg" },
        { "IOWA", "album27.jpg" },
        { "BadMotorFinger", "album28.jpg" },
        { "Artaud", "album29.jpg" },
        { "Amor Amarillo", "album30.jpg" },

        // Puedes seguir agregando más títulos aquí
    };

            // Determinar el nombre de la imagen según el título
            string nombreImagen;
            if (!mapaImagenes.TryGetValue(txtTitulo.Text, out nombreImagen))
            {
                // Si el título no está en el diccionario, usar un valor por defecto
                nombreImagen = "default.jpg";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                cmd.Parameters.AddWithValue("@Artista", txtArtista.Text);
                cmd.Parameters.AddWithValue("@Genero", cmbGenero.Text);
                cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@Stock", int.Parse(txtStock.Text));
                cmd.Parameters.AddWithValue("@Imagen", nombreImagen);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Vinilo guardado correctamente");
            CargarVinilos();


        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Vinilos SET Titulo=@Titulo, Artista=@Artista, Genero=@Genero, " +
                       "Precio=@Precio, Stock=@Stock, Imagen=@Imagen WHERE IdVinilo=@IdVinilo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                cmd.Parameters.AddWithValue("@Artista", txtArtista.Text);
                cmd.Parameters.AddWithValue("@Genero", cmbGenero.Text);
                cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@Stock", int.Parse(txtStock.Text));
                cmd.Parameters.AddWithValue("@Imagen", txtTitulo.Text.Replace("Bocanada ", "album3").ToLower() + ".jpg");
                cmd.Parameters.AddWithValue("@IdVinilo", int.Parse(txtId.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Vinilo actualizado correctamente");
            CargarVinilos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Debes seleccionar un vinilo para eliminar.");
                return;
            }

            if (!int.TryParse(txtId.Text, out int idVinilo))
            {
                MessageBox.Show("El Id del vinilo no tiene el formato correcto.");
                return;
            }

            string query = "DELETE FROM Vinilos WHERE IdVinilo=@IdVinilo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdVinilo", idVinilo);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Vinilo eliminado correctamente");
            CargarVinilos();


        }

        private void dgvVinilos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvVinilos.Rows[e.RowIndex];

                // Asignar valores a los controles de texto
                txtId.Text = row.Cells["IdVinilo"].Value.ToString();
                txtTitulo.Text = row.Cells["Titulo"].Value.ToString();
                txtArtista.Text = row.Cells["Artista"].Value.ToString();
                cmbGenero.Text = row.Cells["Genero"].Value.ToString();
                txtPrecio.Text = row.Cells["Precio"].Value.ToString();
                txtStock.Text = row.Cells["Stock"].Value.ToString();

                // Obtener el nombre del archivo de imagen desde la BD
                string nombreImagen = row.Cells["Imagen"].Value.ToString();

                // Carpeta donde guardas las imágenes
                string rutaImagenes = @"C:\Users\josue\Desktop\Tienda Vinilos\Imagenes";

                // Construir la ruta completa
                string rutaCompleta = Path.Combine(rutaImagenes, nombreImagen);

                
                MessageBox.Show("Buscando imagen en: " + rutaCompleta);

                // Verificar si el archivo existe y cargarlo en el PictureBox
                if (File.Exists(rutaCompleta))
                {
                    picAlbum.Image = Image.FromFile(rutaCompleta);
                }
                else
                {
                    // Si no existe, mostrar una imagen por defecto
                    string rutaDefault = Path.Combine(rutaImagenes, "default.jpg");
                    if (File.Exists(rutaDefault))
                    {
                        picAlbum.Image = Image.FromFile(rutaDefault);
                    }
                    else
                    {
                        picAlbum.Image = null;
                    }
                }
            }
        }

        private void gestionDeClientesToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            FormClientes frm = new FormClientes();
            frm.Show();
            
        }

        private void registroDeVentasToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            RegistroForm frm = new RegistroForm();
            frm.Show();
        }
    }
}
