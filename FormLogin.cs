using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Akira_Store
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();

            txtContraseña.KeyDown += TxtContraseña_KeyDown;
            btnLogin.Click += btnLogin_Click;
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ValidarLogin();
        }


        private void ValidarLogin()
        {
            // Validación simple (puedes reemplazar con DB)
            if (txtUsuario.Text == "admin" && txtContraseña.Text == "1234")
            {
                FormPrincipal frm = new FormPrincipal();
                frm.Show();
                this.Hide();
               
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void TxtContraseña_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Evita el sonido "ding" al presionar Enter
                e.SuppressKeyPress = true;

                // Validar usuario y contraseña (ejemplo simple)
                if (txtUsuario.Text == "admin" && txtContraseña.Text == "1234")
                {
                    // Abrir otro formulario
                    FormPrincipal frm = new FormPrincipal();
                    frm.Show();

                    // Ocultar el login
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }


        }
