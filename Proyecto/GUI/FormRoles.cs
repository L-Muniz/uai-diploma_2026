using BE;
using SERVICE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace GUI
{
    public partial class FormRoles : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_ROL bllRol;
        private BE_Rol rolSeleccionado;

        public FormRoles()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();
            bllRol = new BLL_ROL();

            CrearColumnasDataGrid();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarRoles();
            this.Load += (s, e) => AplicarIdentidadVisual();
        }

        private void AplicarIdentidadVisual()
        {
            UITheme.DiferenciarModulo(
                panelBotones,
                panelIzquierdo,
                idiomaManager.Obtener("ROLES"),
                "Grupos de acceso asignables a usuarios",
                UITheme.AcentoRoles,
                UITheme.FondoRoles,
                btnNuevo, btnEditar, btnEliminar);
        }

        private void CrearColumnasDataGrid()
        {
            dgvComponentes.Columns.Clear();
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Tipo", HeaderText = "Tipo", DataPropertyName = "Tipo", Width = 80 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Nombre", HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 200 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Codigo", HeaderText = "Código", DataPropertyName = "Codigo", Width = 100 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Descripcion", HeaderText = "Descripción", DataPropertyName = "Descripcion", Width = 200 });
            dgvComponentes.AutoGenerateColumns = false;
            DataGridViewSortHelper.HabilitarOrdenamiento(dgvComponentes);
            dgvComponentes.DataBindingComplete -= DgvComponentes_DataBindingComplete;
            dgvComponentes.DataBindingComplete += DgvComponentes_DataBindingComplete;
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("ROLES");
            if (lblRoles != null) lblRoles.Text = idiomaManager.Obtener("ROLES");
            if (lblDetalles != null) lblDetalles.Text = "Detalles del rol";
            if (lblNombreRol != null) lblNombreRol.Text = idiomaManager.Obtener("NOMBRE");
            if (lblDescripcionRol != null) lblDescripcionRol.Text = idiomaManager.Obtener("DESCRIPCION");
            if (btnNuevo != null) btnNuevo.Text = idiomaManager.Obtener("NUEVO_ROL");
            if (btnEditar != null) btnEditar.Text = idiomaManager.Obtener("EDITAR");
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");
            if (lblComponentesDisponibles != null) lblComponentesDisponibles.Text = "Agregar permiso:";
            if (lblComponentes != null) lblComponentes.Text = "Permisos del rol";
            if (btnAgregarComponente != null) btnAgregarComponente.Text = "Agregar";
            if (btnDesasignarComponente != null) btnDesasignarComponente.Text = "Quitar";
            if (IsHandleCreated)
                AplicarIdentidadVisual();
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void btnNuevo_Click(object sender, EventArgs e) => NuevoRol();
        private void btnEditar_Click(object sender, EventArgs e) => EditarRol();
        private void btnEliminar_Click(object sender, EventArgs e) => EliminarRol();
        private void btnAgregarComponente_Click(object sender, EventArgs e) => AgregarComponente();
        private void btnDesasignarComponente_Click(object sender, EventArgs e) => DesasignarComponente();

        private void dgvRoles_SelectionChanged(object sender, EventArgs e)
        {
            MostrarDetallesRol();
        }

        private void CargarRoles()
        {
            try
            {
                var roles = bllRol.ObtenerTodosLosRoles().OrderBy(r => r.Nombre).ToList();
                DataGridViewSortHelper.Bind(dgvRoles, roles);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDetallesRol()
        {
            LimpiarDetalle();

            if (dgvRoles.SelectedRows.Count == 0)
                return;

            var rol = dgvRoles.SelectedRows[0].DataBoundItem as BE_Rol;
            if (rol == null) return;

            rolSeleccionado = rol;
            txtNombreRol.Text = rol.Nombre;
            txtDescripcionRol.Text = rol.Descripcion ?? "";
            CargarComponentes(rol.ID);
        }

        private void CargarComponentes(int idRol)
        {
            try
            {
                DataGridViewSortHelper.Bind(dgvComponentes, bllRol.ObtenerComponentesDelRol(idRol));
                cbComponentesDisponibles.DataSource = null;
                cbComponentesDisponibles.DisplayMember = "Nombre";
                cbComponentesDisponibles.ValueMember = "ID";
                cbComponentesDisponibles.DataSource = bllRol.ObtenerComponentesDisponiblesParaAsignar(idRol);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar componentes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvComponentes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvComponentes.Rows)
            {
                var componente = row.DataBoundItem as BE_ComponenteAcceso;
                if (componente?.EsHeredado == true)
                    row.DefaultCellStyle.ForeColor = Color.DimGray;
            }
        }

        private void AgregarComponente()
        {
            if (rolSeleccionado == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbComponentesDisponibles.SelectedItem == null) return;

            var componente = cbComponentesDisponibles.SelectedItem as BE_ComponenteAcceso;

            try
            {
                bllRol.AsignarComponente(rolSeleccionado.ID, componente);
                CargarRoles();
                MostrarDetallesRol();
                MessageBox.Show("Permiso agregado al rol", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DesasignarComponente()
        {
            if (rolSeleccionado == null || dgvComponentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un permiso para quitar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var componente = dgvComponentes.SelectedRows[0].DataBoundItem as BE_ComponenteAcceso;
            if (componente == null) return;

            try
            {
                bllRol.DesasignarComponente(rolSeleccionado.ID, componente);
                CargarRoles();
                MostrarDetallesRol();
                MessageBox.Show("Permiso removido del rol", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NuevoRol()
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Nuevo Rol";
                dialog.Width = 400;
                dialog.Height = 200;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 350 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 350 };
                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 350 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 350, Height = 50, Multiline = true };

                Button btnOK = new Button() { Text = "Crear", Left = 200, Width = 80, Top = 160, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 290, Width = 80, Top = 160, DialogResult = DialogResult.Cancel };
                dialog.Controls.AddRange(new Control[] { lblNombre, txtNombre, lblDesc, txtDesc, btnOK, btnCancel });
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(txtNombre.Text))
                {
                    try
                    {
                        BE_Rol nuevoRol = new BE_Rol(txtNombre.Text, txtDesc.Text);
                        bllRol.CrearRol(nuevoRol);
                        CargarRoles();
                        MessageBox.Show("Rol creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EditarRol()
        {
            if (rolSeleccionado == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Form dialog = new Form())
            {
                dialog.Text = "Editar Rol";
                dialog.Width = 400;
                dialog.Height = 200;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 350 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 350, Text = rolSeleccionado.Nombre };
                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 350 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 350, Height = 50, Multiline = true, Text = rolSeleccionado.Descripcion ?? "" };

                Button btnOK = new Button() { Text = "Guardar", Left = 200, Width = 80, Top = 160, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 290, Width = 80, Top = 160, DialogResult = DialogResult.Cancel };
                dialog.Controls.AddRange(new Control[] { lblNombre, txtNombre, lblDesc, txtDesc, btnOK, btnCancel });
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        rolSeleccionado.Nombre = txtNombre.Text;
                        rolSeleccionado.Descripcion = txtDesc.Text;
                        bllRol.ActualizarRol(rolSeleccionado);
                        CargarRoles();
                        MostrarDetallesRol();
                        MessageBox.Show("Rol actualizado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EliminarRol()
        {
            if (rolSeleccionado == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"¿Eliminar el rol '{rolSeleccionado.Nombre}'?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                bllRol.EliminarRol(rolSeleccionado.ID);
                CargarRoles();
                LimpiarDetalle();
                MessageBox.Show("Rol eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarDetalle()
        {
            txtNombreRol.Text = "";
            txtDescripcionRol.Text = "";
            dgvComponentes.DataSource = null;
            cbComponentesDisponibles.DataSource = null;
            rolSeleccionado = null;
        }
    }
}
