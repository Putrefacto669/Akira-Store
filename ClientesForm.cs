using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Akira_Store
{
    public partial class FormClientes : Form
    {
        private string connectionString = "Server=DESKTOP-9U57LTA;Database=TiendaVinilos;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";
        private SqlConnection conexion;
        private DataTable dtClientes = new DataTable();
        public FormClientes()
        {
            InitializeComponent();
            conexion = new SqlConnection(connectionString);
            dgvClientes.CellClick += dgvClientes_CellClick;
            dgvClientes.CellEndEdit += dgvClientes_CellEndEdit;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
        }

        private void FormClientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ConfigurarDGV();
        }
        private void CargarClientes()
        {
            try
            {
                using (var da = new SqlDataAdapter("SELECT IdCliente, Nombre, Correo, Telefono FROM Clientes", connectionString))
                {
                    dtClientes = new DataTable();
                    da.Fill(dtClientes);
                    dgvClientes.DataSource = dtClientes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ConfigurarDGV()
        {
            dgvClientes.ReadOnly = false;
            dgvClientes.Columns["IdCliente"].ReadOnly = true;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        // Validaciones básicas
        private bool ValidarCampos(out string mensaje)
        {
            mensaje = "";
            return ValidarCampos(txtNombre.Text, txtCorreo.Text, txtTelefono.Text, out mensaje);
        }
        private bool ValidarCampos(string nombre, string correo, string telefono, out string mensaje)
        {
            mensaje = "";

            if (string.IsNullOrWhiteSpace(nombre))
            {
                mensaje = "El nombre es obligatorio.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(correo))
            {
                mensaje = "El correo es obligatorio.";
                return false;
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(correo.Trim()))
            {
                mensaje = "El correo no tiene un formato válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(telefono))
            {
                mensaje = "El teléfono es obligatorio.";
                return false;
            }

            var telRegex = new Regex(@"^[\d+\s-]{7,20}$");
            if (!telRegex.IsMatch(telefono.Trim()))
            {
                mensaje = "El teléfono debe contener solo dígitos y símbolos básicos (+, -, espacio).";
                return false;
            }

            return true;
        }
        // ============================
        //   MÉTODO GENERAL EJECUTAR COMANDO
        // ============================
        private bool EjecutarComando(SqlCommand cmd, out string mensaje)
        {
            mensaje = "";
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(out string msg))
            {
                MessageBox.Show(msg, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string query = "INSERT INTO Clientes (Nombre, Correo, Telefono) VALUES (@Nombre, @Correo, @Telefono)";
            using (var cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text.Trim());
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());

                if (EjecutarComando(cmd, out string error))
                {
                    MessageBox.Show("Cliente guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarClientes();
                }
                else
                {
                    MessageBox.Show("Error al guardar: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Selecciona un cliente del listado para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarCampos(out string msg))
            {
                MessageBox.Show(msg, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string query = "UPDATE Clientes SET Nombre=@Nombre, Correo=@Correo, Telefono=@Telefono WHERE IdCliente=@IdCliente";
            using (var cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text.Trim());
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
                cmd.Parameters.AddWithValue("@IdCliente", int.Parse(txtID.Text));

                if (EjecutarComando(cmd, out string error))
                {
                    MessageBox.Show("Cliente actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarClientes();
                }
                else
                {
                    MessageBox.Show("Error al actualizar: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Selecciona un cliente del listado para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Seguro que deseas eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            const string query = "DELETE FROM Clientes WHERE IdCliente=@IdCliente";
            using (var cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@IdCliente", int.Parse(txtID.Text));

                if (EjecutarComando(cmd, out string error))
                {
                    MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarClientes();
                }
                else
                {
                    MessageBox.Show("Error al eliminar: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            dgvClientes.ClearSelection();
            dgvClientes.Rows[e.RowIndex].Selected = true;

            var row = dgvClientes.Rows[e.RowIndex];
            txtID.Text = row.Cells["IdCliente"].Value?.ToString();
            txtNombre.Text = row.Cells["Nombre"].Value?.ToString();
            txtCorreo.Text = row.Cells["Correo"].Value?.ToString();
            txtTelefono.Text = row.Cells["Telefono"].Value?.ToString();
        }
        private void dgvClientes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgvClientes.Rows[e.RowIndex];
            int idCliente = Convert.ToInt32(row.Cells["IdCliente"].Value);
            string nombre = row.Cells["Nombre"].Value?.ToString();
            string correo = row.Cells["Correo"].Value?.ToString();
            string telefono = row.Cells["Telefono"].Value?.ToString();

            if (!ValidarCampos(nombre, correo, telefono, out string msg))
            {
                MessageBox.Show(msg, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CargarClientes(); // revertir cambios
                return;
            }

            const string query = "UPDATE Clientes SET Nombre=@Nombre, Correo=@Correo, Telefono=@Telefono WHERE IdCliente=@IdCliente";
            using (var cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@Nombre", nombre.Trim());
                cmd.Parameters.AddWithValue("@Correo", correo.Trim());
                cmd.Parameters.AddWithValue("@Telefono", telefono.Trim());
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);

                if (!EjecutarComando(cmd, out string error))
                {
                    MessageBox.Show("Error al actualizar: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CargarClientes();
                }
            }
        }
        // Utilidad: limpiar campos
        private void LimpiarCampos()
        {
            txtID.Text = "";
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtNombre.Focus();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            dgvClientes.ClearSelection();
            dgvClientes.Rows[e.RowIndex].Selected = true;

            var row = dgvClientes.Rows[e.RowIndex];
            txtID.Text = row.Cells["IdCliente"].Value?.ToString();
            txtNombre.Text = row.Cells["Nombre"].Value?.ToString();
            txtCorreo.Text = row.Cells["Correo"].Value?.ToString();
            txtTelefono.Text = row.Cells["Telefono"].Value?.ToString();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filtro))
            {
                dgvClientes.DataSource = dtClientes;
            }
            else
            {
                DataView dv = dtClientes.DefaultView;
                dv.RowFilter = $"Nombre LIKE '%{filtro}%' OR Correo LIKE '%{filtro}%' OR Telefono LIKE '%{filtro}%'";
                dgvClientes.DataSource = dv.ToTable();
            }
        }
    }
}
   
