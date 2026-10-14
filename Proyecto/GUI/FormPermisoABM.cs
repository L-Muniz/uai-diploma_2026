using BE;
using SERVICE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormPermisoABM : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_PERMISO bllPermiso;
        private BE_Permiso permisoSeleccionado;

        public FormPermisoABM()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();
            bllPermiso = new BLL_PERMISO();

            CrearColumnasDataGrid();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarPermisos();
        }

        private void CrearColumnasDataGrid()
        {
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Tipo", DataPropertyName = "Tipo", Width = 80 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 150 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Código", DataPropertyName = "Codigo", Width = 100 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Descripción", DataPropertyName = "Descripcion", Width = 180 });
            DataGridViewSortHelper.HabilitarOrdenamiento(dgvComponentes);
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("PERMISOS");
            if (lblNombre != null) lblNombre.Text = idiomaManager.Obtener("NOMBRE");
            if (lblDescripcion != null) lblDescripcion.Text = idiomaManager.Obtener("DESCRIPCION");
            if (lblComponentes != null) lblComponentes.Text = "Componentes:";
            if (chkCompuesto != null) chkCompuesto.Text = "Es permiso compuesto";
            if (btnNuevoCompuesto != null) btnNuevoCompuesto.Text = "Crear Permiso\nCompuesto";
            if (btnAgregarComponente != null) btnAgregarComponente.Text = "Agregar";
            if (btnQuitarComponente != null) btnQuitarComponente.Text = "Quitar";
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void CargarPermisos()
        {
            try
            {
                tvPermisos.Nodes.Clear();
                var permisos = bllPermiso.ObtenerTodosLosPermisos();

                foreach (var permiso in permisos.OrderBy(p => p.Nombre))
                {
                    var nodo = new TreeNode(permiso.Nombre) { Tag = permiso };
                    tvPermisos.Nodes.Add(nodo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar permisos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tvPermisos_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LimpiarDetalle();

            if (e.Node?.Tag is BE_Permiso permiso)
            {
                permisoSeleccionado = permiso;
                txtNombre.Text = permiso.Nombre;
                txtDescripcion.Text = permiso.Descripcion ?? "";
                btnEliminar.Enabled = true;
                btnNuevoCompuesto.Enabled = true;

                CargarComponentes(permiso.ID);
            }
        }

        private void CargarComponentes(int idPermisoCompuesto)
        {
            try
            {
                var componentes = bllPermiso.ObtenerComponentes(idPermisoCompuesto);
                DataGridViewSortHelper.Bind(dgvComponentes, componentes);

                chkCompuesto.Checked = componentes.Count > 0;
                btnAgregarComponente.Enabled = true;
                btnQuitarComponente.Enabled = dgvComponentes.Rows.Count > 0;
            }
            catch
            {
                chkCompuesto.Checked = false;
                btnAgregarComponente.Enabled = false;
                btnQuitarComponente.Enabled = false;
            }
        }

        private void LimpiarDetalle()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            chkCompuesto.Checked = false;
            dgvComponentes.DataSource = null;
            btnEliminar.Enabled = false;
            btnNuevoCompuesto.Enabled = false;
            btnAgregarComponente.Enabled = false;
            btnQuitarComponente.Enabled = false;
            permisoSeleccionado = null;
        }

        private void btnNuevoCompuesto_Click(object sender, EventArgs e)
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Crear Permiso Compuesto";
                dialog.Width = 450;
                dialog.Height = 200;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 400 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 400 };

                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 400 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 400, Height = 40, Multiline = true };

                Button btnOK = new Button() { Text = "Crear", Left = 250, Width = 80, Top = 145, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 340, Width = 80, Top = 145, DialogResult = DialogResult.Cancel };

                dialog.Controls.AddRange(new Control[] { lblNombre, txtNombre, lblDesc, txtDesc, btnOK, btnCancel });
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtNombre.Text))
                    {
                        MessageBox.Show("El nombre es requerido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        BE_Permiso nuevoPermiso = new BE_Permiso(
                            txtNombre.Text.ToUpper(),
                            txtNombre.Text,
                            txtDesc.Text
                        );
                        bllPermiso.CrearPermiso(nuevoPermiso);
                        CargarPermisos();
                        MessageBox.Show("Permiso compuesto creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAgregarComponente_Click(object sender, EventArgs e)
        {
            if (permisoSeleccionado == null)
            {
                MessageBox.Show("Selecciona un permiso compuesto", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var permisosDisponibles = bllPermiso.ObtenerTodosLosPermisos()
                .Where(p => p.ID != permisoSeleccionado.ID)
                .ToList();

            using (Form dialog = new Form())
            {
                dialog.Text = "Agregar Componente";
                dialog.Width = 400;
                dialog.Height = 180;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblPermiso = new Label() { Left = 20, Top = 20, Text = "Selecciona permiso:", Width = 350 };
                ComboBox cbPermiso = new ComboBox() { Left = 20, Top = 45, Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };

                cbPermiso.DisplayMember = "Nombre";
                cbPermiso.ValueMember = "ID";
                cbPermiso.DataSource = permisosDisponibles;

                Button btnOK = new Button() { Text = "Agregar", Left = 200, Width = 80, Top = 100, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 290, Width = 80, Top = 100, DialogResult = DialogResult.Cancel };

                dialog.Controls.AddRange(new Control[] { lblPermiso, cbPermiso, btnOK, btnCancel });
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK && cbPermiso.SelectedItem is BE_Permiso permisoSeleccionado)
                {
                    try
                    {
                        bllPermiso.AgregarComponente(this.permisoSeleccionado.ID, permisoSeleccionado.ID);
                        CargarComponentes(this.permisoSeleccionado.ID);
                        MessageBox.Show("Componente agregado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnQuitarComponente_Click(object sender, EventArgs e)
        {
            if (permisoSeleccionado == null || dgvComponentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un componente para quitar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var componente = dgvComponentes.SelectedRows[0].DataBoundItem as BE_Permiso;
            if (componente == null) return;

            try
            {
                bllPermiso.QuitarComponente(permisoSeleccionado.ID, componente.ID);
                CargarComponentes(permisoSeleccionado.ID);
                MessageBox.Show("Componente removido", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (permisoSeleccionado == null)
            {
                MessageBox.Show("Selecciona un permiso", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"¿Eliminar el permiso '{permisoSeleccionado.Nombre}'?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                bllPermiso.EliminarPermiso(permisoSeleccionado.ID);
                CargarPermisos();
                LimpiarDetalle();
                MessageBox.Show("Permiso eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
