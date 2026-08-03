using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using SERVICE;

namespace GUI
{
    public partial class Register : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;

        public Register()
        {
            InitializeComponent();
            idiomaManager = SV_IdiomaManager.GetInstance();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            UITheme.AplicarLogin(this);
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("REGISTRARSE_TITULO");
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUserR.Text?.Trim() ?? "";
                string password = txtContraR.Text ?? "";

                if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Complete el nombre de usuario y la contraseña.",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("El nombre de usuario es obligatorio.",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUserR.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("La contraseña es obligatoria.",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContraR.Focus();
                    return;
                }

                if (password.Length <= 4)
                {
                    MessageBox.Show("La contraseña debe tener más de 4 caracteres.",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContraR.Focus();
                    return;
                }

                BLL_USUARIO bll_usu = new BLL_USUARIO();
                bll_usu.CrearUsuario(username, password);
                MessageBox.Show("Usuario creado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema al crear el usuario.\n\nDetalle: " + ex.Message,
                    "Error del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
