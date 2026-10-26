using System;
using System.Globalization;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // Datos personales del usuario: fecha de nacimiento (edad mínima) y ubicación (punto verde más cercano).
    public partial class FormMiPerfil : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_DATOS_USUARIO bllDatos = new BLL_DATOS_USUARIO();

        public FormMiPerfil(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            CargarDatos();
        }

        private void CargarDatos()
        {
            BE_DatosUsuario datos = bllDatos.Obtener(usuario);
            dtpFechaNacimiento.Checked = datos.FechaNacimiento != null;
            if (datos.FechaNacimiento != null)
                dtpFechaNacimiento.Value = datos.FechaNacimiento.Value;
            txtLocalidad.Text = datos.Localidad ?? "";
            txtLatitud.Text = datos.Latitud?.ToString(CultureInfo.InvariantCulture) ?? "";
            txtLongitud.Text = datos.Longitud?.ToString(CultureInfo.InvariantCulture) ?? "";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                bllDatos.Guardar(usuario, new BE_DatosUsuario
                {
                    FechaNacimiento = dtpFechaNacimiento.Checked ? dtpFechaNacimiento.Value.Date : (DateTime?)null,
                    Localidad = txtLocalidad.Text.Trim(),
                    Latitud = string.IsNullOrWhiteSpace(txtLatitud.Text) ? (decimal?)null : UIHelper.LeerDecimal(txtLatitud.Text, "Latitud"),
                    Longitud = string.IsNullOrWhiteSpace(txtLongitud.Text) ? (decimal?)null : UIHelper.LeerDecimal(txtLongitud.Text, "Longitud")
                });
                UIHelper.Informar("Perfil guardado.");
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
