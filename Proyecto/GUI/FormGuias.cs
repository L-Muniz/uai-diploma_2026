using System;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // ABM de guías de reutilización (permiso PROC_ABM_GUIAS).
    public partial class FormGuias : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_CATALOGO bllCatalogo = new BLL_CATALOGO();
        private readonly BLL_GESTION bllGestion = new BLL_GESTION();
        private int idSeleccionado;

        public FormGuias(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            UIHelper.CargarCombo(cboMaterial, bllGestion.ListarMateriales(), "Nombre", "ID");
            dgvGuias.SelectionChanged += (s, e) => CargarSeleccion();

            // Sin permiso de ABM (ciudadano u operador) la guía se consulta pero no se modifica: se ocultan los botones.
            bool puedeEditar = BLL_AUTORIZACION.TienePermiso(usuario, "PROC_ABM_GUIAS");
            btnNuevo.Visible = puedeEditar;
            btnGuardar.Visible = puedeEditar;
            btnEliminar.Visible = puedeEditar;
            cboMaterial.Enabled = puedeEditar;
            cboDificultad.Enabled = puedeEditar;
            txtTitulo.ReadOnly = !puedeEditar;
            txtPasos.ReadOnly = !puedeEditar;

            Refrescar();
            Nuevo();
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvGuias, bllCatalogo.ListarGuias());
            // Los pasos se leen en el cuadro de abajo: en la grilla quedan ilegibles.
            if (dgvGuias.Columns.Contains("Pasos"))
                dgvGuias.Columns["Pasos"].Visible = false;
        }

        private void CargarSeleccion()
        {
            if (dgvGuias.CurrentRow == null) return;
            var fila = dgvGuias.CurrentRow;
            idSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);
            cboMaterial.SelectedValue = Convert.ToInt32(fila.Cells["IDMaterial"].Value);
            txtTitulo.Text = fila.Cells["Titulo"].Value.ToString();
            cboDificultad.Text = fila.Cells["Dificultad"].Value.ToString();
            txtPasos.Text = fila.Cells["Pasos"].Value.ToString();
        }

        private void Nuevo()
        {
            idSeleccionado = 0;
            txtTitulo.Clear();
            txtPasos.Clear();
            cboDificultad.SelectedIndex = 0;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboMaterial.SelectedValue == null)
                    throw new ArgumentException("Seleccione el material.");

                bllCatalogo.GuardarGuia(usuario, new BE_GuiaReutilizacion
                {
                    ID = idSeleccionado,
                    Material = new BE_Material { ID = Convert.ToInt32(cboMaterial.SelectedValue) },
                    Titulo = txtTitulo.Text.Trim(),
                    Pasos = txtPasos.Text.Trim(),
                    Dificultad = cboDificultad.Text
                });
                UIHelper.Informar("Guía guardada.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSeleccionado == 0)
                    throw new ArgumentException("Seleccione una guía.");
                if (!UIHelper.Confirmar("¿Eliminar la guía seleccionada?"))
                    return;

                bllCatalogo.EliminarGuia(usuario, idSeleccionado);
                UIHelper.Informar("Guía eliminada.");
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
