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
    public partial class FormPermisoABM : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_PERMISO bllPermiso;

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
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Código", DataPropertyName = "Codigo", Width = 100, Tag = "CODIGO" });
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 200, Tag = "NOMBRE" });
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Descripción", DataPropertyName = "Descripcion", Width = 300, Tag = "DESCRIPCION" });
            dgvPermisos.AutoGenerateColumns = false;
            DataGridViewSortHelper.HabilitarOrdenamiento(dgvPermisos);
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("PERMISOS");
            if (btnNuevo != null) btnNuevo.Text = idiomaManager.Obtener("NUEVO");
            if (btnEditar != null) btnEditar.Text = idiomaManager.Obtener("EDITAR");
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");

            if (dgvPermisos.Columns.Count >= 3)
            {
                dgvPermisos.Columns[0].HeaderText = idiomaManager.Obtener("CODIGO");
                dgvPermisos.Columns[1].HeaderText = idiomaManager.Obtener("NOMBRE");
                dgvPermisos.Columns[2].HeaderText = idiomaManager.Obtener("DESCRIPCION");
            }
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            NuevoPermiso();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            EditarPermiso();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarPermiso();
        }

        private void CargarPermisos()
        {
            try
            {
                var permisos = bllPermiso.ObtenerTodosLosPermisos();
                DataGridViewSortHelper.Bind(dgvPermisos, permisos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar permisos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NuevoPermiso()
        {
            using (Form dialog = new Form())
            {
                dialog.Text = idiomaManager.Obtener("NUEVO") + " - " + idiomaManager.Obtener("PERMISO");
                dialog.Width = 450;
                dialog.Height = 280;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblCodigo = new Label() { Left = 20, Top = 20, Text = idiomaManager.Obtener("CODIGO") + ":", Width = 400 };
                TextBox txtCodigo = new TextBox() { Left = 20, Top = 45, Width = 400 };

                Label lblNombre = new Label() { Left = 20, Top = 75, Text = idiomaManager.Obtener("NOMBRE") + ":", Width = 400 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 100, Width = 400 };

                Label lblDesc = new Label() { Left = 20, Top = 130, Text = idiomaManager.Obtener("DESCRIPCION") + ":", Width = 400 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 155, Width = 400, Height = 50, Multiline = true };

                Button btnOK = new Button() { Text = "OK", Left = 250, Width = 80, Top = 220, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = idiomaManager.Obtener("CANCELAR"), Left = 340, Width = 80, Top = 220, DialogResult = DialogResult.Cancel };

                dialog.Controls.Add(lblCodigo);
                dialog.Controls.Add(txtCodigo);
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
                    if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
                    {
                        MessageBox.Show(idiomaManager.Obtener("CAMPOS_REQUERIDOS"), "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        BE_Permiso nuevoPermiso = new BE_Permiso(txtCodigo.Text, txtNombre.Text, txtDesc.Text);
                        bllPermiso.CrearPermiso(nuevoPermiso);
                        CargarPermisos();
                        MessageBox.Show(idiomaManager.Obtener("OPERACION_EXITOSA"), "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EditarPermiso()
        {
            if (dgvPermisos.SelectedRows.Count == 0)
            {
                MessageBox.Show(idiomaManager.Obtener("SELECCIONA_PERMISO"), "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var permisoSeleccionado = dgvPermisos.SelectedRows[0].DataBoundItem as BE_Permiso;
            if (permisoSeleccionado == null) return;

            using (Form dialog = new Form())
            {
                dialog.Text = idiomaManager.Obtener("EDITAR") + " - " + idiomaManager.Obtener("PERMISO");
                dialog.Width = 450;
                dialog.Height = 280;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblCodigo = new Label() { Left = 20, Top = 20, Text = idiomaManager.Obtener("CODIGO") + ":", Width = 400 };
                TextBox txtCodigo = new TextBox() { Left = 20, Top = 45, Width = 400, Text = permisoSeleccionado.Codigo };

                Label lblNombre = new Label() { Left = 20, Top = 75, Text = idiomaManager.Obtener("NOMBRE") + ":", Width = 400 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 100, Width = 400, Text = permisoSeleccionado.Nombre };

                Label lblDesc = new Label() { Left = 20, Top = 130, Text = idiomaManager.Obtener("DESCRIPCION") + ":", Width = 400 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 155, Width = 400, Height = 50, Multiline = true, Text = permisoSeleccionado.Descripcion ?? "" };

                Button btnOK = new Button() { Text = "OK", Left = 250, Width = 80, Top = 220, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = idiomaManager.Obtener("CANCELAR"), Left = 340, Width = 80, Top = 220, DialogResult = DialogResult.Cancel };

                dialog.Controls.Add(lblCodigo);
                dialog.Controls.Add(txtCodigo);
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
                    if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
                    {
                        MessageBox.Show(idiomaManager.Obtener("CAMPOS_REQUERIDOS"), "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        permisoSeleccionado.Codigo = txtCodigo.Text;
                        permisoSeleccionado.Nombre = txtNombre.Text;
                        permisoSeleccionado.Descripcion = txtDesc.Text;

                        bllPermiso.ActualizarPermiso(permisoSeleccionado);
                        CargarPermisos();
                        MessageBox.Show(idiomaManager.Obtener("OPERACION_EXITOSA"), "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EliminarPermiso()
        {
            if (dgvPermisos.SelectedRows.Count == 0)
            {
                MessageBox.Show(idiomaManager.Obtener("SELECCIONA_PERMISO"), "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var permisoSeleccionado = dgvPermisos.SelectedRows[0].DataBoundItem as BE_Permiso;
            if (permisoSeleccionado == null) return;

            if (MessageBox.Show($"¿Eliminar el permiso '{permisoSeleccionado.Nombre}'?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                bllPermiso.EliminarPermiso(permisoSeleccionado.ID);
                CargarPermisos();
                MessageBox.Show(idiomaManager.Obtener("OPERACION_EXITOSA"), "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
