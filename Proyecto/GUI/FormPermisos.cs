using BE;
using SERVICE;
using BLL;
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
    public partial class FormPermisos : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_ROL bllRol;
        private List<BE_Rol> rolesRaiz;

        public FormPermisos()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();
            bllRol = new BLL_ROL();

            CrearColumnasDataGrid();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarRoles();
        }

        private void CrearColumnasDataGrid()
        {
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Permiso", DataPropertyName = "Nombre", Width = 200, Tag = "NOMBRE" });
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Código", DataPropertyName = "Codigo", Width = 150 });
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Descripción", DataPropertyName = "Descripcion", Width = 300, Tag = "DESCRIPCION" });
            dgvPermisos.AutoGenerateColumns = false;
            DataGridViewSortHelper.HabilitarOrdenamiento(dgvPermisos);
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("PERMISOS");
            if (lblPerfiles != null) lblPerfiles.Text = idiomaManager.Obtener("ROLES");
            if (lblDetalles != null) lblDetalles.Text = idiomaManager.Obtener("DETALLES_ROL");
            if (lblNombrePerfil != null) lblNombrePerfil.Text = idiomaManager.Obtener("NOMBRE");
            if (lblDescripcionPerfil != null) lblDescripcionPerfil.Text = idiomaManager.Obtener("DESCRIPCION");
            if (lblPermisosLabel != null) lblPermisosLabel.Text = idiomaManager.Obtener("PERMISOS_PERFIL");
            if (lblPermisosDisponibles != null) lblPermisosDisponibles.Text = idiomaManager.Obtener("AGREGAR_PERMISO");

            if (dgvPermisos.Columns.Count >= 3)
            {
                dgvPermisos.Columns[0].HeaderText = idiomaManager.Obtener("NOMBRE");
                dgvPermisos.Columns[2].HeaderText = idiomaManager.Obtener("DESCRIPCION");
            }
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void btnAgregarPermiso_Click(object sender, EventArgs e)
        {
            AgregarPermiso();
        }

        private void btnEliminarPermiso_Click(object sender, EventArgs e)
        {
            QuitarPermiso();
        }

        private void tvPerfiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MostrarDetallesRol();
        }

        private void CargarRoles()
        {
            try
            {
                rolesRaiz = bllRol.ObtenerRolesPrincipales();
                tvPerfiles.Nodes.Clear();

                foreach (var rol in rolesRaiz)
                {
                    var nodo = CrearNodoRol(rol);
                    tvPerfiles.Nodes.Add(nodo);
                }

                CargarPermisosDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TreeNode CrearNodoRol(BE_Rol rol)
        {
            var nodo = new TreeNode(rol.Nombre) { Tag = rol };

            var hijos = bllRol.ObtenerRolesHijos(rol.ID);
            foreach (var hijo in hijos)
            {
                nodo.Nodes.Add(CrearNodoRol(hijo));
            }

            return nodo;
        }

        private void MostrarDetallesRol()
        {
            if (tvPerfiles.SelectedNode == null) return;

            var rol = tvPerfiles.SelectedNode.Tag as BE_Rol;
            if (rol == null) return;

            txtNombrePerfil.Text = rol.Nombre;
            txtDescripcionPerfil.Text = rol.Descripcion ?? "";

            CargarPermisosDelRol(rol.ID);
        }

        private void CargarPermisosDelRol(int idRol)
        {
            try
            {
                var permisos = bllRol.ObtenerPermisosConHerencia(idRol);
                DataGridViewSortHelper.Bind(dgvPermisos, permisos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar permisos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPermisosDisponibles()
        {
            try
            {
                var permisos = bllRol.ObtenerTodosLosPermisos();
                cbPermisosDisponibles.DataSource = permisos;
                cbPermisosDisponibles.DisplayMember = "Nombre";
                cbPermisosDisponibles.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar permisos disponibles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AgregarPermiso()
        {
            if (tvPerfiles.SelectedNode == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbPermisosDisponibles.SelectedItem == null) return;

            var rol = tvPerfiles.SelectedNode.Tag as BE_Rol;
            var permiso = cbPermisosDisponibles.SelectedItem as BE_Permiso;

            if (rol == null || permiso == null) return;

            try
            {
                bllRol.AgregarPermisoAlRol(rol.ID, permiso.ID);
                CargarPermisosDelRol(rol.ID);
                MessageBox.Show("Permiso agregado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar permiso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void QuitarPermiso()
        {
            if (dgvPermisos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un permiso para quitar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tvPerfiles.SelectedNode == null) return;

            var rol = tvPerfiles.SelectedNode.Tag as BE_Rol;
            var permisoSeleccionado = dgvPermisos.SelectedRows[0].DataBoundItem as BE_Permiso;

            if (rol == null || permisoSeleccionado == null) return;

            try
            {
                bllRol.QuitarPermisoDelRol(rol.ID, permisoSeleccionado.ID);
                CargarPermisosDelRol(rol.ID);
                MessageBox.Show("Permiso removido", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al quitar permiso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
