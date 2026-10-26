using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // ABM del catálogo de materiales (permiso PROC_ABM_MATERIALES).
    public partial class FormMateriales : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_CATALOGO bllCatalogo = new BLL_CATALOGO();
        private int idSeleccionado;

        public FormMateriales(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            dgvMateriales.SelectionChanged += (s, e) => CargarSeleccion();
            Refrescar();
            Nuevo();
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvMateriales, bllCatalogo.ListarMateriales());
        }

        private void CargarSeleccion()
        {
            if (dgvMateriales.CurrentRow == null) return;
            idSeleccionado = Convert.ToInt32(dgvMateriales.CurrentRow.Cells["ID"].Value);
            txtNombre.Text = dgvMateriales.CurrentRow.Cells["Nombre"].Value.ToString();
            txtCategoria.Text = dgvMateriales.CurrentRow.Cells["Categoria"].Value.ToString();
            txtUnidad.Text = dgvMateriales.CurrentRow.Cells["Unidad"].Value.ToString();
        }

        private void Nuevo()
        {
            idSeleccionado = 0;
            txtNombre.Clear();
            txtCategoria.Clear();
            txtUnidad.Text = "kg";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                bllCatalogo.GuardarMaterial(usuario, new BE_Material
                {
                    ID = idSeleccionado,
                    Nombre = txtNombre.Text.Trim(),
                    Categoria = txtCategoria.Text.Trim(),
                    UnidadMedida = txtUnidad.Text.Trim()
                });
                UIHelper.Informar("Material guardado.");
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
                    throw new ArgumentException("Seleccione un material.");
                if (!UIHelper.Confirmar("¿Eliminar el material seleccionado?"))
                    return;

                bllCatalogo.EliminarMaterial(usuario, idSeleccionado);
                UIHelper.Informar("Material eliminado.");
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
