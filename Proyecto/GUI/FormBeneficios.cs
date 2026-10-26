using System;
using System.Globalization;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // ABM de beneficios canjeables (permiso PROC_ABM_BENEFICIOS).
    public partial class FormBeneficios : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_CATALOGO bllCatalogo = new BLL_CATALOGO();
        private int idSeleccionado;
        private bool activoSeleccionado = true;

        public FormBeneficios(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            dgvBeneficios.SelectionChanged += (s, e) => CargarSeleccion();
            Refrescar();
            Nuevo();
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvBeneficios, bllCatalogo.ListarBeneficios());
        }

        private void CargarSeleccion()
        {
            if (dgvBeneficios.CurrentRow == null) return;
            var fila = dgvBeneficios.CurrentRow;
            idSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);
            cboTipo.Text = fila.Cells["Tipo"].Value.ToString();
            txtDescripcion.Text = fila.Cells["Descripcion"].Value.ToString();
            numPuntos.Value = Convert.ToDecimal(fila.Cells["PuntosRequeridos"].Value);
            txtCosto.Text = Convert.ToDecimal(fila.Cells["CostoEmpresa"].Value).ToString("0.00", CultureInfo.InvariantCulture);
            activoSeleccionado = fila.Cells["Activo"].Value.ToString() == "Sí";
            btnActivar.Text = activoSeleccionado ? "Desactivar" : "Activar";
        }

        private void Nuevo()
        {
            idSeleccionado = 0;
            cboTipo.SelectedIndex = 0;
            txtDescripcion.Clear();
            numPuntos.Value = 100;
            txtCosto.Text = "0";
            btnActivar.Text = "Activar / desactivar";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                bllCatalogo.GuardarBeneficio(usuario, new BE_Beneficio
                {
                    ID = idSeleccionado,
                    Tipo = cboTipo.Text,
                    Descripcion = txtDescripcion.Text.Trim(),
                    PuntosRequeridos = (int)numPuntos.Value,
                    CostoEmpresa = UIHelper.LeerDecimal(txtCosto.Text, "Costo")
                });
                UIHelper.Informar("Beneficio guardado.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSeleccionado == 0)
                    throw new ArgumentException("Seleccione un beneficio.");

                bllCatalogo.CambiarEstadoBeneficio(usuario, idSeleccionado, !activoSeleccionado);
                UIHelper.Informar(activoSeleccionado ? "Beneficio desactivado." : "Beneficio activado.");
                Refrescar();
                Nuevo();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
