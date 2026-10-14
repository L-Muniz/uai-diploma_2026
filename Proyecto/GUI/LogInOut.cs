using BE;
using BLL;
using SERVICE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class LogInOut : Form, ISV_IdiomaObserver, ISV_ProveedorTraduccion
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_Idioma bllIdioma;

        public LogInOut()
        {
            InitializeComponent();

            bllIdioma = new BLL_Idioma();
            var idiomasDisponibles = bllIdioma.ObtenerIdiomasDisponibles();
            System.Diagnostics.Debug.WriteLine($"Idiomas disponibles: {idiomasDisponibles.Count}");
            foreach (var idioma in idiomasDisponibles)
            {
                System.Diagnostics.Debug.WriteLine($"  - {idioma}");
            }

            if (idiomasDisponibles.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("Creando idioma ES por defecto...");
                bllIdioma.AgregarIdioma("ES", "Español");
                idiomasDisponibles = bllIdioma.ObtenerIdiomasDisponibles();
            }

            idiomaManager = SV_IdiomaManager.GetInstance();
            idiomaManager.EstablecerProveedorTraduccion(this);

            CargarIdiomasEnComboBox(idiomasDisponibles);

            if (idiomasDisponibles.Count > 0)
            {
                idiomaManager.CambiarIdioma("ES");
                cmbIdioma.SelectedIndex = 0;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ERROR: No se pudo crear idioma ES");
            }

            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            VerificarIntegridadAlInicio();
            UITheme.AplicarLogin(this);

            this.FormClosed += (s, e) => Application.Exit();
        }

        private void CargarIdiomasEnComboBox(List<string> idiomas)
        {
            cmbIdioma.Items.Clear();
            var nombresIdiomas = idiomaManager.ObtenerNombresIdiomas();
            foreach (var codigo in idiomas.OrderBy(i => i))
            {
                if (nombresIdiomas.TryGetValue(codigo, out var nombre))
                {
                    cmbIdioma.Items.Add(new KeyValuePair<string, string>(codigo, nombre));
                }
            }
            cmbIdioma.DisplayMember = "Value";
            cmbIdioma.ValueMember = "Key";
        }

        private void cmbIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIdioma.SelectedItem is KeyValuePair<string, string> seleccionado)
            {
                idiomaManager.CambiarIdioma(seleccionado.Key);
            }
        }

        public string ObtenerTraduccion(string clave, string codigoIdioma)
        {
            return bllIdioma.ObtenerTraduccion(clave, codigoIdioma);
        }

        public List<string> ObtenerIdiomasDisponibles()
        {
            return bllIdioma.ObtenerIdiomasDisponibles();
        }

        public Dictionary<string, string> ObtenerNombresIdiomas()
        {
            return bllIdioma.ObtenerIdiomas()
                .ToDictionary(i => i.Codigo, i => i.Nombre, StringComparer.OrdinalIgnoreCase);
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
            ActualizarComboBoxIdiomas();
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("LOGIN_TITULO");
            if (string.IsNullOrWhiteSpace(this.Text) || this.Text.StartsWith("["))
                this.Text = "Iniciar Sesión";

            grpLogin.Text = idiomaManager.Obtener("LOGIN_TITULO");
            label1.Text = idiomaManager.Obtener("LOGIN_USUARIO");
            label2.Text = idiomaManager.Obtener("LOGIN_CONTRASENA");
            btnIngresar.Text = idiomaManager.Obtener("LOGIN_INGRESAR");
            lblRegistrarse.Text = idiomaManager.Obtener("LOGIN_REGISTRARSE");
            lblIdioma.Text = idiomaManager.Obtener("IDIOMA");
        }

        private void ActualizarComboBoxIdiomas()
        {
            cmbIdioma.DisplayMember = "Value";
            cmbIdioma.ValueMember = "Key";
        }

        private void VerificarIntegridadAlInicio()
        {
            
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            BLL_USUARIO bll_usu = new BLL_USUARIO();
            string nombreUsuario = txtUser.Text;
            string password = txtContra.Text;

            try
            {
                BE_USUARIO usuario = bll_usu.Login(nombreUsuario, password);

                if (usuario != null)
                {
                    MessageBox.Show("Login correcto");

                    bool esAdmin = usuario.Rol?.ToUpper() == "ADMIN" || usuario.Rol?.ToUpper() == "ADMINISTRADOR";

                    if (Program.IntegridadFallo && esAdmin)
                    {
                        var resultado = MessageBox.Show(
                            "Se detectó un error de integridad en la BD.\n\n" +
                            Program.MensajeIntegridad + "\n\n" +
                            "Sí = Restaurar desde backup\n" +
                            "No = Abrir recálculo de dígitos verificadores\n" +
                            "Cancelar = Continuar sin restaurar",
                            "Restauración de integridad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (resultado == DialogResult.Yes)
                        {
                            using (var form = new FormBackups())
                            {
                                UITheme.Aplicar(form);
                                form.ShowDialog();
                            }
                        }
                        else if (resultado == DialogResult.No)
                        {
                            using (var form = new FormIntegridad())
                            {
                                UITheme.Aplicar(form);
                                form.ShowDialog();
                            }
                        }
                    }

                    new MainForm(usuario).Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            txtUser.Clear();
            txtContra.Clear();
            txtUser.Focus();
        }

        private void lblRegistrarse_Click(object sender, EventArgs e)
        {
            FormUnico.Abrir<Register>();
        }
    }
}
