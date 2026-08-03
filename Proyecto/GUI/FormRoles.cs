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
    public partial class FormRoles : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_ROL bllRol;
        private List<BE_Rol> rolesRaiz;

        public FormRoles()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();
            bllRol = new BLL_ROL();

            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarRoles();
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("ROLES");
            if (lblRoles != null) lblRoles.Text = idiomaManager.Obtener("JERARQUIA_ROLES");
            if (lblDetalles != null) lblDetalles.Text = idiomaManager.Obtener("DETALLES_ROL");
            if (lblNombreRol != null) lblNombreRol.Text = idiomaManager.Obtener("NOMBRE");
            if (lblDescripcionRol != null) lblDescripcionRol.Text = idiomaManager.Obtener("DESCRIPCION");
            if (btnNuevo != null) btnNuevo.Text = idiomaManager.Obtener("NUEVO_ROL");
            if (btnEditar != null) btnEditar.Text = idiomaManager.Obtener("EDITAR");
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            NuevoRol();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            EditarRol();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarRol();
        }

        private void tvRoles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MostrarDetallesRol();
        }

        private void CargarRoles()
        {
            try
            {
                rolesRaiz = bllRol.ObtenerRolesPrincipales();
                tvRoles.Nodes.Clear();

                foreach (var rol in rolesRaiz)
                {
                    var nodo = CrearNodoRol(rol);
                    tvRoles.Nodes.Add(nodo);
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

        private void MostrarDetallesRol()
        {
            if (tvRoles.SelectedNode == null)
            {
                txtNombreRol.Text = "";
                txtDescripcionRol.Text = "";
                return;
            }

            var rol = tvRoles.SelectedNode.Tag as BE_Rol;
            if (rol == null) return;

            txtNombreRol.Text = rol.Nombre;
            txtDescripcionRol.Text = rol.Descripcion ?? "";
        }

        private void NuevoRol()
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

                var rolesDisponibles = new List<BE_Rol>(bllRol.ObtenerRolesPrincipales());
                cbPadre.DataSource = rolesDisponibles;
                cbPadre.DisplayMember = "Nombre";
                cbPadre.ValueMember = "ID";
                cbPadre.SelectedIndex = -1;

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

        private void EditarRol()
        {
            if (tvRoles.SelectedNode == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rol = tvRoles.SelectedNode.Tag as BE_Rol;
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

                var rolesDisponibles = new List<BE_Rol>(bllRol.ObtenerRolesPrincipales());
                cbPadre.DataSource = rolesDisponibles;
                cbPadre.DisplayMember = "Nombre";
                cbPadre.ValueMember = "ID";
                cbPadre.SelectedIndex = -1;

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

        private void EliminarRol()
        {
            if (tvRoles.SelectedNode == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rol = tvRoles.SelectedNode.Tag as BE_Rol;
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
