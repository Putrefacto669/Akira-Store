using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Akira_Store
{
    public partial class RegistroForm : Form
    {
        private string connectionString = "Server=DESKTOP-9U57LTA;Database=TiendaVinilos;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";
        private SqlConnection conexion;
        DataTable carrito = new DataTable();
        public RegistroForm()
        {
            InitializeComponent();
            ConfigurarCarrito();

        }
        private void ConfigurarCarrito()
        {
            // NOMBRES SIMPLES Y CONSISTENTES
            carrito.Columns.Add("IdVinilo", typeof(int));
            carrito.Columns.Add("Album", typeof(string));
            carrito.Columns.Add("Artista", typeof(string));
            carrito.Columns.Add("Cantidad", typeof(int));
            carrito.Columns.Add("Precio", typeof(decimal));
            carrito.Columns.Add("Subtotal", typeof(decimal));

            dgvCarrito.DataSource = carrito;
            ConfigurarGridCarrito();
        }

        private void ConfigurarGridCarrito()
        {
            dgvCarrito.Columns["IdVinilo"].Visible = false;
            dgvCarrito.Columns["Album"].HeaderText = "Álbum";
            dgvCarrito.Columns["Artista"].HeaderText = "Artista";
            dgvCarrito.Columns["Cantidad"].HeaderText = "Cantidad";
            dgvCarrito.Columns["Precio"].HeaderText = "Precio Unitario";
            dgvCarrito.Columns["Subtotal"].HeaderText = "Subtotal";
        }


        private void RegistroForm_Load(object sender, EventArgs e)
        {
            CargarCatalogoVinilos();
            CargarClientes();
        }
        private void CargarClientes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT IdCliente, Nombre FROM Clientes";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbClientes.DataSource = dt;
                    cmbClientes.DisplayMember = "Nombre";
                    cmbClientes.ValueMember = "IdCliente";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }
        private void CargarCatalogoVinilos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT IdVinilo, Titulo, Artista, Precio, Stock FROM Vinilos";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvCatalogo.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar catálogo: " + ex.Message);
            }
        }
        

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // 1️⃣ Validar selección
            if (dgvCatalogo.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un vinilo del catálogo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2️⃣ Obtener datos del vinilo seleccionado
            var fila = dgvCatalogo.CurrentRow;
            int idVinilo = Convert.ToInt32(fila.Cells["IdVinilo"].Value);
            string titulo = fila.Cells["Titulo"].Value.ToString();
            string artista = fila.Cells["Artista"].Value.ToString();
            decimal precio = Convert.ToDecimal(fila.Cells["Precio"].Value);
            int stock = Convert.ToInt32(fila.Cells["Stock"].Value);
            int cantidad = (int)nuCantidad.Value;

            // 3️⃣ Validar cantidad
            if (!ValidarCantidad(cantidad, stock)) return;

            // 4️⃣ Agregar o actualizar en el carrito
            if (CarritoContieneVinilo(idVinilo, out DataRow rowExistente))
            {
                // Aumentar cantidad y recalcular subtotal
                int nuevaCantidad = Convert.ToInt32(rowExistente["Cantidad"]) + cantidad;

                if (nuevaCantidad > stock)
                {
                    MessageBox.Show($"Stock insuficiente. Disponible: {stock}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                rowExistente["Cantidad"] = nuevaCantidad;
                rowExistente["Subtotal"] = nuevaCantidad * precio;
            }
            else
            {
                // Agregar nuevo item al carrito
                carrito.Rows.Add(idVinilo, titulo, artista, cantidad, precio, cantidad * precio);
            }

            // 5️⃣ Recalcular total
            CalcularTotal();
        }
        private bool ValidarCantidad(int cantidad, int stock)
        {
            if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a 0.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cantidad > stock)
            {
                MessageBox.Show($"Stock insuficiente. Disponible: {stock}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private bool CarritoContieneVinilo(int idVinilo, out DataRow filaExistente)
        {
            filaExistente = null;

            foreach (DataRow row in carrito.Rows)
            {
                if (Convert.ToInt32(row["IdVinilo"]) == idVinilo)
                {
                    filaExistente = row;
                    return true;
                }
            }

            return false;
        }


        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataRow row in carrito.Rows)
                total += Convert.ToDecimal(row["Subtotal"]);
            lblTotal.Text = total.ToString("0.00");
        }
        private int RegistrarVenta(decimal total)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Ventas (Fecha, Total) OUTPUT INSERTED.IdVenta VALUES (@fecha, @total)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@total", total);

                return (int)cmd.ExecuteScalar();
            }
        }


        private void RegistrarDetalles(int idVenta)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataRow row in carrito.Rows)
                {
                    string query = "INSERT INTO DetalleVenta (IdVenta, IdVinilo, Cantidad, Precio, Subtotal) VALUES (@idVenta, @idVinilo, @cantidad, @precio, @subtotal)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@idVenta", idVenta);
                    cmd.Parameters.AddWithValue("@idVinilo", row["IdVinilo"]);
                    cmd.Parameters.AddWithValue("@cantidad", row["Cantidad"]);
                    cmd.Parameters.AddWithValue("@precio", row["Precio"]);
                    cmd.Parameters.AddWithValue("@subtotal", row["Subtotal"]);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (carrito.Rows.Count == 0)
            {
                MessageBox.Show("Debe agregar productos al carrito.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Obtener información relevante
                decimal total = Convert.ToDecimal(lblTotal.Text);
                string cliente = cmbClientes.Text;
                int cantidadItems = carrito.Rows.Count;
                 


                // Registrar venta y detalles
                int idVenta = RegistrarVenta(total);
                RegistrarDetalles(idVenta);

                // Mostrar mensaje profesional con estilo musical
                StringBuilder mensaje = new StringBuilder();
                mensaje.AppendLine("🎸 COMPRA REGISTRADA EXITOSAMENTE 🎸");
                mensaje.AppendLine("=====================================");
                mensaje.AppendLine($"📋 Venta N°: #{idVenta:D4}");
                mensaje.AppendLine($"👤 Cliente: {cliente}");
                mensaje.AppendLine($"📅 {DateTime.Now:dd/MM/yyyy HH:mm}");
                mensaje.AppendLine("-------------------------------------");
                mensaje.AppendLine("🎵 **TU SELECCIÓN MUSICAL:**");
                mensaje.AppendLine();

                foreach (DataRow item in carrito.Rows)
                {
                    string artista = item["Artista"].ToString();
                    string album = item["Album"].ToString();
                    int cantidad = Convert.ToInt32(item["Cantidad"]);

                    mensaje.AppendLine($"   🎶 {artista}");
                    mensaje.AppendLine($"      \"{album}\" x{cantidad}");
                    mensaje.AppendLine();
                }

                mensaje.AppendLine("-------------------------------------");
                mensaje.AppendLine($"💰 TOTAL: ${total:0.00}");
                mensaje.AppendLine();
                mensaje.AppendLine("❤️  Gracias por elegir Akira Store");
                mensaje.AppendLine("    Donde la música vive...");

                MessageBox.Show(
                    mensaje.ToString(),
                    "🎵 Compra Completada - Akira Store",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Limpiar carrito y actualizar total
                carrito.Rows.Clear();
                CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la venta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            carrito.Rows.Clear();
            CalcularTotal();
        }
    }
}
