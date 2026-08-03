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
    public partial class FormPerfiles : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_PERFIL bllPerfil;
        private BLL_ROL bllRol;
        private List<BE_Perfil> perfilesRaiz;
        private List<BE_Rol> rolesRaiz;
        private ComboBox cbTipo;
        private bool mostrandoRoles = false;

        public FormPerfiles()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();
            bllPerfil = new BLL_PERFIL();
            bllRol = new BLL_ROL();

            CrearComboTipo();
            CrearColumnasDataGrid();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarPerfiles();
        }

        private void CrearComboTipo()
        {
            cbTipo = new ComboBox();
            cbTipo.Location = new Point(10, 10);
            cbTipo.Width = 150;
            cbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTipo.Items.Add("Perfiles");
            cbTipo.Items.Add("Roles");
            cbTipo.SelectedIndex = 0;
            cbTipo.SelectedIndexChanged += (s, e) => CambiarTipo();
            this.Controls.Add(cbTipo);
        }

        private void CambiarTipo()
        {
            mostrandoRoles = cbTipo.SelectedIndex == 1;

            if (btnAgregarPermiso != null) btnAgregarPermiso.Visible = !mostrandoRoles;
            if (btnEliminarPermiso != null) btnEliminarPermiso.Visible = !mostrandoRoles;
            if (lblPermisosLabel != null) lblPermisosLabel.Visible = !mostrandoRoles;
            if (lblPermisosDisponibles != null) lblPermisosDisponibles.Visible = !mostrandoRoles;
            if (dgvPermisos != null) dgvPermisos.Visible = !mostrandoRoles;
            if (cbPermisosDisponibles != null) cbPermisosDisponibles.Visible = !mostrandoRoles;

            ActualizarTextos();

            if (mostrandoRoles)
                CargarRoles();
            else
                CargarPerfiles();
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
            this.Text = idiomaManager.Obtener("PERFILES");

            if (btnNuevo != null) btnNuevo.Text = idiomaManager.Obtener("NUEVO_PERFIL");
            if (btnEditar != null) btnEditar.Text = idiomaManager.Obtener("EDITAR");
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");
            if (btnAgregarPermiso != null) btnAgregarPermiso.Text = idiomaManager.Obtener("AGREGAR_PERMISO");
            if (btnEliminarPermiso != null) btnEliminarPermiso.Text = idiomaManager.Obtener("QUITAR_PERMISO");
            if (lblPerfiles != null) lblPerfiles.Text = idiomaManager.Obtener("JERARQUIA_PERFILES");
            if (lblDetalles != null) lblDetalles.Text = idiomaManager.Obtener("DETALLES_PERFIL");
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

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (mostrandoRoles)
                NuevoRolDialog();
            else
                NuevoPerfil();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (mostrandoRoles)
                EditarRolDialog();
            else
                EditarPerfil();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (mostrandoRoles)
                EliminarRolDialog();
            else
                EliminarPerfil();
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
            MostrarDetallesPerfil();
        }

        private void CargarPerfiles()
        {
            try
            {
                tvPerfiles.Nodes.Clear();

                // Cargar Perfiles
                perfilesRaiz = bllPerfil.ObtenerPerfilesPrincipales();
                foreach (var perfil in perfilesRaiz)
                {
                    var nodo = CrearNodoPerfil(perfil);
                    tvPerfiles.Nodes.Add(nodo);
                }

                // Cargar Roles
                rolesRaiz = bllRol.ObtenerRolesPrincipales();
                if (rolesRaiz != null && rolesRaiz.Count > 0)
                {
                    TreeNode nodoRoles = new TreeNode("Roles");
                    foreach (var rol in rolesRaiz)
                    {
                        var nodo = CrearNodoRol(rol);
                        nodoRoles.Nodes.Add(nodo);
                    }
                    tvPerfiles.Nodes.Add(nodoRoles);
                }

                CargarPermisosDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar perfiles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TreeNode CrearNodoPerfil(BE_Perfil perfil)
        {
            var nodo = new TreeNode(perfil.Nombre) { Tag = perfil };

            var hijos = bllPerfil.ObtenerPerfilesHijos(perfil.ID);
            foreach (var hijo in hijos)
            {
                nodo.Nodes.Add(CrearNodoPerfil(hijo));
            }

            return nodo;
        }

        private void MostrarDetallesPerfil()
        {
            if (tvPerfiles.SelectedNode == null) return;

            if (mostrandoRoles)
            {
                var rol = tvPerfiles.SelectedNode.Tag as BE_Rol;
                if (rol == null) return;

                txtNombrePerfil.Text = rol.Nombre;
                txtDescripcionPerfil.Text = rol.Descripcion ?? "";
                dgvPermisos.DataSource = null;
            }
            else
            {
                var perfil = tvPerfiles.SelectedNode.Tag as BE_Perfil;
                if (perfil == null) return;

                txtNombrePerfil.Text = perfil.Nombre;
                txtDescripcionPerfil.Text = perfil.Descripcion ?? "";

                CargarPermisosDelPerfil(perfil.ID);
            }
        }

        private void CargarPermisosDelPerfil(int idPerfil)
        {
            try
            {
                var permisos = bllPerfil.ObtenerPermisosDelPerfil(idPerfil);
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
                var permisos = bllPerfil.ObtenerTodosLosPermisos();
                cbPermisosDisponibles.DataSource = permisos;
                cbPermisosDisponibles.DisplayMember = "Nombre";
                cbPermisosDisponibles.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar permisos disponibles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NuevoPerfil()
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Nuevo Perfil";
                dialog.Width = 400;
                dialog.Height = 250;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 350 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 350 };
                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 350 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 350, Height = 80, Multiline = true };
                Button btnOK = new Button() { Text = "OK", Left = 200, Width = 80, Top = 190, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 290, Width = 80, Top = 190, DialogResult = DialogResult.Cancel };

                dialog.Controls.Add(lblNombre);
                dialog.Controls.Add(txtNombre);
                dialog.Controls.Add(lblDesc);
                dialog.Controls.Add(txtDesc);
                dialog.Controls.Add(btnOK);
                dialog.Controls.Add(btnCancel);
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(txtNombre.Text))
                {
                    try
                    {
                        bllPerfil.CrearPerfil(txtNombre.Text, txtDesc.Text);
                        CargarPerfiles();
                        MessageBox.Show("Perfil creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al crear perfil: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EditarPerfil()
        {
            if (tvPerfiles.SelectedNode == null)
            {
                MessageBox.Show("Selecciona un perfil", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var perfil = tvPerfiles.SelectedNode.Tag as BE_Perfil;
            if (perfil == null) return;

            using (Form dialog = new Form())
            {
                dialog.Text = "Editar Perfil";
                dialog.Width = 400;
                dialog.Height = 250;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 350 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 350, Text = perfil.Nombre };
                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 350 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 350, Height = 80, Multiline = true, Text = perfil.Descripcion ?? "" };
                Button btnOK = new Button() { Text = "OK", Left = 200, Width = 80, Top = 190, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 290, Width = 80, Top = 190, DialogResult = DialogResult.Cancel };

                dialog.Controls.Add(lblNombre);
                dialog.Controls.Add(txtNombre);
                dialog.Controls.Add(lblDesc);
                dialog.Controls.Add(txtDesc);
                dialog.Controls.Add(btnOK);
                dialog.Controls.Add(btnCancel);
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        perfil.Nombre = txtNombre.Text;
                        perfil.Descripcion = txtDesc.Text;
                        bllPerfil.ActualizarPerfil(perfil);
                        CargarPerfiles();
                        MessageBox.Show("Perfil actualizado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al editar perfil: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EliminarPerfil()
        {
            if (tvPerfiles.SelectedNode == null)
            {
                MessageBox.Show("Selecciona un perfil", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var perfil = tvPerfiles.SelectedNode.Tag as BE_Perfil;
            if (perfil == null) return;

            if (MessageBox.Show($"¿Eliminar el perfil '{perfil.Nombre}'?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                bllPerfil.EliminarPerfil(perfil.ID);
                CargarPerfiles();
                MessageBox.Show("Perfil eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar perfil: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AgregarPermiso()
        {
            if (tvPerfiles.SelectedNode == null)
            {
                MessageBox.Show("Selecciona un perfil", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbPermisosDisponibles.SelectedItem == null) return;

            var perfil = tvPerfiles.SelectedNode.Tag as BE_Perfil;
            var permiso = cbPermisosDisponibles.SelectedItem as BE_Permiso;

            if (perfil == null || permiso == null) return;

            try
            {
                bllPerfil.AgregarPermisoAlPerfil(perfil.ID, permiso.ID);
                CargarPermisosDelPerfil(perfil.ID);
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

            var perfil = tvPerfiles.SelectedNode.Tag as BE_Perfil;
            var permisoSeleccionado = dgvPermisos.SelectedRows[0].DataBoundItem as BE_Permiso;

            if (perfil == null || permisoSeleccionado == null) return;

            try
            {
                bllPerfil.QuitarPermisoDelPerfil(perfil.ID, permisoSeleccionado.ID);
                CargarPermisosDelPerfil(perfil.ID);
                MessageBox.Show("Permiso removido", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al quitar permiso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string PromptPerfil(string titulo, string valorPorDefecto)
        {
            using (Form prompt = new Form())
            {
                prompt.Text = titulo;
                prompt.Width = 400;
                prompt.Height = 150;
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;

                Label lbl = new Label() { Left = 20, Top = 20, Text = "Nombre del perfil:", Width = 350 };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 350, Text = valorPorDefecto };
                Button okBtn = new Button() { Text = "OK", Left = 200, Width = 80, Top = 80, DialogResult = DialogResult.OK };
                Button cancelBtn = new Button() { Text = "Cancelar", Left = 290, Width = 80, Top = 80, DialogResult = DialogResult.Cancel };

                prompt.Controls.Add(lbl);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(okBtn);
                prompt.Controls.Add(cancelBtn);
                prompt.AcceptButton = okBtn;
                prompt.CancelButton = cancelBtn;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
            }
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
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

        private void NuevoRolDialog()
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Nuevo Rol";
                dialog.Width = 450;
                dialog.Height = 350;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 400 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 400 };

                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 400 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 400, Height = 80, Multiline = true };

                Label lblPadre = new Label() { Left = 20, Top = 190, Text = "Rol Padre (opcional):", Width = 400 };
                ComboBox cbPadre = new ComboBox() { Left = 20, Top = 215, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };

                var rolesDisponibles = bllRol.ObtenerRolesPrincipales();
                cbPadre.DataSource = rolesDisponibles;
                cbPadre.DisplayMember = "Nombre";
                cbPadre.ValueMember = "ID";

                Button btnOK = new Button() { Text = "OK", Left = 250, Width = 80, Top = 260, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 340, Width = 80, Top = 260, DialogResult = DialogResult.Cancel };

                dialog.Controls.Add(lblNombre);
                dialog.Controls.Add(txtNombre);
                dialog.Controls.Add(lblDesc);
                dialog.Controls.Add(txtDesc);
                dialog.Controls.Add(lblPadre);
                dialog.Controls.Add(cbPadre);
                dialog.Controls.Add(btnOK);
                dialog.Controls.Add(btnCancel);
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(txtNombre.Text))
                {
                    try
                    {
                        BE_Rol nuevoRol = new BE_Rol(txtNombre.Text, txtDesc.Text);
                        if (cbPadre.SelectedValue is int id)
                            nuevoRol.IdPadreRol = id;

                        bllRol.CrearRol(nuevoRol);
                        CargarRoles();
                        MessageBox.Show("Rol creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al crear rol: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EditarRolDialog()
        {
            if (tvPerfiles.SelectedNode == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rol = tvPerfiles.SelectedNode.Tag as BE_Rol;
            if (rol == null) return;

            using (Form dialog = new Form())
            {
                dialog.Text = "Editar Rol";
                dialog.Width = 450;
                dialog.Height = 350;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 400 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 400, Text = rol.Nombre };

                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 400 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 400, Height = 80, Multiline = true, Text = rol.Descripcion ?? "" };

                Label lblPadre = new Label() { Left = 20, Top = 190, Text = "Rol Padre (opcional):", Width = 400 };
                ComboBox cbPadre = new ComboBox() { Left = 20, Top = 215, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };

                var rolesDisponibles = bllRol.ObtenerRolesPrincipales();
                cbPadre.DataSource = rolesDisponibles;
                cbPadre.DisplayMember = "Nombre";
                cbPadre.ValueMember = "ID";

                if (rol.IdPadreRol.HasValue)
                    cbPadre.SelectedValue = rol.IdPadreRol.Value;

                Button btnOK = new Button() { Text = "OK", Left = 250, Width = 80, Top = 260, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 340, Width = 80, Top = 260, DialogResult = DialogResult.Cancel };

                dialog.Controls.Add(lblNombre);
                dialog.Controls.Add(txtNombre);
                dialog.Controls.Add(lblDesc);
                dialog.Controls.Add(txtDesc);
                dialog.Controls.Add(lblPadre);
                dialog.Controls.Add(cbPadre);
                dialog.Controls.Add(btnOK);
                dialog.Controls.Add(btnCancel);
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        rol.Nombre = txtNombre.Text;
                        rol.Descripcion = txtDesc.Text;
                        rol.IdPadreRol = cbPadre.SelectedValue is int id ? (int?)id : null;

                        bllRol.ActualizarRol(rol);
                        CargarRoles();
                        MessageBox.Show("Rol actualizado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al editar rol: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EliminarRolDialog()
        {
            if (tvPerfiles.SelectedNode == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rol = tvPerfiles.SelectedNode.Tag as BE_Rol;
            if (rol == null) return;

            if (MessageBox.Show($"¿Eliminar el rol '{rol.Nombre}'?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                bllRol.EliminarRol(rol.ID);
                CargarRoles();
                MessageBox.Show("Rol eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar rol: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
